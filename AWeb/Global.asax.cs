using AWeb.Controllers;
using GL.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AWeb
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
        }


        protected void Application_Error()
        {
            Exception objErr = Server.GetLastError().GetBaseException();
            log.Error("Application级别错误 " + Utils.GetUrl(), objErr);

            //if (Context.IsCustomErrorEnabled) ShowCustomErrorPage(Server.GetLastError());
        }

        /// <summary>  
        /// 错误显示处理  
        /// </summary>  
        private void ShowCustomErrorPage(Exception exception)
        {
            var httpException = exception as HttpException ?? new HttpException(500, "服务器内部错误", exception);

            Response.Clear();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
                case 403:
                    routeData.Values.Add("action", "Error403");
                    break;
                case 404:
                    routeData.Values.Add("action", "Error404");
                    break;
                case 405:
                    routeData.Values.Add("action", "Error405");
                    break;
                case 500:
                    routeData.Values.Add("action", "Error500");
                    break;

                default:
                    routeData.Values.Add("action", "GeneralError");
                    routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                    routeData.Values.Add("error", exception.GetBaseException());
                    break;
            }

            Server.ClearError();

            IController controller = new NoViewController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }


    }
}
