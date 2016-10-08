using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;


using GL.Common;

namespace MWeb.Controllers
{
    [Authorize]
    public class RechargeController : Controller
    {
        // GET: Recharge
        [QueryValues]
        public ActionResult List(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            if (Request.IsAjaxRequest())
            {
                return PartialView("List_PageList", AgentInfoBLL.GetListByPage(page));
            }


            PagedList<AgentInfo> model = AgentInfoBLL.GetListByPage(page);
            return View(model);
        }



        [QueryValues]
        public ActionResult QQZoneRecharge(Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView { UserID = _UserID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            
            ViewData["SumRecharge"] = QQZoneRechargeBLL.GetSumRecharge(vbd); 

            if (Request.IsAjaxRequest())
            {
                return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }


            vbd.BaseDataList = QQZoneRechargeBLL.GetListByPage(page, vbd);

            return View(vbd);


        }

        [QueryValues]
        public ActionResult RechargeRaType(Dictionary<string, string> queryvalues) {

            int tab = queryvalues.ContainsKey("tab") ? Convert.ToInt32(queryvalues["tab"]) : -1;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView { UserID = _UserID, StartDate = _StartDate,
                ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels,
                 RaType = tab
            };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            

            ViewData["SumRecharge"] = QQZoneRechargeBLL.GetSumRecharge(vbd);
            ViewData["tab"] = tab;

            if (Request.IsAjaxRequest())
            {
                return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }


            vbd.BaseDataList = QQZoneRechargeBLL.GetListByPage(page, vbd);

            return View(vbd);
        }

        [QueryValues]
        public ActionResult YiBaoRechargeRaType(Dictionary<string, string> queryvalues)
        {

            int tab = queryvalues.ContainsKey("tab") ? Convert.ToInt32(queryvalues["tab"]) : -1;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView
            {
                UserID = _UserID,
                
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
                return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }


            vbd.BaseDataList = QQZoneRechargeBLL.GetListByPage(page, vbd);

            return View("RechargeRaType", vbd);
        }

        [QueryValues]
        public ActionResult IOSRechargeRaType(Dictionary<string, string> queryvalues)
        {

            int tab = queryvalues.ContainsKey("tab") ? Convert.ToInt32(queryvalues["tab"]) : -1;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView
            {
                UserID = _UserID,
                
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
                return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }


            vbd.BaseDataList = QQZoneRechargeBLL.GetListByPage(page, vbd);

            return View("RechargeRaType", vbd);
        }

        [QueryValues]
        public ActionResult WeiXinRechargeRaType(Dictionary<string, string> queryvalues)
        {

            int tab = queryvalues.ContainsKey("tab") ? Convert.ToInt32(queryvalues["tab"]) : -1;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView
            {
                UserID = _UserID,
                
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
                return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }


            vbd.BaseDataList = QQZoneRechargeBLL.GetListByPage(page, vbd);

            return View("RechargeRaType", vbd);
        }

        [QueryValues]
        public ActionResult ZhiFuBaoRechargeRaType(Dictionary<string, string> queryvalues)
        {

            int tab = queryvalues.ContainsKey("tab") ? Convert.ToInt32(queryvalues["tab"]) : -1;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView
            {
                UserID = _UserID,
                
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
                return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }


            vbd.BaseDataList = QQZoneRechargeBLL.GetListByPage(page, vbd);

            return View("RechargeRaType", vbd);
        }


        [QueryValues]
        public ActionResult FirstRechargeSum(Dictionary<string, string> queryvalues) {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView { UserID = _UserID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            
            ViewData["SumRecharge"] = QQZoneRechargeBLL.GetFirstSumRecharge(vbd);

            if (Request.IsAjaxRequest())
            {
                return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }



            return View(vbd);
        }

        [QueryValues]
        public ActionResult FirstChargeItem(Dictionary<string, string> queryvalues) {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView { UserID = _UserID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            
            ViewData["SumRecharge"] = QQZoneRechargeBLL.GetSumRecharge(vbd);

            if (Request.IsAjaxRequest())
            {
                return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            }


            vbd.BaseDataList = QQZoneRechargeBLL.GetFirstRechargeItemCount( vbd);

            return View(vbd);
        }
        [QueryValues]
        public ActionResult TexasGameGetAward(Dictionary<string, string> queryvalues) {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;

            //GetGameActiveTime
            GL.Data.ActiveTime time = QQZoneRechargeBLL.GetGameActiveTime(GL.Data.ActiveType.德州玩牌领奖);
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");



            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView { UserID = _UserID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();

            ViewData["ActiveTime"] = time;

            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("QQZoneRecharge_PageList", QQZoneRechargeBLL.GetListByPage(page, vbd));
            //}

          
            vbd.BaseDataList = QQZoneRechargeBLL.GetTexasGameGetAwardItemCount(vbd);
            vbd.StartDate = _StartDate;
            return View(vbd);
        }

        [QueryValues]
        public ActionResult NewYearCharge(Dictionary<string, string> queryvalues) {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;

           // string _StartDate = "";
          //  string _ExpirationDate = "";


            //if (queryvalues.ContainsKey("StartDate"))
            //{
            //    _StartDate = queryvalues["StartDate"];
            //}
            //else
            //{
            //    GL.Data.ActiveTime time = QQZoneRechargeBLL.GetGameActiveTime(GL.Data.ActiveType.充值礼包奖励);
            //    _StartDate = time.StartTime.ToString();
            //}



            //if (queryvalues.ContainsKey("ExpirationDate"))
            //{
            //    _ExpirationDate = queryvalues["ExpirationDate"];
            //}
            //else
            //{
            //    GL.Data.ActiveTime time = QQZoneRechargeBLL.GetGameActiveTime(GL.Data.ActiveType.充值礼包奖励);
            //    _ExpirationDate = time.EndTime.ToString();
            //}

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");




            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView { UserID = _UserID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Channels = _Channels };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();

//            select UserID, sum(money) / 100 from 515game.QQZoneRecharge
//where CreateTime BETWEEN '2016-1-5 00:05:00' and '2016-1-15 23:59:00'
//group by UserID


            //新年充值活动数据
            ViewData["NewYear"] = QQZoneRechargeBLL.GetNewYearCharge(vbd);

            //充值排行前10数据
            //ViewData["RechargeRank"] = QQZoneRechargeBLL.NewYearChargeRank(vbd);

            return View(vbd);
        }

        [QueryValues]
        public ActionResult RedeemCode(Dictionary<string, string> queryvalues) {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string ip = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            BaseDataView bdv = new BaseDataView()
            {
                SearchExt = ip,
                Page = page
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("RedeemCode_PageList", IPWhiteListBLL.GetListByPage(bdv));
            }


            bdv.BaseDataList = IPWhiteListBLL.GetListByPage(bdv);

            return View(bdv);

          
        }


        //签到抽奖数据统计
        [QueryValues]
        public ActionResult SignDrawSumData(Dictionary<string, string> queryvalues) {


            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;

            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }

            BaseDataView vbd = new BaseDataView { UserList=_UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Channels= _Channels };


            vbd.BaseDataList = BaseDataBLL.GetSignDraw(vbd);


         
            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
                x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
                ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;



            //SignDraw

            return View(vbd);
        }

        [QueryValues]
        public ActionResult IPAnalyze(Dictionary<string, string> queryvalues) {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartTime") ? queryvalues["StartTime"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _EndTime = queryvalues.ContainsKey("EndTime") ? queryvalues["EndTime"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _OrderID = queryvalues.ContainsKey("OrderID") ?queryvalues["OrderID"].ToString() : "";
            string _OrderIP = queryvalues.ContainsKey("OrderIP") ? queryvalues["OrderIP"].ToString() : "";
            int _ChargeType = queryvalues.ContainsKey("ChargeType") ? Convert.ToInt32(queryvalues["ChargeType"]) : -1;
            UserIpInfo model = new UserIpInfo() {
                 UserID = _UserID, OrderID = _OrderID, OrderIP = _OrderIP,
                 ChargeType = _ChargeType, StartTime=_StartDate, EndTime= _EndTime
            };

            PagedList<UserIpInfo> userinfo = RechargeCheckBLL.GetListByPage(page, model);
   
            if (Request.IsAjaxRequest())
            {
                return PartialView("IPAnalyze_PageList", userinfo);
            }

            model.Data = userinfo;
            return View(model);
        }
        [QueryValues]
        public ActionResult CallBackIP(Dictionary<string, string> queryvalues) {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartTime") ? queryvalues["StartTime"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _EndTime = queryvalues.ContainsKey("EndTime") ? queryvalues["EndTime"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _OrderID = queryvalues.ContainsKey("OrderID") ? queryvalues["OrderID"].ToString() : "";
            string _OrderIP = queryvalues.ContainsKey("OrderIP") ? queryvalues["OrderIP"].ToString() : "";
            int _ChargeType = queryvalues.ContainsKey("ChargeType") ? Convert.ToInt32(queryvalues["ChargeType"]) : -1;
            UserIpInfo model = new UserIpInfo()
            {
                UserID = _UserID,
                OrderID = _OrderID,
                OrderIP = _OrderIP,
                ChargeType = _ChargeType,
                StartTime = _StartDate,
                EndTime = _EndTime
            };

            List<CallBackRechargeIP> callIp = RechargeCheckBLL.GetCallBackIP( model);
            ViewData["type"] = (raType)_ChargeType;
            ViewData["inttype"] = _ChargeType;
            if (Request.IsAjaxRequest())
            {
                return PartialView("CallBackIP_PageList", callIp);
            }

            model.Data = callIp;
            return View(model);

           
        }
        [QueryValues]
        public ActionResult SetSafeIP(Dictionary<string, string> queryvalues) {
          
            string ip = queryvalues.ContainsKey("ip") ? queryvalues["ip"].ToString() : "";
            string type = queryvalues.ContainsKey("type") ? queryvalues["type"].ToString() : "";
            string option = queryvalues.ContainsKey("option") ? queryvalues["option"].ToString() : "";
            bool res = false;
            if (option == "1") {
                res = RechargeCheckBLL.AddChargeIP(ip, type);
            }
            else {
                res = RechargeCheckBLL.DeleteChargeIP(ip, type);
            }

            if (res)
            {
                return Json(new { result = 0 });
            }
            else
            {
                return Json(new { result = 1 });

            }


        }

        [QueryValues]
        public ActionResult SimulatorRecharge(Dictionary<string, string> queryvalues) {
          
            SimulatorRecharge model = new GL.Data.Model.SimulatorRecharge();
         
                return View(model);
       


          
        }
        [QueryValues]
        public ActionResult SimulatorRechargeMoney(Dictionary<string, string> queryvalues) {
            string ispostback = queryvalues.ContainsKey("ispostback") ? queryvalues["ispostback"].ToString() : "";
            SimulatorRecharge model = new GL.Data.Model.SimulatorRecharge();
           

                int UserID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"].ToString()) : 0;
                int Type = queryvalues.ContainsKey("Type") ? Convert.ToInt32(queryvalues["Type"].ToString()) : 1;
                decimal Money = queryvalues.ContainsKey("Money") ? Convert.ToDecimal(queryvalues["Money"].ToString()) : 0;
                double Discounted = queryvalues.ContainsKey("Discounted") ? Convert.ToDouble(queryvalues["Discounted"].ToString()) : 0;

                model.UserID = UserID;
                model.Type = Type;
                model.Money = Money;
                model.Discounted = Discounted;

                uint gold = 0;
                uint dia = 0;
                uint rmb = 0;
                bool firstGif = false;
                isFirst iF = isFirst.否;
                string billNO = Utils.GenerateOutTradeNo("TestPay"); ;
                if (Type == 3)
                {
                    int userid = UserID;
                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = userid });
                    if (recharge != null)//首冲过
                    {
                        return Content("2");//重复首冲
                    }
                }
                long transtimeL = Utils.GetTimeStampL();
                RechargeCheckBLL.Add(new RechargeCheck
                {
                    Money = (int)model.Money,
                    ProductID = "Chip_8",
                    SerialNo = billNO,
                    UserID = UserID,
                    CreateTime = (ulong)transtimeL
                }
                );












                //计算人名币
                if (Discounted > 0)
                {
                    rmb = (uint)Convert.ToInt32((uint)Money * 100 * Discounted / 10);//单位分
                }
                else
                {
                    rmb = (uint)Convert.ToInt32((uint)Money * 100);//单位分
                }


                switch (model.Type)
                {
                    case 1:
                        gold = (uint)Money * 10000;
                        break; //游戏币
                    case 2:
                        dia = (uint)Money * 10;
                        break; //五币
                    default:
                        gold = (uint)Money * 10000;
                        gold = gold + 60000;
                        firstGif = true;
                        iF = isFirst.是;
                        break; //首冲(默认加6万游戏币)
                }


            //normal ServiceNormalS = normal.CreateBuilder()
            //  .SetUserID((uint)UserID)
            //  .SetGold(gold)
            //  .SetDia(dia)
            //  .SetRmb(rmb)//单位 分
            //  .SetFirstGif(firstGif)
            //  .SetBillNo(billNO)
            //  .Build();

            //string tbindStr = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
            return Content("1");
            //switch (tbindStr) {
            //    case "1":
            //        RechargeBLL.Add(
            //         new Recharge
            //         {
            //             BillNo = billNO,
            //             OpenID = billNO,
            //             UserID = model.UserID,
            //             Money = (long)rmb,
            //             CreateTime = DateTime.Now,
            //             Chip = gold,
            //             Diamond = dia,
            //             ChipType = (chipType)model.Type,
            //             IsFirst = iF,
            //             PF = raType.微信,
            //             PayItem = "Chip_8"


            //         });
            //        RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = billNO });
            //        return Content("1");
                  
            //    case "2":
            //        RechargeBLL.Add(
            //      new Recharge
            //      {
            //          BillNo = billNO,
            //          OpenID = billNO,
            //          UserID = model.UserID,
            //          Money = (long)rmb,
            //          CreateTime = DateTime.Now,
            //          Chip = gold,
            //          Diamond = dia,
            //          ChipType = (chipType)model.Type,
            //          IsFirst = iF,
            //          PF = raType.微信,
            //          PayItem = "Chip_8"


            //      });
            //        RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = billNO });
            //        return Content("2");
               
            //    case "3":
            //        return Content("3");
                 
            //    case "4":
            //        return Content("4");
                  
            //    default:
            //        return Content(tbindStr);

            //}



             
                //充值
            }

        [QueryValues]
        public ActionResult ChessFestival(Dictionary<string, string> queryvalues) {
            int _ClientType = queryvalues.ContainsKey("ClientType") ? Convert.ToInt32(queryvalues["ClientType"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartTime") ? queryvalues["StartTime"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            FestivalBaseData fbd = new FestivalBaseData() {
                 StartTime = _StartDate,
                  ExpirationDate = _ExpirationDate,
                   ClientType = _ClientType
            };

            IEnumerable<Festival515> fes515 = QQZoneRechargeBLL.GetFestival515(fbd);

            AllFesLogin login515 = QQZoneRechargeBLL.GetFestivalLogin(fbd);

            FestivalVIP vip515 =  QQZoneRechargeBLL.GetFestivalVIP(fbd);

            List<object> objList = new List<object>();
            objList.Add(fes515);
            objList.Add(login515);
            objList.Add(vip515);

            fbd.objects = objList;

            return View(fbd);
        }

    }
}