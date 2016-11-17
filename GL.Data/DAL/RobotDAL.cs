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
using GL.Data.View;

namespace GL.Data.DAL
{
    public  class RobotDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");
        public static readonly string sqlconnectionString2 = PubConstant.GetConnectionString("ConnectionStringForGameRecord");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<PotGold> GetPotGoldList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
   select * from "+ database3+ @".Pot_Gold                 
");

                IEnumerable<PotGold> i = cn.Query<PotGold>(str.ToString());
                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<RobotOutPut> GetRobotOutPutList(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString2))
            {
                cn.Open();
                IEnumerable<RobotOutPut> i = cn.Query<RobotOutPut>(@"S_Get007", param: new { begin_date=vbd.StartDate,  end_date=vbd.ExpirationDate }, commandType: CommandType.StoredProcedure);
                cn.Close();
                return i;
            }
        }


        internal static PotGold GetPotGoldByType(int gameType)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.Append(@" select * from "+ database3 + @".Pot_Gold where GameType="+ gameType);
                IEnumerable<PotGold> i = cn.Query<PotGold>(str.ToString());

                cn.Close();
                return i.FirstOrDefault();
            }
        }




    }
}
