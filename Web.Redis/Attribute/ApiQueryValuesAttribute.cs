using GL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Web.Redis
{
    /// <summary>
    /// FormUrl解析器
    /// </summary>
    public class ApiQueryValuesAttribute : ActionFilterAttribute
    {
        // Methods
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            string queryvalues = "";
            if (!((filterContext.ControllerContext.RouteData.Values["queryvalues"] == null) || string.IsNullOrWhiteSpace(filterContext.ControllerContext.RouteData.Values["queryvalues"].ToString())))
            {
                queryvalues = filterContext.ControllerContext.RouteData.Values["queryvalues"].ToString();
            }

            Dictionary<string, string> queryvalueslist = Utils.GetDicFormUrl(queryvalues);


            //if (filterContext.HttpContext.Request.QueryString.Count > 0)
            //{
            //    foreach (string key in filterContext.HttpContext.Request.QueryString)
            //    {
            //        queryvalueslist.Add(key, filterContext.HttpContext.Request.QueryString[key]);
            //    }

            //}
            //if (filterContext.HttpContext.Request.Form.Count > 0)
            //{
            //    foreach (string key in filterContext.HttpContext.Request.Form)
            //    {
            //        queryvalueslist.Add(key, filterContext.HttpContext.Request.Form[key]);
            //    }

            //}


            filterContext.ActionArguments["queryvalues"] = queryvalueslist;
            base.OnActionExecuting(filterContext);
        }

        //public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        //    base.OnActionExecuting(actionContext);
        //    //获取请求消息提数据
        //    Stream stream = actionContext.Request.Content.ReadAsStreamAsync().Result;
        //    Encoding encoding = Encoding.UTF8;
        //    stream.Position = 0;
        //    string responseData = "";
        //    using (StreamReader reader = new StreamReader(stream, encoding))
        //    {
        //        responseData = reader.ReadToEnd().ToString();
        //    }
        //    //反序列化进行处理
        //    var serialize = new JavaScriptSerializer();
        //    var obj = serialize.Deserialize<RequestDTO>(responseData);
        //    //在action执行前终止请求时，应该使用填充方法Response，将不返回action方法体。
        //    if (obj == null)
        //        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, obj);

        //    if (string.IsNullOrEmpty(obj.PhoneType) || string.IsNullOrEmpty(obj.PhoneVersion)
        //      || string.IsNullOrEmpty(obj.PhoneID) || obj.StartCity < 1)
        //    {
        //        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, obj);
        //    }
        //}





    }

}