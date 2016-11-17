using GL.Command.DBUtility;
using GL.Dapper;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.DAL
{
    class ActiveDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");


        internal static readonly string Coin = PubConstant.GetConnectionString("coin");


        internal static IEnumerable<Roulette> GetRouletteDataDetail(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"select s.id ID ,ifnull(a.CountNum ,0) CountSum ,case s.id when 1 then '888游戏币' when 2 then '8.88万游戏币'
                        when 3 then '666万游戏币' when 4 then '8888万游戏币' when 5 then '兑换劵*1' when 6 then '兑换劵*2'
                        when 7 then '兑换劵*5' when 8 then '5"+ Coin + @"' when 9 then '30"+ Coin + @"' when 10 then '150"+ Coin + @"' end IDDesc 
                        from S_Ordinal s 
                            left join Clearing_Roulette a on a.TypeID = s.id and a.CountDate = @StartDate and a.Agent != 10010 {0}
                        where s.id >= 1 and s.id <=10 ", bdv.Channels == 0 ? "" : "and a.Agent = @Channels");

                IEnumerable<Roulette> i = cn.Query<Roulette>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }
    }
}
