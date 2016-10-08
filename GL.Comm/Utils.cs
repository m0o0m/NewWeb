using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace GL.Common
{
    public class Utils
    {

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str ?? "");
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret;
        }
        /// <summary>
        /// 返回完整Url
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.OriginalString;
        }
        /// <summary>
        /// 返回完整Url
        /// </summary>
        /// <returns></returns>
        public static string GetUrlReferrer()
        {
            return HttpContext.Current.Request.UrlReferrer.PathAndQuery;
        }

        /// <summary>
        /// 返回Url的Path部分
        /// </summary>
        /// <returns></returns>
        public static string GetUrlPath()
        {
            return HttpContext.Current.Request.Url.AbsolutePath;
        }

        /// <summary>
        /// MD5函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string HashMD5(string str)
        {
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create("MD5");
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
            return data.ToString();  //返回长度为32字节的字符串
        }


        /// <summary>
        /// SHA1函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA1(string str)
        {
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create("Sha1");
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(str));
            return data.ToString();   //返回长度为40字节的字符串

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(str ?? "", "SHA1");  //返回长度为40字节的字符串
        }

        /// <summary>
        /// FormUrl解析器
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns>QueryValues数据字典</returns>
        public static Dictionary<string, string> GetDicFormUrl(string queryvalues)
        {
            var sp = queryvalues.Split('/');

            Dictionary<string, string> spl = new Dictionary<string, string>();
            int len = sp.Length;
            if (len % 2 > 0)
            {
                len = len - 1;
            }
            for (int i = 0; i < len; i = i + 2)
            {
                spl.Add(sp[i], sp[i + 1]);
            }
            return spl;
        }



        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            return Validator.IsNumeric(Expression);
        }

        public static bool IsNum(string value)
        {
            //判断value是否为数字,可正,可负
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public static bool IsDouble(object Expression)
        {
            return Validator.IsDouble(Expression);
        }



        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {

            //string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //if (string.IsNullOrEmpty(result))
            //    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];

            //if (string.IsNullOrEmpty(result))
            //    result = HttpContext.Current.Request.UserHostAddress;

            //if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
            //    return "127.0.0.1";

            //return result;
            try
            {
                string result = string.Empty;
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
                else
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
                    return "127.0.0.1";

                return result;
            }
            catch {
                return "0.0.0.0";
            }
        }

        public static string GetIP2()
        {
            var result = GetIP();
            var aa = string.Empty;
            foreach (var item in result.Split(new char[] { ',' }))
            { 
                var bb = ("00" + item);
                aa = aa + bb.Substring(bb.Length-3, 3);
            }


            return aa;
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static bool IsUrl(string v)
        {
            return Regex.IsMatch(v, @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?");
            //@"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?"
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64(ts.TotalSeconds).ToString();

            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds.ToString();
        }

        public static int GetTimeStampI()
        {
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64(ts.TotalSeconds).ToString();

            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }

        public static long GetTimeStampL()
        {
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64(ts.TotalSeconds).ToString();

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = DateTime.Now;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }

        /**
        * 根据当前系统时间加随机序列来生成订单号
         * @return 订单号
        */
        public static string GenerateOutTradeNo(string Key)
        {
            var ran = new Random();
            //return string.Format("{0}{1}{2}", WxPayConfig.MCHID, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
            return string.Format("{0}{1}{2}", Key, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }

        /**
        * 根据当前系统时间加随机序列来生成订单号
         * @return 订单号
        */
        public static string GenerateOutTradeNoForLen(string Key, int len)
        {
            var ran = new Random();
            var aa = string.Format("{0}{1}{2}", Key, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
            var newkey = Key;


            if (aa.Length <　len)
            {
                var ii = len - aa.Length;
                for (int i = 0; i < ii; i++)
                {
                    newkey = newkey + "0";
                }
                aa = aa.Replace(Key, newkey);
            }


            if (len < aa.Length)
            {
                var ii = aa.Length - len;
                for (int i = 0; i < ii; i++)
                {
                    newkey = newkey.Remove(newkey.Length-1, 1);
                }
                aa = aa.Replace(Key, newkey);
            }

            return aa;
        }

        /**
        * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
         * @return 时间戳
        */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static long GetTimeStampL(DateTime nowTime)
        {
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64(ts.TotalSeconds).ToString();

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //DateTime nowTime = DateTime.Now;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }


        public static DateTime GetTimeStampToTime(int time)
        {
            //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //return Convert.ToInt64(ts.TotalSeconds).ToString();
            //return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddTicks(time);


            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //long lTime = long.Parse(timeStamp + "0000000");
            long lTime = time * 10000000;
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);

            //return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }


        public static string ServerPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }


        public static bool isValidFileContent(string filePath1, string filePath2)
        {
            //创建一个哈希算法对象 
            //using (HashAlgorithm hash = HashAlgorithm.Create())
            //{
            //    using (FileStream file1 = new FileStream(filePath1, FileMode.Open), file2 = new FileStream(filePath2, FileMode.Open))
            //    {
            //        byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 
            //        byte[] hashByte2 = hash.ComputeHash(file2);
            //        string str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串 
            //        string str2 = BitConverter.ToString(hashByte2);
            //        return (str1 == str2);//比较哈希码 
            //    }
            //}
            string str1 = HashFile(filePath1); 
            string str2 = HashFile(filePath2);
            return (str1 == str2);

        }

        /// <summary>
        /// 计算文件的 MD5 值
        /// </summary>
        /// <param name="fileName">要计算 MD5 值的文件名和路径</param>
        /// <returns>MD5 值16进制字符串</returns>
        public static string MD5File(string fileName)
        {
            return HashFile(fileName, "md5");
        }

        /// <summary>
        /// 计算文件的 sha1 值
        /// </summary>
        /// <param name="fileName">要计算 sha1 值的文件名和路径</param>
        /// <returns>sha1 值16进制字符串</returns>
        public static string SHA1File(string fileName)
        {
            return HashFile(fileName, "sha1");
        }

        /// <summary>
        /// 计算文件的哈希值
        /// </summary>
        /// <param name="fileName">要计算哈希值的文件名和路径</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值16进制字符串</returns>
        private static string HashFile(string fileName, string algName = "md5")
        {
            if (!System.IO.File.Exists(fileName))
                return string.Empty;
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] hashBytes = HashData(fs, algName);
            fs.Close();
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        private static byte[] HashData(System.IO.Stream stream, string algName)
        {
            System.Security.Cryptography.HashAlgorithm algorithm;
            if (algName == null)
            {
                throw new ArgumentNullException("algName 不能为 null");
            }
            if (string.Compare(algName, "sha1", true) == 0)
            {
                algorithm = System.Security.Cryptography.SHA1.Create();
            }
            else
            {
                if (string.Compare(algName, "md5", true) != 0)
                {
                    throw new Exception("algName 只能使用 sha1 或 md5");
                }
                algorithm = System.Security.Cryptography.MD5.Create();
            }
            return algorithm.ComputeHash(stream);
        }

        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        private static string ByteArrayToHexString(byte[] buf)
        {
            return BitConverter.ToString(buf).Replace("-", "");
        }






















    }


    public class Converter
    {
        /// <summary>
        /// 转换整形数据网络次序的字节数组
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] IntToBytes(uint i)
        {
            byte[] t = BitConverter.GetBytes(i);
            byte b = t[0];
            t[0] = t[3];
            t[3] = b;
            b = t[1];
            t[1] = t[2];
            t[2] = b;
            return (t);
        }
        public static byte[] Int16ToBytes(ushort i)
        {
            byte[] t = BitConverter.GetBytes(i);
            //byte b = t[0];
            //t[0] = t[1];
            //t[1] = b;
            return (t);
        }


        public static byte[] IntToBytes(uint source, int number)
        {
            byte[] t = new byte[number];
            t = BitConverter.GetBytes(source);
            byte temp;
            for (int i = t.Length - 1; i > t.Length / 2; i--)
            {
                temp = t[i];
                t[i] = t[t.Length - 1 - i];
                t[t.Length - 1 - i] = temp;
            }
            return (t);
        }

        /// <summary>
        /// 返回字节数组代表的整数数字，4个数组
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static uint BytesToUInt(byte[] bs, int startIndex)
        {
            byte[] t = new byte[4];
            for (int i = 0; i < 4 && i < bs.Length - startIndex; i++)
            {
                t[i] = bs[startIndex + i];
            }

            byte b = t[0];
            t[0] = t[3];
            t[3] = b;
            b = t[1];
            t[1] = t[2];
            t[2] = b;

            return BitConverter.ToUInt32(t, 0);
        }


        public static ushort BytesToUInt16(byte[] bs, int startIndex)
        {
            byte[] t = new byte[2];
            for (int i = 0; i < 2 && i < bs.Length - startIndex; i++)
            {
                t[i] = bs[startIndex + i];
            }

            //byte b = t[0];
            //t[0] = t[1];
            //t[1] = b;

            return BitConverter.ToUInt16(t, 0);
        }

        public static uint BytesToUInt(byte[] b, int startIndex, int number)
        {
            byte[] t = new Byte[number];
            for (int i = 0; i < number && i < b.Length - startIndex; i++)
            {
                t[i] = b[startIndex + i];
            }

            byte temp;
            for (int i = t.Length - 1; i > t.Length / 2; i--)
            {
                temp = t[i];
                t[i] = t[t.Length - 1 - i];
                t[i] = temp;
            }
            return (BitConverter.ToUInt32(t, 0));
        }

        /// <summary>
        /// 没有指定起始索引
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static uint BytesToUInt(byte[] bs)
        {
            return (BytesToUInt(bs, 0));
        }

        public static ushort BytesToUInt16(byte[] bs)
        {
            return (BytesToUInt16(bs, 0));
        }



    }

}
