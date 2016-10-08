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
using System.Net; 
//泛型
using System.Collections.Generic;
//socket
using System.Net.Sockets;
using System.Text;
//正则
using System.Text.RegularExpressions;
namespace GL.Pay.QQPay
{
	/**
	 * 统计上报接口调用情况
	 *
	 * @version 3.0.0
	 * @author open.qq.com
	 * @copyright © 2012, Tencent Corporation. All rights reserved.
	 * @ History:
	 *               3.0.0 | coolinchen | 2013-1-11 11:11:11 | initialization
	 */
    public class SnsStat
    {
        public SnsStat()
        {
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns>返回毫秒数</returns>
        static public long GetTime()
        {
            return (DateTime.Now.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
        }

        /// <summary>
        /// 统计上报
        /// </summary>
        /// <param name="stat_url">统计上报的URL </param>
        /// <param name="start_time">统计开始时间</param>
        /// <param name="param">统计参数数组</param>
        static public void StatReport(string stat_url, long start_time, Dictionary<string, string> param)
        {
            long end_time = GetTime();
            double timeCost = (end_time - start_time) / 1000.0;

            try
            {
                string srv_ip_string = "";
                //判断ip还是域名
                string num = "(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)";
                if (Regex.IsMatch(param["svr_name"], ("^" + num + "\\." + num + "\\." + num + "\\." + num + "$"))) 
                {
                    srv_ip_string = param["svr_name"].ToString();
                }
                else
                {
                    IPHostEntry srv_name_info = Dns.GetHostEntry(param["svr_name"]);
                    IPAddress srv_ip = srv_name_info.AddressList[0];
                    srv_ip_string = srv_ip.ToString();
                }
                string param_format = "\"appid\":{0}, \"pf\":\"{1}\",\"rc\":{2},\"svr_name\":\"{3}\",\"interface\":\"{4}\"," +
                                     " \"protocol\":\"{5}\",\"method\":\"{6}\",   \"time\":{7},\"timestamp\":{8},\"collect_point\":\"sdk-dot-net-v3\"";

                string send_str = "{" + string.Format(param_format, param["appid"], param["pf"], param["rc"], srv_ip_string, param["interface"],
                                    param["protocol"], param["method"], timeCost.ToString(), (end_time / 1000).ToString()) + "}";

               
                //上报
                IPHostEntry host_info = Dns.GetHostEntry(stat_url);
                IPAddress ip = host_info.AddressList[0];

                //设置服务IP，设置TCP端口号
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip.ToString()), 19888);

                //创建socket
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                byte[] data = new byte[1024];
                data = Encoding.ASCII.GetBytes(send_str);
                server.SendTo(data, data.Length, SocketFlags.None, ipep);
                server.Close();
            }
            catch(Exception e)
            {
            }
        }
    }
}