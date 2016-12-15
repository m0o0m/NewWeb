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
using log4net;
using GL.Command.DBUtility;

namespace MWeb.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        public static readonly string Coin = PubConstant.GetConnectionString("coin");
        public static readonly string SystemType = PubConstant.GetConnectionString("SystemType");

        // GET: Manage
        //[QueryValues]
        //public ActionResult Management(Dictionary<string, string> queryvalues)
        //{
        //    int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("Management_PageList", ManagerInfoBLL.GetListByPage(page));
        //    }


        //    PagedList<ManagerInfo> model = ManagerInfoBLL.GetListByPage(page);
        //    return View(model);
        //}



        //[QueryValues]
        //public ActionResult ManagementForAdd(Dictionary<string, string> queryvalues)
        //{
        //    int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

        //    ManagerInfo mi = new ManagerInfo();

        //    return View(mi);
        //}
        //[QueryValues]
        //public ActionResult ManagementForUpdate(Dictionary<string, string> queryvalues)
        //{
        //    int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;

        //    ManagerInfo mi = ManagerInfoBLL.GetModelByID(new ManagerInfo() { AdminID = id });


        //    if (mi == null)
        //    {
        //        return RedirectToAction("Management");
        //    }




        //    return View(mi);
        //}



        // GET: Manage
        //[QueryValues]
        //public ActionResult Agent(Dictionary<string, string> queryvalues)
        //{
        //    int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("Agent_PageList", AgentInfoBLL.GetListByPage(page));
        //    }


        //    PagedList<AgentInfo> model = AgentInfoBLL.GetListByPage(page);
        //    return View(model);
        //}


        [QueryValues]
        public ActionResult Member(Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            object _Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "0";


            //int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : Convert.ToInt32(_Value);
            long _upper = queryvalues.ContainsKey("upper") ? Convert.ToInt64(queryvalues["upper"]) : 0;
            long _lower = queryvalues.ContainsKey("lower") ? Convert.ToInt64(queryvalues["lower"]) : 0;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            int _lv = queryvalues.ContainsKey("lv") ? Convert.ToInt32(queryvalues["lv"]) : 0;
            BaseDataView bdv = new BaseDataView();
            ViewData["AllUser"] = BaseDataBLL.GetAllUser(bdv); ;
            if (_Value.ToString() == "")
            {
                if (page == 1)
                {
                    ViewData["Value"] = 0;
                    PagedList<Role> model1 = new PagedList<Role>(new List<Role>(), 1, 1);
                    return View(model1);
                }

            }


            if (_seachtype == 0)
            {
                _Value = queryvalues.ContainsKey("Value") ? Convert.ToInt32(queryvalues["Value"]) : 0;
            }



            MemberSeachView msv = new MemberSeachView { Value = _Value, PageIndex = page, LowerLimit = _lower, UpperLimit = _upper, SeachType = (seachType)_seachtype, Lv = _lv };
            ViewData["Value"] = _Value;
            ViewData["seachtype"] = _seachtype;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Member_PageList", RoleBLL.GetListByPage(msv));
            }
            PagedList<Role> model = RoleBLL.GetListByPage(msv);

            return View(model);
        }

        [QueryValues]
        public ActionResult MemberManage(Dictionary<string, string> queryvalues)
        {
            string _Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "0";
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
            else {
                Role role = RoleBLL.GetGiftByString(new Role { ID = long.Parse(_Value) });
                model.Gift = role.Gift;
                model.GiftExpire = role.GiftExpire;

                if (model.ExtInfo == null)
                {
                    return View(model);
                }
                else {

                    //SystemType

                    if (SystemType == "2")
                    {

                        ////////////////////////////////底层修改///////////////////////////////////

                        BExtInfo BExtInfo = BExtInfo.ParseFrom(model.ExtInfo);

                        model.VipGrade = BExtInfo.VipInfo.Grade;  //VIP等级
                        model.VipPoint = BExtInfo.VipInfo.Current;  //VIP点数
                        model.LevelGrade = BExtInfo.LevelInfo.Level; //玩家等级
                        model.ItemCount = BExtInfo.ToolsInfo.ListToolsList.Count();//道具


                        IList<GameInfo> gameInfoList = BExtInfo.UserExData.ListInfoList;
                        //德州扑克
                        GameInfo game15 = gameInfoList.Where(m => m.GameID == 15).FirstOrDefault();
                        gameinfo g15 = new gameinfo();
                        if (game15 != null)
                        {
                            g15.dwWin = Convert.ToInt32(game15.DwWin);
                            g15.dwTotal = Convert.ToInt32(game15.DwTotal);
                            g15.maxWinChip = game15.MaxWinChip;
                        }
                        else
                        {
                            g15.dwWin = 0;
                            g15.dwTotal = 0;
                            g15.maxWinChip = 0;
                        }

                        model.GameInfo15 = g15;
                        //中发白
                        GameInfo game13 = gameInfoList.Where(m => m.GameID == 15).FirstOrDefault();
                        gameinfo g13 = new gameinfo();
                        if (game13 != null)
                        {
                            g13.dwWin = Convert.ToInt32(game13.DwWin);
                            g13.dwTotal = Convert.ToInt32(game13.DwTotal);
                            g13.maxWinChip = game13.MaxWinChip;
                        }
                        else
                        {
                            g13.dwWin = 0;
                            g13.dwTotal = 0;
                            g13.maxWinChip = 0;
                        }

                        model.GameInfo13 = g13;
                        //十二生肖
                        GameInfo game14 = gameInfoList.Where(m => m.GameID == 15).FirstOrDefault();
                        gameinfo g14 = new gameinfo();
                        if (game14 != null)
                        {
                            g14.dwWin = Convert.ToInt32(game14.DwWin);
                            g14.dwTotal = Convert.ToInt32(game14.DwTotal);
                            g14.maxWinChip = game14.MaxWinChip;
                        }
                        else
                        {
                            g14.dwWin = 0;
                            g14.dwTotal = 0;
                            g14.maxWinChip = 0;
                        }

                        model.GameInfo14 = g14;


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
                    else {

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

                return View(model);
            }
        }

        [QueryValues]
        public ActionResult MemberUpdate(Dictionary<string, string> queryvalues)
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
                    else {
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
                            else {
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
                }else {
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
                    else {


                    }
                }
            }
            //数据查询操作
            if (model.ExtInfo == null)
            {
                return View(model);
            }
            else {
                if (SystemType == "2")
                {
                    BExtInfo BExtInfo = BExtInfo.ParseFrom(model.ExtInfo);
                    model.VipGrade = BExtInfo.VipInfo.Grade;  //VIP等级
                    model.VipPoint = BExtInfo.VipInfo.Current;  //VIP等级
                    model.LevelGrade = BExtInfo.LevelInfo.Level; //玩家等级
                    model.LevelPoint = BExtInfo.LevelInfo.Exp; //玩家等级
                    return View(model);

                }
                else {
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

        [QueryValues]
        public ActionResult MemberNoSpeak(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            object _Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";


            //int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : Convert.ToInt32(_Value);
            long _upper = queryvalues.ContainsKey("upper") ? Convert.ToInt64(queryvalues["upper"]) : 0;
            long _lower = queryvalues.ContainsKey("lower") ? Convert.ToInt64(queryvalues["lower"]) : 0;
            int _seachtype = (int)seachType.禁言;
            int _lv = queryvalues.ContainsKey("lv") ? Convert.ToInt32(queryvalues["lv"]) : 0;

            if (queryvalues.ContainsKey("Value") == false && page==1)//说明是从左边的菜单点击进来的,所以不会传递查询的值到后台
            {
                ViewData["Value"] = "";
                PagedList<Role> model1 = new PagedList<Role>(new List<Role>(), 1, 1);
                return View(model1);
            }



            MemberSeachView msv = new MemberSeachView { Value = _Value, PageIndex = page, LowerLimit = _lower, UpperLimit = _upper, SeachType = (seachType)_seachtype, Lv = _lv };

            ViewData["Value"] = _Value;
            ViewData["seachtype"] = _seachtype;

           


            if (Request.IsAjaxRequest())
            {
                return PartialView("MemberNoSpeak_PageList", RoleBLL.GetListByPage(msv));
            }
            PagedList<Role> model = RoleBLL.GetListByPage(msv);

            return View(model);
        }
        [QueryValues]
        public ActionResult MemberIsFreeze(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            object _Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";


            //int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : Convert.ToInt32(_Value);
            long _upper = queryvalues.ContainsKey("upper") ? Convert.ToInt64(queryvalues["upper"]) : 0;
            long _lower = queryvalues.ContainsKey("lower") ? Convert.ToInt64(queryvalues["lower"]) : 0;
            int _seachtype = (int)seachType.封号;
            int _lv = queryvalues.ContainsKey("lv") ? Convert.ToInt32(queryvalues["lv"]) : 0;

            if (queryvalues.ContainsKey("Value") == false && page==1)//说明是从左边的菜单点击进来的,所以不会传递查询的值到后台
            {
                ViewData["Value"] = "";
                PagedList<Role> model1 = new PagedList<Role>(new List<Role>(), 1, 1);
                return View(model1);
            }



            MemberSeachView msv = new MemberSeachView { Value = _Value, PageIndex = page, LowerLimit = _lower, UpperLimit = _upper, SeachType = (seachType)_seachtype, Lv = _lv };

            ViewData["Value"] = _Value;
            ViewData["seachtype"] = _seachtype;

            if (Request.IsAjaxRequest())
            {
                return PartialView("MemberIsFreeze_PageList", RoleBLL.GetListByPage(msv));
            }
            PagedList<Role> model = RoleBLL.GetListByPage(msv);

            return View(model);
        }





        [QueryValues]
        public ActionResult MemberForUpdate(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            Role model = RoleBLL.GetModelByID(new Role() { ID = id });
            if (model == null)
            {
                return RedirectToAction("Agent");
            }

            AgentInfo Higher = AgentInfoBLL.GetModelByID(new AgentInfo() { AgentID = model.Agent });
            if (Higher == null)
            {
                ViewBag.AgentLv = agentLv.公司;
                ViewBag.HigherID = 0;
                ViewBag.Top = true;
            }
            else
            {
                ViewBag.HigherID = Higher.AgentID;
                ViewBag.AgentLv = Higher.AgentLv + 1;
                ViewBag.Top = false;
            }

            ViewData["Higher"] = Higher;


            return View(model);
        }

                                                                  

        [QueryValues] //个人积分流水
        public ActionResult MemberForSearchGameScore(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            string userId = AgentUserBLL.GetUserListString(0); //new
            GameRecordView grv = new GameRecordView { UserList = userId, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            grv.Data = UserMoneyRecordBLL.GetUserInfo(grv);
            ViewData["modeldata"] = grv.Data;
            if (Request.IsAjaxRequest())
            {
              
                return PartialView("MemberForSearchGameScore_PageList", UserMoneyRecordBLL.GetListByPage(grv));
            }

            grv.DataList = UserMoneyRecordBLL.GetListByPage(grv);
          
            return View(grv);
        }
            
        private GameRecordView GetGameRecordView(GameRecordView grv) {
            UserInfo u=null;
            if (string.IsNullOrEmpty(grv.SearchExt))
            {//如果是空，不加载数据
                grv.DataList = new PagedList<UserMoneyRecord>(new List<UserMoneyRecord>(), 1, 1);
            }
            else
            {

                int total;
                PagedList<UserMoneyRecord> data = UserMoneyRecordBLL.GetListByPage(grv, out total);
                ViewData["totalData"] = total;
                grv.DataList = data;
                grv.UserList = AgentUserBLL.GetUserListString(0); //new
                 u = UserMoneyRecordBLL.GetUserInfo(grv);
                if (u != null)
                {
                    GameRecordView grv2 = new GameRecordView { UserID = u.ID, StartDate = grv.StartDate, ExpirationDate = grv.ExpirationDate, Page = grv.Page };
                    u.ServiceMoney = UserMoneyRecordBLL.GetUserInfoService(grv2);
                    u.Score = UserMoneyRecordBLL.GetUserInfoScore(grv2);

                }
            }
            grv.Data = u;
            return grv;
        }


        [QueryValues]//个人产出消耗
        public ActionResult MemberForSearchGameRecord(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            ViewData["SearchExt"] = _id;
            string hidDataCount = queryvalues.ContainsKey("hidDataCount") ? queryvalues["hidDataCount"] : "";
            UserInfo u = null;
            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };



            if (Request.IsAjaxRequest())
            {


                PagedList<UserMoneyRecord> data = UserMoneyRecordBLL.GetListByPage(grv, Convert.ToInt32(hidDataCount));
                ViewData["totalData"] = hidDataCount;


                u = UserMoneyRecordBLL.GetUserInfo(grv);
                if (u != null)
                {
                    GameRecordView grv2 = new GameRecordView { UserID = u.ID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
                    u.ServiceMoney = UserMoneyRecordBLL.GetUserInfoService(grv2);
                    u.Score = UserMoneyRecordBLL.GetUserInfoScore(grv2);

                }
                ViewData["modeldata"] = grv.Data;
                return PartialView("MemberForSearchGameRecord_PageList", data);


            }

            if (string.IsNullOrEmpty(_SearchExt))
            {//如果是空，不加载数据
                grv.DataList = new PagedList<UserMoneyRecord>(new List<UserMoneyRecord>(), 1, 1);
            }
            else {

                int total;
                PagedList<UserMoneyRecord> data = UserMoneyRecordBLL.GetListByPage(grv, out total);
                ViewData["totalData"] = total;
                grv.DataList = data;
                grv.UserList = AgentUserBLL.GetUserListString(0); //new
                u = UserMoneyRecordBLL.GetUserInfo(grv);
                if (u != null)
                {
                    GameRecordView grv2 = new GameRecordView { UserID = u.ID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
                    u.ServiceMoney = UserMoneyRecordBLL.GetUserInfoService(grv2);
                    u.Score = UserMoneyRecordBLL.GetUserInfoScore(grv2);
                    
                }
            }



            grv.Data = u;
            ViewData["modeldata"] = grv.Data;
            return View(grv);
        }

        [QueryValues]
        public ActionResult MemberForOpenFuDaiOutPut(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");



            GameRecordView grv = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };

            if (Request.IsAjaxRequest())
            {
                return PartialView("MemberForOpenFuDaiOutPut_PageList", OpenFuDaiBLL.GetListByPage(grv));
            }

            grv.DataList = OpenFuDaiBLL.GetListByPage(grv);
            grv.Data = OpenFuDaiBLL.GetOpenFuDai(grv);


            return View(grv);
        }

        [QueryValues]
        public ActionResult MemberForDuiHuan(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            string _UserList = AgentUserBLL.GetUserListString(0); //new
            string _MasterList = AgentUserBLL.GetUserListString(0); //new
            if (_Channels != 0)
            {
                _UserList = _Channels.ToString();
            }
            GameRecordView grv = new GameRecordView { UserList = _UserList, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };



            ViewData["Sum"] = DuiHuanBLL.GetSumDuiHuan(grv);

            if (Request.IsAjaxRequest())
            {
                return PartialView("MemberForDuiHuan_PageList", DuiHuanBLL.GetListByPage(grv));
            }

            grv.DataList = DuiHuanBLL.GetListByPage(grv);

            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
           x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
           ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;


            return View(grv);
        }
        private ILog log = LogManager.GetLogger("MemberForRecord");
        [QueryValues]
        public ActionResult MemberForRecord(Dictionary<string, string> queryvalues) {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            string _Gametype = queryvalues.ContainsKey("Gametype") ? queryvalues["Gametype"] : "0";
            int _itemID = queryvalues.ContainsKey("ItemID") ? Convert.ToInt32(queryvalues["ItemID"]) : 0;
            string _GametypeS = queryvalues.ContainsKey("GametypeS") ? queryvalues["GametypeS"] : "";
            string userId = AgentUserBLL.GetUserListString(0); //new
            GameRecordView grv = new GameRecordView { GametypeS= _GametypeS,  ItemID = _itemID, Gametype = Convert.ToInt32( _Gametype), UserList = userId, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            ViewData["SearchExt"] = _id;
            string hidDataCount = queryvalues.ContainsKey("hidDataCount") ? queryvalues["hidDataCount"] : "";
            UserInfo u = null;
            string pageList = "";
            switch (_Gametype)
            {
                case "0"://游戏币，太特殊，单独拿出来
                   
                    pageList = "MemberForSearchGameRecord_PageList";

                
                    break;
                case "1"://积分
                    grv.DataList = UserMoneyRecordBLL.GetListByPage(grv);
                    grv.Data = UserMoneyRecordBLL.GetUserInfo(grv);
                    ViewData["modeldata"] = grv.Data;
                    pageList = "MemberForSearchGameScore_PageList";
                    break;
                case "2"://经验
                    grv.DataList = ExpRecordBLL.GetListByPage(grv);
                    pageList = "ExpList_PageList";
                    break;
                case "3": //道具
                    if (string.IsNullOrEmpty(grv.SearchExt)) {
                        grv.SearchExt = "0";
                    }
                    grv.DataList = ExpRecordBLL.GetItemRecordList(grv);
                    pageList = "ItemList_PageList";
                    if (grv.SearchExt=="0")
                    {
                        grv.SearchExt = "";
                    }
                    break;
                case "4"://德州返点
                    grv.DataList = RebateBLL.GetListByPage(grv);
                    pageList = "DFanLi_PageList";
                    break;
                case "5"://拼手牌
                    if (string.IsNullOrEmpty(grv.SearchExt))
                    {
                        grv.DataList = new PagedList<SpellCard>(new List<SpellCard>(),1,1);
                    }
                    else {
                        grv.DataList = SpellCardBLL.GetListByPage(grv);
                    }
                    
                    pageList = "MatchCard_PageList";
                    break;
                case "6"://中发白大彩池
                   
                    grv.DataList = GameDataBLL.GetListByPageForScalePot(grv);
                   
                   
                    pageList = "ScalePot_PageList";

                    break;
                case "7"://百人德州大彩池

                    grv.DataList = GameDataBLL.GetListByPageForTexProPot(grv);


                    pageList = "TexProPot_PageList";
                    break;
                case "8"://百人游戏坐庄流水

                    grv.DataList = GameDataBLL.GetListByPageForTexasProAccording(grv);
               

                    pageList = "TexasProAccording_PageList";
                    break;
                case "9": //开心翻翻乐流水日志
                    grv.DataList = GameDataBLL.GetListByPageForMiniGameRecord(grv);


                    pageList = "HappyFanFan_PageList";
                    break;
                case "10"://百家乐大彩池

                    grv.DataList = GameDataBLL.GetListByPageForBaiJiaLePot(grv);


                    pageList = "BaiJiaLePot_PageList";

                    break;
            }

            grv.PageList = pageList;
            if (Request.IsAjaxRequest())
            {
                if (_Gametype == "0") {

                    log.Info("流水查询用户ID开始："+ grv.SearchExt);

                    PagedList<UserMoneyRecord> data = UserMoneyRecordBLL.GetListByPage(grv, Convert.ToInt32(hidDataCount));

                    log.Info("流水查询用户ID结束：" + grv.SearchExt);

                    ViewData["totalData"] = hidDataCount;

                    //u = UserMoneyRecordBLL.GetUserInfo(grv);
                    //if (u != null)
                    //{
                    //    GameRecordView grv2 = new GameRecordView { UserID = u.ID, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
                    //    u.ServiceMoney = UserMoneyRecordBLL.GetUserInfoService(grv2);
                    //    u.Score = UserMoneyRecordBLL.GetUserInfoScore(grv2);

                    //}
                    //grv.Data = UserMoneyRecordBLL.GetUserInfo(grv);
                    ViewData["modeldata"] = grv.Data;

               



                    return PartialView(pageList, data);
                }
                return PartialView(pageList, grv.DataList);
            }

            if (_Gametype == "0")
            {
                grv = GetGameRecordView(grv);
                ViewData["modeldata"] = grv.Data;
            }

            return View(grv);
        }





        [QueryValues]
        public ActionResult SearchMember(Dictionary<string, string> queryvalues)
        {
            GUIModel guim = new GUIModel();
            string values = queryvalues.ContainsKey("value") ? queryvalues["value"] : string.Empty;


            if (string.IsNullOrWhiteSpace(values))
            {
                guim.isSearch = 2;
                return View(guim);
            }

            Service_Query_C ServiceQueryC = Service_Query_C.CreateBuilder()
                .SetSzAccount(values)
                .Build();
            //Bind tbind = new Bind(ServiceCmd.SC_QUERY_USER, ServiceQueryC.ToByteArray());
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_QUERY_USER, ServiceQueryC.ToByteArray()));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_ACCOUNT_ERROR:
                    guim.isSearch = 1;
                    break;
                case CenterCmd.CS_QUERY_UESR:
                    Service_Query_S ServiceQueryS = Service_Query_S.ParseFrom(tbind.body.ToBytes());
                    guim.isSearch = 0;
                    guim.dwUserID = ServiceQueryS.DwUserID;
                    guim.szAccount = ServiceQueryS.SzAccount;
                    guim.szNickName = ServiceQueryS.SzNickName;
                    guim.szTelNum = ServiceQueryS.SzTelNum;
                    guim.szEmail = ServiceQueryS.SzEmail;
                    guim.isOnline = ServiceQueryS.IsOnline;
                    guim.isFreeze = ServiceQueryS.IsFreeze;
                    guim.szTrueName = ServiceQueryS.SzTrueName;
                    guim.szIdentity = ServiceQueryS.SzIdentity;
                    guim.sex = ServiceQueryS.Sex;
                    guim.dwAgentID = ServiceQueryS.DwAgentID;
                    guim.szCreateTime = ServiceQueryS.SzCreateTime;

                    break;
                case CenterCmd.CS_CONNECT_ERROR:
                    guim.isSearch = 100;
                    break;
            }


            if (Request.IsAjaxRequest())
            {

                switch (guim.isSearch)
                {
                    case 0:
                        return PartialView("SearchMember_Page", guim);
                    case 1:
                        return PartialView("SearchMember_NoSearch", guim);
                    default:
                        return PartialView("ServerDoesNotStart", guim);
                }


            }

            return View(guim);
        }

        [QueryValues]
        public ActionResult MemberList(Dictionary<string, string> queryvalues)
        {
           

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            object _Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "0";
            string leftMenuMark = queryvalues.ContainsKey("leftMenuMark") ? queryvalues["leftMenuMark"] : string.Empty;
            string robot = queryvalues.ContainsKey("robot") ? queryvalues["robot"] : string.Empty;
            
            //int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : Convert.ToInt32(_Value);
            long _upper = queryvalues.ContainsKey("upper") ? Convert.ToInt64(queryvalues["upper"]) : 0;
            long _lower = queryvalues.ContainsKey("lower") ? Convert.ToInt64(queryvalues["lower"]) : 0;
            int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            int _lv = queryvalues.ContainsKey("lv") ? Convert.ToInt32(queryvalues["lv"]) : 0;
            BaseDataView bdv = new BaseDataView();
            ViewData["AllUser"] = BaseDataBLL.GetAllUser(bdv);
            
            if (leftMenuMark != "leftMenu")
            {

                if (string.IsNullOrEmpty(_Value.ToString()))
                {
                    _Value = "0";
                }
                if (_seachtype == 10 || _seachtype == 11)
                {
                    _Value = "";
                }

                //点击按钮，分页操作
                MemberSeachView msv1 = new MemberSeachView { Value = _Value, PageIndex = page, LowerLimit = _lower, UpperLimit = _upper, SeachType = (seachType)_seachtype, Lv = _lv };

                ViewData["Value"] = _Value;
                ViewData["seachtype"] = _seachtype;

                if (Request.IsAjaxRequest())
                {
                    if (string.IsNullOrEmpty(robot))
                    {
                        return PartialView("Member_PageList", RoleBLL.GetListByPage(msv1));
                    }
                    else {
                        return PartialView("Member_PageList", RoleBLL.GetListByPage_No007(msv1));
                    }
                   
                }
                if (string.IsNullOrEmpty(robot))
                {
                    PagedList<Role> model1 = RoleBLL.GetListByPage(msv1);

                    return View(model1);
                }
                else {
                    PagedList<Role> model1 = RoleBLL.GetListByPage_No007(msv1);

                    return View(model1);
                }
                   


            }
            else {//说明是点击左侧菜单进来,直接不显示数据

                ViewData["Value"] = 0;
                PagedList<Role> model1 = new PagedList<Role>(new List<Role>(), 1, 1);
                return View(model1);
            }






            if (_Value.ToString() == "")
            {
                ViewData["Value"] = 0;
                PagedList<Role> model1 = new PagedList<Role>(new List<Role>(), 1, 1);
                return View(model1);
            }


            if (_seachtype == 0)
            {
                _Value = queryvalues.ContainsKey("Value") ? Convert.ToInt32(queryvalues["Value"]) : 0;
            }



            MemberSeachView msv = new MemberSeachView { Value = _Value, PageIndex = page, LowerLimit = _lower, UpperLimit = _upper, SeachType = (seachType)_seachtype, Lv = _lv };
            ViewData["Value"] = _Value;
            ViewData["seachtype"] = _seachtype;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Member_PageList", RoleBLL.GetListByPage(msv));
            }
            PagedList<Role> model = RoleBLL.GetListByPage(msv);

            return View(model);
        }



        [QueryValues]
        public ActionResult OnlineMember(Dictionary<string, string> queryvalues)
        {
            //long lineCount = ServiceStackManage.Redis.GetDatabase().HashLength("UserGate", CommandFlags.None);
            ////     public enum gameID
            ////{
            ////    斗地主 = 12,
            ////    中发白 = 13,
            ////    十二生肖 = 14,
            ////    德州扑克 = 15
            ////}
            //ViewBag.AllLineCount = lineCount;


            //int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            //object _Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";

            ////int _UserID = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : Convert.ToInt32(_Value);
            //long _upper = queryvalues.ContainsKey("upper") ? Convert.ToInt64(queryvalues["upper"]) : 0;
            //long _lower = queryvalues.ContainsKey("lower") ? Convert.ToInt64(queryvalues["lower"]) : 0;
            //int _seachtype = queryvalues.ContainsKey("seachtype") ? Convert.ToInt32(queryvalues["seachtype"]) : 0;
            //int _lv = queryvalues.ContainsKey("lv") ? Convert.ToInt32(queryvalues["lv"]) : 0;

            //if (_seachtype == 0)
            //{
            //    _Value = queryvalues.ContainsKey("Value") ? Convert.ToInt32(queryvalues["Value"]) : 0;
            //}



            //MemberSeachView msv = new MemberSeachView { Value = _Value, PageIndex = page, LowerLimit = _lower, UpperLimit = _upper, SeachType = (seachType)_seachtype, Lv = _lv };
            //ViewData["Value"] = _Value;
            //ViewData["seachtype"] = _seachtype;

            //Role userinfo = RoleBLL.GetModelByID(new Role { ID = Convert.ToInt32(_Value) });
            //List<OMModel> Model = new List<OMModel>();


            //if (userinfo != null)
            //{

            //    LoginRecord lr = LoginRecordBLL.GetModel(userinfo);

            //    var list = new List<Role> { userinfo };
            //    var res = ServiceStackManage.Redis.GetDatabase().HashScan("UserGame", _Value.ToString(), 10, CommandFlags.None);
            //    var res2 = ServiceStackManage.Redis.GetDatabase().HashScan("UserGate", _Value.ToString(), 10, CommandFlags.None);

            //    Model = (from x in list
            //             join y in res on x.ID equals Convert.ToInt32(y.Name) into odr
            //             join z in res2 on x.ID equals Convert.ToInt32(z.Name) into emps
            //             from o in odr.DefaultIfEmpty() 
            //             from m in emps.DefaultIfEmpty()
            //             select new OMModel
            //             {
            //                 strAccount = x.Account,
            //                 dwUserID = Convert.ToInt32(x.ID),
            //                 strIP = lr.IP,
            //                 strLoginTime = lr.LoginTime.ToString(),
            //                 strActiveTime = x.LastModify.ToString(),
            //                 strGame = o.Value,
            //                 online = (Convert.ToInt32(m.Value) > 0).ToString()
            //             }).ToList();



            //    //Service_Query_OnlineUser_S ServiceQueryOnlineUserS = Service_Query_OnlineUser_S.ParseFrom(tbind.body.ToBytes());

            //    //IEnumerable<Service_OnlineUserInfo> soui = ServiceQueryOnlineUserS.ListList;

            //    //List<OMModel> om = (from sss in soui
            //    //                    select new OMModel { dwUserID = (int)sss.DwUserID, strAccount = sss.StrAccount, strIP = sss.StrIP, strLoginTime = sss.StrLoginTime }).ToList();
            //    //PagedList<OMModel> model = new PagedList<OMModel>(om, (int)ServiceQueryOnlineUserS.Page, PageSize, (int)ServiceQueryOnlineUserS.PageTotal);

            //}
            // return View(Model);
            return View();
        }
        [QueryValues]
        public ActionResult OnPlayMember(Dictionary<string, string> queryvalues)
        {



            //long playCount = ServiceStackManage.Redis.GetDatabase().HashLength("UserGame", CommandFlags.None);
            ////     public enum gameID
            ////{
            ////    斗地主 = 12,
            ////    中发白 = 13,
            ////    十二生肖 = 14,
            ////    德州扑克 = 15
            ////}
            //ViewBag.AllPlayCount = playCount;


            //int value = queryvalues.ContainsKey("Value") ? Convert.ToInt32(queryvalues["Value"]) : 0;
            ////确认是否有这个人
            //Role userinfo = RoleBLL.GetModelByID(new Role { ID = value });
            //List<OMModel> Model = new List<OMModel>();
            //if (userinfo != null) {
            //    //查询出登录信息
            //    LoginRecord lr = LoginRecordBLL.GetModel(userinfo);
            //    //从redis查询在玩列,查询是否此会员是在玩,在玩什么
            //    if (lr != null) {
            //        bool isExi = ServiceStackManage.Redis.GetDatabase().HashExists("UserGame", value.ToString());
            //        if (isExi) {
            //            IEnumerable<HashEntry> res = ServiceStackManage.Redis.GetDatabase().HashScan("UserGame", value.ToString(), 10, CommandFlags.None);
            //            if (res != null)
            //            {
            //                //说明在在玩了列中有此会员
            //                Model = (from y in res
            //                         where y.Name.Equals(value)
            //                         select new OMModel
            //                         {
            //                             strAccount = userinfo.Account,
            //                             dwUserID = Convert.ToInt32(userinfo.ID),
            //                             strIP = lr.IP,
            //                             strLoginTime = lr.LoginTime.ToString(),
            //                             strActiveTime = userinfo.LastModify.ToString(),
            //                             strGame = Enum.GetName(typeof(gameID), Convert.ToInt32( y.Value.ToString().Trim('|')))
            //                         }).ToList();
            //            }
            //        }


            //    }

            //}

            //return View(Model);
            return View();
        }


        [QueryValues]
        public ActionResult OperationRecord(Dictionary<string, string> queryvalues)
        {


            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int dwUserID = queryvalues.ContainsKey("dwUserID") ? Convert.ToInt32(queryvalues["dwUserID"]) : 0;

            int PageSize = 10;

            Service_Query_UseOperHis_C ServiceQueryUseOperHisC = Service_Query_UseOperHis_C.CreateBuilder()
                .SetDwUserID((uint)dwUserID)
                .SetDwPageIndex((uint)page)
                .SetDwShowNum((uint)PageSize)
                .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_QUERY_USEROPERHIS, ServiceQueryUseOperHisC.ToByteArray()));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_QUERY_USEROPERHIS:
                    {
                        Service_Query_UseOperHis_S ServiceQueryUseOperHisS = Service_Query_UseOperHis_S.ParseFrom(tbind.body.ToBytes());

                        IEnumerable<Service_UserOperHisInfo> soui = ServiceQueryUseOperHisS.ListInfoList;
                        List<SUOHIModel> om = (from sss in soui
                                               select new SUOHIModel { strTime = sss.StrTime, content = sss.Content }).ToList();


                        PagedList<SUOHIModel> model = new PagedList<SUOHIModel>(om, (int)ServiceQueryUseOperHisS.Page, PageSize, (int)ServiceQueryUseOperHisS.PageTotal);


                        if (Request.IsAjaxRequest())
                        {
                            return PartialView("OnlineMember_PageList", model);
                        }

                        return View(model);

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            PagedList<SUOHIModel> Nullmodel = new PagedList<SUOHIModel>(new List<SUOHIModel>(), 1, 10, 0);
            return View(Nullmodel);

        }






        [QueryValues]
        public ActionResult UserEmail(Dictionary<string, string> queryvalues)
        {

            string _SearchExt = queryvalues.ContainsKey("UEUser") ? queryvalues["UEUser"] : "";
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string leftMenuMark = queryvalues.ContainsKey("leftMenuMark") ? queryvalues["leftMenuMark"] : string.Empty;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            if (queryvalues.ContainsKey("ExpirationDate") == false)
            {
                string[] ss = _StartDate.Split(' ');
                ss = ss[0].Split('-');
                DateTime d = new DateTime(Convert.ToInt32(ss[0]), Convert.ToInt32(ss[1]), Convert.ToInt32(ss[2]));
                _ExpirationDate = d.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            }
            //2016-04-26 00:00:00

            int Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;


            BaseDataView bdv = new BaseDataView() { Channels = Channels, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = page, SearchExt = _SearchExt };

            ViewData["UserTotal"] = UserEmailBLL.GetUserTotal(bdv);
            if (leftMenuMark != "leftMenu")
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("UserEmail_PageList", UserEmailBLL.GetListByPage(bdv));
                }
                PagedList<UserEmail> model1 = UserEmailBLL.GetListByPage(bdv);

                bdv.BaseDataList = model1;
                return View(bdv);
            }
            else
            {
                PagedList<UserEmail> model1 = new PagedList<UserEmail>(new List<UserEmail>(), 1, 1);
                bdv.BaseDataList = model1;
                return View(bdv);
            }
        }


        [QueryValues]
        public ActionResult SendUserEmail(Dictionary<string, string> queryvalues)
        {
            string UEContent = queryvalues.ContainsKey("UEContent") ? queryvalues["UEContent"] : string.Empty;
            string UETitle = queryvalues.ContainsKey("UETitle") ? queryvalues["UETitle"] : string.Empty;
            string UENote = queryvalues.ContainsKey("UENote") ? queryvalues["UENote"] : string.Empty;
            string UEUserID = queryvalues.ContainsKey("UEUserID") ? queryvalues["UEUserID"] : string.Empty;
            string UEItemType = queryvalues.ContainsKey("UEItemType") ? queryvalues["UEItemType"] : string.Empty;
            string UEItemValue = queryvalues.ContainsKey("UEItemValue") ? queryvalues["UEItemValue"] : string.Empty;
            string UEItemNum = queryvalues.ContainsKey("UEItemNum") ? queryvalues["UEItemNum"] : string.Empty;
            string leftMenuMark = queryvalues.ContainsKey("leftMenuMark") ? queryvalues["leftMenuMark"] : string.Empty;
            int awardType = queryvalues.ContainsKey("awardType") ? Convert.ToInt32( queryvalues["awardType"]) : 0;
            bool isUploadFile = queryvalues.ContainsKey("isUploadFile") ? object.Equals(queryvalues["isUploadFile"], "true") : false;

            bool isGlobal = queryvalues.ContainsKey("isGlobal") ? Convert.ToBoolean( queryvalues["isGlobal"]) : false;
            UEUserID = UEUserID.Trim(',');

            if (string.IsNullOrEmpty(UEUserID))
            {
             
            }
            else {
                string[] idStrs = UEUserID.Split(',');
                string s = "";
                for (int i = 0; i < idStrs.Length; i++)
                {
                    string[] ss = idStrs[i].Split('(');
                    s += ss[0] + ",";
                }
                UEUserID = s.Trim(',');
            }



            UserEmail ue = new UserEmail { UEUserID = UEUserID };

            UserLimit limit = SUBLL.GetLimitModel(new UserLimit { Category = 1, UserId = User.Identity.GetUserId() });
            Dictionary<int, double> dic = new Dictionary<int, double>();
            if (limit == null || string.IsNullOrEmpty(limit.AccessNo))
            {
                limit = new UserLimit();
                ViewData["dicFirst"] = double.MinValue;
                //dic.Add(1, double.MinValue); dic.Add(2, double.MinValue);
                //dic.Add(3, double.MinValue); dic.Add(4, double.MinValue);
                //dic.Add(5, double.MinValue); dic.Add(6, double.MinValue);
            }
            else
            {
                string lim = limit.AccessNo;
                // 1:1,2:22,3:,4:
                string[] strs = lim.Split(',');

                for (int i = 0; i < strs.Length; i++)
                {
                    string[] s = strs[i].Split(':');

                    if (!string.IsNullOrEmpty(s[1]))
                    {
                        dic.Add(Convert.ToInt32(s[0]), Convert.ToDouble(s[1]));
                        if (i == 0)
                        {
                            ViewData["dicFirst"] = Convert.ToDouble(s[1]);
                        }
                    }
                    else {
                        dic.Add(Convert.ToInt32(s[0]), double.MaxValue);
                        if (i == 0)
                        {
                            ViewData["dicFirst"] = double.MaxValue;
                        }
                    }


                }

            }



            if (User.Identity.GetUserName() == "admin")
            {
                ViewData["dicFirst"] = double.MaxValue;
                dic.Add(1, double.MaxValue); dic.Add(2, double.MaxValue);
                dic.Add(3, double.MaxValue); dic.Add(4, double.MaxValue);
                dic.Add(5, double.MaxValue); dic.Add(6, double.MaxValue);
            }

            ViewData["dic"] = dic;





            if (leftMenuMark == "leftMenu")//说明是左边菜单点击的,虽然是异步，但是需要返回视图
            {
                return View(ue);
            }


            if (Request.IsAjaxRequest())
            {

                if (string.IsNullOrWhiteSpace(UEUserID))
                {
                    if (isGlobal == false) {
                        return Json(new { result = Result.ValueCanNotBeNull });
                    }
                    
                }
               

                if (string.IsNullOrWhiteSpace(UEContent))
                {
                    return Json(new { result = Result.ValueCanNotBeNull });
                }
                if (string.IsNullOrWhiteSpace(UETitle))
                {
                    return Json(new { result = Result.ValueCanNotBeNull });
                }
                if (UETitle.Length > 60)
                {
                    return Json(new { result = Result.ValueIsTooLong });
                }
                if (UENote.Length > 128)
                {
                    return Json(new { result = Result.ValueIsTooLong });
                }
                if (UEContent.Length > 120)
                {
                    return Json(new { result = Result.ValueIsTooLong });
                }
                //if (!Utils.IsNumeric(UEItemType))
                //{
                //    return Json(new { result = Result.ValueIsNotNumber });
                //}
                if (!Utils.IsNumeric(UEItemValue))
                {
                    return Json(new { result = Result.ValueIsNotNumber });
                }
                if (Convert.ToInt32(UEItemValue) <= 0)
                {
                    if (isUploadFile == true)
                    {
                        return Json(new { result = Result.ValueIsNotNumber });
                    }


                }
                if (!Utils.IsNumeric(UEItemNum))
                {
                    return Json(new { result = Result.ValueIsNotNumber });
                }
                if (isGlobal) {
                    UEUserID = "0";
                }
                List<uint> UserIDList = UEUserID.Split(',').Select(x => Convert.ToUInt32(x)).ToList();

                if (UserIDList.Count > 10)
                {
                    return Json(new { result = Result.ValueIsTooLong });
                }


                //检测是否有对应的type权限
                if (UEItemType != "" && User.Identity.Name != "admin")
                {
                    string access = "," + limit.AccessNo.Trim(',');
                    if (!access.Contains("," + UEItemType + ":"))
                    {
                        return Json(new { result = -303 });
                    }
                    //检测数值是否正确
                    double valu = dic[Convert.ToInt32(UEItemType)];//数据库里面的设置的值
                    double beforeValu = 0;//前端传过来的值
                    if (Convert.ToInt32(UEItemType) <= 3)
                    {
                        beforeValu = Convert.ToDouble(UEItemValue);
                    }
                    else
                    {
                        beforeValu = Convert.ToDouble(UEItemNum);
                    }
                    if (beforeValu > valu)
                    {
                        return Json(new { result = -304 });
                    }

                }





                if (string.IsNullOrEmpty(UEItemValue))
                {
                    ViewData["daoju"] = "400";
                }
                else {
                    ViewData["daoju"] = UEItemValue;
                }

              
                Service_ItemMail_C ServiceItemMailC;

                if (!isUploadFile)
                {
                    ServiceItemMailC = Service_ItemMail_C.CreateBuilder()
                        .AddRangeUserID(UserIDList)
                        .SetTitle(UETitle)
                        .SetContext(UEContent)
                        .SetIsGlobal(isGlobal)
                        .SetAwardType(awardType)
                        .Build();
                }
                else
                {
                    UserLimit limitGlobal = SUBLL.GetLimitModel(new UserLimit { Category = 3, UserId = "0" });
                    if (isGlobal) {
                        string limitGs = "20000,20,20000";
                        if (limitGlobal != null) {
                            limitGs = limitGlobal.AccessNo;
                        }
                        string[] s = limitGs.Split(',');
                        if (UEItemType == "1") {
                            if (Convert.ToInt32(UEItemValue) > Convert.ToInt32(s[0])) {
                                return Json(new { result = -298 });
                            }
                        }
                        else if (UEItemType == "2")
                        {
                            if (Convert.ToInt32(UEItemValue) > Convert.ToInt32(s[1]))
                            {
                                return Json(new { result = -298 });
                            }
                        }
                        else if (UEItemType == "3")
                        {
                            if (Convert.ToInt32(UEItemValue) > Convert.ToInt32(s[2]))
                            {
                                return Json(new { result = -298 });
                            }
                        }
                    }

                    ServiceItemMailC = Service_ItemMail_C.CreateBuilder()
                        .AddRangeUserID(UserIDList)
                        .SetTitle(UETitle)
                        .SetContext(UEContent)
                        .SetItemType(Convert.ToUInt32(UEItemType))
                        .SetItemValue(Convert.ToUInt32(UEItemValue))
                        .SetItemNum(Convert.ToUInt32(UEItemNum))
                        .SetIsGlobal(isGlobal)
                        .SetAwardType(awardType)
                        .Build();
                }

                Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SEND_ITEMMAIL, ServiceItemMailC.ToByteArray()));

              
                string author = HttpContext.User.Identity.Name;

                if (UEItemType == "")
                {
                    UEItemType = "0";
                }
                if (isUploadFile==false) {
                    UEItemValue = "0";
                }
                ue = new UserEmail
                {
                    UEUserID = UEUserID,
                    UETime = DateTime.Now,
                    UETitle = UETitle,
                    UENote = UENote,
                    UEContent = UEContent,
                    UEAuthor = author,
                    UEItemNum = Convert.ToInt32(UEItemNum),
                    UEItemType = (ueItemType)Convert.ToInt32(UEItemType),
                    UEItemValue = Convert.ToInt32(UEItemValue),
                    IsGlobal = isGlobal
                };

              
                if ((CenterCmd)tbind.header.CommandID == CenterCmd.CS_SEND_ITEMMAIL)
                {
                    Service_ItemMail_S ServiceItemMailS = Service_ItemMail_S.ParseFrom(tbind.body.ToBytes());
                    if (ServiceItemMailS.Suc)
                    {

                      
                        int result = UserEmailBLL.Add(ue);


                        LogInfo info = new LogInfo()
                        {
                            UserAccount = User.Identity.Name,
                            Detail = "",
                            Content = "发送邮件",
                            CreateTime = DateTime.Now,
                            LoginIP = Request.UserHostAddress,
                            OperModule = "系统邮件"
                        };
                        if (isGlobal == true)
                        {
                            info.Detail = "全局邮件  邮件标题:" + ue.UETitle + @",
发送ID:" + ue.UEUserID + @",
";
                        }
                        else {
                            info.Detail = "邮件标题:" + ue.UETitle + @",
发送ID:" + ue.UEUserID + @",
";
                        }
                      
                        decimal conum = 1;
                        if (ue.UEItemType == ueItemType.金币 || ue.UEItemType == ueItemType.币 || ue.UEItemType == ueItemType.积分)
                        {
                            conum = ue.UEItemValue;
                        }
                        else
                        {
                            conum = ue.UEItemNum;
                        }
                        if (conum > 0)
                        {
                            if (ue.UEItemType.ToString() != "无")
                            {
                                info.Detail += "发送物品:" + ue.UEItemType.ToString() + ",";
                                info.Detail += "发送数额:" + conum + ",";



                            }
                        }

                        if (!string.IsNullOrEmpty(ue.UEContent))
                        {
                            info.Detail += "邮件内容:" + ue.UEContent + ",";
                        }
                        if (!string.IsNullOrEmpty(ue.UENote))
                        {
                            info.Detail += "邮件备注:" + ue.UENote + ",";
                        }


                        info.Detail = info.Detail.Trim(',');
                        SUBLL.AddLog(info);



                        if (result > 0)
                        {
                            return Json(new { result = Result.Redirect });
                        }
                    }

                }

                return Json(new { result = Result.Lost });
            }

            return View(ue);

        }



        [QueryValues]
        public ActionResult FishInfo(Dictionary<string, string> queryvalues)
        {

            string value = queryvalues.ContainsKey("UserID") ? queryvalues["UserID"] : "0";
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";


            long _id = RoleBLL.GetIdByIdOrAccoOrNName(SearchExt);



            GameRecordView grv = new GameRecordView { UserID = _id, SearchExt = SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };




            Service_Fish_C ServiceFishC = Service_Fish_C.CreateBuilder()
                .SetUserID((uint)grv.UserID)
                .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_QUERY_FISH, ServiceFishC.ToByteArray()));


            string UserName = grv.UserID == 0 ? "" : RoleBLL.GetModelByID(new Role() { ID = grv.UserID }).Account;


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_QUERY_FISH:
                    {
                        Service_Fish_S ServiceFishS = Service_Fish_S.ParseFrom(tbind.body.ToBytes());

                        IEnumerable<ProtoCmd.Service.FishInfo> soui = ServiceFishS.FishListList;
                        List<GL.Data.Model.FishInfo> om = (from sss in soui
                                                           select new GL.Data.Model.FishInfo { CreateTime = Convert.ToDateTime(sss.Date), FishID = (int)sss.ItemID, FishName = sss.Name, UserID = grv.UserID, NickName = UserName, GiveID = (int)sss.GiveID, GiveName = grv.UserID == (int)sss.GiveID ? UserName : RoleBLL.GetModelByID(new Role() { ID = (int)sss.GiveID }).Account }).ToList();



                        grv.DataList = om;

                        return View(grv);

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            List<GL.Data.Model.FishInfo> Nullmodel = new List<GL.Data.Model.FishInfo>() { };

            grv.DataList = Nullmodel;
            return View(grv);

        }

        [QueryValues]
        public ActionResult FishDaybook(Dictionary<string, string> queryvalues)
        {

            //int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            string SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            int _fishID = queryvalues.ContainsKey("fishID") ? Convert.ToInt32(queryvalues["fishID"]) : 0;
            int _commandID = queryvalues.ContainsKey("commandID") ? Convert.ToInt32(queryvalues["commandID"]) : 1;
            string ajax = queryvalues.ContainsKey("ajax") ? queryvalues["ajax"] : "";
            ViewData["searchExt"] = SearchExt;

            GameRecordView grv = new GameRecordView { SearchExt = SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page, ItemID = _fishID, commandID = _commandID };

            grv.Data = FishInfoBLL.GetUserInfo(grv);
            ViewData["modeldata"] = grv.Data;

            if (Request.IsAjaxRequest())
            {
                if (_fishID == 0)
                {
                    return PartialView("FishDaybook_PageList", FishInfoBLL.GetListByPage(grv));
                }
                else
                {
                    return PartialView("FishDaybook_PageList", FishInfoBLL.GetListByFish(grv));
                }
            }
            if (ajax == "ajax")
            {
                grv.DataList = FishInfoBLL.GetListByFish(grv);
                return View(grv);
            }

            grv.DataList = FishInfoBLL.GetListByPage(grv);
            return View(grv);
        }


        [QueryValues]
        public ActionResult FishCount(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            grv.DataList = FishInfoBLL.GetFishCount(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult FishCountOnUser(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            GameRecordView grv = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };

            if (Request.IsAjaxRequest())
            {
                return PartialView("FishCountOnUser_PageList", FishInfoBLL.FishCountOnUser(grv));
            }

            grv.DataList = FishInfoBLL.FishCountOnUser(grv);

            return View(grv);
        }

        [QueryValues]
        public ActionResult WhiteIP(Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string ip = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            BaseDataView bdv = new BaseDataView()
            {
                SearchExt = ip,
                Page = page
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("WhiteIP_PageList", IPWhiteListBLL.GetListByPage(bdv));
            }


            bdv.BaseDataList = IPWhiteListBLL.GetListByPage(bdv);

            return View(bdv);


        }

        [QueryValues]
        public ActionResult GetMailUsers(Dictionary<string, string> queryvalues) {
            string ids = queryvalues.ContainsKey("ids") ? queryvalues["ids"] : "";

          
            string[] idStrs = ids.Split(new char[] {' ',',','，' });
            string s = "";
            for (int i = 0; i < idStrs.Length; i++) {
                string id_i = idStrs[i].Replace(" ", "").Trim(',');
                if (id_i != "") { 
                    string[] ss = idStrs[i].Split('(');
                    s += ss[0] + ",";
                }
            }

            s = s.Trim(',');
            if (string.IsNullOrEmpty(s)) {
                return Json(new
                {
                    data = new List<string>()
                });
            }
            IEnumerable<Role> r = RoleBLL.GetModelByIDs(s);
            List<string> res = new List<string> ();
            foreach (var item in r)
            {
                string nname = "";
                if (!string.IsNullOrEmpty(item.NickName)) {
                    nname = item.NickName.Replace(',','，');
                }
                res.Add(""+item.ID + "(" + nname+ ")");
            }
           

            return Json(new {
                data = res
            } );
        }


    }
}