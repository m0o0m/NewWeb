using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GL.Data.JWeb;
using GL.Data.JWeb.BLL;
using GL.Data.Model;
using System.Net.Sockets;
using System.Net;
using System.Text;
using log4net;
using GL.Command.DBUtility;
using GL.Common;

namespace JWeb.Controllers
{
    public class ClubController : Controller
    {
        internal static readonly string serverIP = PubConstant.GetConnectionString("serverIP");
        internal static readonly string serverPort = PubConstant.GetConnectionString("serverPort");
        internal static readonly string gameUrl = PubConstant.GetConnectionString("gameUrl");


        /// <summary>
        /// 初次进入页面,初始化
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        // GET: Club
        public ActionResult Index(Dictionary<string, string> queryvalues)
        {
            ILog log = LogManager.GetLogger("Club");
          


            int uid = queryvalues.ContainsKey("uid") ? (string.IsNullOrEmpty(queryvalues["uid"])?-1:Convert.ToInt32(queryvalues["uid"]) ): -1;
          
         
           // uid = 10701;
            string hip = serverIP;
            int hport = Convert.ToInt32(serverPort);

            Dictionary<char, char> arrC = new Dictionary<char, char>();
            /*'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'*/
            arrC.Add('0', 'a'); arrC.Add('1', 'b'); arrC.Add('2', 'c');
            arrC.Add('3', 'd'); arrC.Add('4', 'e'); arrC.Add('5', 'f');
            arrC.Add('6', 'g'); arrC.Add('7', 'h'); arrC.Add('8', 'i');
            arrC.Add('9', 'j');
            string uidStr = uid.ToString();
            string resStr = "";
            for (int i = 0; i < uidStr.Length; i++) {
                char ci = arrC[uidStr[i]];
                resStr += ci;
            }
            string code = resStr;
            resStr = gameUrl + resStr;

            //生成短网址

           // resStr =  BaiDuHelper.TransLongUrlToTinyUrl(resStr);





             //发送socket请求
             IPAddress ip = IPAddress.Parse(hip);
            IPEndPoint ipe = new IPEndPoint(ip, hport);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ipe);
            Int16 head = 8;
            Int16 protol = 1;
            Int32 content = uid;
            byte[] headBytes = System.BitConverter.GetBytes(head); 
            byte[] protolBytes = System.BitConverter.GetBytes(protol);
            byte[] contentBytes = System.BitConverter.GetBytes(content);
            byte[] c = new byte[headBytes.Length + protolBytes.Length+ contentBytes.Length];
            headBytes.CopyTo(c, 0);
            protolBytes.CopyTo(c, headBytes.Length);
            contentBytes.CopyTo(c, headBytes.Length+ protolBytes.Length);

        

            clientSocket.Send(c);
            // = unpack("vv1/vv2/Lc1/Lc2/Lc3/a16c4/cc5",$out);
            //vv1/vv2/Lc1/Lc2/Lc3/a16c4/cc5
            //2位/2位/4位/4位/4位/16位/1位

       

            byte[] recBytes = new byte[1024];
            int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);

         

            if (bytes <= 0)
            {
                //没有登录
                return View("Nologin", bytes);
            }
            else {
                Int16 v1 = System.BitConverter.ToInt16(recBytes, 0);
                Int16 v2 = System.BitConverter.ToInt16(recBytes, 2);
                Int32 c1 = System.BitConverter.ToInt32(recBytes, 4);
                Int32 c2 = System.BitConverter.ToInt32(recBytes, 8);
                Int32 c3 = System.BitConverter.ToInt32(recBytes, 12);

             
                string c4 = Encoding.ASCII.GetString(recBytes, 16, 16);
              

                byte c5 = recBytes[32];
              

                ClubInit model = new ClubInit()
                {
                    C1 = c1,
                    C2 = c2,
                    C3 = c3,
                    C4 = c4,
                    C5 = c5,
                    V1 = v1,
                    V2 = v2,
                    Mark = resStr,
                    Ip = hip,
                    Port = hport,
                    UID = uid,
                    Code = code
                };
                clientSocket.Close();


                //查询本周总贡献总额
               Int64 weekTotal =  ClubBLL.GetClubWeekTotal(uid);
                string fanli = "20";
                string nextTotal = "";
                string nextFanLi = "";
                if (weekTotal < 500000000)
                {
                    fanli = "20";
                    nextTotal = "5";
                    nextFanLi = "25";
                }
                else if(weekTotal < 1000000000)
                {
                    fanli = "25";
                    nextTotal = "10";
                    nextFanLi = "30";
                }
                else if (weekTotal < 3000000000)
                {
                    fanli = "30";
                    nextTotal = "30";
                    nextFanLi = "40";
                }
                else if (weekTotal < 5000000000)
                {
                    fanli = "40";
                    nextTotal = "50";
                    nextFanLi = "50";
                }
                else
                {
                    fanli = "50";
                }
                model.WeekTotal = weekTotal;
                model.FanLi = fanli;
                model.NextTotal = nextTotal;
                model.NextFanLi = nextFanLi;
                model.WeekTotalStr = weekTotal.ToString("#,0.");
                return View(model);
            }

           

            
        }


        [QueryValues]
        public ActionResult PhoneIndex(Dictionary<string, string> queryvalues)
        {
            ILog log = LogManager.GetLogger("Club");



            int uid = queryvalues.ContainsKey("uid") ? (string.IsNullOrEmpty(queryvalues["uid"]) ? -1 : Convert.ToInt32(queryvalues["uid"])) : -1;


            //uid = 10701;
            string hip = serverIP;
            int hport = Convert.ToInt32(serverPort);

            Dictionary<char, char> arrC = new Dictionary<char, char>();
            /*'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'*/
            arrC.Add('0', 'a'); arrC.Add('1', 'b'); arrC.Add('2', 'c');
            arrC.Add('3', 'd'); arrC.Add('4', 'e'); arrC.Add('5', 'f');
            arrC.Add('6', 'g'); arrC.Add('7', 'h'); arrC.Add('8', 'i');
            arrC.Add('9', 'j');
            string uidStr = uid.ToString();
            string resStr = "";
            for (int i = 0; i < uidStr.Length; i++)
            {
                char ci = arrC[uidStr[i]];
                resStr += ci;
            }
            string code = resStr;
            resStr = gameUrl + resStr;


            //resStr = BaiDuHelper.TransLongUrlToTinyUrl(resStr);


            //发送socket请求
            IPAddress ip = IPAddress.Parse(hip);
            IPEndPoint ipe = new IPEndPoint(ip, hport);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ipe);
            Int16 head = 8;
            Int16 protol = 1;
            Int32 content = uid;
            byte[] headBytes = System.BitConverter.GetBytes(head);
            byte[] protolBytes = System.BitConverter.GetBytes(protol);
            byte[] contentBytes = System.BitConverter.GetBytes(content);
            byte[] c = new byte[headBytes.Length + protolBytes.Length + contentBytes.Length];
            headBytes.CopyTo(c, 0);
            protolBytes.CopyTo(c, headBytes.Length);
            contentBytes.CopyTo(c, headBytes.Length + protolBytes.Length);



            clientSocket.Send(c);
            // = unpack("vv1/vv2/Lc1/Lc2/Lc3/a16c4/cc5",$out);
            //vv1/vv2/Lc1/Lc2/Lc3/a16c4/cc5
            //2位/2位/4位/4位/4位/16位/1位



            byte[] recBytes = new byte[1024];
            int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);



            if (bytes <= 0)
            {
                //没有登录
                return View("Nologin", bytes);
            }
            else
            {
                Int16 v1 = System.BitConverter.ToInt16(recBytes, 0);
                Int16 v2 = System.BitConverter.ToInt16(recBytes, 2);
                Int32 c1 = System.BitConverter.ToInt32(recBytes, 4);
                Int32 c2 = System.BitConverter.ToInt32(recBytes, 8);
                Int32 c3 = System.BitConverter.ToInt32(recBytes, 12);


                string c4 = Encoding.ASCII.GetString(recBytes, 16, 16);


                byte c5 = recBytes[32];


                ClubInit model = new ClubInit()
                {
                    C1 = c1,
                    C2 = c2,
                    C3 = c3,
                    C4 = c4,
                    C5 = c5,
                    V1 = v1,
                    V2 = v2,
                    Mark = resStr,
                    Ip = hip,
                    Port = hport,
                    UID = uid,
                     Code = code
                };
                clientSocket.Close();


                //查询本周总贡献总额
                Int64 weekTotal = ClubBLL.GetClubWeekTotal(uid);
                string fanli = "20";
                string nextTotal = "";
                string nextFanLi = "";
                if (weekTotal < 500000000)
                {
                    fanli = "20";
                    nextTotal = "5";
                    nextFanLi = "25";
                }
                else if (weekTotal < 1000000000)
                {
                    fanli = "25";
                    nextTotal = "10";
                    nextFanLi = "30";
                }
                else if (weekTotal < 3000000000)
                {
                    fanli = "30";
                    nextTotal = "30";
                    nextFanLi = "40";
                }
                else if (weekTotal < 5000000000)
                {
                    fanli = "40";
                    nextTotal = "50";
                    nextFanLi = "50";
                }
                else
                {
                    fanli = "50";
                }
                model.WeekTotal = weekTotal;
                model.FanLi = fanli;
                model.NextTotal = nextTotal;
                model.NextFanLi = nextFanLi;
                model.WeekTotalStr = weekTotal.ToString("#,0.");
                return View(model);
            }


        }

        /// <summary>
        /// 得到俱乐部贡献列表
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult GetClubGives(Dictionary<string, string> queryvalues) {
            int uid = queryvalues.ContainsKey("clubid") ? (string.IsNullOrEmpty(queryvalues["clubid"]) ? -1 : Convert.ToInt32(queryvalues["clubid"])) : -1;
            //uid = 10701;
            IEnumerable<ClubGive> clubGives = ClubBLL.GetClubGive(uid);
            List<ClubGive> resclubGives ;
            if (clubGives == null)
            {
                resclubGives = null;
            }
            else
            {
                resclubGives = new List<ClubGive>();
                foreach (var item in clubGives)
                {
                    ClubGive give = new ClubGive()
                    {
                        CreateTime = item.CreateTime.Replace("0:00:00", "").Replace("/","-"),
                        Gold = item.Gold,
                        GoldStr = item.Gold.ToString("#,0.")
                    };
                    resclubGives.Add(give);
                }
            }



            //查询本周总贡献总额
            Int64 weekTotal = ClubBLL.GetClubWeekTotal(uid);
            string fanli = "20";
            string nextTotal = "";
            string nextFanLi = "";
            if (weekTotal < 500000000)
            {
                fanli = "20";
                nextTotal = "5";
                nextFanLi = "25";
            }
            else if (weekTotal < 1000000000)
            {
                fanli = "25";
                nextTotal = "10";
                nextFanLi = "30";
            }
            else if (weekTotal < 3000000000)
            {
                fanli = "30";
                nextTotal = "30";
                nextFanLi = "40";
            }
            else if (weekTotal < 5000000000)
            {
                fanli = "40";
                nextTotal = "50";
                nextFanLi = "50";
            }
            else
            {
                fanli = "50";
            }




            return Json(new {
                data = resclubGives,
                WeekTotal = weekTotal.ToString("#,0."),
                FanLi = fanli,
                NextTotal = nextTotal,
                NextFanLi = nextFanLi
            });
        }

        /// <summary>
        /// 得到俱乐部成员列表 
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult GetMember(Dictionary<string, string> queryvalues) {
            int clubid = queryvalues.ContainsKey("clubid") ? Convert.ToInt32(queryvalues["clubid"]) : -1;
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : -1;
            int curTime = queryvalues.ContainsKey("curTime") ? Convert.ToInt32(queryvalues["curTime"]) : 0;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string preID = queryvalues.ContainsKey("preID") ? Convert.ToString(queryvalues["preID"]) : "";
            DateTime time = DateTime.Now.AddDays(curTime * -1);
            time = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            //clubid = 10701;
            type = 2;

            int mycount = ClubBLL.GetClubUserCount(clubid);
            int actioncount = 0;
            if (type == 1)
            {
                actioncount = ClubBLL.GetCommonClubCount(clubid);
            }
            else {
                actioncount = ClubBLL.GetHYClubCount(clubid);
            }
            //获取数据
            IEnumerable<MemberMender> data = ClubBLL.GetMemberMender(clubid, time,page);
            List<MemberMender> resData = new List<MemberMender>();
            foreach (var item in data)
            {
              
                MemberMender mem = new MemberMender();
                mem.Gold = item.Gold;
                mem.GoldStr = item.Gold.ToString("#,0.");
                mem.NickName = item.NickName;
                mem.ID = item.ID;
              
                if (item.LastLogin<=0)
                {
                    mem.BeforeLogin = "今天";
                }
                else   if (item.LastLogin < 30)
                {
                    mem.BeforeLogin = item.LastLogin + "天前";
                }
                else if (item.LastLogin < 3 * 30)
                {
                    mem.BeforeLogin = Convert.ToInt32(item.LastLogin / 30) + "个月前";
                }
                else {
                    mem.BeforeLogin = "3个月前";
                }
                resData.Add(mem);
            }

            return Json(new {
                mycount = mycount,
                actioncount = actioncount,
                data = resData,
                time = time.ToString("yyyy-MM-dd")
            });
        }

        /// <summary>
        /// 领取俱乐部奖励
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult GetAward(Dictionary<string, string> queryvalues) {
            int clubid = queryvalues.ContainsKey("clubid") ? (string.IsNullOrEmpty(queryvalues["clubid"]) ? -1 : Convert.ToInt32(queryvalues["clubid"])) : -1;
            int hport = Convert.ToInt32(serverPort);
            string hip = serverIP;


            //发送socket请求
            IPAddress ip = IPAddress.Parse(hip);
            IPEndPoint ipe = new IPEndPoint(ip, hport);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ipe);

            Int16 head = 8;
            Int16 protol = 3;
            Int32 content = clubid;
            byte[] headBytes = System.BitConverter.GetBytes(head);
            byte[] protolBytes = System.BitConverter.GetBytes(protol);
            byte[] contentBytes = System.BitConverter.GetBytes(content);
            byte[] c = new byte[headBytes.Length + protolBytes.Length + contentBytes.Length];
            headBytes.CopyTo(c, 0);
            protolBytes.CopyTo(c, headBytes.Length);
            contentBytes.CopyTo(c, headBytes.Length + protolBytes.Length);

            clientSocket.Send(c);


            byte[] recBytes = new byte[1024];
            int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
            Int16 v1 = System.BitConverter.ToInt16(recBytes, 0);
            Int16 v2 = System.BitConverter.ToInt16(recBytes, 2);
            Int32 c1 = System.BitConverter.ToInt32(recBytes, 4);
            string c2 = Encoding.ASCII.GetString(recBytes, 8, 16);
            byte c3 = recBytes[24];
            //unpack("vv1/vv2/Lc1/a16c2/cc3",$out);
            //        2    2   4   16    1
            //  string c4 = Encoding.ASCII.GetString(recBytes, 16, 16);

            return Json(new
            {
                c3 = c3,
                c2 = c2
            });
        }

        /// <summary>
        /// 左边的那个菜单，点击之后加载index页面
        /// </summary>
        /// <returns></returns>
        [QueryValues]
        public ActionResult LeftMenu(Dictionary<string, string> queryvalues) {

            ILog log = LogManager.GetLogger("Club");
            log.Info("进入LeftMenu");


            int uid = queryvalues.ContainsKey("uid") ? (string.IsNullOrEmpty(queryvalues["uid"]) ? -1 : Convert.ToInt32(queryvalues["uid"])) : -1;
            //int hport = queryvalues.ContainsKey("hpost") ? (string.IsNullOrEmpty(queryvalues["hpost"]) ? -1 : Convert.ToInt32(queryvalues["hpost"])) : -1;
            // string hip = queryvalues.ContainsKey("hip") ? queryvalues["hip"] : "";
            //uid = 10701;
            int hport = Convert.ToInt32(serverPort);
            string hip = serverIP;
            //115.159.100.154
            //192.168.1.18

            ClubInit model = new ClubInit()
            {
                Ip = hip,
                Port = hport,
                UID = uid
            };
            return View(model);
        }


        public ActionResult Nologin() {
            return View();
        }

        /// <summary>
        /// 测试页面
        /// </summary>
        /// <returns></returns>
        [QueryValues]
        public ActionResult Test(Dictionary<string, string> queryvalues) {
          
                return View();
           
        }
    }
}