using GL.Common;
using GL.Data;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using GL.Protocol;
using MWeb.protobuf.SCmd;
using ProtoCmd.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;



namespace MWeb.Controllers
{

    [Authorize]
    public class BaseController : Controller
    {
        [QueryValues]
        public ActionResult BaseOperationalData(Dictionary<string, string> queryvalues)
        {


            return View();

        }


        [QueryValues]
        public ActionResult NumberOfRegisteredUsers(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);


            //int MasterID = User.Identity.GetUserId<int>();   //new
            //if (AgentUserBLL.CheckUser(_Channels, MasterID)) //new
            //{                         //new
            //    _Channels = MasterID;   //new
            //}   //new
            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0) {
                _UserList = _Channels.ToString();
            }


            BaseDataView vbd = new BaseDataView { UserList= _UserList,  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby,Channels=_Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
                x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
                ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;
            

            vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetRegisteredUsers(vbd));

            ViewData["AllUser"] = BaseDataBLL.GetAllUser(vbd);



            return View(vbd);
        }

        [QueryValues]
        public ActionResult NumberOfRegisteredUsersDetail(Dictionary<string, string> queryvalues) {

            //oper
            string oper = queryvalues.ContainsKey("oper") ? queryvalues["oper"] : "";
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            var arr = _StartDate.Split('-');
            DateTime dts = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(_StartDate.Substring(8, 2)), 0, 0, 0);
            DateTime dte = dts.AddDays(1);
            string _ExpirationDate = dts.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

      
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            string _UserList = AgentUserBLL.GetUserListString(0); //new
        
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }


            BaseDataView vbd = new BaseDataView { UserList = _UserList, Channels =_Channels,  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, SearchExt=oper };

            vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetRegisteredUsersOnHour(vbd));
            //通过时间查询role列表
            BaseDataView vbd2 = new BaseDataView { UserList = _UserList, Channels = _Channels, StartDate = dts.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = dte.ToString("yyyy-MM-dd 00:00:00"), Groupby = _Groupby, SearchExt = oper };

            ViewData["rRoles"] = RoleBLL.GetListByCreateTime(vbd2);

            //通过渠道查询注册信息

            ViewData["rChannel"] = BaseDataBLL.GetRegisteredUsersByChannel(vbd2);

            string target = queryvalues.ContainsKey("target") ? queryvalues["target"] :"";
            if (target == "boxR") {
                return View("NumberOfRegisteredUsersRegisterDetail", ViewData["rRoles"]);
            }

            return View(vbd);
        }

        [QueryValues]
        public ActionResult NumberOfRegisteredUsersRegisterDetail(Dictionary<string, string> queryvalues) {
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            var arr = _StartDate.Split('-');
            DateTime dts = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(_StartDate.Substring(8, 2)), 0, 0, 0);
            DateTime dte = dts.AddDays(1);
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _UserList = AgentUserBLL.GetUserListString(0); //new

            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }

            BaseDataView vbd = new BaseDataView { UserList = _UserList, Channels =_Channels, StartDate = _StartDate, ExpirationDate = dte.ToString() };

             //通过时间查询role列表

            vbd.BaseDataList = RoleBLL.GetListByCreateTime(vbd);
            if (Request.IsAjaxRequest())
            {
                return PartialView("NumberOfRegisteredUsersRegisterDetail", vbd.BaseDataList);
            }
            return View(vbd.BaseDataList);
        }


        [QueryValues]
        public ActionResult NumberOfActiveUsers(Dictionary<string, string> queryvalues)
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
            List<SelectListItem> sel = vbd.Groupby.ToSelectListItemForSelect();
            sel.Add(new SelectListItem { Text = "按周", Value = "11", Selected = ((int)vbd.Groupby == 11) });
            ViewData["groupby"] = sel;
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
              x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
              ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;

            vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetActiveUsers(vbd));


            return View(vbd);
        }

        [QueryValues]
        public ActionResult OnlinePlay(Dictionary<string, string> queryvalues)
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

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
              x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
              ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;
            vbd.BaseDataList = new List<BaseDataInfoForOnlinePlay>(BaseDataBLL.GetOnlinePlay(vbd));


            return View(vbd);

        }




        //[QueryValues]
        //public ActionResult NumberOfBoard(Dictionary<string, string> queryvalues)
        //{
        //    int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
        //    string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
        //    string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
        //    groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);
        //    BaseDataView vbd = new BaseDataView {  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };
        //    ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
        //    ViewData["AllPlayCount"] = BaseDataBLL.GetAllPlayCount(vbd);
        //    vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetPlayCount(vbd));
        //    return View(vbd);
        //}

        [QueryValues]
        public ActionResult UsersGoldDistributionRatio(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _TypeID = queryvalues.ContainsKey("UserType") ? Convert.ToInt32(queryvalues["UserType"]) : 1;
            BaseDataView vbd = new BaseDataView { Channels = _Channels, TypeID = _TypeID };
            //ViewData["Channels"] = _Channels;

            BaseDataInfoForUsersGoldDistributionRatio model = BaseDataBLL.GetUsersGoldDistributionRatio(vbd);
            model.Channels = _Channels;
            model.UserType = _TypeID;
            return View(model);

        }

        [QueryValues]
        public ActionResult UsersDiamondDistributionRatio(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _TypeID = queryvalues.ContainsKey("UserType") ? Convert.ToInt32(queryvalues["UserType"]) : 1;
            BaseDataView vbd = new BaseDataView { Channels = _Channels ,TypeID = _TypeID };
            //ViewData["Channels"] = _Channels;
            //ViewData["UserType"] = _TypeID;

            BaseDataInfoForUsersDiamondDistributionRatio model = BaseDataBLL.GetUsersDiamondDistributionRatio(vbd);
            model.Channels = _Channels;
            model.UserType = _TypeID;
            return View(model);

        }

        [QueryValues]
        public ActionResult BankruptcyRate(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            BaseDataView vbd = new BaseDataView {  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            


            vbd.BaseDataList = BaseDataBLL.GetBankruptcyRate(vbd);


            return View(vbd);


        }

        [QueryValues]
        public ActionResult UserRetentionRates(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _TypeID = queryvalues.ContainsKey("TypeID") ? Convert.ToInt32(queryvalues["TypeID"]) : 1; 
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);
            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }

            BaseDataView vbd = new BaseDataView { UserList=_UserList,  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby , Channels = _Channels ,TypeID = _TypeID };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
            x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
            ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;
            ViewData["TypeID"] = _TypeID;
            vbd.BaseDataList = BaseDataBLL.GetRetentionRates(vbd);

            return View(vbd);
        }

        [QueryValues]
        public ActionResult GameRetentionRates(Dictionary<string, string> queryvalues)
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

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
            x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
            ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;
            vbd.BaseDataList = BaseDataBLL.GetGameRetentionRates(vbd);

            return View(vbd);
        }

        [QueryValues]
        public ActionResult QQZoneRechargeCount(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : -1;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);
            string _target = queryvalues.ContainsKey("target") ? queryvalues["target"] : "";

            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }

            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels , Page= _page };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();

            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
                     x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
                     ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;

            ViewData["SumRecharge"] = QQZoneRechargeBLL.GetSumRecharge(vbd);

            if (_target == "box")
            {
                vbd.Page = _page;
                ViewData["data"] = QQZoneRechargeBLL.GetListByPage(_page, vbd);
                return PartialView("QQZoneRechargeCount_PageList", ViewData["data"]);
            }
            else if (_target == "boxGroup")
            {
                vbd.Page = _pageGroup;
             
                Webdiyer.WebControls.Mvc.PagedList<GL.Data.Model.QQZoneRechargeCount> pa = new Webdiyer.WebControls.Mvc.PagedList<GL.Data.Model.QQZoneRechargeCount>(BaseDataBLL.GetQQZoneRechargeCount(vbd), _pageGroup,10);
                vbd.BaseDataList = pa;
                ViewData["dataGroup"] = vbd.BaseDataList;
                return PartialView("QQZoneRechargeCountGroup_PageList", ViewData["dataGroup"]);
            }
            else {
                ViewData["data"] = QQZoneRechargeBLL.GetListByPage(_page, vbd);
                Webdiyer.WebControls.Mvc.PagedList<GL.Data.Model.QQZoneRechargeCount> pa = new Webdiyer.WebControls.Mvc.PagedList<GL.Data.Model.QQZoneRechargeCount>(BaseDataBLL.GetQQZoneRechargeCount(vbd), _page, 10);
                vbd.BaseDataList = pa;
                ViewData["dataGroup"] = vbd.BaseDataList;
            }
        
            return View(vbd);
        }
        /// <summary>
        ///注册是哪些人，再次充值是哪些人，当日注册且充值是哪些人
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult QQZoneRechargeDetail(Dictionary<string, string> queryvalues) {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;//渠道id
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");//时间
           //select * from 515game.Role where CreateTime  between '2016-1-5' and '2016-1-6' and Agent=if(a=0,Agent,a) ;
           string type = queryvalues.ContainsKey("type") ? queryvalues["type"] : "";
            //AllCount
            string _allCount = queryvalues.ContainsKey("AllCount") ? queryvalues["AllCount"] : "";
           
            var arr = _StartDate.Split('-');
            DateTime dts = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(_StartDate.Substring(8, 2)), 0, 0, 0);
            DateTime dte = dts.AddDays(1);

            BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = dte.ToString("yyyy-MM-dd 00:00:00"),  Channels = _Channels};

            IEnumerable<QQZoneRechargeCountDetail> detail = new List<QQZoneRechargeCountDetail>();
            switch (type) {
                
                case "1":
                    detail = BaseDataBLL.GetQQZoneRechargeFirstChargeDetail(vbd);
                    ViewData["AllCount"] = "首次充值人数["+_allCount+"]";
                    break;
                case "2":
                    detail = BaseDataBLL.GetQQZoneRechargeReChargeDetail(vbd);
                    ViewData["AllCount"] = "再次付费人数[" + _allCount + "]";
                    break;
                case "3":
                    detail = BaseDataBLL.GetQQZoneRechargeCurReChaDetail(vbd);
                    ViewData["AllCount"] = "当日注册且充值玩家[" + _allCount + "]";
                    break;
                case "4":
                    detail = BaseDataBLL.GetQQZoneRechargeAllReChaDetail(vbd);
                    ViewData["AllCount"] = "总充值人数[" + _allCount + "]";
                    break;
                default:
                    break;
            }


            return View(detail);
        }



        [QueryValues]
        public ActionResult QQZoneFlow(Dictionary<string, string> queryvalues) {
            int tab = queryvalues.ContainsKey("RaType") ? Convert.ToInt32(queryvalues["RaType"]) : -1;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            // int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            string SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView
            {
                SearchExt = SearchExt,
                
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate,
                Groupby = _Groupby,
                Channels = _Channels,
                RaType = tab
            };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();

            ViewData["SumRecharge"] = QQZoneRechargeBLL.GetSumRecharge(vbd);
            ViewData["tab"] = tab;

            if (Request.IsAjaxRequest())
            {
                return PartialView("QQZoneFlow_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }

           
            vbd.BaseDataList = QQZoneRechargeBLL.GetListByPage(page, vbd);

            return View(vbd);
        }
        
        /*
        [QueryValues]
        public ActionResult GameOutput(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);
            BaseDataView vbd = new BaseDataView {  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };
            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            vbd.BaseDataList = BaseDataBLL.GetGameOutput(vbd);
            return View(vbd);
        }

        [QueryValues]
        public ActionResult GameOutputDetail(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);
            BaseDataView vbd = new BaseDataView {  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };
            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            vbd.BaseDataList = BaseDataBLL.GetGameOutputDetail(vbd);
            if (Request.IsAjaxRequest())
            {
                return PartialView("GameOutputDetail", vbd);
            }
            return View(vbd);
        }
        */

        [QueryValues]
        public ActionResult GameOutput2(Dictionary<string, string> queryvalues)
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

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();

            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
                       x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
                       ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;

            vbd.BaseDataList = BaseDataBLL.GetGameOutput2(vbd);
            return View(vbd);
        }


        [QueryValues]
        public ActionResult GameOutputDetail2(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");

            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            BaseDataView vbd = new BaseDataView {  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            

            vbd.BaseDataList = BaseDataBLL.GetGameOutputDetail2(vbd);

            if (Request.IsAjaxRequest())
            {
                return PartialView("GameOutputDetail", vbd);
            }


            return View(vbd);
        }

        [QueryValues]
        public ActionResult GetOutAccurate(Dictionary<string, string> queryvalues) {
            string time = queryvalues.ContainsKey("time") ? queryvalues["time"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
           
            string oper = queryvalues.ContainsKey("oper") ? queryvalues["oper"] : "";

            switch (oper) {
                case "first"://第一次，只计算系统游戏币，和计算的时间
                    GameOutAccurate model = BaseDataBLL.GetGameOutAccurateFirst();
                    return Json(new { Data= model }, JsonRequestBehavior.AllowGet);
                case "notfirst"://不是第一次,计算系统游戏币，产出，消耗，和计算的时间
                    GameOutAccurate model2 = BaseDataBLL.GetGameOutAccurateFirst();
                    //计算产出，消耗

                    GameOutAccurate model3 = BaseDataBLL.GetGameOutAccurate(time, model2.CurTime);


                    model2.OutPut = model3.OutPut;
                    model2.InPut = model3.InPut;

                    return Json(new { Data = model2 }, JsonRequestBehavior.AllowGet);
                default:
                    //首次进页面，返回视图
                    return View();
            }
        }





        [QueryValues]
        public ActionResult GameOutTest(Dictionary<string, string> queryvalues) {
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

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();

            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
                       x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
                       ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;

            vbd.BaseDataList = BaseDataBLL.GetGameOutput2(vbd);
            return View(vbd);
        }

        [QueryValues]
        public ActionResult GameOutTestDetail(Dictionary<string, string> queryvalues) {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");

            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();


           // vbd.BaseDataList = BaseDataBLL.GetGameOutputRecursion(vbd);
            vbd.BaseDataList = BaseDataBLL.GetGameOutputDetail2(vbd);
            if (Request.IsAjaxRequest())
            {
                return PartialView("GameOutTestDetail", vbd);
            }

            
            return View(vbd);
        }

        [QueryValues]
        public ActionResult FuDaiPot(Dictionary<string, string> queryvalues)
        {
            IEnumerable<PotRecord> model = BaseDataBLL.GetPotRecord(1);
            return View(model);
        }

        




        [QueryValues]
        public ActionResult Scoreboard(Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
           
         


           PagedList<JiFen> model = BaseDataBLL.GetNowScoreboard(page);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Scoreboard_PageList", model);
            }


            return View(model);
        }
        

        [QueryValues]
        public ActionResult OpenFuDaiBoard(Dictionary<string, string> queryvalues)
        {


            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");

            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            BaseDataView vbd = new BaseDataView {  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            




            vbd.BaseDataList = BaseDataBLL.GetOpenFuDai(vbd);



            return View(vbd);
        }



        [QueryValues]
        public ActionResult PotRakeback(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");

            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            BaseDataView vbd = new BaseDataView {  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            


            IEnumerable<BaseDataInfoForPotRakeback> pr = BaseDataBLL.GetPotRakeback(vbd);



            ViewData["Sum"] = pr.Sum(x => x.Chip);


            vbd.BaseDataList = pr;

            return View(vbd);
        }

        

        [QueryValues]
        public ActionResult VIPDistributionRatio(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _TypeID = queryvalues.ContainsKey("UserType") ? Convert.ToInt32(queryvalues["UserType"]) : 1;
            BaseDataView vbd = new BaseDataView { Channels = _Channels, TypeID = _TypeID };
            //ViewData["Channels"] = _Channels;

            BaseDataInfoForVIPDistributionRatio model = BaseDataBLL.GetVIPDistributionRatio(vbd);
            model.Channels = _Channels;
            model.UserType = _TypeID;
            return View(model);

        }



        [QueryValues]
        public ActionResult OnLineList(Dictionary<string, string> queryvalues) {

           
            //var onlineUseralle = ServiceStackManage.Redis.GetDatabase().HashGetAll("UserInfo",StackExchange.Redis.CommandFlags.None);
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"].Trim() : "";
            string leftMenuMark = queryvalues.ContainsKey("leftMenuMark") ? queryvalues["leftMenuMark"] : string.Empty;
            GameRecordView mode = new GameRecordView();

            if (!string.IsNullOrEmpty(SearchExt))
            {
                RedisValue rv = ServiceStackManage.Redis.GetDatabase().HashGet("UserInfo", SearchExt, StackExchange.Redis.CommandFlags.None);
                if (rv.IsNull)
                {
                    List<Role> rlist = new List<Role>();
                    PagedList<Role> pageRole = new PagedList<Role>(rlist, 1, 1, 1);

                    mode.Data = pageRole;
                    mode.SearchExt = SearchExt;
                    return View(mode);
                }
                else {
                    IEnumerable<Role> roleList = RoleBLL.GetModelByIDList(SearchExt);

                    PagedList<Role> pageRole = new PagedList<Role>(roleList, page, 10, 1);
                    // MemberSeachView
                    mode.Data = pageRole;
                    mode.SearchExt = SearchExt;
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("OnLineList_PageList", pageRole);
                    }

                    return View(mode);
                }
               
            }
            else {
                //IEnumerable<HashEntry> onlineUseralle = ServiceStackManage.Redis.GetDatabase().HashScan("UserInfo", "", 1,0,2, CommandFlags.None);

                var onlineUseralle = ServiceStackManage.Redis.GetDatabase().HashGetAll("UserInfo", StackExchange.Redis.CommandFlags.None);


                // IEnumerable<HashEntry> onlineUseralle = ServiceStackManage.Redis.GetDatabase().HashScan("UserInfo", SearchExt, CommandFlags.None);

                long lineCount = ServiceStackManage.Redis.GetDatabase().HashLength("UserInfo", CommandFlags.None);
                IEnumerable<HashEntry> onlineUserHashEntry = onlineUseralle.Skip((page - 1) * 10).Take(10);

             
                string id = "";
                foreach (HashEntry item in onlineUserHashEntry)
                {

                    id += item.Name + ",";
                }
                id = id.Trim(',');

                IEnumerable<Role> roleList = RoleBLL.GetModelByIDList(id);

                PagedList<Role> pageRole = new PagedList<Role>(roleList, page, 10, Convert.ToInt32(lineCount));
                // MemberSeachView
                mode.Data = pageRole;
                mode.SearchExt = SearchExt;
                if (leftMenuMark != "leftMenu") {
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("OnLineList_PageList", pageRole);
                    }
                }
             

                return View(mode);
            }

           
        }


        [QueryValues]
        public ActionResult OnPlayList(Dictionary<string, string> queryvalues)
        {
            //var onlineUseralle = ServiceStackManage.Redis.GetDatabase().HashGetAll("UserInfo",StackExchange.Redis.CommandFlags.None);
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string leftMenuMark = queryvalues.ContainsKey("leftMenuMark") ? queryvalues["leftMenuMark"] : string.Empty;
            string SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            int gametype = queryvalues.ContainsKey("Gametype") ? Convert.ToInt32( queryvalues["Gametype"]) : 0;
            //IEnumerable<HashEntry> onlineUseralle = ServiceStackManage.Redis.GetDatabase().HashScan("UserInfo", "", 1,0,2, CommandFlags.None);

         



            GameRecordView mode = new GameRecordView();
            mode.Gametype = gametype;
         
            if (!string.IsNullOrEmpty(SearchExt))
            {
                RedisValue rv = ServiceStackManage.Redis.GetDatabase().HashGet("UserGame", SearchExt, StackExchange.Redis.CommandFlags.None);
                if (rv.IsNull)
                {
                    List<OnModel> rlist = new List<OnModel>();
                    PagedList<OnModel> pageRole = new PagedList<OnModel>(rlist, 1, 1, 1);

                    mode.Data = pageRole;
                    mode.SearchExt = SearchExt;
                    return View(mode);
                }
                else
                {
                    Role r = new Role(); r.ID = Convert.ToInt32(SearchExt);
                    Role role = RoleBLL.GetModelByID(r);
                    OnModel onModel = new OnModel()
                    {
                        ID = role.ID,
                        Account = role.Account,
                        NickName = role.NickName,
                        Type = OnModel.GetGameType(rv.ToString())
                    };


                    if (gametype == -1)
                    {
                        PagedList<OnModel> pageRole = new PagedList<OnModel>(new List<OnModel>() { onModel }, 1, 10, 1);
                        // MemberSeachView
                        mode.Data = pageRole;
                        mode.SearchExt = SearchExt;
                        if (Request.IsAjaxRequest())
                        {
                            return PartialView("OnLineList_PageList", pageRole);
                        }

                        return View(mode);
                    }
                    else {
                        if (rv.ToString().Contains(gametype.ToString()) && gametype != -1)
                        {
                            PagedList<OnModel> pageRole = new PagedList<OnModel>(new List<OnModel>() { onModel }, 1, 10, 1);
                            // MemberSeachView
                            mode.Data = pageRole;
                            mode.SearchExt = SearchExt;
                            if (Request.IsAjaxRequest())
                            {
                                return PartialView("OnLineList_PageList", pageRole);
                            }

                            return View(mode);
                        }
                        else
                        {
                            List<OnModel> rlist = new List<OnModel>();
                            PagedList<OnModel> pageRole = new PagedList<OnModel>(rlist, 1, 1, 1);

                            mode.Data = pageRole;
                            mode.SearchExt = SearchExt;
                            return View(mode);
                        }
                    }

                  

                  
                }

            }
            else {

                var onPlayUseralle = ServiceStackManage.Redis.GetDatabase().HashGetAll("UserGame", StackExchange.Redis.CommandFlags.None);
                IEnumerable<HashEntry> onPlayUseralle2;
                    if (gametype == -1) {
                    onPlayUseralle2 = onPlayUseralle.ToList();
                }
                else {
                    onPlayUseralle2 =onPlayUseralle.Where(m => m.Value.ToString().Contains(gametype.ToString()));
                }
                   

                long playCount = onPlayUseralle2.Count();
                IEnumerable<HashEntry> onPlayUserHashEntry = onPlayUseralle2.Skip((page - 1) * 10).Take(10);

            

                string id = "";
                foreach (HashEntry item in onPlayUserHashEntry)
                {
                    /*
                     HashEntry hash = onlineUseralle[i];


                    string Name = onlineUseralle[i].Name;
                    string Value = onlineUseralle[i].Value;
                    string[] s = Value.Split('_');
                    int VipGrade = Convert.ToInt32(s[4]);
                    */
                    id += item.Name + ",";
                }
                id = id.Trim(',');

                IEnumerable<Role> roleList = RoleBLL.GetModelByIDList(id);
                List<OnModel> list = new List<OnModel>();


                list = (from x in roleList
                        join y in onPlayUserHashEntry on x.ID equals Convert.ToInt32(y.Name) into odr
                        from o in odr.DefaultIfEmpty()
                        select new OnModel
                        {
                            ID = x.ID,
                            Account = x.Account,
                            NickName = x.NickName,
                            Type = OnModel.GetGameType(o.Value)
                        }).ToList();





                PagedList<OnModel> pageRole = new PagedList<OnModel>(list, page, 10, Convert.ToInt32(playCount));
                // MemberSeachView
                mode.Data = pageRole;
                mode.SearchExt = SearchExt;
                if (leftMenuMark != "leftMenu") {
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("OnPlayList_PageList", pageRole);
                    }

                }

                return View(mode);

            }



        }
        [QueryValues]
        public ActionResult Default(Dictionary<string, string> queryvalues) {
            return View();
        }
        [QueryValues]
        public ActionResult UserProfit(Dictionary<string, string> queryvalues)
        {
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"].ToString() : "";
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            BaseDataView vbd = new BaseDataView
            {
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate,
                Channels = _Channels,
                SearchExt = _SearchExt
            };

            ViewData["SearchExt"] = _SearchExt;
            if (_SearchExt != "")
            {
                //ViewData["DetailProfit"] = BaseDataBLL.GetGameOutputDetailUser(vbd);
                ViewData["GameProfit"] = BaseDataBLL.GetGameProfit(vbd);
            }
            else
            {
                ViewData["GameProfit"] = "";
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("UserProfit_PageList", BaseDataBLL.GetUsreProfit(_page, vbd));
            }
            vbd.BaseDataList = BaseDataBLL.GetUsreProfit(_page, vbd);
            return View(vbd);
        }

        [QueryValues]
        public ActionResult Ruin(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Channels = _Channels };
 
            vbd.BaseDataList = new List<Ruin>(BaseDataBLL.GetRuinUsers(vbd));

            return View(vbd);
        }


        [QueryValues]
        public ActionResult BoardDist(Dictionary<string, string> queryvalues)//牌局分布
        {

            int _Gametype = queryvalues.ContainsKey("Gametype") ? string.IsNullOrWhiteSpace(queryvalues["Gametype"]) ? 0 : Convert.ToInt32(queryvalues["Gametype"]) : 0;
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _UserType = queryvalues.ContainsKey("UserType") ? Convert.ToInt32(queryvalues["UserType"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
              x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
              ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;


            GameRecordView grv = new GameRecordView { StartDate = _StartDate, ExpirationDate = _ExpirationDate,
                 UserType = _UserType, Channels = _Channels, Gametype=_Gametype
            };




            if (Request.IsAjaxRequest())
            {
                return PartialView("BoardDist_PageList", GameDataBLL.GetListByPageForBoardDist(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForBoardDist(grv);

            return View(grv);

           
        }

        [QueryValues]
        public ActionResult BoardRate(Dictionary<string, string> queryvalues)//牌局留存率
        {
            int _Gametype = queryvalues.ContainsKey("Gametype") ? string.IsNullOrWhiteSpace(queryvalues["Gametype"]) ? 13 : Convert.ToInt32(queryvalues["Gametype"]) : 13;
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _UserType = queryvalues.ContainsKey("UserType") ? Convert.ToInt32(queryvalues["UserType"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
              x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
              ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;


            GameRecordView grv = new GameRecordView
            {
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate,
                UserType = _UserType,
                Channels = _Channels,
                Gametype = _Gametype
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("BoardDist_PageList", GameDataBLL.GetListByPageForBoardRate(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForBoardRate(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult BoardDetail(Dictionary<string, string> queryvalues)//牌局细节
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;//渠道
            int _UserType = queryvalues.ContainsKey("UserType") ? Convert.ToInt32(queryvalues["UserType"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _Gametype = queryvalues.ContainsKey("Gametype") ? queryvalues["Gametype"] : "15";
            string chujiItemk = queryvalues.ContainsKey("chujiItemk") ? queryvalues["chujiItemk"] : "";
            if (_Gametype != "15")
            {
                chujiItemk = _Gametype;
            }

            //string zhongjiItem = queryvalues.ContainsKey("zhongjiItem") ? queryvalues["zhongjiItem"] : "";
            //string gaojiItem = queryvalues.ContainsKey("gaojiItem") ? queryvalues["gaojiItem"] : "";
            string tongji = queryvalues.ContainsKey("tongji") ? queryvalues["tongji"] : "";
            string tempChuji = "";
            if (!string.IsNullOrEmpty(chujiItemk)) {
                string[] s = chujiItemk.Split(',');
                for (int i = 0; i < s.Length; i++) {
                    tempChuji += "'"+s[i]+"',";
                }
                tempChuji = tempChuji.Trim(',');
           
            }

            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
              x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
              ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;
            ViewData["GametypeS"] = _Gametype;
            ViewData["tongji"] = tongji;
            ViewData["chujiItemk"] = chujiItemk;
            GameRecordView grv = new GameRecordView
            {
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate,
                UserType = _UserType,
                Channels = _Channels,
                GametypeS = _Gametype, Gametype = Convert.ToInt32(_Gametype), SearchExt= tempChuji
            };





            IEnumerable<BoardDetail_Left> lefts = GameDataBLL.GetBoardDetail_LeftList(grv);
            IEnumerable<BoardDetail_Right> rights = new List<BoardDetail_Right>();

            if (string.IsNullOrEmpty(chujiItemk))
            {
             
            }
            else {
               rights = GameDataBLL.GetBoardDetail_RightList(grv);
            }

         

            //List<BoardDetail_Left> lefts = new List<BoardDetail_Left>();




            //lefts.Add(new BoardDetail_Left { CreateTime =new DateTime(2016,8,1), TotalBoard = 111, TotalCount = 222, TotalRecive = 333 });
            //lefts.Add(new BoardDetail_Left { CreateTime = new DateTime(2016, 8, 2), TotalBoard = 222, TotalCount = 122, TotalRecive = 123 });

            //List<BoardDetail_Right> rights = new List<BoardDetail_Right>();

            //if (_Gametype == "15")
            //{
            //    if (!string.IsNullOrEmpty(chujiItemk)) {
            //        string[] ss = chujiItemk.Split(',');
            //        for (int i = 0; i < ss.Length; i++)
            //        {
            //            rights.Add(new BoardDetail_Right
            //            {
            //                BoardCount = 1,
            //                BoardFlowPer = 2,
            //                BoardLv = 3,
            //                BoardNum = 4,
            //                BoardNumPer = 5,
            //                BoardRate = 1,
            //                BoardTimePer = 1,
            //                BoardTimePerMan = 2,
            //                CallBack = 3,
            //                CreateTime = new DateTime(2016, 8, 1),
            //                GameID = 15,
            //                RoomCategory = ss[i]
            //            });
            //            rights.Add(new BoardDetail_Right
            //            {
            //                BoardCount = 1,
            //                BoardFlowPer = 2,
            //                BoardLv = 13,
            //                BoardNum = 24,
            //                BoardNumPer = 25,
            //                BoardRate = 1,
            //                BoardTimePer = 1,
            //                BoardTimePerMan = 22,
            //                CallBack = 23,
            //                CreateTime = new DateTime(2016, 8, 2),
            //                GameID = 15,
            //                RoomCategory = ss[i]
            //            });
            //        }
            //    }



            //}
            //else
            //{
            //    rights.Add(new BoardDetail_Right
            //    {
            //        BoardCount = 31,
            //        BoardFlowPer = 42,
            //        BoardLv = 33,
            //        BoardNum = 34,
            //        BoardNumPer = 35,
            //        BoardRate = 31,
            //        BoardTimePer = 31,
            //        BoardTimePerMan = 32,
            //        CallBack = 33,
            //        CreateTime = new DateTime(2016, 8, 1),
            //        GameID = Convert.ToInt32(_Gametype),
            //        RoomCategory = _Gametype
            //    });
            //    rights.Add(new BoardDetail_Right
            //    {
            //        BoardCount = 31,
            //        BoardFlowPer = 42,
            //        BoardLv = 33,
            //        BoardNum = 34,
            //        BoardNumPer = 35,
            //        BoardRate = 31,
            //        BoardTimePer = 31,
            //        BoardTimePerMan = 32,
            //        CallBack = 33,
            //        CreateTime = new DateTime(2016, 8, 2),
            //        GameID = Convert.ToInt32(_Gametype),
            //        RoomCategory = _Gametype
            //    });

            //}


            //   grv.DataList = GameDataBLL.GetListByPageForDownLevel(grv);
            grv.Data = lefts;
            grv.DataList = rights;
            return View(grv);
        }


        [QueryValues]
        public ActionResult DownLevel(Dictionary<string, string> queryvalues)//下注梯度
        {
            string _Gametype = queryvalues.ContainsKey("Gametype") ? queryvalues["Gametype"] : "13";
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 1;
            int _UserType = queryvalues.ContainsKey("UserType") ? Convert.ToInt32(queryvalues["UserType"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
              x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
              ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;
            ViewData["GametypeS"] = _Gametype;

            GameRecordView grv = new GameRecordView
            {
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate,
                UserType = _UserType,
                Channels = _Channels,
                GametypeS = _Gametype
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("DownLevel_PageList", GameDataBLL.GetListByPageForDownLevel(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForDownLevel(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult VersionSum(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Channels = _Channels };

            vbd.BaseDataList = new List<VersionSum>(BaseDataBLL.GetVersionSum(vbd));

            return View(vbd);
        }


        [QueryValues]
        public ActionResult ChanleSum(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Channels = _Channels };

            vbd.BaseDataList = new List<ChangleSum>(BaseDataBLL.GetChangleSum(vbd));

            return View(vbd);
        }


        /// <summary>
        /// 日报
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult DayReport(Dictionary<string, string> queryvalues) {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            int _Channels = queryvalues.ContainsKey("Channels") ? string.IsNullOrWhiteSpace(queryvalues["Channels"]) ? 0 : Convert.ToInt32(queryvalues["Channels"]) : 0;
          
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            BaseDataView vbd = new BaseDataView { UserList=_UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Channels = _Channels };

            if (Request.IsAjaxRequest())
            {
                return PartialView("DayReport_PageList", BaseDataBLL.GetDayReportList(vbd));
            }

            vbd.BaseDataList = BaseDataBLL.GetDayReportList(vbd);

            return View(vbd);
        }

        /// <summary>
        /// 周报
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult WeekReport(Dictionary<string, string> queryvalues)
        {
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");

            int _Channels = queryvalues.ContainsKey("Channels") ? string.IsNullOrWhiteSpace(queryvalues["Channels"]) ? 0 : Convert.ToInt32(queryvalues["Channels"]) : 0;

            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, Channels = _Channels };

        
            if (Request.IsAjaxRequest())
            {
                return PartialView("WeekReport_PageList", BaseDataBLL.GetWeekReport(vbd));
            }

            vbd.BaseDataList = BaseDataBLL.GetWeekReport(vbd);

            return View(vbd);
        }

        /// <summary>
        /// 月报
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult MonthReport(Dictionary<string, string> queryvalues)
        {
         
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-01");
            string[] stimes = _StartDate.Split('-');
            _StartDate = stimes[0] + "-" + stimes[1] + "-01";


            int _Channels = queryvalues.ContainsKey("Channels") ? string.IsNullOrWhiteSpace(queryvalues["Channels"]) ? 0 : Convert.ToInt32(queryvalues["Channels"]) : 0;

            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate,Channels = _Channels };
            var d = BaseDataBLL.GetMonthReport(vbd);
            string[] stimes2 = _StartDate.Split('-');
            vbd.StartDate = stimes2[0] + "-" + stimes2[1];



            if (Request.IsAjaxRequest())
            {
                return PartialView("MonthReport_PageList",d );
            }

            vbd.BaseDataList = d;

            return View(vbd);
        }


    }
}