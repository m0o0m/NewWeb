using GL.Command.DBUtility;
using GL.Common;
using GL.Data.BLL;
using GL.Data.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MWeb.Controllers
{
    public class SimulatorRechargeController : Controller
    {
        private const string _key = "515IWOXXXeYiw89y";
        internal static readonly string payUrl = PubConstant.GetConnectionString("payUrl");
        internal static readonly string monicallbackUrl = PubConstant.GetConnectionString("monicallbackUrl");


        public static readonly string Coin = PubConstant.GetConnectionString("coin");

        // GET: SimulatorRecharge
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult MZ() {

            #region 组装数据
            string _other = "868232000894311";
            string _productid = "Chip_12";
            int _identityid = 88;
            int _othertype = 2;
            string _customsign = "";
            string _transtime = "1460182643";
            _customsign = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, _key));
            #endregion

            #region 生成订单
            string url = payUrl+ "/Pay/YYHPay?" + @"
other=" + _other + @"&
productid=" + _productid + @"&
identityid=" + _identityid + @"&
othertype=" + _othertype + @"&
customsign=" + _customsign + @"&
transtime=" + _transtime + @"
";
            url = url.Replace("\r", "").Replace("\n", "");

            System.Net.WebRequest wReq = System.Net.WebRequest.Create(url);

            System.Net.WebResponse wResp = wReq.GetResponse();
            System.IO.Stream respStream = wResp.GetResponseStream();
            // Dim reader As StreamReader = New StreamReader(respStream)
            string s = "";
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream))
            {
                s = reader.ReadToEnd();
            }
            string order = "";
            #endregion

            #region 发货


            string json = "transdata={\"exorderno\":\"YYHPay20160411114633262\",\"transid\":\"Simulated00000000000\",\"waresid\":1,\"appid\":\"5000805433\",\"feetype\":0,\"money\":100,\"count\":1,\"result\":0,\"transtype\":0,\"transtime\":\"2016-04-09 14:30:11\",\"cpprivate\":\"515游戏-1元首冲大礼包\",\"paytype\":401}&sign=7ee830b175e9de8d24f199a718daa9fd 6c6def0ccada85dab58e0ae796283fa6 2a01126a488b125dbcdbb714f2db6466 ";

            string callbackurl = payUrl+ "/Callback/YYHPay";

            callbackurl = callbackurl.Replace("\r", "").Replace("\n", "");
            System.Net.WebRequest cwReq = System.Net.WebRequest.Create(callbackurl);
            byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(json);
            cwReq.Method = "POST";
            cwReq.ContentType = "text/xml";
            cwReq.ContentLength = requestBytes.Length;
            Stream requestStream = cwReq.GetRequestStream();
            requestStream.Write(requestBytes, 0, requestBytes.Length);
            requestStream.Close();
            System.Net.WebResponse cResp = cwReq.GetResponse();
            System.IO.Stream crespStream = cResp.GetResponseStream();
            // Dim reader As StreamReader = New StreamReader(respStream)
            string cs = "";
            using (System.IO.StreamReader creader = new System.IO.StreamReader(crespStream))
            {
                cs = creader.ReadToEnd();
            }
            return Content("1");
            #endregion
        }

   

        [QueryValues]
        public ActionResult CommonPay(Dictionary<string, string> queryvalues)
        {

            #region 组装数据 生成订单

            string billNO = Utils.GenerateOutTradeNo(queryvalues["Name"]);
            AddNo(queryvalues, billNO);
            CommonRecive rec = CallbackData(queryvalues, billNO);
            #endregion

            #region 发货

            string callbackurl = rec.URL + @"
amt="+ rec.Amt+ @"&" + @"
appid="+rec.Appid+@"&" + @"
billno="+ billNO + @"&" + @"
ibazinga=1&" + @"
openid="+rec.Openid+@"&" + @"
payamt_coins=0&" + @"
paychannel=qdqb&" + @"
paychannelsubid=1&" + @"
payitem="+rec.Payitem+@"&" + @"
providetype=0&" + @"
pubacct_payamt_coins=&" + @"
token="+Guid.NewGuid().ToString()+@"&" + @"
ts=1460310868&version=v3&" + @"
zoneid=0&" + @"
trade_status=1&" + @"
sig=123&" + @"
out_trade_no=" + billNO + @"&" + @"
sign=123" + @"
";

            callbackurl = callbackurl.Replace("\r", "").Replace("\n", "");
            System.Net.WebRequest cwReq = System.Net.WebRequest.Create(callbackurl);


            System.Net.WebResponse cResp = cwReq.GetResponse();
            System.IO.Stream crespStream = cResp.GetResponseStream();
            // Dim reader As StreamReader = New StreamReader(respStream)
            string cs = "";
            using (System.IO.StreamReader creader = new System.IO.StreamReader(crespStream))
            {
                cs = creader.ReadToEnd();
            }
            return Content(cs);
            #endregion
        }



        private string AddNo(Dictionary<string, string> queryvalues, string billNo)
        {
            SimulatorRecharge model = new GL.Data.Model.SimulatorRecharge();
            int UserID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"].ToString()) : 0;
            int Type = queryvalues.ContainsKey("Type") ? Convert.ToInt32(queryvalues["Type"].ToString()) : 1;
            decimal Money = queryvalues.ContainsKey("Money") ? Convert.ToDecimal(queryvalues["Money"].ToString()) : 0;
            string ProductID = queryvalues.ContainsKey("ProductID") ? queryvalues["ProductID"].ToString() : "";
            double Discounted = queryvalues.ContainsKey("Discounted") ? Convert.ToDouble(queryvalues["Discounted"].ToString()) : 0;

            model.UserID = UserID;
            model.Type = Type;
            model.Money = Money;
            model.Discounted = Discounted;

            string billNO = billNo;
            if (Type == 3)
            {
                int userid = UserID;
                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });
                if (recharge != null)//首冲过
                {
                    return "2";//重复首冲
                }
            }
            long transtimeL = Utils.GetTimeStampL();
            RechargeCheckBLL.Add(new RechargeCheck
            {
                Money = (int)model.Money,
                ProductID = ProductID,
                SerialNo = billNO,
                UserID = UserID,
                CreateTime = (ulong)transtimeL
            }
            );

            return billNO;
        }

        private CommonRecive CallbackData(Dictionary<string, string> queryvalues, string billNo)
        {
            CommonRecive model = new CommonRecive();
            int UserID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"].ToString()) : 0;
            int Type = queryvalues.ContainsKey("Type") ? Convert.ToInt32(queryvalues["Type"].ToString()) : 1;
            decimal Money = queryvalues.ContainsKey("Money") ? Convert.ToDecimal(queryvalues["Money"].ToString()) : 0;
            string ProductID = queryvalues.ContainsKey("ProductID") ? queryvalues["ProductID"].ToString() : "";
            double Discounted = queryvalues.ContainsKey("Discounted") ? Convert.ToDouble(queryvalues["Discounted"].ToString()) : 0;
            string URL = queryvalues.ContainsKey("URL") ? queryvalues["URL"].ToString() : "";



            model.Appid = "1103881749";
            model.Amt = (int)(((int)Money * 100) * Discounted * 0.1);
            model.Sign = "123";
            model.URL = "";
            model.Payitem = billNo + "*" + Money + "*1";
            model.Billno = billNo;
            model.URL = monicallbackUrl+"/" + URL;

            Role r = RoleBLL.GetModelByID(new Role() { ID = UserID });
            model.Openid = r.OpenID;
            return model;
        }

        [QueryValues]
        public ActionResult AppTreasure(Dictionary<string, string> queryvalues)
        {
            return View();
        }
        [QueryValues]
        public ActionResult GetAppTreasureYE(Dictionary<string, string> queryvalues) {
            int UserID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"].ToString()) : 0;

            AppTreasure model = RechargeCheckBLL.GetModelByUserID(UserID.ToString());


            return Content(model.Balance.ToString());
        }
        [QueryValues]
        public ActionResult AppTreasureRechar(Dictionary<string, string> queryvalues) {
            int UserID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"].ToString()) : 0;
            string ProductID = queryvalues.ContainsKey("ProductID") ?queryvalues["ProductID"].ToString() :"";


            string url = payUrl+ "/Callback/AppTrePay?userid=" + UserID +"&productID="+ ProductID;
            url = url.Replace("\r", "").Replace("\n", "");

            System.Net.WebRequest wReq = System.Net.WebRequest.Create(url);

            System.Net.WebResponse wResp = wReq.GetResponse();
            System.IO.Stream respStream = wResp.GetResponseStream();
            // Dim reader As StreamReader = New StreamReader(respStream)
            string s = "";
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream))
            {
                s = reader.ReadToEnd();
            }

            return Content(s);
        }





    }





    public class CommonRecive{
         public string Sign { get; set; }
         public string Appid { get; set; }

        public string Payitem { get; set; }

        public int Amt { get; set; }

        public string Billno { get; set; }

        public string Openid { get; set; }
        public string URL { get; set; }
    }
}