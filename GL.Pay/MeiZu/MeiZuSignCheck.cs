using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
namespace GL.Pay.MeiZu
{
    public class MeiZuSignCheck
    {


        public MeiZuSignCheck()
        {

        }

        /// <summary>
        ///   生成签名
        /// </summary>
        /// <param name="method">请求方法 get/post </param>
        /// <param name="url_path">url_path</param>
        /// <param name="param">表单参数</param>
        /// <param name="secret">密钥</param>
        /// <returns>返回签名结果</returns>
        //
        static public string MakeSig(Dictionary<string, string> param, string secret)
        {
            string mk = MakeSource(param, secret);
            ILog log = LogManager.GetLogger("Callback");
            log.Info(" 魅族md5校验第3步：获得编码后的url组合参数:" + mk);
            //使用SHA1的HMAC
            //HMAC hmac = HMACSHA1.Create();
            //hmac.Key = Encoding.UTF8.GetBytes(secret);
            //byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mk));
            ////转为base64编码
            //string my_sign = Convert.ToBase64String(hash);


            string my_sign = CalculateMD5Hash(mk);
            log.Info(" 魅族md5校验第四步：新的md5编码" + my_sign);
            return my_sign;
        }

        static public string MakeSigCreateDD(MeiZuModel model, string secret)
        {
            string mk = MakeSourceDD(model, secret);
            ILog log = LogManager.GetLogger("Callback");
            log.Info(" 魅族生成md5第1步：获得编码后的url组合参数:" + mk);
            //使用SHA1的HMAC
            //HMAC hmac = HMACSHA1.Create();
            //hmac.Key = Encoding.UTF8.GetBytes(secret);
            //byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mk));
            ////转为base64编码
            //string my_sign = Convert.ToBase64String(hash);


            string my_sign = CalculateMD5Hash(mk);
            log.Info(" 魅族生成md5第2步：新的md5编码" + my_sign);
            return my_sign;
        }




        static public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }


        static private string MakeSource(Dictionary<string, string> param, string secret)
        {

            string query_string = "";
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>(param);
            //myList.Sort(delegate (KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
            //{
            //    return s1.Key.CompareTo(s2.Key);
            //});
            myList = myList.OrderBy(m => m.Key).ToList();
            foreach (KeyValuePair<string, string> pair in myList)
            {
                if (String.IsNullOrEmpty(pair.Value)) {
                    continue;
                }
                if (pair.Key == "sign" || pair.Key == "sign_type") {
                    continue;
                }
                // System.Text.Encoding.UTF8.GetString()
                query_string = query_string + pair.Key.Trim() + "=" +pair.Value.Trim() + "&";
            }
            query_string = query_string.Substring(0, query_string.Length - 1);
            query_string += (":"+secret);
          
            return query_string;
        }




        static private string GetUTF8S(string s)
        {
            byte[] bs = Encoding.UTF8.GetBytes(s);

            //转为base64编码
            string my_bs = Encoding.UTF8.GetString(bs);

            return my_bs;
        }


        static private string MakeSourceDD(MeiZuModel model, string secret)
        {


            string query_string = "app_id=" + model.app_id + "&buy_amount=" + model.buy_amount + "&cp_order_id=" + model.cp_order_id + "&create_time=" + model.create_time + "&pay_type=" + model.pay_type + "&product_body=" + GetUTF8S(model.product_body) + "&product_id=" + GetUTF8S(model.product_id) + "&product_per_price=" + GetUTF8S(model.product_per_price) + "&product_subject=" + GetUTF8S(model.product_subject) + "&product_unit=" + GetUTF8S(model.product_unit) + "&total_price=" + GetUTF8S(model.total_price) + "&uid=" + GetUTF8S(model.uid) + "&user_info=" + GetUTF8S(model.user_info) + ":" + secret;



            //            string query_string = @"app_id="+ model.app_id + @"&buy_amount="+model.buy_amount+@"&cp_order_id="+model.cp_order_id+@"&create_time="+model.create_time+@"
            //&pay_type="+model.pay_type+@"&product_body="+ GetUTF8S(model.product_body)+@"&product_id="+ GetUTF8S(model.product_id)+
            //@"&product_per_price="+ GetUTF8S(model.product_per_price)+@"&product_subject="+ GetUTF8S(model.product_subject)+@"&product_unit="+ GetUTF8S(model.product_unit)+@"
            //&total_price="+ GetUTF8S(model.total_price)+@"&uid="+ GetUTF8S(model.uid)+@"&user_info="+ GetUTF8S(model.user_info)+":"+ secret;

            return query_string;
        }


        /// <summary>
        ///验证回调发货URL的签名 (注意和普通的OpenAPI签名算法不一样, 详见
        ///http://wiki.open.qq.com/wiki/%E5%9B%9E%E8%B0%83%E5%8F%91%E8%B4%A7URL%E7%9A%84%E5%8D%8F%E8%AE%AE%E8%AF%B4%E6%98%8E_V3
        /// </summary>
        /// <param name="method">请求方法 get/post</param>
        /// <param name="url_path">url_path</param>
        /// <param name="param">腾讯调用发货回调URL携带的请求参数</param>
        /// <param name="secret">密钥</param>
        /// <param name="sig">腾讯调用发货回调URL时传递的签名</param>
        /// <returns>签名校验结果</returns>

        static public bool VerifySig(Dictionary<string, string> param, string secret)
        {
            ILog log = LogManager.GetLogger("Callback");
            log.Info(" 魅族md5校验第一步：检测参数:" + JsonConvert.SerializeObject(param));

            string sign = param["sign"];
            if (param.ContainsKey("sign"))
            {
                param.Remove("sign");
            }

            log.Info(" 魅族md5校验第二步：检测过滤后的参数:" + JsonConvert.SerializeObject(param));
            //Dictionary<string, string> new_param = new Dictionary<string, string>();

            // 先使用专用的编码规则对value编码
            //foreach (string key in param.Keys)
            //{
            //    new_param.Add(key, EncodeValue(param[key]));
            //}

            // 再计算签名
            string sig_new = MakeSig(param, secret);
            log.Info(" 魅族md5校验最后一步：对比签名,传过来签名：" + sign + " ;新生成签名: " + sig_new);
            return sig_new.Equals(sign, StringComparison.CurrentCultureIgnoreCase);
        }




        /// <summary>
        ///回调发货URL专用的编码算法
        ///编码规则为：除了 0~9 a~z A~Z !*()之外其他字符按其ASCII码的十六进制加%进行表示，例如"-"编码为"%2D"
        ///详见 http://wiki.open.qq.com/wiki/%E5%9B%9E%E8%B0%83%E5%8F%91%E8%B4%A7URL%E7%9A%84%E5%8D%8F%E8%AE%AE%E8%AF%B4%E6%98%8E_V3
        /// </summary>
        /// <param name="value">待编码的字符串</param>
        /// <returns>返回编码结果</returns>
        static public string EncodeValue(string value)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                if (Regex.IsMatch(value[i].ToString(), @"[a-zA-Z0-9!()*]"))
                {
                    builder.Append(value[i].ToString());
                }
                else
                {
                    //byte[] bt = new byte[1];
                    byte[] bt = Encoding.UTF8.GetBytes(value[i].ToString());//中文的话，一个汉字有三个字节
                    for (int k = 0; k < bt.Length; k++)
                    {
                        int ascii = (short)(bt[k]);//计算每个字节的ascii码值
                        builder.Append("%" + Convert.ToString(ascii, 16).ToUpper());
                    }

                }
            }
            return builder.ToString();
        }



    }
}
