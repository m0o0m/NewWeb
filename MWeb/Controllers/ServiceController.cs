using GL.Data.BLL;
using GL.Data.DAL;
using GL.Data.Model;
using GL.Data.OWeb.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace MWeb.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {

        private CustomerUserManager _customeruserManager;

        public ServiceController() : this(new CustomerUserManager(new CustomerUserStore<CustomerUser, CustomerRole, string, CustomerUserLogin, CustomerUserRole, CustomerUserClaim>(new CustomerDbContext())))
        {

        }

        public ServiceController(CustomerUserManager userManager)
        {
            UserManager = userManager;
        }

        public CustomerUserManager UserManager
        {
            get
            {

                return _customeruserManager;
            }
            private set
            {
                _customeruserManager = value;
            }
        }






        [QueryValues]
        public ActionResult CustomerInfo(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;


            //ar query = _accountManager.UserManager.Users;
            //var total = await query.CountAsync();
            //var users = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            if (Request.IsAjaxRequest())
            {
                return PartialView("CustomerInfo_PageList", CustomerUserBLL.GetListByPage(page));
            }

            PagedList<CustomerUser> model = CustomerUserBLL.GetListByPage(page);

            return View(model);


        }
        

       [QueryValues]
       [HttpPost]
        public async Task<ActionResult> CustomerForDelete(Dictionary<string, string> queryvalues, CustomerUser cu)
        {

            //int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            //int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            CustomerUser user = await UserManager.FindByIdAsync(cu.Id);
            if (user != null)
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { result = 0 });
                }
                return Json(new { result = 1 });
            }
            return Json(new { result = Result.UserDoesNotExist });


        }

        [QueryValues]
        public ActionResult CustomerForAdd()
        {

            CustomerUser model = new CustomerUser();

            model.UserName = "service00";
            model.NickName = "客服00";
            model.PhoneNumber = "13800000000";
            model.Email = "support@515.com";


            return View(model);
        }

        [QueryValues]
        [HttpPost]
        public async Task<ActionResult> CustomerForAdd(Dictionary<string, string> queryvalues, CustomerUser cu)
        {

            string confirmPasswd = queryvalues.ContainsKey("ConfirmPasswd") ? queryvalues["ConfirmPasswd"] : "";

            var user = await UserManager.FindByNameAsync(cu.UserName);
            if (user==null)
            {
                var result = await UserManager.CreateAsync(cu, confirmPasswd);

                if (result.Succeeded)
                {
                    return Json(new { result = 0 });
                }
                return Json(new { result = 1, msg = result.Errors });
            }
            return Json(new { result = Result.AccountHasBeenRegistered });


            //return Json(new { Result = 10000 });
            //return View(model);
        }

        [QueryValues]
        public async Task<ActionResult> CustomerForUpdate( CustomerUser cu)
        {

            var user = await UserManager.FindByIdAsync(cu.Id);

            if (user == null)
            {
                return RedirectToAction("CustomerInfo");
            }

            return View(user);
        }

        [QueryValues]
        [HttpPost]
        public async Task<ActionResult> CustomerForUpdate(Dictionary<string, string> queryvalues, CustomerUser cu)
        {

            string confirmPasswd = queryvalues.ContainsKey("ConfirmPasswd") ? queryvalues["ConfirmPasswd"] : "";

            var user = await UserManager.FindByIdAsync(cu.Id);
            if (user != null)
            {
                user.NickName = cu.NickName;
                user.Email = cu.Email;
                user.PhoneNumber = cu.PhoneNumber;
                if (!string.IsNullOrWhiteSpace(confirmPasswd))
                {
                    string NewPassword = UserManager.PasswordHasher.HashPassword(confirmPasswd);

                    var Store = new CustomerUserStore<CustomerUser, CustomerRole, string, CustomerUserLogin, CustomerUserRole, CustomerUserClaim>(new CustomerDbContext());
                    await Store.SetPasswordHashAsync(user, NewPassword);

                }
                
                //await UserManager.PasswordHashAsync(user, user.PasswordHash);

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { result = 0 });
                }
                return Json(new { result = 1, msg = result.Errors });

            }

            return Json(new { result = Result.UserDoesNotExist });
        }



        [QueryValues]
        public ActionResult OnlineProblem(Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            ViewData["StartDate"] = _StartDate;
            ViewData["ExpirationDate"] = _ExpirationDate;
            BaseDataView bdv = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page= page };

            if (queryvalues.ContainsKey("StartDate")==false)
            {
                
                PagedList<CustomerServCenter> model2 = new PagedList<CustomerServCenter>(new List<CustomerServCenter>(), 1, 1);
                bdv.BaseDataList = model2;
                return View(bdv);
            }


            if (Request.IsAjaxRequest())
            {
                return PartialView("OnlineProblem_PageList", CustomerServCenterBLL.GetListByPage(bdv));
            }


            PagedList<CustomerServCenter> model = CustomerServCenterBLL.GetListByPage(bdv);
            bdv.BaseDataList = model;
            return View(bdv);
        }


        [QueryValues]
        public ActionResult OnlineProblemForReply(Dictionary<string, string> queryvalues)
        {

            int CSCMainID = queryvalues.ContainsKey("CSCMainID") ? Convert.ToInt32(queryvalues["CSCMainID"]) : 0;
            if (CSCMainID > 0)
            {
                IEnumerable<CustomerServCenter> result = CustomerServCenterBLL.GetListWithCSCSubId(new CustomerServCenter { CSCMainID = CSCMainID });

                return View(result);

            }
            return View();
        }




        [QueryValues]
        public ActionResult CommonProblems(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string leftMenuMark = queryvalues.ContainsKey("leftMenuMark") ? queryvalues["leftMenuMark"] : string.Empty;
            if (leftMenuMark != "leftMenu")
            {


                if (Request.IsAjaxRequest())
                {
                    return PartialView("CommonProblems_PageList", FAQBLL.GetListByPage(page));
                }


                PagedList<FAQ> model = FAQBLL.GetListByPage(page);
                return View(model);

            }
            else
            {


                PagedList<FAQ> model = FAQBLL.GetListByPage(page);
                return View(model);
            }


        
        }

        [QueryValues]
        public ActionResult CommonProblemsForAdd(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;




            FAQ model = new FAQ();


            return View(model);

        }


        [QueryValues]
        public ActionResult CommonProblemsForUpdate(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;




            FAQ model = FAQBLL.GetModelByID(new FAQ { Id = id });
            if (model == null)
            {
                return RedirectToAction("CommonProblems");
            }


            return View(model);

        }

    }
}