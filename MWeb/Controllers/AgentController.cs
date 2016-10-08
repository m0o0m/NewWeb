using GL.Common;
using GL.Data;
using GL.Data.AWeb.Identity;
using GL.Data.BLL;
using GL.Data.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace MWeb.Controllers
{
    [Authorize]
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

        // GET: Agent

        [QueryValues]
        public ActionResult Agent(Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;

            //var userid = User.Identity.GetUserId<int>();
            //if (!AgentUserBLL.CheckUser(userid, id))
            //{
            //    id = userid;
            //}

            if (Request.IsAjaxRequest())
            {
                return PartialView("Agent_PageList", AgentUserBLL.GetListByPage(page, id));
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


        [HttpPost]
        [QueryValues]
        public ActionResult AgentForDelete(Dictionary<string, string> queryvalues, AgentInfo model)
        {

            if (AgentInfoBLL.GetRecordCount(model.AgentID) > 0)
            {
                return Json(new { result = Result.AccountOfTheLowerAgentMustBeEmpty });
            }
            if (RoleBLL.GetRecordCount(model.AgentID) > 0)
            {
                return Json(new { result = Result.AccountOfTheLowerMemberMustBeEmpty });
            }



            AgentInfo mi = AgentInfoBLL.GetModelByID(model);


            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo { AgentID = mi.HigherLevel.Value });
            Higher.AmountAvailable = Higher.AmountAvailable + mi.AmountAvailable;
            Higher.HavaAmount = Higher.HavaAmount - mi.AmountAvailable;

            AgentInfoBLL.Update(Higher);
            int result = AgentInfoBLL.Delete(model);
            if (result > 0)
            {
                return Json(new { result = 0 });
            }
            return Json(new { result = 1 });
        }



        [QueryValues]
        public ActionResult AgentForAdd(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;

            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = id });
            if (Higher == null)
            {
                ViewBag.AgentLv = agentLv.公司;
                ViewBag.HigherID = 0;
                ViewBag.Top = true;
            }
            else
            {
                ViewBag.HigherID = Higher.AgentID;
                ViewBag.AgentLv = Higher.AgentLv + 1;
                ViewBag.Top = false;
            }

            ViewData["Higher"] = Higher;

            AgentInfo model = new AgentInfo();


            model.DrawingPasswd = "0000";
            model.EarningsRatio = 95;
            model.RebateRate = 20;

            return View(model);
        }

        [QueryValues]
        [HttpPost]
        public ActionResult AgentForAdd(Dictionary<string, string> queryvalues, AgentInfo model)
        {
            if (string.IsNullOrWhiteSpace(model.AgentAccount) || string.IsNullOrWhiteSpace(model.AgentName) || string.IsNullOrWhiteSpace(model.AgentPasswd) || string.IsNullOrWhiteSpace(model.AgentQQ) || string.IsNullOrWhiteSpace(model.AgentEmail) || string.IsNullOrWhiteSpace(model.AgentTel))
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }

            if (model.InitialAmount == null || model.InitialAmount < 0)
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }



            if (!Regex.IsMatch(model.AgentAccount, @"^[a-zA-Z_]\w{3,16}"))
            {
                return Json(new { result = Result.AccountOnlyConsistOfLettersAndNumbers });
            }
            if (!Regex.IsMatch(model.AgentName, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,16}"))
            {
                return Json(new { result = Result.AccountOnlyConsistOfLettersAndNumbers });
            }
            //if (!Regex.IsMatch(model.AgentName, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,16}"))
            //{
            //    return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            //}

            if (!Regex.IsMatch(model.AgentQQ, @"^\w{5,20}"))
            {
                return Json(new { result = Result.AccountOnlyConsistOfLettersAndNumbers });
            }

            if (!Regex.IsMatch(model.AgentTel, @"^(((13[0-9]{1})|(15[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})"))
            {
                return Json(new { result = Result.PhoneIsWrong });
            }

            if (!Regex.IsMatch(model.AgentEmail, @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))"))
            {
                return Json(new { result = Result.EmailIsWrong });
            }

            AgentInfo mi = AgentInfoBLL.GetModel(model);
            if (mi != null)
            {
                return Json(new { result = Result.AccountHasBeenRegistered });
            }

            model.AgentState = 0;
            model.OnlineState = 0;
            model.AgentPasswd = Utils.MD5(model.AgentPasswd);
            model.Deposit = 0;
            model.AgentLv = agentLv.公司;

            model.AmountAvailable = model.InitialAmount;
            model.HavaAmount = 0;

            model.Drawing = 0;
            model.DrawingPasswd = Utils.MD5(model.DrawingPasswd);

            model.LoginIP = "127.0.0.1";
            model.LoginTime = DateTime.MinValue;
            model.LowerLevel = 0;
            model.Recharge = 0;
            model.RegisterTime = DateTime.Now;
            model.JurisdictionID = string.Empty;



            if (model.HigherLevel != null)
            {
                AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo { AgentID = model.HigherLevel.Value });


                if (Higher.AmountAvailable < model.InitialAmount)
                {
                    return Json(new { result = Result.ValueIsTooBiger, value = Higher.AmountAvailable });
                }

                Higher.AmountAvailable = Higher.AmountAvailable - model.InitialAmount;
                Higher.HavaAmount = Higher.HavaAmount + model.AmountAvailable;

                model.AgentLv = Higher.AgentLv + 1;

                model.EarningsRatio = model.EarningsRatio > Higher.EarningsRatio ? Higher.EarningsRatio : model.EarningsRatio;
                model.RebateRate = model.RebateRate > Higher.RebateRate ? Higher.RebateRate : model.RebateRate;


                AgentInfoBLL.Update(Higher);

            }
            else
            {
                model.HigherLevel = 0;
            }




            int result = AgentInfoBLL.Add(model);
            if (result > 0)
            {
                return Json(new { result = 0 });
            }

            return Json(new { result = 4 });
        }




        [QueryValues]
        public ActionResult AgentForUpdate(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            AgentInfo model = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = id });
            if (model == null)
            {
                return RedirectToAction("Agent");
            }

            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = model.HigherLevel.Value });
            if (Higher == null)
            {
                ViewBag.AgentLv = agentLv.公司;
                ViewBag.HigherID = 0;
                ViewBag.Top = true;
            }
            else
            {
                ViewBag.HigherID = Higher.AgentID;
                ViewBag.AgentLv = Higher.AgentLv + 1;
                ViewBag.Top = false;
            }

            ViewData["Higher"] = Higher;


            AgentInfo Lower = AgentInfoBLL.GetModelByIDForLower(new AgentInfo() { AgentID = model.AgentID });

            if (Lower == null)
            {
                ViewBag.Low = true;
            }
            else
            {
                ViewBag.Low = false;
            }

            ViewData["Lower"] = Lower;
            model.DrawingPasswd = "";


            return View(model);
        }


        [QueryValues]
        [HttpPost]
        public ActionResult AgentForUpdate(Dictionary<string, string> queryvalues, AgentInfo model)
        {

            if (string.IsNullOrWhiteSpace(model.AgentName) || string.IsNullOrWhiteSpace(model.AgentQQ) || string.IsNullOrWhiteSpace(model.AgentEmail) || string.IsNullOrWhiteSpace(model.AgentTel))
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }


            if (!Regex.IsMatch(model.AgentName, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,16}"))
            {
                return Json(new { result = Result.AccountOnlyConsistOfLettersAndNumbers });
            }
            //if (!Regex.IsMatch(model.AgentName, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,16}"))
            //{
            //    return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            //}

            if (!Regex.IsMatch(model.AgentQQ, @"^\w{5,20}"))
            {
                return Json(new { result = Result.AccountOnlyConsistOfLettersAndNumbers });
            }

            if (!Regex.IsMatch(model.AgentTel, @"^(((13[0-9]{1})|(15[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})"))
            {
                return Json(new { result = Result.PhoneIsWrong });
            }

            if (!Regex.IsMatch(model.AgentEmail, @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))"))
            {
                return Json(new { result = Result.EmailIsWrong });
            }

            AgentInfo mi = AgentInfoBLL.GetModelByID(model);

            if (!string.IsNullOrWhiteSpace(model.AgentPasswd) && mi.AgentPasswd != Utils.MD5(model.AgentPasswd))
            {
                mi.AgentPasswd = Utils.MD5(model.AgentPasswd);
            }
            if (!string.IsNullOrWhiteSpace(model.DrawingPasswd) && mi.DrawingPasswd != Utils.MD5(model.DrawingPasswd))
            {
                mi.DrawingPasswd = Utils.MD5(model.DrawingPasswd);
            }




            //mi.AgentAccount = model.AgentAccount;
            mi.AgentName = model.AgentName;
            //mi.AgentLv = model.AgentLv;
            mi.AgentQQ = model.AgentQQ;
            mi.AgentEmail = model.AgentEmail;
            mi.AgentTel = model.AgentTel;
            //mi.InitialAmount = model.InitialAmount;
            mi.RevenueModel = model.RevenueModel;
            mi.EarningsRatio = model.EarningsRatio;
            mi.RebateRate = model.RebateRate;

            //model.AgentState = 0;
            //model.OnlineState = 0;
            //model.AgentPasswd = Utils.MD5(model.AgentPasswd);
            //model.HigherLevel = 0;
            //model.Deposit = 0;

            //model.AmountAvailable = 0;
            //model.HavaAmount = 0;

            //model.Drawing = 0;
            //model.DrawingPasswd = Utils.MD5(model.DrawingPasswd);

            //model.LoginIP = "127.0.0.1";
            //model.LoginTime = DateTime.MinValue;
            //model.LowerLevel = 0;
            //model.Recharge = 0;
            //model.RegisterTime = DateTime.Now;
            //model.JurisdictionID = string.Empty;


            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = mi.HigherLevel.Value });
            AgentInfo Lower = AgentInfoBLL.GetModelByIDForLower(new AgentInfo() { AgentID = mi.AgentID });

            if (Higher != null)
            {
                if (Higher.EarningsRatio < mi.EarningsRatio)
                {
                    return Json(new { result = Result.BeyondTheScopeOfNumerical });
                }
                if (Higher.RebateRate < mi.RebateRate)
                {
                    return Json(new { result = Result.BeyondTheScopeOfNumerical });
                }

            }


            if (Lower != null)
            {
                if (Lower.EarningsRatio > mi.EarningsRatio)
                {
                    return Json(new { result = Result.BeyondTheScopeOfNumerical });
                }
                if (Lower.RebateRate > mi.RebateRate)
                {
                    return Json(new { result = Result.BeyondTheScopeOfNumerical });
                }
            }





            int result = AgentInfoBLL.Update(mi);
            if (result > 0)
            {
                return Json(new { result = 0 });
            }

            return Json(new { result = 4 });

        }


        [QueryValues]
        public ActionResult AgentForUpperScores(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            AgentInfo model = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = id });
            if (model == null)
            {
                return RedirectToAction("Agent");
            }

            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = model.HigherLevel.Value });
            if (Higher == null)
            {
                ViewBag.AgentLv = agentLv.公司;
                ViewBag.HigherID = 0;
                ViewBag.Top = true;
            }
            else
            {
                ViewBag.HigherID = Higher.AgentID;
                ViewBag.AgentLv = Higher.AgentLv + 1;
                ViewBag.Top = false;
            }
            ViewData["Higher"] = Higher;

            return View(model);
        }

        [QueryValues]
        [HttpPost]
        public ActionResult AgentForUpperScores(Dictionary<string, string> queryvalues, AgentInfo model)
        {

            if (model.AmountAvailable == 0)
            {
                return Json(new { result = 0 });
            }
            if (model.AmountAvailable < 0)
            {
                return Json(new { result = Result.BeyondTheScopeOfNumerical });
            }

            AgentInfo mi = AgentInfoBLL.GetModelByID(model);
            if (mi == null)
            {
                return Json(new { result = Result.UnknownError });
            }


            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = mi.HigherLevel.Value });

            if (Higher != null)
            {

                if (model.LowerLevel == 0)
                {
                    if (Higher.AmountAvailable < model.AmountAvailable)
                    {
                        return Json(new { result = Result.BeyondTheScopeOfNumerical });
                    }
                }
                else
                {
                    if (mi.AmountAvailable < model.AmountAvailable)
                    {
                        return Json(new { result = Result.BeyondTheScopeOfNumerical });
                    }
                    model.AmountAvailable = decimal.Negate(model.AmountAvailable.Value);
                }


                Higher.AmountAvailable = Higher.AmountAvailable - model.AmountAvailable;
                Higher.HavaAmount = Higher.HavaAmount + model.AmountAvailable;


                AgentInfoBLL.Update(Higher);
            }
            else
            {
                if (model.LowerLevel != 0)
                {
                    if (mi.AmountAvailable < model.AmountAvailable)
                    {
                        return Json(new { result = Result.BeyondTheScopeOfNumerical });
                    }
                    model.AmountAvailable = decimal.Negate(model.AmountAvailable.Value);
                }
            }


            mi.AmountAvailable = mi.AmountAvailable + model.AmountAvailable;
            int result = AgentInfoBLL.Update(mi);
            if (result > 0)
            {
                return Json(new { result = 0 });
            }
            return Json(new { result = 1 });
        }


        [QueryValues]
        public ActionResult AgentForLowerScores(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            AgentInfo model = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = id });
            if (model == null)
            {
                return RedirectToAction("Agent");
            }

            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = model.HigherLevel.Value });
            if (Higher == null)
            {
                ViewBag.AgentLv = agentLv.公司;
                ViewBag.HigherID = 0;
                ViewBag.Top = true;
            }
            else
            {
                ViewBag.HigherID = Higher.AgentID;
                ViewBag.AgentLv = Higher.AgentLv + 1;
                ViewBag.Top = false;
            }
            ViewData["Higher"] = Higher;

            return View(model);
        }

        [QueryValues]
        [HttpPost]
        public ActionResult AgentForLowerScores(Dictionary<string, string> queryvalues, AgentInfo model)
        {

            if (model.AmountAvailable == 0)
            {
                return Json(new { result = 0 });
            }
            if (model.AmountAvailable < 0)
            {
                return Json(new { result = Result.BeyondTheScopeOfNumerical });
            }

            AgentInfo mi = AgentInfoBLL.GetModelByID(model);
            if (mi == null)
            {
                return Json(new { result = Result.UnknownError });
            }


            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = mi.HigherLevel.Value });

            if (Higher != null)
            {

                if (model.LowerLevel == 0)
                {
                    if (Higher.AmountAvailable < model.AmountAvailable)
                    {
                        return Json(new { result = Result.BeyondTheScopeOfNumerical });
                    }
                }
                else
                {
                    if (mi.AmountAvailable < model.AmountAvailable)
                    {
                        return Json(new { result = Result.BeyondTheScopeOfNumerical });
                    }
                    model.AmountAvailable = decimal.Negate(model.AmountAvailable.Value);
                }


                Higher.AmountAvailable = Higher.AmountAvailable - model.AmountAvailable;
                Higher.HavaAmount = Higher.HavaAmount + model.AmountAvailable;


                AgentInfoBLL.Update(Higher);
            }
            else
            {
                if (model.LowerLevel != 0)
                {
                    if (mi.AmountAvailable < model.AmountAvailable)
                    {
                        return Json(new { result = Result.BeyondTheScopeOfNumerical });
                    }
                    model.AmountAvailable = decimal.Negate(model.AmountAvailable.Value);
                }
            }


            mi.AmountAvailable = mi.AmountAvailable + model.AmountAvailable;
            int result = AgentInfoBLL.Update(mi);
            if (result > 0)
            {
                return Json(new { result = 0 });
            }
            return Json(new { result = 1 });
        }



        [QueryValues]
        public ActionResult AgentForSetTheMain(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;

            List<SelectListItem> model = AgentInfoBLL.GetAgentList().Select(x => new SelectListItem { Text = x.AgentAccount, Value = x.AgentID.ToString(), Selected = x.Extend_isDefault }).ToList();

            return View(model);
        }


        [HttpPost]
        [QueryValues]
        public ActionResult AgentForSetTheMain(Dictionary<string, string> queryvalues, AgentInfo model)
        {

            AgentInfo mi = AgentInfoBLL.GetModelByID(model);
            if (mi != null)
            {

                AgentInfoBLL.ResetDifaultMain();
                mi.Extend_isDefault = true;
                int result = AgentInfoBLL.Update(mi);
                if (result > 0)
                {
                    return Json(new { result = 0 });
                }

            }
            return Json(new { result = 1 });
        }


    }
}