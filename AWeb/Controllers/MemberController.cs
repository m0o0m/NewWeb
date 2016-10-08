using GL.Data.AWeb.Identity;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace AWeb.Controllers
{
    public class MemberController : Controller
    {
        private AgentUserManager _userManager;

        public MemberController()
        {
        }
        public MemberController(AgentUserManager userManager)
        {
            UserManager = userManager;
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




        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult MemberForSearchGameRecord(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            string hidDataCount = queryvalues.ContainsKey("hidDataCount") ? queryvalues["hidDataCount"] : "";

            int MasterID = User.Identity.GetUserId<int>();
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);



            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, UserList = _MasterList };



            if (Request.IsAjaxRequest())
            {


                PagedList<UserMoneyRecord> data = UserMoneyRecordBLL.GetListByPageForAgent(grv);
                return PartialView("MemberForSearchGameRecord_PageList", data);


            }

            grv.DataList = UserMoneyRecordBLL.GetListByPageForAgent(grv);
            grv.Data = UserMoneyRecordBLL.GetUserInfo(grv);

            return View(grv);
        }


        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult MemberForSearchGameScore(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            int MasterID = User.Identity.GetUserId<int>();
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, UserList = _MasterList };
            if (Request.IsAjaxRequest())
            {
                return PartialView("MemberForSearchGameScore_PageList", UserMoneyRecordBLL.GetListByPageForAgent(grv));
            }
            grv.DataList = UserMoneyRecordBLL.GetListByPageForAgent(grv);
            return View(grv);
        }



        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult MemberForSearchGameExp(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            int MasterID = User.Identity.GetUserId<int>();
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);

            GameRecordView model = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, UserList = _MasterList };
            if (Request.IsAjaxRequest())
            {
                return PartialView("MemberForSearchGameExp_PageList", ExpRecordBLL.GetListByPageForAgent(model));
            }
            model.DataList = ExpRecordBLL.GetListByPageForAgent(model);
            return View(model);

        }


    }

}