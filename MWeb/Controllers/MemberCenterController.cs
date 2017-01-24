using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using ProtoCmd.Service;
using GL.Command.DBUtility;
using GL.Protocol;
using MWeb.protobuf.SCmd;
using GL.Data;
namespace MWeb.Controllers
{
    [Authorize]
    public class MemberCenterController : Controller
    {

        public static readonly string SystemType = PubConstant.GetConnectionString("SystemType");
        // GET: MemberCenter
        [QueryValues]
        public ActionResult MemberManagementList(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string userID = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";   //用户ID
            ViewData["Value"] = userID;
            PagedList<Role> model = MemberCenterBLL.GetAllListByPage(page, userID);

            return View(model);
        }
        //Abnormal user   IsFreeze

        [QueryValues]
        public ActionResult GetDataForAbnormalUser(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string userID = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";   //用户ID
            string type = queryvalues.ContainsKey("type") ? queryvalues["type"] : "";   //用户ID
            ViewData["Value"] = userID;
            PagedList<Role> model = null;
            if (!string.IsNullOrEmpty(userID))
            {
                model = MemberCenterBLL.GetAllListByPage(page, userID);
                return PartialView("MemberManagementList", model);
            }
            model = MemberCenterBLL.GetDataForAbnormalUser(page, type);
            return PartialView("MemberManagementList", model);
        }

        [QueryValues]
        public ActionResult MemberManagementListForStopIMEI(ValueView model, Dictionary<string, string> queryvalues)  //封IMEI
        {
            //   int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 10;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            //     string userID = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";   //用户ID
            //      string type = queryvalues.ContainsKey("type") ? queryvalues["type"] : "";   //用户ID
            //if (!string.IsNullOrEmpty(userID))
            //{
            //    PagedList<Role> mm = MemberCenterBLL.GetAllListByPage(page, userID);
            //    return PartialView("MemberManagementList", mm);
            //}
            //  ViewData["title"] = "黑名单";
            model.DataList = MemberCenterBLL.GetListByPageForMac(_page, model);
            return View(model);

        }


        [QueryValues]
        public ActionResult MemberManagementListForStopIP(ValueView model, Dictionary<string, string> queryvalues)  //封IMEI
        {
            //    int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 10;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            //   string userID = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";   //用户ID
            //   string type = queryvalues.ContainsKey("type") ? queryvalues["type"] : "";   //用户ID
            //  ViewData["title"] = "IP";
            //(CurrentPage - 1) * PageSize
            model.DataList = MemberCenterBLL.GetListByPageFor(_page, model);
            return View(model);

        }

        //MemberCenter
        [QueryValues]
        public ActionResult MemberManagementForDetails(Dictionary<string, string> queryvalues)
        {
            string _Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "0";
            //   string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"].ToString() : "";
            ViewData["Value"] = _Value;
            Role model = new Role();
            long a = 0;
            if (long.TryParse(_Value, out a) == false)
            {
                return View(model);
            }
            model = RoleBLL.GetVRoleByString(new Role() { ID = long.Parse(_Value) });
            if (model == null)
            {
                return View(model);
            }
            else
            {
                Role role = RoleBLL.GetGiftByString(new Role { ID = long.Parse(_Value) });
                model.Gift = role.Gift;
                model.GiftExpire = role.GiftExpire;

                if (model.ExtInfo == null)
                {
                    return View(model);
                }
                else
                {

                    //SystemType

                    if (SystemType == "2")
                    {

                        ////////////////////////////////底层修改///////////////////////////////////

                        BExtInfo BExtInfo = BExtInfo.ParseFrom(model.ExtInfo);

                        model.VipGrade = BExtInfo.VipInfo.Grade;  //VIP等级
                        model.VipPoint = BExtInfo.VipInfo.Current;  //VIP点数
                        model.LevelGrade = BExtInfo.LevelInfo.Level; //玩家等级
                        model.ItemCount = BExtInfo.ToolsInfo.ListToolsList.Count();//道具


                        //IList<GameInfo> gameInfoList = BExtInfo.UserExData.ListInfoList;
                        //德州扑克
                        //GameInfo game15 = gameInfoList.Where(m => m.GameID == 15).FirstOrDefault();
                        //gameinfo g15 = new gameinfo();
                        //if (game15 != null)
                        //{
                        //    g15.dwWin = Convert.ToInt32(game15.DwWin);
                        //    g15.dwTotal = Convert.ToInt32(game15.DwTotal);
                        //    g15.maxWinChip = game15.MaxWinChip;
                        //}
                        //else
                        //{
                        //    g15.dwWin = 0;
                        //    g15.dwTotal = 0;
                        //    g15.maxWinChip = 0;
                        //}

                        //model.GameInfo15 = g15;
                        ////中发白
                        //GameInfo game13 = gameInfoList.Where(m => m.GameID == 15).FirstOrDefault();
                        //gameinfo g13 = new gameinfo();
                        //if (game13 != null)
                        //{
                        //    g13.dwWin = Convert.ToInt32(game13.DwWin);
                        //    g13.dwTotal = Convert.ToInt32(game13.DwTotal);
                        //    g13.maxWinChip = game13.MaxWinChip;
                        //}
                        //else
                        //{
                        //    g13.dwWin = 0;
                        //    g13.dwTotal = 0;
                        //    g13.maxWinChip = 0;
                        //}

                        //model.GameInfo13 = g13;
                        //十二生肖
                        //GameInfo game14 = gameInfoList.Where(m => m.GameID == 15).FirstOrDefault();
                        //gameinfo g14 = new gameinfo();
                        //if (game14 != null)
                        //{
                        //    g14.dwWin = Convert.ToInt32(game14.DwWin);
                        //    g14.dwTotal = Convert.ToInt32(game14.DwTotal);
                        //    g14.maxWinChip = game14.MaxWinChip;
                        //}
                        //else
                        //{
                        //    g14.dwWin = 0;
                        //    g14.dwTotal = 0;
                        //    g14.maxWinChip = 0;
                        //}

                        //model.GameInfo14 = g14;


                        model.Friend = (short)BExtInfo.UserExData.WMaxFriend;

                        uint lastLoginTime = BExtInfo.UserExData.LastLoginTime;
                        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                        long lTime = long.Parse(lastLoginTime + "0000000");
                        TimeSpan toNow = new TimeSpan(lTime);
                        dtStart = dtStart.Add(toNow);
                        model.LastLoginTime = dtStart;

                        model.lastMoney = BExtInfo.UserExData.LastMoney;
                    }
                    else if (SystemType == "1")
                    {
                        int indexData = 4;
                        byte[] numData = new byte[4];
                        Array.Copy(model.ExtInfo, indexData, numData, 0, 4);
                        int num = System.BitConverter.ToInt32(numData, 0);
                        indexData = indexData + 4;

                        byte[] DataInfo = new byte[num];
                        Array.Copy(model.ExtInfo, 0, DataInfo, 0, num);
                        while (indexData < num)
                        {
                            //类型ID
                            int typeid = System.BitConverter.ToInt32(DataInfo, indexData);
                            indexData = indexData + 4;
                            int datanum = System.BitConverter.ToInt32(DataInfo, indexData);
                            indexData = indexData + 4;


                            //VIP等级
                            if (typeid == 1)
                            {
                                model.VipGrade = System.BitConverter.ToInt32(DataInfo, indexData);
                                model.VipPoint = System.BitConverter.ToInt32(DataInfo, indexData + 4);
                                indexData = indexData + datanum;
                            }
                            //玩家等级
                            else if (typeid == 2)
                            {
                                model.LevelGrade = System.BitConverter.ToInt32(DataInfo, indexData);
                                indexData = indexData + datanum;
                            }
                            //道具
                            else if (typeid == 4)
                            {
                                model.ItemCount = datanum / 24;
                                indexData = indexData + datanum;
                            }
                            //牌局
                            else if (typeid == 9)
                            {
                                int flag = indexData + datanum;
                                model.Friend = System.BitConverter.ToInt16(DataInfo, indexData);

                                uint lastLoginTime = System.BitConverter.ToUInt32(DataInfo, indexData + 4);
                                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                long lTime = long.Parse(lastLoginTime + "0000000");
                                TimeSpan toNow = new TimeSpan(lTime);
                                dtStart = dtStart.Add(toNow);
                                model.LastLoginTime = dtStart;

                                //long lastMoney = System.BitConverter.ToInt32(DataInfo, indexData + 16);
                                //model.lastMoney = lastMoney;

                                indexData = indexData + 32;
                                while (indexData < flag)
                                {
                                    short gameid = System.BitConverter.ToInt16(DataInfo, indexData);
                                    indexData = indexData + 4;
                                    switch (gameid)
                                    {
                                        case 15:  //德州
                                            gameinfo gameinfo15 = new gameinfo();
                                            gameinfo15.dwWin = System.BitConverter.ToInt32(DataInfo, indexData);
                                            gameinfo15.dwTotal = System.BitConverter.ToInt32(DataInfo, indexData + 4);
                                            gameinfo15.maxWinChip = System.BitConverter.ToInt32(DataInfo, indexData + 8);
                                            model.GameInfo15 = gameinfo15;
                                            indexData = indexData + 12;
                                            break;
                                        case 13:  //中发白
                                            gameinfo gameinfo13 = new gameinfo();
                                            gameinfo13.dwWin = System.BitConverter.ToInt32(DataInfo, indexData);
                                            gameinfo13.dwTotal = System.BitConverter.ToInt32(DataInfo, indexData + 4);
                                            gameinfo13.maxWinChip = System.BitConverter.ToInt32(DataInfo, indexData + 8);
                                            model.GameInfo13 = gameinfo13;
                                            indexData = indexData + 12;
                                            break;
                                        case 14:  //十二生肖  
                                            gameinfo gameinfo14 = new gameinfo();
                                            gameinfo14.dwWin = System.BitConverter.ToInt32(DataInfo, indexData);
                                            gameinfo14.dwTotal = System.BitConverter.ToInt32(DataInfo, indexData + 4);
                                            gameinfo14.maxWinChip = System.BitConverter.ToInt32(DataInfo, indexData + 8);
                                            model.GameInfo14 = gameinfo14;
                                            indexData = indexData + 12;
                                            break;
                                        default:
                                            indexData = flag;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                indexData = indexData + datanum;
                            }
                        }
                    }
                    else
                    {

                    }

                }

                // 计算在线状态
                UserStateReq UserStateReq;
                // model.Minu 传一个时间长短给服务器
                UserStateReq = UserStateReq.CreateBuilder()
                      .SetUserId(Convert.ToInt32(model.ID))
                       .Build();



                Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_USER_STATE, UserStateReq.ToByteArray()));


                switch ((CenterCmd)tbind.header.CommandID)
                {
                    case CenterCmd.CS_USER_STATE:
                        UserStateRes UserStateRes = UserStateRes.ParseFrom(tbind.body.ToBytes());
                        int state = UserStateRes.State;
                        int roomId = UserStateRes.RoomId;
                        int roomtype = UserStateRes.RoomType;
                        model.IsOnLine = state > 0 ? IsOnLine.在线 : IsOnLine.离线;
                        model.RoomID = roomId;
                        model.RoomType = (gameID)roomtype;
                        break;
                    case CenterCmd.CS_CONNECT_ERROR:
                        break;
                }
                BaseDataView vbd = new BaseDataView
                {
                    StartDate = DateTime.Now.AddDays(-10).Date.ToString(),
                    ExpirationDate = DateTime.Now.AddDays(1).Date.ToString(),
                    //Channels = _Channels,
                    SearchExt = _Value
                };

                IEnumerable<BaseDataInfo> ibd = BaseDataBLL.GetGameProfit(vbd);
                foreach (BaseDataInfo m in ibd)
                {
                    model.tenDayYinkui = m.ProfitAdd1 + m.ProfitDel1 +
                        m.ProfitAdd2 + m.ProfitDel2 +
                        m.ProfitAdd3 + m.ProfitDel3 +
                        m.ProfitAdd4 + m.ProfitDel4 +
                        m.ProfitAdd5 + m.ProfitDel5 +
                        m.ProfitAdd6 + m.ProfitDel6 +
                        m.ProfitAdd7 + m.ProfitDel7 +
                        m.ProfitAdd8 + m.ProfitDel8 +
                        m.ProfitAdd9 + m.ProfitDel9 +
                        m.ProfitAdd10 + m.ProfitDel10;
                }
                foreach (BaseDataInfo m in ibd)
                {
                    model.tenDayChanchu = m.ProfitAdd1 + m.ProfitAdd2 + m.ProfitAdd3 + m.ProfitAdd4 + m.ProfitAdd5 + m.ProfitAdd6 + m.ProfitAdd7 + m.ProfitAdd8 + m.ProfitAdd9 + m.ProfitAdd10;
                }
                foreach (BaseDataInfo m in ibd)
                {
                    model.tenDayXiaohao = m.ProfitDel1 + m.ProfitDel2 + m.ProfitDel3 + m.ProfitDel4 + m.ProfitDel5 + m.ProfitDel6 + m.ProfitDel7 + m.ProfitDel8 + m.ProfitDel9 + m.ProfitDel10;
                }
                model.RemarksName = MemberCenterBLL.GetRemarksNameByID(_Value);
            }
            BaseDataView vbd2 = new BaseDataView
            {
                StartDate = DateTime.Now.Date.ToString(),
                ExpirationDate = DateTime.Now.AddDays(1).Date.ToString(),
                //Channels = _Channels,
                SearchExt = _Value
            };
            if (_Value != "")
            {
                //ViewData["DetailProfit"] = BaseDataBLL.GetGameOutputDetailUser(vbd);
                ViewData["GameProfit"] = BaseDataBLL.GetGameProfit(vbd2);
            }
            else
            {
                ViewData["GameProfit"] = "";
            }
            Role rl = RoleBLL.GetModelByID(new Role { ID = Convert.ToInt64(_Value) });
            if (rl != null)
            {
                string nikeName = rl.NickName;
            }
            model.SendEmailCount = MemberCenterBLL.GetSendEmailCount(_Value);
            int fishCount = 0;
            GameRecordView grv = new GameRecordView { SearchExt = _Value, StartDate = DateTime.Now.AddDays(-1).Date.ToString(), ExpirationDate = DateTime.Now.AddDays(1).Date.ToString() };
            IEnumerable<UserFishInfo> Fishmodel = FishInfoBLL.GetUserInfo(grv);
            foreach (GL.Data.Model.UserFishInfo m in Fishmodel)
            {
                fishCount = fishCount + m.Fish1 * 200000;
                fishCount = fishCount + m.Fish2 * 1000000;
                fishCount = fishCount + m.Fish3 * 2000000;
                fishCount = fishCount + m.Fish4 * 5000000;
                fishCount = fishCount + m.Fish5 * 100000;
                fishCount = fishCount + m.Fish6 * 50000;
            }


            return View(model);
        }


        [QueryValues]
        public ActionResult updateForUser(OMModel model, Dictionary<string, string> queryvalues)
        {
            //Remarksname
            string id = queryvalues.ContainsKey("id") ? queryvalues["id"] : "";   //用户ID
            string name = queryvalues.ContainsKey("name") ? queryvalues["name"] : "";   //用户ID
            string type = queryvalues.ContainsKey("type") ? queryvalues["type"] : "";   //用户ID
            if (type == "Remarksname")
            {
                if (MemberCenterBLL.UpdateRemarksname(id, name) > 0)
                {
                    return Json(new { result = 0 });
                }
                else
                {
                    return Json(new { result = 1 });
                }
            }
            if (type == "NickName")
            {

                if (MemberCenterBLL.UpdateNickName(id, name) > 0)
                {
                    return Json(new { result = 0 });
                }
                else
                {
                    return Json(new { result = 1 });
                }
            }

            return Json(new { result = 0 });
        }


        [QueryValues]
        public ActionResult GetDetailsDataForUser(Dictionary<string, string> queryvalues)   //无效
        {

            string userID = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";   //用户ID
            BaseDataView vbd = new BaseDataView
            {
                StartDate = DateTime.Now.AddDays(-1).Date.ToString(),   //计算今天的
                ExpirationDate = DateTime.Now.AddDays(1).Date.ToString(),
                //Channels = _Channels,
                SearchExt = userID
            };
            UserRecord model = new UserRecord();

            IEnumerable<BaseDataInfo> ibd = BaseDataBLL.GetGameProfit(vbd);
            foreach (BaseDataInfo m in ibd)
            {
                model.TexasLost = m.ProfitDel1;
                model.TexasWin = m.ProfitAdd2;
                model.ScaleLost = m.ProfitDel2;
                model.ScaleWin = m.ProfitAdd2;
                model.ZodiacLost = m.ProfitDel3;
                model.ZodiacWin = m.ProfitAdd3;
                model.CarLost = m.ProfitDel5;
                model.CarWin = m.ProfitAdd5;
                model.HundredLost = m.ProfitDel6;
                model.HundredWin = m.ProfitAdd6;
            }
            model.TexasZyk = 0;
            model.ScaleZyk = MemberCenterBLL.GetScaleForZyinkui(int.Parse(userID)) * -1;
            model.ZodiacZyk = MemberCenterBLL.GetListForZodiacyinkui(int.Parse(userID));  //这个不用*-1  因为直接算的是庄家输赢 而不是玩家输赢的总和
            model.CarZyk = MemberCenterBLL.GetListForCaryinkui(int.Parse(userID));   //这个不用*-1  因为直接算的是庄家输赢 而不是玩家输赢的总和
            model.HundredZyk = MemberCenterBLL.GetListForHundredyinkui(int.Parse(userID));
            return View(model);
        }


        [QueryValues]
        public ActionResult MemberUpdateForVIP(Dictionary<string, string> queryvalues)
        {
            int _type = queryvalues.ContainsKey("Type") ? Convert.ToInt32(queryvalues["Type"]) : 0;
            int _id = queryvalues.ContainsKey("ID") ? Convert.ToInt32(queryvalues["ID"]) : 0;
            int _level = queryvalues.ContainsKey("Grade") ? Convert.ToInt32(queryvalues["Grade"]) : 0;
            int _point = queryvalues.ContainsKey("Point") ? Convert.ToInt32(queryvalues["Point"]) : 0;
            Role model = new Role();
            model = RoleBLL.GetRoleByString(new Role() { ID = _id });
            if (model == null)
            {
                return View(model);
            }
            model.UpdateProperty = _type;
            //数据修改操作
            if (Request.IsAjaxRequest())
            {
                //没有封号不能修改
                if (model.IsFreeze == isSwitch.开)
                {
                    return Content("-1");
                }
                if (_type == 3)     //绑定俱乐部
                {

                    //检查是不是俱乐部，不是俱乐部，不让绑定
                    IEnumerable<ClubInfo> clubs = ClubBLL.GetClubInfo(_level);
                    if (clubs != null && clubs.Count() > 0)
                    {

                    }
                    else
                    {
                        return Content("4");//不是俱乐部
                    }


                    Beland_Club_C BelandlubC;
                    // model.Minu 传一个时间长短给服务器
                    BelandlubC = Beland_Club_C.CreateBuilder()
                           .SetClubID((uint)_level)
                           .SetDwUserID((uint)_id)
                           .Build();



                    Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_BELAND_CLUB, BelandlubC.ToByteArray()));


                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_BELAND_CLUB:
                            Beland_Club_S BelandClubS = Beland_Club_S.ParseFrom(tbind.body.ToBytes());
                            bool res = BelandClubS.Suc;
                            if (res)
                            {
                                return Content("5");
                            }
                            else
                            {
                                return Content("6");
                            }

                        case CenterCmd.CS_CONNECT_ERROR:
                            break;
                    }

                    return Content("6");




                    //model.ClubID = _level;
                    //int res = RoleBLL.UpdateRoleClub(model);
                    //if(res == 1)    //绑定不是俱乐部ID
                    //{
                    //    return Content("4");    
                    //}
                    //else
                    //{
                    //    return Content(res.ToString());
                    //}
                }
                //玩家数据为空
                if (model.ExtInfo == null)
                {
                    return Content("-2");
                }
                else
                {
                    /////////////////////////////////////////////////////////////////////////////////

                    if (SystemType == "2")//万人德州
                    {

                        BExtInfo BExtInfo = BExtInfo.ParseFrom(model.ExtInfo);
                        if (_type == 1)  //修改VIP等级
                        {

                            VipInfo VipInfo22;

                            VipInfo22 = VipInfo.CreateBuilder(BExtInfo.VipInfo)
                                   .SetGrade((int)_level)
                                   .SetCurrent((int)_point)
                                   .Build();


                            BExtInfo = BExtInfo.CreateBuilder(BExtInfo)
                                   .SetVipInfo(VipInfo22)
                                   .Build();


                        }
                        else if (_type == 2) //修改等级
                        {

                            LevelInfo LevelInfo22;

                            LevelInfo22 = LevelInfo.CreateBuilder(BExtInfo.LevelInfo)
                                   .SetLevel(_level)
                                   .SetExp(_point)
                                   .Build();


                            BExtInfo = BExtInfo.CreateBuilder(BExtInfo)
                                   .SetLevelInfo(LevelInfo22)
                                   .Build();


                        }
                        byte[] bs = BExtInfo.ToByteArray();
                        model.ExtInfo = bs;
                        return Content(RoleBLL.UpdateRole(model).ToString());
                    }
                    else if (SystemType == "1")
                    {
                        byte[] Data = new byte[4];
                        Array.Copy(model.ExtInfo, 4, Data, 0, 4);
                        int num = System.BitConverter.ToInt32(Data, 0);
                        byte[] Grade = System.BitConverter.GetBytes(_level);
                        byte[] Point = System.BitConverter.GetBytes(_point);
                        for (int j = 0; j < 4; j++)
                        {
                            if (_type == 1)  //修改VIP等级
                            {
                                model.ExtInfo[16 + j] = Grade[j];
                                model.ExtInfo[20 + j] = Point[j];
                            }
                            else if (_type == 2) //修改等级
                            {
                                model.ExtInfo[36 + j] = Grade[j];
                                model.ExtInfo[40 + j] = Point[j];
                            }
                        }
                        return Content(RoleBLL.UpdateRole(model).ToString());
                    }
                    else
                    {


                    }
                }
            }
            //数据查询操作
            if (model.ExtInfo == null)
            {
                return View(model);
            }
            else
            {
                if (SystemType == "2")
                {
                    BExtInfo BExtInfo = BExtInfo.ParseFrom(model.ExtInfo);
                    model.VipGrade = BExtInfo.VipInfo.Grade;  //VIP等级
                    model.VipPoint = BExtInfo.VipInfo.Current;  //VIP等级
                    model.LevelGrade = BExtInfo.LevelInfo.Level; //玩家等级
                    model.LevelPoint = BExtInfo.LevelInfo.Exp; //玩家等级
                    return View(model);

                }
                else
                {
                    int indexData = 4;
                    byte[] numData = new byte[4];
                    Array.Copy(model.ExtInfo, indexData, numData, 0, 4);
                    int num = System.BitConverter.ToInt32(numData, 0);
                    indexData = indexData + 4;

                    byte[] DataInfo = new byte[num];
                    Array.Copy(model.ExtInfo, 0, DataInfo, 0, num);
                    while (indexData < num)
                    {
                        //类型ID
                        int typeid = System.BitConverter.ToInt32(DataInfo, indexData);
                        indexData = indexData + 4;
                        //数据长度
                        int datanum = System.BitConverter.ToInt32(DataInfo, indexData);
                        indexData = indexData + 4;
                        //VIP等级
                        if (typeid == 1)
                        {
                            model.VipGrade = System.BitConverter.ToInt32(DataInfo, indexData);
                            model.VipPoint = System.BitConverter.ToInt32(DataInfo, indexData + 4);
                            indexData = indexData + datanum;
                        }
                        //玩家等级
                        else if (typeid == 2)
                        {
                            model.LevelGrade = System.BitConverter.ToInt32(DataInfo, indexData);
                            model.LevelPoint = System.BitConverter.ToInt32(DataInfo, indexData + 4);
                            indexData = indexData + datanum;
                        }
                        else
                        {
                            indexData = indexData + datanum;
                        }
                    }
                    return View(model);
                }

            }
        }



        public ActionResult UserProfit(Dictionary<string, string> queryvalues)
        {
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _SearchExt = queryvalues.ContainsKey("Value") ? queryvalues["Value"].ToString() : "";
            //  string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            //    string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _StartDate = DateTime.Now.AddDays(-2).Date.ToString();
            string _ExpirationDate = DateTime.Now.AddDays(1).Date.ToString();
            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            BaseDataView vbd = new BaseDataView
            {
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate,
                Channels = _Channels,
                SearchExt = _SearchExt
            };

            ViewData["SearchExt"] = _SearchExt;
            //if (_SearchExt != "")
            //{
            //    //ViewData["DetailProfit"] = BaseDataBLL.GetGameOutputDetailUser(vbd);
            //    ViewData["GameProfit"] = BaseDataBLL.GetGameProfit(vbd);
            //}
            //else
            //{
            //    ViewData["GameProfit"] = "";
            //}
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("UserProfit_PageList", BaseDataBLL.GetUsreProfit(_page, vbd));
            //}
            //vbd.BaseDataList = BaseDataBLL.GetUsreProfit(_page, vbd);
            return View(BaseDataBLL.GetUsreProfit(_page, vbd));
        }

        [QueryValues]
        public ActionResult GameLog(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            ViewData["UserId"] = _id;
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
            GameRecordView grv = new GameRecordView { Gametype = Convert.ToInt32(_Gametype), Data = _data, UserID = _id, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, SeachType = (seachType)_seachtype, RoundID = _roundID, RoundID2 = _roundID2 };
            string pageList = "";
            switch (_Gametype)
            {
                case "1"://德州扑克
                    grv.DataList = MemberCenterBLL.GetListByPageForTexas(grv);
                    pageList = "TexasGameLog_PageList";
                    break;
                case "2"://中发白
                    grv.DataList = MemberCenterBLL.GetListByPageForScale(grv);
                    pageList = "ScaleGameLog_PageList";
                    break;
                case "3": //十二生肖
                    grv.DataList = GameDataBLL.GetListByPageForZodiac(grv);
                    pageList = "ZodiacGameLog_PageList";
                    break;
                case "5"://奔驰宝马
                    grv.DataList = GameDataBLL.GetListByPageForCar(grv);
                    pageList = "CarGameLog_PageList";
                    break;
                case "6"://百人德州
                    grv.DataList = GameDataBLL.GetListByPageForTexPro(grv);
                    pageList = "TexProGameLog_PageList";
                    break;
                case "9"://百家乐 ShuihuGameRecord  BaccaratGameRecord
                    grv.DataList = GameDataBLL.GetListByRoundForBaiJiaLe(grv);
                    pageList = "BaijialeGameLog_PageList";
                    break;
            }
            grv.PageList = pageList;
            if (Request.IsAjaxRequest())
            {
                return PartialView(pageList, grv.DataList);
            }
            return View(grv);

        }







        }
}