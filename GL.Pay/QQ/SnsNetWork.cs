using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//泛型
using System.Collections.Generic;
//网络
using System.Net;
using System.Net.Security;
using System.Text;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
//签名类

namespace GL.Pay.QQPay
{
    /**
     * 发送HTTP网络请求类
     *
     * @version 3.0.0
     * @author open.qq.com
     * @copyright © 2012, Tencent Corporation. All rights reserved.
     * @ History:
     *               3.0.0 | coolinchen | 2013-1-11 11:11:11 | initialization
     *               3.0.3 | coolinchen | 2014-1-20 11:11:11 | 增大DefaultConnectionLimit连接限制数到500
     */

    /*结果数组类*/
    public class RstArray
    {
        public int Ret = 0;
        public string Msg = "";
    }
	
	/*http 请求类*/
    public class SnsNetWork
    {
        const int ERROR_SNSNETWORK_HTTP = 1; //http请求异常，查看返回的错误信息
        public SnsNetWork()
        {
        }

        /// <summary>
        /// 执行一个 HTTP 请求
        /// </summary>
        /// <param name="url">执行请求的URL</param>
        /// <param name="param">表单参数</param>
        /// <param name="cookie">cookie参数 </param>
        /// <param name="method">请求方法 post / get</param>
        ///  <param name="protocol"> http协议类型 http / https</param>
        /// <returns>返回结果数组</returns>
        static public RstArray MakeRequest(string url, Dictionary<string, string> param, Dictionary<string, string> cookie, string method, string protocol)
        {
            string query_string = MakeQueryString(param);
            string cookie_string = MakeCookieString(cookie);
            //结果
            RstArray result = new RstArray();
            //请求类
            HttpWebRequest request = null;
            //请求响应类
            HttpWebResponse response = null;
            //响应结果读取类
            StreamReader reader = null;

            //http连接数限制默认为2，多线程情况下可以增加该连接数，非多线程情况下可以注释掉此行代码
            ServicePointManager.DefaultConnectionLimit = 500;

            try
            {

                if (method.Equals("get", StringComparison.OrdinalIgnoreCase))
                {

                    if (url.IndexOf("?") > 0)
                    {
                        url = url + "&" + query_string;
                    }
                    else
                    {
                        url = url + "?" + query_string;
                    }
                    //如果是发送HTTPS请求   
                    if (protocol.Equals("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        request = WebRequest.Create(url) as HttpWebRequest;
                        request.ProtocolVersion = HttpVersion.Version10;
                    }
                    else
                    {
                        request = WebRequest.Create(url) as HttpWebRequest;
                    }
                    request.Method = "GET";
                    request.Timeout = 3000;

                }
                else
                {
                    //如果是发送HTTPS请求   
                    if (protocol.Equals("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        request = WebRequest.Create(url) as HttpWebRequest;
                        request.ProtocolVersion = HttpVersion.Version10;
                    }
                    else
                    {
                        request = WebRequest.Create(url) as HttpWebRequest;
                    }
                    //去掉“Expect: 100-Continue”请求头，不然会引起post（417） expectation failed
                    ServicePointManager.Expect100Continue = false;

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Timeout = 3000;
                    //POST数据   
                    byte[] data = Encoding.UTF8.GetBytes(query_string);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                //cookie
                if (cookie_string != null)
                {
                    request.Headers.Add("Cookie", cookie_string);
                }

                //response
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                //return
                result.Msg = reader.ReadToEnd();
                result.Ret = 0;

            }
            catch (Exception e)
            {
                result.Msg = e.Message;
                result.Ret = ERROR_SNSNETWORK_HTTP;
            }
            finally
            {
                if(request != null)
                {
                    request.Abort();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }


        /// <summary>
        /// 执行一个 HTTP 请求,以post方式，multipart/form-data的编码类型上传文件
        /// </summary>
        /// <param name="url">执行请求的URL</param>
        /// <param name="param">表单参数</param>
        /// <param name="cookie">cookie参数 </param>
        /// <param name="protocol"> http协议类型 http / https</param>
        ///  <param name="file_name"> 文件名，文件相应参数的参数名，例如/v3/t/add_pic_t中的 "pic" </param>
        ///  <param name="file_path"> 文件的路径 </param>
        /// <returns>返回结果数组</returns>
        public static RstArray MutilpartPostFile(string url,Dictionary<string, string> param,Dictionary<string, string> cookie, 
                                             string protocol,string file_name,string file_path)
        {
            //结果
            RstArray result = new RstArray();
            //请求类
            HttpWebRequest request = null;
            //包体填充类
            MemoryStream mem_stream = null;
            Stream req_stream = null;

            //请求响应类
            HttpWebResponse response = null;
            //文件流
            FileStream file_stream = null;
            //响应结果读取类
            StreamReader reader = null;

            //http连接数限制默认为2，多线程情况下可以增加该连接数，非多线程情况下可以注释掉此行代码
            ServicePointManager.DefaultConnectionLimit = 500;

            try
            {
                //https请求   
                if (protocol.Equals("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }

                // 设置属性
                request.Method = "POST";
                request.Timeout = 3000;

                //cookie
                string cookie_string = MakeCookieString(cookie);
                if (cookie_string != null)
                {
                    request.Headers.Add("Cookie", cookie_string);
                }

                //去掉“Expect: 100-Continue”请求头，不然会引起post（417） expectation failed
                ServicePointManager.Expect100Continue = false;

                //设置请求体
                mem_stream = new MemoryStream();

                //边界符
                string boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                byte[] begin_boundary = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
                byte[] end_boundary = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");

                request.ContentType = "multipart/form-data; boundary=" + boundary;

                //开头
                mem_stream.Write(begin_boundary, 0, begin_boundary.Length);

                //参数 
                string param_format = "Content-Disposition: form-data; name=\"{0}\"" +
                                       "\r\n\r\n{1}\r\n--" + boundary + "\r\n";
                StringBuilder param_string = new StringBuilder();
                foreach (string key in param.Keys)
                {
                    param_string.AppendFormat(param_format, key, param[key]);
                }
                byte[] param_byte = Encoding.UTF8.GetBytes(param_string.ToString());
                mem_stream.Write(param_byte, 0, param_byte.Length);

                //文件
                file_stream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
                string file_format = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                                              "Content-Type: application/octet-stream\r\n\r\n";
                string file_string = string.Format(file_format, file_name, file_path);
                byte[] file_byte = Encoding.UTF8.GetBytes(file_string);
                mem_stream.Write(file_byte, 0, file_byte.Length);

                byte[] buffer = new byte[1024];
                int bytes;
                while ((bytes = file_stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    mem_stream.Write(buffer, 0, bytes);
                }

                //结尾
                mem_stream.Write(end_boundary, 0, end_boundary.Length);


                request.ContentLength = mem_stream.Length;
                req_stream = request.GetRequestStream();
                mem_stream.Position = 0;
                byte[] temp_buffer = new byte[mem_stream.Length];
                mem_stream.Read(temp_buffer, 0, temp_buffer.Length);
                req_stream.Write(temp_buffer, 0, temp_buffer.Length);

                //响应
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                //return
                result.Msg = reader.ReadToEnd();
                result.Ret = 0;
            }
            catch (Exception e)
            {
                result.Msg = e.Message;
                result.Ret = ERROR_SNSNETWORK_HTTP;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (file_stream != null)
                {
                    file_stream.Close();
                    file_stream.Dispose();
                }
                if (mem_stream != null)
                {
                    mem_stream.Close();
                    mem_stream.Dispose();
                }
                if (req_stream != null)
                {
                    req_stream.Close();
                    req_stream.Dispose();
                }
            }

            return result;
        }


        static public  string MakeQueryString(Dictionary<string, string> param)
        {
            if (param.Count == 0)
            {
                return "";
            }
            string query_string = "";
            foreach (string key in param.Keys)
            {
                query_string = query_string + SnsSigCheck.UrlEncode(key, Encoding.UTF8) + "=" + SnsSigCheck.UrlEncode(param[key], Encoding.UTF8) + "&";
            }
            query_string = query_string.Substring(0, query_string.Length - 1);

            return query_string;
        }


        static private string MakeCookieString(Dictionary<string, string> cookie)
        {
            if (cookie.Count == 0)
            {
                return null;
            }
            string[] arr_cookies = new string[cookie.Count];
            int i=0;
            foreach(string key in cookie.Keys)
            {
                arr_cookies[i] = key + "=" + cookie[key];
                i++;
            }
            return string.Join("; ", arr_cookies);
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }  

    }
}