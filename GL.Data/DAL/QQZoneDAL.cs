using MySql.Data.MySqlClient;
using GL.Dapper;
using GL.Command.DBUtility;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GL.Data.Model;
using System;

namespace GL.Data.DAL
{
    public class QQZoneDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static int Add(QQZone model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"replace INTO 515game.QQZone (OpenID, Json, UpdateTime) VALUES (@OpenID, @Json, @UpdateTime)", model);
                cn.Close();
                return i;
            }
        }
    }
}
