using GL.Command.DBUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TWeb.App_Start;

namespace TWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            TaskRegister.TimerTask();
        }


        protected void Application_End()
        {
            Thread.Sleep(1000);
            string url = PubConstant.GetConnectionString("homeUrl");

        

       

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            Stream receiveStream = Response.GetResponseStream();//得到回写的字节流
        }


    }
}
