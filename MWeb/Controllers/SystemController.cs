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

    public class MobileSwitch2 {
        public int Id { get; set; }
        public int Agent { get; set; }
        public int MobileDevice { get; set; }
        public string MobileBrand { get; set; }
        public List<int> CloseLoginTypeList { get; set; }
        public List<int> CloseRegisterTypeList { get; set; }
        public List<string> MobileModelList { get; set; }
    }


    [Authorize]
    public class SystemController : Controller
    {
        [QueryValues]
        public ActionResult ServControl(Dictionary<string, string> queryvalues)
        {


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_QUERY_SERVERSTATUS, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_QUERY_SERVERSTATUS:
                    {
                        Service_Query_ServerStatus_S ServiceQueryServerStatusS = Service_Query_ServerStatus_S.ParseFrom(tbind.body.ToBytes());

                        return View(new SCModel() { Static = ServiceQueryServerStatusS.Close, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }



            return View(new SCModel() { Static = true, ServerIsNotResponding = true });

        }


        [QueryValues]
        public ActionResult LoginControl(Dictionary<string, string> queryvalues)
        {


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_QUERY_INTERNALLOGIN, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_QUERY_INTERNALLOGIN:
                    {
                        Service_QueryInternalLogin_S ServiceQueryInternalLoginS = Service_QueryInternalLogin_S.ParseFrom(tbind.body.ToBytes());

                        return View(new SCModel() { Static = ServiceQueryInternalLoginS.IsOpen, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }



            return View(new SCModel() { Static = false, ServerIsNotResponding = true });

        }




        [QueryValues]
        public ActionResult ServEmail(Dictionary<string, string> queryvalues)
        {



            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string leftMenuMark = queryvalues.ContainsKey("leftMenuMark") ? queryvalues["leftMenuMark"] : string.Empty;
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            BaseDataView bdv = new BaseDataView() { StartDate = _StartDate, ExpirationDate = _ExpirationDate , Page=page};
            if (leftMenuMark != "leftMenu")
            {


                if (Request.IsAjaxRequest())
                {
                    return PartialView("ServEmail_PageList", ServEmailBLL.GetListByPage(bdv));
                }


                PagedList<ServEmail> model = ServEmailBLL.GetListByPage(bdv);
              
                bdv.BaseDataList = model;
                return View(bdv);
            }
            else {
      
                PagedList<ServEmail> model = new PagedList<ServEmail>(new List<ServEmail>(), 1, 1);
                bdv.BaseDataList = model;
                return View(bdv);
            }

        }


        [QueryValues]
        public ActionResult SendServEmail(Dictionary<string, string> queryvalues)
        {
            string ServEmailContent = queryvalues.ContainsKey("ServEmailContent") ? queryvalues["ServEmailContent"] : string.Empty;
            string ServEmailTitle = queryvalues.ContainsKey("ServEmailTitle") ? queryvalues["ServEmailTitle"] : string.Empty;



            if (Request.IsAjaxRequest())
            {
                if (string.IsNullOrWhiteSpace(ServEmailContent))
                {
                    return Json(new { result = Result.ValueCanNotBeNull });
                }
                if (string.IsNullOrWhiteSpace(ServEmailTitle))
                {
                    return Json(new { result = Result.ValueCanNotBeNull });
                }
                if (ServEmailTitle.Length > 30)
                {
                    return Json(new { result = Result.ValueIsTooLong });
                }
                if (ServEmailContent.Length > 70)
                {
                    return Json(new { result = Result.ValueIsTooLong });
                }

                Service_Send_SysMail_C ServiceSendSysMailC = Service_Send_SysMail_C.CreateBuilder()
                    .SetSzTitle(ServEmailTitle)
                    .SetSzContext(ServEmailContent)
                    .Build();

                Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SEND_SYSMAIL, ServiceSendSysMailC.ToByteArray()));

                if ((CenterCmd)tbind.header.CommandID == CenterCmd.CS_SEND_SYSMAIL)
                {
                    int result = ServEmailBLL.Insert(new ServEmail { ServEmailTime = DateTime.Now, ServEmailTitle = ServEmailTitle, ServEmailContent = ServEmailContent, ServEmailAuthor = "admin" });
                    if (result > 0)
                    {
                        return Json(new { result = Result.Redirect });
                    }
                }

                return Json(new { result = Result.Lost });
            }

            return View();

        }


        [QueryValues]
        public ActionResult SendSystemBroadcasts(Dictionary<string, string> queryvalues)
        {
            string ServEmailContent = queryvalues.ContainsKey("ServEmailContent") ? queryvalues["ServEmailContent"] : string.Empty;
            int ServEmailTitle = queryvalues.ContainsKey("ServEmailTitle") ? Convert.ToInt32(queryvalues["ServEmailTitle"]) : 0;
            int ServEmailInterval = queryvalues.ContainsKey("ServEmailInterval") ? Convert.ToInt32(queryvalues["ServEmailInterval"]) : 0;

            string leftMenuMark = queryvalues.ContainsKey("leftMenuMark") ? queryvalues["leftMenuMark"] : string.Empty;
            if (leftMenuMark != "leftMenu")
            {

                if (Request.IsAjaxRequest())
                {
                    if (string.IsNullOrWhiteSpace(ServEmailContent))
                    {
                        return Json(new { result = Result.ValueCanNotBeNull });
                    }
                    if (ServEmailTitle <= 0)
                    {
                        return Json(new { result = Result.ValueCanNotBeNull });
                    }
                    if (ServEmailTitle > 999)
                    {
                        return Json(new { result = Result.ValueIsTooLong });
                    }
                    if (ServEmailInterval <= 0)
                    {
                        return Json(new { result = Result.ValueCanNotBeNull });
                    }
                    if (ServEmailInterval > 9999)
                    {
                        return Json(new { result = Result.ValueIsTooLong });
                    }
                    if (ServEmailContent.Length > 200)
                    {
                        return Json(new { result = Result.ValueIsTooLong });
                    }

                    Service_Send_AnnounceMent_C ServiceSendAnnounceMentC = Service_Send_AnnounceMent_C.CreateBuilder()
                        .SetDwRepeat((uint)ServEmailTitle)
                        .SetContent(ServEmailContent)
                        .SetDwInterval((uint)ServEmailInterval)
                        .Build();

                    Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SEND_ANNOUNCEMENT, ServiceSendAnnounceMentC.ToByteArray()));

                    if ((CenterCmd)tbind.header.CommandID == CenterCmd.CS_SEND_ANNOUNCEMENT)
                    {
                        int result = ServEmailBLL.Insert(new ServEmail { ServEmailTime = DateTime.Now, ServEmailTitle = string.Concat("循环播放", ServEmailTitle, "次,每次间隔", ServEmailInterval, "秒"), ServEmailContent = ServEmailContent, ServEmailAuthor = User.Identity.Name });
                        if (result > 0)
                        {
                            return Json(new { result = Result.Redirect });
                        }
                    }

                    return Json(new { result = Result.Lost });
                }

                return View();
            }
            else {
                return View();
            }

        }

        [QueryValues]
        public ActionResult Announcement()
        {

            object model = UnAnnouncementBLL.GetModel();



            return View(model);

        }


        [QueryValues]
        [HttpPost]
        public ActionResult Announcement(Dictionary<string, string> queryvalues)
        {
            string Content = queryvalues.ContainsKey("Content") ? queryvalues["Content"] : string.Empty;


            if (string.IsNullOrWhiteSpace(Content))
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }
            if (Content.Length > 150)
            {
                return Json(new { result = Result.ValueIsTooLong });
            }

            int res = UnAnnouncementBLL.Update(Content);

            if (res == 1)
            {
                return Json(new { result = 210 });
            }


            return Json(new { result = 1 });


        }

        [QueryValues]
        public ActionResult GameServControl()
        {
            return View();

        }


        [QueryValues]
        public ActionResult RedevenlopeGameControl()
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SELECT_REDEVENLOPE_Q, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SELECT_REDEVENLOPE_P:
                    {
                        REDEVENLOPE_SELECT_S REDEVENLOPESELECTS = REDEVENLOPE_SELECT_S.ParseFrom(tbind.body.ToBytes());

                        return View(new SCModel() { Static = REDEVENLOPESELECTS.State, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            return View(new SCModel() { Static = false, ServerIsNotResponding = true });
        }

        [HttpPost]
        [QueryValues]
        public ActionResult RedevenlopeGameControl(SCModel model, Dictionary<string, string> queryvalues)
        {

            REDEVENLOPE_OPERATOR_C REDEVENLOPEOPERATORC = REDEVENLOPE_OPERATOR_C.CreateBuilder()
                .SetState(model.Static)
                .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_OPERTOR_REDEVENLOPE_Q, REDEVENLOPEOPERATORC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_OPERTOR_REDEVENLOPE_P:
                    {
                        REDEVENLOPE_OPERATOR_S REDEVENLOPEOPERATORS = REDEVENLOPE_OPERATOR_S.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (REDEVENLOPEOPERATORS.Bsucc)
                        {
                            return Json(new { result = 0 });
                        }
                        return Json(new { result = 1 });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 1 });


        }

        [QueryValues]
        public ActionResult PotGameControl()
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_GET_POT_SWITCH, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_GET_POT_SWITCH_RESP:
                    {
                        GetPotSwitchResp GetPotSwitchResp = GetPotSwitchResp.ParseFrom(tbind.body.ToBytes());

                        return View(new SCModel() { Static = GetPotSwitchResp.Switch == 1 ? true : false, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            return View(new SCModel() { Static = false, ServerIsNotResponding = true });
        }
        [HttpPost]
        [QueryValues]
        public ActionResult PotGameControl(SCModel model, Dictionary<string, string> queryvalues)
        {

            PotSwitch PotSwitch = PotSwitch.CreateBuilder()
                .SetTableid(0)
                .SetConfig((uint)(model.Static?1:0))
                .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_POT_SWITCH, PotSwitch.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_POT_SWITCH_RESP:
                    {
                        PotSwitchResp PotSwitchResp = PotSwitchResp.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (PotSwitchResp.Result == 0)
                        {
                            return Json(new { result = 0 });
                        }
                        return Json(new { result = 1 });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 1 });


        }


        [QueryValues]
        public ActionResult ZFBPotControl()
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_ZFBPOT_OPEN, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_ZFBPOT_OPEN:
                    {
                        BGSetZfbPotOpenRes BGSetZfbPotOpenRes = BGSetZfbPotOpenRes.ParseFrom(tbind.body.ToBytes());

                        return View(new SCModel() { Static = BGSetZfbPotOpenRes.CurrStatus == 1 ? true : false, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            return View(new SCModel() { Static = false, ServerIsNotResponding = true });
        }


        [QueryValues]
        public ActionResult BaiJiaLePotControl()   //本方法是仿照上面中发白的
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_BACCARATPOT_OPEN, new byte[0] { }));    //这里不知道对不对

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_BACCARART_OPEN:       
                    {
                        BGSetBaccaratPOTOpenRes BGSetBaccaratPOTOpenRes = BGSetBaccaratPOTOpenRes.ParseFrom(tbind.body.ToBytes());  //这里没弄好

                        return View(new SCModel() { Static = BGSetBaccaratPOTOpenRes.CurrStatus == 1 ? true : false, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            return View(new SCModel() { Static = false, ServerIsNotResponding = true });
        }

        [HttpPost]
        [QueryValues]
        public ActionResult BaiJiaLePotControl(SCModel model, Dictionary<string, string> queryvalues)
        {

            BGSetBaccaratPOTOpenReq BGSetBaccaratPOTOpenReq = BGSetBaccaratPOTOpenReq.CreateBuilder()
               .SetIsOpen((model.Static ? 1 : 0))
               .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_BACCARATPOT_OPEN, BGSetBaccaratPOTOpenReq.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_BACCARART_OPEN:
                    {
                        BGSetBaccaratPOTOpenRes BGSetBaccaratPOTOpenRes = BGSetBaccaratPOTOpenRes.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (BGSetBaccaratPOTOpenRes.CurrStatus == 1)
                        {
                            return Json(new { result = 1 });
                        }
                        return Json(new { result = 0 });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 0 });


        }


        [HttpPost]
        [QueryValues]
        public ActionResult ZFBPotControl(SCModel model, Dictionary<string, string> queryvalues)
        {

            BGSetZfbPotOpenReq BGSetZfbPotOpenReq = BGSetZfbPotOpenReq.CreateBuilder()
               .SetIsOpen((model.Static ? 1 : 0))
               .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_ZFBPOT_OPEN, BGSetZfbPotOpenReq.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_ZFBPOT_OPEN:
                    {
                        BGSetZfbPotOpenRes BGSetZfbPotOpenRes = BGSetZfbPotOpenRes.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (BGSetZfbPotOpenRes.CurrStatus == 1)
                        {
                            return Json(new { result = 1 });
                        }
                        return Json(new { result = 0});
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result =0 });


        }





        [QueryValues]
        public ActionResult TexProControl()
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_TEXPROPOT_OPEN, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_TEXPROPOT_OPEN:
                    {
                        BGSetTexProPOTOpenRes BGSetTexProPOTOpenRes = BGSetTexProPOTOpenRes.ParseFrom(tbind.body.ToBytes());
                        return View(new SCModel() { Static = BGSetTexProPOTOpenRes.CurrStatus == 1 ? true : false, ServerIsNotResponding = false });
                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            return View(new SCModel() { Static = false, ServerIsNotResponding = true });
        }
        [HttpPost]
        [QueryValues]
        public ActionResult TexProControl(SCModel model, Dictionary<string, string> queryvalues)
        {

            BGSetTexProPOTOpenReq BGSetTexProPOTOpenReq = BGSetTexProPOTOpenReq.CreateBuilder()
               .SetIsOpen((model.Static ? 1 : 0))
               .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_TEXPROPOT_OPEN, BGSetTexProPOTOpenReq.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_TEXPROPOT_OPEN:
                    {
                        BGSetTexProPOTOpenRes BGSetTexProOpenRes = BGSetTexProPOTOpenRes.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (BGSetTexProOpenRes.CurrStatus == 1)
                        {
                            return Json(new { result = 1 });
                        }
                        return Json(new { result = 0 });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 0 });


        }






        [QueryValues]
        public ActionResult YouKeControl()
        {

            int res = GameDataBLL.GetSwitchIsOpen(2);
            if (res == 1)
            {
                return View(new SCModel() { Static = true , ServerIsNotResponding = false });
            }
            else if (res == 0)
            {
                return View(new SCModel() { Static = false, ServerIsNotResponding = false });
            }
            else {
                return View(new SCModel() { Static = false, ServerIsNotResponding = true });

            }

        
        }
        [HttpPost]
        [QueryValues]
        public ActionResult YouKeControl(SCModel model, Dictionary<string, string> queryvalues)
        {
            bool res = model.Static;

            int r = GameDataBLL.UpdateSwitchIsOpen(2, res);

            if (r > 0)
            {
                if (res)
                {
                    return Json(new { result = 1 });
                }
                else
                {
                    return Json(new { result = 0 });
                }
              

            }
            else
            {
                if (res)
                {
                    return Json(new { result = 1 });
                }
                else
                {
                    return Json(new { result = 0 });
                }
            }



        }







        [QueryValues]
        public ActionResult PotGameIpControl() {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SELECT_IPSTATUS_Q, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SELECT_IPSTATUS:
                    {
                        IPSTATUS_SELECT_S IPSTATUSSELECTS = IPSTATUS_SELECT_S.ParseFrom(tbind.body.ToBytes());

                        return View(new SCModel() { Static = IPSTATUSSELECTS.State, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            return View(new SCModel() { Static = false, ServerIsNotResponding = true });

           
        }

        [HttpPost]
        [QueryValues]
        public ActionResult PotGameIpControl(SCModel model, Dictionary<string, string> queryvalues)
        {
            IPSTATUS_OPERATOR_C IPSTATUSOPERATORC = IPSTATUS_OPERATOR_C.CreateBuilder()
               .SetState(model.Static)
               .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_OPERTOR_IPSTATUS_Q, IPSTATUSOPERATORC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_OPERTOR_IPSTATUS:
                    {
                        IPSTATUS_OPERATOR_S IPSTATUSOPERATORS = IPSTATUS_OPERATOR_S.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (IPSTATUSOPERATORS.Bsucc)
                        {
                            return Json(new { result = 0 });
                        }
                        return Json(new { result = 1 });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 1 });
        }

        //[QueryValues]
        //public ActionResult LunPanControl()
        //{
        //    Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_QUERY_TURNTABLE, new byte[0] { }));

        //    switch ((CenterCmd)tbind.header.CommandID)
        //    {
        //        case CenterCmd.CS_QUERY_TURNTABLE:
        //            {
        //                TURNTABLE_INFO_S TURNTABLE_INFO_S = TURNTABLE_INFO_S.ParseFrom(tbind.body.ToBytes());

        //                return View(new SCModel() { Static = TURNTABLE_INFO_S.IsOpen, ServerIsNotResponding = false });

        //            }
        //        case CenterCmd.CS_CONNECT_ERROR:

        //            break;
        //    }

        //    return View(new SCModel() { Static = false, ServerIsNotResponding = true });
        //}

        //[HttpPost]
        //[QueryValues]
        //public ActionResult LunPanControl(SCModel model, Dictionary<string, string> queryvalues)
        //{
        //    TURNTABLE_SET_C TURNTABLE_SET_C = TURNTABLE_SET_C.CreateBuilder()
        //    .SetOpen(model.Static)
        //     .Build();


        //    Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_TURNTABLE, TURNTABLE_SET_C.ToByteArray()));

        //    switch ((CenterCmd)tbind.header.CommandID)
        //    {
        //        case CenterCmd.CS_SET_TRUNTABLE:
        //            {
        //                TURNTABLE_SET_S TURNTABLE_SET_S = TURNTABLE_SET_S.ParseFrom(tbind.body.ToBytes());

        //                //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

        //                if (TURNTABLE_SET_S.Suc)
        //                {
        //                    return Json(new { result = 0 });
        //                }
        //                return Json(new { result = 1 });
        //            }

        //        case CenterCmd.CS_CONNECT_ERROR:
        //            return Json(new { result = 2 });
        //    }

        //    return Json(new { result = 1 });
        //}


        //[HttpPost]
        //[QueryValues]
        //public ActionResult RouletteryControl(SCModel model, Dictionary<string, string> queryvalues)
        //{
        //    double ChipPot = string.IsNullOrEmpty(queryvalues["ChipPot"]) ? 0 : Convert.ToDouble(queryvalues["ChipPot"]);


        //    TURNTABLEPOT_SET_C TURNTABLEPOT_SET_C = TURNTABLEPOT_SET_C.CreateBuilder()
        //     .SetPot(ChipPot)
        //     .Build();


        //    Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_TRUNTABLE_POT, TURNTABLEPOT_SET_C.ToByteArray()));

        //    switch ((CenterCmd)tbind.header.CommandID)
        //    {
        //        case CenterCmd.CS_SET_TURNTABLE_POT:
        //            {
        //                TURNTABLEPOT_SET_S TURNTABLEPOT_SET_S = TURNTABLEPOT_SET_S.ParseFrom(tbind.body.ToBytes());

        //                //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

        //                if (TURNTABLEPOT_SET_S.Suc)
        //                {
        //                    return Json(new { result = 0 });
        //                }
        //                return Json(new { result = 1 });
        //            }

        //        case CenterCmd.CS_CONNECT_ERROR:
        //            return Json(new { result = 2 });
        //    }

        //    return Json(new { result = 1 });
        //}

        [QueryValues]
        public ActionResult GameAnnouncement() {

            object model = UnAnnouncementBLL.GetGameAnnouncement();
          
            return View(model);
        }

        [QueryValues]
        [HttpPost]
        public ActionResult GameAnnouncement(Dictionary<string, string> queryvalues)
        {
            string Content = queryvalues.ContainsKey("Content") ? queryvalues["Content"] : string.Empty;
            if (string.IsNullOrWhiteSpace(Content))
            {
                return Json(new { result = Result.ValueCanNotBeNull });
            }
            Content = new System.Text.RegularExpressions.Regex("[\\n][\\n]+").Replace(Content, "\n\n");
            string[] contents = Content.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<GL.Data.Model.GameAnnouncement> games = new List<GL.Data.Model.GameAnnouncement>();
            for (int i = 0; i < contents.Length; i++) {
                string c = contents[i];
                string[] cs = c.Split('\n');
                string title = cs[0];
                string con = "";
                GL.Data.Model.GameAnnouncement m = new GL.Data.Model.GameAnnouncement();
                m.Title = title;
                for (int j =1; j < cs.Length; j++) {
                    string cj = cs[j];
                    con = con + cj+"\n";
                }
                m.Content = con.Trim('\n');
                m.IndexNo = i + 1;
                games.Add(m);
            }

           int res = UnAnnouncementBLL.UpdateGameAnnouncement(games);
          
            if (res >= 1)
            {
                return Json(new { result = 210 });
            }


            return Json(new { result = 1 });


        }

        [QueryValues]
        public ActionResult GameViewAnnouncement(Dictionary<string, string> queryvalues) {
            string con = queryvalues.ContainsKey("con") ? queryvalues["con"] : string.Empty;
            con = new System.Text.RegularExpressions.Regex("[\\n][\\n]+").Replace(con, "\n\n");
            string[] contents = con.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<GL.Data.Model.GameAnnouncement> games = new List<GL.Data.Model.GameAnnouncement>();
            for (int i = 0; i < contents.Length; i++)
            {
                string c = contents[i];
                string[] cs = c.Split('\n');
                string title = cs[0];
                string cc = "";
                GL.Data.Model.GameAnnouncement m = new GL.Data.Model.GameAnnouncement();
                m.Title = title;
                for (int j = 1; j < cs.Length; j++)
                {
                    string cj = cs[j];
                    cc = cc + cj + "\n";
                }
                m.Content = cc.Trim('\n');
                m.IndexNo = i + 1;
                games.Add(m);
            }

            string res = "";
            foreach (GL.Data.Model.GameAnnouncement item in games)
            {
                item.Content = item.Content.Replace("\n", "<br>").Replace(" ", "&nbsp;&nbsp;");
                string head = "<label style ='color:yellow'>" + item.Title + " </label ><br>";

                



                string content = "<label style ='color:white'>"+ item.Content+ " </label ><br>";
                res = res + head + content;

            }
            res = "<label id='lblcon'>"+res+ "</label>";


            return Content(res,"string");
        }

        [QueryValues]
        public ActionResult GameControl(Dictionary<string, string> queryvalues) {
            return View();
        }


        [QueryValues]
        public ActionResult SetGameSwitch(Dictionary<string, string> queryvalues)
        {
            int  type = queryvalues.ContainsKey("type") ? Convert.ToInt32( queryvalues["type"]) : -1;
            ViewData["type"] = type;
            int res = GameDataBLL.GetSwitchIsOpen(type);
            if (res == 1)
            {
                return View(new SCModel() { Static = true, ServerIsNotResponding = false });
            }
            else if (res == 0)
            {
                return View(new SCModel() { Static = false, ServerIsNotResponding = false });
            }
            else
            {
                return View(new SCModel() { Static = false, ServerIsNotResponding = true });

            }

          
        }


        [HttpPost]
        [QueryValues]
        public ActionResult SetClientGameSwitch(SCModel model, Dictionary<string, string> queryvalues)
        {
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32( queryvalues["type"]) : -1;
            int ck1 = queryvalues.ContainsKey("ck1") ? Convert.ToInt32(queryvalues["ck1"]) :0;
            int ck2 = queryvalues.ContainsKey("ck2") ? Convert.ToInt32(queryvalues["ck2"]) :0;
            int ck3 = queryvalues.ContainsKey("ck3") ? Convert.ToInt32(queryvalues["ck3"]) : 0;

         

            ClientGame_Switch_C ClientGame_Switch_C = ClientGame_Switch_C.CreateBuilder()
             .SetGameID((uint)type)
             .AddPlatOpen((uint)ck1)
             .AddPlatOpen((uint)ck2)
             .AddPlatOpen((uint)ck3)
             .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_CLIENT_GAME_SWITCH, ClientGame_Switch_C.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_CLIENT_GAME_SWITCH:
                    {
                        ClientGame_Switch_S ClientGame_Switch_S = ClientGame_Switch_S.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (ClientGame_Switch_S.Suc == true)
                        {
                            IList<uint> lists = ClientGame_Switch_S.PlatOpenList;
                            string res = "";
                            foreach (uint item in lists)
                            {
                                res += "," + item;
                            }
                            res = res.Trim(',');

                            return Json(new { result = res });
                        }
                        return Json(new { result ="-1" }); //设置失败
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = "-2" }); //连接失败
            }

            return Json(new { result = "-3"});//未知错误

        }

        [QueryValues]
        public ActionResult ProtolBJ(Dictionary<string, string> queryvalues) {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            BaseDataView bdv = new BaseDataView() {
                Page = page, StartDate = _StartDate, ExpirationDate = _ExpirationDate
            };

            PagedList<MonitorLog> model = BaseDataBLL.GetMonitorLogList(bdv);
            if (Request.IsAjaxRequest())
            {
                return PartialView("ProtolBJ_PageList", model);
            }
            bdv.BaseDataList = model;

            return View(bdv);

           
        }


        [QueryValues]
        public ActionResult ClientGameControl(Dictionary<string, string> queryvalues)
        {
            return View();
        }

        [QueryValues]
        public ActionResult SetClientGameSwitch(Dictionary<string, string> queryvalues)
        {
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : -1;
            ViewData["type"] = type;
            SSwitch model = GameDataBLL.GetSwitchModel(type);
            ViewData["a"] = model.para1.ToString();
            ViewData["b"] = model.para2.ToString();
            ViewData["c"] = model.para3.ToString();
            return View();

        }

        [HttpPost]
        [QueryValues]
        public ActionResult SetGameSwitch(SCModel model, Dictionary<string, string> queryvalues)
        {
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : -1;

            Module_Switch_C Module_Switch_C = Module_Switch_C.CreateBuilder()
            .SetModule((uint)type)
             .SetOpend((model.Static ? (uint)1 : (uint)0))
             .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_MODULE_SWITCH, Module_Switch_C.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_MODULE_SWITCH:
                    {
                        Module_Switch_S Module_Switch_S = Module_Switch_S.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (Module_Switch_S.Suc == true)
                        {
                            return Json(new { result = Module_Switch_S.Opend });
                        }
                        return Json(new { result = Module_Switch_S.Opend });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 0 });

        }





        [QueryValues]
        public ActionResult ThanksGivingControl()
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_INDEBATED_REBATE_OPEN, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_INDEBATED_REBATE_OPEN:
                    {
                        BGSetIndebatedRebateOpenRes BGSetIndebatedRebateOpenRes = BGSetIndebatedRebateOpenRes.ParseFrom(tbind.body.ToBytes());

                        return View(new SCModel() { Static = BGSetIndebatedRebateOpenRes.CurrStatus == 1 ? true : false, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            return View(new SCModel() { Static = false, ServerIsNotResponding = true });
        }
        [HttpPost]
        [QueryValues]
        public ActionResult ThanksGivingControl(SCModel model, Dictionary<string, string> queryvalues)
        {

            BGSetIndebatedRebateOpenReq BGSetIndebatedRebateOpenReq = BGSetIndebatedRebateOpenReq.CreateBuilder()
               .SetIsOpen((model.Static ? 1 : 0))
               .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_INDEBATED_REBATE_OPEN, BGSetIndebatedRebateOpenReq.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_INDEBATED_REBATE_OPEN:
                    {
                        BGSetIndebatedRebateOpenRes BGSetIndebatedRebateOpenRes = BGSetIndebatedRebateOpenRes.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (BGSetIndebatedRebateOpenRes.CurrStatus == 1)
                        {
                            return Json(new { result = 1 });
                        }
                        return Json(new { result = 0 });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 0 });


        }





        [QueryValues]
        public ActionResult HerocraftTwiceControl()
        {
         
          
            int res = GameDataBLL.GetSwitchIsOpen(104);
            if (res == 1)
            {
                return View(new SCModel() { Static = true, ServerIsNotResponding = false });
            }
            else if (res == 0)
            {
                return View(new SCModel() { Static = false, ServerIsNotResponding = false });
            }
            else
            {
                return View(new SCModel() { Static = false, ServerIsNotResponding = true });

            }



        }
        [HttpPost]
        [QueryValues]
        public ActionResult HerocraftTwiceControl(SCModel model, Dictionary<string, string> queryvalues)
        {

            BGSetHerocraftTwiceOpenReq BGSetHerocraftTwiceOpenReq = BGSetHerocraftTwiceOpenReq.CreateBuilder()
               .SetIsOpen((model.Static ? 1 : 0))
               .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_Herocraft_Twice_OPEN, BGSetHerocraftTwiceOpenReq.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_Herocraft_Twice_OPEN:
                    {
                        BGSetHerocraftTwiceOpenRes BGSetHerocraftTwiceOpenRes = BGSetHerocraftTwiceOpenRes.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (BGSetHerocraftTwiceOpenRes.CurrStatus == 1)
                        {
                            return Json(new { result = 1 });
                        }
                        return Json(new { result = 0 });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 0 });


        }


        // MFestivalGivingControl
        [QueryValues]
        public ActionResult MFestivalGivingControl()
        {
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_MIDAUTUMN_FESTIVAL_OPEN, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_MIDAUTUMN_FESTIVAL_OPEN:
                    {
                        BGSetMidautumnFestivalOpenRes BGSetMidautumnFestivalOpenRes = BGSetMidautumnFestivalOpenRes.ParseFrom(tbind.body.ToBytes());

                        return View(new SCModel() { Static = BGSetMidautumnFestivalOpenRes.CurrStatus == 1 ? true : false, ServerIsNotResponding = false });

                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }

            return View(new SCModel() { Static = false, ServerIsNotResponding = true });
        }
        [HttpPost]
        [QueryValues]
        public ActionResult MFestivalGivingControl(SCModel model, Dictionary<string, string> queryvalues)
        {

            BGSetMidautumnFestivalOpenReq BGSetMidautumnFestivalOpenReq = BGSetMidautumnFestivalOpenReq.CreateBuilder()
               .SetIsOpen((model.Static ? 1 : 0))
               .Build();

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SET_MIDAUTUMN_FESTIVAL_OPEN, BGSetMidautumnFestivalOpenReq.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SET_MIDAUTUMN_FESTIVAL_OPEN:
                    {
                        BGSetMidautumnFestivalOpenRes BGSetMidautumnFestivalOpenRes = BGSetMidautumnFestivalOpenRes.ParseFrom(tbind.body.ToBytes());

                        //return View(new SCModel() { Static = REDEVENLOPEOPERATORS.State, ServerIsNotResponding = false });

                        if (BGSetMidautumnFestivalOpenRes.CurrStatus == 1)
                        {
                            return Json(new { result = 1 });
                        }
                        return Json(new { result = 0 });
                    }

                case CenterCmd.CS_CONNECT_ERROR:
                    return Json(new { result = 2 });
            }

            return Json(new { result = 0 });


        }





        [QueryValues]
        public ActionResult PopUpControl(Dictionary<string, string> queryvalues) {

            string Platform = queryvalues.ContainsKey("Platform") ? queryvalues["Platform"].ToString().Replace(',','|') : "";
            int Position = queryvalues.ContainsKey("Position") ? Convert.ToInt32(queryvalues["Position"]) : 0;
            int JumpPage = queryvalues.ContainsKey("JumpPage") ? Convert.ToInt32(queryvalues["JumpPage"]) : 1;
            string StartTime = queryvalues.ContainsKey("StartTime") ? queryvalues["StartTime"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string EndTime = queryvalues.ContainsKey("EndTime") ? queryvalues["EndTime"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            int IsOpen = queryvalues.ContainsKey("IsOpen") ? Convert.ToInt32(queryvalues["IsOpen"]) : 1;
            string btnAdd = queryvalues.ContainsKey("btnAdd") ? queryvalues["btnAdd"].ToString() : "";
            BasePopOutDataView bdv = new BasePopOutDataView()
            {
               EndTime = EndTime, IsOpen = IsOpen, JumpPage = JumpPage, OpenWinNo=1, Platform = Platform, Position = Position, StartTime=StartTime
            };


           



            PopUpInfo p = new PopUpInfo()
            {
                EndTime = EndTime,
                IsOpen = IsOpen,
                JumpPage =(JumpPage)JumpPage,
                OpenWinNo = 1,
                Platform = Platform,
                Position = (PopPosition)Position,
                StartTime = StartTime
            };

            if (!string.IsNullOrEmpty(btnAdd)) {
                var modelExist = GameDataBLL.GetPopUpDataByPosition(Position);
                if (modelExist != null)
                {

                }
                else {
                    int res = GameDataBLL.AddPopUpData(p);

                    if (res >= 1)
                    {
                        PopUpMessageToServer();
                    }
                }

              
            }



            bdv.DataList = GameDataBLL.GetPopUpDataList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("PopUpControl_PageList", bdv.DataList);
            }

            return View(bdv);
        }

        [QueryValues]
        public ActionResult PopUpDelete(Dictionary<string, string> queryvalues) {


            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : -1;

            int res = GameDataBLL.DeletePopUpData(new PopUpInfo() { id = id });
            if (res >= 1)
            {
                PopUpMessageToServer();
            }

            return Content(res.ToString());
        }

        [QueryValues]
        public ActionResult PopUpUpdate(Dictionary<string, string> queryvalues) {

            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : -1;
            //btnUpdate
            string btnUpdate = queryvalues.ContainsKey("btnUpdate") ? queryvalues["btnUpdate"].ToString() : "";
            if (!string.IsNullOrEmpty(btnUpdate)) {//非空是修改
              
                string Platform = queryvalues.ContainsKey("Platform") ? queryvalues["Platform"].ToString().Trim(',').Replace(',', '|') : "";
                int Position = queryvalues.ContainsKey("Position") ? Convert.ToInt32(queryvalues["Position"]) : 1;
                int JumpPage = queryvalues.ContainsKey("JumpPage") ? Convert.ToInt32(queryvalues["JumpPage"]) : 1;
                string StartTime = queryvalues.ContainsKey("StartTime") ? queryvalues["StartTime"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                string EndTime = queryvalues.ContainsKey("EndTime") ? queryvalues["EndTime"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                int IsOpen = queryvalues.ContainsKey("IsOpen") ? Convert.ToInt32(queryvalues["IsOpen"]) : 1;

                PopUpInfo p = new PopUpInfo()
                {
                    EndTime = EndTime,
                    IsOpen = IsOpen,
                    JumpPage = (JumpPage)JumpPage,
                    OpenWinNo = 1,
                    Platform = Platform,
                    Position = (PopPosition)Position,
                    StartTime = StartTime
                };


                var modelExist = GameDataBLL.GetPopUpDataByPosition(Position);
                if (modelExist != null)
                {
                    //如果此位置已存在，看是否是本身
                    PopUpInfo self = GameDataBLL.GetPopUpData(id);

                    if (self.Position == modelExist.Position)
                    {
                        int res = GameDataBLL.UpdatePopUpData(p, id);

                        if (res >= 1)
                        {
                            PopUpMessageToServer();
                        }

                        return Content(res.ToString());
                    }
                    else {
                        return Content("-1");
                    }

                   
                }
                else {
                    int res = GameDataBLL.UpdatePopUpData(p, id);

                    if (res >= 1)
                    {
                        PopUpMessageToServer();
                    }

                    return Content(res.ToString());
                }


             
            }



            PopUpInfo model = GameDataBLL.GetPopUpData(id);

            return View(model);
        }


        private bool PopUpMessageToServer() {

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_NTF_UPDATE_POPUPCONTROL, new byte[0] { }));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_NTF_UPDATE_POPUPCONTROL:
                    {
                        PopUpControl_S PopUpControl_S= PopUpControl_S.ParseFrom(tbind.body.ToBytes());

                        return PopUpControl_S.Suc;
                    }
                case CenterCmd.CS_CONNECT_ERROR:
                       return false;
                 
            }
            return false;

        }


        [QueryValues]
        public ActionResult MobileLoginRegister(Dictionary<string, string> queryvalues) {

            //查询参数
            LoginRegisterDataView model = new LoginRegisterDataView();
            int _Channels = queryvalues.ContainsKey("Channels") ? Convert.ToInt32(queryvalues["Channels"]) : -1;
            int _Platform = queryvalues.ContainsKey("Platform") ? Convert.ToInt32(queryvalues["Platform"]) : -1;
            string  _PhoneBoard = queryvalues.ContainsKey("PhoneBoard") ? queryvalues["PhoneBoard"].ToString() : "all_brand";//品牌
            string _PhoneModels = queryvalues.ContainsKey("PhoneModels") ? queryvalues["PhoneModels"].ToString() : "all_model";//型号
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;


            model.Channels = _Channels;
            model.Platform = _Platform;
            model.PhoneModels = _PhoneModels;
            model.PhoneBoard = _PhoneBoard;


            ViewData["LoginRegisterDataView"] = model;


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




           IEnumerable<CommonIDName> phoneBoards =  BaseDataBLL.GetPhoneBoard();
            List<SelectListItem> ieList2 = BaseDataBLL.GetPhoneBoard().Select(
            x => new SelectListItem { Text = x.Name, Value = x.Name }
            ).ToList();
            ieList2.Insert(0, new SelectListItem { Text = "所有品牌", Value = "all_brand", Selected = "所有品牌" == _PhoneBoard });
            ViewData["PhoneBoard"] = ieList2;



            IEnumerable<CommonIDName> datas = BaseDataBLL.GetModelByBoard(_PhoneBoard);
            List<SelectListItem> ieList3 = datas.Select(
                x => new SelectListItem { Text = x.Name, Value = x.Name }
            ).ToList();
            ieList3.Insert(0, new SelectListItem { Text = "所有机型", Value = "all_model", Selected = "所有机型" == _PhoneBoard });
            ViewData["PhoneModel"] = ieList3;




            List<RLFlow> flowList = new List<RLFlow>();//操作记录

            #region 查询列表操作记录
            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_MOBILE_SWITCHS, new byte[0] { }));
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_MOBILE_SWITCHS:
                    {
                        MobileSwitchs_S MobileSwitchs_S = MobileSwitchs_S.ParseFrom(tbind.body.ToBytes());
                        if (MobileSwitchs_S.Suc)
                        {
                            IList<MobileSwitch> data = MobileSwitchs_S.MobileSwitchsList;

                            //LoginRegisterDetail
                            //过滤后的数据用来显示
                            //IEnumerable<MobileSwitch> dGuoLv = data.Where(
                            //    m => m.Agent == _Channels &
                            //    m.MobileDevice == _Platform &
                            //    m.MobileBrand == _PhoneBoard
                            //    );

                            IEnumerable<MobileSwitch> dGuoLv = data.Where(m=>1==1);



                            foreach (MobileSwitch item in dGuoLv)
                            {
                                RLFlow flow = new RLFlow();
                                flow.Agent = item.Agent;  //渠道id

                               var agen =   AgentUserBLL.GetModel(item.Agent);
                                if (agen == null)
                                {
                                    flow.AgentName = "所有渠道";//渠道名称

                                }
                                else {
                                    flow.AgentName = agen.AgentName;//渠道名称
                                }

                              
                                flow.PhoneBoardName = item.MobileBrand; //品牌
                             
                                flow.PlatformName = ((PlatformL)item.MobileDevice).ToString();//平台
                                int id = item.Id;
                                flow.ID = id;
                                IList<int> CloseLoginTypeList = item.CloseLoginTypeList;
                                IList<int> CloseRegisterTypeList = item.CloseRegisterTypeList;
                                IList<string> MobileModelList = item.MobileModelList;
                                flow.PhoneModelsName = MobileModelList[0];//型号
                                if (MobileModelList.Contains(_PhoneModels))//找出此型号的手机机型的登录注册操作记录
                                {
                                 
                                }

                                for (var i = 0; i < CloseLoginTypeList.Count(); i++)
                                {
                                    var closeItem = ((LoginType)CloseLoginTypeList[i]).ToString() + "登录关闭 ";
                                    flow.OperType += closeItem;
                                }
                                for (var j = 0; j < CloseRegisterTypeList.Count(); j++)
                                {
                                    var closeRItem = ((LoginType)CloseRegisterTypeList[j]).ToString() + "注册关闭 ";
                                    flow.OperType += closeRItem;
                                }
                                flowList.Add(flow);



                            }




                        }

                    }
                    break;
                case CenterCmd.CS_CONNECT_ERROR: //伪造数据，真正的协议通了就可删除

                    break;
            }

            #endregion



            //PagedList<RLFlow> dataL = GameDataBLL.GetListByPageForRegisterLogin(new RLFlow()
            //{
            //     Agent = _Channels,
            //      Platform = _Platform,
            //       PhoneBoardID = _PhoneBoardID,
            //        PhoneModelsID = _PhoneModelsID,
            //         Page = _page
            //});




            //IList<MobileSwitch2> data = new List<MobileSwitch2>();

            //MobileSwitch2 d1 = new MobileSwitch2()
            //{
            //    Agent = _Channels,
            //    Id = 1,
            //    MobileBrand = _PhoneBoard,
            //    MobileDevice = _Platform
            //};
            //List<int> Cl1 = new List<int>() { 0, 1 };
            //d1.CloseLoginTypeList = Cl1;
            //List<int> Cr1 = new List<int>() { 2, 3 };
            //d1.CloseRegisterTypeList = Cr1;
            //List<string> m1 = new List<string>() { "ak1", "ak2" };
            //d1.MobileModelList = m1;
            //data.Add(d1);


            //MobileSwitch2 d2 = new MobileSwitch2()
            //{
            //    Agent = _Channels,
            //    Id = 2,
            //    MobileBrand = _PhoneBoard,
            //    MobileDevice = _Platform
            //};
            //List<int> Cl2 = new List<int>() { 4 };
            //d2.CloseLoginTypeList = Cl2;
            //List<int> Cr2 = new List<int>() { 4 };
            //d2.CloseRegisterTypeList = Cr2;
            //List<string> m2 = new List<string>() { "ak1", "ak3" };
            //d2.MobileModelList = m2;
            //data.Add(d2);

            ////LoginRegisterDetail
            ////过滤后的数据用来显示
            //IEnumerable<MobileSwitch2> dGuoLv = data.Where(
            //    m => m.Agent == _Channels &
            //    m.MobileDevice == _Platform &
            //    m.MobileBrand == _PhoneBoard
            //    );

         



            //foreach (MobileSwitch2 item in dGuoLv)
            //{
            //    RLFlow flow = new RLFlow();
            //    flow.Agent = _Channels;//渠道id
            //    flow.AgentName = "";//渠道名称
            //    flow.PhoneBoardName = _PhoneBoard; //品牌
            //    flow.PhoneModelsName = _PhoneModels;//型号
            //    flow.PlatformName = ((PlatformL)_Platform).ToString();//平台
            //    flow.OperType = "";
            //    int id = item.Id;
            //    flow.ID = id;
               
            //    IList<int> CloseLoginTypeList = item.CloseLoginTypeList;
            //    IList<int> CloseRegisterTypeList = item.CloseRegisterTypeList;
            //    IList<string> MobileModelList = item.MobileModelList;
            //    if (MobileModelList.Contains(_PhoneModels))//找出此型号的手机机型的登录注册操作记录
            //    {
            //        for (var i = 0; i < CloseLoginTypeList.Count(); i++)
            //        {
            //            var closeItem = ((LoginType)CloseLoginTypeList[i]).ToString() + "登录关闭 ";
            //            flow.OperType += closeItem;
            //        }
            //        for (var j = 0; j < CloseRegisterTypeList.Count(); j++)
            //        {
            //            var closeRItem = ((LoginType)CloseRegisterTypeList[j]).ToString() + "注册关闭 ";
            //            flow.OperType += closeRItem;
            //        }
            //        flowList.Add(flow);

            //    }

            //}


            model.Data = flowList;

            return View(model);
        }

        [QueryValues]
        public ActionResult MobileLoginRegisterDelete(Dictionary<string, string> queryvalues) {

            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : -1;

            MobileSwitch mobileSwitch = MobileSwitch.CreateBuilder() //删除只需要设置id
                .SetId(id)
                .Build();
            MobileSwitchOperator_C MobileSwitchOperator_C = MobileSwitchOperator_C.CreateBuilder()
               .SetOperatorType(2) //1添加，2删除
               .SetMobileSwitch(mobileSwitch)
               .Build();

         

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_MOBILE_SWITCH_OPERATOR, MobileSwitchOperator_C.ToByteArray()));
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_MOBILE_SWITCH_OPERATOR:
                    {
                        MobileSwitchOperator_S MobileSwitchOperator_S = MobileSwitchOperator_S.ParseFrom(tbind.body.ToBytes());
                        if (MobileSwitchOperator_S.Suc)
                        {
                            return Content("1");
                        }
                      
                    }
                    break;
                case CenterCmd.CS_CONNECT_ERROR: 

                    break;
            }
            return Content("0");

        }

        [QueryValues]
        public ActionResult GetModel(Dictionary<string, string> queryvalues) {
            string id = queryvalues.ContainsKey("id") ? queryvalues["id"].ToString(): "";//渠道

            IEnumerable<CommonIDName> datas =   BaseDataBLL.GetModelByBoard(id);
            string options = "<option value='all_model'>所有机型</option>";
            foreach (CommonIDName item in datas)
            {
                string op = "<option value='"+item.Name+"'>"+item.Name+"</option>";
                options += op;
            }

            return Content(options);
        }


        [QueryValues]
        public ActionResult MobileLoginRegisterAdd(Dictionary<string, string> queryvalues)
        {

            int agent = queryvalues.ContainsKey("agent") ? Convert.ToInt32(queryvalues["agent"]) : -1;//渠道
        
            string brand = queryvalues.ContainsKey("brand") ? queryvalues["brand"].ToString() : "all_brand";//品牌
            int loginType = queryvalues.ContainsKey("loginType") ? Convert.ToInt32(queryvalues["loginType"]) : -1;//登录方式
            int RegisterType = queryvalues.ContainsKey("RegisterType") ? Convert.ToInt32(queryvalues["RegisterType"]) : -1;//注册方式
            string mobileModel = queryvalues.ContainsKey("mobileModel") ? queryvalues["mobileModel"].ToString() : "all_model";//型号
            int device = queryvalues.ContainsKey("device") ? Convert.ToInt32(queryvalues["device"]) : -1;//设备

            MobileSwitch mobileSwitch;
            if (RegisterType <= -100)
            {//说明是登录操作
                mobileSwitch = MobileSwitch.CreateBuilder() //删除只需要设置id
                 .SetMobileDevice(device)
                 .SetAgent(agent)
                 .SetMobileBrand(brand)
                 .AddCloseLoginType(loginType)
                 .AddMobileModel(mobileModel)
                 .Build();
            }
            else {//注册操作
                mobileSwitch = MobileSwitch.CreateBuilder() //删除只需要设置id
                .SetMobileDevice(device)
                .SetAgent(agent)
                .SetMobileBrand(brand)
                .AddCloseRegisterType(RegisterType)
                .AddMobileModel(mobileModel)
                .Build();
            }

          
            MobileSwitchOperator_C MobileSwitchOperator_C = MobileSwitchOperator_C.CreateBuilder()
               .SetOperatorType(1) //1添加，2删除
               .SetMobileSwitch(mobileSwitch)
               .Build();



            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_MOBILE_SWITCH_OPERATOR, MobileSwitchOperator_C.ToByteArray()));
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_MOBILE_SWITCH_OPERATOR:
                    {
                        MobileSwitchOperator_S MobileSwitchOperator_S = MobileSwitchOperator_S.ParseFrom(tbind.body.ToBytes());
                        if (MobileSwitchOperator_S.Suc)
                        {
                            return Content("1");
                        }

                    }
                    break;
                case CenterCmd.CS_CONNECT_ERROR:

                    break;
            }
            return Content("0");

        }


        [QueryValues]
        public ActionResult OperLog(Dictionary<string, string> queryvalues) {
          
            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
          
            string _StartDate = queryvalues.ContainsKey("StartTime") ? queryvalues["StartTime"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("EndTime") ? queryvalues["EndTime"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            string UserAccount = queryvalues.ContainsKey("UserAccount") ? queryvalues["UserAccount"] : "";
            OperLogView olv = new OperLogView {
                 StartTime = _StartDate,
                  EndTime = _ExpirationDate,
                   UserAccount = UserAccount,
                Page = _page
            };


        

            if (Request.IsAjaxRequest())
            {
                return PartialView("OperLog_PageList", OperLogBLL.GetListByPageForOperLog(olv));
            }

            olv.DataList = OperLogBLL.GetListByPageForOperLog(olv);

            return View(olv);
        }




        [QueryValues]
        public ActionResult FreezeLog(Dictionary<string, string> queryvalues)
        {

            int _page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            string _StartDate = queryvalues.ContainsKey("StartTime") ? queryvalues["StartTime"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("EndTime") ? queryvalues["EndTime"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");

            //int _UserID = queryvalues.ContainsKey("UserID") ?
            //    queryvalues["UserID"].ToString()==""?0:Convert.ToInt32(queryvalues["UserID"]) : 0;

            string _OperUserName = queryvalues.ContainsKey("OperUserName") ? queryvalues["OperUserName"].ToString() : "";
            string _Search = queryvalues.ContainsKey("Search") ? queryvalues["Search"].ToString() : "";


          


            FreezeLogView flv = new FreezeLogView
            {
                StartTime = _StartDate,
                EndTime = _ExpirationDate,
            
                Page = _page,
                OperUserName = _OperUserName,
                Search = _Search

            };




            if (Request.IsAjaxRequest())
            {
                return PartialView("FreezeLog_PageList", OperLogBLL.GetListByPageForFreezeLog(flv));
            }

            flv.DataList = OperLogBLL.GetListByPageForFreezeLog(flv);

            return View(flv);
        }



    }

}