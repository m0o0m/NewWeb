using GL.Command.DBUtility;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILog log = LogManager.GetLogger("Application");
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4netRegister.Register();

            TaskAction.TaskRegister();

            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

        }


        protected void Application_End()
        {
            Thread.Sleep(1000);
          


               string url = PubConstant.GetConnectionString("homeUrl");
            //#if Debug


            //#endif
            //#if P17
            //               url = "http://192.168.1.17:9005/Base/Default";
            //#endif
            //#if Test
            //              url = "http://ttt.515.com/Base/Default";
            //#endif
            //#if Release
            //              url = "http://qqq.515.com/Base/Default";
            //#endif


            string[] urls = url.Split(',');

            for (int i = 0; i < urls.Length; i++) {
                string u = urls[i];
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(u);
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                Stream receiveStream = Response.GetResponseStream();//得到回写的字节流
            }
        }


        protected void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。
            Hashtable hOnline = (Hashtable)Application["Online"];
            if (hOnline[Session.SessionID] != null)
            {
                hOnline.Remove(Session.SessionID);//清除当前SessionID
                Application.Lock();
                Application["Online"] = hOnline;
                Application.UnLock();
            }

          
        }


#if Debug

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                StackExchange.Profiling.MiniProfiler.Start();
            }
        }
        protected void Application_EndRequest()
        {
            StackExchange.Profiling.MiniProfiler.Stop();
        }

#endif

    }
}
