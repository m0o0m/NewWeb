﻿using System;
using System.Collections.Generic;
using System.Web;

namespace GL.Pay.WxPay
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public const string APPID = "wxfc0ef38c696301e9";
        public const string MCHID = "1279789801";
        public const string KEY = "c3198977325173db1f94537f05e762ba";
        public const string APPSECRET = "f2e9a4e18d1f919d29c5681a5bb841d2";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public const string SSLCERT_PASSWORD = "1233410002";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */

#if Debug
        public const string NOTIFY_URL = "http://tpay.515.com/Callback/ResultNotifyPageForWxPay";
#endif
#if P17
        public const string NOTIFY_URL = "http://tpay.515.com/Callback/ResultNotifyPageForWxPay";
#endif
#if Test
        public const string NOTIFY_URL = "http://tpay.515.com/Callback/ResultNotifyPageForWxPay";
#endif
#if Release
        public const string NOTIFY_URL = "http://pay.515.com/Callback/ResultNotifyPageForWxPay";
#endif
        //public const string NOTIFY_URL = "http://www.weixin.qq.com/wxpay/pay.php";

        public const string tokenUrl       = "https://api.weixin.qq.com/cgi-bin/token";
        public const string gateUrl      = "https://api.weixin.qq.com/pay/genprepay";
		//$this->notifyUrl	= 'https://gw.tenpay.com/gateway/simpleverifynotifyid.xml';


        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://0.0.0.0:0";
        //public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
#if Debug
        public const int REPORT_LEVENL = 1;
#endif
#if P17
        public const int REPORT_LEVENL = 1;
#endif
#if Test
        public const int REPORT_LEVENL = 1;
#endif
#if Release
        public const int REPORT_LEVENL = 0;
#endif


    }
}