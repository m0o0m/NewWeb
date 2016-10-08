using GL.Common;
using GL.Data.BLL;
using GL.Data.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebGrease;

namespace MWeb.Controllers
{
    public class ADController : Controller
    {
       
      
        // GET: AD
        public ActionResult JRTT()
        {
            string ispost = Request["ispost"];
            string ip = Request.UserHostAddress;
            //AddClickInfo

            ADInfo info = new ADInfo() { ChannlID = 10024, IP = ip, Url = "/AD/JRTT" };
            ADBLL.Add(info);

            ADInfo info2 = new ADInfo() { ChannlID = 10024, IP = ip, Url = "/AD/JRTT" };
            ADBLL.AddClickInfo(info2);

            return Redirect("https://itunes.apple.com/cn/app/id1066070758");  

           
        }


        public ActionResult App() {
            return Redirect("https://itunes.apple.com/cn/app/id1066070758");
        }

        public ActionResult Appdm() {
            return View();
        }

        public ActionResult Appdm2()
        {
            return View();
        }

        public ActionResult UCTT()
        {
            string ispost = Request["ispost"];
            string ip = Request.UserHostAddress;

            ADInfo info = new ADInfo() { ChannlID = 10025, IP = ip, Url = "/AD/UCTT" };
            ADBLL.Add(info);

            ADInfo info2 = new ADInfo() { ChannlID = 10025, IP = ip, Url = "/AD/UCTT" };
            ADBLL.AddClickInfo(info2);
         

            return Redirect("https://itunes.apple.com/cn/app/id1066070758");
        }

        /// <summary>
        /// 点击汇报连接
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult DMADClick(Dictionary<string, string> queryvalues) {

         
            string appkey = queryvalues.ContainsKey("appkey") ? queryvalues["appkey"] : string.Empty;
            string ifa = queryvalues.ContainsKey("ifa") ? queryvalues["ifa"] : string.Empty;
            string ifamd5 = queryvalues.ContainsKey("ifamd5") ? queryvalues["ifamd5"] : string.Empty;
            string mac = queryvalues.ContainsKey("mac") ?queryvalues["mac"] : string.Empty;
            string macmd5 = queryvalues.ContainsKey("macmd5") ? queryvalues["macmd5"] : string.Empty;
            string source = queryvalues.ContainsKey("source") ? queryvalues["source"] : string.Empty;
            ILog log = log4net.LogManager.GetLogger("Callback");
            log.Info(" DMADClick Url: " + Utils.GetUrl());
            DMModel model = new DMModel()
            {
                Appkey = appkey,
                Ifa = ifa,
                Ifamd5 = ifamd5,
                Mac = mac,
                MacMD5 = macmd5,
                Source = source,
                Iddate =  Utils.GetTimeStampL(),
                 CreateTime = DateTime.Now,
                  Flag = 0
        };

            int i = ADBLL.Add(model);



            Response.Clear();
            if (i > 0)
            {
                return Json(new
                {
                    message = "信息保存成功",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new
                {
                    message = "信息保存失败",
                    success = false
                },JsonRequestBehavior.AllowGet);
            }
          

           
        }

        /// <summary>
        /// 激活链接
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult DMADActive(Dictionary<string, string> queryvalues) {
           
            ILog log = log4net.LogManager.GetLogger("Callback");
            log.Info("DMADActive Url: " + Utils.GetUrl());
            string recappkey = queryvalues.ContainsKey("appkey") ? queryvalues["appkey"] : string.Empty;
            string recifa = queryvalues.ContainsKey("ifa") ? queryvalues["ifa"] : string.Empty;
            string recifamd5 = queryvalues.ContainsKey("ifamd5") ? queryvalues["ifamd5"] : string.Empty;
            string recmac = queryvalues.ContainsKey("mac") ? queryvalues["mac"] : string.Empty;
            string recmacmd5 = queryvalues.ContainsKey("macmd5") ? queryvalues["macmd5"] : string.Empty;

            DMModel recModel = new DMModel()
            {
                Ifa = recifa,
                Ifamd5 = recifamd5,
                Mac = recmac,
                MacMD5 = recmacmd5,
                Appkey = recappkey
            };


            DMModel model = ADBLL.GetDMModel(recModel);
            if (model != null)//验证过了
            {
                    string url = "http://e.domob.cn/track/ow/api/callback?";
                    string appkey = model.Appkey;
                    string mac = model.Mac==null?"" : model.Mac;
                    string ifa = model.Ifa==null?"": model.Ifa;
                    string ifamd5 = model.Ifamd5==null?"": model.Ifamd5;
                    long acttime = Utils.GetTimeStampL();
                    int acttype = 2;
                    int returnFormat = 3;//3638ff657a23a5b5891fec6a8ea97230
                                         //                appkey = 1066070758
                                         //sign_key = 3638ff657a23a5b5891fec6a8ea97230

                log.Info("Appkey:" + model.Appkey+ @"
Mac="+ model.Mac + @"
MacMD5=" + model.MacMD5 + @"
Ifa=" + model.Ifa + @"
Ifamd5=" + model.Ifamd5 +@"
");
                string md5str = model.Appkey+","+ model.Mac+","+ model.MacMD5+","+model.Ifa+","+ model.Ifamd5+","+"3638ff657a23a5b5891fec6a8ea97230";
                log.Info("md5str:"+ md5str);
                string md5 = Utils.MD5(md5str);
                    string sign = md5;
                log.Info("md5=" + md5);
                    url = url + "appkey=" + appkey +
                             "&mac=" + mac +
                             "&ifa=" + ifa +
                             "&acttime=" + acttime +
                             "&acttype=" + acttype +
                             "&returnFormat=" + returnFormat +
                             "&sign=" + sign +
                             //"&actip=115.183.152.45" +
                             //"&appversion=2.0.1" +
                             //"&userid=4124bc0a9335c27f086f24ba207a4912" +
                             "&clktime=" + model.Iddate;
                   // "&clkip=119.255.14.220"; 
                    log.Info("请求多盟url="+ url);
          
                    string jstring = GL.Common.Helper.HttpClientHelper.GetResponse(url);


                   log.Info("请求多盟结果" + jstring);


                   string[] strs = jstring.Split(new string[] { "||" }, StringSplitOptions.None);

               

                    if (strs[1] == "true")
                    {
                        ADBLL.Update(new DMModel() { Id = model.Id });

                        return Json(new
                        {
                            message = "ok",
                            success = true,
                            data = model
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                     
                        return Json(new
                        {
                            message = "激活失败",
                            success = false
                        }, JsonRequestBehavior.AllowGet);
                    }
              
            }
            else {
                return Json(new
                {
                    message = "数据库无此信息",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

         
        }

        /// <summary>
        /// 去除重复
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult DMADRepeat(Dictionary<string, string> queryvalues) {

            ILog log = log4net.LogManager.GetLogger("Callback");
            log.Info("DMADRepeat Url: " + Utils.GetUrl());

            string appid = queryvalues.ContainsKey("appid") ? queryvalues["appid"] : string.Empty;
            string idfa = queryvalues.ContainsKey("idfa") ? queryvalues["idfa"] : string.Empty;
            string[] idfas = idfa.Trim().Split(',');
            List<DMModel> models = new List<DMModel>();
            string josn = "";
            for (int i = 0; i < idfas.Length; i++) {
                DMModel recModel = new DMModel()
                {
                    Ifa = idfas[i],
                    Ifamd5 = idfas[i],
                    Mac = idfas[i],
                    MacMD5 = idfas[i],
                    Appkey = appid
                };


                DMModel model = ADBLL.GetDMReapeatModel(recModel);
                if (model != null && model.Flag == 1)
                {//已经激活
                    recModel.Flag = 1;
                   
                }
                else {
                    recModel.Flag = 0;
                }
                josn = josn + "\""+recModel.Ifa + "\":\"" + recModel.Flag+"\",";
                
            }
            josn = josn.Trim(',');
            josn = "{" + josn + "}";
            return Content(josn);

        }
    }
}