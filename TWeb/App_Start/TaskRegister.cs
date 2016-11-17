using GL.Data.TWeb.BLL;
using GL.Data.TWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Net.Mime;
using System.IO;
using System.Timers;
using System.Xml;
using ProtoCmd.BackRecharge;
using GL.Protocol;
using Web.Pay.protobuf.SCmd;
using GL.Command.DBUtility;

namespace TWeb.App_Start
{
    public class TimerParam
    {
        public List<TMonitorData> TMonitorDatas { get; set; }

        public TMonitorData TMonitorSingle { get; set; }
        public void ThreadProc()
        {

            while (true)
            {
                string execSql = "";
                //这里就是要执行的任务,本处只显示一下传入的参数


                foreach (var item in TMonitorDatas)
                {

                    //SafeBoxSet
                    SUpdate times = TimerBLL.GetTimeForGame(item.UpdateTable);
                    string sql = item.ExecSQL.Replace("@StartDate", "'" + times.CountDate + "'")
                        .Replace("@EndDate", "'" + times.id_date + "'");

                    //修改时间

                    sql = sql + "update record.S_Update set CountDate='" + times.id_date + "' , id_date=now() where UpdateTable='" + item.UpdateTable + "';";

                    execSql += sql;


                }

                int i = TimerBLL.ExecuteSql(execSql);


                Thread.Sleep(TMonitorDatas[0].ExecType * 60 * 1000);
            }


        }


        public void ThreadPort5500()
        {
            while (true)
            {
                bool res = false;
                //检测后台端口，充值端口5500
                try
                {
                    ProtoCmd.BackRecharge.normal ServiceNormalS = ProtoCmd.BackRecharge.normal.CreateBuilder()
                       .SetUserID((uint)0)
                       .SetList("")
                       .SetRmb(0)
                       .SetRmbActual(0)
                       .SetFirstGif(false)
                       .SetBillNo("JK001")
                       .Build();

                    Bind tbind = Cmd.runClient(new Bind(ProtoCmd.BackRecharge.BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            res = true; break;
                        case CenterCmd.CS_CONNECT_ERROR:
                            res = false;
                            break;
                    }
                }
                catch
                {
                    res = false;
                }
                if (!res)//报警  充值端口5500
                {
                    TimerBLL.AddTMonitorLog(25);
                }

                Thread.Sleep(TMonitorSingle.ExecType * 60 * 1000);

            }
        }

        public void ThreadPort5200()
        {
            while (true)
            {
                bool res = false;
                //检测后台端口，充值端口5200
                try
                {
                    GL.ProtocolBack.Bind tbind = MWeb.protobuf.SCmd.Cmd.runClient(new GL.ProtocolBack.Bind(ProtoCmd.Service.ServiceCmd.SC_SELECT_REDEVENLOPE_Q, new byte[0] { }));

                    switch ((ProtoCmd.Service.CenterCmd)tbind.header.CommandID)
                    {
                        case ProtoCmd.Service.CenterCmd.CS_SELECT_REDEVENLOPE_P:
                            res = true;
                            break;
                        case ProtoCmd.Service.CenterCmd.CS_CONNECT_ERROR:
                            res = false;
                            break;
                    }


                }
                catch
                {
                    res = false;
                }
                if (!res)//报警  充值端口5500
                {
                    TimerBLL.AddTMonitorLog(26);
                }


                Thread.Sleep(TMonitorSingle.ExecType * 60 * 1000);





            }
        }
    }
        public class TaskRegister
    {
        public static void TimerTask()
        {


            //IEnumerable<TMonitorData> data = TimerBLL.GetTimeHasSQL();

            //IEnumerable<IGrouping<int, TMonitorData>> groupData = data.GroupBy(m => m.ExecType);
            //foreach (IGrouping<int, TMonitorData> iteml in groupData)
            //{
            //    List<TMonitorData> sList = iteml.ToList<TMonitorData>();
            //    TimerParam tp = new TimerParam { TMonitorDatas = sList };


            //    Thread t = new Thread(tp.ThreadProc);
            //    t.Start();


            //}





            ////接口协议类报警1   5500端口，后台支付端口    后台端口异常（不同的端口）
            //TMonitorData data5500 = TimerBLL.GetTimeByID(25);
            //TimerParam ports5500 = new TimerParam { TMonitorSingle = data5500 };
            //Thread t5500 = new Thread(ports5500.ThreadPort5500);
            //t5500.Start();
            ////5200端口，后台封号端口
            //TMonitorData data5200 = TimerBLL.GetTimeByID(26);
            //TimerParam ports5200 = new TimerParam { TMonitorSingle = data5200 };
            //Thread t5200 = new Thread(ports5200.ThreadPort5200);
            //t5200.Start();




            //Thread emailSend = new Thread(SendMailUseGmail);
            //emailSend.Start();




         




        }

        private static void SendMailUseGmail()
        {

            //while (true)
            //{
            //    //查询需要发送的邮件
            //    IEnumerable<TMonitorLog> datas = TimerBLL.GetTMonitorLog();


            //    bool pass = true;

            //    if (datas == null || datas.Count() <= 0)
            //    {
            //        pass = false;
            //    }


            //    string ids = "";
            //    foreach (var item in datas)
            //    {
            //        ids = ids + "," + item.ID;
            //    }
            //    ids = ids.Trim(',');


            //    if (string.IsNullOrEmpty(ids)) {
            //        pass = false;
            //    }

            //    if (pass)
            //    {

            //        System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();


            //       string toemail =  PubConstant.GetConnectionString("toemail");
            //        string[] toemails = toemail.Split(',');
            //        for(int i = 0; i < toemails.Length; i++)
            //        {
            //            msg.To.Add(toemails[i]);
            //        }

            //        string fromemailName = PubConstant.GetConnectionString("fromemailName");
            //        string fromemailPwd = PubConstant.GetConnectionString("fromemailPwd");
            //        string Host = PubConstant.GetConnectionString("Host");


            //        msg.From = new MailAddress(fromemailName, "515游戏", System.Text.Encoding.UTF8);
            //        /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            //        msg.Subject = "监控系统";//邮件标题   
            //        msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码   

            //        string body = "<table border='1' cellpadding='3' cellspacing ='1' width ='100%' align ='center' style ='background-color: #b9d8f3;' > ";
            //        body += "<tr><td>时间</td><td>监控名称</td><td>用户</td><td>描述</td></tr>";
            //        foreach (var item in datas)
            //        {
            //            if (item.UserID == 0)
            //            {

            //                body += "<tr><td>" + item.CreateTime + "</ td><td>" + item.MonitorName + "</td><td>" + "" + "</td><td>" + item.ExecDesc + "</td></tr>";
            //            }
            //            else
            //            {
            //                body += "<tr><td>" + item.CreateTime + "</ td><td>" + item.MonitorName + "</td><td>" + item.UserID + "</td><td>" + item.ExecDesc + "</td></tr>";
            //            }

            //        }
            //        body += "</table>";
            //        msg.Body = body;
            //        msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            //        msg.IsBodyHtml = true;//是否是HTML邮件   
            //        msg.Priority = MailPriority.High;//邮件优先级   
            //        SmtpClient client = new SmtpClient();
            //        client.Credentials = new System.Net.NetworkCredential(fromemailName, fromemailPwd);
            //        client.Host = Host;

            //        object userState = msg;
            //        try
            //        {
            //            client.Send(msg);

            //            TimerBLL.UpdateTMonitorLog(ids);


            //        }
            //        catch (System.Net.Mail.SmtpException ex)
            //        {

            //        }

            //    }



            //    Thread.Sleep(1 * 60 * 1000);

            //  }


        }




    }
}