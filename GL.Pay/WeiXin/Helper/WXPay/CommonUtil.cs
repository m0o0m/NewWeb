using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;
using GL.Pay.WxPay.Consts;

namespace GL.Pay.WxPay.Helper.WXPay
{

    public class CommonUtil
    {

        public static String CreateNoncestr(int length)
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        public static String CreateNoncestr()
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < 16; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        public static string FormatQueryParaMap(Dictionary<string, string> parameters)
        {

            string buff = "";
            try
            {

                var result = from pair in parameters orderby pair.Key select pair;
                foreach (KeyValuePair<string, string> pair in result)
                {
                    if (pair.Key != "")
                    {
                        buff += pair.Key + "="
                                + System.Web.HttpUtility.UrlEncode(pair.Value) + "&";
                    }
                }
                if (buff.Length == 0 == false)
                {
                    buff = buff.Substring(0, (buff.Length - 1) - (0));
                }
            }
            catch (Exception e)
            {
                throw new SDKRuntimeException(e.Message);
            }

            return buff;
        }

        public static string FormatBizQueryParaMap(Dictionary<string, string> paraMap,
                bool urlencode)
        {

            string buff = "";
            try
            {
                var result = from pair in paraMap orderby pair.Key select pair;
                foreach (KeyValuePair<string, string> pair in result)
                {
                    if (pair.Key != "")
                    {

                        string key = pair.Key;
                        string val = pair.Value;
                        if (urlencode)
                        {
                            val = System.Web.HttpUtility.UrlEncode(val);
                        }
                        buff += key.ToLower() + "=" + val + "&";

                    }
                }

                if (buff.Length == 0 == false)
                {
                    buff = buff.Substring(0, (buff.Length - 1) - (0));
                }
            }
            catch (Exception e)
            {
                throw new SDKRuntimeException(e.Message);
            }
            return buff;
        }

        /// <summary>
        /// ͳһ֧��  ��������ϣ�
        /// </summary>
        /// <param name="paraMap"></param>
        /// <param name="urlencode"></param>
        /// <returns></returns>
        public static string FormatBizQueryParaMapForUnifiedPay(Dictionary<string, string> paraMap)
        {
            string buff = "";
            try
            {
                var result = from pair in paraMap orderby pair.Key select pair;
                foreach (KeyValuePair<string, string> pair in result)
                {
                    if (pair.Key != "")
                    {

                        string key = pair.Key;
                        string val = pair.Value;
                        buff += key + "=" + val + "&";
                    }
                }

                if (buff.Length == 0 == false)
                {
                    buff = buff.Substring(0, (buff.Length - 1) - (0));
                }
            }
            catch (Exception e)
            {
                throw new SDKRuntimeException(e.Message);
            }
            return buff;
        }

        public static bool IsNumeric(String str)
        {
            try
            {
                int.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ArrayToXml(Dictionary<string, string> arr)
        {
            String xml = "<xml>";

            foreach (KeyValuePair<string, string> pair in arr)
            {
                String key = pair.Key;
                String val = pair.Value;
                if (IsNumeric(val))
                {
                    xml += "<" + key + ">" + val + "</" + key + ">";

                }
                else
                    xml += "<" + key + "><![CDATA[" + val + "]]></" + key + ">";
            }

            xml += "</xml>";
            return xml;
        }

        /**
        * ���ݵ�ǰϵͳʱ���������������ɶ�����
         * @return ������
        */
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", WeiXinConst.PartnerId, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }

        /**
        * ����ʱ�������׼����ʱ�䣬ʱ��Ϊ����������1970��1��1�� 0��0��0������������
         * @return ʱ���
        */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * ����������������������ĸ������
        * @return �����
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

    }
}
