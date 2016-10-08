using GL.Command.DBUtility;
using GL.Data.Model;
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
    public class DuiHuanDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static int Update(DuiHuan model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update DuiHuan set GiveOut = @GiveOut where ID = @ID", model);
                cn.Close();
                return i;
            }
        }



        internal static decimal GetSumDuiHuan(GameRecordView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                decimal i = 0;
                // bdv.Channels > 0 ? "and find_in_set(Agent, @UserList)" : ""
                if (!string.IsNullOrEmpty(model.SearchExt))
                {
                    i = cn.Query<decimal>(@"select q.NeedZiKa from DuiHuan as q ,Role as r where q.UserID = r.ID and ( r.ID =@SearchExt or r.NickName=@SearchExt or r.Account=@SearchExt ) and q.CreateTime between @StartDate and @ExpirationDate"+ (model.Channels > 0 ? " and find_in_set(r.Agent, @UserList)" : ""), model).Sum();
                }
                else
                {
                    i = cn.Query<decimal>(@"select q.NeedZiKa from DuiHuan as q, Role as r where q.UserID = r.ID and q.CreateTime between @StartDate and @ExpirationDate" + (model.Channels > 0 ? " and find_in_set(r.Agent, @UserList)" : ""), model).Sum();
                }



                cn.Close();
                return i;
            }
        }
    }
}
