using GL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Redis
{
    /// <summary>
    /// FormUrl解析器
    /// </summary>
    public class QueryValuesAttribute : ActionFilterAttribute
    {
        // Methods
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string queryvalues = "";
            if (!((filterContext.RouteData.Values["queryvalues"] == null) || string.IsNullOrWhiteSpace(filterContext.RouteData.Values["queryvalues"].ToString())))
            {
                queryvalues = filterContext.RouteData.Values["queryvalues"].ToString();
            }

            Dictionary<string, string> queryvalueslist = Utils.GetDicFormUrl(queryvalues);


            if (filterContext.HttpContext.Request.QueryString.Count > 0)
            {
                foreach (string key in filterContext.HttpContext.Request.QueryString)
                {
                    queryvalueslist.Add(key, filterContext.HttpContext.Request.QueryString[key]);
                }

            }
            if (filterContext.HttpContext.Request.Form.Count > 0)
            {
                foreach (string key in filterContext.HttpContext.Request.Form)
                {
                    queryvalueslist.Add(key, filterContext.HttpContext.Request.Form[key]);
                }

            }


            filterContext.ActionParameters["queryvalues"] = queryvalueslist;
            base.OnActionExecuting(filterContext);
        }
    }

}