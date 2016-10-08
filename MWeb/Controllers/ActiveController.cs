using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using GL.Protocol;
using MWeb.protobuf.SCmd;
using ProtoCmd.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace MWeb.Controllers
{
    public class ActiveController : Controller
    {
        [QueryValues]
        public ActionResult RouletteData(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _target = queryvalues.ContainsKey("target") ? queryvalues["target"] : "";
            int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;

            BaseDataView vbd = new BaseDataView {StartDate = _StartDate, ExpirationDate = _ExpirationDate, Channels = _Channels, Page = _page };
            PagedList<Roulette> pa = new PagedList<Roulette>(BaseDataBLL.GetRouletteData(vbd), _pageGroup, 10);
            vbd.BaseDataList = pa;
            ViewData["dataGroup"] = vbd.BaseDataList;

            return View(vbd);
        }

        [QueryValues]
        public ActionResult RouletteDataDetail(Dictionary<string ,string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            BaseDataView vbd = new BaseDataView { Channels = _Channels, StartDate = _StartDate };
            IEnumerable<Roulette> detail = new List<Roulette>();
            detail = GL.Data.BLL.ActiveBLL.GetRouletteDataDetail(vbd);

            return View(detail);
        }

        [QueryValues]
        public ActionResult RouletteShopGet(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("ID") ? string.IsNullOrWhiteSpace(queryvalues["ID"]) ? 0 : Convert.ToInt32(queryvalues["ID"]) : 0;

            int res = BaseDataBLL.GetRouletteShop(_id);

            if (res > 0)
            {
                return Json(new { result = 0 });
            }
            return Json(new { result = 1 });

        }

        [QueryValues]
        public ActionResult RouletteShop(Dictionary<string, string> queryvalues)
        {
            int _id = queryvalues.ContainsKey("ID") ? Convert.ToInt32(queryvalues["ID"]) : 0;
            int _userID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;

            BaseDataView vbd = new BaseDataView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, UserID = _userID, Page = _page };
            PagedList<Roulette> pa = new PagedList<Roulette>(BaseDataBLL.GetRouletteShop(vbd), _pageGroup, 10);
            vbd.BaseDataList = pa;
            ViewData["dataGroup"] = vbd.BaseDataList;

            return View(vbd);
        }

        //[QueryValues]
        //public ActionResult RouletteLottery(Dictionary<string, string> queryvalues) {

        //    Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_QUERY_TURNTABLE_POT, new byte[0] { }));

            
        //    switch ((CenterCmd)tbind.header.CommandID)
        //    {
        //        case CenterCmd.CS_QUERY_TURNTABLE_POT:
        //            {
        //                TURNTABLEPOT_INFO_S TURNTABLEPOT_INFO_S = TURNTABLEPOT_INFO_S.ParseFrom(tbind.body.ToBytes());


        //                double d = TURNTABLEPOT_INFO_S.Pot;

        //                ViewData["pot"] = d;

        //            }
        //            break;
        //        case CenterCmd.CS_CONNECT_ERROR:
                  
        //            break;
        //    }

          


        //    return View();
        //}

        [QueryValues]
        public ActionResult RouletteLotteryUpdate(Dictionary<string, string> queryvalues) {
            string pot = queryvalues.ContainsKey("pot") ? queryvalues["pot"] : "";
            ViewData["pot"] = pot;
            return View();
        }

        [QueryValues]
        public ActionResult DuiHuanUserInfo(Dictionary<string, string> queryvalues) {

            int _id = queryvalues.ContainsKey("SearchExt") ? string.IsNullOrWhiteSpace(queryvalues["SearchExt"]) ? 0 : Convert.ToInt32(queryvalues["SearchExt"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
        
            GameRecordView grv = new GameRecordView { UserID = _id, Page = _page };

            if (Request.IsAjaxRequest())
            {
                return PartialView("DuiHuanUserInfo_PageList",
                    RoleBLL.GetRouletteDuiHuanInfo(grv.Page, _id));

               
            }

            grv.DataList = RoleBLL.GetRouletteDuiHuanInfo(grv.Page, _id);
            grv.SearchExt = _id.ToString();
            return View(grv);


          
        }
    }
}