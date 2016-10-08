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
    public class FishInfoDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        

        internal static IEnumerable<FishCount> GetFishCount(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                if (!string.IsNullOrEmpty(vbd.SearchExt))
                {
                    str.Append(@"SELECT count(f.FishID) as Num, f.FishID, `Type` FROM FishInfoRecord f,Role r 
                                 where f.UserID = r.ID and ( r.ID =@SearchExt or r.NickName=@SearchExt or r.Account=@SearchExt )  and 
                                       f.CreateTime between @StartDate and @ExpirationDate 
                                 group by f.FishID, `Type`;");
                }
                else
                {
                    str.Append(@"SELECT count(FishID) as Num, FishID, `Type` FROM FishInfoRecord where CreateTime between @StartDate and @ExpirationDate group by FishID, `Type`;");
                }
                IEnumerable<FishCount> i = cn.Query<FishCount>(str.ToString(), vbd);

                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<UserFishInfo> GetUserInfo(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                //str.Append(@"SELECT count(f.FishID) as Num, f.FishID, `Type` FROM FishInfoRecord f,Role r 
                //             where f.UserID = r.ID and ( r.ID =@SearchExt or r.NickName=@SearchExt or r.Account=@SearchExt )  and 
                //                   f.CreateTime between @StartDate and @ExpirationDate 
                //             group by f.FishID, `Type`;");

                if (vbd.SearchExt == "")
                {
                    str.Append(@"
                    select a.Type ,case a.Type when 1 then '被赠送' when 2 then '购买' when 3 then '放生' when 0 then '赠送给' end Oper,sum(b.FishPrice) FishPrice 
                        ,sum(case a.FishID when 1 then 1 else 0 end ) Fish1 ,sum(case a.FishID when 2 then 1 else 0 end ) Fish2
                        ,sum(case a.FishID when 3 then 1 else 0 end ) Fish3 ,sum(case a.FishID when 4 then 1 else 0 end ) Fish4 
                        ,sum(case a.FishID when 5 then 1 else 0 end ) Fish5,sum(case a.FishID when 6 then 1 else 0 end ) Fish6
                    from 515game.FishInfoRecord a join record.S_FishInfo b on a.FishID = b.FishID
                        join 515game.Role r on a.UserID = r.id  and r.agent <> 10010
                    where a.Type in (0,1,2,3) and a.CreateTime between @StartDate and @ExpirationDate and a.CreateTime>='2016-05-28 06:00:00'
                    group by a.Type ,case a.Type when 1 then '被赠送' when 2 then '购买' when 3 then '放生' when 0 then '赠送给' end
                    union all 
                    select 5 ,'用户鱼缸' ,sum(case a.Type when 3 then (FishPrice * -1) else b.FishPrice end) FishPrice 
                        ,sum(case a.FishID when 1 then 1 else 0 end ) Fish1 ,sum(case a.FishID when 2 then 1 else 0 end ) Fish2
                        ,sum(case a.FishID when 3 then 1 else 0 end ) Fish3 ,sum(case a.FishID when 4 then 1 else 0 end ) Fish4 
                        ,sum(case a.FishID when 5 then 1 else 0 end ) Fish5,sum(case a.FishID when 6 then 1 else 0 end ) Fish6
                    from FishInfoRecord a join record.S_FishInfo b on a.FishID = b.FishID 
                        join 515game.Role r on a.UserID = r.id  and r.agent <> 10010 
                    where a.Type in (1,2,3) and a.Flag = 1 and a.CreateTime between @StartDate and @ExpirationDate  ;
                ");
                }
                else
                {
                    str.Append(@"
                    select a.Type ,case a.Type when 1 then '被赠送' when 2 then '购买' when 3 then '放生' when 0 then '赠送给' end Oper,sum(b.FishPrice) FishPrice 
                        ,sum(case a.FishID when 1 then 1 else 0 end ) Fish1 ,sum(case a.FishID when 2 then 1 else 0 end ) Fish2
                        ,sum(case a.FishID when 3 then 1 else 0 end ) Fish3 ,sum(case a.FishID when 4 then 1 else 0 end ) Fish4 
                        ,sum(case a.FishID when 5 then 1 else 0 end ) Fish5,sum(case a.FishID when 6 then 1 else 0 end ) Fish6
                    from 515game.FishInfoRecord a join record.S_FishInfo b on a.FishID = b.FishID
                        join 515game.Role r on a.UserID = r.id and r.agent <> 10010
                    where a.Type in (0,1,2,3) and a.CreateTime between @StartDate and @ExpirationDate and a.userid = @SearchExt and a.CreateTime>='2016-05-28 06:00:00'
                    group by a.Type ,case a.Type when 1 then '被赠送' when 2 then '购买' when 3 then '放生' when 0 then '赠送给' end 
                    union all 
                    select 5 ,'用户鱼缸' ,sum(case a.Type when 3 then (FishPrice * -1) else b.FishPrice end) FishPrice 
                        ,sum(case a.FishID when 1 then 1 else 0 end ) Fish1 ,sum(case a.FishID when 2 then 1 else 0 end ) Fish2
                        ,sum(case a.FishID when 3 then 1 else 0 end ) Fish3 ,sum(case a.FishID when 4 then 1 else 0 end ) Fish4 
                        ,sum(case a.FishID when 5 then 1 else 0 end ) Fish5,sum(case a.FishID when 6 then 1 else 0 end ) Fish6
                    from FishInfoRecord a join record.S_FishInfo b on a.FishID = b.FishID 
                        join 515game.Role r on a.UserID = r.id and r.agent <> 10010 
                    where a.Type in (1,2,3) and a.Flag = 1 and a.CreateTime between @StartDate and @ExpirationDate and a.userid = @SearchExt ;
                ");
                }


                IEnumerable<UserFishInfo> i = cn.Query<UserFishInfo>(str.ToString(), vbd);
                cn.Close();
                return i;
            }
        }

    }
}
