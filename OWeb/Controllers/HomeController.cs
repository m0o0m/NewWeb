using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.DAL;
using OWeb.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Webdiyer.WebControls.Mvc;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Principal;
using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.Owin.Security;
using GL.Data.OWeb.Identity;

namespace OWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private CustomerSignInManager _signInManager;
        private CustomerUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(CustomerUserManager userManager, CustomerSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public CustomerSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<CustomerSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public CustomerUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<CustomerUserManager>();
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


        private async Task SignInAsync(CustomerUser user, bool isPersistent)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Create a claims based identity for the current user
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(identity.Name, isPersistent);
        }

        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("CustomerService", "Home");
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
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            //MLogin _login = new MLogin();
            model.UserName = "";
            model.Password = "";
            model.ReturnUrl = "";

#if DEBUG
            model.UserName = "serv333";
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // First we clean the authentication ticket like always
            //FormsAuthentication.SignOut();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Second we clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
            // this clears the Request.IsAuthenticated flag since this triggers a new request
            return RedirectToLocal();
        }


        public ActionResult CustomerService()
        {

            var user = UserManager.FindByName(User.Identity.Name);


            //CustomerInfo cust = CustomerInfoBLL.GetModelByID(new CustomerInfo { CustomerID = Session.GetValue<int>(SessionKey.UserId) });


            //var aa = User.Identity.Name;
            //var bb = ((FormsIdentity)User.Identity).Ticket.UserData;


            return View(user);
        }






    }
}