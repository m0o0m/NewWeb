using GL.Protocol;
using ProtoCmd.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace MWeb.protobuf.SCmd
{
    public class CmdTt
    {
        internal static Bind runClient(Bind tbind)
        {


            switch ((ServiceCmd)tbind.header.CommandID)
            {
                case ServiceCmd.SC_BEGIN:
                    break;
                case ServiceCmd.SC_KICK_USER:

                    Service_Kick_S ServiceKickS = Service_Kick_S.CreateBuilder()
                        .SetSuc(true)
                        .Build();

                    return new Bind(CenterCmd.CS_KICK_USER, ServiceKickS.ToByteArray());



                case ServiceCmd.SC_QUERY_USER:

                     Service_Query_S ServiceQueryS =  Service_Query_S.CreateBuilder()
                         .SetDwUserID(0)
                         .SetDwAgentID(0)
                         .SetIsOnline(false)
                         .SetIsFreeze(false)
                         .SetSex(0)
                         .SetSzCreateTime("2014-10-22 12:00:01")
                         .SetSzEmail("asdf@fasdf.com")
                         .SetSzIdentity("440507198012300077")
                         .SetSzNickName("小T")
                         .SetSzTelNum("13012345678")
                         .SetSzTrueName("钱多多")
                         .SetSzAccount("Money")
                         .Build();
                    return new Bind(CenterCmd.CS_QUERY_UESR, ServiceQueryS.ToByteArray());

                case ServiceCmd.SC_FREEZE_USER:

                    Service_Freeze_S ServiceFreezeS = Service_Freeze_S.CreateBuilder()
                        .SetSuc(true)
                        .Build();

                    return new Bind(CenterCmd.CS_FREEZE_USER, ServiceFreezeS.ToByteArray());

                case ServiceCmd.SC_SEND_SYSMAIL:
                    break;
                case ServiceCmd.SC_SERVER_STOP:
                    break;
                case ServiceCmd.SC_QUERY_ONLINEUSER:


                    Service_Query_OnlineUser_C ServiceQueryOnlineUserC = Service_Query_OnlineUser_C.ParseFrom(tbind.body.ToBytes());



                    IList<Service_OnlineUserInfo> list = new List<Service_OnlineUserInfo>();


                    for (int i = 0; i < 10; i++)
                    {
                        Service_OnlineUserInfo ServiceOnlineUserInfo = Service_OnlineUserInfo.CreateBuilder()
                            .SetDwUserID( (uint)(1000+i) )
                            .SetStrAccount("test001" + i )
                            .SetStrIP("127.0.0.1")
                            .SetStrLoginTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            .Build();
                        list.Add(ServiceOnlineUserInfo);
                    }

                    Service_Query_OnlineUser_S ServiceQueryOnlineUserS = Service_Query_OnlineUser_S.CreateBuilder()
                        .SetPage(ServiceQueryOnlineUserC.DwPageIndex)
                        .SetPageTotal(110)
                        .SetShowNum(ServiceQueryOnlineUserC.DwShowNum)
                        .SetStartIndex(1)
                        .AddRangeList(list)
                        .Build();



                    return new Bind(CenterCmd.CS_QUERY_ONLINEUSER, ServiceQueryOnlineUserS.ToByteArray());






                case ServiceCmd.SC_TOTAL:
                    break;

                case ServiceCmd.SC_QUERY_USEROPERHIS:

                    Service_Query_UseOperHis_C ServiceQueryUseOperHisC = Service_Query_UseOperHis_C.ParseFrom(tbind.body.ToBytes());



                    IList<Service_UserOperHisInfo> ServiceUserOperHisInfoList = new List<Service_UserOperHisInfo>();


                    for (int i = 0; i < 10; i++)
                    {
                        Service_UserOperHisInfo ServiceUserOperHisInfo = Service_UserOperHisInfo.CreateBuilder()
                            .SetStrTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            .SetContent("账号234ASDewe……@……哈哈哈"+i)
                            .Build();
                        ServiceUserOperHisInfoList.Add(ServiceUserOperHisInfo);
                    }

                    Service_Query_UseOperHis_S ServiceQueryUseOperHisS = Service_Query_UseOperHis_S.CreateBuilder()
                        .SetPage(ServiceQueryUseOperHisC.DwPageIndex)
                        .SetPageTotal(63)
                        .SetShowNum(ServiceQueryUseOperHisC.DwShowNum)
                        .SetStartIndex(1)
                        .SetDwUserID(ServiceQueryUseOperHisC.DwUserID)
                        .AddRangeListInfo(ServiceUserOperHisInfoList)
                        .Build();



                    return new Bind(CenterCmd.CS_QUERY_USEROPERHIS, ServiceQueryUseOperHisS.ToByteArray());

            }







            return null;
        }

    }
}