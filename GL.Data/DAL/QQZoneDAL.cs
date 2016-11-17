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

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static int Add(QQZone model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"replace INTO "+ database1 + @".QQZone (OpenID, Json, UpdateTime) VALUES (@OpenID, @Json, @UpdateTime)", model);
                cn.Close();
                return i;
            }
        }
    }
}
