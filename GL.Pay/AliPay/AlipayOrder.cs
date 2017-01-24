using Aop.Api.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GL.Pay.AliPay
{
    public  class AlipayOrder
    {
        public static Dictionary<string, string> BuildDic(string orderID,decimal price,string product_id) {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            keyValues.Add("app_id", "2015102300523054");

            //keyValues.Add("biz_content", "{\"timeout_express\":\"30m\",\"seller_id\":\"\",\"product_code\":\"QUICK_MSECURITY_PAY\",\"total_amount\":\"0.01\",\"subject\":\"1\",\"out_trade_no\":\"" + "O0O2YP0VNQZW6ZQ" + "\"}");

            keyValues.Add("biz_content", "{\"timeout_express\":\"30m\",\"seller_id\":\"\",\"product_code\":\""+ product_id + "\",\"total_amount\":\""+ price + "\",\"subject\":\"1\",\"body\":\"test\",\"out_trade_no\":\""+ orderID + "\"}");

            keyValues.Add("charset", "utf-8");
            //keyValues.Add("format", "json");
            keyValues.Add("method", "alipay.trade.app.pay");
            keyValues.Add("notify_url", "http://pay.515.com/Callback/ResultNotifyPageForAliPay2");
      

            keyValues.Add("sign_type",  "RSA");


            //keyValues.Add("timestamp", "2017-01-11 12:39:12");


            keyValues.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            keyValues.Add("version", "1.0");
            keyValues.Add("pri_key", "MIICXAIBAAKBgQC3Sz3pHsjBUFPrnWhps2ksCfdrkxw7yBRasR1iqGyq95HUG0RzZVwwE4DhBCDZb/1/hAchRC8wdJtPttVrGSGIqwRFG7x/srvKtMRcBwQsK0z87rqJxeew3jHB7RDvq8xVKAHFfd9MCyohU9W/Ngz2PeiNsd+fVLU/nsFciCdqkwIDAQABAoGAJSIUJ885wpggeEJKbeeP7gES4/NIq//Lx9fL6TnP0g8Xtw3TH0GwnHjHCk8IzKQ4igXYX+/tU3a8JDkZIXpU5YBbsMYaC+EPmComSnd62Q3xuWTRxezA8qd4WqPP/alsi7TWi77a2hZOSug6oqdeOdsXP48m5JFYiEN37v8mqEECQQDeNAGeiT0r6VXqb9xa47p2o5ZzNaOU8q0smq4E+nijQhxb20gPcfip6xj5mpS1VQK3h3ZStQ5WhKbN7ZyDlyGzAkEA0yw24gu2xcRWfB2cnkbN0TjKQOtnhWmQIcARaoPDL+ifFAbba7dUPpcRU9uXDrC3ZjJNS1YSHcVcaCnfFCFjoQJBAKpgQBmaa3AfEwSWTuTWFqRfXL3sFAjiZsx7shEZKKUtzObV5ZQKNLu9C0JgN8Qucc3drWlPcLYAMpJVrhvsJycCQDrlK/FjXvhNR+mZwKKMEL73XcE5ZkfZJy+ih7jzQq7L7AID35JtMPu72kNPDRQ1yRChmtkWCjtvXdRXSTYuEcECQCAAReniCPhoDfpGcBGHklRS4MgqPaeeCjaSW7Pom/NOSPnjwWt1jsMxapymLSyQzCglkJSTO0H+3ewTqHvYNh0=");


            return keyValues;

        }


        public static string GetOrderParamCode(Dictionary<string, string> map)
        {


            //string query_string2 = "app_id=2015102300523054&biz_content=%7B%22timeout_express%22%3A%2230m%22%2C%22seller_id%22%3A%22%22%2C%22product_code%22%3A%22QUICK_MSECURITY_PAY%22%2C%22total_amount%22%3A%220.01%22%2C%22subject%22%3A%221%22%2C%22body%22%3A%22test%22%2C%22out_trade_no%22%3A%22YWWK33AC32YIIAI%22%7D&charset=utf-8&method=alipay.trade.app.pay&sign_type=RSA&timestamp=2017-01-10%2019%3A42%3A32&version=1.0";

            //return query_string2;
            // key排序
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>(map);
            myList.Sort(delegate (KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
            {
                return s1.Key.CompareTo(s2.Key);
            });
            string query_string = "";
            foreach (KeyValuePair<string, string> pair in myList)
            {
                if (pair.Key == "pri_key")
                {
                    continue;
                }
                if (string.IsNullOrEmpty(pair.Value))
                {
                    query_string = query_string + pair.Key + "=&";
                }
                else
                {
                    //UrlEncode(value, Encoding.UTF8)
                    //query_string = query_string + pair.Key + "=" + HttpUtility.UrlEncode(pair.Value, Encoding.UTF8) + "&";
                    query_string = query_string + pair.Key + "=" + UrlEncode(pair.Value, Encoding.UTF8) + "&";

                    // query_string = query_string + pair.Key + "=" + pair.Value + "&";
                }
              
            }
            query_string = query_string.Substring(0, query_string.Length - 1);

            return query_string;
        }

        public static string GetOrderParam(Dictionary<string, string> map) {

         
            //string query_string2 = "app_id=2015102300523054&biz_content=%7B%22timeout_express%22%3A%2230m%22%2C%22seller_id%22%3A%22%22%2C%22product_code%22%3A%22QUICK_MSECURITY_PAY%22%2C%22total_amount%22%3A%220.01%22%2C%22subject%22%3A%221%22%2C%22body%22%3A%22test%22%2C%22out_trade_no%22%3A%22YWWK33AC32YIIAI%22%7D&charset=utf-8&method=alipay.trade.app.pay&sign_type=RSA&timestamp=2017-01-10%2019%3A42%3A32&version=1.0";

            //return query_string2;
            // key排序
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>(map);
            myList.Sort(delegate (KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
            {
                return s1.Key.CompareTo(s2.Key);
            });
            string query_string = "";
            foreach (KeyValuePair<string, string> pair in myList)
            {
                if (pair.Key == "pri_key")
                {
                    continue;
                }
                if (string.IsNullOrEmpty(pair.Value))
                {
                    query_string = query_string + pair.Key + "=&";
                }
                else
                {
                    //UrlEncode(value, Encoding.UTF8)
                    //query_string = query_string + pair.Key + "=" + HttpUtility.UrlEncode(pair.Value, Encoding.UTF8) + "&";
                //  query_string = query_string + pair.Key + "=" + UrlEncode(pair.Value, Encoding.UTF8) + "&";

                    query_string = query_string + pair.Key + "=" + pair.Value + "&";
                }

            }
            query_string = query_string.Substring(0, query_string.Length - 1);

            return query_string;
        }



        static public string UrlEncodeUper(string temp)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i].ToString();
                string k;
                k = HttpUtility.UrlEncode(t, Encoding.UTF8);
                if (t == k)
                {

                    builder.Append(t);
                }
                else
                {
                    builder.Append(k.ToUpper());
                }
            }
            return builder.ToString();
        }


        static public string UrlEncode(string temp, Encoding encoding)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i].ToString();
                string k;
                switch (t)
                {
                    case "{":
                        t = "%7B";
                        builder.Append(t);
                        break;

                    case " ":
                        t = "%20";
                        builder.Append(t);
                        break;
                    //case " ":
                    //    t = "+";
                    //    builder.Append(t);
                    //    break;
                    case ":":
                        t = "%3A";
                        builder.Append(t);
                        break;

                    case ",":
                        t = "%2C";
                        builder.Append(t);
                        break;

                    case "}":
                        t = "%7D";
                        builder.Append(t);
                        break;

                    case "*":
                        t = "%2A";
                        builder.Append(t);
                        break;

                    default:
                        k = HttpUtility.UrlEncode(t, encoding);
                        if (t == k)
                        {
                          
                            builder.Append(t);
                        }
                        else
                        {
                            builder.Append(k.ToUpper());
                        }
                        break;
                }
            }
            return builder.ToString();
        }



        public static String GetSign(Dictionary<String, String> map)
        {

             string query_string = GetOrderParam(map);
            //string query_string = "app_id=2015052600090779&biz_content={\"timeout_express\":\"30m\",\"seller_id\":\"\",\"product_code\":\"QUICK_MSECURITY_PAY\",\"total_amount\":\"0.01\",\"subject\":\"1\",\"body\":\"test2\",\"out_trade_no\":\"IQJZSRC1YMQB5HU\"}&charset=utf-8&format=json&method=alipay.trade.app.pay&notify_url=http://domain.merchant.com/payment_notify&sign_type=RSA2&timestamp=2017-1-10 17:58:01&version=1.0";
            // string s = RSAFromPkcs8.sign(query_string, map["pri_key"], "UTF-8");

            //   string query_string = AlipaySignature.GetSignContent(map);


          

            string sign = AlipaySignature.RSASignCharSet(query_string, map["pri_key"],  "UTF-8",false, "RSA");

           
            bool isd = AlipaySignature.RSACheckContent(query_string, sign, "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC3Sz3pHsjBUFPrnWhps2ksCfdrkxw7yBRasR1iqGyq95HUG0RzZVwwE4DhBCDZb/1/hAchRC8wdJtPttVrGSGIqwRFG7x/srvKtMRcBwQsK0z87rqJxeew3jHB7RDvq8xVKAHFfd9MCyohU9W/Ngz2PeiNsd+fVLU/nsFciCdqkwIDAQAB", "UTF-8", "RSA", false);

            


              return sign;
        }


    }
}
