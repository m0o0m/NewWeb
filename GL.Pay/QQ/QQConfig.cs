using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace GL.Pay.QQPay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.3
    /// 日期：2012-07-05
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段
        private static string appid = "";
        private static string appkey = "";
        private static Dictionary<string,string> appkeyList;
        private static string input_charset = "";
        private static string sign_type = "";
        private static string notify_url = "";

        #endregion

        static Config()
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

#if Debug
            appid = "1104965754";
            appkey = "4ftSvBSlY7BnYc59&";

            //appid = "1104953359";
            //appkey = "UGDnnFYX15itqzPD&";


#endif
#if P17
            appid = "1104965754";
            appkey = "4ftSvBSlY7BnYc59&";


#endif
#if Test
            appid = "1104965754";
            appkey = "4ftSvBSlY7BnYc59&";
#endif
#if Release
            appid = "1103881749";
            appkey = "thJ4XbABcGOhi5Kr&";
#endif

            appkeyList = new Dictionary<string, string>();
            appkeyList.Add("1104965754", "4ftSvBSlY7BnYc59");
            appkeyList.Add("1104953359", "UGDnnFYX15itqzPD");
            appkeyList.Add("1103881749", "thJ4XbABcGOhi5Kr");
            appkeyList.Add("1105069831", "Tt2TTzG7AjvocxLE");
            appkeyList.Add("1105006651", "iSsYpcaCzCBXsin2SHWlU9Tuahjg7Ber");
            //55Wa78ecH3S0ZcK2
          //  appkeyList.Add("1105006651", "55Wa78ecH3S0ZcK2");
            //商户的私钥

            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";

            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "RSA";
        }

        public static string GetAppKey(string AppKey)
        {

            return appkeyList.ContainsKey(AppKey) ? appkeyList[AppKey] : appkey;
        }

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string AppID
        {
            get { return appid; }
            set { appid = value; }
        }

        /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public static string AppKey
        {
            get { return appkey; }
            set { appkey = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string Sign_type
        {
            get { return sign_type; }
        }

        /// <summary>
        /// 获取回调URL
        /// </summary>
        public static string Notify_url
        {
            get { return notify_url; }
        }

        #endregion
    }
}