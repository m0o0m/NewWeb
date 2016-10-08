using GL.Instance;
using WOHAIWeb.Code.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WOHAIWeb
{
    /// <summary>
    /// 验证类型列举
    /// </summary>
    public enum UserToUrlEnum
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login,
        /// <summary>
        /// 注册
        /// </summary>
        Register,
        /// <summary>
        /// 认证
        /// </summary>
        Certificate,
    }

    /// <summary>
    /// 用户验证过滤器
    /// </summary>
    public class UserAuthentication : AuthorizeAttribute
    {
        public UserToUrlEnum UserToUrlEnum { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserAuthentication()
        {
            this.UserToUrlEnum = UserToUrlEnum.Login;
        }

        /// <summary>
        /// 执行前验证
        /// </summary>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            base.OnAuthorization(filterContext);

            switch (filterContext.HttpContext.Response.StatusCode)
            {
                case 403:
                    switch (this.UserToUrlEnum)
                    {
                        case UserToUrlEnum.Login:
                        //VCommons.Http.CookieHelper.Write("return_page", HttpContext.Current.Request.Url.ToString());
                        //Utils.WriteCookie(CookieKey.JumpRedirect, HttpContext.Current.Request.RawUrl);
                        //HttpContext.Current.Response.Redirect("/member/signin", true);
                        //new TransferResult("/member/signin");

                        case UserToUrlEnum.Register:
                            //filterContext.Result = new RedirectResult("/home/login"); new ActionResult("Default");
                            break;
                    }
                    //filterContext.Result = new RedirectResult("/NoView/Error403");
                    break;
                case 404:
                    filterContext.Result = new RedirectResult("/NoView/Error404");
                    break;
                case 405:
                    filterContext.Result = new RedirectResult("/NoView/Error405");
                    break;
                case 500:
                    filterContext.Result = new RedirectResult("/NoView/Error500");
                    break;

            }

        }

        /// <summary>
        /// 覆寫AuthorizeAttribute類別的AuthorizeCore方法
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool result = false;
            if (httpContext.User.Identity.IsAuthenticated)
            {
                if (CheckLoginInfo(httpContext))
                {
                    result = true;
                }
            }
            else
            {
                httpContext.Session.Abandon();
                httpContext.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            }
            if (result == false)
            {
                httpContext.Response.Redirect("/home/login");
            }
            return result;
        }

        /// <summary>
        /// 檢查Session是否還在，依據cookie取出使用者資訊，並取出該使用者的資料
        /// </summary>
        private bool CheckLoginInfo(HttpContextBase httpContext)
        {
            bool result = false;


            //if (string.IsNullOrEmpty(httpContext.Session.GetValue<int>(SessionKey.UserId).ToString()))
            //{

                //沒有使用者資訊
                //session不在，所以check cookie是否在
                //string cookieName = FormsAuthentication.FormsCookieName;
                //HttpCookie authCookie = httpContext.Request.Cookies[cookieName];
                //FormsAuthenticationTicket authTicket = null;
                //authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                //string usrid = authTicket.Name;


                string SessionID = ((FormsIdentity)httpContext.User.Identity).Ticket.UserData;
                if (string.IsNullOrEmpty(SessionID) == false)
                {
                //    //自動登入系統
                //    //FormsService.SignIn(usrid, true);
                //    var ou = OnlineUserHelper.GetUserOnline(SessionID);
                //    if (object.Equals(null, ou))
                //    {
                //        AuthRules.SignOut();
                //        return result;
                //    }
                //    try
                //    {
                //        if (ou.IsLeave)
                //        {
                //            Rules.Exit(SessionID);
                //            return result;
                //        }
                //    }
                //    catch (System.Exception ex)
                //    {
                //        AuthRules.SignOut();
                //        return result;
                //    }
                //    OnlineUserHelper.removeUserOnline(SessionID);
                //    Rules.Reset(ou);

                    var ou = OnlineStaticHelper.Select(SessionID);
                    if (ou.Id == 0)
                    {
                        Rule.Dispose();
                        return result;
                    }
                    if (ou.IsLeave)
                    {
                        Rule.Dispose();
                        return result;
                    }
                    Rule.Action();
                    httpContext.Session.SetValue<int>(SessionKey.UserId, Convert.ToInt32(httpContext.User.Identity.Name));

                    result = true;
                }
            //}
            //else { 
            //    result = true;
            //}
            return result;
        }



    }
}
