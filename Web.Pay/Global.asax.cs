using GL.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web.Pay
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILog log = LogManager.GetLogger("Application");

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4netRegister.Register();
        }

        protected void Application_Error(object sender, EventArgs e)
        {

            //Exception exception = Server.GetLastError();
            Exception objErr = Server.GetLastError().GetBaseException();
            //string error = string.Empty;
            //string errortime = string.Empty;
            //string erroraddr = string.Empty;
            //string errorinfo = string.Empty;
            //string errorsource = string.Empty;
            //string errortrace = string.Empty;

            //error += "发生时间:" + System.DateTime.Now.ToString() + "<br>";
            //errortime = "发生时间:" + System.DateTime.Now.ToString();

            //error += "发生异常页: " + Request.Url.ToString() + "<br>";
            //erroraddr = "发生异常页: " + Request.Url.ToString();

            //error += "异常信息: " + objErr.Message + "<br>";
            //errorinfo = "异常信息: " + objErr.Message;

            //errorsource = "错误源:" + objErr.Source;
            //errortrace = "堆栈信息:" + objErr.StackTrace;
            //error += "--------------------------------------<br>";
            //Server.ClearError();
            //Application["error"] = error;

            //logsb.Append(DateTime.Now.ToString() + "\n");
            //logsb.Append(DateTime.Now.ToString() + "\n");




            //log.Error(string.Concat("Application级别错误 ", exception.GetHttpCode(), " \n", Utils.GetUrl()), objErr);

            log.Error("Application级别错误 " + Utils.GetUrl(), objErr);
            //不再捕获未知错误
            //Server.ClearError();
            //Response.StatusCode = 404;
            //Response.Redirect("~/error/error.aspx");
            //Server.ClearError();
        }


    }
}
