using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Data.Model;
using  GL.Command.DBUtility;
using MySql.Data.MySqlClient;
using GL.Common;
using GL.Dapper;
using System.Data;

namespace GL.Data.DAL
{
    public class BaseDataDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static IEnumerable<BaseDataInfo> GetRegisteredUsers(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, sum(RegisterNum) as count FROM record.Clearing_RoleStatic where CreateTime between @StartDate and @ExpirationDate");

                switch (bdv.Terminals)
                {
                    case terminals.页游:
                    case terminals.安卓:
                    case terminals.IOS:
                        str.AppendFormat(" and AgentID = {0}", (int)terminals.IOS);
                        break;
                    case terminals.所有终端:
                    default:
                        break;
                }

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m' );");
                        break;
                }


                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);


                //SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, count(0) as count FROM Role where UNIX_TIMESTAMP(CreateTime) >= UNIX_TIMESTAMP(@StartDate) and UNIX_TIMESTAMP(CreateTime) <= UNIX_TIMESTAMP(@ExpirationDate) GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<BaseDataInfo> GetActiveUsers(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, sum(ActiveNum) as count FROM record.Clearing_RoleStatic where CreateTime between @StartDate and @ExpirationDate");
                //str.Append("SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, sum(count) as count FROM BG_ActiveUsers where date(CreateTime) >= date(@StartDate) and date(CreateTime) < date(@ExpirationDate)");

                switch (bdv.Terminals)
                {
                    case terminals.页游:
                    case terminals.安卓:
                    case terminals.IOS:
                        str.AppendFormat(" and AgentID = {0}", (int)terminals.IOS);
                        break;
                    case terminals.所有终端:
                    default:
                        break;
                }

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m' );");
                        break;
                }


                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);


                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<BaseDataInfoForOnlinePlay> GetOnlinePlay(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append("SELECT CreateTime as date, Online as online, Playing as playing FROM BG_OnlinePlaying where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d')");


                str.Append("SELECT CreateTime as date, Online as online, Playing as playing FROM 515game.BG_OnlinePlaying where CreateTime >= date(@StartDate) and CreateTime < date_add(date(@StartDate), interval 1 day)");

                //switch (bdv.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append("");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );");
                        break;
                }


                IEnumerable<BaseDataInfoForOnlinePlay> i = cn.Query<BaseDataInfoForOnlinePlay>(str.ToString(), bdv);


                cn.Close();
                return i;
            }
        }

        internal static int GetAllPlayCount(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();


                //str.Append("SELECT sum(Count) FROM (SELECT Count FROM PlayCountTexas union all SELECT Count from PlayCountLand) A;");
                str.Append("record.sys_get_game_times");
                int i = cn.Query<int>(str.ToString(), commandType: CommandType.StoredProcedure).First();
                cn.Close();
                return i;
            }
        }



        internal static object GetAllUser(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("SELECT count(0) FROM Role;");
                int i = cn.Query<int>(str.ToString()).First();
                cn.Close();
                return i;
            }
        }



        internal static IEnumerable<BaseDataInfo> GetPlayCount(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( @StartDate, '%Y-%m-%d') as date, sum(count) as count FROM (SELECT Count, CreateTime FROM PlayCountTexas union all SELECT Count,CreateTime from PlayCountLand) A ");

                //switch (bdv.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                        break;
                    case groupby.按月:
                        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                        break;
                }


                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);


                cn.Close();
                return i;
            }
        }
        internal static IEnumerable<BaseDataInfoForRetentionRates> GetRetentionRates(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //                str.Append(@"select aaa.newuser, aaa.date, 
                //(select count(0) from BG_LoginRecord where UserID in (select ID from Role where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(aaa.date, '%Y-%m-%d')) and DATE_FORMAT( LoginTime, '%Y-%m-%d') = DATE_FORMAT(date_add(aaa.date, interval 1 day), '%Y-%m-%d')) / aaa.newuser as oneday,

                //(select count(0) from BG_LoginRecord where UserID in (select ID from Role where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(aaa.date, '%Y-%m-%d')) and DATE_FORMAT( LoginTime, '%Y-%m-%d') = DATE_FORMAT(date_add(aaa.date, interval 2 day), '%Y-%m-%d')) / aaa.newuser as twoday,

                //(select count(0) from BG_LoginRecord where UserID in (select ID from Role where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(aaa.date, '%Y-%m-%d')) and DATE_FORMAT( LoginTime, '%Y-%m-%d') = DATE_FORMAT(date_add(aaa.date, interval 3 day), '%Y-%m-%d')) / aaa.newuser as threeday,

                //(select count(0) from BG_LoginRecord where UserID in (select ID from Role where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(aaa.date, '%Y-%m-%d')) and DATE_FORMAT( LoginTime, '%Y-%m-%d') = DATE_FORMAT(date_add(aaa.date, interval 5 day), '%Y-%m-%d')) / aaa.newuser as fiveday,

                //(select count(0) from BG_LoginRecord where UserID in (select ID from Role where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(aaa.date, '%Y-%m-%d')) and DATE_FORMAT( LoginTime, '%Y-%m-%d') = DATE_FORMAT(date_add(aaa.date, interval 7 day), '%Y-%m-%d')) / aaa.newuser as sevenday,

                //(select count(0) from BG_LoginRecord where UserID in (select ID from Role where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(aaa.date, '%Y-%m-%d')) and DATE_FORMAT( LoginTime, '%Y-%m-%d') = DATE_FORMAT(date_add(aaa.date, interval 10 day), '%Y-%m-%d')) / aaa.newuser as tenday,

                //(select count(0) from BG_LoginRecord where UserID in (select ID from Role where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(aaa.date, '%Y-%m-%d')) and DATE_FORMAT( LoginTime, '%Y-%m-%d') = DATE_FORMAT(date_add(aaa.date, interval 15 day), '%Y-%m-%d')) / aaa.newuser as fifteenday,

                //(select count(0) from BG_LoginRecord where UserID in (select ID from Role where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(aaa.date, '%Y-%m-%d')) and DATE_FORMAT( LoginTime, '%Y-%m-%d') = DATE_FORMAT(date_add(aaa.date, interval 30 day), '%Y-%m-%d')) / aaa.newuser as thirtyday

                // from (select count(0) as newuser, DATE_FORMAT(CreateTime, '%Y-%m-%d') as date from Role where CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT( CreateTime, '%Y-%m-%d')) as aaa;");

                str.Append(@"select SUM(RegNum) newuser, CountDate date, 
                              case SUM(RegNum) when 0 then 100 else SUM(oneday)/SUM(RegNum) end AS oneday ,
                              case SUM(RegNum) when 0 then 100 else SUM(twoday)/SUM(RegNum) end AS twoday ,
                              case SUM(RegNum) when 0 then 100 else SUM(threeday)/SUM(RegNum) end AS threeday ,
                              case SUM(RegNum) when 0 then 100 else sum(fiveday)/SUM(RegNum) end AS fiveday ,
                              case SUM(RegNum) when 0 then 100 else sum(sevenday)/SUM(RegNum) end AS sevenday ,
                              case SUM(RegNum) when 0 then 100 else sum(tenday)/SUM(RegNum) end AS tenday ,
                              case SUM(RegNum) when 0 then 100 else sum(fifteenday)/SUM(RegNum) end AS fifteenday ,
                              case SUM(RegNum) when 0 then 100 else sum(thirtday)/SUM(RegNum) end AS thirtday 
                            from record.Clearing_UserKeep
                            where CountDate between @StartDate and @ExpirationDate 
                            group by CountDate order by date desc;");


                IEnumerable<BaseDataInfoForRetentionRates> i = cn.Query<BaseDataInfoForRetentionRates>(str.ToString(), bdv);

                cn.Close();
                return i;
            }
        }






        internal static IEnumerable<BaseDataInfoForBankruptcyRate> GetBankruptcyRate(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append("SELECT DATE_FORMAT(curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold > 10) as a, (SELECT count(0) as count FROM Role where Gold <= 10) as b;");
                str.Append("SELECT curdate() as date ,sum(case when Gold > 10 then 1 else 0 end) as count, sum(case when Gold <= 10 then 1 else 0 end) as activeuser FROM 515game.Role ; ");

                //switch (vbd.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                //switch (vbd.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                //        break;
                //    case groupby.按月:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                //        break;
                //}


                IEnumerable<BaseDataInfoForBankruptcyRate> i = cn.Query<BaseDataInfoForBankruptcyRate>(str.ToString(), vbd);


                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<QQZoneRechargeCount> GetQQZoneRechargeCount(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append(@"
                //    SELECT DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, count(distinct UserID) as rechargeUser, sum(Money) as rechargeCount FROM QQZoneRecharge where CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                //    select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, count(0) as registerUser  from Role where CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                //    select DATE_FORMAT(q.CreateTime, '%Y-%m-%d') as date, count(distinct UserID) as fristRecharge from QQZoneRecharge as q,Role as r where UserID = ID and DATE_FORMAT(q.CreateTime, '%Y-%m-%d') = DATE_FORMAT(r.CreateTime, '%Y-%m-%d') and IsFirst = 1 and q.CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(q.CreateTime, '%Y-%m-%d');

                //    select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date,count(distinct UserID) as nextRecharge from (select UserID,IsFirst,CreateTime from (select UserID,IsFirst,CreateTime from QQZoneRecharge where CreateTime between @StartDate and @ExpirationDate order by IsFirst desc) as aa group by UserID) as bb where IsFirst = 0 group by DATE_FORMAT(CreateTime, '%Y-%m-%d');

                //    select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, count(distinct UserID) as newRecharge from QQZoneRecharge where IsFirst = 1 and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');");

                //str.Append(@"select t1.CountDate as date,t1.RegNum as registerUser,t2.NumFirst as fristRecharge,t2.Money as rechargeCount, t2.NumNew/t1.RegNum as rechargeRate,t2.Money/t2.Num as ARPU,t2.Num - t2.NumNew as nextRecharge, t2.NumNew as newRecharge
                //            from record.Clearing_Reg t1
                //            left join (
                //              select Agent ,CountDate ,sum(Money) Money ,sum(Num) Num ,sum(MoneyFirst) MoneyFirst 
                //                ,sum(NumFirst) NumFirst ,sum(MoneyNew) MoneyNew ,sum(NumNew) NumNew
                //              from record.Clearing_Charge where CountDate between @StartDate and @ExpirationDate
                //              group by Agent
                //            )t2 on t1.CountDate = t2.CountDate and t1.Agent = t2.Agent; ");

                //str.Append(@"select t1.CountDate as date,
                //            t1.RegNum as registerUser,
                //            t2.NumFirst as rechargeUser,
                //            t2.Money as rechargeCount, 
                //            t2.NumNew/t1.RegNum as rechargeRate,
                //            t2.Money/t2.Num as ARPU,
                //            t2.Num - t2.NumNew as nextRecharge, 
                //            t2.NumNew as newRecharge
                //            from (select CountDate, sum(RegNum)RegNum from record.Clearing_Reg where CountDate between @StartDate and @ExpirationDate group by CountDate) t1
                //            left join( select CountDate, sum(Money) Money, sum(Num) Num, sum(MoneyFirst) MoneyFirst, sum(NumFirst) NumFirst, sum(MoneyNew) MoneyNew, sum(NumNew) NumNew
                //            from record.Clearing_Charge where CountDate between @StartDate and @ExpirationDate
                //            group by CountDate)t2 on t1.CountDate = t2.CountDate; ");


                str.Append(@"record.sys_get_charge_rate");

                //call sys_get_charge_rate('2015-11-01', '2015-11-10');




                //IEnumerable<QQZoneRechargeCount> i;
                //using (var multi = cn.QueryMultiple(str.ToString(), vbd))
                //{
                //    var recharge = multi.Read<QQZoneRechargeCount>().ToList();
                //    var register = multi.Read<QQZoneRechargeCount>().ToList();
                //    var rechargeCount = multi.Read<QQZoneRechargeCount>().ToList();
                //    var next = multi.Read<QQZoneRechargeCount>().ToList();
                //    var newRecharge = multi.Read<QQZoneRechargeCount>().ToList();


                //    i = recharge.Union(register).Union(rechargeCount).Union(next).Union(newRecharge).GroupBy(n => n.date).Select(g => new QQZoneRechargeCount { date = g.Select(s => s.date).First(), registerUser = g.Select(s => s.registerUser).Sum(), rechargeUser = g.Select(s => s.newRecharge).Sum(), rechargeCount = g.Select(s => s.rechargeCount).Sum(), nextRecharge = g.Select(s => s.nextRecharge).Sum(), ARPU = g.Select(s => s.rechargeCount).Sum() / (g.Select(s => s.rechargeUser).Sum() == 0 ? 1 : g.Select(s => s.rechargeUser).Sum()), rechargeRate = g.Select(s => s.fristRecharge).Sum() / (g.Select(s => s.registerUser).Sum() == 0 ? 1 : g.Select(s => s.registerUser).Sum()), newRecharge = g.Select(s => s.newRecharge).Sum() }).OrderBy(x => x.date).ToList();

                //    //i = recharge.Zip<QQZoneRechargeCount, QQZoneRechargeCount, QQZoneRechargeCount>(register, (n, w) => new QQZoneRechargeCount { date = n.date, registerUser = n.registerUser + w.registerUser, rechargeUser = n.rechargeUser + w.rechargeUser, rechargeCount = n.rechargeCount + w.rechargeCount, rechargeRate = 0, ARPU = 0, nextRecharge = n.nextRecharge + w.nextRecharge }).ToList();


                //}
                IEnumerable<QQZoneRechargeCount> i = cn.Query<QQZoneRechargeCount>(str.ToString(), param: new { begin_date = vbd.StartDate, end_date = vbd.ExpirationDate, agent_id=vbd.Channels }, commandType: CommandType.StoredProcedure);


                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<GameOutput> GetGameOutput(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                //                str.Append(@"select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(ChipChange > 0, ChipChange, 0)) as Output, sum( if(ChipChange > 0, 0, ChipChange)) as Consume from record.BG_UserMoneyRecord where CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                //                            select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum(ChipChange) as Recharge from record.BG_UserMoneyRecord where CreateTime between @StartDate and @ExpirationDate and type = 5 group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                //                            select sum(Gold) as systemBargainingChip from Role");

                str.Append(@"select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(ChipChange > 0, ChipChange, 0)) as ChipOutput from record.BG_UserMoneyRecord where `type` in (1,2,3, 5,20,12,13,16,9,37,39,45,46,48,51,44,59,60,61,62,63,67,70,71,74,79,80,81,82,83,84,85,86,88) and  CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                    select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(ChipChange > 0, 0, ChipChange)) as ChipConsume from record.BG_UserMoneyRecord where CreateTime between @StartDate and @ExpirationDate and `type` in (24,25,26,22,23,19,7,21,18,17,47,50,64,65,68,76) group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                    select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum(ChipChange) as ChipRecharge from record.BG_UserMoneyRecord where type = 5 and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');

                    select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(DiamondChange > 0, DiamondChange, 0)) as DiamondOutput from record.BG_UserMoneyRecord where `type` in (40,6,38,45) and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                    select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(DiamondChange > 0, 0, DiamondChange)) as DiamondConsume from record.BG_UserMoneyRecord where `type` in (8,4) and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                    select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum(DiamondChange) as DiamondRecharge from record.BG_UserMoneyRecord where type = 6 and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');

                    

                    select sum(Gold + SafeBox) as systemBargainingChip, sum(Diamond) as systemDiamond, sum(Zicard) as systemScore from Role where IsFreeze = 0 and agent <> 10010;

                    SELECT sum(if(Type = 2, 1, 0) + if(Type = 3, -1, 0)) as ChipOutput, FishID as DiamondOutput FROM FishInfoRecord group by FishID");

                //switch (vbd.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                //switch (vbd.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                //        break;
                //    case groupby.按月:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                //        break;
                //}


                IEnumerable<GameOutput> i;
                using (var multi = cn.QueryMultiple(str.ToString(), vbd))
                {
                    var ChipOutput = multi.Read<GameOutput>().ToList();
                    var ChipConsume = multi.Read<GameOutput>().ToList();
                    var ChipRecharge = multi.Read<GameOutput>().ToList();
                    var DiamondOutput = multi.Read<GameOutput>().ToList();
                    var DiamondConsume = multi.Read<GameOutput>().ToList();
                    var DiamondRecharge = multi.Read<GameOutput>().ToList();
                    var systemSum = multi.Read<GameOutput>().ToList();
                    var FishChip = multi.Read<GameOutput>().ToList();




                    i = ChipOutput.Union(ChipConsume).Union(ChipRecharge).Union(DiamondOutput).Union(DiamondConsume).Union(DiamondRecharge).GroupBy(n => n.date).Select(g => new GameOutput
                    {
                        date = g.Select(s => s.date).First(),
                        ChipOutput = g.Select(s => s.ChipOutput).Sum(),
                        ChipConsume = g.Select(s => s.ChipConsume).Sum(),
                        ChipRecharge = g.Select(s => s.ChipRecharge).Sum(),
                        DiamondOutput = g.Select(s => s.DiamondOutput).Sum(),
                        DiamondConsume = g.Select(s => s.DiamondConsume).Sum(),
                        DiamondRecharge = g.Select(s => s.DiamondRecharge).Sum(),
                        systemBargainingChip = systemSum.Select(x => x.systemBargainingChip).Sum(),
                        systemDiamond = systemSum.Select(x => x.systemDiamond).Sum(),
                        systemFishChip = FishChip.Select(x => x.DiamondOutput == 1 ? x.ChipOutput * 200000 : x.DiamondOutput == 2 ? x.ChipOutput * 1000000 : x.DiamondOutput == 3 ? x.ChipOutput * 2000000 : x.DiamondOutput == 4 ? x.ChipOutput * 5000000 : 0).Sum()
                    }).OrderBy(x => x.date).ToList();

                    //i = recharge.Zip<QQZoneRechargeCount, QQZoneRechargeCount, QQZoneRechargeCount>(register, (n, w) => new QQZoneRechargeCount { date = n.date, registerUser = n.registerUser + w.registerUser, rechargeUser = n.rechargeUser + w.rechargeUser, rechargeCount = n.rechargeCount + w.rechargeCount, rechargeRate = 0, ARPU = 0, nextRecharge = n.nextRecharge + w.nextRecharge }).ToList();


                }


                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<GameOutput> GetGameOutput2(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                //                str.Append(@"select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(ChipChange > 0, ChipChange, 0)) as Output, sum( if(ChipChange > 0, 0, ChipChange)) as Consume from record.BG_UserMoneyRecord where CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                //                            select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum(ChipChange) as Recharge from record.BG_UserMoneyRecord where CreateTime between @StartDate and @ExpirationDate and type = 5 group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
                //                            select sum(Gold) as systemBargainingChip from Role");

                str.Append(@"select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipOutput from record.Clearing_UserMoneyRecord where `RecordType` in (1,2,3, 5,20,12,13,16,9,37,39,45,46,48,51,44,59,60,61,62,63,67,70,71,74,79,80,81,82,83,84,85,86,88) and RecordTime between @StartDate and @ExpirationDate group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                    select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipConsume from record.Clearing_UserMoneyRecord where RecordTime between @StartDate and @ExpirationDate and `RecordType` in (24,25,26,22,23,19,7,21,18,17,47,50,64,65,68,76) group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                    select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipRecharge from record.Clearing_UserMoneyRecord where RecordType = 5 and RecordTime between @StartDate and @ExpirationDate group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                    select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondOutput from record.Clearing_UserMoneyRecord where `RecordType` in (40,6,38,45) and RecordTime between @StartDate and @ExpirationDate group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                    select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondConsume from record.Clearing_UserMoneyRecord where `RecordType` in (8,4) and RecordTime between @StartDate and @ExpirationDate group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                    select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondRecharge from record.Clearing_UserMoneyRecord where RecordType = 6 and RecordTime between @StartDate and @ExpirationDate group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                    select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, systemChip as systemBargainingChip, systemDiamond, systemScore, systemFish as systemFishChip from record.Clearing_UserMoneyStock where RecordTime between @StartDate and @ExpirationDate group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                   ");

                //switch (vbd.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                //switch (vbd.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                //        break;
                //    case groupby.按月:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                //        break;
                //}


                IEnumerable<GameOutput> i;
                using (var multi = cn.QueryMultiple(str.ToString(), vbd))
                {
                    var ChipOutput = multi.Read<GameOutput>().ToList();
                    var ChipConsume = multi.Read<GameOutput>().ToList();
                    var ChipRecharge = multi.Read<GameOutput>().ToList();
                    var DiamondOutput = multi.Read<GameOutput>().ToList();
                    var DiamondConsume = multi.Read<GameOutput>().ToList();
                    var DiamondRecharge = multi.Read<GameOutput>().ToList();
                    var systemSum = multi.Read<GameOutput>().ToList();
                    //var FishChip = multi.Read<GameOutput>().ToList();

                    i = ChipOutput.Union(ChipConsume).Union(ChipRecharge).Union(DiamondOutput).Union(DiamondConsume).Union(DiamondRecharge).Union(systemSum).GroupBy(n => n.date).Select(g => new GameOutput
                    {
                        date = g.Select(s => s.date).First(),
                        ChipOutput = g.Select(s => s.ChipOutput).Sum(),
                        ChipConsume = g.Select(s => s.ChipConsume).Sum(),
                        ChipRecharge = g.Select(s => s.ChipRecharge).Sum(),
                        DiamondOutput = g.Select(s => s.DiamondOutput).Sum(),
                        DiamondConsume = g.Select(s => s.DiamondConsume).Sum(),
                        DiamondRecharge = g.Select(s => s.DiamondRecharge).Sum(),
                        systemBargainingChip = g.Select(s => s.systemBargainingChip).Sum(),
                        systemDiamond = g.Select(s => s.systemDiamond).Sum(),
                        systemFishChip = g.Select(s => s.systemFishChip).Sum()
                    }).OrderBy(x => x.date).ToList();

                    //i = recharge.Zip<QQZoneRechargeCount, QQZoneRechargeCount, QQZoneRechargeCount>(register, (n, w) => new QQZoneRechargeCount { date = n.date, registerUser = n.registerUser + w.registerUser, rechargeUser = n.rechargeUser + w.rechargeUser, rechargeCount = n.rechargeCount + w.rechargeCount, rechargeRate = 0, ARPU = 0, nextRecharge = n.nextRecharge + w.nextRecharge }).ToList();


                }


                cn.Close();
                return i;
            }
        }


        internal static GameOutputDetail GetGameOutputDetail(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append(@"SELECT sum(ChipChange) as Chip, sum(DiamondChange) as Diamond, sum(ScoreChange) as Score, `type` as ChipChangeType from record.BG_UserMoneyRecord where `type` in (1,2,3,4,5,6,7,8,9,12,13,16,17,18,19,20,21,22,23,24,25,26,37,38,39,40,41,42,43,44,45,46,47,48,50,51,59,60,61,62,63,64,65,67,68,70,71,74,76,79,80,81,82,83,84,85,86,88) and CreateTime between @StartDate and date_add( @StartDate, interval 1 day) group by `Type`;
                
                select sum(Gold + SafeBox) as systemBargainingChip, sum(Diamond) as systemDiamond, sum(Zicard) as systemScore from Role where IsFreeze = 0 and agent <> 10010;");
                //                str.Append(@"SELECT sum(ChipChange) as Chip, sum(DiamondChange) as Diamond, sum(ScoreChange) as Score, `type` as ChipChangeType from record.BG_UserMoneyRecord where `type` in (1,2,3,4,5,6,7,8,9,12,13,16,17,18,19,20,21,22,23,24,25,26,37,38,39,40,41,42,43,44,45,46,47,48,50,51,59,60,61,62,63) and CreateTime between @StartDate and date_add( @StartDate, interval 1 day) group by `Type`;
                //                select sum(Gold) as systemBargainingChip, sum(Diamond) as systemDiamond, sum(Zicard) as systemScore from Role where IsFreeze = 0;");
                //str.Append(@"SELECT sum(ChipChange) as Chip, sum(DiamondChange) as Diamond, `type` as ChipChangeType from record.BG_UserMoneyRecord where  CreateTime between @StartDate and date_add( @StartDate, interval 1 day) group by `Type`;
                //select sum(Gold) as systemBargainingChip from Role");

                //switch (vbd.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                //switch (vbd.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                //        break;
                //    case groupby.按月:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                //        break;
                //}


                GameOutputDetail j = new GameOutputDetail();

                using (var multi = cn.QueryMultiple(str.ToString(), vbd))
                {
                    var i = multi.Read<GameOutputList>().ToList();
                    var systemSum = multi.Read<GameOutput>().ToList();

                    j.date = Convert.ToDateTime(vbd.StartDate);
                    j.systemBargainingChip = systemSum.Select(x => x.systemBargainingChip).Sum();
                    j.systemDiamond = systemSum.Select(x => x.systemDiamond).Sum();
                    j.systemScore = systemSum.Select(x => x.systemScore).Sum();
                    j.list = i.ToList();


                }

                cn.Close();
                return j;
            }
        }

        internal static GameOutputDetail GetGameOutputDetail2(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append(@"SELECT sum(Chip) as Chip, sum(Diamond) as Diamond, sum(Score) as Score, `RecordType` as ChipChangeType from record.Clearing_UserMoneyRecord where `RecordType` in (1,2,3,4,5,6,7,8,9,12,13,16,17,18,19,20,21,22,23,24,25,26,37,38,39,40,41,42,43,44,45,46,47,48,50,51,59,60,61,62,63,64,65,67,68,70,71,74,76,79,80,81,82,83,84,85,86,88) and RecordTime = @StartDate group by `RecordType`;
                  select systemChip as systemBargainingChip, systemDiamond, systemScore, systemFish as systemFishChip from record.Clearing_UserMoneyStock where RecordTime = @StartDate ");



                //                str.Append(@"SELECT sum(ChipChange) as Chip, sum(DiamondChange) as Diamond, sum(ScoreChange) as Score, `type` as ChipChangeType from record.BG_UserMoneyRecord where `type` in (1,2,3,4,5,6,7,8,9,12,13,16,17,18,19,20,21,22,23,24,25,26,37,38,39,40,41,42,43,44,45,46,47,48,50,51,59,60,61,62,63) and CreateTime between @StartDate and date_add( @StartDate, interval 1 day) group by `Type`;
                //                select sum(Gold) as systemBargainingChip, sum(Diamond) as systemDiamond, sum(Zicard) as systemScore from Role where IsFreeze = 0;");
                //str.Append(@"SELECT sum(ChipChange) as Chip, sum(DiamondChange) as Diamond, `type` as ChipChangeType from record.BG_UserMoneyRecord where  CreateTime between @StartDate and date_add( @StartDate, interval 1 day) group by `Type`;
                //select sum(Gold) as systemBargainingChip from Role");

                //switch (vbd.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                //switch (vbd.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                //        break;
                //    case groupby.按月:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                //        break;
                //}


                GameOutputDetail j = new GameOutputDetail();

                using (var multi = cn.QueryMultiple(str.ToString(), vbd))
                {
                    var i = multi.Read<GameOutputList>().ToList();
                    var systemSum = multi.Read<GameOutput>().ToList();

                    j.date = Convert.ToDateTime(vbd.StartDate);
                    j.systemBargainingChip = systemSum.Select(x => x.systemBargainingChip).Sum();
                    j.systemDiamond = systemSum.Select(x => x.systemDiamond).Sum();
                    j.systemScore = systemSum.Select(x => x.systemScore).Sum();
                    j.list = i.ToList();


                }

                cn.Close();
                return j;
            }
        }


        internal static IEnumerable<PotRecord> GetPotRecord()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT * from BG_HidePotRecord;");


                IEnumerable<PotRecord> i = cn.Query<PotRecord>(str.ToString());


                cn.Close();
                return i;
            }
        }
        internal static IEnumerable<PotRecord> GetPotRecord(int Num)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat("SELECT * from BG_HidePotRecord where GameID = {0};", Num);


                IEnumerable<PotRecord> i = cn.Query<PotRecord>(str.ToString());


                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<JiFen> GetScoreboard(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                str.Append("SELECT playerID, UserName, sum(JiFen) as JiFen FROM JiFen group by playerID order by JiFen desc limit 0,30;");



                IEnumerable<JiFen> i = cn.Query<JiFen>(str.ToString());


                cn.Close();
                return i;
            }
        }



        internal static IEnumerable<OpenFuDai> GetOpenFuDai(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("SELECT UserID,UserName ,sum(Count) as Count,Createtime FROM OpenFuDai where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d') group by UserID order by Count desc limit 0,30;");

                IEnumerable<OpenFuDai> i = cn.Query<OpenFuDai>(str.ToString(), vbd);


                cn.Close();
                return i;
            }
        }





        internal static BaseDataInfoForUsersGoldDistributionRatio GetUsersGoldDistributionRatio()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                //str.Append("SELECT DATE_FORMAT( curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold <= 10) as a, (SELECT count(0) as count FROM Role where Gold > 10) as b;");

                str.Append(@"record.sys_get_gold_rate;");

                //switch (vbd.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                //switch (vbd.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                //        break;
                //    case groupby.按月:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                //        break;
                //}


                IEnumerable<BaseDataInfoForUsersGoldDistributionRatio> i = cn.Query<BaseDataInfoForUsersGoldDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure);


                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static BaseDataInfoForUsersDiamondDistributionRatio GetUsersDiamondDistributionRatio()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();




                //str.Append("SELECT DATE_FORMAT( curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold <= 10) as a, (SELECT count(0) as count FROM Role where Gold > 10) as b;");


                //str.Append(@"select 
                //(SELECT count(0) FROM Role) as a0
                //,(SELECT count(0) FROM Role where Diamond = 0) as a1
                //,(SELECT count(0) FROM Role where Diamond between 1 and 10) as a2
                //,(SELECT count(0) FROM Role where Diamond between 11 and 50) as a3
                //,(SELECT count(0) FROM Role where Diamond between 51 and 99) as a4
                //,(SELECT count(0) FROM Role where Diamond between 100 and 1000) as a5
                //,(SELECT count(0) FROM Role where Diamond between 1001 and 5000) as a6
                //,(SELECT count(0) FROM Role where Diamond between 5001 and 20000) as a7
                //,(SELECT count(0) FROM Role where Diamond between 20001 and 50000) as a8
                //,(SELECT count(0) FROM Role where Diamond between 50001 and 100000) as a9
                //,(SELECT count(0) FROM Role where Diamond between 100001 and 300000) as a10
                //,(SELECT count(0) FROM Role where Diamond between 300001 and 1000000) as a11
                //,(SELECT count(0) FROM Role where Diamond > 1000000) as a12");

                str.Append(@"record.sys_get_diamond_rate");


                //switch (vbd.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                //switch (vbd.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                //        break;
                //    case groupby.按月:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                //        break;
                //}


                IEnumerable<BaseDataInfoForUsersDiamondDistributionRatio> i = cn.Query<BaseDataInfoForUsersDiamondDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure);


                cn.Close();
                return i.FirstOrDefault();
            }
        }





        internal static IEnumerable<BaseDataInfoForPotRakeback> GetPotRakeback(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                str.Append(@"select CreateTime as date, ChipChange as Chip from record.BG_UserMoneyRecord where Type = 49 and CreateTime between @StartDate and @ExpirationDate");


                IEnumerable<BaseDataInfoForPotRakeback> i = cn.Query<BaseDataInfoForPotRakeback>(str.ToString(), vbd);


                cn.Close();
                return i;
            }
        }

        internal static BaseDataInfoForVIPDistributionRatio GetVIPDistributionRatio()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();




                //str.Append("SELECT DATE_FORMAT( curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold <= 10) as a, (SELECT count(0) as count FROM Role where Gold > 10) as b;");


                //str.Append(@"select 
                //(SELECT count(0) FROM Role) as a0
                //,(SELECT count(0) FROM Role where MaxNoble = 0) as a1
                //,(SELECT count(0) FROM Role where MaxNoble = 1) as a2
                //,(SELECT count(0) FROM Role where MaxNoble = 2) as a3
                //,(SELECT count(0) FROM Role where MaxNoble = 3) as a4
                //,(SELECT count(0) FROM Role where MaxNoble = 4) as a5
                //,(SELECT count(0) FROM Role where MaxNoble = 5) as a6
                //,(SELECT count(0) FROM Role where MaxNoble = 6) as a7
                //,(SELECT count(0) FROM Role where MaxNoble = 7) as a8
                //,(SELECT count(0) FROM Role where MaxNoble = 8) as a9
                //,(SELECT count(0) FROM Role where MaxNoble = 9) as a10
                //,(SELECT count(0) FROM Role where MaxNoble = 10) as a11");

                str.Append(@"record.sys_get_vip_rate");




                IEnumerable<BaseDataInfoForVIPDistributionRatio> i = cn.Query<BaseDataInfoForVIPDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure);


                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static IEnumerable<BaseDataInfo> GetNoviceTask(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( FinishTime, '%Y-%m-%d') as date, count(0) as count FROM FinishTask where date(FinishTime) >= date(@StartDate) and date(FinishTime) < date(@ExpirationDate) group by DATE_FORMAT( FinishTime, '%Y-%m-%d')");


                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);


                cn.Close();
                return i;
            }
        }


    }
}
