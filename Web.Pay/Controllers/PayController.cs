using GL.Common;
using GL.Data.Model;
using GL.Data.BLL;
using GL.Pay.YeePay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GL.Pay.WxPay;
using log4net;
using GL.Pay.QQPay;
using GL.Pay.AliPay;
using Web.Pay.Models;
using Newtonsoft.Json;
using GL.Pay.MeiZu;

namespace Web.Pay.Controllers
{
    public class PayController : Controller
    {
        private ILog log = LogManager.GetLogger("Web.Pay");

        private const string _key = "515IWOXXXeYiw89y";



        /// <summary>
        /// transtime   	交易时间  时间戳，例如：1361324896，精确到秒
        /// productid 	    商品ID 
        /// identityid  	用户ID
        /// othertype       终端类型  0、IMEI；1、MAC；2、UUID；3、other，如果终端类型未知的话，那么就传other
        /// other       	终端硬件标识  
        /// customSign  	MD5码。transtime+identityid+othertype+other+productid+约定字符串 计算出的MD5码  
        /// 约定字符串为:515IWOXXXeYiw89y
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult Yeepay(Dictionary<string, string> queryvalues)
        {

            log.Info("##################Yeepay易宝支付生成订单号开始: #######################");
            log.Info("生成订单Yeepay: " + Utils.GetUrl());

            return Content("请使用别的充值方式!");

            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            string Key = _key;


            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));

            if (!_customSign.Equals(md5))
            {
                log.Error("生成订单Yeepay：MD5校验失败_customSign="+ _customSign+ ",md5="+ md5);
                return Content("参数错误!");
            }

        


            //请求移动终端网页收银台支付

            //一键支付URL前缀
            string apiprefix = APIURLConfig.payMobilePrefix;

            //网页支付地址
            string mobilepayURI = APIURLConfig.webpayURI;

            //商户账户编号
            string merchantAccount = GL.Pay.YeePay.Config.merchantAccount;

            //商户公钥（该商户公钥需要在易宝一键支付商户后台报备）
            string merchantPublickey = GL.Pay.YeePay.Config.merchantPublickey;

            //商户私钥（商户公钥对应的私钥）
            string merchantPrivatekey = GL.Pay.YeePay.Config.merchantPrivatekey;

            //易宝支付分配的公钥（进入商户后台公钥管理，报备商户的公钥后分派的字符串）
            string yibaoPublickey = GL.Pay.YeePay.Config.yibaoPublickey;

            //随机生成商户AESkey
            string merchantAesKey = AES.GenerateAESKey();

            //日志字符串
            StringBuilder logsb = new StringBuilder();
            logsb.Append(DateTime.Now.ToString() + "\n");


            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);


            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是) {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//首冲过
                {
                    log.Error("生成订单Yeepay： 重复首冲 ");
                    return Content("重复首冲!");
                }


            }



                Random ra = new Random();
            int amount = Convert.ToInt32(iap.price * 100);//支付金额为分
            int currency = 156;//币种，默认为156人民币
            string identityid = _identityid.ToString();//用户身份标识
            int identitytype = 2; //用户身份标识类型，0为IMEI，1为MAC，2为用户ID，3为用户Email，4为用户手机号，5为用户身份证号，6为用户纸质协议单号

            int transtime = Utils.GetTimeStampI();//交易发生时间，时间戳，例如：1361324896，精确到秒
            long transtimeL = Utils.GetTimeStampL();//交易发生时间，时间戳，例如：1361324896，精确到秒

            string orderid = Utils.GenerateOutTradeNo("Yeepay");// string.Format("yeepay{0:00000000000000}", transtimeL);
            int orderexpdate = 60;//订单有效期，单位：分钟，例如：60，表示订单有效期为60分钟
            string productcatalog = "1";//商品类别码，商户支持的商品类别码由易宝支付运营人员根据商务协议配置
            string productdesc = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            string productname = iap.productname;
            log.Info("生成订单Yeepay订单id："+ orderid);
            //DateTime t1 = DateTime.Now;
            //DateTime t2 = new DateTime(1970, 1, 1);
            //double t = t1.Subtract(t2).TotalSeconds;

            string userip = Utils.GetIP();
            int terminaltype = _othertype; //终端类型，0、IMEI；1、MAC；2、UUID；3、other，如果终端类型未知的话，那么就传other
            String terminalid = _other;

            string paytypes = "1|2";//支付方式（信用卡or借记卡）;1为借记卡；2为信用卡；信用卡、借记卡都支持则传1|2；不传参视为两种方式都支持


#if Debug
            //商户提供的商户后台系统异步支付回调地址
            string callbackurl = "http://localhost:2335/Callback/fYeePay";
            //商户提供的商户前台系统异步支付回调地址
            string fcallbackurl = "http://localhost:2335/Callback/YeePay";
#endif
#if P17
            //商户提供的商户后台系统异步支付回调地址
            string callbackurl = "http://192.168.1.17:8016/Callback/fYeePay";
            //商户提供的商户前台系统异步支付回调地址
            string fcallbackurl = "http://192.168.1.17:8016/Callback/YeePay";
#endif
#if Test
            //商户提供的商户后台系统异步支付回调地址
            string callbackurl = "http://tpay.515.com/Callback/fYeePay";
            //商户提供的商户前台系统异步支付回调地址
            string fcallbackurl = "http://tpay.515.com/Callback/YeePay";
#endif
#if Release
            //商户提供的商户后台系统异步支付回调地址
            string callbackurl = "http://pay.515.com/Callback/fYeePay";
            //商户提供的商户前台系统异步支付回调地址
            string fcallbackurl = "http://pay.515.com/Callback/YeePay";
#endif



            //用户浏览器ua
            string userua = Request.UserAgent;

            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("amount", amount);
            sd.Add("currency", currency);
            sd.Add("identityid", identityid);
            sd.Add("identitytype", identitytype);
            sd.Add("orderid", orderid);
            sd.Add("orderexpdate", orderexpdate);
            sd.Add("productcatalog", productcatalog);
            sd.Add("productdesc", productdesc);
            sd.Add("productname", productname);
            sd.Add("transtime", transtime);
            sd.Add("userip", userip);
            sd.Add("terminaltype", terminaltype);
            sd.Add("terminalid", terminalid);
            sd.Add("callbackurl", callbackurl);
            sd.Add("fcallbackurl", fcallbackurl);
            sd.Add("userua", userua);
            sd.Add("paytypes", paytypes);

            //生成RSA签名
            string sign = EncryptUtil.handleRSA(sd, merchantPrivatekey);
            logsb.Append("生成订单Yeepay生成的签名为：" + sign + "\n");

            sd.Add("sign", sign);

            //将网页支付对象转换为json字符串
            string wpinfo_json = Newtonsoft.Json.JsonConvert.SerializeObject(sd);
            logsb.Append("生成订单Yeepay网页支付明文数据json格式为：" + wpinfo_json + "\n");
            string datastring = AES.Encrypt(wpinfo_json, merchantAesKey);
            logsb.Append("生成订单Yeepay网页支付业务数据经过AES加密后的值为：" + datastring + "\n");

            //将商户merchantAesKey用RSA算法加密
            logsb.Append("生成订单YeepaymerchantAesKey为：" + merchantAesKey + "\n");
            string encryptkey = GL.Pay.YeePay.RSAFromPkcs8.encryptData(merchantAesKey, yibaoPublickey, "UTF-8");
            logsb.Append("生成订单Yeepayencryptkey为：" + encryptkey + "\n");

            //打开浏览器访问一键支付网页支付链接地址，请求方式为get
            string postParams = "data=" + HttpUtility.UrlEncode(datastring) + "&encryptkey=" + HttpUtility.UrlEncode(encryptkey) + "&merchantaccount=" + merchantAccount;
            string url = apiprefix + mobilepayURI + "?" + postParams;

            logsb.Append("生成订单Yeepay网页支付链接地址为：" + url + "\n");
            logsb.Append("生成订单Yeepay网页支付链接地址长度为：" + url.Length + "\n");

            //写日志

            log.Info(string.Concat("生成订单Yeepay \n", logsb.ToString()));


            RechargeCheckBLL.Add(new RechargeCheck { Money = amount, ProductID = _productid, SerialNo = orderid, UserID = _identityid, CreateTime = (uint)transtimeL });

            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = orderid, CreateTime = DateTime.Now, ChargeType = (int)raType.易宝, OrderIP= GetClientIp() });

            //重定向跳转到易宝支付移动终端网页收银台
            Response.Redirect(url);
            return Content(userip);
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



        /// <summary>
        /// transtime   	交易时间  时间戳，例如：1361324896，精确到秒
        /// productid 	    商品ID 
        /// identityid  	用户ID
        /// othertype       终端类型  0、IMEI；1、MAC；2、UUID；3、other，如果终端类型未知的话，那么就传other
        /// other       	终端硬件标识  
        /// customSign  	MD5码。transtime+identityid+othertype+other+productid+约定字符串 计算出的MD5码  
        /// 约定字符串为:515IWOXXXeYiw89y
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult WxPay(Dictionary<string, string> queryvalues)
        {
            log.Info("##################WxPay微信支付生成订单号开始: #######################" );
            log.Info("生成订单WxPay: " + Utils.GetUrl());
          

            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            string Key = _key;

         

            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));

            if (!_customSign.Equals(md5))
            {
                log.Error("生成订单WxPay md5失败 ");
                return Content("参数错误!");
            }



            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);
            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("WxPay");
            log.Info("生成订单WxPay订单号" + appPay.out_trade_no);
            WxPayData unifiedOrderResult = appPay.GetUnifiedOrderResult();
            long transtimeL = Utils.GetTimeStampL();

            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {//如果是首冲

                //判断是否首冲过
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge {  UserID = userid});
             

                if (recharge == null)//没有首冲过
                {
                    RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                    RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.微信, OrderIP = GetClientIp() });


                    unifiedOrderResult.SetValue("ret", 0);
                    return Content(unifiedOrderResult.ToJson());
                }
                else{//首冲过

                    unifiedOrderResult.SetValue("ret", 1);
                    unifiedOrderResult.RemoveKey("prepay_id");
                 
                    return Content(unifiedOrderResult.ToJson());

                }



              
            }
            else {//不是首冲
                RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });

                unifiedOrderResult.SetValue("ret", 0);

                RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.微信, OrderIP = GetClientIp() });


                return Content(unifiedOrderResult.ToJson());
            }



         


        }



        /// <summary>
        /// transtime   	交易时间  时间戳，例如：1361324896，精确到秒
        /// productid 	    商品ID 
        /// identityid  	用户ID
        /// othertype       终端类型  0、IMEI；1、MAC；2、UUID；3、other，如果终端类型未知的话，那么就传other
        /// other       	终端硬件标识  
        /// customSign  	MD5码。transtime+identityid+othertype+other+productid+约定字符串 计算出的MD5码  
        /// 约定字符串为:515IWOXXXeYiw89y
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult AliPay(Dictionary<string, string> queryvalues)
        {
            log.Info("##################AliPay支付宝生成订单号开始: #######################");
            log.Info("生成订单AliPay支付宝: " + Utils.GetUrl());
         

            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            string Key = _key;

            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));
            if (!_customSign.Equals(md5))
            {
                log.Error(this.GetType().ToString() + "生成订单AliPay md5失败 ");
                return Content("参数错误!");
            }


            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);


            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//首冲过
                {
                    log.Error("生成订单AliPay 重复首冲 ");
                    return Content("重复首冲!");
                }


            }



            int amount = Convert.ToInt32(iap.price * 100);//支付金额为分

            string identityid = _identityid.ToString();//用户身份标识


            long transtimeL = Utils.GetTimeStampL();//交易发生时间，时间戳，例如：1361324896，精确到秒

            string orderid = Utils.GenerateOutTradeNo("Alipay");
            log.Info("生成订单AliPay 订单号: "+ orderid);
            string productdesc = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            string productname = iap.productname;


            //AppPay appPay = new AppPay();
            //appPay.total_fee = Convert.ToInt32(iap.price * 100);
            //appPay.body = iap.productname;
            //appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            //appPay.spbill_create_ip = Utils.GetIP();
            //appPay.out_trade_no = Utils.GenerateOutTradeNo("WxPay");
            //WxPayData unifiedOrderResult = appPay.GetUnifiedOrderResult();
            //long transtimeL = Utils.GetTimeStampL();
            RechargeCheckBLL.Add(new RechargeCheck { Money = amount, ProductID = _productid, SerialNo = orderid, UserID = _identityid, CreateTime = (ulong)transtimeL });
            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = orderid, CreateTime = DateTime.Now, ChargeType = (int)raType.支付宝, OrderIP = GetClientIp() });

            return Json(new
            {
                prepay_id = orderid,
                notify_url = GL.Pay.AliPay.Config.Notify_url
            }, JsonRequestBehavior.AllowGet);


        }


        /// <summary>
        /// transtime   	交易时间  时间戳，例如：1361324896，精确到秒
        /// productid 	    商品ID 
        /// identityid  	用户ID
        /// othertype       终端类型  0、IMEI；1、MAC；2、UUID；3、other，如果终端类型未知的话，那么就传other
        /// other       	终端硬件标识  
        /// customSign  	MD5码。transtime+identityid+othertype+other+productid+约定字符串 计算出的MD5码  
        /// 约定字符串为:515IWOXXXeYiw89y
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult BaiDuPay(Dictionary<string, string> queryvalues)
        {

            log.Info("##################BaiDuPay生成订单号开始: #######################");
            log.Info("BaiDuPay生成订单: " + Utils.GetUrl());
         


            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            string Key = _key;

            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));
            if (!_customSign.Equals(md5))
            {
                log.Error(this.GetType().ToString() + "参数不正确 ");
                return Content("参数错误!");
            }


            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);



            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//首冲过
                {
                    log.Error("BaiDuPay 重复首冲 ");
                    return Content("重复首冲!");
                }


            }



            int amount = Convert.ToInt32(iap.price * 100);//支付金额为分

            string identityid = _identityid.ToString();//用户身份标识


            long transtimeL = Utils.GetTimeStampL();//交易发生时间，时间戳，例如：1361324896，精确到秒

            string orderid = Utils.GenerateOutTradeNo("BaiDuPay");

            string productdesc = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            string productname = iap.productname;

            log.Info("BaiDuPay生成订单 订单号: " + orderid);
            //AppPay appPay = new AppPay();
            //appPay.total_fee = Convert.ToInt32(iap.price * 100);
            //appPay.body = iap.productname;
            //appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            //appPay.spbill_create_ip = Utils.GetIP();
            //appPay.out_trade_no = Utils.GenerateOutTradeNo("WxPay");
            //WxPayData unifiedOrderResult = appPay.GetUnifiedOrderResult();
            //long transtimeL = Utils.GetTimeStampL();
            RechargeCheckBLL.Add(new RechargeCheck { Money = amount, ProductID = _productid, SerialNo = orderid, UserID = _identityid, CreateTime = (ulong)transtimeL });
            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = orderid, CreateTime = DateTime.Now, ChargeType = (int)raType.百度, OrderIP = GetClientIp() });



#if Debug || P17 || Test
            return Json(new
            {
                prepay_id = orderid,
                notify_url = "http://tpay.515.com/callback/baidupay"
            }, JsonRequestBehavior.AllowGet);
#endif
#if Release
            return Json(new
            {
                prepay_id = orderid
            }, JsonRequestBehavior.AllowGet);
#endif

        }


        /// <summary>
        /// transtime   	交易时间  时间戳，例如：1361324896，精确到秒
        /// productid 	    商品ID 
        /// identityid  	用户ID
        /// othertype       终端类型  0、IMEI；1、MAC；2、UUID；3、other，如果终端类型未知的话，那么就传other
        /// other       	终端硬件标识  
        /// customSign  	MD5码。transtime+identityid+othertype+other+productid+约定字符串 计算出的MD5码  
        /// 约定字符串为:515IWOXXXeYiw89y
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        /// 
        [QueryValues]
        public ActionResult QQPay(Dictionary<string, string> queryvalues)
        {
            log.Info("##################QQPay生成订单号开始: #######################");

            log.Info("QQPay生成订单: " + JsonConvert.SerializeObject(queryvalues));


            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            //公共参数
            string openid = queryvalues.ContainsKey("openid") ? queryvalues["openid"] : string.Empty;
            string openkey = queryvalues.ContainsKey("openkey") ? queryvalues["openkey"] : string.Empty;
            string appid = queryvalues.ContainsKey("appid") ? queryvalues["appid"] : string.Empty;
            //string sig = queryvalues.ContainsKey("sig") ? queryvalues["sig"] : string.Empty;
            string pf = queryvalues.ContainsKey("pf") ? queryvalues["pf"] : string.Empty;

            string format = queryvalues.ContainsKey("sig") ? queryvalues["sig"] : string.Empty;
            string userip = queryvalues.ContainsKey("pf") ? queryvalues["pf"] : string.Empty;
            //私有参数

            string pfkey = queryvalues.ContainsKey("pfkey") ? queryvalues["pfkey"] : string.Empty;
            string amt = queryvalues.ContainsKey("amt") ? queryvalues["amt"] : string.Empty;
            string amttype = queryvalues.ContainsKey("amttype") ? queryvalues["amttype"] : string.Empty;
            //string ts = queryvalues.ContainsKey("ts") ? queryvalues["ts"] : string.Empty;

            string payitem = queryvalues.ContainsKey("payitem") ? queryvalues["payitem"] : string.Empty;
            string appmode = queryvalues.ContainsKey("appmode") ? queryvalues["appmode"] : string.Empty;

            string max_num = queryvalues.ContainsKey("max_num") ? queryvalues["max_num"] : string.Empty;
            string goodsmeta = queryvalues.ContainsKey("goodsmeta") ? queryvalues["goodsmeta"] : string.Empty;
            string goodsurl = queryvalues.ContainsKey("goodsurl") ? queryvalues["goodsurl"] : string.Empty;
            string zoneid = queryvalues.ContainsKey("zoneid") ? queryvalues["zoneid"] : string.Empty;
            string manyouid = queryvalues.ContainsKey("manyouid") ? queryvalues["manyouid"] : string.Empty;
            string present = queryvalues.ContainsKey("present") ? queryvalues["present"] : string.Empty;
            string paymode = queryvalues.ContainsKey("paymode") ? queryvalues["paymode"] : string.Empty;
            string cee_extend = queryvalues.ContainsKey("cee_extend") ? queryvalues["cee_extend"] : string.Empty;


            //msg.openid = GameConfig.instance.meInfo.openid;
            //msg.openkey = GameConfig.instance.meInfo.openkey;
            //msg.appid = GameConfig.instance.meInfo.appid;
            //msg.pf = GameConfig.instance.meInfo.pf;

            //msg.pfkey = GameConfig.instance.meInfo.pfkey;
            //msg.amt = obj.amt;
            //msg.payitem = StringUtil.trim(obj.payitem);
            //msg.appmode = 1;
            //msg.goodsmeta = obj.goodsmeta;
            //msg.goodsurl = "http://515webapp.sh.1251261263.clb.myqcloud.com/assets/rechange_1.jpg";

            //msg.transtime = new Date().time;
            //msg.identityid = GameConfig.instance.playerID;
            //msg.othertype = 2;
            //msg.other = Capabilities.os;
            //msg.productid = msg.payitem.split('*')[0];
            //msg.customsign = MD5(msg.appid + msg.goodsmeta + msg.payitem + msg.amt + msg.goodsurl + msg.openid + msg.openkey + msg.pfkey + msg.pf
            //    + msg.appmode + msg.transtime + msg.identityid + msg.othertype + msg.other + msg.productid);


            string Key = _key;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));

            if (!_customSign.Equals(md5))
            {
                log.Error("QQPay 参数不正确1 ");
                return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrWhiteSpace(openid) || string.IsNullOrWhiteSpace(openkey) || string.IsNullOrWhiteSpace(appid) || string.IsNullOrWhiteSpace(pf) || string.IsNullOrWhiteSpace(pfkey) || string.IsNullOrWhiteSpace(amt) || string.IsNullOrWhiteSpace(payitem) || string.IsNullOrWhiteSpace(appmode) || string.IsNullOrWhiteSpace(goodsmeta) || string.IsNullOrWhiteSpace(goodsurl))
            {
                log.Error("QQPay 参数不正确2 ");
                return Json(new { Ret = 5001, Msg = "参数不正确2" }, JsonRequestBehavior.AllowGet);
            }


            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);
            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("QQPay");
            log.Info("QQPay生成订单 订单号: " + appPay.out_trade_no);


            string productid = payitem.Split('*')[0];
            uint price = Convert.ToUInt32(payitem.Split('*')[1]);
            uint num = Convert.ToUInt32(payitem.Split('*')[2]);

            if (!productid.Equals(_productid) )
            {
                log.Error("QQPay 物品错误 "+ productid+" "+ _productid);
                return Json(new { Ret = 5002, Msg = "物品错误" }, JsonRequestBehavior.AllowGet);
            }
            if (!(iap.price*10).Equals(price))
            {
                log.Error("QQPay 价格错误 " + iap.price*10 + " " + price);
                return Json(new { Ret = 5003, Msg = "价格错误" }, JsonRequestBehavior.AllowGet);
            }


            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是) {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//没有首冲过
                {
                    log.Error("QQPay 物品错误 " + productid + " " + _productid);
                    return Json(new { Ret = 5004, Msg = "已经首冲过" }, JsonRequestBehavior.AllowGet);
                }
            }








                long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();

            string script_name = "/v3/pay/buy_goods";
            OpenApiV3 sdk = new OpenApiV3(Convert.ToInt32(appid), GL.Pay.QQPay.Config.GetAppKey(appid));

#if Release
            string server_name = "openapi.tencentyun.com";
            sdk.SetServerName(server_name);
#endif

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("openid", openid.Trim());
            param.Add("openkey", openkey.Trim());
            param.Add("appid", appid.ToString().Trim());
            //param.Add("sig", sig);
            param.Add("pf", pf.Trim());

            param.Add("pfkey", pfkey.Trim());
            //param.Add("amt", amt.Trim());
            param.Add("ts", transtimeI.ToString().Trim());
            param.Add("payitem", payitem.Replace(_productid, appPay.out_trade_no).Trim());
            //param.Add("payitem", payitem.Trim());
            param.Add("appmode", appmode.Trim());
            param.Add("goodsmeta", goodsmeta.Trim());
            param.Add("goodsurl", goodsurl.Trim());
            param.Add("zoneid", "0");

            RstArray result = sdk.Api(script_name, param, "post", "https");

            RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.腾讯, OrderIP = GetClientIp() });

            log.Info("QQPay生成订单 result.Ret: " + result.Ret);

            //log.Info("QQPay RstArray " + result.Ret + " " + result.Msg);
            if (result.Ret == 0)
            {
                return Content(result.Msg);
            }
            log.Info("QQPay生成订单 result: " + JsonConvert.SerializeObject(result));
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        




        /// <summary>
        /// 应用汇支付接口   
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult YYHPay(Dictionary<string, string> queryvalues) {

            log.Info("###############YYHPay进入应用汇支付接口 ################### " );
            log.Info("YYHPay生成订单 " + Utils.GetUrl());
            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

        
            //公共参数
            string Key = _key;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));
           

            if (!_customSign.Equals(md5))
            {
                log.Error("YYHPay md5校验失败 ");
                return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
            }

            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);
            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            var ran = new Random();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("YYHPay");
            log.Info("YYHPay生成订单 订单号" + appPay.out_trade_no);

            long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();


            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {//如果是首冲
             //判断是否首冲过
                int userid = _identityid;
                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });
                if (recharge == null)//没有首冲过
                {

                    RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                 
                    return Json(new
                    {
                        prepay_id = appPay.out_trade_no,
                        notify_url = "",
                        ret = 0
                    }, JsonRequestBehavior.AllowGet);
                    /* {"prepay_id":"ZYPay20160227165324223","notify_url":"","ret":0} */
                }
                else
                {//首冲过

                    return Json(new
                    {
                        notify_url = "",
                        ret = 1
                    }, JsonRequestBehavior.AllowGet);
                    /* {"notify_url":"","ret":1} */
                }


            }
            else
            {
                RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.应用汇, OrderIP = GetClientIp() });

           
                return Json(new
                {
                    prepay_id = appPay.out_trade_no,
                    notify_url = "",
                    ret = 0
                }, JsonRequestBehavior.AllowGet);

                /* {"prepay_id":"ZYPay20160227165324223","notify_url":"","ret":0} */
            }





           
        }




        /// <summary>
        /// 联通
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult UnicomPay(Dictionary<string, string> queryvalues)
        {
            log.Info("##################UnicomPay联通生成订单号开始: #######################");
            log.Info("UnicomPay联通生成订单: " + Utils.GetUrl());

            return Json(new { Ret = 5000, Msg = "充值方式暂停" }, JsonRequestBehavior.AllowGet);

            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            //公共参数
            string gameaccount = queryvalues.ContainsKey("gameaccount") ? queryvalues["gameaccount"] : string.Empty;
            string imei = queryvalues.ContainsKey("imei") ? queryvalues["imei"] : string.Empty;
            string macaddress = queryvalues.ContainsKey("macaddress") ? queryvalues["macaddress"] : string.Empty;
            string ipaddress = Utils.GetIP2();
            string appversion = queryvalues.ContainsKey("appversion") ? queryvalues["appversion"] : string.Empty;
            string Key = _key;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));

            if (!_customSign.Equals(md5))
            {
                log.Error("UnicomPay 参数不正确1 ");
                return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrWhiteSpace(gameaccount) || string.IsNullOrWhiteSpace(imei) || string.IsNullOrWhiteSpace(macaddress) || string.IsNullOrWhiteSpace(appversion))
            {
                log.Error("UnicomPay 参数不正确2 ");
                return Json(new { Ret = 5001, Msg = "参数不正确2" }, JsonRequestBehavior.AllowGet);
            }


            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);





            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//首冲过
                {
                    log.Error("UnicomPay 重复首冲 ");
                    return Json(new { Ret = 5002, Msg = "重复首冲" }, JsonRequestBehavior.AllowGet);
                }


            }




            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            appPay.out_trade_no = Utils.GenerateOutTradeNoForLen("UniPay", 24);

            log.Info("UnicomPay联通 生成订单 订单号: " + appPay.out_trade_no);
            long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();


            RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL, CheckInfo = string.Concat(gameaccount, ",", imei, ",", macaddress, ",", ipaddress, ",", appversion) });

            log.Error("UnicomPay 订单 ："+ appPay.out_trade_no);
            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.联通, OrderIP = GetClientIp() });

            return Json(new
            {
                prepay_id = appPay.out_trade_no,
                notify_url = ""
            }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// XY苹果助手支付接口
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult XYPay(Dictionary<string, string> queryvalues)
        {

            log.Info("##################XYPay苹果助手生成订单号开始: #######################");
            log.Info("XYPay苹果助手生成订单: " + Utils.GetUrl());
       


            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            //公共参数
            string Key = _key;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));

            if (!_customSign.Equals(md5))
            {
                log.Error("XYPay 参数不正确1 ");
                return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
            }

            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);
            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("XYPay");
         

            long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();

            log.Info("XYPay苹果助手生成订单 订单号: " + appPay.out_trade_no);

            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {//如果是首冲
                int userid = _identityid;
                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });
                if (recharge == null)
                {//没有首冲过
                    RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                    RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.XY助手, OrderIP = GetClientIp() });

                    return Json(new
                    {
                        prepay_id = appPay.out_trade_no,
                        notify_url = "",
                        ret = 0
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {//首冲过
                    return Json(new
                    {
                        notify_url = "",
                        ret = 1
                    }, JsonRequestBehavior.AllowGet);
                }    



            }
            else
            {//不是首冲
                RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.XY助手, OrderIP = GetClientIp() });
                return Json(new
                {
                    prepay_id = appPay.out_trade_no,
                    notify_url = "",
                    ret = 0
                }, JsonRequestBehavior.AllowGet);
            }




              

         


        
        }


        /// <summary>
        /// 应用宝支付接口
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult AppTreasure(Dictionary<string, string> queryvalues) {
            log.Info("#############AppTreasure : 进入应用宝支付接口#########  ");
            log.Info("AppTreasure应用宝生成订单" + Utils.GetUrl());

            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            //公共参数
            string Key = _key;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));

            if (!_customSign.Equals(md5))
            {
                log.Error("AppTreasure md5校验失败 ");
                return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
            }

            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);



            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//首冲过
                {
                    log.Error("AppTreasure 重复首冲 ");
                    return Json(new { Ret = 5001, Msg = "重复首冲" }, JsonRequestBehavior.AllowGet);
                }


            }





            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("AppTreasure");


            long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();


            RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.应用宝, OrderIP = GetClientIp() });
            log.Info("AppTreasure应用宝生成订单 订单号" + appPay.out_trade_no);
          
            return Json(new
            {
                prepay_id = appPay.out_trade_no,
                notify_url = ""
            }, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// 海马支付
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult HippocampiPay(Dictionary<string, string> queryvalues) {
            log.Info("#############HippocampiPay :海马生成订单#########  ");
            log.Info("HippocampiPay海马生成订单" + Utils.GetUrl());

            return Content("暂停使用!");

            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

            string Key = _key;


            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));

            if (!_customSign.Equals(md5))
            {
                log.Error("HippocampiPay MD5校验参数不正确 ");
                return Content("参数错误!");
            }



            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);


            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//首冲过
                {
                    log.Error("HippocampiPay 重复首冲 ");
                    return Content("重复首冲!");
                }


            }



            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("HippocampiPay");

            log.Info("HippocampiPay海马生成订单 订单号" + appPay.out_trade_no);

            long transtimeL = Utils.GetTimeStampL();
            RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });

            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.海马玩, OrderIP = GetClientIp() });

            return Json(new
            {
                prepay_id = appPay.out_trade_no,
                notify_url = ""
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 移动支付接口
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult yidongLTVPP(Dictionary<string, string> queryvalues)
        {

   
            /*
            http://tpay.515.com:80/pay/yidongD?
            other=867677023665133&
            productid=Chip_1&
            identityid=79303&
            othertype=2&
            customsign=469fe8f4cbe1d666fb0744abdf14d60c&
            transtime=1454121439?
            controller=pay&
            action=yidongD
            */
            log.Info("##################移动进入支付接口##############  ");
            log.Info("移动生成订单" + Utils.GetUrl());


            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

        
            //检测_identityid 是否被封号
            Role role =RoleBLL.GetModelByID(new Role() { ID = _identityid });
            if (role.IsFreeze == isSwitch.关) {//说明被封号了，不允许充钱
                log.Error("yidongD _identity= "+ _identityid+",被封号了，不能产生订单!");
                return Json(new { Ret = 5000, Msg = "参数不正确" }, JsonRequestBehavior.AllowGet);
            }
         

            //公共参数
            string Key = _key;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, "5g16jI4w&%4^W2gh!"));
        
            if (!_customSign.Equals(md5))
            {
                log.Error("yidongD md5校验失败 ");
                return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
            }
            //检测是否已存在此md5和此用户id
            IEnumerable<MD5Flow> ieum = RoleBLL.GetMD5Flow(new MD5Flow() { userid = _identityid, md5 = md5 });
            if (ieum != null) {
                if (ieum.Count() != 0) {
                    //说明存在此md5，不能重复使用
                    log.Error("yidongD md5不能重复使用!");
                    return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
                }
            }

            //保存生成订单的值
            RoleBLL.AddMD5Flow(new MD5Flow() { userid= _identityid, Category=(int)raType.移动, CreateTime=DateTime.Now, md5= md5 });




            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);



            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//首冲过
                {
                    log.Error("yidongD 重复首冲 ");
                    return Json(new { Ret = 5001, Msg = "重复首冲" }, JsonRequestBehavior.AllowGet);
                }


            }




            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            var ran = new Random();
            appPay.out_trade_no ="Y0"+ DateTime.Now.ToString("yyMMddHHmmss") + ran.Next(10, 99);


            long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();


            RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.移动, OrderIP = GetClientIp() });


            log.Info("移动生成订单 订单号" + appPay.out_trade_no);
            return Json(new
            {
                prepay_id = appPay.out_trade_no,
                notify_url = ""
            }, JsonRequestBehavior.AllowGet);
        }


        [QueryValues]
        public ActionResult ZYPay(Dictionary<string, string> queryvalues) {
            /*

              http://tpay.515.com:80/pay/ZYPay?other=A10000421C3BF1&productid=firstCharge_1&identityid=159258&othertype=2&customsign=48e0ed336e4d580412090fe48692cc45&transtime=1454746910
            
              http://pay.515.com:80/pay/ZYPay?other=866533021660207&productid=Chip_8&identityid=230731&othertype=2&customsign=07910c779987a915e403ffb04f015fdd&transtime=1460996227
            */

            log.Info("#####################ZYPay进入卓悠支付接口####################" );
            log.Info("ZYPay卓悠生成订单" + Utils.GetUrl());
            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

           
            //公共参数
            string Key = _key;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));
           
            if (!_customSign.Equals(md5))
            {
                log.Error("ZYPay md5校验失败 ");
                return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
            }

            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);
            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            var ran = new Random();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("ZYPay");

            log.Info("ZYPay卓悠生成订单 订单号" + appPay.out_trade_no);
            long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();


            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {//如果是首冲
             //判断是否首冲过
                int userid = _identityid;
                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });
                if (recharge == null)//没有首冲过
                {
                   
                    RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                    RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.卓悠, OrderIP = GetClientIp() });

                 
                    return Json(new
                    {
                        prepay_id = appPay.out_trade_no,
                        notify_url = "",
                        ret = 0
                    }, JsonRequestBehavior.AllowGet);
                    /* {"prepay_id":"ZYPay20160227165324223","notify_url":"","ret":0} */
                }
                else
                {//首冲过

                    return Json(new
                    {
                        notify_url = "",
                        ret = 1
                    }, JsonRequestBehavior.AllowGet);
                    /* {"notify_url":"","ret":1} */
                }


            }
            else {
                RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.卓悠, OrderIP = GetClientIp() });
             
                return Json(new
                {
                    prepay_id = appPay.out_trade_no,
                    notify_url = "",
                    ret = 0
                }, JsonRequestBehavior.AllowGet);

                /* {"prepay_id":"ZYPay20160227165324223","notify_url":"","ret":0} */
            }





        }


        /// <summary>
        /// 魅族支付接口
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult MZPay(Dictionary<string, string> queryvalues) {
            log.Info("###################MZPay魅族进入支付接口################  " );
            log.Info("MZPay魅族生成订单 " + Utils.GetUrl());

            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;//魅族的id
            int _uid = queryvalues.ContainsKey("uid") ? Convert.ToInt32(queryvalues["uid"]) : 0;//我们的id

         
            ////公共参数
            //string md5 = MeiZuSignCheck.MakeSigCreateDD(queryvalues, "Qx2Yhspw5UNJQ7FLa3ieZScYjGl6GO1b");




            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);

           

            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {
                int userid = _identityid;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });

                if (recharge != null)//首冲过
                {
                    log.Error("MZPay 重复首冲 ");
                    return Json(new MeiZuRecModel
                    {
                        code = 900000,
                        message = "重复首冲",
                        redirect = "",
                        value = null
                    }, JsonRequestBehavior.AllowGet);
                }


            }




            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            var ran = new Random();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("MZPay");


            long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();


            RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _uid, CreateTime = (uint)transtimeL });
            RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.魅族, OrderIP = GetClientIp() });
         
            log.Info("MZPay魅族生成订单 订单号" + appPay.out_trade_no);
            MeiZuModel mo = new MeiZuModel
            {
                app_id = 3061955,
                buy_amount = 1,
                cp_order_id = transtimeL,
                create_time = transtimeL,
                pay_type = 0,
                product_body = "gold",
                product_id = _productid,
                product_per_price = "1.0",
                product_subject = "",
                product_unit = "m",
                sign_type = "md5",
                total_price = iap.price.ToString(),
                uid = _identityid.ToString(),
                user_info = appPay.out_trade_no+ "."+ _uid,

            };



             mo.sign = MeiZuSignCheck.MakeSigCreateDD(mo, "Qx2Yhspw5UNJQ7FLa3ieZScYjGl6GO1b");

      

            log.Info("MZPay魅族生成订单 返回的订单实体信息 " + JsonConvert.SerializeObject(mo));



            JsonResult xx = Json(new MeiZuRecModel
            {
                code = 200,
                message = "OK",
                redirect = "",
                value = mo
            }, JsonRequestBehavior.AllowGet);


            Response.Clear();
           


            return Json(new MeiZuRecModel
            {
                code = 200,
                message = "",
                redirect = "",
                value = mo
            }, JsonRequestBehavior.AllowGet);
        }



        [QueryValues]
        public ActionResult HuaWei(Dictionary<string, string> queryvalues)
        {
          
            log.Info("#############HuaWei进入华为支付接口 ############# " );
            log.Info("HuaWei生成订单" + Utils.GetUrl());

            string _transtime = queryvalues.ContainsKey("transtime") ? queryvalues["transtime"] : string.Empty;
            string _productid = queryvalues.ContainsKey("productid") ? queryvalues["productid"] : string.Empty;
            int _identityid = queryvalues.ContainsKey("identityid") ? Convert.ToInt32(queryvalues["identityid"]) : 0;
            int _othertype = queryvalues.ContainsKey("othertype") ? Convert.ToInt32(queryvalues["othertype"]) : 0;
            string _other = queryvalues.ContainsKey("other") ? queryvalues["other"] : string.Empty;
            string _customSign = queryvalues.ContainsKey("customsign") ? queryvalues["customsign"] : string.Empty;

          
            //公共参数
            string Key = _key;
            string md5 = Utils.MD5(string.Concat(_transtime, _identityid, _othertype, _other, _productid, Key));
       
            if (!_customSign.Equals(md5))
            {
                log.Error("HuaWei md5校验失败 ");
                return Json(new { Ret = 5000, Msg = "参数不正确1" }, JsonRequestBehavior.AllowGet);
            }

            IAPProduct iap = IAPProductBLL.GetModelByID(_productid);
            AppPay appPay = new AppPay();
            appPay.total_fee = Convert.ToInt32(iap.price * 100);
            appPay.body = iap.productname;
            appPay.attach = (iap.goods + iap.attach_chip + iap.attach_5b).ToString();
            appPay.spbill_create_ip = Utils.GetIP();
            var ran = new Random();
            appPay.out_trade_no = Utils.GenerateOutTradeNo("HuaWeiPay");
            log.Info("HuaWei生成订单 订单号" + appPay.out_trade_no);

            long transtimeL = Utils.GetTimeStampL();
            long transtimeI = Utils.GetTimeStampI();


            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
            if (iF == isFirst.是)
            {//如果是首冲
             //判断是否首冲过
                int userid = _identityid;
                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });
                if (recharge == null)//没有首冲过
                {

                    RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                    RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.卓悠, OrderIP = GetClientIp() });

                 
                    return Json(new
                    {
                        prepay_id = appPay.out_trade_no,
                        notify_url = "",
                        ret = 0
                    }, JsonRequestBehavior.AllowGet);
                    /* {"prepay_id":"ZYPay20160227165324223","notify_url":"","ret":0} */
                }
                else
                {//首冲过

                    return Json(new
                    {
                        notify_url = "",
                        ret = 1
                    }, JsonRequestBehavior.AllowGet);
                    /* {"notify_url":"","ret":1} */
                }


            }
            else
            {
                RechargeCheckBLL.Add(new RechargeCheck { Money = appPay.total_fee, ProductID = _productid, SerialNo = appPay.out_trade_no, UserID = _identityid, CreateTime = (uint)transtimeL });
                RechargeCheckBLL.AddOrderIP(new UserIpInfo { UserID = _identityid, OrderID = appPay.out_trade_no, CreateTime = DateTime.Now, ChargeType = (int)raType.华为, OrderIP = GetClientIp() });
             
                return Json(new
                {
                    prepay_id = appPay.out_trade_no,
                    notify_url = "",
                    ret = 0
                }, JsonRequestBehavior.AllowGet);

                /* {"prepay_id":"ZYPay20160227165324223","notify_url":"","ret":0} */
            }





        }


    }
}