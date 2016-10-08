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

        internal static List<GL.Data.Model.SystemPay> GetSystemPay(string sTime,string eTime,int channels)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<SystemPay> i = cn.Query<SystemPay>("record.sys_get_sysrecord", param: new { StartDate = sTime, ExpirationDate = eTime, Channels = channels }, commandType: CommandType.StoredProcedure);
              
                cn.Close();
                return i.ToList();
            }
        }

        public static List<GL.Data.Model.SystemExpend> GetSystemSystemExpend(string sTime, string eTime, int channels = 0)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<SystemExpend> i = cn.Query<SystemExpend>("record.sys_get_sysconsume", param: new { StartDate = sTime, ExpirationDate = eTime, Channels = channels }, commandType: CommandType.StoredProcedure);

                cn.Close();
                return i.ToList();
            }
        }


        internal static void PaiJuAfter()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute("record.sys_calc_user_profit", param: new { }, commandType: CommandType.StoredProcedure);
                
                cn.Close();
               
            }
        }

    }
}
