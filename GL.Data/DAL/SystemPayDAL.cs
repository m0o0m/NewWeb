using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using GL.Dapper;
using System.Data;

namespace GL.Data.DAL
{
    public class SystemPayDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static List<GL.Data.Model.SystemPay> GetSystemPay(string sTime,string eTime,int channels)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<SystemPay> i = cn.Query<SystemPay>(""+ database3 + @".sys_get_sysrecord", param: new { StartDate = sTime, ExpirationDate = eTime, Channels = channels }, commandType: CommandType.StoredProcedure);
              
                cn.Close();
                return i.ToList();
            }
        }

        public static List<GL.Data.Model.SystemExpend> GetSystemSystemExpend(string sTime, string eTime, int channels = 0)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<SystemExpend> i = cn.Query<SystemExpend>(""+ database3 + @".sys_get_sysconsume", param: new { StartDate = sTime, ExpirationDate = eTime, Channels = channels }, commandType: CommandType.StoredProcedure);

                cn.Close();
                return i.ToList();
            }
        }


        internal static void PaiJuAfter()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(""+ database3 + @".sys_calc_user_profit", param: new { }, commandType: CommandType.StoredProcedure);
                
                cn.Close();
               
            }
        }

        internal static void ShuihuAfter(string startTime ,string endTime)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();


                IEnumerable<SystemExpend> i = cn.Query<SystemExpend>("" + database3 + @".S_ShuihuGame", param: new {
                    I_BCountDate = startTime,
                    I_ECountDate = endTime
                }, commandType: CommandType.StoredProcedure);



                //int i1 = cn.Execute(""+ database3 + @".S_ShuihuGame", param: new {
                //    I_ECountDate = "2016-11-14",
                //    I_BCountDate = "2016-11-13"
                //}, commandType: CommandType.StoredProcedure); 

                cn.Close();

            }
        }

    }
}
