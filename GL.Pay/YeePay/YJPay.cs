using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GL.Pay.YeePay
{
    public class YJPay
    {
        //商户账户编号
        public static string merchantAccount = Config.merchantAccount;

        //商户私钥（商户公钥对应的私钥）
        public static string merchantPrivatekey = Config.merchantPrivatekey;

        //易宝支付分配的公钥（进入商户后台公钥管理，报备商户的公钥后分派的字符串）
        public static string yibaoPublickey = Config.yibaoPublickey;

        //一键支付商户通用接口URL前缀
        public string apimercahntprefix = APIURLConfig.merchantPrefix;


        /// <summary>
        /// 订单单笔查询
        /// </summary>
        /// <param name="orderid">商户订单号</param>
        /// <returns></returns>
        public string queryPayOrderInfo(string orderid)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            if (orderid != null)
            {
                if (orderid.Trim() != "")
                {
                    sd.Add("orderid", orderid);
                }
            }
            string uri = APIURLConfig.queryOrderURI;

            string viewYbResult = createMerchantDataAndRequestYb(sd, uri, false);

            return viewYbResult;

        }

        /// <summary>
        /// 单笔退款接口
        /// </summary>
        /// <param name="orderid">商户退款订单号</param>
        /// <param name="origyborderid">原来易宝支付交易订单号</param>
        /// <param name="amount">退款金额（单位：分）</param>
        /// <param name="currency">币种</param>
        /// <param name="cause">退款原因</param>
        /// <returns></returns>
        public string directRefund(string orderid, string origyborderid, int amount, int currency, string cause)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("orderid", orderid);
            sd.Add("origyborderid", origyborderid);
            sd.Add("amount", amount);
            sd.Add("currency", currency);
            sd.Add("cause", cause);

            string uri = APIURLConfig.directFundURI;

            string viewYbResult = createMerchantDataAndRequestYb(sd, uri, true);

            return viewYbResult;

        }

        /// <summary>
        /// 退款单笔查询接口
        /// </summary>
        /// <param name="yborderid">退款请求号</param>
        /// <returns></returns>
        public string queryRefundOrder(string orderid)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("orderid", orderid);
            sd.Add("yborderid", "");

            string uri = APIURLConfig.queryRefundURI;

            string viewYbResult = createMerchantDataAndRequestYb(sd, uri, false);

            return viewYbResult;

        }

        /// <summary>
        /// 消费对账文件下载
        /// </summary>
        /// <param name="startdate">开始日期，如:2015-04-02</param>
        /// <param name="enddate">结束日期，如:2015-04-02</param>
        /// <returns></returns>
        public string getClearPayData(string startdate, string enddate)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("startdate", startdate);
            sd.Add("enddate", enddate);

            string uri = APIURLConfig.clearPayDataURI;

            string viewYbResult = createMerchantDataAndRequestYb2(sd, uri, false);

            return viewYbResult;

        }

        /// <summary>
        /// 退款对账文件下载
        /// </summary>
        /// <param name="startdate">开始日期，如:2014-03-01</param>
        /// <param name="enddate">结束日期，如:2014-03-01</param>
        /// <returns></returns>
        public string getClearRefundData(string startdate, string enddate)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("startdate", startdate);
            sd.Add("enddate", enddate);

            string uri = APIURLConfig.clearRefundDataURI;

            string viewYbResult = createMerchantDataAndRequestYb2(sd, uri, false);

            return viewYbResult;

        }

        /// <summary>
        /// 将请求接口中的业务明文参数加密并请求一键支付接口--商户通用接口
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="apiUri"></param>
        /// <returns></returns>
        private string createMerchantDataAndRequestYb(SortedDictionary<string, object> sd, string apiUri, bool ispost)
        {
            //随机生成商户AESkey
            string merchantAesKey = AES.GenerateAESKey();

            //生成RSA签名
            string sign = EncryptUtil.handleRSA(sd, merchantPrivatekey);
            sd.Add("sign", sign);


            //将对象转换为json字符串
            string bpinfo_json = Newtonsoft.Json.JsonConvert.SerializeObject(sd);
            string datastring = AES.Encrypt(bpinfo_json, merchantAesKey);

            //将商户merchantAesKey用RSA算法加密
            string encryptkey = RSAFromPkcs8.encryptData(merchantAesKey, yibaoPublickey, "UTF-8");

            String ybResult = "";

            if (ispost)
            {
                ybResult = YJPayUtil.payAPIRequest(apimercahntprefix + apiUri, datastring, encryptkey, true);
            }
            else
            {
                ybResult = YJPayUtil.payAPIRequest(apimercahntprefix + apiUri, datastring, encryptkey, false);
            }
            String viewYbResult = YJPayUtil.checkYbResult(ybResult);

            return viewYbResult;

        }

        /// <summary>
        /// 将请求接口中的业务明文参数加密并请求一键支付接口，单不对返回的数据进行解密，用于获取清算对账单接口--商户通用接口
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="apiUri"></param>
        /// <returns></returns>
        private string createMerchantDataAndRequestYb2(SortedDictionary<string, object> sd, string apiUri, bool ispost)
        {
            //随机生成商户AESkey
            string merchantAesKey = AES.GenerateAESKey();

            //生成RSA签名
            string sign = EncryptUtil.handleRSA(sd, merchantPrivatekey);
            sd.Add("sign", sign);


            //将对象转换为json字符串
            string bpinfo_json = Newtonsoft.Json.JsonConvert.SerializeObject(sd);
            string datastring = AES.Encrypt(bpinfo_json, merchantAesKey);

            //将商户merchantAesKey用RSA算法加密
            string encryptkey = RSAFromPkcs8.encryptData(merchantAesKey, yibaoPublickey, "UTF-8");

            String ybResult = "";

            if (ispost)
            {
                ybResult = YJPayUtil.payAPIRequest(apimercahntprefix + apiUri, datastring, encryptkey, true);
            }
            else
            {
                ybResult = YJPayUtil.payAPIRequest(apimercahntprefix + apiUri, datastring, encryptkey, false);
            }

            return YJPayUtil.checkYbClearResult(ybResult);

        }
    }
}
