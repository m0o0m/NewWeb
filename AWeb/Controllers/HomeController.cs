using AWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using System.Security.Principal;
using GL.Data.AWeb.Identity;

namespace AWeb.Controllers
{

    [Authorize(Roles = "SU,公司,股东,总代,代理,CPS,CPA,Guest,策划")]
    public class HomeController : Controller
    {
        private AgentSignInManager _signInManager;
        private AgentUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(AgentUserManager userManager, AgentSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AgentSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AgentSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AgentUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AgentUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("AgentInfo", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            // Add all errors that were returned to the page error collection
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        [AllowAnonymous]
        public ActionResult login(MLogin model)
        {
            if (Request.IsAuthenticated)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            }

            //MLogin _login = new MLogin();
            model.UserName = "";
            model.Password = "";
            //model.ReturnUrl = "";

#if DEBUG
            model.UserName = "AA515";
            model.Password = "zxcasdqwe123QQ";
#endif


            ModelState.Clear();
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(MLogin model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

           
            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true




            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "账号被锁定。");
                    //return View("Lockout");
                    return View(model);
                case SignInStatus.RequiresVerification:
                    //return RedirectToAction("SendCode", new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
                    ModelState.AddModelError("", "验证码错误。");
                    return View(model);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "客服账号或密码错误。");
                    return View(model);
            }

        }


        //[HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // First we clean the authentication ticket like always
            //FormsAuthentication.SignOut();
            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            // Second we clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            FormsAuthentication.SignOut();



            // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
            // this clears the Request.IsAuthenticated flag since this triggers a new request
            return RedirectToAction("Login", "Home");
        }


        public ActionResult RefreshToken()
        {
            return PartialView("_AntiForgeryToken");
        }


        public ActionResult AgentInfo()
        {
            if (Request.IsAuthenticated)
            {
            }

            //var user = UserManager.FindByName(User.Identity.Name);
            var user = UserManager.FindById(User.Identity.GetUserId<int>());

            return View(user);
        }

    }
}