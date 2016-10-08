using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WOHAIWeb.Controllers
{
    [UserAuthentication]
    public class BaseController : Controller
    {
        [QueryValues]
        public ActionResult OnlinePlay(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            terminals _Terminals = (terminals)(queryvalues.ContainsKey("Terminals") ? Convert.ToInt32(queryvalues["Terminals"]) : 0);
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView { Terminals = _Terminals, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            ViewData["Terminals"] = vbd.Terminals.ToSelectListItemForSelect();

            vbd.BaseDataList = new List<BaseDataInfoForOnlinePlay>(BaseDataBLL.GetOnlinePlay(vbd));


            return View(vbd);

        }

        


        [QueryValues]
        public ActionResult DailyOutput(Dictionary<string, string> queryvalues)
        {

            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            terminals _Terminals = (terminals)(queryvalues.ContainsKey("Terminals") ? Convert.ToInt32(queryvalues["Terminals"]) : 0);
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);



            BaseDataView vbd = new BaseDataView { Terminals = _Terminals, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby };

            ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
            ViewData["Terminals"] = vbd.Terminals.ToSelectListItemForSelect();

            vbd.BaseDataList = new List<BaseDataInfoForDailyOutput>(BaseDataBLL.GetDailyOutput(vbd));


            return View(vbd);

        }





    }
}