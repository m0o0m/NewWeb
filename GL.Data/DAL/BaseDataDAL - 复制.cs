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
    public class BaseDataDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        /// <summary>
        /// 注册用户 小时
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<BaseDataInfo> GetRegisteredUsersOnHour(BaseDataView bdv) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append(@"
                    select hour, sum(RegisterNum) as count from(
                    select CountDate, hour(CountDate) as hour, RegNum as RegisterNum
                    FROM record.Clearing_Reg
                    where CountDate between @StartDate and @ExpirationDate and CountDate != @ExpirationDate
                    ) a group by a.`hour` ");

                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<BaseDataInfo> GetRegisteredUsers(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( CountDate, '%Y-%m-%d') as date, sum(RegNum) as count FROM record.Clearing_Reg where CountDate between @StartDate and @ExpirationDate  ");

                if (bdv.Channels > 0)
                {
                    str.AppendFormat(" and find_in_set(Agent, '{0}')", bdv.UserList); 
                }

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" GROUP BY DATE_FORMAT( CountDate, '%Y-%m-%d' )");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CountDate, '%Y-%m' ) ");
                        break;
                }
                str.Append("  order by CountDate desc  ");

                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);

                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 活跃用户
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<BaseDataInfo> GetActiveUsers(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, sum(ActiveNum) as count FROM record.Clearing_RoleStatic where CreateTime between @StartDate and @ExpirationDate");

                if (bdv.Channels > 0)
                {
                    str.AppendFormat(" and find_in_set(AgentID, '{0}')", bdv.UserList);
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

        /// <summary>
        /// 在线在玩
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<BaseDataInfoForOnlinePlay> GetOnlinePlay(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.Append("SELECT CreateTime as date, Online as online, Playing as playing FROM 515game.BG_OnlinePlaying where CreateTime >= date(@StartDate) and CreateTime < date_add(date(@StartDate), interval 1 day)");
                if (bdv.Channels > 0)
                {
                    str.AppendFormat(" and find_in_set(AgentID, '{0}')", bdv.UserList);
                }
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


        /// <summary>
        /// 用户统计
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        internal static object GetAllUser(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                if (vbd.Channels > 0)
                {
                    str.AppendFormat("SELECT count(0) FROM V_Role where find_in_set(Agent, '{0}')", vbd.UserList);
                }
                else
                {
                    str.Append("SELECT count(0) FROM V_Role;");
                }
                int i = cn.Query<int>(str.ToString()).First();
                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 留存率
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<BaseDataInfoForRetentionRates> GetRetentionRates(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"select SUM(RegNum) newuser, CountDate date, 
                            case SUM(RegNum) when 0 then 100 else SUM(oneday)/SUM(RegNum) end AS oneday ,
                            case SUM(RegNum) when 0 then 100 else SUM(twoday)/SUM(RegNum) end AS twoday ,
                            case SUM(RegNum) when 0 then 100 else SUM(threeday)/SUM(RegNum) end AS threeday ,
                            case SUM(RegNum) when 0 then 100 else sum(fiveday)/SUM(RegNum) end AS fiveday ,
                            case SUM(RegNum) when 0 then 100 else sum(sevenday)/SUM(RegNum) end AS sevenday ,
                            case SUM(RegNum) when 0 then 100 else sum(tenday)/SUM(RegNum) end AS tenday ,
                            case SUM(RegNum) when 0 then 100 else sum(fifteenday)/SUM(RegNum) end AS fifteenday ,
                            case SUM(RegNum) when 0 then 100 else sum(thirtday)/SUM(RegNum) end AS thirtyday 
                        from record.Clearing_UserKeep
                        where CountDate between @StartDate and @ExpirationDate {0}
                        group by CountDate order by date desc;", bdv.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");

                IEnumerable<BaseDataInfoForRetentionRates> i = cn.Query<BaseDataInfoForRetentionRates>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 破产率
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        internal static IEnumerable<BaseDataInfoForBankruptcyRate> GetBankruptcyRate(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //call sys_get_ruin_rate(begin_date date ,end_date date ,agent_id varchar(600));

                str.Append(@"record.sys_get_ruin_rate");

                IEnumerable<BaseDataInfoForBankruptcyRate> i = cn.Query<BaseDataInfoForBankruptcyRate>(str.ToString(), param: new { begin_date = vbd.StartDate, end_date = vbd.ExpirationDate, agent_id = vbd.Channels > 0 ? vbd.UserList : "0" }, commandType: CommandType.StoredProcedure);


                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 充值统计
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        internal static IEnumerable<QQZoneRechargeCount> GetQQZoneRechargeCount(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append(@"record.sys_get_charge_rate");

                //call sys_get_charge_rate('2015-11-01', '2015-11-10');
                IEnumerable<QQZoneRechargeCount> i = cn.Query<QQZoneRechargeCount>(str.ToString(), param: new { begin_date = vbd.StartDate, end_date = vbd.ExpirationDate, agent_id = vbd.Channels > 0 ? vbd.UserList : "0" }, commandType: CommandType.StoredProcedure);

                


                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 产出消耗
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        internal static IEnumerable<GameOutput> GetGameOutput2(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipOutput from record.Clearing_UserMoneyRecord where `RecordType` in (1,2,3, 5,20,12,13,16,9,37,39,45,46,48,51,44,59,60,61,62,63,67,70,71,74,79,80,81,82,83,84,85,86,88,90) and RecordTime between @StartDate and @ExpirationDate {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipConsume from record.Clearing_UserMoneyRecord where RecordTime between @StartDate and @ExpirationDate and `RecordType` in (24,25,26,22,23,19,7,21,18,17,47,50,64,65,68,76) {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipRecharge from record.Clearing_UserMoneyRecord where RecordType = 5 and RecordTime between @StartDate and @ExpirationDate {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondOutput from record.Clearing_UserMoneyRecord where `RecordType` in (40,6,38,45) and RecordTime between @StartDate and @ExpirationDate {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondConsume from record.Clearing_UserMoneyRecord where `RecordType` in (8,4) and RecordTime between @StartDate and @ExpirationDate {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondRecharge from record.Clearing_UserMoneyRecord where RecordType = 6 and RecordTime between @StartDate and @ExpirationDate {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(systemChip) as systemBargainingChip, sum(systemDiamond) as systemDiamond, systemScore, sum(systemFish) as systemFishChip from record.Clearing_UserMoneyStock where RecordTime between @StartDate and @ExpirationDate {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                   ", vbd.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");


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

                }


                cn.Close();
                return i;
            }
        }


        /// <summary>
        /// 产出消耗明细
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        internal static GameOutputDetail GetGameOutputDetail2(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"SELECT sum(Chip) as Chip, sum(Diamond) as Diamond, sum(Score) as Score, `RecordType` as ChipChangeType from record.Clearing_UserMoneyRecord where `RecordType` in (1,2,3,4,5,6,7,8,9,12,13,16,17,18,19,20,21,22,23,24,25,26,37,38,39,40,41,42,43,44,45,46,47,48,50,51,59,60,61,62,63,64,65,67,68,70,71,74,76,79,80,81,82,83,84,85,86,88,90) and RecordTime = @StartDate {0} group by `RecordType`;
                  select systemChip as systemBargainingChip, systemDiamond, systemScore, systemFish as systemFishChip from record.Clearing_UserMoneyStock where RecordTime = @StartDate {0}", vbd.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");

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

        /// <summary>
        /// 金币分布比
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static BaseDataInfoForUsersGoldDistributionRatio GetUsersGoldDistributionRatio(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.Append(@"record.sys_get_gold_rate;");
                IEnumerable<BaseDataInfoForUsersGoldDistributionRatio> i = cn.Query<BaseDataInfoForUsersGoldDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure, param: new { agentids = bdv.Channels > 0 ? bdv.UserList : "0" });

                cn.Close();
                return i.FirstOrDefault();
            }
        }

        /// <summary>
        /// 钻石分布比
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static BaseDataInfoForUsersDiamondDistributionRatio GetUsersDiamondDistributionRatio(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@"record.sys_get_diamond_rate");
                IEnumerable<BaseDataInfoForUsersDiamondDistributionRatio> i = cn.Query<BaseDataInfoForUsersDiamondDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure, param: new { agentids = bdv.Channels > 0 ? bdv.UserList : "0" });
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        /// <summary>
        /// vip分布比
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static BaseDataInfoForVIPDistributionRatio GetVIPDistributionRatio(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@"record.sys_get_vip_rate");
                IEnumerable<BaseDataInfoForVIPDistributionRatio> i = cn.Query<BaseDataInfoForVIPDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure, param: new { agentids = bdv.Channels > 0 ? bdv.UserList : "0" });
                cn.Close();
                return i.FirstOrDefault();
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
        internal static IEnumerable<PotRecord> GetPotRecord( int Num)
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

        public static PagedList<BaseDataInfo> GetNoviceTaskPage(BaseDataView vbd)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = vbd.Page;
            pq.PageSize = 10;

            pq.RecordCount = DAL.PagedListDAL<BaseDataInfo>.GetRecordCount(string.Format(@" select count(0) from ( SELECT DATE_FORMAT(FinishTime, '%Y-%m-%d') as date, count(0) as count FROM FinishTask where FinishTime BETWEEN '{0}'  and '{1}' group by DATE_FORMAT(FinishTime, '%Y-%m-%d') ) a ", vbd.StartDate, vbd.ExpirationDate), sqlconnectionString);



            pq.Sql = string.Format(@"SELECT DATE_FORMAT(FinishTime, '%Y-%m-%d') as date, count(0) as count FROM FinishTask where FinishTime BETWEEN '{0}'  and '{1}' group by DATE_FORMAT(FinishTime, '%Y-%m-%d')  limit {2}, {3}", vbd.StartDate, vbd.ExpirationDate, pq.StartRowNumber, pq.PageSize);

            PagedList<BaseDataInfo> obj = new PagedList<BaseDataInfo>(PagedListDAL<BaseDataInfo>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }


        /// <summary>
        /// 牌局统计
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        //internal static IEnumerable<BaseDataInfo> GetPlayCount(BaseDataView bdv)
        //{
        //    using (var cn = new MySqlConnection(sqlconnectionString))
        //    {
        //        cn.Open();

        //        StringBuilder str = new StringBuilder();

        //        str.Append("SELECT DATE_FORMAT( @StartDate, '%Y-%m-%d') as date, sum(count) as count FROM (SELECT Count, CreateTime FROM PlayCountTexas union all SELECT Count,CreateTime from PlayCountLand) A ");


        //        switch (bdv.Groupby)
        //        {
        //            case groupby.按日:
        //                str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
        //                break;
        //            case groupby.按月:
        //                str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
        //                break;
        //        }


        //        IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);


        //        cn.Close();
        //        return i;
        //    }
        //}


        //internal static int GetAllPlayCount(BaseDataView bdv)
        //{
        //    using (var cn = new MySqlConnection(sqlconnectionString))
        //    {
        //        cn.Open();
        //        StringBuilder str = new StringBuilder();

        //        //str.Append("SELECT sum(Count) FROM (SELECT Count FROM PlayCountTexas union all SELECT Count from PlayCountLand) A;");
        //        str.Append("record.sys_get_game_times");
        //        int i = cn.Query<int>(str.ToString(), commandType : CommandType.StoredProcedure).First();
        //        cn.Close();
        //        return i;
        //    }
        //}

        ///// <summary>
        ///// 产出消耗
        ///// </summary>
        ///// <param name="vbd"></param>
        ///// <returns></returns>
        //internal static IEnumerable<GameOutput> GetGameOutput(BaseDataView vbd)
        //{
        //    using (var cn = new MySqlConnection(sqlconnectionString))
        //    {
        //        cn.Open();

        //        StringBuilder str = new StringBuilder();

        //        str.Append(@"select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(ChipChange > 0, ChipChange, 0)) as ChipOutput from record.BG_UserMoneyRecord where `type` in (1,2,3, 5,20,12,13,16,9,37,39,45,46,48,51,44,59,60,61,62,63,67,70,71,74,79,80,81,82,83,84,85,86,88,90) and  CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
        //            select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(ChipChange > 0, 0, ChipChange)) as ChipConsume from record.BG_UserMoneyRecord where CreateTime between @StartDate and @ExpirationDate and `type` in (24,25,26,22,23,19,7,21,18,17,47,50,64,65,68,76) group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
        //            select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum(ChipChange) as ChipRecharge from record.BG_UserMoneyRecord where type = 5 and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');

        //            select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(DiamondChange > 0, DiamondChange, 0)) as DiamondOutput from record.BG_UserMoneyRecord where `type` in (40,6,38,45) and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
        //            select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum( if(DiamondChange > 0, 0, DiamondChange)) as DiamondConsume from record.BG_UserMoneyRecord where `type` in (8,4) and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
        //            select DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum(DiamondChange) as DiamondRecharge from record.BG_UserMoneyRecord where type = 6 and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');
        //            select sum(Gold + SafeBox) as systemBargainingChip, sum(Diamond) as systemDiamond, sum(Zicard) as systemScore from Role where IsFreeze = 0 and agent <> 10010;

        //            SELECT sum(if(Type = 2, 1, 0) + if(Type = 3, -1, 0)) as ChipOutput, FishID as DiamondOutput FROM FishInfoRecord group by FishID");

        //        IEnumerable<GameOutput> i;
        //        using (var multi = cn.QueryMultiple(str.ToString(), vbd))
        //        {
        //            var ChipOutput = multi.Read<GameOutput>().ToList();
        //            var ChipConsume = multi.Read<GameOutput>().ToList();
        //            var ChipRecharge = multi.Read<GameOutput>().ToList();
        //            var DiamondOutput = multi.Read<GameOutput>().ToList();
        //            var DiamondConsume = multi.Read<GameOutput>().ToList();
        //            var DiamondRecharge = multi.Read<GameOutput>().ToList();
        //            var systemSum = multi.Read<GameOutput>().ToList();
        //            var FishChip = multi.Read<GameOutput>().ToList();

        //            i = ChipOutput.Union(ChipConsume).Union(ChipRecharge).Union(DiamondOutput).Union(DiamondConsume).Union(DiamondRecharge).GroupBy(n => n.date).Select(g => new GameOutput
        //            {
        //                date = g.Select(s => s.date).First(),
        //                ChipOutput = g.Select(s => s.ChipOutput).Sum(),
        //                ChipConsume = g.Select(s => s.ChipConsume).Sum(),
        //                ChipRecharge = g.Select(s => s.ChipRecharge).Sum(),
        //                DiamondOutput = g.Select(s => s.DiamondOutput).Sum(),
        //                DiamondConsume = g.Select(s => s.DiamondConsume).Sum(),
        //                DiamondRecharge = g.Select(s => s.DiamondRecharge).Sum(),
        //                systemBargainingChip = systemSum.Select(x => x.systemBargainingChip).Sum(),
        //                systemDiamond = systemSum.Select(x => x.systemDiamond).Sum(),
        //                systemFishChip = FishChip.Select(x => x.DiamondOutput == 1 ? x.ChipOutput * 200000 : x.DiamondOutput == 2 ? x.ChipOutput * 1000000 : x.DiamondOutput == 3 ? x.ChipOutput * 2000000 : x.DiamondOutput == 4 ? x.ChipOutput * 5000000 : 0).Sum()
        //            }).OrderBy(x => x.date).ToList();

        //        }


        //        cn.Close();
        //        return i;
        //    }
        //}

        ///// <summary>
        ///// 产出消耗明细
        ///// </summary>
        ///// <param name="vbd"></param>
        ///// <returns></returns>
        //internal static GameOutputDetail GetGameOutputDetail(BaseDataView vbd)
        //{
        //    using (var cn = new MySqlConnection(sqlconnectionString))
        //    {
        //        cn.Open();

        //        StringBuilder str = new StringBuilder();

        //        str.Append(@"SELECT sum(ChipChange) as Chip, sum(DiamondChange) as Diamond, sum(ScoreChange) as Score, `type` as ChipChangeType from record.BG_UserMoneyRecord where `type` in (1,2,3,4,5,6,7,8,9,12,13,16,17,18,19,20,21,22,23,24,25,26,37,38,39,40,41,42,43,44,45,46,47,48,50,51,59,60,61,62,63,64,65,67,68,70,71,74,76,79,80,81,82,83,84,85,86,88,90) and CreateTime between @StartDate and date_add( @StartDate, interval 1 day) group by `Type`;
                
        //        select sum(Gold + SafeBox) as systemBargainingChip, sum(Diamond) as systemDiamond, sum(Zicard) as systemScore from Role where IsFreeze = 0 and agent <> 10010;");


        //        GameOutputDetail j = new GameOutputDetail();

        //        using (var multi = cn.QueryMultiple(str.ToString(), vbd))
        //        {
        //            var i = multi.Read<GameOutputList>().ToList();
        //            var systemSum = multi.Read<GameOutput>().ToList();

        //            j.date = Convert.ToDateTime(vbd.StartDate);
        //            j.systemBargainingChip = systemSum.Select(x => x.systemBargainingChip).Sum();
        //            j.systemDiamond = systemSum.Select(x => x.systemDiamond).Sum();
        //            j.systemScore = systemSum.Select(x => x.systemScore).Sum();
        //            j.list = i.ToList();


        //        }

        //        cn.Close();
        //        return j;
        //    }
        //}


    }
}