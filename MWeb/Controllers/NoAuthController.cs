using GL.Common;
using GL.Data;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using GL.Protocol;
using log4net;
using MWeb.Models;
using MWeb.protobuf.SCmd;
using ProtoCmd.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Mail;

namespace MWeb.Controllers
{
    //localhost:24276/NoAuth/FreezeNo?dwUserID=1001&Reason=1
    public class NoAuthController : Controller
    {
        public static string PostUrl = ConfigurationManager.AppSettings["WebReference.Service.PostUrl"];
        public object FreezeNo([FromBody]OMModel model)
        {
            Service_Freeze_C ServiceFreezeC;

            ServiceFreezeC = Service_Freeze_C.CreateBuilder()
                   .SetDwUserID((uint)model.dwUserID)
                   .SetDwFreeze((uint)model.Reason)
                   .SetDwMinute((uint)5256000)
                   .Build();



            Bind tbind = protobuf.SCmd.Cmd.runClient(new Bind(ServiceCmd.SC_FREEZE_USER, ServiceFreezeC.ToByteArray()));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_FREEZE_USER:
                    Service_Freeze_S ServiceFreezeS = Service_Freeze_S.ParseFrom(tbind.body.ToBytes());

                    if (ServiceFreezeS.Suc)
                    {
                        return Content("0", "string");
                    }
                    else
                    {
                        RoleBLL.UpdateRoleNoFreeze(
                          model.Reason, DateTime.Now.AddMinutes(5256000),
                          model.dwUserID
                              );

                        return Content("0", "string");
                    }


                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Content("2", "string");
        }



        public ActionResult GetSerStatus()
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_QUERY_SERVERSTATUS, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_QUERY_SERVERSTATUS:
                    {
                        Service_Query_ServerStatus_S ServiceQueryServerStatusS = Service_Query_ServerStatus_S.ParseFrom(tbind.body.ToBytes());

                        return Json(new
                        {
                            result = !ServiceQueryServerStatusS.Close
                        }, JsonRequestBehavior.AllowGet);

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }


            return Json(new
            {
                result = false
            }, JsonRequestBehavior.AllowGet);

        }



        /// <summary>
        /// 生成验证码的链接
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");



        }




        [QueryValues]
        public ActionResult SendUserEmail(Dictionary<string, string> queryvalues)
        {
            ILog log = LogManager.GetLogger("Callback");
            log.Info("开始接受参数!");



            string UEUserID = queryvalues.ContainsKey("userid") ? queryvalues["userid"] : string.Empty;
            string UETitle = queryvalues.ContainsKey("title") ? queryvalues["title"] : string.Empty;
            string UEContent = queryvalues.ContainsKey("content") ? queryvalues["content"] : string.Empty;
            string UEItemValue = queryvalues.ContainsKey("gold") ? queryvalues["gold"] : string.Empty;
            string UEItemType = "1";
            string UEItemNum = "1";

            log.Info(UEUserID + "," + UETitle + "," + UEContent + "," + UEItemValue);

            UserEmail ue = new UserEmail { UEUserID = UEUserID };

            List<uint> UserIDList = UEUserID.Split(',').Select(x => Convert.ToUInt32(x)).ToList();



            Service_ItemMail_C ServiceItemMailC;


            ServiceItemMailC = Service_ItemMail_C.CreateBuilder()
                .AddRangeUserID(UserIDList)
                .SetTitle(UETitle)
                .SetContext(UEContent)
                .SetItemType(Convert.ToUInt32(UEItemType))
                .SetItemValue(Convert.ToUInt32(UEItemValue))
                .SetItemNum(Convert.ToUInt32(UEItemNum))
                .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SEND_ITEMMAIL, ServiceItemMailC.ToByteArray()));

            ue = new UserEmail { UEUserID = UEUserID, UETime = DateTime.Now, UETitle = UETitle, UEContent = UEContent, UEAuthor = "admin", UEItemNum = Convert.ToInt32(UEItemNum), UEItemType = (ueItemType)Convert.ToInt32(UEItemType), UEItemValue = Convert.ToInt32(UEItemValue) };


            if ((CenterCmd)tbind.header.CommandID == CenterCmd.CS_SEND_ITEMMAIL)
            {
                Service_ItemMail_S ServiceItemMailS = Service_ItemMail_S.ParseFrom(tbind.body.ToBytes());
                if (ServiceItemMailS.Suc)
                {
                    int result = UserEmailBLL.Add(ue);
                    if (result > 0)
                    {
                        return Content("0", "string");
                    }
                }

            }

            return Content("1", "string");


        }

        [QueryValues]
        public ActionResult SendSMS(Dictionary<string, string> queryvalues)
        {



            string account = "jiekou-clcs-13";
            string password = "Txb123456";
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : -1;
            string mobile = queryvalues.ContainsKey("mobile") ? queryvalues["mobile"] : string.Empty;
            Random r = new Random();
            int num = r.Next(100000, 999999);
            string sign = num.ToString();

            string content = "亲爱的用户，您的手机验证码是" + sign + "，此验证码1分钟内有效，请尽快完成验证。";
            string postStrTpl = "account={0}&pswd={1}&mobile={2}&msg={3}&needstatus=true&product=&extno=";
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(string.Format(postStrTpl, account, password, mobile, content));
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string msg = reader.ReadToEnd();
                //反序列化upfileMmsMsg.Text

                //保存数据
                NoAuthBLL.AddSMS(userid, sign);

                return Content("1", "string");
            }
            else
            {
                //访问失败
                return Content("0", "string");
            }





        }

        [QueryValues]
        public ActionResult SendMobile(Dictionary<string, string> queryvalues)
        {//发送手机短信
            string mobile = queryvalues.ContainsKey("mobile") ? queryvalues["mobile"] : string.Empty;
            string yzm = queryvalues.ContainsKey("yzm") ? queryvalues["yzm"] : string.Empty;
            string postStrTpl = "name={0}&pwd={1}&content={2}&mobile={3}&type=pt";

            string str = string.Format(
                postStrTpl,
                "张小勇",
                "7392E60BF411C7AE2170A4355223",
                "您正在登录验证，验证码" + yzm + "，请在30分钟内按页面提示提交验证码，切勿将验证码泄漏于他人。",
                mobile
               );

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(str);

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://web.daiyicloud.com/asmx/smsservice.aspx");
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush(); 
            newStream.Close();
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string msg = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(msg))
                {
                    string[] res = msg.Split(',');
                    if (res[0] == "0")
                    {
                        return Content("1", "string");
                    }
                    else
                    {
                        return Content("0", "string");
                    }
                }
                //保存数据
            }
            return Content("0", "string");
        }

        [QueryValues]
        public ActionResult SendEmail(Dictionary<string, string> queryvalues)
        {
            string email = queryvalues.ContainsKey("email") ? queryvalues["email"] : string.Empty;
            string yzm = queryvalues.ContainsKey("yzm") ? queryvalues["yzm"] : string.Empty;
            MailMessage mailObj = new MailMessage();
            mailObj.From = new MailAddress("523513655@qq.com"); //发送人邮箱地址
            mailObj.To.Add(email);   //收件人邮箱地址
            mailObj.Subject = "515游戏公司验证码";  //主题
            mailObj.Body = "您的验证码是：" + yzm+ "，请在30分钟内按页面提示提交验证码，切勿将验证码泄漏于他人。";    //正文
            mailObj.SubjectEncoding = System.Text.Encoding.ASCII;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.qq.com";         //smtp服务器名称
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential("523513655@qq.com", "fzgvpplnblpzcbac");  //发送人的登录名和授权码(腾讯的smtp是要用授权码而不是邮箱密码)
            try
            {
                smtp.Send(mailObj);
                return Content("发送邮件成功！");
            }
            catch (Exception ex)
            {
                return Content("发送邮件失败；" + ex.ToString());
            }
        }



        [QueryValues] 
        public ActionResult ValidateNameNO(Dictionary<string, string> queryvalues)
        {

            string name = queryvalues.ContainsKey("name") ? queryvalues["name"] : string.Empty;
            string no = queryvalues.ContainsKey("no") ? queryvalues["no"] : string.Empty;


            string lic = @"?v*qU=/S^C5q2Q%F+$+%<*:*H5?c?o;a?k?h7q?[XJ;*C]/F-YZV+WH ?c2jRqd5Wi/f>x:b.3?vJmJvBxFvBd[.a3?x?vGlHuUjdMCaa KfOz?x$r@sJc_wNa?x?m&w?g?s0b?v?jPuUoOmNa?x?vUdDdOmOs?x?m?h=d?/?h?v?jUd`7HxKh?x?v_nGhJv]t?xHXGZ4P7FMB]9:AJJ7x?d?s)p?/>c4uMePrFhb5?x7saXb2EwFqWzDsHyTj?x@g9.?g?g?g?g1f0[Eqa]bX\hXhWrRzd]8m?vOm`ZdI-;?v.xIiEw\tQvRuYeLaUy?x2gHuCmVbOtSyNfa3QrVrIhYw?x?v[o]aa7Vl?x?d?h7c2m6a?jKfOrLn^y.j?v?j_dYicXSnHxJ/HfEdNzEvZq<c6a?j_;CaHsc5AhBkJh(w";


            string xml = @"<?xml version='1.0' encoding='UTF-8' ?>
<ROWS><INFO>
<SBM>深圳市五一五游戏网络有限公司</SBM></INFO><ROW><GMSFHM>
公民身份号码</GMSFHM>
<XM>姓名</XM></ROW><ROW FSD='100600' YWLX='个人贷款'><GMSFHM>
" + no + @"</GMSFHM><XM>" + name + @"</XM></ROW>
</ROWS>";
            TestDemo.NciicServices ser = new TestDemo.NciicServices();

            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

            string res = ser.nciicCheck(lic, xml);



            if (res.Contains("<result_xm>不一致</result_xm>"))
            {
                return Json(new
                {
                    ret = 0,
                    message = "失败"
                }, JsonRequestBehavior.AllowGet);


            }
            else if (res.Contains("<result_xm>一致</result_xm>"))
            {
                return Json(new
                {
                    ret = 1,
                    message = "成功"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                ret = 0,
                message = "失败"
            }, JsonRequestBehavior.AllowGet);




        }



        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {

            //为了通过证书验证，总是返回true

            return true;

        }




        [QueryValues]
        public ActionResult GameLog(Dictionary<string, string> queryvalues)
        {
            string stime = queryvalues.ContainsKey("stime") ? queryvalues["stime"] : string.Empty;
            string etime = queryvalues.ContainsKey("etime") ? queryvalues["etime"] : string.Empty;

            while (true)
            {
                int curm = DateTime.Now.Minute;
                if (curm != 0 && curm != 30)
                {
                    int i = Math.Abs(curm - 30);
                    //暂停i分钟
                    Thread.Sleep(i * 60);
                }

            }




            //TexasGameLog(stime, etime);

            return Json(new
            {
                ret = 0,

            }, JsonRequestBehavior.AllowGet);

        }


        [QueryValues]
        public ActionResult RegisterSwitch(Dictionary<string, string> queryvalues)
        {

            int res = GameDataBLL.GetSwitchIsOpen(2);

            return Json(new { result = res }, JsonRequestBehavior.AllowGet);

        }



        private static void TexasGameLog(string startTime, string endTime)
        {
            //查询开始时间




            GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Texas", StartDate = startTime.ToString(), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };




            IEnumerable<TexasGameRecord> data = GameDataBLL.GetListForTexas(grv);




            List<CommonGameData> resdata = new List<CommonGameData>();



            foreach (TexasGameRecord m in data)
            {

                var userList = m.UserData.Split('_').ToList();
                userList.RemoveAt(userList.Count - 1);
                var j = userList.Count;
                for (int i = 0; i < j; i++)
                {
                    CommonGameData com = new CommonGameData();
                    var userData = userList[i].Split(',').ToList();
                    int tem = 0;

                    DateTime t = m.CreateTime;
                    com.CountDate = new DateTime(t.Year, t.Month, t.Day);
                    //int 房间ID = m.RoomID;
                    //decimal 牌局号 = m.Round;
                    string 盲注 = m.BaseScore;

                    string[] s = m.BaseScore.Split('/');
                    int v = int.Parse(s[0]);
                    tem = v;
                    //int 服务费 = m.Service;


                    var wj = userData[2];
                    com.UserID = int.Parse(wj);

                    decimal d = Convert.ToDecimal(userData[4]);
                    if (tem <= 100) { com.Initial = d; com.InitialCount = 1; }
                    else if (tem >= 5000) { com.HighRank = d; com.HighRankCount = 1; }
                    else { com.Secondary = d; com.SecondaryCount = 1; }

                    com.Key = com.UserID + com.CountDate.ToString();
                    resdata.Add(com);
                }
            }
            IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
            List<CommonGameData> sumData = new List<CommonGameData>();
            foreach (IGrouping<string, CommonGameData> info in query)
            {
                List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
                CommonGameData co = new CommonGameData();
                co.UserID = sl[0].UserID;
                co.CountDate = sl[0].CountDate;
                co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
                co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
                co.InitialCount = sl.Sum(m => m.InitialCount);
                co.HighRankL = sl.Where(m => m.HighRank < 0).Sum(m => m.HighRank);
                co.HighRankW = sl.Where(m => m.HighRank > 0).Sum(m => m.HighRank);
                co.HighRankCount = sl.Sum(m => m.HighRankCount);
                co.SecondaryL = sl.Where(m => m.Secondary < 0).Sum(m => m.Secondary);
                co.SecondaryW = sl.Where(m => m.Secondary > 0).Sum(m => m.Secondary);
                co.SecondaryCount = sl.Sum(m => m.SecondaryCount);
                sumData.Add(co);
            }

            foreach (CommonGameData comdata in sumData)
            {
                string sql = @"
replace into Clearing_Game(UserID ,CountDate ,Texas_LCount ,Texas_LAward_L ,Texas_LAward_W ,Texas_MCount ,Texas_MAward_L ,Texas_MAward_W ,Texas_HCount ,Texas_HAward_L ,Texas_HAward_W)
select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Texas_LCount,0) + " + comdata.InitialCount + " ,ifnull(b.Texas_LAward_L,0) + " + comdata.InitialL + " ,ifnull(b.Texas_LAward_W,0) + " + comdata.InitialW + @" 
,ifnull(b.Texas_MCount,0) + " + comdata.SecondaryCount + @" ,ifnull(b.Texas_MAward_L,0) + " + comdata.SecondaryL + @" ,ifnull(b.Texas_MAward_W,0) + " + comdata.SecondaryW + @" 
,ifnull(b.Texas_HCount,0) + " + comdata.HighRankCount + @" ,ifnull(b.Texas_HAward_L,0) + " + comdata.HighRankL + @" ,ifnull(b.Texas_HAward_W,0) + " + comdata.HighRankW + @"
from (select " + comdata.UserID + @" userid ) a left join (
select * from Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
";


                GameDataBLL.Add(sql);
            }

            GameDataBLL.UpdateBeginTimeForGame(grv);
            //修改时间



        }

        private const string _key = "515IWOXYHeYiw34x";
        private ILog log = LogManager.GetLogger("BindClub");
        [QueryValues]
        public ActionResult BindClub(Dictionary<string, string> queryvalues)
        {

            log.Info("绑定俱乐部URL: " + Utils.GetUrl());

            string id = queryvalues.ContainsKey("id") ? queryvalues["id"] : string.Empty;
            string clubid = queryvalues.ContainsKey("clubid") ? queryvalues["clubid"] : string.Empty;
            string sign = queryvalues.ContainsKey("sign") ? queryvalues["sign"] : string.Empty;



            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(clubid) || string.IsNullOrEmpty(sign))
            {
                return Content("-2");
            }

            sign = sign.ToUpper();

            string md5 = Utils.MD5(string.Concat(id, clubid, _key)).ToUpper();



            if (!sign.Equals(md5))
            {

                return Content("-1");
            }


            Beland_Club_C BelandlubC;
            // model.Minu 传一个时间长短给服务器
            BelandlubC = Beland_Club_C.CreateBuilder()
                   .SetClubID((uint)Convert.ToInt32(clubid))
                   .SetDwUserID((uint)Convert.ToInt32(id))
                   .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_BELAND_CLUB, BelandlubC.ToByteArray()));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_BELAND_CLUB:
                    Beland_Club_S BelandClubS = Beland_Club_S.ParseFrom(tbind.body.ToBytes());
                    bool res = BelandClubS.Suc;
                    if (res)
                    {
                        return Content("1");
                    }
                    else
                    {
                        return Content("0");
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Content("0");
        }



        [QueryValues]
        public ActionResult DownLoadFile(Dictionary<string, string> queryvalues)
        {

            return View();

        }




    }
}