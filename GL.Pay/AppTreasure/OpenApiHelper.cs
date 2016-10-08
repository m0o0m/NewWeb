using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.AppTreasure
{
    public class OpenApiHelper
    {
        public int Appid { get; set; }
        public string Openid { get; set; }
        public string OpenKey { get; set; }
        public string Pay_token { get; set; }
        public string PF { get; set; }
        public string PfKey { get; set; }
        public string Session_id { get; set; }

        public string Session_type { get; set; }

        public string billno { get; set; }
        public decimal amt { get; set; }
        //mpay/cancel_pay_m
        public RstArray InPay()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("appid", Appid.ToString());
          //  param.Add("appkey", GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));
            param.Add("openid", Openid);
            param.Add("openkey", OpenKey);
            if (Pay_token == null)
            {
                Pay_token = "";
            }
          
                param.Add("pay_token", Pay_token);
          
            param.Add("ts", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            param.Add("pf", PF);
            param.Add("pfkey", PfKey);
            param.Add("zoneid", "1");
            param.Add("amt", amt.ToString());
            string script_name = "/mpay/pay_m";

            Dictionary<string, string> cookie = new Dictionary<string, string>();
            cookie.Add("session_id", Session_id);
            cookie.Add("session_type", Session_type);
            cookie.Add("org_loc", script_name);

            RstArray result = new RstArray();
            OpenApiV3 sdk = new OpenApiV3(Appid, GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));

            string server_name = "msdk.qq.com";
            //  string server_name = "msdktest.qq.com";
            sdk.SetServerName(server_name);

            ILog log = LogManager.GetLogger("Callback");

            log.Info("应用宝支付参数:" + JsonConvert.SerializeObject(param));
            log.Info("应用宝支付参数:" + script_name);
            log.Info("应用宝支付参数:" + cookie);


            return sdk.ApiPay(script_name, param, cookie, "get", "http");
        }

        public RstArray CancelPay()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("appid", Appid.ToString());
           // param.Add("appkey", GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));
            param.Add("openid", Openid);
            param.Add("openkey", OpenKey);
            if (Pay_token == null)
            {
                Pay_token = "";
            }
           
                param.Add("pay_token", Pay_token);
         
            param.Add("ts", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            param.Add("pf", PF);
            param.Add("pfkey", PfKey);
            param.Add("zoneid", "1");
            param.Add("amt", amt.ToString());
            param.Add("billno", billno);
            string script_name = "/mpay/cancel_pay_m";

            Dictionary<string, string> cookie = new Dictionary<string, string>();
            cookie.Add("session_id", Session_id);
            cookie.Add("session_type", Session_type);
            cookie.Add("org_loc", script_name);

            RstArray result = new RstArray();
            OpenApiV3 sdk = new OpenApiV3(Appid, GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));

            string server_name = "msdk.qq.com";
            //  string server_name = "msdktest.qq.com";
            sdk.SetServerName(server_name);

            ILog log = LogManager.GetLogger("Callback");
            log.Info("应用宝取消支付参数:" + JsonConvert.SerializeObject(param));
            log.Info("应用宝取消支付参数:" + script_name);
            log.Info("应用宝取消支付参数:" + cookie);




            return sdk.ApiPay(script_name, param, cookie, "get", "http");
        }


        public  RstArray GetBalance()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("appid", Appid.ToString());
            param.Add("appkey", GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));
            param.Add("openid", Openid);
            param.Add("openkey", OpenKey);
            if (Pay_token == null) {
                Pay_token = "";
            }
          
                param.Add("pay_token", Pay_token);
         
          
            param.Add("ts", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            param.Add("pf", PF);
            param.Add("pfkey", PfKey);
            param.Add("zoneid", "1");

            string script_name = "/mpay/get_balance_m";

            Dictionary<string, string> cookie = new Dictionary<string, string>();
            cookie.Add("session_id", Session_id);
            cookie.Add("session_type", Session_type);
          
          
         
            cookie.Add("org_loc", script_name);

            RstArray result = new RstArray();
            OpenApiV3 sdk = new OpenApiV3(Appid, GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));



            string server_name = "msdk.qq.com";
            //  string server_name = "msdktest.qq.com";



            sdk.SetServerName(server_name);

            ILog log = LogManager.GetLogger("Callback");
            log.Info("应用宝查询余额参数:" + JsonConvert.SerializeObject(param));
            log.Info("应用宝查询余额路径:" + script_name);
          
            return sdk.ApiPay(script_name, param, cookie, "get", "http");
        }


        public RstArray GetBalanceLogin()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("appid", Appid.ToString());
            param.Add("appkey", GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));
            param.Add("openid", Openid);
            param.Add("openkey", OpenKey);
            if (Pay_token == null)
            {
                Pay_token = "";
            }

            param.Add("pay_token", Pay_token);

            string ts = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            param.Add("ts", ts);
            param.Add("pf", PF);
            param.Add("pfkey", PfKey);
            param.Add("zoneid", "1");

            string script_name = "/mpay/get_balance_m";

            Dictionary<string, string> cookie = new Dictionary<string, string>();
            cookie.Add("session_id", Session_id);
            cookie.Add("session_type", Session_type);



            cookie.Add("org_loc", script_name);







            RstArray result = new RstArray();
            OpenApiV3 sdk = new OpenApiV3(Appid, GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));



            string server_name = "ysdk.qq.com";
            //  string server_name = "msdktest.qq.com";



            sdk.SetServerName(server_name);

            ILog log = LogManager.GetLogger("Callback");
            log.Info("应用宝查询余额参数:" + JsonConvert.SerializeObject(param));
            log.Info("应用宝查询余额路径:" + script_name);

            return sdk.ApiPayLogin(script_name, param, cookie, "get", "https");
        }

        public RstArray InPayLogin()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("appid", Appid.ToString());
            //  param.Add("appkey", GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));
            param.Add("openid", Openid);
            param.Add("openkey", OpenKey);
            if (Pay_token == null)
            {
                Pay_token = "";
            }

            param.Add("pay_token", Pay_token);

            param.Add("ts", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            param.Add("pf", PF);
            param.Add("pfkey", PfKey);
            param.Add("zoneid", "1");
            param.Add("amt", amt.ToString());
            string script_name = "/mpay/pay_m";

            Dictionary<string, string> cookie = new Dictionary<string, string>();
            cookie.Add("session_id", Session_id);
            cookie.Add("session_type", Session_type);
            cookie.Add("org_loc", script_name);

            RstArray result = new RstArray();
            OpenApiV3 sdk = new OpenApiV3(Appid, GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));

            string server_name = "ysdk.qq.com";
            //  string server_name = "msdktest.qq.com";
            sdk.SetServerName(server_name);

            ILog log = LogManager.GetLogger("Callback");

            log.Info("应用宝支付参数:" + JsonConvert.SerializeObject(param));
            log.Info("应用宝支付参数:" + script_name);
            log.Info("应用宝支付参数:" + cookie);


            return sdk.ApiPayLogin(script_name, param, cookie, "get", "https");
        }

        public RstArray CancelPayLogin()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("appid", Appid.ToString());
            // param.Add("appkey", GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));
            param.Add("openid", Openid);
            param.Add("openkey", OpenKey);
            if (Pay_token == null)
            {
                Pay_token = "";
            }

            param.Add("pay_token", Pay_token);

            param.Add("ts", ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            param.Add("pf", PF);
            param.Add("pfkey", PfKey);
            param.Add("zoneid", "1");
            param.Add("amt", amt.ToString());
            param.Add("billno", billno);
            string script_name = "/mpay/cancel_pay_m";

            Dictionary<string, string> cookie = new Dictionary<string, string>();
            cookie.Add("session_id", Session_id);
            cookie.Add("session_type", Session_type);
            cookie.Add("org_loc", script_name);

            RstArray result = new RstArray();
            OpenApiV3 sdk = new OpenApiV3(Appid, GL.Pay.QQPay.Config.GetAppKey(Appid.ToString()));

            string server_name = "ysdk.qq.com";
            //  string server_name = "msdktest.qq.com";
            sdk.SetServerName(server_name);

            ILog log = LogManager.GetLogger("Callback");
            log.Info("应用宝取消支付参数:" + JsonConvert.SerializeObject(param));
            log.Info("应用宝取消支付参数:" + script_name);
            log.Info("应用宝取消支付参数:" + cookie);




            return sdk.ApiPayLogin(script_name, param, cookie, "get", "https");
        }

        public OpenApiHelper(int _appid,string _openid, string _openkey, string  _pay_token, string _pf, string _pfkey, string _session_id, string _session_type)
        {
            Appid = _appid;
            Openid = _openid;
            OpenKey = _openkey;
            Pay_token = _pay_token;
            PF = _pf;
            PfKey = _pfkey;
            Session_id = _session_id;
            Session_type = _session_type;
        }
    }
}
