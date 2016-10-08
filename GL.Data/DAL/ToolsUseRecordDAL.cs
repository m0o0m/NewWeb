using GL.Command.DBUtility;
using GL.Data.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;

namespace GL.Data.DAL
{
    public class ToolsUseRecordDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");


        
        internal static int UpdateExchange(GameRecordView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update ToolsUseRecord set ExchangeStatus = 1 where id = @UserID", model);
                cn.Close();
                return i;
            }
        }
    }
}
