using GL.Command.DBUtility;
using GL.Dapper;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.DAL
{
    public class ExpRecordDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        internal static List<ItemGroup> GetItemGroupList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<ItemGroup> i = cn.Query<ItemGroup>(@"select 1 as ItemID ,'金钥匙' as ItemName ,1 as TypeList union all select 2 ,'兑换券' ,2");
                cn.Close();
                return i.ToList();
            }
        }
    }
}
