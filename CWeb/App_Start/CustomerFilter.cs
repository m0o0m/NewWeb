using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWeb.App_Start
{
    public class CustomerFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["name"] == null)
            {
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();
                if (controllerName == "Home" && actionName == "Login")
                {
                  
                }
                else {
                    HttpContext.Current.Response.Redirect("/Home/Login");
                }


               
            }
            else {
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();
                if (controllerName == "Home" && actionName == "Login") {
                    HttpContext.Current.Response.Redirect("/Base/ClubList");
                }


            }

            base.OnActionExecuting(filterContext);
        }
    }
}