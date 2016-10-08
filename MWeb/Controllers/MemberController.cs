using GL.Common;
using GL.Data;
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
    [Authorize]
    public class MemberController : Controller
    {
        #region Member

        [QueryValues]
        public ActionResult Member(Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            ViewBag.AgentID = id;
            ViewBag.AgentLv = agentLv.公司;
            ViewBag.HigherID = 0;
            ViewBag.Top = false;




            if (Request.IsAjaxRequest())
            {
                return PartialView("Member_PageList", RoleBLL.GetListByPage(page, id));
            }


            AgentInfo mi = AgentInfoBLL.GetModelByID(new AgentInfo { AgentID = id });
            PagedList<Role> model = RoleBLL.GetListByPage(page, id);
            if (mi != null)
            {
                ViewBag.AgentLv = mi.AgentLv + 1;
                ViewBag.HigherID = mi.HigherLevel;
                ViewBag.Top = mi != null;


                ViewData["Higher"] = mi;

                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        [QueryValues]
        public ActionResult banip(OMModel model, Dictionary<string, string> queryvalues)
        {
            Service_BanIP_C ServiceBanIPC;

            switch (model.strAccount)
            {
                case "true":
                    ServiceBanIPC = Service_BanIP_C.CreateBuilder()
                        .SetIp(model.strIP)
                        .SetIsBan(true)
                        .Build();

                    break;
                default:
                    ServiceBanIPC = Service_BanIP_C.CreateBuilder()
                        .SetIp(model.strIP)
                        .SetIsBan(false)
                        .Build();

                    break;
            }

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_BAN_LOGIN_IP, ServiceBanIPC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_BAN_LOGIN_IP:
                    Service_BanIP_S ServiceBanIPS = Service_BanIP_S.ParseFrom(tbind.body.ToBytes());




                    if (model.strAccount== "true")
                    {//封注册IP
                        OperLogBLL.InsertFreezeLog(new FreezeLog()
                        {
                            UserID = -1,
                            IP = model.strIP,
                            CreateTime = DateTime.Now.ToString(),
                            OperUserName = User.Identity.Name,
                            Reason = "",
                            TimeSpan = "",
                            Type = "封注册IP"
                        });
                    }
                    else
                    {//解封注册IP
                        OperLogBLL.InsertFreezeLog(new FreezeLog()
                        {
                            UserID = -1,
                            IP = model.strIP,
                            CreateTime = DateTime.Now.ToString(),
                            OperUserName = User.Identity.Name,
                            Reason = "",
                            TimeSpan = "",
                            Type = "解封注册IP"
                        });
                    }







                    RoleBLL.UpdateIsFreeze(model.strIP, isSwitch.关);





                    return Json(new { result = ServiceBanIPS.Suc ? 0 : 1 });
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Json(new { result = 2 });


        }

        [HttpPost]
        [QueryValues]
        public ActionResult Whiteip(OMModel model, Dictionary<string, string> queryvalues)
        {
            //6
            if (model.Ext == "true") {
               bool ipw = IPWhiteListBLL.CheckWhiteIp( model.strIP);
                if (ipw==true)
                {
                    return Json(new { result = 6 });
                }
            }
         


            IPWHITE_C IPWHITEC;

            switch (model.Ext)
            {
                case "true":
                    IPWHITEC = IPWHITE_C.CreateBuilder()
                        .SetIp(model.strIP)
                        .SetAdd(true)//是添加
                        .Build();

                    break;
                default:
                    IPWHITEC = IPWHITE_C.CreateBuilder()
                        .SetIp(model.strIP)
                        .SetAdd(false)//是删除
                        .Build();

                    break;
            }

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_IP_WHITELIST, IPWHITEC.ToByteArray()));
            /*
            1 添加成功
            2 添加失败
            */
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_IP_WHITELIST:
                    IPWHITE_S IPWHITES = IPWHITE_S.ParseFrom(tbind.body.ToBytes());
                    if (IPWHITES.Suc == true)
                    { //操作成功
                        if (IPWHITES.Add == true)
                        {  //添加操作

                           bool addres = IPWhiteListBLL.Add(new Role { CreateIP = IPWHITES.Ip.Trim() });
                            if (addres)
                            {
                                return Json(new { result = 1 });
                            }
                            else {
                                return Json(new { result = 2 });
                            }
                        }
                        else
                        {//删除操作
                            bool delres = IPWhiteListBLL.Delete(new Role { CreateIP = IPWHITES.Ip.Trim() });
                            if (delres)
                            {
                                return Json(new { result = 3 });
                            }
                            else
                            {
                                return Json(new { result = 4 });
                            }
                        }
                    }
                    else {//操作失败,服务器返回false
                        return Json(new { result = 5 });
                    }
                  
                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 5 });
                   
            }

            return Json(new { result = 5 });


        }

        [HttpPost]
        [QueryValues]
        public ActionResult banMac(OMModel model, Dictionary<string, string> queryvalues)
        {
            Service_BanMac_C ServiceBanMacC;

            switch (model.strAccount)
            {
                case "true":
                    ServiceBanMacC = Service_BanMac_C.CreateBuilder()
                        .SetMacAddr(model.strIP)
                        .SetIsBan(true)
                        .Build();

                    break;
                default:
                    ServiceBanMacC = Service_BanMac_C.CreateBuilder()
                        .SetMacAddr(model.strIP)
                        .SetIsBan(false)
                        .Build();

                    break;
            }

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_BAN_LOGIN_MAC, ServiceBanMacC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_BAN_LOGIN_MAC:
                    Service_BanMac_S ServiceBanMacS = Service_BanMac_S.ParseFrom(tbind.body.ToBytes());

                    //RoleBLL.UpdateIsFreeze(model.strIP, isSwitch.关);



                    if (model.strAccount == "true")
                    {//封注册IP
                        OperLogBLL.InsertFreezeLog(new FreezeLog()
                        {
                            UserID = -1,
                            IMei = model.strIP,
                            CreateTime = DateTime.Now.ToString(),
                            OperUserName = User.Identity.Name,
                            Reason = "",
                            TimeSpan = "",
                            Type = "封IMei"
                        });
                    }
                    else
                    {//解封注册IP
                        OperLogBLL.InsertFreezeLog(new FreezeLog()
                        {
                            UserID = -1,
                            IMei = model.strIP,
                            CreateTime = DateTime.Now.ToString(),
                            OperUserName = User.Identity.Name,
                            Reason = "",
                            TimeSpan = "",
                            Type = "解封IMei"
                        });
                    }





                    return Json(new { result = ServiceBanMacS.Suc ? 0 : 1 });
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Json(new { result = 2 });


        }

        [HttpPost]
        [QueryValues]
        public ActionResult btnResetSafeBox(OMModel model, Dictionary<string, string> queryvalues)
        {
            Service_ResetSafePwd_C ServiceResetSafePwdC = Service_ResetSafePwd_C.CreateBuilder()
                        .SetDwUserID((uint)model.dwUserID)
                        .Build();
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_RESET_SAFEBOXPWD, ServiceResetSafePwdC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_RESET_SAFEBOXPWD:
                    Service_ResetSafePwd_S ServiceResetSafePwdS = Service_ResetSafePwd_S.ParseFrom(tbind.body.ToBytes());
                    bool res = ServiceResetSafePwdS.Suc;
                    if (res) {
                        RoleBLL.UpdateRoleSafePwd("123456", model.dwUserID);

                        return Json(new { result =  0 });
                    }
                    else {
                        RoleBLL.UpdateRoleSafePwd("123456", model.dwUserID);

                        return Json(new { result = 0 });
                    }
                   

                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Json(new { result = 2 });


        }






        #endregion

        #region 黑名单



        [QueryValues]
        public ActionResult BlackList(ValueView model, Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            if (Request.IsAjaxRequest())
            {
                return PartialView("BlackList_PageList", BlackListBLL.GetListByPageFor(page, model));
            }


            model.DataList = BlackListBLL.GetListByPageFor(page, model);

            return View(model);

        }

        [QueryValues]
        public ActionResult MacBlackList(ValueView model, Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            if (Request.IsAjaxRequest())
            {
                return PartialView("MacBlackList_PageList", BlackListBLL.GetListByPageForMac(page, model));
            }


            model.DataList = BlackListBLL.GetListByPageForMac(page, model);

            return View(model);

        }





        #endregion
        [QueryValues] //个人道具流水
        public ActionResult ItemList(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("SearchExt") ? string.IsNullOrWhiteSpace(queryvalues["SearchExt"]) ? 0 : Convert.ToInt32(queryvalues["SearchExt"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _itemID = queryvalues.ContainsKey("ItemID") ? Convert.ToInt32(queryvalues["ItemID"]) : 0;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            ViewData["SearchExt"] = _id;

            GameRecordView model = new GameRecordView {StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page , UserID = _id ,ItemID = _itemID,SearchExt = _id.ToString()};
            if (Request.IsAjaxRequest())
            {
                return PartialView("ItemList_PageList", ExpRecordBLL.GetItemRecordList(model));
            }
            model.DataList = ExpRecordBLL.GetItemRecordList(model);
            return View(model);
        }


        [QueryValues] //个人经验流水
        public ActionResult ExpList(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";


            GameRecordView model = new GameRecordView { SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };

            if (Request.IsAjaxRequest())
            {
                return PartialView("ExpList_PageList", ExpRecordBLL.GetListByPage(model));
            }


            model.DataList = ExpRecordBLL.GetListByPage(model);

            return View(model);

        }


        [QueryValues]
        public ActionResult CountNoviceTask(Dictionary<string, string> queryvalues)
        {    //
            string _target = queryvalues.ContainsKey("target") ? queryvalues["target"] : "";
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : 0;
            terminals _Terminals = (terminals)(queryvalues.ContainsKey("Terminals") ? Convert.ToInt32(queryvalues["Terminals"]) : 0);
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            groupby _Groupby = (groupby)(queryvalues.ContainsKey("groupby") ? Convert.ToInt32(queryvalues["groupby"]) : 1);
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


            if (_target == "boxgroup")
            {
               
               
                int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;
             
              

                BaseDataView vbd = new BaseDataView { UserList = _UserList, Channels = _Channels, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Page = _pageGroup };
                ViewData["taskGroup"] = BaseDataBLL.GetNoviceTaskPage(vbd);
                return PartialView("CountNoviceTaskGroup_PageList");
            }
            else if (_target == "box")
            {
                
           
                int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
              
          
                GameRecordView model = new GameRecordView { UserList = _UserList, UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
                ViewData["Data"] = FinishTaskBLL.GetListByPage(model);
                return PartialView("CountNoviceTask_PageList");
            }
            else {
                
             
                int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
                int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;
             
             

                BaseDataView vbd = new BaseDataView { UserList = _UserList, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Groupby = _Groupby, Page = _pageGroup };

                GameRecordView model = new GameRecordView { UserList = _UserList, UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
                //ViewData["groupby"] = vbd.Groupby.ToSelectListItemForSelect();
                //ViewData["Terminals"] = vbd.Terminals.ToSelectListItemForSelect();


                ViewData["taskGroup"] = BaseDataBLL.GetNoviceTaskPage(vbd);



                if (Request.IsAjaxRequest())
                {


                    return PartialView("CountNoviceTask_PageList", FinishTaskBLL.GetListByPage(model));
                }

                ViewData["Data"] = FinishTaskBLL.GetListByPage(model);

                return View(vbd);
            }
         
        }


        [QueryValues]
        public ActionResult RechargeForNoviceTask(Dictionary<string, string> queryvalues)
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

            GameRecordView model = new GameRecordView { Channels=_Channels, UserList = _UserList, SearchExt = _SearchExt, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };

            if (Request.IsAjaxRequest())
            {
                return PartialView("RechargeForNoviceTask_PageList", ToolsUseRecordBLL.GetListByPage(model));
            }


            model.DataList = ToolsUseRecordBLL.GetListByPage(model);

            List<SelectListItem> ieList = AgentUserBLL.GetUserList(_MasterList).Select(
        x => new SelectListItem { Text = x.AgentName, Value = x.Id.ToString(), Selected = x.Id == _Channels }
        ).ToList();
            ieList.Insert(0, new SelectListItem { Text = "所有渠道", Value = "0", Selected = 0 == _Channels });
            ViewData["Channels"] = ieList;

            return View(model);

        }   

        [HttpPost]
        [QueryValues]
        public ActionResult NoviceTaskExchange(Dictionary<string, string> queryvalues)
        {
           
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            GameRecordView model = new GameRecordView { UserID = _id };


            int res = ToolsUseRecordBLL.UpdateExchange(model);

            if (res > 0)
            {
                return Json(new { result = 0 });
            }
            return Json(new { result = 1 });

        }   


        [QueryValues]
        public ActionResult SearchUserTools(Dictionary<string, string> queryvalues)
        {
          
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");


            GameRecordView model = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };

            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("RechargeForNoviceTask_PageList", RoleBLL.GetModelByID(new Role { ID = model.UserID }));
            //}
            //byte[] resultRecv = new byte[newRecv.Count];
            //newRecv.CopyTo(resultRecv, 0);
            //Operator MyOper = new Operator();
            //MyOper = (Operator)BytesToStruct(resultRecv, MyOper.GetType()); // 将字节数组转换成结构

            var User = RoleBLL.GetModelByID(new Role { ID = model.UserID });

            byte[] resultRecv = new byte[User.ExtInfo.Length];
            User.ExtInfo.CopyTo(resultRecv, 0);
            SerializeDataHeader MyOper = new SerializeDataHeader();
            MyOper = (SerializeDataHeader)CtoC.BytesToStruct(resultRecv, MyOper.GetType()); // 将字节数组转换成结构


            ///
            byte[] ExtData = new byte[resultRecv.Length - MyOper.Length];
            CtoC.GetByte(resultRecv, 8).CopyTo(ExtData, 0);

            ExtData ed = new ExtData();
            ed = (ExtData)CtoC.BytesToStruct(ExtData, ed.GetType()); // 将字节数组转换成结构

            ///
            byte[] Data1 = new byte[ExtData.Length - ed.Length];
            CtoC.GetByte(ExtData, ed.Length).CopyTo(Data1, 0);

            userVip uv = new userVip();
            uv = (userVip)CtoC.BytesToStruct(Data1, uv.GetType()); // 将字节数组转换成结构


            ///
            byte[] Data2 = new byte[Data1.Length - uv.Length];
            CtoC.GetByte(Data1, uv.Length).CopyTo(Data2, 0);

            ExtData ed1 = new ExtData();
            ed1 = (ExtData)CtoC.BytesToStruct(Data2, ed1.GetType()); // 将字节数组转换成结构


            ///
            byte[] Data3 = new byte[Data2.Length - ed1.Length];
            CtoC.GetByte(Data2, ed1.Length).CopyTo(Data3, 0);

            RLEVEL rl = new RLEVEL();
            rl = (RLEVEL)CtoC.BytesToStruct(Data3, rl.GetType()); // 将字节数组转换成结构


            ///
            byte[] Data4 = new byte[Data3.Length - rl.Length];
            CtoC.GetByte(Data3, rl.Length).CopyTo(Data4, 0);

            ExtData ed2 = new ExtData();
            ed2 = (ExtData)CtoC.BytesToStruct(Data4, ed2.GetType()); // 将字节数组转换成结构



            ///
            byte[] Data5 = new byte[Data4.Length - ed2.Length];
            CtoC.GetByte(Data4, ed2.Length).CopyTo(Data5, 0);

            userPaijuInfo upj = new userPaijuInfo();
            upj = (userPaijuInfo)CtoC.BytesToStruct(Data5, upj.GetType()); // 将字节数组转换成结构




            model.Data = User;

            return View(model);

        }


        [QueryValues]
        public ActionResult RedisterIP(ValueView model, Dictionary<string, string> queryvalues)
        {
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");





            if (Request.IsAjaxRequest())
            {
                return PartialView("RedisterIP_PageList", RedisterIPBLL.GetListByPage(_page, model, _StartDate, _ExpirationDate));
            }


            model.DataList = RedisterIPBLL.GetListByPage(_page, model, _StartDate, _ExpirationDate);

            return View(model);

        }

        [QueryValues]
        public ActionResult IpBan(RedisterIP model)
        {


            return View(model);
        }


        [QueryValues]
        [HttpPost]
        public ActionResult IpBan(RedisterIP model, Dictionary<string, string> queryvalues)
        {

            if (!(model.Total > 0))
            {
                return Json(new { result = 200 });
            }
            if (!(model.Reason.Length > 0))
            {
                return Json(new { result = 200 });
            }


            Service_RegIP_C ServiceRegIPC = Service_RegIP_C.CreateBuilder()
                .SetIp(model.IP)
                .SetTotal((uint)model.Total)
                .SetContent(model.Reason)
                .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_REG_IP, ServiceRegIPC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_REG_IP:
                    Service_RegIP_S ServiceRegIPS = Service_RegIP_S.ParseFrom(tbind.body.ToBytes());
                    return Json(new { result = ServiceRegIPS.Suc ? 0 : 1 });
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Json(new { result = 2 });







            //return View(model);

        }

 

        [QueryValues]
        public ActionResult LoginList(ValueView model, Dictionary<string, string> queryvalues)
        { 
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int _pageGroup = queryvalues.ContainsKey("pageGroup") ? Convert.ToInt32(queryvalues["pageGroup"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            string _Value = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";
            //string _repeat = queryvalues.ContainsKey("repeat") ? queryvalues["repeat"] : "";
            string _target = queryvalues.ContainsKey("target") ? queryvalues["target"] : "";
            
            ViewData["seachtype"] = model.Type;

            model.Target = _target;
            model.StartDate = _StartDate;
            model.ExpirationDate = _ExpirationDate;
            model.Value = _Value;

            if (queryvalues.Count() == 0)
            {
                ViewData["Value"] = "0";
                PagedList<LoginRecord> model1 = new PagedList<LoginRecord>(new List<LoginRecord>(), 1, 1);
                model.DataList = model1;
                model.Value = "0";
                return View(model);
            }


            if (_Value == "0")
            {
                model.Value = null;

            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("LoginList_PageList", LoginRecordBLL.GetListByPage(_page, model));
            }



            model.DataList = LoginRecordBLL.GetListByPage(_page, model);

            return View(model);

            //if (_Value == "0")
            //{
            //    model.Value = null;

            //}

            //if (_target == "boxGroup")
            //{
            //    model.Page = _pageGroup;
            //    model.Data = LoginRecordBLL.GetRepeatByPage(model);
            //    return PartialView("LoginRepeat", model.Data);
            //}
            //else if (_target == "box")
            //{
            //    model.Page = _page;
            //    model.DataList = LoginRecordBLL.GetListByPage(_page, model);
            //    return PartialView("LoginList_PageList", model.DataList);
            //}
            //else
            //{
            //    model.Page = 1;
            //    model.DataList = LoginRecordBLL.GetListByPage(_page, model);
            //    model.Data = LoginRecordBLL.GetRepeatByPage(model);      
            //}            

            //return View(model);

        }


        
        [QueryValues]
        public ActionResult ClubList(Dictionary<string, string> queryvalues)
        {

            
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");



            GameRecordView model = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };
            Role r = RoleBLL.GetModelByID(new Role() { ID = _id });
            if (r == null)
            {
                model.DataList = new List<UserClub>();
            }
            else {
                model.DataList = ClubBLL.GetRebate(model);
            }

          
            if (Request.IsAjaxRequest())
            {
                return PartialView("ClubList_PageList", model.DataList);
            }


         

            return View(model);

        }

        [QueryValues]
        public ActionResult ClubListDetail(Dictionary<string, string> queryvalues)
        {

          
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");



            GameRecordView model = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };

            model.DataList = ClubBLL.GetRebateDetail(model);

            if (Request.IsAjaxRequest())
            {
                return PartialView("ClubListDetail", model);
            }

            return View(model);


        }

        

        [QueryValues]
        [HttpPost]
        public ActionResult SetClub(Dictionary<string, string> queryvalues)
        {

           
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            Service_ClubUserMode_C ServiceClubUserModeC = Service_ClubUserMode_C.CreateBuilder()
                .SetClubID((uint)_id)
                .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_CLUB_USERMODE, ServiceClubUserModeC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_CLUB_USERMODE:
                    Service_ClubUserMode_S ServiceClubUserModeS = Service_ClubUserMode_S.ParseFrom(tbind.body.ToBytes());
                    return Json(new { result = ServiceClubUserModeS.Suc ? 0 : 1 });
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Json(new { result = 2 });







            //return View(model);

        }

        [QueryValues]
        public ActionResult OffLine(Dictionary<string, string> queryvalues) {
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            Service_Kick_C ServiceKickC = Service_Kick_C.CreateBuilder()
              .SetDwUserID((uint)_id)
              .Build();



            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_KICK_USER, ServiceKickC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_KICK_USER:
                   // CANCEL_CLUB_S ServiceClubUserModeS = CANCEL_CLUB_S.ParseFrom(tbind.body.ToBytes());


                    //取消俱乐部





                   // return Json(new { result = ServiceClubUserModeS.Suc ? 0 : 1 });
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Json(new { result = 2 });
        }


        [QueryValues]
        public ActionResult CancleClub(Dictionary<string, string> queryvalues)
        {

            //取消俱乐部的功能能


            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;

            CANCEL_CLUB_C ServiceCancelClubModeC = CANCEL_CLUB_C.CreateBuilder()
                .SetDwUserID((uint)_id)
                .Build();

         

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_CANCEL_CLUB, ServiceCancelClubModeC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_CANCEL_CLUB:
                    CANCEL_CLUB_S ServiceClubUserModeS = CANCEL_CLUB_S.ParseFrom(tbind.body.ToBytes());


                    //取消俱乐部





                    return Json(new { result = ServiceClubUserModeS.Suc ? 0 : 1 });
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Json(new { result = 2 });








            //return View(model);

        }



        [QueryValues]
        public ActionResult ClubData(Dictionary<string, string> queryvalues) {
           int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");


            GameRecordView model = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };

            if (Request.IsAjaxRequest())
            {
                return PartialView("ClubData_PageList", ClubBLL.GetClubData(model));
            }


            model.DataList = ClubBLL.GetClubData(model);

            return View(model);        }


        [QueryValues]
        public ActionResult ClubDataDetail(Dictionary<string, string> queryvalues)
        {

            int _id = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"]) : 0;
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");



            GameRecordView model = new GameRecordView { UserID = _id, StartDate = _StartDate, ExpirationDate = _ExpirationDate, Page = _page };

            model.DataList = ClubBLL.GetClubDataDetail(model);

            if (Request.IsAjaxRequest())
            {
                return PartialView("ClubDataDetail", model);
            }

            return View(model);


        }

        [QueryValues]
        public ActionResult ClubConfig(Dictionary<string, string> queryvalues) {

            string SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"] : "";
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
          
            GameRecordView model = new GameRecordView { SearchExt = SearchExt,  Page = _page };

            if (Request.IsAjaxRequest())
            {
                return PartialView("ClubConfig_PageList", ClubBLL.GetCLoginUserListByPage(_page, SearchExt));
            }

            model.DataList = ClubBLL.GetCLoginUserListByPage(_page, SearchExt);




            return View(model);
        }

        [QueryValues]
        public ActionResult SetClubConfig(Dictionary<string, string> queryvalues) {
            string userAccount = queryvalues.ContainsKey("userAccount") ? queryvalues["userAccount"] : "";
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            int userId = queryvalues.ContainsKey("userId") ? Convert.ToInt32(queryvalues["userId"]) : -1;


            int SearchExt = queryvalues.ContainsKey("SearchExt") ? string.IsNullOrWhiteSpace(queryvalues["SearchExt"]) ? -1 : Convert.ToInt32(queryvalues["SearchExt"]) : -1;

            GameRecordView model = new GameRecordView {  Page = _page , UserID = userId, SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"]:""  };
            //查询此用户下的俱乐部
            if (Request.IsAjaxRequest())
            {
                return PartialView("SetClubConfig_PageList", ClubBLL.GetCLoginUserClubListByPage(_page, userId, SearchExt));
            }
            model.DataList = ClubBLL.GetCLoginUserClubListByPage(_page, userId, SearchExt);


            return View(model);
        }

        [QueryValues]
        public ActionResult CancelClubConfig(Dictionary<string, string> queryvalues) {
            int userId = queryvalues.ContainsKey("userId") ? Convert.ToInt32(queryvalues["userId"]) : -1;
            int clubid = queryvalues.ContainsKey("clubid") ? Convert.ToInt32(queryvalues["clubid"]) : -1;//俱乐部id
            int oper = queryvalues.ContainsKey("oper") ? Convert.ToInt32(queryvalues["oper"]) : -1;//操作
            CLoginUserClub club = new CLoginUserClub { ClubId = clubid, UserId = userId };
            bool res = false;
            if (oper > 0)
            {//取消操作
                res=ClubBLL.DeleteCLoginUserClub(club);
            }
            else {//添加操作
                res = ClubBLL.AddCLoginUserClub(club);
            }
            if (res)
            {
                return Content("1");
            }
            else {
                return Content("0");
            }
         
        }


        [QueryValues]
        public ActionResult CancelUserConfig(Dictionary<string, string> queryvalues) {

            int userId = queryvalues.ContainsKey("userId") ? Convert.ToInt32(queryvalues["userId"]) : -1;
            string UserAccount = queryvalues.ContainsKey("UserAccount") ? queryvalues["UserAccount"] : "";
            string pwd = queryvalues.ContainsKey("pwd") ? queryvalues["pwd"] :"";
            int oper = queryvalues.ContainsKey("oper") ? Convert.ToInt32(queryvalues["oper"]) : -1;//操作
            CLoginUser user = new CLoginUser { UserId = userId , UserAccount= UserAccount , UserPassword = pwd };
            bool res = false;
            if (oper > 0)
            {//取消操作
                res = ClubBLL.DeleteCLoginUser(user);
            }
            else
            {//添加操作

                //204已存在
                CLoginUser c = ClubBLL.GetCLoginUserByLoginName(user);
                if (c != null) {
                    return Json(new { result = 204 });
                }

                res = ClubBLL.AddCLoginUser(user);
            }
            if (res)
            {
                return Json(new { result = 1 });
            }
            else
            {
                return Json(new { result = 0 });
            }
        }

        [QueryValues]
        public ActionResult ClubConfigUserIndex(Dictionary<string, string> queryvalues) {
            return View();
        }


        [QueryValues]
        public ActionResult RebateManage(Dictionary<string, string> queryvalues)
        {
            string _groupID = queryvalues.ContainsKey("UserID") ? queryvalues["UserID"] : "0";
            GameRecordView model = new GameRecordView { };
            long a = 0;
            if (long.TryParse(_groupID, out a) == false)
            {
                return View(model);
            }

            model.UserID = long.Parse(_groupID) ;

            if (Request.IsAjaxRequest())
            {
                return PartialView("RebateManage_PageList", ClubBLL.GetRebateGroup(model));
            }

            model.DataList = ClubBLL.GetRebateGroup(model);
            return View(model);
        }


        [QueryValues]
        public ActionResult RebateDetail(Dictionary<string, string> queryvalues)
        {
            int _groupID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"]) : -1;
            GameRecordView model = new GameRecordView { UserID = _groupID };
            model.DataList = ClubBLL.GetRebateGroupDetail(model);

            if (Request.IsAjaxRequest())
            {
                return PartialView("RebateDetail", model);
            }

            return View(model);
        }

        [QueryValues]
        public ActionResult RebateAddInfo(Dictionary<string, string> queryvalues)
        {
            int _groupID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"]) : -1;
            string _groupName = queryvalues.ContainsKey("GroupName") ? queryvalues["GroupName"] : "";
            string _groupDesc = queryvalues.ContainsKey("GroupDesc") ? queryvalues["GroupDesc"] : "";
            int _per = queryvalues.ContainsKey("Per") ? Convert.ToInt32(queryvalues["Per"]) : 0;
            string _userID = queryvalues.ContainsKey("UserIDs") ? queryvalues["UserIDs"] : "";
            int _type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : 0;
            ViewData["type"] = _type;
            ViewData["groupid"] = _groupID;

            CLoginUser user = new CLoginUser { Name = _groupName, Desc = _groupDesc, Num = _per, ClubIds = _userID , GroupID = _groupID };

            if (_userID != "" || _groupName != "")
            {
                int res = 0;
                res = ClubBLL.AddCRebateUser(user);
                return Json(new { result = res });
            }
            return View();
        }

        [QueryValues]
        public ActionResult CancleGroup(Dictionary<string, string> queryvalues)
        {
            //取消返利分组的功能能
            int _groupID = queryvalues.ContainsKey("UserID") ? Convert.ToInt32(queryvalues["UserID"]) : -1;
            int _type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : 0;

            CLoginUser user = new CLoginUser { GroupID = _groupID , Num = _type };
            int res = 0;
            res = ClubBLL.CancleCRebateUser(user);
            if(res > 0)
            {
                return Json(new { result = 0 });
            }
            else
            {
                return Json(new { result = 1 });
            }
        }

        [QueryValues]
        public ActionResult Rebateuser(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string _SearchExt = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"].ToString() : "0";
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            ViewData["SearchExt"] = _SearchExt;

            BaseDataView vbd = new BaseDataView{StartDate = _StartDate, ExpirationDate = _ExpirationDate, SearchExt = _SearchExt };

            vbd.BaseDataList = ClubBLL.GetRebateuser(page, vbd);

            return View(vbd);
        }




        [QueryValues]
        public ActionResult FMPost(Dictionary<string, string> queryvalues)
        {
           
            int dwUserID = queryvalues.ContainsKey("dwUserID") ? Convert.ToInt32(queryvalues["dwUserID"]) : -1;

            Service_Kick_C ServiceKickC = Service_Kick_C.CreateBuilder()
                 .SetDwUserID((uint)dwUserID)
                 .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_KICK_USER, ServiceKickC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_KICK_USER:
                    Service_Kick_S ServiceKickS = Service_Kick_S.ParseFrom(tbind.body.ToBytes());
                    return Json(new { result = ServiceKickS.Suc ? 0 : 1 });
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return Json(new { result = 2 });
        }

        /// <summary>
        /// 封号，解封
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult FMPut(Dictionary<string, string> queryvalues)
        {
            int Minu = queryvalues.ContainsKey("Minu") ? Convert.ToInt32(queryvalues["Minu"]) : -1;
            int dwUserID = queryvalues.ContainsKey("dwUserID") ? Convert.ToInt32(queryvalues["dwUserID"]) : -1;
            int Reason = queryvalues.ContainsKey("Reason") ? Convert.ToInt32(queryvalues["Reason"]) : -1;

            DateTime newTime = DateTime.Now.AddMinutes(Minu);



            Service_Freeze_C ServiceFreezeC;
            // model.Minu 传一个时间长短给服务器
            ServiceFreezeC = Service_Freeze_C.CreateBuilder()
                   .SetDwUserID((uint)dwUserID)
                   .SetDwFreeze((uint)Reason)
                   .SetDwMinute((uint)Minu)
                   .Build();

         

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_FREEZE_USER, ServiceFreezeC.ToByteArray()));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_FREEZE_USER:

                 
                    Service_Freeze_S ServiceFreezeS = Service_Freeze_S.ParseFrom(tbind.body.ToBytes());
                    bool res = ServiceFreezeS.Suc;

                    if (Minu <= 0)
                    {//解封
                        OperLogBLL.InsertFreezeLog(new FreezeLog() {
                            UserID = dwUserID,
                             IP = "",
                              IMei = "",
                            CreateTime = DateTime.Now.ToString(),
                            OperUserName = User.Identity.Name,
                             Reason  ="",
                              TimeSpan = "",
                               Type = "解封"
                        });
                    }
                    else
                    {//封号
                        OperLogBLL.InsertFreezeLog(new FreezeLog()
                        {
                            UserID = dwUserID,
                            IP = "",
                             IMei = "",
                            CreateTime = DateTime.Now.ToString(),
                            OperUserName = User.Identity.Name,
                            Reason =(( FreezeStatus)Reason).ToString(),
                            TimeSpan = ((FreezeTimeSpanStatus)Minu).ToString(),
                            Type = "封号"
                        });
                    }




                    if (res)
                    {
                        return Json( new { result = 0, Times = "已封号(" + (Minu >= 5256000 ? "永久" : "截止" + newTime.ToString()) + ")" });
                    }
                    else
                    {
                        if (Reason == -1) {
                            Reason = 0;
                        }
                        RoleBLL.UpdateRoleNoFreeze(
                           Reason, DateTime.Now.AddMinutes(Minu),
                             dwUserID
                               );

                        return Json( new { result = 0, Times = "已封号(" + (Minu >= 5256000 ? "永久" : "截止" + newTime.ToString()) + ")" });
                    }

              
                


                case CenterCmd.CS_CONNECT_ERROR:

                   
                    break;
            }

            return Json( new { result = 2 });




        }


        [QueryValues]
        public ActionResult FMDelete(Dictionary<string, string> queryvalues)
        {
            int Minu = queryvalues.ContainsKey("Minu") ? Convert.ToInt32(queryvalues["Minu"]) : -1;
            int dwUserID = queryvalues.ContainsKey("dwUserID") ? Convert.ToInt32(queryvalues["dwUserID"]) : -1;
            int Reason = queryvalues.ContainsKey("Reason") ? Convert.ToInt32(queryvalues["Reason"]) : -1;
            string strAccount = queryvalues.ContainsKey("strAccount") ? queryvalues["strAccount"] : "";

            Service_BanSpeak_C ServiceBanSpeakC;



            //model.Reason;

            // model.Minu;
          
            DateTime newTime = DateTime.Now.AddMinutes(Minu);


            switch (strAccount)
            {
                case "true"://禁言
                    ServiceBanSpeakC = Service_BanSpeak_C.CreateBuilder()
                        .SetDwUserID((uint)dwUserID)
                        .SetDwBanSpeak((uint)Reason)
                        .SetMinute((uint)Minu)

                        .Build();

                    break;
                default://解除禁言
                    ServiceBanSpeakC = Service_BanSpeak_C.CreateBuilder()
                        .SetDwUserID((uint)dwUserID)
                         .SetDwBanSpeak((uint)0)
                         .SetMinute((uint)0)
                        .Build();

                    break;
            }

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_BAN_SPEAK, ServiceBanSpeakC.ToByteArray()));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_BAN_SPEAK:


                    if (strAccount== "true")
                    {//禁言
                      

                        OperLogBLL.InsertFreezeLog(new FreezeLog()
                        {
                            UserID = dwUserID,
                             IP = "",
                              IMei = "",
                            CreateTime = DateTime.Now.ToString(),
                            OperUserName = User.Identity.Name,
                            Reason = ((SpeakStatus)Reason).ToString(),
                            TimeSpan = ((FreezeTimeSpanStatus)Minu).ToString(),
                            Type = "禁言"
                        });
                    }
                    else
                    {//解除禁言
                        OperLogBLL.InsertFreezeLog(new FreezeLog()
                        {
                            UserID = dwUserID,
                            IP = "",
                             IMei = "",
                            CreateTime = DateTime.Now.ToString(),
                            OperUserName = User.Identity.Name,
                            Reason = "",
                            TimeSpan = "",
                            Type = "解除禁言"
                        });
                    }




                    Service_BanSpeak_S ServiceBanSpeakS = Service_BanSpeak_S.ParseFrom(tbind.body.ToBytes());
                    if (strAccount == "true")//禁言
                    {
                        bool res = ServiceBanSpeakS.Suc;
                        if (res)
                        {

                            return Json( new { result = 0, Times = "已禁言(" + (Minu >= 5256000 ? "永久" : "截止" + newTime.ToString()) + ")" });
                        }
                        else
                        {//离线禁言

                            RoleBLL.UpdateRoleNoSpeak(
                            Reason, DateTime.Now.AddMinutes(Minu),
                             dwUserID
                               );

                            return Json( new { result = 0, Times = "已禁言(" + (Minu >= 5256000 ? "永久" : "截止" + newTime.ToString()) + ")" });

                        }

                    }
                    else
                    {
                        bool res = ServiceBanSpeakS.Suc;
                        if (res)
                        {
                            return Json( new { result = 0 });
                        }
                        else
                        {//离线解除禁言

                            RoleBLL.UpdateRoleNoSpeak(
                            0, DateTime.Now,
                            dwUserID
                               );


                            return Json( new { result = 0 });
                        }
                    }

                case CenterCmd.CS_CONNECT_ERROR:

                  
                    break;
            }

            return Json( new { result = 2 });

        }

        [QueryValues]
        public ActionResult FMOptions(Dictionary<string, string> queryvalues)
        {
            int ServEmailID = queryvalues.ContainsKey("ServEmailID") ? Convert.ToInt32(queryvalues["ServEmailID"]) : -1;
            int result = ServEmailBLL.Delete(new ServEmail() { ServEmailID = ServEmailID });
            if (result > 0)
            {
                return Json( new { result = 0 });
            }
            return Json( new { result = 1 });

        }


        [QueryValues]
        public ActionResult ServControlOptions(Dictionary<string, string> queryvalues)
        {

            bool Static = queryvalues.ContainsKey("Static") ? Convert.ToBoolean(queryvalues["Static"]) : false;

            Service_SetInternalLogin_C ServiceSetInternalLoginC = Service_SetInternalLogin_C.CreateBuilder()
              .SetBOpen(Static)
              .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_INTERALLOGIN, ServiceSetInternalLoginC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_INTERALLOGIN:

                    //Service_SetInternalLogin_S ServiceSetInternalLoginS = Service_SetInternalLogin_S.ParseFrom(tbind.body.ToBytes());


                    //if (ServiceSetInternalLoginS.IsOpen)
                    //{
                    //    return new { result = 0 };
                    //}
                    //return new { result = 1 };
                    return Json( new { result = 0 });
                case CenterCmd.CS_CONNECT_ERROR:
                    return Json( new { result = 2 });
            }

            return Json( new { result = 1 });
        }

        [QueryValues]
        public ActionResult ServControlPost(Dictionary<string, string> queryvalues)
        {

            bool Static = queryvalues.ContainsKey("Static") ? Convert.ToBoolean(queryvalues["Static"]) : false;


            Service_Send_CloseServer ServiceSendCloseServer = Service_Send_CloseServer.CreateBuilder()
                .SetClose(Static)
                .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SERVER_STOP, ServiceSendCloseServer.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SERVER_STOP:
                    return Json( new { result = 0 });
                case CenterCmd.CS_CONNECT_ERROR:
                    return Json( new { result = 2 });
            }

            return  Json( new { result = 1 });
        }



    }
}