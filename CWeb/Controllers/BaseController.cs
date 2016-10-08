using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace CWeb.Controllers
{
    public class BaseController : Controller
    {

        [QueryValues]
        public ActionResult ClubList(Dictionary<string, string> queryvalues)
        {
           
            string _target = queryvalues.ContainsKey("target") ? queryvalues["target"] : "";
            int _id = queryvalues.ContainsKey("SearchExter") ? string.IsNullOrWhiteSpace(queryvalues["SearchExter"]) ? 0 : Convert.ToInt32(queryvalues["SearchExter"]) : 0;
            int Search = queryvalues.ContainsKey("Search") ? string.IsNullOrWhiteSpace(queryvalues["Search"]) ? 0 : Convert.ToInt32(queryvalues["Search"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int page2 = queryvalues.ContainsKey("page2") ? Convert.ToInt32(queryvalues["page2"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            GameRecordView model = new GameRecordView { UserID = _id, SearchExt = Session["name"].ToString(), SearchExter = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            ViewData["pageindex"] = _page;
            ViewData["pageindex2"] = page2;
            if (_target == "box")
            {
                int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;

                ViewData["ClubList_PageList"] = ClubBLL.GetMyRebate(model);
                return PartialView("ClubList_PageList", ViewData["ClubList_PageList"]);
            }
            else if ((_target == "box2"))
            {
                ViewData["hid"] = "1";
                model.Page = page2;
                model.SearchExter = Search;
                model.UserID = Search;
                ViewData["ClubListDetail"] = ClubBLL.GetMyRebateDetail(model);
                return PartialView("ClubListDetail", ViewData["ClubListDetail"]);
            }
            else {

                int btn = queryvalues.ContainsKey("btn") ? string.IsNullOrWhiteSpace(queryvalues["btn"]) ? 0 : Convert.ToInt32(queryvalues["btn"]) : 0;

                int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;
                //Search
                ViewData["ClubList_PageList"] = ClubBLL.GetMyRebate(model);
               
                PagedList<UserClubDetail> obj = new PagedList<UserClubDetail>(
                    new List<UserClubDetail>()
                    , 1, 1, 1);

                ViewData["ClubListDetail"] = obj;

                if (btn == 1) {
                    ViewData["hid"] = "1";

                    int tmp = model.SearchExter;
                    model.SearchExter = Search;
                    model.UserID = Search;
                    ViewData["ClubListDetail"] = ClubBLL.GetMyRebateDetail(model);
                    model.SearchExter = tmp;
                    model.UserID = tmp;
                }

                return View(model);
            }

              

        

        }

        [QueryValues]
        public ActionResult ClubListDetail(Dictionary<string, string> queryvalues)
        {


            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");



            GameRecordView model = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };



          
            


            model.DataList = ClubBLL.GetMyRebateDetail(model);
            if (Request.IsAjaxRequest())
            {
                return PartialView("ClubListDetail", model.DataList);
            }

            return View(model.DataList);


        }


     

        [QueryValues]
        public ActionResult Test(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }

            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };
          
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
              x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
              ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;

            vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetActiveUsers(vbd));


            return View(vbd);
        }


    }
}