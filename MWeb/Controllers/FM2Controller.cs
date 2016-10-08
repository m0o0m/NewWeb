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

namespace MWeb.Controllers
{
    [Authorize]
    public class FM2Controller : ApiController
    {

        // POST: api/FM
        public object Post([FromBody]OMModel model)
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
                    return new { result = ServiceBanIPS.Suc ? 0 : 1 };
                case CenterCmd.CS_CONNECT_ERROR:
                    break;
            }

            return new { result = 2 };
        }


        



    }
}
