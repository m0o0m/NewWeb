using GL.Command.DBUtility;
using GL.Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.DAL
{
    public class NoAuthDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static int AddSMS(int userid,string sign)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"replace into "+ database1 + @".SMSValidate( userid, sign,CreateTime) values (@userid, @sign,'" + DateTime.Now+"')", 
                    new { userid= userid, sign= sign }
                    );
                cn.Close();
                return i;
            }

        }

    }
}
