using GL.Data.BLL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MWeb
{
    public class CustomerApiFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            var controllerName = actionContext.ControllerContext.RouteData.Values["controller"].ToString();

            var method = (actionContext.ControllerContext.Request.Method).Method;
            var url = controllerName + "/" + method;

            var userid = HttpContext.Current.User.Identity.GetUserId();
            var userName = HttpContext.Current.User.Identity.Name.ToLower();

            if (userName != "admin")
            {//如果不是内置初始管理员，那么就走权限系统
              //不是登录登出，登录登出不走权限系统
                    //检查用户是否有此action的权限，没有就跳转到提示无权限的页面
                    bool check = SUBLL.CheckUserAction(userid, url);
                    if (check == false)
                    {
                    //跳转
                        HttpContext.Current.Response.Clear();

                  

                       HttpContext.Current.Response.Write("2020");
                        HttpContext.Current.Response.End();
                    }
                
            }

            base.OnActionExecuting(actionContext);
        }
    }
}