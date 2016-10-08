using System;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using GL.Dapper;
using GL.Command.DBUtility;

namespace GL.Data.DAL
{
    public class LoginRecordDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");
        internal static LoginRecord GetModel(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<LoginRecord> i = cn.Query<LoginRecord>(@"select * from BG_LoginRecord where UserID = @ID order by LoginTime desc limit 1", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }




        internal static LoginRecord GetModel(int userid,string startTime, string endTime)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<LoginRecord> i = cn.Query<LoginRecord>(@"
select * from BG_LoginRecord where LoginAgent=1 and UserID = "+userid+ @" and LoginTime>='"+startTime+ "' and LoginTime<'"+endTime+"' order by LoginTime desc limit 1"
);
                cn.Close();
                return i.FirstOrDefault();
            }
        }



    }
}
