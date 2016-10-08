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
using Webdiyer.WebControls.Mvc;
using GL.Common;

namespace GL.Data.DAL
{
    public class IPWhiteListDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static int Delete(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"delete from IPWhiteList where IP = @CreateIP", model);
                cn.Close();
                return i;
            }
        }

        internal static bool CheckWhiteIp(string ip) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"select count(*) from IPWhiteList where IP = '"+ ip+"'");
                cn.Close();
                return i>0;
            }
        }

        internal static int Add(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"INSERT INTO IPWhiteList (IP) VALUES (@CreateIP)", model);
                cn.Close();
                return i;
            }
        }

    

    }
}
