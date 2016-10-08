using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;
using GL.Data.View;

namespace GL.Data.DAL
{
    public class OpenFuDaiDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static IEnumerable<OpenFuDai> GetOpenFuDai(GameRecordView grv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                if (grv.UserID > 0)
                {

                    str.Append("SELECT DATE_FORMAT(CreateTime, '%Y-%m-%d') as CreateTime, sum(Count) as Count, sum(NeedGold) as NeedGold FROM OpenFuDai where UserID = @UserID and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d')");
                }
                else
                {

                    str.Append("SELECT DATE_FORMAT(CreateTime, '%Y-%m-%d') as CreateTime, sum(Count) as Count, sum(NeedGold) as NeedGold FROM openfudai where CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d')");
                }
                IEnumerable<OpenFuDai> i = cn.Query<OpenFuDai>(str.ToString(), grv);
                cn.Close();
                return i;
            }
        }

    }
}
