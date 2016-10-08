using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GL.Data.Model;
using GL.Data.BLL;
using  GL.Data.AWeb.Identity;
using AWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using System.Security.Principal;
namespace AWeb.Controllers
{
    public class BaseController : Controller
    {

        //private AgentUserManager _userManager;

        //public BaseController()
        //{
        //}
        //public BaseController(AgentUserManager userManager)
        //{
        //    UserManager = userManager;
        //}
        //public AgentUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AgentUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}


        /// <summary>
        /// 注册用户统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        // GET: Base
        [QueryValues]
        [Authorize(Roles = "SU,CPS,CPA,公司,股东,总代,代理,策划")]
        public ActionResult NumberOfRegisteredUsers(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };


            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();

            IEnumerable < SelectListItem > cha = AgentUserBLL.GetUserList(_MasterList).Select(x=> new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });
            if (cha.Count() <= 1)
            {
                ViewData["cCount"] = 1;
            }
            else
            {
                ViewData["cCount"] = 2;
            }
            ViewData["Channels"] = cha;

            vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetRegisteredUsers(vbd));
            ViewData["AllUser"] = BaseDataBLL.GetAllUser(vbd);

            return View(vbd);
        }



        [QueryValues]
        [Authorize(Roles = "SU,CPS,CPA,公司,股东,总代,代理,策划")]
        public ActionResult NumberOfRegisteredUsersDetail(Dictionary<string, string> queryvalues)
        {

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

          


            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);



            BaseDataView vbd = new BaseDataView { UserList = _UserList, Channels = _Channels, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, SearchExt = oper };

            vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetRegisteredUsersOnHour(vbd));
            //通过时间查询role列表
            BaseDataView vbd2 = new BaseDataView { UserList = _UserList, Channels = _Channels, StartDate = dts.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = dte.ToString("yyyy-MM-dd 00:00:00"), Groupby = _Groupby, SearchExt = oper };

          
            //通过渠道查询注册信息

          
            string target = queryvalues.ContainsKey("target") ? queryvalues["target"] : "";
          

            return View(vbd);
        }


        [QueryValues]
        [Authorize(Roles = "SU,CPS,CPA,公司,股东,总代,代理,策划")]
        public ActionResult NumberOfRegisteredUsersDetailMonth(Dictionary<string, string> queryvalues)
        {

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




            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);



            BaseDataView vbd = new BaseDataView { UserList = _UserList, Channels = _Channels, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, SearchExt = oper };

            vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetRegisteredUsersOnHour(vbd));
            //通过时间查询role列表
            BaseDataView vbd2 = new BaseDataView { UserList = _UserList, Channels = _Channels, StartDate = dts.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = dte.ToString("yyyy-MM-dd 00:00:00"), Groupby = _Groupby, SearchExt = oper };


            //通过渠道查询注册信息


            string target = queryvalues.ContainsKey("target") ? queryvalues["target"] : "";


            return View(vbd);
        }

    



        /// <summary>
        /// 活跃用户统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        [Authorize(Roles = "SU,CPS,CPA,公司,股东,总代,代理,策划")]
        public ActionResult NumberOfActiveUsers(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);


            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            var cha = AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });
        
            if (cha.Count() <= 1)
            {
                ViewData["cCount"] = 1;
            }
            else
            {
                ViewData["cCount"] = 2;
            }
            ViewData["Channels"] = cha;

            vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetActiveUsers(vbd));


            return View(vbd);
        }
        [QueryValues]
        /// <summary>
        /// 在线玩家统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult OnlinePlay(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);


            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            var cha = AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem {
                Text = x.AgentName.AddBlank((int)x.AgentLv),
                Value = x.Id.ToString(),
                Selected = x.Id == _Channels
            });


            if (cha.Count() <= 1)
            {
                ViewData["cCount"] = 1;
            }
            else
            {
                ViewData["cCount"] = 2;
            }
            ViewData["Channels"] = cha;


            vbd.BaseDataList = new List<BaseDataInfoForOnlinePlay>(BaseDataBLL.GetOnlinePlay(vbd));


            return View(vbd);

        }




        //[QueryValues]
        ///// <summary>
        ///// 牌局统计
        ///// </summary>
        ///// <param name="queryvalues"></param>
        ///// <returns></returns>
        //public ActionResult NumberOfBoard(Dictionary<string, string> queryvalues)
        //{
        //    int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
        //    string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
        //    string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
        //    groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);
        //    BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };
        //    ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
        //    ViewData["AllPlayCount"] = BaseDataBLL.GetAllPlayCount(vbd);
        //    vbd.BaseDataList = new List<BaseDataInfo>(BaseDataBLL.GetPlayCount(vbd));
        //    return View(vbd);
        //}

        [QueryValues]
        /// <summary>
        /// 用户游戏币分布比
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult UsersGoldDistributionRatio(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;

            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, Channels = _Channels };

            ViewData["Channels"] = AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });

            BaseDataInfoForUsersGoldDistributionRatio model = BaseDataBLL.GetUsersGoldDistributionRatio(vbd);
            return View(model);

        }

        [QueryValues]
        /// <summary>
        /// 用户钻石分布比
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult UsersDiamondDistributionRatio(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;

            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, Channels = _Channels };

            ViewData["Channels"] = AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });

            BaseDataInfoForUsersDiamondDistributionRatio model = BaseDataBLL.GetUsersDiamondDistributionRatio(vbd);
            return View(model);


        }

        [QueryValues]
        /// <summary>
        /// 用户vip分布表
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult VIPDistributionRatio(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;

            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, Channels = _Channels };

            ViewData["Channels"] = AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });

            BaseDataInfoForVIPDistributionRatio model = BaseDataBLL.GetVIPDistributionRatio(vbd);
            return View(model);


        }
        [QueryValues]
        /// <summary>
        /// 破产率
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult BankruptcyRate(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            ViewData["Channels"] = AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });

            vbd.BaseDataList = BaseDataBLL.GetBankruptcyRate(vbd);
            return View(vbd);

        }


        /// <summary>
        /// 留存率
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,CPS,CPA,策划")]
        public ActionResult UserRetentionRates(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
           var cha= AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });


            if (cha.Count() <= 1)
            {
                ViewData["cCount"] = 1;
            }
            else
            {
                ViewData["cCount"] = 2;
            }
            ViewData["Channels"] = cha;

            vbd.BaseDataList = BaseDataBLL.GetRetentionRates(vbd);
            return View(vbd);

        }

        /// <summary>
        /// 充值统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,CPS,策划")]
        public ActionResult QQZoneRechargeCount(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : -1;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);
            string _target = queryvalues.ContainsKey("target") ? queryvalues["target"] : "";

            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
         
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            if(_MasterList== MasterID.ToString())
            {
                _Channels = MasterID;
            }

            string _UserList = AgentUserBLL.GetUserListString(_Channels);

            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels, Page = _page };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();

            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
                     x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
                     ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });

            var cha = AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });
            if (cha.Count() <= 1)
            {
                ViewData["cCount"] = 1;
            }
            else
            {
                ViewData["cCount"] = 2;
            }
            ViewData["Channels"] = cha;


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

                Webdiyer.WebControls.Mvc.PagedList<GL.Data.Model.QQZoneRechargeCount> pa = new Webdiyer.WebControls.Mvc.PagedList<GL.Data.Model.QQZoneRechargeCount>(BaseDataBLL.GetQQZoneRechargeCount(vbd), _pageGroup, 10);
                vbd.BaseDataList = pa;
                ViewData["dataGroup"] = vbd.BaseDataList;
                return PartialView("QQZoneRechargeCountGroup_PageList", ViewData["dataGroup"]);
            }
            else
            {
                ViewData["data"] = QQZoneRechargeBLL.GetListByPage(_page, vbd);
                Webdiyer.WebControls.Mvc.PagedList<GL.Data.Model.QQZoneRechargeCount> pa = new Webdiyer.WebControls.Mvc.PagedList<GL.Data.Model.QQZoneRechargeCount>(BaseDataBLL.GetQQZoneRechargeCount(vbd), _page, 10);
                vbd.BaseDataList = pa;
                ViewData["dataGroup"] = vbd.BaseDataList;
            }

            return View(vbd);

        }

        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,CPS,策划")]
        public ActionResult QQZoneRechargeDetail(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;//渠道id
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");//时间
                                                                                                                                               //select * from 515game.Role where CreateTime  between '2016-1-5' and '2016-1-6' and Agent=if(a=0,Agent,a) ;
            string type = queryvalues.ContainsKey("type") ? queryvalues["type"] : "";
            //AllCount
            string _allCount = queryvalues.ContainsKey("AllCount") ? queryvalues["AllCount"] : "";

            var arr = _StartDate.Split('-');
            DateTime dts = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(_StartDate.Substring(8, 2)), 0, 0, 0);
            DateTime dte = dts.AddDays(1);

            BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = dte.ToString("yyyy-MM-dd 00:00:00"), Channels = _Channels };

            IEnumerable<QQZoneRechargeCountDetail> detail = new List<QQZoneRechargeCountDetail>();
            switch (type)
            {

                case "1":
                    detail = BaseDataBLL.GetQQZoneRechargeFirstChargeDetail(vbd);
                    ViewData["AllCount"] = "首次充值人数[" + _allCount + "]";
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


        //[QueryValues]
        ///// <summary>
        ///// 游戏产出消耗
        ///// </summary>
        ///// <param name="queryvalues"></param>
        ///// <returns></returns>
        //public ActionResult GameOutput(Dictionary<string, string> queryvalues)
        //{

        //    AgentUser user = HttpContext.GetOwinContext().GetUserManager<AgentUserManager>().FindById(User.Identity.GetUserId<int>());
        //    int _Channels = user.Id;



        //    string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
        //    string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
        //    groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

        //    BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

        //    ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();



        //    vbd.BaseDataList = BaseDataBLL.GetGameOutput(vbd);
        //    return View(vbd);

        //}


        //[QueryValues]
        //public ActionResult GameOutputDetail(Dictionary<string, string> queryvalues)
        //{

        //    AgentUser user = HttpContext.GetOwinContext().GetUserManager<AgentUserManager>().FindById(User.Identity.GetUserId<int>());
        //    int _Channels = user.Id;

        //    string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");

        //    string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
        //    groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

        //    BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

        //    ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();


        //    vbd.BaseDataList = BaseDataBLL.GetGameOutputDetail(vbd);

        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("GameOutputDetail", vbd);
        //    }


        //    return View(vbd);
        //}


        [QueryValues]
        /// <summary>
        /// 游戏产出消耗(优化)
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult GameOutput2(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);

            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);
            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            ViewData["Channels"] = AgentUserBLL.GetUserList(_MasterList).Select(x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels });

            vbd.BaseDataList = BaseDataBLL.GetGameOutput2(vbd);
            return View(vbd);

        }

        [QueryValues]
        [Authorize(Roles = "SU,公司,股东,总代,代理,策划")]
        public ActionResult GameOutputDetail2(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);


            int MasterID = User.Identity.GetUserId<int>();
            if (AgentUserBLL.CheckUser(_Channels, MasterID))
            {
                _Channels = MasterID;
            }
            string _UserList = AgentUserBLL.GetUserListString(_Channels);
            string _MasterList = AgentUserBLL.GetUserListString(MasterID);



            BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, Groupby = _Groupby, Channels = _Channels };


            vbd.BaseDataList = BaseDataBLL.GetGameOutputDetail2(vbd);
            if (Request.IsAjaxRequest())
            {
                return PartialView("GameOutputDetail2", vbd);
            }
            return View(vbd);
        }


    }
}