using GL.Command.DBUtility;
using GL.Dapper;
using GL.Data.TWeb.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.TWeb.DAL
{
    public class TimerDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("connectionstring");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        public static IEnumerable<TMonitorData> GetTimeHasSQL()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

              
                  
                    str.AppendFormat(@"
select * from "+ database2 + @".MonitorConfig
where IsOpen=1 and  (ExecSQl is not null and ExecSQl!='')
");
        
            

                IEnumerable<TMonitorData> i = cn.Query<TMonitorData>(str.ToString(), new {  });
                cn.Close();
                return i;
            }
        }


        public static IEnumerable<TMonitorData> GetTimeNoSQL()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();



                str.AppendFormat(@"
select * from "+ database2 + @".MonitorConfig
where IsOpen=1 and  (ExecSQl is  null or ExecSQl='')
");



                IEnumerable<TMonitorData> i = cn.Query<TMonitorData>(str.ToString(), new { });
                cn.Close();
                return i;
            }
        }



        public static TMonitorData GetTimeByID(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();



                str.AppendFormat(@"
select * from "+ database2 + @".MonitorConfig
where IsOpen=1 and  MonitorID = "+id+@"
");



                IEnumerable<TMonitorData> i = cn.Query<TMonitorData>(str.ToString(), new { });
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        //TMonitorLog



        public static SUpdate GetTimeForGame(string SearchExt)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from "+ database3+ @".S_Update where UpdateTable = '" + SearchExt + "'");

                IEnumerable<SUpdate> i = cn.Query<SUpdate>(str.ToString());
                cn.Close();
                SUpdate mode = i.FirstOrDefault();
                if (mode == null)
                {
                   
                    DateTime dtN = GetDataBaseTime();
                    string sql = @"
insert into "+ database3 + @".S_Update(CountDate,UpdateTable,id_date,Description)
VALUES('"+ dtN + "', '" + SearchExt+ "','"+ dtN + "', '');";

                    cn.Execute(sql);

                    return new SUpdate() { CountDate = dtN, id_date = dtN };;//主动添加
                }
                return mode;
            }
        }


        public static IEnumerable<TMonitorLog> GetTMonitorLog()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select a.id ,a.userid ,b.MonitorName ,b.ExecDesc ,a.CreateTime
from MonitorLog a join MonitorConfig b on a.MonitorID = b.MonitorID 
where a.IsSend = 0 order by a.userid asc,a.id  asc;
");
                
                IEnumerable<TMonitorLog> i = cn.Query<TMonitorLog>(str.ToString(), new { });
                cn.Close();
                return i;
            }
        }



        public static int UpdateTMonitorLog(string ids)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
update MonitorLog set IsSend = 1 where id in (
 "+ids+@"
);
");

                int i = cn.Execute(str.ToString());
                cn.Close();
                return i;
            }
        }

        internal static DateTime GetDataBaseTime()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("select now() as CurTime");

                IEnumerable<Time> i = cn.Query<Time>(str.ToString());
                cn.Close();
                return i.FirstOrDefault().CurTime;
            }
        }


        public static int AddTMonitorLog(int monitorID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
INSERT into "+ database2+ @".MonitorLog(MonitorID,UserID,IsSend,CreateTime) 
VALUES(" + monitorID + @"," + 0 + @",0,now());
");
                int i = cn.Execute(str.ToString());


                return i;
            }
        }


        public static int AddTMonitorLog(int monitorID,int userid)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
INSERT into "+ database2+ @".MonitorLog(MonitorID,UserID,IsSend,CreateTime) 
VALUES("+ monitorID + @","+ userid + @",0,now());
");
                int i =  cn.Execute(str.ToString());

              
                return i;
            }
        }






        public static int ExecuteSql(string sql)
        {
            //修改时间


            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                cn.Execute(sql, new {
                  
                });
                cn.Close();
                return 1;
            }
        }

    }
}
