using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GL.Data.DAL;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GL.Common;
using GL.Data.MWeb.Identity;
using MWeb.Models;
using GL.Data.View;
using System.Web.UI;

namespace MWeb.Controllers
{
    [Authorize]
    public class SUController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public SUController()
        {
        }

        public SUController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }



        // GET: SU
        [QueryValues]
        public ActionResult Management(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Management_PageList", SUBLL.GetListByPage(page));
            }
            PagedList<ApplicationUser> model = SUBLL.GetListByPage(page);
            return View(model);
        }



        [QueryValues]
        public ActionResult ManagementForAdd(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            ApplicationUser model = new ApplicationUser();

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [QueryValues]
        public async Task<ActionResult> ManagementForAdd(ApplicationUser model)
        {

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }
            if (string.IsNullOrWhiteSpace(model.NickName))
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }
            //if (!Regex.IsMatch(model.UserName, @"^[a-zA-Z_]\w{3,16}"))
            //{
            //    return Json(new { result = Result.AccountOnlyConsistOfLettersAndNumbers });
            //}


            //ApplicationUser au = await UserManager.FindByNameAsync(model.UserName);
            //if (au != null)
            //{
            //    return Json(new { result = Result.AccountHasBeenRegistered });
            //}

            var user = new ApplicationUser { NickName = model.NickName, UserName = model.UserName.Trim(), Email = "Your@mail.com", PhoneNumber = "13912345678" };

            var result = await UserManager.CreateAsync(user, model.PasswordHash);
            if (result.Succeeded)
            {
                return Json(new { result = Result.Normal });
            }

            return Json(new { result = 4, msg = result.Errors });




            //ManagerInfo mi = ManagerInfoBLL.GetModel(model);
            //if (mi != null)
            //{
            //    return Json(new { result = Result.AccountHasBeenRegistered });
            //}

            //model.AdminMasterRight = 0;
            //model.AdminPasswd = Utils.MD5(model.AdminPasswd);
            //model.CreateDate = DateTime.Now;

            //int result = ManagerInfoBLL.Add(model);
            //if (result > 0)
            //{
            //    return Json(new { result = 0 });
            //}

            //return Json(new { result = 4 });




            //if (ModelState.IsValid)
            //{
            //    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //    var result = await UserManager.CreateAsync(user, model.Password);
            //    if (result.Succeeded)
            //    {
            //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            //        // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
            //        // 发送包含此链接的电子邮件
            //        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //        // await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">這裏</a>来确认你的帐户");

            //        return RedirectToAction("Index", "Home");
            //    }
            //    AddErrors(result);
            //}

            //// 如果我们进行到这一步时某个地方出错，则重新显示表单
            //return View(model);

        }


        [HttpPost]
        [QueryValues]
        public async Task<ActionResult> ManagementForDelete(ApplicationUser model)
        {


            var user = await UserManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var result = await UserManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return Json(new { result = Result.Normal });
                }
                return Json(new { result = 1, msg = result.Errors });
            }
            return Json(new { result = Result.UserDoesNotExist});

        }

        [QueryValues]
        public async Task<ActionResult> ManagementForUpdate(ApplicationUser model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return RedirectToAction("Management");
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [QueryValues]
        public async Task<ActionResult> ManagementForUpdate(Dictionary<string, string> queryvalues)
        {
            string Id = queryvalues.ContainsKey("Id") ? queryvalues["Id"] : "";
            string OldPassword = queryvalues.ContainsKey("OldPassword") ? queryvalues["OldPassword"] : "";
            string NewPassword = queryvalues.ContainsKey("NewPassword") ? queryvalues["NewPassword"] : "";

            var result = await UserManager.ChangePasswordAsync(Id, OldPassword, NewPassword);
            if (result.Succeeded)
            {
                return Json(new { result = Result.Normal });
            }
            return Json(new { result = 4, msg = result.Errors });
        }



        [QueryValues]
      
        public async Task<ActionResult> ManagementForRole(ApplicationUser model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return RedirectToAction("Management");
            }
            SelectUserRolesViewModel viewModel = new SelectUserRolesViewModel(user, RoleManager.Roles);

            viewModel.Roles.RemoveAll(m => m.RoleName == "su");

            return View(viewModel);
        }

        [HttpPost]
       
        [ValidateAntiForgeryToken]
        [QueryValues]
        public async Task<ActionResult> ManagementForRole(Dictionary<string, string> queryvalues, SelectUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                IdentityResult res;
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        if (await UserManager.IsInRoleAsync(user.Id, role.RoleName))
                        {
                           await UserManager.RemoveFromRoleAsync(user.Id, role.RoleName);
                        }

                        res = await UserManager.AddToRoleAsync(user.Id, role.RoleName);
                        if (!res.Succeeded)
                        {
                            return View("Error", res.Errors);
                        }
                    }
                    else
                    {
                        if(await UserManager.IsInRoleAsync(user.Id, role.RoleName)) {
                            res = await UserManager.RemoveFromRoleAsync(user.Id, role.RoleName);
                            if (!res.Succeeded)
                            {
                                return View("Error", res.Errors);
                            }
                        }
                    }
                }
                return RedirectToAction("Management");
            }
            return View(model);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            if (disposing && _roleManager != null)
            {
                _roleManager.Dispose();
                _roleManager = null;
            }

            base.Dispose(disposing);
        }




        // GET: SU
      
        [QueryValues]
        public ActionResult Role(Dictionary<string, string> queryvalues)
        {
            var rolesList = new List<RoleViewModel>();
            foreach (ApplicationRole role in RoleManager.Roles)
            {
                var roleModel = new RoleViewModel(role);
                if (role.Name != "su")
                {
                    rolesList.Add(roleModel);
                }
            }
            return View(rolesList);
        }

      
        public ActionResult RoleForCreate()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<ActionResult> RoleForCreate([Bind(Include = "RoleName, Description")]RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await RoleManager.RoleExistsAsync(model.RoleName))
                {
                    ModelState.AddModelError("", "权限名已被使用");
                }
                else
                {
                    var role = new ApplicationRole(model.RoleName, model.Description);
                    var res = await RoleManager.CreateAsync(role);

                    if (res.Succeeded)
                    {
                        return RedirectToAction("Role", "SU");
                    }
                    AddErrors(res);
                }
            }
            return View();
        }


      
        public async Task<ActionResult> RoleForEdit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Role");
            }
            var role = await RoleManager.FindByIdAsync(id);
            var roleModel = new EditRoleViewModel(role);
            return View(roleModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<ActionResult> RoleForEdit([Bind(Include =
            "RoleID,OriginalRoleName,RoleName,Description")] EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(model.RoleID);
                role.Name = model.RoleName;
                role.Description = model.Description;
                var res = await RoleManager.UpdateAsync(role);
                if (res.Succeeded)
                {
                    return RedirectToAction("Role", "SU");
                }
                AddErrors(res);
            }
            return View(model);
        }


       
        [HttpPost]
        public async Task<ActionResult> RoleForDelete(string id)
        {
            if (id == null)
            {
                return Json(new { result = 1 });
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return Json(new { result = 2, msg = "找不到所要删除的权限" });
            }
            var res = await RoleManager.DeleteAsync(role);
            if (res.Succeeded)
            {
                return Json(new { result = 0});
            }
            return Json(new { result = 2, msg = res.Errors });
        }


        public ActionResult Resource() {
            return View();
        }

     
        public ActionResult SetRoleResource(string id) {

            ResourceView model = new ResourceView();

            IEnumerable<Resource> resList = SUBLL.GetResourceListByRoleId(id);
            model.DataList = resList;
            model.RoleID = id;
            return View(model);
        }

        [QueryValues]
        public ActionResult SaveRoleResource(Dictionary<string, string> queryvalues)
        {
            string roleid = queryvalues.ContainsKey("roleid") ? queryvalues["roleid"] : "";
            string no = queryvalues.ContainsKey("checkbox-inline") ? queryvalues["checkbox-inline"] : "";

            int res = SUBLL.AddRoleResource(roleid, no);

            return Redirect("/SU/Role");
        }



        public ActionResult SetUserResource(string id) {
            ResourceView model = new ResourceView();

            IEnumerable<Resource> resList = SUBLL.GetResourceListByUserId(id);
            model.DataList = resList;
            model.UserID = id;
            return View(model);
        }

        [QueryValues]
        public ActionResult SaveUserResource(Dictionary<string, string> queryvalues)
        {
            string userid = queryvalues.ContainsKey("userid") ? queryvalues["userid"] : "";
            string no = queryvalues.ContainsKey("checkbox-inline") ? queryvalues["checkbox-inline"] : "";

            int res = SUBLL.AddUserResource(userid, no);

            return Redirect("/SU/Management");
        }

        [QueryValues]
        public ActionResult GetPowerList(Dictionary<string, string> queryvalues) {
            string seachtype = queryvalues.ContainsKey("seachtype") ? queryvalues["seachtype"] : "";
            string Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";
            ViewData["seachtype"] = seachtype;
            ViewData["Value"] = Value;
            ResourceView model = new ResourceView();
            IEnumerable<Resource> resList = new List<Resource>();
            if (seachtype == "1")
            {//角色查询
                resList = SUBLL.GetResourceListByRoleName(Value.Trim());
            }
            else if (seachtype == "2")
            {//用户查询

                if (Value.Trim() == "admin")
                {
                    resList = SUBLL.GetAdminResourceList();
                   
                }
                else {
                    resList = SUBLL.GetUserRoleResourceListByUserId(Value.Trim());
                }

               
            }
            else {

            }
            model.DataList = resList;
            model.UserID = Value;
            return View(model);
        }


        [QueryValues]
        public ActionResult SysEmailDefaultLimit(Dictionary<string, string> queryvalues) {
            return View();
        }

        [QueryValues]
        public ActionResult SetSysEmailLimit(Dictionary<string, string> queryvalues)
        {


            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            if (Request.IsAjaxRequest())
            {
                return PartialView("SetSysEmailLimit_PageList", SUBLL.GetListByPage(page));
            }
            PagedList<ApplicationUser> model = SUBLL.GetListByPage(page);
            return View(model);

        
        }

        [OutputCache(Duration = 0, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = false)]
        public ActionResult SetSysEmailLimitForUpdate(string id) {

            ViewData["id"] = id;
            UserLimit limit = SUBLL.GetLimitModel(new UserLimit {  Category=1, UserId=id});
            Dictionary<int, string> dic = new Dictionary<int, string>();
         
            if (limit == null|| string.IsNullOrEmpty( limit.AccessNo))
            {
                limit = new UserLimit();
                limit.AccessNo = ",";
                dic.Add(1, "");
                dic.Add(2,"");
                dic.Add(3,"");
                dic.Add(4, "");
                dic.Add(5,"");
                dic.Add(6, "");
            }
            else {
                string lim = limit.AccessNo;

                // 1:1,2:22,3:,4:
                string[] strs = lim.Split(',');

                for (int i = 0; i < strs.Length; i++) {
                    string[] s = strs[i].Split(':');
                    dic.Add(Convert.ToInt32(s[0]),s[1]);
                }




                limit.AccessNo = "," + limit.AccessNo.Trim(',');
            }

            ViewData["dic"] = dic;

            return View(limit);
        }

       
        [OutputCache(Duration = 0, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = false)]
        public ActionResult SetGlobalEmailLimit()
        {

          
            UserLimit limit = SUBLL.GetLimitModel(new UserLimit { Category =3, UserId = "0" });
            if (limit == null) {
                limit = new UserLimit() {
                    AccessNo = "20000,20,20000"
                };
            }

            return View(limit);
        }

        [QueryValues]
        public ActionResult SaveGlobalEmailLimit(Dictionary<string, string> queryvalues)
        {
            string gold = queryvalues.ContainsKey("gold") ? queryvalues["gold"] : "";
            string wubi = queryvalues.ContainsKey("wubi") ? queryvalues["wubi"] : "";
            string jifen = queryvalues.ContainsKey("jifen") ? queryvalues["jifen"] : "";
            //   1,2,4,5
            //   1,2,,1,
            string saveStr = "";
            try
            {
                if (!string.IsNullOrEmpty(gold))
                {
                    Convert.ToInt32(gold);
                    saveStr += (gold + ",");
                }
                else {
                    saveStr += (20000 + ",");
                }
                if (!string.IsNullOrEmpty(wubi))
                {
                    Convert.ToInt32(wubi);
                    saveStr += (wubi + ",");
                }
                else {
                    saveStr += (20 + ",");
                }
                if (!string.IsNullOrEmpty(jifen))
                {
                    Convert.ToInt32(jifen);
                    saveStr += (jifen + ",");
                }
                else {
                    saveStr += (20000 + ",");
                }
            }
            catch {
                return Content("-1");
            }

            saveStr = saveStr.Trim(',');

           bool res = SUBLL.AddUserLimit("0", saveStr, 3);

            if (res)
            {
                return Content("1");
            }
            else {
                return Content("0");
            }

           
        }



        [QueryValues]
        public ActionResult SaveSysEmailUpdate(Dictionary<string, string> queryvalues) {
            string checkbox = queryvalues.ContainsKey("checkbox") ? queryvalues["checkbox"] : "";
            string numLimit = queryvalues.ContainsKey("faqtitle") ? queryvalues["faqtitle"] : "";
            string id = queryvalues.ContainsKey("Id") ? queryvalues["Id"] : "";
            //   1,2,4,5
            //   1,2,,1,
            numLimit += ',';
            string[] checkboxS = checkbox.Split(',');
            string[] numLimits = numLimit.Split(',');
            string saveStr = "";
            if (checkbox != "")
            {
                for (int i = 0; i < checkboxS.Length; i++)
                {
                    saveStr += checkboxS[i] + ":" + numLimits[Convert.ToInt32(checkboxS[i]) - 1] + ",";
                }
            }

            saveStr = saveStr.Trim(',');

            bool res = SUBLL.AddUserLimit(id, saveStr, 1);



            return Redirect("/SU/SetSysEmailLimit");
        }


        [QueryValues]
        public ActionResult OperLog(Dictionary<string, string> queryvalues)
        {

            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            string _UserAccount = queryvalues.ContainsKey("UserAccount") ? queryvalues["UserAccount"] : "";



            BaseDataView vbd = new BaseDataView { SearchExt = _UserAccount, Page = _page, StartDate = _StartDate, ExpirationDate = _ExpirationDate };


            IEnumerable<AspNetUser> users = SUBLL.GetAspNetUsers();
            List<SelectListItem> ieList = new List<SelectListItem>();
            ieList.Insert(0, new SelectListItem { Text = "所有账号", Value = "", Selected = "" == _UserAccount });
            int i = 1;
            foreach (var item in users)
            {
                if (string.IsNullOrEmpty(item.NickName))
                {
                    ieList.Insert(i++, new SelectListItem { Text = item.UserName, Value = item.UserName, Selected = item.UserName == _UserAccount });
                }
                else
                {
                    ieList.Insert(i++, new SelectListItem { Text = item.UserName + "(" + item.NickName + ")", Value = item.UserName, Selected = item.UserName == _UserAccount });
                }

            }


            ViewData["UserAccount"] = ieList;

            //通过时间查询role列表




            vbd.BaseDataList = SUBLL.GetLogListByPage(vbd);
            if (Request.IsAjaxRequest())
            {
                return PartialView("OperLog_PageList", vbd.BaseDataList);
            }
            return View(vbd);


        }


    }
}