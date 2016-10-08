using A2.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace A2.Web.Controllers
{
    public class TAsyncController : AsyncController
    {

        public async Task<ViewResult> New(string city)  
        { 
            return View(new ViewStringModel(){   
                Text = await NewThread(city)
            });
        }   
        private async Task<string>NewThread(string input)   
        { 
            Thread.Sleep(5000);   
            return input; 
        } 


        public async Task<ActionResult> NewsAsyncAsync()
        {
            DateTime dt1 = DateTime.Now;
            var rss = new string[] 
            {
                "http://articles.csdn.net/api/rss.php?tid=1008",
                "http://aspnet.codeplex.com/project/feeds/rss",
                "http://solidot.org.feedsportal.com/c/33236/f/556826/index.rss",
                "http://www.codeguru.com/icom_includes/feeds/codeguru/rss-csharp.xml",
                "http://feed.google.org.cn/"
            };
            List<List<NewsItem>> list = new List<List<NewsItem>>();
            foreach (var item in rss)
            {
                List<NewsItem> news = await (NewsModels.GetNews(item));
                list.Add(news);
            }
            var model = list.SelectMany(x => x)
                            .ToList()
                            .OrderByDescending(x => x.PostDate)
                            .Take(100);
            DateTime dt2 = DateTime.Now;
            ViewBag.TimeCost = new TimeSpan(dt2.Ticks - dt1.Ticks).ToString();
            return View("News", model);
        }

        public ActionResult NewsSync()
        {
            DateTime dt1 = DateTime.Now;
            var rss = new string[] 
                {
                    "http://articles.csdn.net/api/rss.php?tid=1008",
                    "http://aspnet.codeplex.com/project/feeds/rss",
                    "http://solidot.org.feedsportal.com/c/33236/f/556826/index.rss",
                    "http://www.codeguru.com/icom_includes/feeds/codeguru/rss-csharp.xml",
                    "http://feed.google.org.cn/"
                };
            var model = rss.SelectMany(x => NewsModels.GetNews(x).Result)
                           .ToList()
                           .OrderByDescending(x => x.PostDate)
                           .Take(100);
            DateTime dt2 = DateTime.Now;
            ViewBag.TimeCost = new TimeSpan(dt2.Ticks - dt1.Ticks).ToString();
            return View("News", model);
        }






        public async Task<ActionResult> Socket()
        {

            var aa = GetWhoisInfo();


            return View(new ViewStringModel()
            {
                Text = await GetWhoisInfo()
            });
        }


        private async Task ProcessWSChat(AspNetWebSocketContext arg)
        {
            WebSocket socket = arg.WebSocket;
            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {
                    string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    string returnMessage = "You send :" + message + ". at" + DateTime.Now.ToLongTimeString();



                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMessage));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }

        public static async Task<string> GetWhoisInfo()
        {
            string server = "192.168.1.17";
            string cmd = "1";

            string result = "";

            UTF8Encoding utf8 = new UTF8Encoding();
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(server, 5200);
            NetworkStream networkStream = tcpClient.GetStream();





            cmd = cmd.Replace("0x00", "");

            //byte[] buffer = Encoding.GetEncoding("GB2312").GetBytes(cmd + cmd.Length + "\r\n");

            byte[] buffer = new byte[4] { 0x04, 0x7e, 0x7e, 0x7e };

            
            
            
            
            
            networkStream.Write(buffer, 0, buffer.Length);
            buffer = new byte[10240];


            int i = networkStream.Read(buffer, 0, buffer.Length);
            while (i > 0)
            {
                i = networkStream.Read(buffer, 0, buffer.Length);
                result += utf8.GetString(buffer);
                //result   +=Encoding.GetEncoding("GB2312").GetString(buffer);   
            }
            networkStream.Close();
            tcpClient.Close();
            result = result.Replace("\u0000", "");
            return result;
        }



    }
}
