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

        internal static int AddSMS(int userid,string sign)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"replace into 515game.SMSValidate( userid, sign,CreateTime) values (@userid, @sign,'" + DateTime.Now+"')", 
                    new { userid= userid, sign= sign }
                    );
                cn.Close();
                return i;
            }

        }

    }
}
