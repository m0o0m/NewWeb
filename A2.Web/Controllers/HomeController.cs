using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;

using A2.Web.Models;
using System.IO;
using ProtoCmd.Service;  

namespace A2.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List()
        {
            return View();
        }

        public ActionResult sub()
        {
            return View();
        }
        


        public ActionResult Socket()
        {
            try
            {
                CMsgHead head = CMsgHead.CreateBuilder()
                    .SetMsglen(2+2+1)
                    .SetMsgtype(2)
                    .Build();

                CMsgReg body = CMsgReg.CreateBuilder().
                    SetT(1)
                   .Build();

                CMsg msg = CMsg.CreateBuilder()
                    .SetMsghead(head.ToByteString().ToStringUtf8())
                    .SetMsgbody(body.ToByteString().ToStringUtf8())
                    .Build();


                Console.WriteLine("CLIENT : 对象构造完毕 ...");

                using (TcpClient client = new TcpClient())
                {
                    // client.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.116"), 12345));
                    client.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.30"), 5200));
                    Console.WriteLine("CLIENT : socket 连接成功 ...");

                    using (NetworkStream stream = client.GetStream())
                    {
                        //发送
                        Console.WriteLine("CLIENT : 发送数据 ...");
                      
                        msg.WriteTo(stream);

                        //关闭
                        stream.Close();
                    }
                    client.Close();
                    Console.WriteLine("CLIENT : 关闭 ...");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("CLIENT ERROR : {0}", error.ToString());
            }

            return View();
        }















    }
}
