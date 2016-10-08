using GL.Data.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using GL.Dapper;
using GL.Command.DBUtility;

namespace GL.Data.DAL
{
    public class ManagerInfoDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        internal static ManagerInfo GetModelByID(ManagerInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<ManagerInfo> i = cn.Query<ManagerInfo>(@"select * from ManagerInfo where AdminID = @AdminID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        internal static ManagerInfo GetModel(ManagerInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<ManagerInfo> i = cn.Query<ManagerInfo>(@"select * from ManagerInfo where AdminAccount = @AdminAccount", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static int Update(ManagerInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update ManagerInfo set AdminAccount = @AdminAccount, AdminPasswd = @AdminPasswd, AdminMasterRight = @AdminMasterRight where AdminID = @AdminID", model);
                cn.Close();
                return i;
            }
        }



        internal static int Delete(ManagerInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"delete from ManagerInfo where AdminID = @AdminID", model);
                cn.Close();
                return i;
            }
        }

        internal static int Add(ManagerInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"insert into ManagerInfo(AdminAccount,AdminPasswd,CreateDate,AdminMasterRight) values (@AdminAccount, @AdminPasswd,@CreateDate,@AdminMasterRight);", model);
                cn.Close();
                return i;
            }
        }

    }
}