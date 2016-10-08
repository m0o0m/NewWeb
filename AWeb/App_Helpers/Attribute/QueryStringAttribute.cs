using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AWeb
{
    public class QueryStringAttribute : ActionFilterAttribute
    {
        // Methods
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Dictionary<string, string> querystringlist = new Dictionary<string, string>();

            if (filterContext.HttpContext.Request.QueryString.Count > 0)
            {
                foreach (string key in filterContext.HttpContext.Request.QueryString)
                 {
                     querystringlist.Add(key, filterContext.HttpContext.Request.QueryString[key]);
                 }

            }
            if (filterContext.HttpContext.Request.Form.Count > 0)
            {
                foreach (string key in filterContext.HttpContext.Request.Form)
                {
                    querystringlist.Add(key, filterContext.HttpContext.Request.Form[key]);
                }

            }

            filterContext.ActionParameters["querystring"] = querystringlist;
            base.OnActionExecuting(filterContext);
        }
    }
}