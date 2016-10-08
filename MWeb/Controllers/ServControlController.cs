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
    public class ServControlController : ApiController
    {

        // POST: api/FM
        public object Post([FromBody]SCModel model)
        {


            Service_Send_CloseServer ServiceSendCloseServer = Service_Send_CloseServer.CreateBuilder()
                .SetClose(model.Static)
                .Build();


            Bind tbind = Cmd.runClient(new Bind(ServiceCmd.SC_SERVER_STOP, ServiceSendCloseServer.ToByteArray()));

            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SERVER_STOP:
                    return new { result =  0 };
                case CenterCmd.CS_CONNECT_ERROR:
                    return new { result = 2 };
            }

            return new { result = 1 };
        }
        public object Options([FromBody]SCModel model)
        {


            Service_SetInternalLogin_C ServiceSetInternalLoginC = Service_SetInternalLogin_C.CreateBuilder()
                .SetBOpen(model.Static)
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
                    return new { result = 0 };
                case CenterCmd.CS_CONNECT_ERROR:
                    return new { result = 2 };
            }

            return new { result = 1 };
        }

        
    
    

    }
}
