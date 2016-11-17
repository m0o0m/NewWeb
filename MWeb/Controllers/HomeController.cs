using GL.Data.BLL;
using GL.Data.DAL;
using GL.Data.Model;
using MWeb.Models;
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

namespace MWeb.Controllers
{
    [Authorize]

    public class HomeController : CaptchaController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        [AllowAnonymous]
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> login(MLogin _login, string returnUrl)
        {
            string yzm = _login.YZM;
            string sessYZM = Session["ValidateCode"].ToString();

         


            var user = await UserManager.FindByNameAsync(_login.UserName);

            string username = _login.UserName;
            string passwd = _login.Password;

            if (user == null || string.IsNullOrEmpty(user.UserName))
            {
                return Json(
                    new { result = Result.UserDoesNotExist }
                );
            }

            ASPNetUserLimit limit = OperLogBLL.GetASPNetUserLimit(new ASPNetUserLimit()
            {
                Username = username
            });
            if (limit != null)
            {
                if (limit.ErrorNum >= 3) {
                    return Json(
                         new { result = Result.ParaErrorCount }
                     );
                }
            }
           


            if (yzm != sessYZM)
            {
                OperLogBLL.UpdateASPNetUserLimit(new ASPNetUserLimit() {
                     Username = username
                });

                return Json(
                   new { result = Result.ParaYZMError }
               );
            }
            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(username, passwd, isPersistent : true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    //用户登录成功
                    AfterLoginSucess(username);

                    //SUBLL.AddLog(new LogInfo()
                    //{
                    //    UserAccount = username,
                    //    Detail = "",
                    //    Content = "登入",
                    //    CreateTime = DateTime.Now,
                    //    LoginIP = Request.UserHostAddress,
                    //    OperModule = "登录后台"
                    //});



                    OperLogBLL.InsertOperLog(new OperLog()
                    {
                        CreateTime = DateTime.Now.ToString(),
                        LeftMenu = "登入",
                        OperDetail = "",
                        OperType = "登入",
                        UserAccount = username,
                        UserName = username,
                        IP = Request.UserHostAddress
                    });

                    OperLogBLL.UpdateASPNetUserReset(new ASPNetUserLimit()
                    {
                        Username = username
                    });

                    return Json(RedirectToLocal(returnUrl));

                case SignInStatus.LockedOut:
                    //return View("Lockout");
                case SignInStatus.RequiresVerification:
                    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    //ModelState.AddModelError("", "无效的登录尝试。");
                    //return View(model);

                    OperLogBLL.UpdateASPNetUserLimit(new ASPNetUserLimit()
                    {
                        Username = username
                    });


                    return Json(new { result = Result.PasswordIsIncorrect });
            }


        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            return RedirectToAction("Login");
        }

        [HttpPost]
        [QueryValues]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(Dictionary<string, string> queryvalues)
        {
            LogInfo info = new LogInfo()
            {
                UserAccount = User.Identity.Name,
                Detail = "",
                Content = "登出",
                CreateTime = DateTime.Now,
                LoginIP = Request.UserHostAddress,
                OperModule = "登录后台"
            };
            SUBLL.AddLog(info);

            AuthenticationManager.SignOut();
            return Json(new { result = Result.Normal });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private object RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return new { result = Result.Redirect, msg = returnUrl };
            }
            return new { result = Result.Redirect, msg = Url.Action("Default", "Base") };
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
            string sID = Guid.NewGuid().ToString();

            HttpContext.Response.Cookies.Add(
  new HttpCookie("markCookie",sID) { Expires = DateTime.Now.AddYears(1) }
  );

           

           

            List<OnePoint> hOnline = (List<OnePoint>)HttpContext.Application["Online"];//读取全局变量
            if (hOnline != null)
            {
                if (hOnline.Count() != 0) {
                   
                    for (int i = 0; i < hOnline.Count(); i++) {
                        OnePoint point = hOnline[i];
                        if (point.LoginName != null && point.LoginName.Equals(username))
                        {//如果当前用户已经登录，
                           
                            point.LoginName = "XX";//将当前用户已经在全局变量中的值设置为XX
                            hOnline.Remove(hOnline[i]);
                            hOnline.Add(point);
                         
                            break;
                        }

                    }
                }
            }
            else
            {
                hOnline = new List<OnePoint>();
            }
            OnePoint pointLogin = new OnePoint()
            {
                SessionId = sID, LoginName = username
            };
            hOnline.Add(pointLogin);
            HttpContext.Application.Lock();
            HttpContext.Application["Online"] = hOnline;
            HttpContext.Application.UnLock();
           
        }


    }
}