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

namespace GL.Data.DAL
{
    public class TexasPotLogDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static int Add(TexasPotLog model)
        {
           
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@" 
INSERT into "+ database1 + @".TexasPotLog(Time,Type,Value,GameType,Content)
values(@Time,@Type,@Value,@GameType,@Content);
");
                cn.Query<TexasPotLog>(str.ToString(),
                 model);

                return 1;
            }



        }


        internal static int AddKuCun(TexasPotLog model)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@" 
INSERT into " + database1 + @".KuCunUpdateLog(Time,Type,Value,GameType,Content)
values(@Time,@Type,@Value,@GameType,@Content);
");
                cn.Query<TexasPotLog>(str.ToString(),
                 model);

                return 1;
            }



        }

    }
}
