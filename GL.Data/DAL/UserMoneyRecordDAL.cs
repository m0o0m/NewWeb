using GL.Command.DBUtility;
using GL.Data.Model;
using GL.Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Data.View;

namespace GL.Data.DAL
{
    public class UserMoneyRecordDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");


        internal static UserInfo GetUserInfo(GameRecordView vbd)
        {
            if (!string.IsNullOrEmpty(vbd.SearchExt))
            {

                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();

                    StringBuilder str = new StringBuilder();

                    //                    str.Append(@"select Role.ID, Role.CreateTime, Role.CreateIP, IFNULL(q.recharge, 0), IFNULL(q.rechargecount,0), Role.Gold as money 
                    //                             from Role left join (select sum(Money) as recharge, count(0) as rechargecount, UserID 
                    //                                                  from QQZoneRecharge 
                    //                                                  where CreateTime between @StartDate and @ExpirationDate and
                    //                                                        UserID = (
                    //   select ID from Role where ID = @SearchExt or Account=@SearchExt or NickName= @SearchExt and find_in_set(Agent, @UserList) limit 1

                    //)  group by UserID) as q on q.UserID = Role.ID where ID = (select ID from Role where ID = @SearchExt or Account=@SearchExt or NickName= @SearchExt limit 1);");


                    str.Append(@" select a.ID ,a.CreateTime ,a.CreateIP ,IFNULL(sum(b.Money), 0)recharge ,count(b.userid) rechargecount,a.Gold as money
                        from Role a
                          left
                        join QQZoneRecharge b on b.CreateTime between @StartDate and @ExpirationDate and b.UserID = a.id
                        where(a.ID = @SearchExt or a.Account = @SearchExt or a.NickName = @SearchExt) and find_in_set(a.Agent, @UserList)
                        group by a.ID ,a.CreateTime ,a.CreateIP ,a.Gold;");

                    UserInfo i = cn.Query<UserInfo>(str.ToString(), vbd).FirstOrDefault();


                    cn.Close();
                    return i;
                }
            }
            else
            {

                return new UserInfo { CreateTime = DateTime.Today, CreateIP = "127.0.0.1" };
            }

        }
        internal static UserInfo GetUserInfo007(GameRecordView vbd)
        {
            if (!string.IsNullOrEmpty(vbd.SearchExt))
            {

                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();

                    StringBuilder str = new StringBuilder();



                    str.Append(@" select a.ID ,a.CreateTime ,a.CreateIP ,IFNULL(sum(b.Money), 0) ,count(b.userid) ,a.Gold as money
                        from Role a
                          left
                        join QQZoneRecharge b on b.CreateTime between @StartDate and @ExpirationDate and b.UserID = a.id
                        where(a.ID = @SearchExt or a.Account = @SearchExt or a.NickName = @SearchExt) and find_in_set(a.Agent, @UserList) and a.Agent = 10010
                        group by a.ID ,a.CreateTime ,a.CreateIP ,a.Gold;");

                    UserInfo i = cn.Query<UserInfo>(str.ToString(), vbd).FirstOrDefault();


                    cn.Close();
                    return i;
                }
            }
            else
            {

                return new UserInfo { CreateTime = DateTime.Today, CreateIP = "127.0.0.1" };
            }

        }

        internal static long GetUserInfoService(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("select sum(ChipChange) ServiceMoney from record.BG_UserMoneyRecord where CreateTime between @StartDate and @ExpirationDate and UserID = @UserID and Type in (24,25,26,47,65,92,97,106,114,119) ");

               object iem = cn.ExecuteScalar(str.ToString(), vbd);
                cn.Close();
                if (iem != null)
                {
                    return Convert.ToInt64(iem);
                }
                else
                {
                    return 0;
                }
            }
        }

        internal static decimal GetUserInfoScore(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("select sum(Gold) from 515game.ScoreAwardRecord where CreateTime between date_add(@StartDate ,interval -1 day) and @StartDate and UserID = @UserID  ");


                object iem = cn.ExecuteScalar(str.ToString(), vbd);
                cn.Close();
                if (iem != null)
                {
                    return Convert.ToDecimal(iem);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
