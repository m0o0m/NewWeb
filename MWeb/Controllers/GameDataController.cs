using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MWeb.protobuf.SCmd;
using ProtoCmd.Service;
using GL.Protocol;
using GL.Common;
using GL.Data;
using GL.Command.DBUtility;

namespace MWeb.Controllers
{
    [Authorize]
    public class GameDataController : Controller
    {
        public static readonly string Coin = PubConstant.GetConnectionString("coin");

        [QueryValues]
        public ActionResult LandGameLog(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };

            if (Request.IsAjaxRequest())
            {
                return PartialView("LandGameLog_PageList", GameDataBLL.GetListByPageForLand(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForLand(grv);

            return View(grv);

        }
        [QueryValues]
        public ActionResult GameLog(Dictionary<string, string> queryvalues)
        {


            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            string _Gametype = queryvalues.ContainsKey("Gametype") ? queryvalues["Gametype"] : "0";
            object _data = queryvalues.ContainsKey("Data") ? string.IsNullOrWhiteSpace(queryvalues["Data"]) ? 0 : Convert.ToInt64(queryvalues["Data"]) : 0;

            int _roundID = queryvalues.ContainsKey("RoundID") ? string.IsNullOrWhiteSpace(queryvalues["RoundID"]) ? 0 : Convert.ToInt32(queryvalues["RoundID"]) : 0;
            int _roundID2 = queryvalues.ContainsKey("RoundID2") ? string.IsNullOrWhiteSpace(queryvalues["RoundID2"]) ? 0 : Convert.ToInt32(queryvalues["RoundID2"]) : 0;
            int _RoundID3 = queryvalues.ContainsKey("RoundID3") ? string.IsNullOrWhiteSpace(queryvalues["RoundID3"]) ? 0 : Convert.ToInt32(queryvalues["RoundID3"]) : 0;

            GameRecordView grv = new GameRecordView { Gametype = Convert.ToInt32(_Gametype), Data = _data, UserID = _id, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype, RoundID = _roundID, RoundID2 = _roundID2 ,RoundID3= _RoundID3 };
            string pageList = "";
            switch (_Gametype)
            {
                case "0"://斗地主
                    grv.DataList = GameDataBLL.GetListByPageForLand(grv);
                    pageList = "LandGameLog_PageList";
                    break;
                case "1"://德州扑克
                    grv.DataList = GameDataBLL.GetListByPageForTexas(grv);
                    pageList = "TexasGameLog_PageList";
                    break;
                case "2"://中发白
                    grv.DataList = GameDataBLL.GetListByPageForScale(grv);
                    pageList = "ScaleGameLog_PageList";
                    break;
                case "3": //十二生肖
                    grv.DataList = GameDataBLL.GetListByPageForZodiac(grv);
                    pageList = "ZodiacGameLog_PageList";
                    break;
                case "4"://小马快跑
                    grv.DataList = GameDataBLL.GetListByPageForHorse(grv);
                    pageList = "PonyGameLog_PageList";
                    break;
                case "5"://奔驰宝马
                    grv.DataList = GameDataBLL.GetListByPageForCar(grv);
                    pageList = "CarGameLog_PageList";
                    break;
                case "6"://百人德州
                    grv.DataList = GameDataBLL.GetListByPageForTexPro(grv);
                    pageList = "TexProGameLog_PageList";
                    break;
                case "7"://水浒传 ShuihuGameRecord
                    grv.RoundID2 = 0;

                    grv.DataList = GameDataBLL.GetListByPageForShuihu(grv); //修改方法
                    pageList = "ShuihuGameLog_PageList";
                    break;
                case "8"://水果机   FruitGameRecord

                    grv.RoundID = 0;

                    grv.DataList = GameDataBLL.GetListByPageForShuiguo(grv); //修改方法
                    pageList = "ShuiguoGameLog_PageList";
                    break;
                case "9"://百家乐 ShuihuGameRecord  BaccaratGameRecord
                    grv.DataList = GameDataBLL.GetListByRoundForBaiJiaLe(grv);
                    pageList = "BaijialeGameLog_PageList";
                    break;
                case "10"://连环夺宝
                    grv.DataList = GameDataBLL.GetListByRoundForSerial(grv);
                    pageList = "SerialGameLog_PageList";
                    break;
            }
            grv.PageList = pageList;
            if (Request.IsAjaxRequest())
            {
                return PartialView(pageList, grv.DataList);
            }

            string[] str = new string[3];



            return View(grv);
        }







        [QueryValues]
        public ActionResult GameLogDetail(Dictionary<string, string> queryvalues)
        {



            string roundNo = queryvalues.ContainsKey("roundNo") ? queryvalues["roundNo"] : "0";

            TexasGameRecord re = new TexasGameRecord();

            // grv.DataList = GameDataBLL.GetListByPageForTexas(grv);

            re = GameDataBLL.GetTexasModelForRound(Convert.ToDecimal(roundNo));







            return View(re);
        }


        [QueryValues]
        public ActionResult TexasGameLog(Dictionary<string, string> queryvalues)
        {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("TexasGameLog_PageList", GameDataBLL.GetListByPageForTexas(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForTexas(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult ScaleGameLog(Dictionary<string, string> queryvalues)
        {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            object _data = queryvalues.ContainsKey("Data") ? Convert.ToInt64(queryvalues["Data"]) : 0;


            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype, Data = _data };

            if (Request.IsAjaxRequest())
            {
                return PartialView("ScaleGameLog_PageList", GameDataBLL.GetListByPageForScale(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForScale(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult ZodiacGameLog(Dictionary<string, string> queryvalues)
        {
            object _data = queryvalues.ContainsKey("Data") ? Convert.ToInt64(queryvalues["Data"]) : 0;


            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype, Data = _data };

            if (Request.IsAjaxRequest())
            {
                return PartialView("ZodiacGameLog_PageList", GameDataBLL.GetListByPageForZodiac(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForZodiac(grv);


            return View(grv);
        }

        [QueryValues]
        public ActionResult PonyGameLog(Dictionary<string, string> queryvalues)
        {
            object _data = queryvalues.ContainsKey("Data") ? Convert.ToInt64(queryvalues["Data"]) : 0;


            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype, Data = _data };

            if (Request.IsAjaxRequest())
            {
                return PartialView("PonyGameLog_PageList", GameDataBLL.GetListByPageForHorse(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForHorse(grv);


            return View(grv);
        }

        [QueryValues]
        public ActionResult CarGameGameLog(Dictionary<string, string> queryvalues)
        {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            object _data = queryvalues.ContainsKey("Data") ? Convert.ToInt64(queryvalues["Data"]) : 0;


            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype, Data = _data };

            if (Request.IsAjaxRequest())
            {
                return PartialView("CarGameLog_PageList", GameDataBLL.GetListByPageForCar(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForCar(grv);

            return View(grv);
        }


        /// <summary>
        /// 龙虎斗
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult LongHuGameLog(Dictionary<string, string> queryvalues)
        {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            object _data = queryvalues.ContainsKey("Data") ? Convert.ToInt64(queryvalues["Data"]) : 0;


            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype, Data = _data };

            if (Request.IsAjaxRequest())
            {
                return PartialView("LongHuGameLog_PageList", GameDataBLL.GetListByPageForLongHu(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForLongHu(grv);

            return View(grv);
        }





        [QueryValues]
        public ActionResult PotList(Dictionary<string, string> queryvalues)
        {
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");


            GameRecordView model = new GameRecordView { StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };

            if (Request.IsAjaxRequest())
            {
                return PartialView("PotList_PageList", TexasPotLogBLL.GetListByPage(model));
            }

            model.DataList = TexasPotLogBLL.GetListByPage(model);
            List<PotRecord> pr = new List<PotRecord>();

            var res1 = (TexasPot_Select_S)CmdResult.Result(ServiceCmd.SC_SELECT_TEXAS_POT, new byte[0]);
            if (res1 != null)
            {
                pr.Add(new PotRecord { ChipPot = res1.ChipNum, GameID = (int)gameID.德州扑克, TableID = 0 });
            }
            var res2 = (Scale_Select_S)CmdResult.Result(ServiceCmd.SC_SELECT_SCALE_POT, new byte[0]);
            if (res2 != null)
            {
                pr.Add(new PotRecord { ChipPot = res2.ChipNum, GameID = (int)gameID.中发白, TableID = 0 });
            }

            var res3 = (TexPro_Select_S)CmdResult.Result(ServiceCmd.SC_SELECT_TEXPRO_POT, new byte[0]);
            if (res3 != null)
            {
                pr.Add(new PotRecord { ChipPot = res3.ChipNum, GameID = (int)gameID.百人德州, TableID = 0 });
            }
            var res4 = (Baccarat_Select_S)CmdResult.Result(ServiceCmd.SC_SELECT_BACCARAT_POT, new byte[0]);
            if (res4 != null)
            {
                pr.Add(new PotRecord { ChipPot = res4.ChipNum, GameID = (int)gameID.澳门扑克, TableID = 0 });
            }



            model.Data = pr;
            return View(model);

        }

        [QueryValues]
        public ActionResult PotListUpdate(Dictionary<string, string> queryvalues)
        {
            PotRecordView model = new PotRecordView();
            int gameid = queryvalues.ContainsKey("gameid") ? Convert.ToInt32(queryvalues["gameid"]) : 0;
            object res;
            switch ((gameID)gameid)
            {
                case gameID.斗地主:
                    break;
                case gameID.中发白:
                    res = CmdResult.Result(ServiceCmd.SC_SELECT_SCALE_POT, new byte[0]);
                    if (res == null)
                    {
                        return RedirectToAction("PotList");
                    }
                    model = new PotRecordView { ChipPot = ((Scale_Select_S)res).ChipNum, GameID = gameID.中发白 };

                    break;
                case gameID.十二生肖:
                    break;
                case gameID.德州扑克:
                    res = CmdResult.Result(ServiceCmd.SC_SELECT_TEXAS_POT, new byte[0]);
                    if (res == null)
                    {
                        return RedirectToAction("PotList");
                    }
                    model = new PotRecordView { ChipPot = ((TexasPot_Select_S)res).ChipNum, GameID = gameID.德州扑克 };
                    break;
                case gameID.百人德州:
                    res = CmdResult.Result(ServiceCmd.SC_SELECT_TEXPRO_POT, new byte[0]);
                    if (res == null)
                    {
                        return RedirectToAction("PotList");
                    }
                    model = new PotRecordView { ChipPot = ((TexPro_Select_S)res).ChipNum, GameID = gameID.百人德州 };

                    break;
                case gameID.澳门扑克:
                    res = CmdResult.Result(ServiceCmd.SC_SELECT_BACCARAT_POT, new byte[0]);
                    if (res == null)
                    {
                        return RedirectToAction("PotList");
                    }
                    model = new PotRecordView { ChipPot = ((Baccarat_Select_S)res).ChipNum, GameID = gameID.澳门扑克 };

                    break;
                default:
                    break;
            }





            return View(model);

        }


        [HttpPost]
        [QueryValues]
        public ActionResult PotListUpdate(PotRecordView model, Dictionary<string, string> queryvalues)
        {

            if (model.ChipPot <= 0)
            {
                return Json(new { result = -900 });
            }

            if (string.IsNullOrWhiteSpace(model.Context))
            {
                return Json(new { result = -1000 });
            }
            if (model.Context.Length > 45)
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }
            if (!(model.ChipPot > 0 && model.ChipPot <= 1000000000))
            {
                return Json(new { result = -2000 });
            }

            switch (model.GameID)
            {
                case gameID.斗地主:
                    break;
                case gameID.中发白:
                    Scale_Operator_C ScaleOperatorC = Scale_Operator_C.CreateBuilder()
                    .SetOpType((uint)model.Flag)
                    .SetOpValue((uint)model.ChipPot)
                    .SetStrContent(model.Context)
                    .Build();

                    object ScaleOperatorS = CmdResult.Result(ServiceCmd.SC_OPERTOR_SACLE_POT, ScaleOperatorC.ToByteArray());

                    if (ScaleOperatorS != null)
                    {
                        bool res = ((Scale_Operator_S)ScaleOperatorS).Suc;

                        if (res)
                        {
                            if (model.Flag == 1)
                            {//增加
                                TexasPotLog log = new TexasPotLog
                                {
                                    Content = model.Context,
                                    Time = DateTime.Now,
                                    GameType = gameID.中发白,
                                    Type = ctrlType.增加,
                                    Value = model.ChipPot

                                };

                                TexasPotLogBLL.Add(log);
                            }
                            else
                            {//减少
                                TexasPotLog log = new TexasPotLog
                                {
                                    Content = model.Context,
                                    Time = DateTime.Now,
                                    GameType = gameID.中发白,
                                    Type = ctrlType.减少,
                                    Value = model.ChipPot

                                };

                                TexasPotLogBLL.Add(log);
                            }
                        }
                        return Json(new { result = res ? 0 : 1 });
                    }

                    break;
                case gameID.十二生肖:
                    break;
                case gameID.德州扑克:

                    TexasPot_Operator_C TexasPotOperatorC = TexasPot_Operator_C.CreateBuilder()
                    .SetOpType((uint)model.Flag)
                    .SetOpValue((uint)model.ChipPot)
                    .SetStrContent(model.Context)
                    .Build();

                    object TexasPotOperatorS = CmdResult.Result(ServiceCmd.SC_OPERTOR_TEXAS_POT, TexasPotOperatorC.ToByteArray());
                    if (TexasPotOperatorS != null)
                    {
                        bool res = ((TexasPot_Operator_S)TexasPotOperatorS).Suc;
                        if (res)
                        {
                            if (model.Flag == 1)
                            {//增加
                                TexasPotLog log = new TexasPotLog
                                {
                                    Content = model.Context,
                                    Time = DateTime.Now,
                                    GameType = gameID.德州扑克,
                                    Type = ctrlType.增加,
                                    Value = model.ChipPot

                                };

                                TexasPotLogBLL.Add(log);
                            }
                            else
                            {//减少
                                TexasPotLog log = new TexasPotLog
                                {
                                    Content = model.Context,
                                    Time = DateTime.Now,
                                    GameType = gameID.德州扑克,
                                    Type = ctrlType.减少,
                                    Value = model.ChipPot

                                };

                                TexasPotLogBLL.Add(log);
                            }
                        }
                        return Json(new { result = res ? 0 : 1 });
                    }
                    break;
                case gameID.百人德州:
                    TexPro_Operator_C TexPro_Operator_C = TexPro_Operator_C.CreateBuilder()
                    .SetOpType((uint)model.Flag)
                    .SetOpValue((uint)model.ChipPot)
                    .SetStrContent(model.Context)
                    .Build();

                    object TexPro_Operator_S = CmdResult.Result(ServiceCmd.SC_OPERTOR_TEXPRO_POT, TexPro_Operator_C.ToByteArray());

                    if (TexPro_Operator_S != null)
                    {
                        bool res = ((TexPro_Operator_S)TexPro_Operator_S).Suc;

                        if (res)
                        {
                            if (model.Flag == 1)
                            {//增加
                                TexasPotLog log = new TexasPotLog
                                {
                                    Content = model.Context,
                                    Time = DateTime.Now,
                                    GameType = gameID.百人德州,
                                    Type = ctrlType.增加,
                                    Value = model.ChipPot

                                };

                                TexasPotLogBLL.Add(log);
                            }
                            else
                            {//减少
                                TexasPotLog log = new TexasPotLog
                                {
                                    Content = model.Context,
                                    Time = DateTime.Now,
                                    GameType = gameID.百人德州,
                                    Type = ctrlType.减少,
                                    Value = model.ChipPot

                                };

                                TexasPotLogBLL.Add(log);
                            }
                        }
                        return Json(new { result = res ? 0 : 1 });
                    }
                    break;
                case gameID.澳门扑克:
                    Baccarat_Operator_C BaiJiaLeOperatorC = Baccarat_Operator_C.CreateBuilder()
                    .SetOpType((uint)model.Flag)
                    .SetOpValue((uint)model.ChipPot)
                    .SetStrContent(model.Context)
                    .Build();

                    object BaiJiaLeOperatorS = CmdResult.Result(ServiceCmd.SC_OPERTOR_BACCARAT_POT, BaiJiaLeOperatorC.ToByteArray());

                    if (BaiJiaLeOperatorS != null)
                    {
                        bool res = ((Baccarat_Operator_S)BaiJiaLeOperatorS).Suc;

                        if (res)
                        {
                            if (model.Flag == 1)
                            {//增加
                                TexasPotLog log = new TexasPotLog
                                {
                                    Content = model.Context,
                                    Time = DateTime.Now,
                                    GameType = gameID.澳门扑克,
                                    Type = ctrlType.增加,
                                    Value = model.ChipPot

                                };

                                TexasPotLogBLL.Add(log);
                            }
                            else
                            {//减少
                                TexasPotLog log = new TexasPotLog
                                {
                                    Content = model.Context,
                                    Time = DateTime.Now,
                                    GameType = gameID.澳门扑克,
                                    Type = ctrlType.减少,
                                    Value = model.ChipPot

                                };

                                TexasPotLogBLL.Add(log);
                            }
                        }
                        return Json(new { result = res ? 0 : 1 });
                    }

                    break;
                default:
                    break;
            }

            return Json(new { result = 2 });

        }

        /// <summary>
        /// 中发白大彩池数据统计
        /// </summary>
        /// <returns></returns>
        [QueryValues]
        public ActionResult ZFBPotDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("ZFBPotDataSum_PageList", GameDataBLL.GetListByPageForScalePot(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForScalePot(grv);

            return View(grv);
        }


        /// <summary>
        ///  百家乐大彩池数据统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
              [QueryValues]
        public ActionResult BaiJiaLePotDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("BaiJiaLePotDataSum_PageList", GameDataBLL.GetListByPageForBaiJiaLePot(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForBaiJiaLePot(grv);

            return View(grv);
        }




        /// <summary>
        /// 中发白大彩池打点统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult ZFBPotDotSum(Dictionary<string, string> queryvalues)
        {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            int Channels = queryvalues.ContainsKey("Channels") ? string.IsNullOrWhiteSpace(queryvalues["Channels"]) ? 0 : Convert.ToInt32(queryvalues["Channels"]) : 0;
            //Channels
            GameRecordView grv = new GameRecordView { Channels = Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("ZFBPotDotSum_PageList", GameDataBLL.GetListByPageForDotSum(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForDotSum(grv);

            return View(grv);
        }



        /// <summary>
        /// 百家乐大彩池打点统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult BaiJiaLePotDotSum(Dictionary<string, string> queryvalues)
        {  int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            int Channels = queryvalues.ContainsKey("Channels") ? string.IsNullOrWhiteSpace(queryvalues["Channels"]) ? 0 : Convert.ToInt32(queryvalues["Channels"]) : 0;
            //Channels
            GameRecordView grv = new GameRecordView { Channels = Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("BaiJiaLePotDotSum_PageList", GameDataBLL.GetListByPageForDotSumForBaiJiaLe(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForDotSumForBaiJiaLe(grv);

            return View(grv);
        }



        // ShuihuDataSum
        /// <summary>
        /// 水浒传数据统计
        /// </summary>
        /// <returns></returns>
        [QueryValues]
        public ActionResult ShuihuDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot

            int _RoundID = queryvalues.ContainsKey("RoundID") ? string.IsNullOrWhiteSpace(queryvalues["RoundID"]) ? 0 : Convert.ToInt32(queryvalues["RoundID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { RoundID = _RoundID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            if (Request.IsAjaxRequest()) //ShuihuDataSum
            {
                return PartialView("ShuihuDataSum_PageList", GameDataBLL.GetListByPageForShuihuDataSum(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForShuihuDataSum(grv);

            return View(grv);
        }


        // 
        /// <summary>
        /// 水果机数据统计
        /// </summary>
        /// <returns></returns>
        [QueryValues]
        public ActionResult FruitDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot

            int _RoundID = queryvalues.ContainsKey("RoundID") ? string.IsNullOrWhiteSpace(queryvalues["RoundID"]) ? 0 : Convert.ToInt32(queryvalues["RoundID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { RoundID = _RoundID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            if (Request.IsAjaxRequest()) //ShuihuDataSum
            {
                //FruitDataSum
                return PartialView("FruitDataSum_PageList", GameDataBLL.GetListByPageForFruitDataSum(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForFruitDataSum(grv);

            return View(grv);
        }




        [QueryValues]
        public ActionResult ShuihuChangguiDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot

            int _RoundID = queryvalues.ContainsKey("RoundID") ? string.IsNullOrWhiteSpace(queryvalues["RoundID"]) ? 0 : Convert.ToInt32(queryvalues["RoundID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { RoundID = _RoundID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            if (Request.IsAjaxRequest()) //  ShuihuChangguiDataSum
            {
                return PartialView("ShuihuChangguiDataSum_PageList", GameDataBLL.GetListByPageForShuihuChangguiDataSum(grv));
            }


            grv.DataList = GameDataBLL.GetListByPageForShuihuChangguiDataSum(grv);

            return View(grv);
        }



        [QueryValues]
        public ActionResult FruitChangguiDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot

            int _RoundID = queryvalues.ContainsKey("RoundID") ? string.IsNullOrWhiteSpace(queryvalues["RoundID"]) ? 0 : Convert.ToInt32(queryvalues["RoundID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView grv = new GameRecordView { RoundID = _RoundID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            if (Request.IsAjaxRequest()) //  ShuihuChangguiDataSum
            {
                return PartialView("FruitChangguiDataSum_PageList", GameDataBLL.GetListByPageForFruitChangguiDataSum(grv));
            }


            grv.DataList = GameDataBLL.GetListByPageForFruitChangguiDataSum(grv);

            return View(grv);
        }



        /// <summary>
        /// 百人德州大彩池数据统计
        /// </summary>
        /// <returns></returns>
        [QueryValues]
        public ActionResult TexProPotDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("TexProPotDataSum_PageList", GameDataBLL.GetListByPageForTexProPot(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForTexProPot(grv);

            return View(grv);
        }

        /// <summary>
        /// 百人德州大彩池打点统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult TexProPotDotSum(Dictionary<string, string> queryvalues)
        {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            int Channels = queryvalues.ContainsKey("Channels") ? string.IsNullOrWhiteSpace(queryvalues["Channels"]) ? 0 : Convert.ToInt32(queryvalues["Channels"]) : 0;
            //Channels
            GameRecordView grv = new GameRecordView { Channels = Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("TexProPotDotSum_PageList", GameDataBLL.GetListByPageForDotSum(grv, 2));
            }

            grv.DataList = GameDataBLL.GetListByPageForDotSum(grv, 2);

            return View(grv);
        }


        /// <summary>
        /// 翻翻乐打点统计
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult TexMiniDotSum(Dictionary<string, string> queryvalues)
        {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            int Channels = queryvalues.ContainsKey("Channels") ? string.IsNullOrWhiteSpace(queryvalues["Channels"]) ? 0 : Convert.ToInt32(queryvalues["Channels"]) : 0;
            //Channels
            GameRecordView grv = new GameRecordView { Channels = Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };

            if (Channels != 0)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("TexMiniDotSum_PageList", GameDataBLL.GetListByPageForDotSumByModuleID(grv, 24));
                }

                grv.DataList = GameDataBLL.GetListByPageForDotSumByModuleID(grv, 24);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("TexMiniDotSum2_PageList", GameDataBLL.GetMiniGameSum(grv));
                }

                grv.DataList = GameDataBLL.GetMiniGameSum(grv);
            }
            return View(grv);
        }


        //TexMiniDataSum
        /// <summary>
        /// 翻翻乐数据统计
        /// </summary>
        /// <returns></returns>
        [QueryValues]
        public ActionResult TexMiniDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("TexMiniDataSum_PageList", GameDataBLL.GetMiniGameSum(grv));
            }

            grv.DataList = GameDataBLL.GetMiniGameSum(grv);

            return View(grv);
        }





        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [QueryValues]
        public ActionResult ThanksGivingDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

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

            GameRecordView grv = new GameRecordView { Channels = _Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("ThanksGivingDataSum_PageList", GameDataBLL.GetListByPageForThanksGiving(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForThanksGiving(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult ThanksBaoGuang(Dictionary<string, string> queryvalues)
        {
            //ScalePot
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

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

            GameRecordView grv = new GameRecordView { Channels = _Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("ThanksBaoGuang_PageList", GameDataBLL.GetListByPageForThanksBaoGuang(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForThanksBaoGuang(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult ThanksRank(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            var arr = _StartDate.Split('-');
            DateTime dts = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(_StartDate.Substring(8, 2)), 0, 0, 0);
            DateTime dte = dts.AddDays(1);
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

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

            GameRecordView grv = new GameRecordView { Channels = _Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = dte.ToString(), Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("ThanksRank_PageList", GameDataBLL.GetThanksRankList(grv));
            }

            grv.DataList = GameDataBLL.GetThanksRankList(grv);

            return View(grv);
        }




        [QueryValues]
        public ActionResult MFestivalDataSum(Dictionary<string, string> queryvalues)
        {
            //ScalePot
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

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

            GameRecordView grv = new GameRecordView { Channels = _Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("MFestivalDataSum_PageList", GameDataBLL.GetListByPageForMFestival(grv));
            }

            grv.DataList = GameDataBLL.GetListByPageForMFestival(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult MFestivalRank(Dictionary<string, string> queryvalues)
        {
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            var arr = _StartDate.Split('-');
            DateTime dts = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(_StartDate.Substring(8, 2)), 0, 0, 0);
            DateTime dte = dts.AddDays(1);
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";

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

            GameRecordView grv = new GameRecordView { Channels = _Channels, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = dte.ToString(), Page = _page, SeachType = (seachType)_seachtype };
            if (Request.IsAjaxRequest())
            {
                return PartialView("MFestivalRank_PageList", GameDataBLL.GetMFestivalRankList(grv));
            }

            grv.DataList = GameDataBLL.GetMFestivalRankList(grv);

            return View(grv);
        }



    }
}