using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WOHAIWeb.Code.Instance;
using WOHAIWeb.Models;

namespace WOHAIWeb.Controllers
{
    [UserAuthentication]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult login()
        {
            MLogin _login = new MLogin();


            _login.UserName = "";
            _login.Password = "";

#if Debug
            _login.UserName = "admin";
            _login.Password = "123";
#endif

            return View(_login);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(MLogin _login)
        {

            ManagerInfo cust = ManagerInfoBLL.GetModel(new ManagerInfo { AdminAccount = _login.UserName });

            string username = _login.UserName;
            string passwd = _login.Password;
            string md5pass = GL.Common.Utils.MD5(passwd);

            if (cust == null || string.IsNullOrEmpty(cust.AdminAccount))
            {
                return Json(
                    new { result = Result.UserDoesNotExist }
                );
            }
            else if (md5pass != cust.AdminPasswd.ToLower())
            {
                return Json(
                    new { result = Result.PasswordIsIncorrect }
                );
            }
            else
            {
                cust.AdminMasterRight = masterRight.在线;
                ManagerInfoBLL.UpdateState(cust);
                Rule.Create(cust);


                return Json(
                    new { result = Result.Redirect }
                );
            }



        }

        public ActionResult LogOut()
        {
            Rule.Dispose();
            return RedirectToAction("login");
        }





    }
}