using GL.Protocol;
using ProtoCmd.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWeb.protobuf.SCmd
{
    public class CmdResult
    {
        public static object Result(ServiceCmd Commond, byte[] body)
        {
            Bind tbind = Cmd.runClient(new Bind(Commond, body));


            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_SELECT_TEXAS_POT:
                    return TexasPot_Select_S.ParseFrom(tbind.body.ToBytes());
                case CenterCmd.CS_OPERTOR_TEXAS_POT:
                    return TexasPot_Operator_S.ParseFrom(tbind.body.ToBytes());
                case CenterCmd.CS_SELECT_SCALE_POT:
                    return Scale_Select_S.ParseFrom(tbind.body.ToBytes());
                case CenterCmd.CS_OPERTOR_SCALE_POT:
                    return Scale_Operator_S.ParseFrom(tbind.body.ToBytes());
                case CenterCmd.CS_SELECT_TEXPROPOT_POT:
                    return TexPro_Select_S.ParseFrom(tbind.body.ToBytes());
                case CenterCmd.CS_OPERTOR_TEXPROPOT_POT:
                    return TexPro_Operator_S.ParseFrom(tbind.body.ToBytes());
                case CenterCmd.CS_OPERTOR_BACCARAT_POT:
                    return Baccarat_Operator_S.ParseFrom(tbind.body.ToBytes());
                case CenterCmd.CS_SELECT_BACCARAT_POT:
                    return Baccarat_Select_S.ParseFrom(tbind.body.ToBytes());
                case CenterCmd.CS_CONNECT_ERROR:
                    return null;
            }
            return null;
        }




    }
}