using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using GL.Protocol;
using Microsoft.AspNet.Identity;
using MWeb.Models;
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
    [Authorize]
    public class RobotController : Controller
    {
        // 彩金显示
        [QueryValues]
        public ActionResult PotGold(Dictionary<string, string> queryvalues)
        {
            //  gametype ChipGold ChipStandard
            int gametype = queryvalues.ContainsKey("gametype") ? Convert.ToInt32(queryvalues["gametype"]) : 0;
            decimal ChipGold = queryvalues.ContainsKey("ChipGold") ? Convert.ToInt32(queryvalues["ChipGold"]) : 0;
            decimal ChipStandard = queryvalues.ContainsKey("ChipStandard") ? Convert.ToInt32(queryvalues["ChipStandard"]) : 0;

            GameRecordView vbd = new GameRecordView
            {
            };


            List<PotGold>  data = new List<PotGold>(RobotBLL.GetPotGoldList());

            if (gametype != 0) {
                data.RemoveAll(m => m.GameType == gametype);

                data.Add(new PotGold() { GameType = gametype, Gold = ChipGold, Standard = ChipStandard });
            }

            vbd.DataList = data.OrderBy(m=>m.GameType).ToList();

            return View(vbd);
        }

        [QueryValues]
        public ActionResult PotGoldUpdate(Dictionary<string, string> queryvalues) {




            int gametype = queryvalues.ContainsKey("gametype") ? Convert.ToInt32(queryvalues["gametype"]) : 0;
            if (queryvalues.ContainsKey("ChipGold"))//修改操作,发送协议
            {
                double ChipGold;
                double ChipStandard;
                try
                {
                     ChipGold = queryvalues.ContainsKey("ChipGold") ? Convert.ToInt32(queryvalues["ChipGold"]) : 0;
                     ChipStandard = queryvalues.ContainsKey("ChipStandard") ? Convert.ToInt32(queryvalues["ChipStandard"]) : 0;
                }
                catch {
                    return Json(new { result = 1 });
                }



                ROBOT_POT_C ROBOT_POT_C;

                ROBOT_POT_C = ROBOT_POT_C.CreateBuilder()
                        .SetGameID((uint)gametype)
                        .SetPot(ChipGold)
                        .SetStand(ChipStandard)
                       .Build();



                Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_ROBOT_POT, ROBOT_POT_C.ToByteArray()));


                switch ((CenterCmd)tbind.header.CommandID)
                {
                    case CenterCmd.CS_SET_ROBOT_POT:
                        ROBOT_POT_S ROBOT_POT_S = ROBOT_POT_S.ParseFrom(tbind.body.ToBytes());
                        if (ROBOT_POT_S.Suc) {
                            return Json(new { result = 0 });
                        }
                        else {
                            return Json(new { result = 2 });
                        }
                           
                    case CenterCmd.CS_CONNECT_ERROR:
                        break;
                }


                return Json(new { result = 2 });


                //开始通信



            }
            else {//查看操作
                PotGold potGold = RobotBLL.GetPotGoldByType(gametype);


                return View(potGold);

             
            }
        }



        [QueryValues]
        public ActionResult PotRobotFlow(Dictionary<string, string> queryvalues)
        {
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
        
            string hidDataCount = queryvalues.ContainsKey("hidDataCount") ? queryvalues["hidDataCount"] : "";
            UserInfo u = null;
            GameRecordView grv = new GameRecordView {  StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };



            if (Request.IsAjaxRequest())
            {
                PagedList<UserMoneyRecord> data2 = UserMoneyRecordBLL.Get007ListByPage(grv, Convert.ToInt32(hidDataCount));
                ViewData["totalData"] = hidDataCount;
                return PartialView("PotRobotFlow_PageList", data2);
            }

        

                int total;
                PagedList<UserMoneyRecord> data = UserMoneyRecordBLL.Get007ListByPage(grv, out total);
                ViewData["totalData"] = total;
                grv.DataList = data;
            
         


        
            return View(grv);
        }





        //机器人金额显示
        [QueryValues]
        public ActionResult PotTotalSum(Dictionary<string, string> queryvalues) {

            int _Channels =10010;
          
           
          
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            GameRecordView vbd = new GameRecordView
            {
               
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate,
             
                Channels = _Channels,
              
            };
            //RobotOutPut


            vbd.DataList = new List<RobotOutPut>(RobotBLL.GetRobotOutPutList(vbd));

            return View(vbd);
        }

        [QueryValues]
        public ActionResult RobotManager(Dictionary<string, string> queryvalues) {

         
            //获取全部信息
            RobotControl robotCon = new RobotControl();

            QueryRobotStatu QueryRobotStatu;
            QueryRobotStatu = QueryRobotStatu.CreateBuilder()
                .SetModuleType(0)
                .Build();

            Bind tbind = Cmd.runClientRobot(new Bind(ServiceCmd.SC_ROBOT_QUERY, QueryRobotStatu.ToByteArray()),12001);
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_ROBOT_STATU:

                    RobotStatus RobotStatus = RobotStatus.ParseFrom(tbind.body.ToBytes());
                    uint moduleType = RobotStatus.ModuleType; 
                    IList<GameStatu> gameStatuList = RobotStatus.GameStatuList;
                    ProtoCmd.Service.GameOverView gameOverview = RobotStatus.GameOverview;
                    SystemStatu systemStatu =RobotStatus.SystemStatu;

               
                    RobotSystemStatu robotSystemStatu = new RobotSystemStatu
                    {
                        CPUPercent = systemStatu.CpuPercent.ToString(),
                        MemUsed = systemStatu.MemUsed.ToString(),
                        MemTotal = systemStatu.MemTotal.ToString(),
                        FlowRate = systemStatu.FlowRate.ToString(),
                        ConnCnt = (int)systemStatu.ConnCnt,
                        ConnLimit = (int)systemStatu.ConnLimit
                    };
                    robotCon.RobotSystemStatu = robotSystemStatu;
                  
                    GameStatu CARSGame = gameStatuList.FirstOrDefault(m => m.GameType == (int)ModuleType.CARS);
                    GameStatu TexasGame = gameStatuList.FirstOrDefault(m => m.GameType == (int)ModuleType.TEXAS);
                    GameStatu TEXAS_EXGame = gameStatuList.FirstOrDefault(m => m.GameType == (int)ModuleType.TEXAS_EX);
                    GameStatu ZFBGame = gameStatuList.FirstOrDefault(m => m.GameType == (int)ModuleType.ZFB);
                    GameStatu ZODIACGame = gameStatuList.FirstOrDefault(m => m.GameType == (int)ModuleType.ZODIAC);
                   

                    RobotGameStatu robotGameStatu = new RobotGameStatu
                    {
                        CARS = CARSGame.PlayerCnt,
                        All = CARSGame.PlayerCnt+ TexasGame.PlayerCnt+ ZFBGame.PlayerCnt+ ZODIACGame.PlayerCnt,
                        Texas = TexasGame.PlayerCnt,
                        ZFB = ZFBGame.PlayerCnt,
                        ZODIAC = ZODIACGame.PlayerCnt,
                        TEXAS_EX = TEXAS_EXGame.PlayerCnt
                    };

                    RobotDown robotDown = new RobotDown
                    {
                        CARS = CARSGame.IsBet > 0,
                        Texas = TexasGame.IsBet > 0,
                        ZFB = ZFBGame.IsBet > 0,
                        ZODIAC = ZODIACGame.IsBet > 0,
                        TEXAS_EX = TEXAS_EXGame.IsBet > 0
                    };
                    robotCon.RobotDown = robotDown;
                    robotCon.RobotGameStatu = robotGameStatu;

                    RobotPlayerLimit robotPlayerLimit = new RobotPlayerLimit
                    {
                        CARSLimit = CARSGame.PlayerLimitList.ToList(),
                        TexasLimit = TexasGame.PlayerLimitList.ToList(),
                        ZFBLimit = ZFBGame.PlayerLimitList.ToList(),
                        ZODIACLimit = ZODIACGame.PlayerLimitList.ToList(),
                        TEXAS_EXLimit = TEXAS_EXGame.PlayerLimitList.ToList()
                    };
                    robotCon.RobotPlayerLimit = robotPlayerLimit;
                    break;
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    res = 1,
                    data = robotCon
                
                });
            }
           
            return View(robotCon);
        }

        [QueryValues]
        public ActionResult RobotUpdateLimit(Dictionary<string, string> queryvalues) {
            int gametype = queryvalues.ContainsKey("gametype") ? Convert.ToInt32(queryvalues["gametype"]) : 0;
            int limit0 = queryvalues.ContainsKey("limit0") ? Convert.ToInt32(queryvalues["limit0"]) : 0;
            int limit1 = queryvalues.ContainsKey("limit1") ? Convert.ToInt32(queryvalues["limit1"]) : 0;
            int limit2 = queryvalues.ContainsKey("limit2") ? Convert.ToInt32(queryvalues["limit2"]) : 0;
            int limit3 = queryvalues.ContainsKey("limit3") ? Convert.ToInt32(queryvalues["limit3"]) : 0;
            int limit4 = queryvalues.ContainsKey("limit4") ? Convert.ToInt32(queryvalues["limit4"]) : 0;


            RobotControl robotCon = new RobotControl();

            ModifyRobotConfig ModifyRobotConfig;
            ModifyRobotConfig = ModifyRobotConfig.CreateBuilder()
                .SetGameType((uint)gametype)
                .AddLimit((uint)limit0)
                .AddLimit( (uint)limit1)
                .AddLimit((uint)limit2)
                .AddLimit((uint)limit3)
                .AddLimit((uint)limit4)
                .Build();

            Bind tbind = Cmd.runClientRobot(new Bind(ServiceCmd.SC_ROBOT_MODIFY, ModifyRobotConfig.ToByteArray()),12001);
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_ROBOT_STATU:
                    RobotStatus RobotStatus = RobotStatus.ParseFrom(tbind.body.ToBytes());
                    IList<GameStatu> gameStatuList = RobotStatus.GameStatuList;
                    GameStatu game = gameStatuList.FirstOrDefault(m => m.GameType == gametype);
                    string limits = "";
                    for (int i = 0; i < game.PlayerLimitList.Count(); i++) {
                        limits += game.PlayerLimitList[i] + ",";
                    }
                    limits = limits.Trim(',');
                    return Json(new 
                    {
                        res = 1,Data = limits
                    });
                  
            }
            return Json(new
            {
                res = 0,
                Data = ""
            });

        }
        [QueryValues]
        public ActionResult RobotLogon(Dictionary<string, string> queryvalues) {
            int gametype = queryvalues.ContainsKey("gametype") ? Convert.ToInt32(queryvalues["gametype"]) : -1;
            int openmodule = queryvalues.ContainsKey("openmodule") ? Convert.ToInt32(queryvalues["openmodule"]) : -1;
            int second = queryvalues.ContainsKey("second") ? Convert.ToInt32(queryvalues["second"]) : 0;
            int num = queryvalues.ContainsKey("num") ? Convert.ToInt32(queryvalues["num"]) : 0;

          

            RobotControl robotCon = new RobotControl();

            LoginRequest LoginRequest;
            LoginRequest = LoginRequest.CreateBuilder()
                .SetGameType((uint)gametype)
                .SetLoginFrequency((uint)second)
                .SetLoginCnt(num)
                .SetStrategy((uint)openmodule)
                .Build();

            Bind tbind = Cmd.runClientRobot(new Bind(ServiceCmd.SC_ROBOT_LOGIN, LoginRequest.ToByteArray()),12001);
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_ROBOT_STATU:
                   
                    return Json(new
                    {
                        res = 1,
                        Data = ""
                    });
                case CenterCmd.C2S_ROBOT_ERRMSG:
                    ErrorMessage ErrorMessage = ErrorMessage.ParseFrom(tbind.body.ToBytes());
                    return Json(new
                    {
                        res = 0,
                        Data = ErrorMessage.ErrorMessage_
                    });

            }
            return Json(new
            {
                res = 0,
                Data = ""
            });
        }
        [QueryValues]
        public ActionResult RobotDown(Dictionary<string, string> queryvalues) {
            int gametype = queryvalues.ContainsKey("gametype") ? Convert.ToInt32(queryvalues["gametype"]) : 0;
            int downtype = queryvalues.ContainsKey("downtype") ? Convert.ToInt32(queryvalues["downtype"]) : 0;


            RobotControl robotCon = new RobotControl();

            ModifyRobotConfig ModifyRobotConfig;
            ModifyRobotConfig = ModifyRobotConfig.CreateBuilder()
                .SetGameType((uint)gametype)
                .SetIsBet((uint)downtype)
                .Build();

            Bind tbind = Cmd.runClientRobot(new Bind(ServiceCmd.SC_ROBOT_MODIFY, ModifyRobotConfig.ToByteArray()), 12001);
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_ROBOT_STATU:
                    //RobotStatus robotStatus = RobotStatus.ParseFrom(tbind.body.ToBytes());
                    return Json(new
                    {
                        res = 1,
                        Data = "",
                        down = (downtype)
                    });

            }
            return Json(new
            {
                res = 0,
                Data = "",
                down = (downtype == 1 ? 0 : 1)
            });
        }
    }
}