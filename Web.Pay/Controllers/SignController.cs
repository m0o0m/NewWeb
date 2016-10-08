using GL.Common;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GL.Pay.Apple.ReceiptVerifier;
using GL.Data.Model;
using GL.Data.BLL;
using ProtoCmd.BackRecharge;
using GL.Protocol;
using Web.Pay.protobuf.SCmd;
using System.Text;
using GL.Pay.AliPay;

namespace Web.Pay.Controllers
{
    public class SignController : Controller
    {
        private const string _key = "515IWOXXXeYiw89y";
        private ILog log = LogManager.GetLogger("Sign");
        // GET: Sign
        [QueryValues]
        //[HttpPost]
        public ActionResult AppStore(Dictionary<string, string> queryvalues)
        {


//#if Debug
//            queryvalues = new Dictionary<string, string>();
//            queryvalues.Add("transtime", "1472653281");
//            queryvalues.Add("productid", "Chip_3");
//            queryvalues.Add("identityid", "403278");
//            queryvalues.Add("othertype", "2");
//            queryvalues.Add("other", "8B58253C-9B85-4D13-93EF-0F9D21162895");
//            queryvalues.Add("customsign", "7fa5c658db8804a14879c972f271d58d");

//            queryvalues.Add("transactionid", "440000246529282");
//            queryvalues.Add("isfirstrecharge", "0");
//            queryvalues.Add("transactionreceipt", "ewoJInNpZ25hdHVyZSIgPSAiQXhvZGJkQlFNWnBVUkhKbXQvdWw4MExza2RuWDZlaXVzaGl4ZzI4WFNOM0ROZTdBbFp3K2NPVDhENEo5Q0dUMlJUS0d4TlZCeVFnRjNzb3YrL2FISS9VWEZUL29rMUhtVXowcmRSVmhzaTErUmFvdVdDNmpqNzUydGR0WTQzb3FhQ0pPcnJ1VCs1ckI1VTl0aXFwNVRJR1BEbnRVTzhkeERuM2MwVjM0Qk5JcWt5aUQ4UmlOd1M3ZW55Wk04RkUxeGtMWm56ZW82MjNXRU9tU0Y2QTVVK2ZHMDBRQlcvUysweGZhUEU1TjN4WEk4bzVFNDl0UXlybUVRUWFKVGhBWFJLamJqMjJOTlRDMkZML0tOM2M1eU1mNkkrVU9WcE1sREUxZUVxalhOREZVRmZYYmFWVHJVaEtKTmo1Y1BWZ2Jid0laeW1kZlFpZU9rSzcwNDdvVU0vVUFBQVdBTUlJRmZEQ0NCR1NnQXdJQkFnSUlEdXRYaCtlZUNZMHdEUVlKS29aSWh2Y05BUUVGQlFBd2daWXhDekFKQmdOVkJBWVRBbFZUTVJNd0VRWURWUVFLREFwQmNIQnNaU0JKYm1NdU1Td3dLZ1lEVlFRTERDTkJjSEJzWlNCWGIzSnNaSGRwWkdVZ1JHVjJaV3h2Y0dWeUlGSmxiR0YwYVc5dWN6RkVNRUlHQTFVRUF3dzdRWEJ3YkdVZ1YyOXliR1IzYVdSbElFUmxkbVZzYjNCbGNpQlNaV3hoZEdsdmJuTWdRMlZ5ZEdsbWFXTmhkR2x2YmlCQmRYUm9iM0pwZEhrd0hoY05NVFV4TVRFek1ESXhOVEE1V2hjTk1qTXdNakEzTWpFME9EUTNXakNCaVRFM01EVUdBMVVFQXd3dVRXRmpJRUZ3Y0NCVGRHOXlaU0JoYm1RZ2FWUjFibVZ6SUZOMGIzSmxJRkpsWTJWcGNIUWdVMmxuYm1sdVp6RXNNQ29HQTFVRUN3d2pRWEJ3YkdVZ1YyOXliR1IzYVdSbElFUmxkbVZzYjNCbGNpQlNaV3hoZEdsdmJuTXhFekFSQmdOVkJBb01Da0Z3Y0d4bElFbHVZeTR4Q3pBSkJnTlZCQVlUQWxWVE1JSUJJakFOQmdrcWhraUc5dzBCQVFFRkFBT0NBUThBTUlJQkNnS0NBUUVBcGMrQi9TV2lnVnZXaCswajJqTWNqdUlqd0tYRUpzczl4cC9zU2cxVmh2K2tBdGVYeWpsVWJYMS9zbFFZbmNRc1VuR09aSHVDem9tNlNkWUk1YlNJY2M4L1cwWXV4c1FkdUFPcFdLSUVQaUY0MWR1MzBJNFNqWU5NV3lwb041UEM4cjBleE5LaERFcFlVcXNTNCszZEg1Z1ZrRFV0d3N3U3lvMUlnZmRZZUZScjZJd3hOaDlLQmd4SFZQTTNrTGl5a29sOVg2U0ZTdUhBbk9DNnBMdUNsMlAwSzVQQi9UNXZ5c0gxUEttUFVockFKUXAyRHQ3K21mNy93bXYxVzE2c2MxRkpDRmFKekVPUXpJNkJBdENnbDdaY3NhRnBhWWVRRUdnbUpqbTRIUkJ6c0FwZHhYUFEzM1k3MkMzWmlCN2o3QWZQNG83UTAvb21WWUh2NGdOSkl3SURBUUFCbzRJQjF6Q0NBZE13UHdZSUt3WUJCUVVIQVFFRU16QXhNQzhHQ0NzR0FRVUZCekFCaGlOb2RIUndPaTh2YjJOemNDNWhjSEJzWlM1amIyMHZiMk56Y0RBekxYZDNaSEl3TkRBZEJnTlZIUTRFRmdRVWthU2MvTVIydDUrZ2l2Uk45WTgyWGUwckJJVXdEQVlEVlIwVEFRSC9CQUl3QURBZkJnTlZIU01FR0RBV2dCU0lKeGNKcWJZWVlJdnM2N3IyUjFuRlVsU2p0ekNDQVI0R0ExVWRJQVNDQVJVd2dnRVJNSUlCRFFZS0tvWklodmRqWkFVR0FUQ0IvakNCd3dZSUt3WUJCUVVIQWdJd2diWU1nYk5TWld4cFlXNWpaU0J2YmlCMGFHbHpJR05sY25ScFptbGpZWFJsSUdKNUlHRnVlU0J3WVhKMGVTQmhjM04xYldWeklHRmpZMlZ3ZEdGdVkyVWdiMllnZEdobElIUm9aVzRnWVhCd2JHbGpZV0pzWlNCemRHRnVaR0Z5WkNCMFpYSnRjeUJoYm1RZ1kyOXVaR2wwYVc5dWN5QnZaaUIxYzJVc0lHTmxjblJwWm1sallYUmxJSEJ2YkdsamVTQmhibVFnWTJWeWRHbG1hV05oZEdsdmJpQndjbUZqZEdsalpTQnpkR0YwWlcxbGJuUnpMakEyQmdnckJnRUZCUWNDQVJZcWFIUjBjRG92TDNkM2R5NWhjSEJzWlM1amIyMHZZMlZ5ZEdsbWFXTmhkR1ZoZFhSb2IzSnBkSGt2TUE0R0ExVWREd0VCL3dRRUF3SUhnREFRQmdvcWhraUc5Mk5rQmdzQkJBSUZBREFOQmdrcWhraUc5dzBCQVFVRkFBT0NBUUVBRGFZYjB5NDk0MXNyQjI1Q2xtelQ2SXhETUlKZjRGelJqYjY5RDcwYS9DV1MyNHlGdzRCWjMrUGkxeTRGRkt3TjI3YTQvdncxTG56THJSZHJqbjhmNUhlNXNXZVZ0Qk5lcGhtR2R2aGFJSlhuWTR3UGMvem83Y1lmcnBuNFpVaGNvT0FvT3NBUU55MjVvQVE1SDNPNXlBWDk4dDUvR2lvcWJpc0IvS0FnWE5ucmZTZW1NL2oxbU9DK1JOdXhUR2Y4YmdwUHllSUdxTktYODZlT2ExR2lXb1IxWmRFV0JHTGp3Vi8xQ0tuUGFObVNBTW5CakxQNGpRQmt1bGhnd0h5dmozWEthYmxiS3RZZGFHNllRdlZNcHpjWm04dzdISG9aUS9PamJiOUlZQVlNTnBJcjdONFl0UkhhTFNQUWp2eWdhWndYRzU2QWV6bEhSVEJoTDhjVHFBPT0iOwoJInB1cmNoYXNlLWluZm8iID0gImV3b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdVdGNITjBJaUE5SUNJeU1ERTJMVEE0TFRNeElEQTNPakl4T2pFNUlFRnRaWEpwWTJFdlRHOXpYMEZ1WjJWc1pYTWlPd29KSW5CMWNtTm9ZWE5sTFdSaGRHVXRiWE1pSUQwZ0lqRTBOekkyTlRNeU56azBNamtpT3dvSkluVnVhWEYxWlMxcFpHVnVkR2xtYVdWeUlpQTlJQ0prWmpnMVpUUTBOekkxTm1RNU16SXhNR1F4WWpOak1tRTNaV1psTURCalptRmlOalptTWpJMElqc0tDU0p2Y21sbmFXNWhiQzEwY21GdWMyRmpkR2x2YmkxcFpDSWdQU0FpTkRRd01EQXdNalEyTlRJNU1qZ3lJanNLQ1NKaWRuSnpJaUE5SUNJNUlqc0tDU0poY0hBdGFYUmxiUzFwWkNJZ1BTQWlNVEExTkRNeE1qZzFOaUk3Q2draWRISmhibk5oWTNScGIyNHRhV1FpSUQwZ0lqUTBNREF3TURJME5qVXlPVEk0TWlJN0Nna2ljWFZoYm5ScGRIa2lJRDBnSWpFaU93b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdVdGJYTWlJRDBnSWpFME56STJOVE15TnprME1qa2lPd29KSW5WdWFYRjFaUzEyWlc1a2IzSXRhV1JsYm5ScFptbGxjaUlnUFNBaU1VTkJOVVkyUlRBdE1FVkVPQzAwTkVZeExUazRNVE10T1VNNE9ERXlNa1E1UXpoQklqc0tDU0pwZEdWdExXbGtJaUE5SUNJeE1EVTBNekl4T1RnMElqc0tDU0oyWlhKemFXOXVMV1Y0ZEdWeWJtRnNMV2xrWlc1MGFXWnBaWElpSUQwZ0lqZ3hPRFUxTnpRM09TSTdDZ2tpY0hKdlpIVmpkQzFwWkNJZ1BTQWlkM2wzWDBOb2FYQmZPQ0k3Q2draWNIVnlZMmhoYzJVdFpHRjBaU0lnUFNBaU1qQXhOaTB3T0Mwek1TQXhORG95TVRveE9TQkZkR012UjAxVUlqc0tDU0p2Y21sbmFXNWhiQzF3ZFhKamFHRnpaUzFrWVhSbElpQTlJQ0l5TURFMkxUQTRMVE14SURFME9qSXhPakU1SUVWMFl5OUhUVlFpT3dvSkltSnBaQ0lnUFNBaVkyOXRMbWRoYldVMU1UVXVkM2wzWkdWNmFHOTFJanNLQ1NKd2RYSmphR0Z6WlMxa1lYUmxMWEJ6ZENJZ1BTQWlNakF4Tmkwd09DMHpNU0F3TnpveU1Ub3hPU0JCYldWeWFXTmhMMHh2YzE5QmJtZGxiR1Z6SWpzS2ZRPT0iOwoJInBvZCIgPSAiNDQiOwoJInNpZ25pbmctc3RhdHVzIiA9ICIwIjsKfQ==");
//#endif



            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            string Key = _key;

            string _transactionid = queryvalues.ContainsKey("transactionid") ? queryvalues["transactionid"] : string.Empty;
            int _isfirstrecharge = queryvalues.ContainsKey("isfirstrecharge") ? Convert.ToInt32(queryvalues["isfirstrecharge"]) : 0;
            string _transactionreceipt = queryvalues.ContainsKey("transactionreceipt") ? queryvalues["transactionreceipt"] : string.Empty;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));

            if (!_customSign.Equals(md5))
            {
                log.Error("AppStore 参数不正确");
                return Json(new { result = 10001, msg = "AppStore 参数不正确" });
            }


            //Convert.FromBase64String(System.Text.Encoding.ASCII.GetBytes(_transactionreceipt))
            var res = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(_transactionreceipt));

            //(JObject)JsonConvert.DeserializeObject


            ReceiptManager receiptManager = new ReceiptManager();
            var env = Environments.Production;



            if (res.IndexOf("Sandbox") > 0)
            {
                env = Environments.Sandbox;
            }



            var response = receiptManager.ValidateReceipt(env, res);

            log.Info("response.Status="+response.Status);
            log.Info("Environments.Sandbox" + res.IndexOf("Sandbox"));


            if (response.Status > 0)
            {
                log.Error("AppStore ReceiptVerifier Error Status :"+ response.Status);
                return Json(new { result = response.Status });
            }


            if (!(response.Receipt.BundleIdentifier == "com.game515.wywdezhou" || response.Receipt.BundleIdentifier == "com.game515.515dezhou"))
            {
                log.Error("AppStore ReceiptVerifier Error bid :" + response.Receipt.BundleIdentifier);
                return Json(new { result = 10006, msg = "AppStore ReceiptVerifier Error bid :" + response.Receipt.BundleIdentifier });
            }


            try
            {


                //RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = out_trade_no });

                //if (rc == null)
                //{
                //    log.Error(" AppStore 订单[" + out_trade_no + "]不存在");
                //    return Content("fail");
                //}
                Recharge rc = RechargeBLL.GetModelByBillNo(new Recharge { BillNo = _transactionid });
                if (rc != null)
                {
                    log.Error(" AppStore 订单[" + _transactionid + "]已存在");
                    return Json(new { result = 1, msg = "AppStore 订单[" + _transactionid + "]已存在" });
                }


                Role user = RoleBLL.GetModelByID(new Role { ID = _identityid });
                if (user == null)
                {

                    log.Error(" AppStore 用户[" + _identityid + "]不存在");
                    return Json(new { result = 10002, msg = "AppStore 用户[" + _identityid + "]不存在" });
                }
                IAPProduct iap = IAPProductBLL.GetModelByID(_productid);
                if (iap == null)
                {
                    log.Error(" AppStore 支付配表出错");
                    return Json(new { result = 10003, msg = "AppStore 支付配表出错" });
                }


                isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;

              

                bool firstGif = iF == isFirst.是;


             
                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = _identityid });
                if (recharge != null)//首冲过
                {
                    if (firstGif == true)
                    {//又是首冲，则改变其为不首冲
                        firstGif = false;
                        ct = (chipType)1;
                        iF = isFirst.否;
                        iap.goodsType = 1;
                        log.Error(" 多次首冲，修改firstGif值");
                    }
                }

                RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = _transactionid, CreateTime = DateTime.Now, ChargeType = (int)raType.AppStore, OrderIP = GetClientIp() });
                RechargeIP ipmodel = GetCallBackClientIp();
                RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = _identityid, OrderID = _transactionid, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.AppStore });



                uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                if (firstGif)
                {
                    gold = (uint)(iap.goods + iap.attach_chip);
                    dia = (uint)iap.attach_5b;
                }
                string list = iap.attach_props;
                list = list.Trim(',') + (gold > 0 ? ",10000:" + gold + "," : "");
                list = list.Trim(',') + (dia > 0 ? ",20000:" + dia + "," : "");
                list = list.Trim(',') + ",";
                uint rmb = (uint)iap.price;
                log.Info(" 分 :"+ rmb * 100);
                // rmb = rmb * 100;//将元单位转换为分的单位转给游戏服务器



                RechargeBLL.Add(new Recharge { BillNo = _transactionid, OpenID = response.Receipt.UniqueIdentifier, UserID = _identityid, Money = (rmb * 100), CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.AppStore, UserAccount = user.NickName ,ActualMoney = (rmb * 100), ProductNO = list.Trim(',') });



                normal ServiceNormalS = normal.CreateBuilder()
                .SetUserID((uint)_identityid)
                .SetList(list)
                .SetRmb(rmb*100)//将元单位转换为分的单位转给游戏服务器
                .SetRmbActual(rmb * 100)
                .SetFirstGif(firstGif)
                .SetBillNo(_transactionid)
                .Build();

                Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                switch ((CenterCmd)tbind.header.CommandID)
                {
                    case CenterCmd.CS_NORMAL:
                        normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                        log.Info(" AppStore ServiceResult : " + ServiceNormalC.Suc);
                        if (ServiceNormalC.Suc)
                        {
                            log.Info(" AppStore ServiceResult [" + _transactionid + "] : " + ServiceNormalC.Suc);

                            RechargeBLL.UpdateReachTime(_transactionid);
                            //RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                        }
                       
                        //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                        break;
                    case CenterCmd.CS_CONNECT_ERROR:
                        break;
                }
              

                log.Info(" AppStore Process Success[" + _transactionid + "]: " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
              
                return Json(new
                {
                    result = 0,
                    productid = _productid,
                    transactionid = _transactionid,
                    isfirstrecharge = _isfirstrecharge
                });


                //log.Error(" AppStore fail : q " + Utils.GetUrl() + Utils.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
                //return Json(new { result = 10004, msg = "AppStore 服务器发货失败" });
            }
            catch (Exception err)
            {
                log.Error(" AppStore fail : q " + Utils.GetUrl() + Utils.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8), err);
                return Json(new { result = 10005, msg = "AppStore 服务器发货失败 err:" + err.Message });
            }
        }


        private string GetClientIp()
        {
            try
            {

                string userIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(userIP))
                {
                    userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(userIP))
                {
                    userIP = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return userIP;
            }
            catch {
                return "0.0.0.0";
            }
          

        }

        private RechargeIP GetCallBackClientIp()
        {
            RechargeIP mode = new RechargeIP();
            string userIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userIP))
            {
                userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                mode.Method = "REMOTE_ADDR";
            }
            else
            {
                mode.Method = "HTTP_X_FORWARDED_FOR";
            }
            mode.IP = userIP;
            return mode;

        }

    }
}