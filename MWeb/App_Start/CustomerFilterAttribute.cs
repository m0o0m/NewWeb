using GL.Data.MWeb.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.SessionState;
using GL.Data.BLL;
using System.Collections;
using MWeb.Models;

namespace MWeb
{
    public class CustomerFilterAttribute : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
          
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            var url = controllerName + "/" + actionName;
          

            var userid = HttpContext.Current.User.Identity.GetUserId();
            var userName = HttpContext.Current.User.Identity.Name.ToLower();
            if (url == "Home/Login" || url == "Home/login") {
                if (!string.IsNullOrEmpty(userName)) {
                    HttpContext.Current.Response.Redirect("/Base/Default");
                }
            }


            if (userName != "admin") {//如果不是内置初始管理员，那么就走权限系统
                if ( 
                     url.ToLower() != "home/login" && 
                     url.ToLower() != "home/logoff" && 
                     controllerName.ToLower()!= "error" &&
                     url.ToLower() !="base/default" &&
                      controllerName.ToLower() != "noauth" &&  
                         controllerName.ToUpper() != "AD" &&
                         controllerName.ToLower()!= "simulatorrecharge"
                     ) {//不是登录登出，登录登出不走权限系统
                    //检查用户是否有此action的权限，没有就跳转到提示无权限的页面
                    bool check = SUBLL.CheckUserAction(userid, url);
                    if (check == false) {
                        //跳转
                        HttpContext.Current.Response.Redirect("/Error/NoPower?url="+ url);
                    }
                }
            }

            OnePointLogin();



          

            OperLog("/"+controllerName + "/" + actionName, filterContext.HttpContext.Request, userName);





            base.OnActionExecuting(filterContext);


        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
         
        
        }

        private void OnePointLogin()
        {
            List<OnePoint> hOnline = (List<OnePoint>)HttpContext.Current.Application["Online"];//获取已经存储的application值

            bool hasCookie = HttpContext.Current.Request.Cookies["markCookie"] != null;

            if (hOnline != null && hasCookie==true)
            {
                string sID = HttpContext.Current.Request.Cookies["markCookie"].Value.ToString();
                if (hOnline.Count != 0) {
                    for (int i = 0; i < hOnline.Count(); i++)
                    {
                        OnePoint onepoint = hOnline[i];
                        if (onepoint.SessionId != null && onepoint.SessionId.Equals(sID)) {
                            if (onepoint.LoginName != null && "XX".Equals(onepoint.LoginName.ToString()))//说明在别处登录
                            {
                                hOnline.Remove(onepoint);
                              
                                HttpContext.Current.Application.Lock();
                                HttpContext.Current.Application["Online"] = hOnline;
                                HttpContext.Current.Application.UnLock();

                                for (int z = 0; z < HttpContext.Current.Request.Cookies.Count; z++)
                                {
                                    string cookieName = HttpContext.Current.Request.Cookies[z].Name;

                                    try
                                    {
                                        HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-10);
                                    }
                                    catch {

                                    }
                                }
                                HttpContext.Current.Response.Write("<script>alert('你的帐号已在别处登陆，你被强迫下线！');top.location.href='/Home/login';window.close();</script>");//退出当前到登录页面                                                                                                                                  // Response.Redirect("Default.aspx");
                                HttpContext.Current.Response.End();
                            }
                        }
                    }
                }

            }
        }


        private void OperLog(string Action,HttpRequestBase Request,string UserName) {

            List<string> querystringlist = new List<string>();
            if (Request.QueryString.Count > 0)
            {
                foreach (string key in Request.QueryString)
                {
                    querystringlist.Add(key);
                }

            }
            if (Request.Form.Count > 0)
            {
                foreach (string key in Request.Form)
                {
                    querystringlist.Add(key);
                }
            }
            string param = "";
            for (int i=0;i<querystringlist.Count();i++)
            {
                string item = querystringlist[i];
                if (i == 0)
                {
                    param += "%%"+item +":%%";
                }
                else {
                    param += (  "%%%"+item+":%%%"  );
                }
            }


            //// 'Channels:%StartDate:%%ExpirationDate:%'


            OperLogBLL.WriteOperLog(Action, param, UserName, Request.UserHostAddress);





        }

    }
}