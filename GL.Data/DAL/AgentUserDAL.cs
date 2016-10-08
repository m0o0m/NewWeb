using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;
using System.Data;
using GL.Data.AWeb.Identity;
using GL.Common;

namespace GL.Data.DAL
{
    public class AgentUserDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;
        internal static bool CheckUser(int mid, int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<bool> i = cn.Query<bool>(@"select find_in_set(@ID ,f_get_low(@MasterID))>0;", param: new { ID = id, MasterID = mid });
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static int GetRecordCount(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                object i = cn.ExecuteScalar(@"select count(0) from AgentUsers where HigherLevel = @ID or Id = @ID;", param: new { ID = id });
                //object i = cn.ExecuteScalar(@"select count(0) from AgentUsers", param: new { ID = id });
                cn.Close();
                return Convert.ToInt32(i);
            }
        }

        internal static IEnumerable<AgentUser> GetListByPage(PagerQuery pq, int mid, int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AgentUser> i = cn.Query<AgentUser>(pq.Sql, param: new { StartRowNumber = pq.StartItemIndex, PageSize = pq.PageSize, ID = id });
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<AgentUser> GetUserList(string idlist)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AgentUser> i = cn.Query<AgentUser>(@"select * from AgentUsers where find_in_set(Id, @idlist) order by convert(agentname using gbk) ;", param: new { idlist = idlist });
                cn.Close();
                return i;
            }



        }


        internal static AgentUser GetModel(int agentID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AgentUser> i = cn.Query<AgentUser>(@"select * from AgentUsers where id ="+agentID+" limit 1;");
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static string GetUserListString(int masterID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<string> i = cn.Query<string>(@"select f_get_low(@MasterID);", param: new { MasterID = masterID });
                cn.Close();
                return i.FirstOrDefault();
            }
        }
    }
}
