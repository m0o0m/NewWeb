using GL.Data.BLL;
using GL.Data.DAL;
using GL.Data.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using GL.Data.MWeb.Identity;
using System.Security.Principal;
using System.Collections;
using CWeb.Models;

namespace CWeb.Controllers
{
  
    public class HomeController : Controller
    {
      

        public ActionResult login(string returnUrl)
        {

            //AuthenticationManager.SignOut();
            //HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            ViewBag.ReturnUrl = returnUrl;
            MLogin _login = new MLogin();
            _login.UserName = "";
            _login.Password = "";


            return View(_login);
        }


        [HttpPost]
        [QueryValues]
        public  ActionResult login(Dictionary<string,string> queryvalues)
        {
         
          
                string username = queryvalues.ContainsKey("username") ? queryvalues["username"] : "";
              string password = queryvalues.ContainsKey("password") ? queryvalues["password"] : "";
             string ReturnUrl = queryvalues.ContainsKey("ReturnUrl") ? queryvalues["ReturnUrl"] : "";
        
             CLoginUser user = ClubBLL.GetCLoginUserByLoginName(new CLoginUser { UserAccount = username });





            if (user == null || string.IsNullOrEmpty(user.UserAccount))
            {

               

                return Json(
                    new { result = Result.UserDoesNotExist }
                );
            }

            if (user.UserPassword == password)
            {
       
                Session["name"] = username;

                return Json(RedirectToLocal(ReturnUrl));
            }
            else
            {
                return Json(new { result = Result.PasswordIsIncorrect });
            }

        


        }

        public ActionResult LogOff()
        {
            Session["name"] = null;
            return RedirectToAction("Login");
        }

        [HttpPost]
        [QueryValues]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(Dictionary<string, string> queryvalues)
        {
            AuthenticationManager.SignOut();
            return Json(new { result = Result.Normal });
        }


        private object RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return new { result = Result.Redirect, msg = returnUrl };
            }
            return new { result = Result.Redirect, msg = Url.Action("ClubList", "Base") };
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //private ActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion




        public void AfterLoginSucess(string username)
        {
            //登录成功。。。。。。。


            Hashtable hOnline = (Hashtable)HttpContext.Application["Online"];//读取全局变量
            if (hOnline != null)
            {
                IDictionaryEnumerator idE = hOnline.GetEnumerator();
                string strKey = "";
                while (idE.MoveNext())
                {
                    if (idE.Value != null && idE.Value.ToString().Equals(username))//如果当前用户已经登录，
                    {
                        //already login            
                        strKey = idE.Key.ToString();
                        hOnline[strKey] = "XX";//将当前用户已经在全局变量中的值设置为XX
                        Session[strKey] = null;
                        break;
                    }
                }
            }
            else
            {
                hOnline = new Hashtable();
            }

            hOnline[HttpContext.Request.Cookies[0].Value] = username;//初始化当前用户的
            HttpContext.Application.Lock();
            HttpContext.Application["Online"] = hOnline;
            HttpContext.Application.UnLock();

        }


    }
}