using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using GL.Protocol;
using MWeb.protobuf.SCmd;
using ProtoCmd.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
namespace MWeb.Controllers
{
    [Authorize]
    public class FMController : ApiController
    {
        ILog log = LogManager.GetLogger("TEST");
        // POST: api/FM
        public object Post([FromBody]OMModel model)
        {
            Service_Kick_C ServiceKickC = Service_Kick_C.CreateBuilder()
                .SetDwUserID((uint)model.dwUserID)
                .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_KICK_USER, ServiceKickC.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_KICK_USER:
                    Service_Kick_S ServiceKickS = Service_Kick_S.ParseFrom(tbind.body.ToBytes());
                    return new { result = ServiceKickS.Suc ? 0 : 1 };
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return new { result = 2 };
        }

        // PUT: api/FM/5
        public object Put([FromBody]OMModel model)
        {
            //            message Service_Freeze_C
            //            {
            //                required uint32 dwUserID = 1;   //帐户ID
            //                required bool isFreeze = 2; //true 是封号, false是解封
            //            }

            log.Info("进入开始封号解封操作,model.dwUserID:" + model.dwUserID + ",model.Reason=" + model.Reason);

            DateTime newTime = DateTime.Now.AddMinutes(model.Minu);



            Service_Freeze_C ServiceFreezeC;
           // model.Minu 传一个时间长短给服务器
            ServiceFreezeC = Service_Freeze_C.CreateBuilder()
                   .SetDwUserID((uint)model.dwUserID)
                   .SetDwFreeze((uint)model.Reason)
                   .SetDwMinute((uint)model.Minu)
                   .Build();

            log.Info("开始封号解封操作,model.dwUserID:" + model.dwUserID + ",model.Reason=" + model.Reason);


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_FREEZE_USER, ServiceFreezeC.ToByteArray()));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_FREEZE_USER:

                    log.Info("封号解封操作CenterCmd.CS_FREEZE_USER,model.dwUserID:" + model.dwUserID + ",model.Reason=" + model.Reason);


                    Service_Freeze_S ServiceFreezeS = Service_Freeze_S.ParseFrom(tbind.body.ToBytes());
                    bool res = ServiceFreezeS.Suc;
                    if (res)
                    {
                        return new { result = 0, Times = "已封号(" + (model.Minu >= 5256000 ? "永久" : "截止" + newTime.ToString()) + ")" };
                    }
                    else
                    {
                        RoleBLL.UpdateRoleNoFreeze(
                            model.Reason, DateTime.Now.AddMinutes(model.Minu),
                             model.dwUserID
                               );

                        return new { result = 0, Times = "已封号(" + (model.Minu >= 5256000 ? "永久" : "截止" + newTime.ToString()) + ")" };
                    }
                case CenterCmd.CS_CONNECT_ERROR:

                    log.Info("封号解封操作CenterCmd.CS_CONNECT_ERROR,model.dwUserID:" + model.dwUserID + ",model.Reason=" + model.Reason);

                    break;
            }

            return new { result = 2 };



        }
        // DELETE: api/FM/5
        public object Delete([FromBody]OMModel model)
        {

            Service_BanSpeak_C ServiceBanSpeakC;



            //model.Reason;

            // model.Minu;
            log.Info("开始禁言解除禁言操作,model.dwUserID:" + model.dwUserID + ",model.Reason=" + model.Reason);

            DateTime newTime = DateTime.Now.AddMinutes(model.Minu);
           

            switch (model.strAccount)
            {
                case "true"://禁言
                    ServiceBanSpeakC = Service_BanSpeak_C.CreateBuilder()
                        .SetDwUserID((uint)model.dwUserID)
                        .SetDwBanSpeak((uint)model.Reason)
                        .SetMinute((uint)model.Minu)
                       
                        .Build();

                    break;
                default://解除禁言
                    ServiceBanSpeakC = Service_BanSpeak_C.CreateBuilder()
                        .SetDwUserID((uint)model.dwUserID)
                         .SetDwBanSpeak((uint)0)
                         .SetMinute((uint)0)
                        .Build();

                    break;
            }

            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_BAN_SPEAK, ServiceBanSpeakC.ToByteArray()));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_BAN_SPEAK:

                    log.Info("开始禁言解除禁言操作CenterCmd.CS_BAN_SPEAK,model.dwUserID:" + model.dwUserID + ",model.Reason=" + model.Reason);


                    Service_BanSpeak_S ServiceBanSpeakS = Service_BanSpeak_S.ParseFrom(tbind.body.ToBytes());
                    if (model.strAccount == "true")//禁言
                    {
                        bool res = ServiceBanSpeakS.Suc;
                        if (res)
                        {
                            
                            return new { result = 0,Times = "已禁言("+  (model.Minu>= 5256000?"永久":"截止"+newTime.ToString())+")" };
                        }
                        else
                        {//离线禁言

                            RoleBLL.UpdateRoleNoSpeak(
                            model.Reason, DateTime.Now.AddMinutes(model.Minu),
                             model.dwUserID
                               );

                            return new { result = 0, Times = "已禁言(" + (model.Minu >= 5256000 ? "永久" : "截止" + newTime.ToString()) + ")" };
                           
                        }

                    }
                    else
                    {
                        bool res = ServiceBanSpeakS.Suc;
                        if (res)
                        {
                            return new { result = 0 };
                        }
                        else
                        {//离线解除禁言

                            RoleBLL.UpdateRoleNoSpeak(
                            0, DateTime.Now,
                             model.dwUserID
                               );


                            return new { result = 0 };
                        }
                    }

                case CenterCmd.CS_CONNECT_ERROR:

                    log.Info("开始禁言解除禁言操作CenterCmd.CS_CONNECT_ERROR,model.dwUserID:" + model.dwUserID + ",model.Reason=" + model.Reason);

                    break;
            }

            return new { result = 2 };

        }

        // DELETE: api/FM/5
        public object Options([FromBody]ServEmail model)
        {

            int result = ServEmailBLL.Delete(model);
            if (result > 0)
            {
                return new { result = 0 };
            }
            return new { result = 1 };

        }





    }
}
