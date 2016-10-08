using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AWeb.Models;
using GL.Data.AWeb.Identity;
using GL.Data.BLL;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;
using GL.Data.Model;
using GL.Common;
using GL.Data;

namespace AWeb.Controllers
{
    public class AgentController : Controller
    {
        private AgentUserManager _userManager;
        private AgentRoleManager _roleManager;

        public AgentController()
        {
        }

        public AgentController(AgentUserManager userManager, AgentRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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

        public AgentRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<AgentRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }



        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        [QueryValues]
        // GET: Agent
        public ActionResult AgentList(Dictionary<string, string> queryvalues)
        {


            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;

            var userid = User.Identity.GetUserId<int>();
            if (!AgentUserBLL.CheckUser(userid, id))
            {
                id = userid;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("AgentList_PageList", AgentUserBLL.GetListByPage(page, id));
            }

            AgentUser mi = UserManager.FindById(id);
            PagedList<AgentUser> model = AgentUserBLL.GetListByPage(page, id);
            if (mi != null)
            {
                if (mi.AgentLv == agentLv.代理)
                {
                    return Redirect("/Manage/Member?id=" + id);
                }

                ViewBag.AgentID = mi.Id;
                ViewBag.AgentLv = mi.AgentLv + 1;
                ViewBag.HigherID = mi.HigherLevel;
                //ViewBag.Top = mi != null;
                //ViewData["Higher"] = mi;

                return View(model);
            }
            return View(model);

        }

        //[QueryValues]
        //// GET: Agent
        //public ActionResult GetAgentList(Dictionary<string, string> queryvalues, jQueryDataTableParamModel param)
        //{
        //    int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
        //    int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;

        //    var user = UserManager.FindById(User.Identity.GetUserId<int>());

        //    if (!AgentUserBLL.CheckUser(user.Id, id))
        //    {
        //        id = user.Id;
        //    }

        //    //AgentInfo mi = AgentInfoBLL.GetModelByID(new AgentInfo { AgentID = id });
        //    //AgentUser mi = UserManager.FindById(id);
        //    PagerQuery pagerQuery = new PagerQuery();
        //    IEnumerable<AgentUser> agentUserList = AgentUserBLL.GetListByPage(param.iDisplayStart, param.iDisplayLength, user.Id, id, out pagerQuery);
        //    //if (mi != null)
        //    //{
        //    //    if (mi.AgentLv == agentLv.代理)
        //    //    {
        //    //        return Redirect("/Manage/Member?id=" + id);
        //    //    }
        //    //    ViewBag.AgentLv = mi.AgentLv + 1;
        //    //    ViewBag.HigherID = mi.HigherLevel;
        //    //    ViewBag.Top = true;
        //    //    ViewData["Higher"] = mi;
        //    //    return View(model);
        //    //}


        //    return Json(new
        //    {
        //        sEcho = param.sEcho,
        //        iTotalRecords = pagerQuery.RecordCount,
        //        iTotalDisplayRecords = pagerQuery.RecordCount,
        //        aaData = agentUserList.Select(x=> new { ID = x.Id, UserName = x.UserName, UserNick = x.AgentName, Lv = x.AgentLv.ToString(), Money = x.AmountAvailable, Phone = x.PhoneNumber })
        //    },
        //    JsonRequestBehavior.AllowGet);

        //}

        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,CPS,CPA,Guest,策划")]
        public async Task<ActionResult> AgentForUpdate(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("Id") ? Convert.ToInt32(queryvalues["Id"]) : 0;
            var userid = User.Identity.GetUserId<int>();
            if (!AgentUserBLL.CheckUser(userid, id))
            {
                id = userid;
            }


            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("AgentInfo");
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,CPS,CPA,Guest,策划")]
        public async Task<ActionResult> AgentForUpdate(Dictionary<string, string> queryvalues, AgentUser model)
        {
            int Id = queryvalues.ContainsKey("Id") ?  Convert.ToInt32(queryvalues["Id"]) : 0;
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
        [Authorize(Roles = "SU")]
        public async Task<ActionResult> AgentForRole(AgentUser model)
        {
            ViewData["ReturnUrl"] = Utils.GetUrlReferrer();
            var userid = User.Identity.GetUserId<int>();
            if (!AgentUserBLL.CheckUser(userid, model.Id))
            {
                return RedirectToAction("AgentList");
            }
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return RedirectToAction("AgentList");
            }
            var viewModel = new SelectUserRolesViewModel(user, RoleManager.Roles);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "SU")]
        [ValidateAntiForgeryToken]
        [QueryValues]
        public async Task<ActionResult> AgentForRole(Dictionary<string, string> queryvalues, SelectUserRolesViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                IdentityResult res;
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        if (!await UserManager.IsInRoleAsync(user.Id, role.RoleName))
                        {
                            res = await UserManager.AddToRoleAsync(user.Id, role.RoleName);
                            if (!res.Succeeded)
                            {
                                AddErrors(res);
                                return View(model);
                            }
                        }
                    }
                    else
                    {
                        if (await UserManager.IsInRoleAsync(user.Id, role.RoleName))
                        {
                            res = await UserManager.RemoveFromRoleAsync(user.Id, role.RoleName);
                            if (!res.Succeeded)
                            {
                                AddErrors(res);
                                return View(model);
                            }
                        }
                    }
                }
                return Redirect(ReturnUrl);
            }
            return View(model);
        }





        // GET: SU
        [Authorize(Roles = "SU")]
        [QueryValues]
        public ActionResult Role(Dictionary<string, string> queryvalues)
        {
            var rolesList = new List<RoleViewModel>();
            foreach (AgentRole role in RoleManager.Roles)
            {
                var roleModel = new RoleViewModel(role);
                rolesList.Add(roleModel);
            }
            return View(rolesList);
        }


        [Authorize(Roles = "SU")]
        public ActionResult RoleForCreate()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "SU")]
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
                    var role = new AgentRole(model.RoleName, model.Description);
                    var res = await RoleManager.CreateAsync(role);

                    if (res.Succeeded)
                    {
                        return RedirectToAction("Role", "Agent");
                    }
                    AddErrors(res);
                }
            }
            return View();
        }


        [Authorize(Roles = "SU")]
        public async Task<ActionResult> RoleForEdit(int id)
        {
            if (id<=0)
            {
                return RedirectToAction("Role");
            }
            var role = await RoleManager.FindByIdAsync(id);
            var roleModel = new EditRoleViewModel(role);
            return View(roleModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SU")]
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
                    return RedirectToAction("Role", "Agent");
                }
                AddErrors(res);
            }
            return View(model);
        }


        [Authorize(Roles = "SU")]
        [HttpPost]
        public async Task<ActionResult> RoleForDelete(int id)
        {
            if (id == 0)
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
                return Json(new { result = 0 });
            }
            return Json(new { result = 2, msg = res.Errors });
        }


    }
}