using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{*queryvalues}",
                new { controller = "Home", action = "login", queryvalues = UrlParameter.Optional }
                , new string[] { "MWeb.Controllers" }
            );


          //  routes.MapRoute(
          //    name: "Default",
          //    url: "{controller}/{action}/{*queryvalues}",
          //    defaults: new { controller = "Home", action = "login", queryvalues = UrlParameter.Optional }
          //    , new string[] { "MWeb.Controllers" }
          //);
        }
    }
}
