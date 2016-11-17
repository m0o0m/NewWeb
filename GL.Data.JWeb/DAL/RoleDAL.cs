using GL.Command.DBUtility;
using GL.Dapper;
using GL.Data.JWeb.Model;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.JWeb.DAL
{
    public class RoleDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("connectionstringforgamedata");

     

        public static SingleData GetNickNameByID(int userid)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"select NickName as ObjData from Role where id=@id");

                IEnumerable<SingleData> i = cn.Query<SingleData>(str.ToString(), new { id = userid });
                cn.Close();
                return i.FirstOrDefault();
            }
        }

    }
}
