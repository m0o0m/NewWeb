using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GL.Pay.YeePay
{
    /// <summary>
    /// 一键支付工具类
    /// </summary>

    public class YJPayUtil
    {
        //商户账户编号
        public static string merchantAccount = Config.merchantAccount;

        //商户私钥（商户公钥对应的私钥）
        public static string merchantPrivatekey = Config.merchantPrivatekey;

        //易宝支付分配的公钥（进入商户后台公钥管理，报备商户的公钥后分派的字符串）
        public static string yibaoPublickey = Config.yibaoPublickey;

        /// <summary>
        /// 请求一键支付接口
        /// </summary>
        /// <param name="requestURL">完整的URL</param>
        /// <param name="data">加密后的业务数据</param>
        /// <param name="encryptkey">随机生成的AESKey</param>
        /// <param name="merchantaccount">商户号</param>
        /// <param name="post">是否发生post请求</param>
        /// <returns></returns>
        public static string payAPIRequest(string requestURL, string datastring,
            string encryptkey, bool post)
        {

            string yibaoPublickey = Config.yibaoPublickey;

            string postParams = "data=" + HttpUtil.UrlEncode(datastring) + "&encryptkey=" + HttpUtil.UrlEncode(encryptkey) + "&merchantaccount=" + merchantAccount;
            string responseStr = "";
            if (post)
            {
                responseStr = HttpUtil.HttpPost(requestURL, postParams);
            }
            else
            {
                responseStr = HttpUtil.HttpGet(requestURL, postParams);
            }
            return responseStr;
        }

        /// <summary>
        /// 将一键支付返回的结果进行解析，支付结果回调解析一样可以调用该方法
        /// </summary>
        /// <param name="ybResult">易宝支付返回的结果</param>
        /// <returns></returns>
        public static string checkYbResult(String ybResult)
        {
            if (ybResult.IndexOf("error") < 0)
            {
                //将支付结果json字符串反序列化为对象
                RespondJson respJson = Newtonsoft.Json.JsonConvert.DeserializeObject<RespondJson>(ybResult);
                string yb_encryptkey = respJson.encryptkey;
                string yb_data = respJson.data;
                //将易宝返回的结果进行验签名
                bool passSign = EncryptUtil.checkDecryptAndSign(yb_data, yb_encryptkey, yibaoPublickey, merchantPrivatekey);
                if (passSign)
                {
                    string yb_aeskey = RSAFromPkcs8.decryptData(yb_encryptkey, merchantPrivatekey, "UTF-8");

                    string payresult_view = AES.Decrypt(yb_data, yb_aeskey);
                    return payresult_view;
                }
                else
                {
                    return "验签未通过";
                }
            }
            else
            {
                return ybResult;
            }
        }

        /// <summary>
        /// 将一键支付清算接口返回的结果进行解析，支付结果回调解析一样可以调用该方法
        /// </summary>
        /// <param name="ybResult">易宝支付返回的结果</param>
        /// <returns></returns>
        public static string checkYbClearResult(String ybResult)
        {
            if (ybResult.IndexOf("data") >= 0)
            {
                //将支付结果json字符串反序列化为对象
                RespondJson respJson = Newtonsoft.Json.JsonConvert.DeserializeObject<RespondJson>(ybResult);
                string yb_encryptkey = respJson.encryptkey;
                string yb_data = respJson.data;
                //将易宝返回的结果进行验签名
                bool passSign = EncryptUtil.checkDecryptAndSign(yb_data, yb_encryptkey, yibaoPublickey, merchantPrivatekey);
                if (passSign)
                {
                    string yb_aeskey = RSAFromPkcs8.decryptData(yb_encryptkey, merchantPrivatekey, "UTF-8");

                    string payresult_view = AES.Decrypt(yb_data, yb_aeskey);
                    return payresult_view;
                }
                else
                {
                    return "验签未通过";
                }
            }
            else
            {
                return ybResult;
            }
        }
        /// <summary>
        /// 解密易宝支付回调结果
        /// </summary>
        /// <param name="data">易宝支付回调的业务数据密文</param>
        /// <param name="encryptkey">易宝支付回调密钥密文</param>
        /// <returns></returns>
        public static string checkYbCallbackResult(string data, string encryptkey)
        {
            string yb_encryptkey = encryptkey;
            string yb_data = data;
            //将易宝返回的结果进行验签名
            bool passSign = EncryptUtil.checkDecryptAndSign(yb_data, yb_encryptkey, yibaoPublickey, merchantPrivatekey);
            if (passSign)
            {
                string yb_aeskey = RSAFromPkcs8.decryptData(yb_encryptkey, merchantPrivatekey, "UTF-8");

                string payresult_view = AES.Decrypt(yb_data, yb_aeskey);
                //返回易宝支付回调的业务数据明文
                return payresult_view;
            }
            else
            {
                return "验签未通过";
            }

        }


    }
}
