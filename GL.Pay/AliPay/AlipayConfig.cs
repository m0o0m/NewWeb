using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace GL.Pay.AliPay
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
        private static string partner = "";
        private static string private_key = "";
        private static string public_key = "";
        private static string input_charset = "";
        private static string sign_type = "";
        private static string notify_url = "";

        #endregion

        static Config()
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            partner = "2088121009887361";

            //商户的私钥
            private_key = @"MIICXgIBAAKBgQDK8RVNS63B1wRB7RnXs0iK/gXHVSndPT/uhyvsWq7rJlVw+i0eAn0YoTJYkHakHYV+gTt2sgIbsckvIOi4q52v2V+QhA3FAnwFFN3cz/cNbIHuBO8UvYVGDCV824Z4nlSrka/t3NbXP+nOXBPuPuFNTX/Biilsyn/88RLX1s7j+QIDAQABAoGBAKCDHCEduVm2cfSezrDPaZIdpn5peoo1FqrXmMMBWpY8pJmOFj9FIqJnZMWtxVi6zMoo9tpDou06qfAvrEHb4wQ3AxAe8OalecEowBpZfbIdzG0RLdbPgo6qjVp5MPfdQtplDiwWmj3w9ExuDcZMKlWD7kVj2iNiAnA8rQmTXYS9AkEA/+yoNUmkLjyUQtcDKIA4nyG+HDzHrVf+IUKtKDfWkxptTcrwf1lxgPNHnzPHYpEg9ArgW1IbfCYcHq97wAgJVwJBAMsAa/MyWm7LSHz0nakGTIPa13P4OpNjQXE5imHlCEtobp2yqID5HYaSOTAV8B6sCRhozEB/2yzoIffhuenPGy8CQDPo37F3T2rldLkd/ZEAePumD3aQyXuahnxeltq6gLZzvMnzqNZb4Q7jyZo8GWwfvcxiTGGZhOOoozmCJJqtxp8CQQCMqtUi56B8p8aqN8NMTb4VnD6x8K9+VEgT0mCO52sw/cOw6Q0UmgsYphiUto72YhXHYU8v3qkc3Bo1r6fpWYcnAkEAvOqnceHFgWWTp0D/5b9cRW5sd/GxNbS+HMECpSJCSN3rowQ5h+cIhrUm2dj6wdG7kAWxe8v93O9ImgNgQwAceQ==";

            ///支付宝的公钥
            ///合作伙伴密钥管理
            public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";


            ///开放平台密钥管理
            ///public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDDI6d306Q8fIfCOaTXyiUeJHkrIvYISRcc73s3vF1ZT7XN8RNPwJxo8pWaJMmvyTn9N4HQ632qJBVHf8sxHi/fEsraprwCtzvzQETrNRwVxLO5jVmRGi60j8Ue1efIlzPXV9je9mkjzOmdssymZkh2QhUrCmZYI/FCEa3/cNMW0QIDAQAB";

            //无线产品密钥管理
            //public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCIqMGpcgxIXDknGeQEsOQJvKYze7AWuOU0MYRNWTUs4IHzNqqN2bf5UREhgq3TqQmqLDBxbuhNq5kjjKBWy5Cl8ICT7sfINZFLasm57t971kotnFeC6bZndn9tI6cmjsNGHMGDhE/DtFFx3NhPeJ6U3VMR91mqcIPmsWDZA22TEwIDAQAB";

            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            //ar718ei0veuu8m6zgys4hm46gb4f92hr 
            //回调页面
#if Debug
            notify_url = "http://tpay.515.com/Callback/ResultNotifyPageForAliPay";
#endif
#if P17
             notify_url = "http://tpay.515.com/Callback/ResultNotifyPageForAliPay";
#endif
#if Test
             notify_url = "http://tpay.515.com/Callback/ResultNotifyPageForAliPay";
#endif
#if Release
             notify_url = "http://pay.515.com/Callback/ResultNotifyPageForAliPay";
#endif
            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";

            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "RSA";
        }

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public static string Private_key
        {
            get { return private_key; }
            set { private_key = value; }
        }

        /// <summary>
        /// 获取或设置支付宝的公钥
        /// </summary>
        public static string Public_key
        {
            get { return public_key; }
            set { public_key = value; }
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