using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILog log = LogManager.GetLogger("JWeb");
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            log4netRegister.Register();

        }
    }
}
