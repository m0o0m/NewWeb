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

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


        internal static IEnumerable<BaseDataInfo> GetGameProfit(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select sum(ProfitAdd1) ProfitAdd1 ,sum(ProfitDel1) ProfitDel1 ,sum(ProfitAdd2) ProfitAdd2 ,sum(ProfitDel2) ProfitDel2 ,
  sum(ProfitAdd3) ProfitAdd3 ,sum(ProfitDel3) ProfitDel3 ,sum(ProfitAdd4) ProfitAdd4 ,sum(ProfitDel4) ProfitDel4 ,sum(ProfitAdd5) ProfitAdd5 ,
  sum(ProfitDel5) ProfitDel5 
,sum(ProfitAdd6) ProfitAdd6 ,
  sum(ProfitDel6) ProfitDel6 
,sum(ProfitScale) ProfitScale ,sum(ProfitHorse) ProfitHorse ,sum(ProfitZodiac) ProfitZodiac ,sum(ProfitCar) ProfitCar 
  ,sum(ProfitHundred) ProfitHundred
from (
select ifnull(sum(Texas_LAward_W + Texas_MAward_W + Texas_HAward_W) ,0) ProfitAdd1 
    ,ifnull(sum(Texas_LAward_L + Texas_MAward_L + Texas_HAward_L) ,0) ProfitDel1
    ,ifnull(sum(Scale_Award_W) ,0) ProfitAdd2 ,ifnull(sum(Scale_Award_L) ,0) ProfitDel2
    ,ifnull(sum(Zodiac_Award_W) ,0) ProfitAdd3 ,ifnull(sum(Zodiac_Award_L) ,0) ProfitDel3
    ,ifnull(sum(Horse_Award_W) ,0) ProfitAdd4 ,ifnull(sum(Horse_Award_L) ,0) ProfitDel4
    ,ifnull(sum(Car_Award_W) ,0) ProfitAdd5 ,ifnull(sum(Car_Award_L) ,0) ProfitDel5
    ,ifnull(sum(Hundred_Award_W) ,0) ProfitAdd6 ,ifnull(sum(Hundred_Award_L) ,0) ProfitDel6 
    ,0 ProfitScale ,0 ProfitHorse ,0 ProfitZodiac ,0 ProfitCar ,0 ProfitHundred
from "+ database3+ @".Clearing_Game a
where a.CountDate >= @StartDate and a.CountDate < @ExpirationDate and a.userid = @SearchExt
union all
select 0,0,0,0,0,0,0,0,0,0,0,0,ifnull(sum(case when a.type in (31,32,33) then ChipChange else 0 end),0) ProfitScale 
  ,ifnull(sum(case when a.type in (93,94,95) then ChipChange else 0 end),0) ProfitHorse
  ,ifnull(sum(case when a.type in (103,104,105) then ChipChange else 0 end),0) ProfitZodiac 
  ,ifnull(sum(case when a.type in (115,116,117) then ChipChange else 0 end),0) ProfitCar
  ,ifnull(sum(case when a.type in (158,159,160) then ChipChange else 0 end),0) ProfitHundred 
from "+ database3 + @".BG_UserMoneyRecord a
where a.CreateTime >= @StartDate and a.CreateTime < @ExpirationDate and a.userid = @SearchExt 
  and a.type in (31,32,33,93,94,95,103,104,105,115,116,117,158,159,160)
)t
                ;");

                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 玩家个人产出消耗
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        internal static GameOutputDetail GetGameOutputDetailUser(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"SELECT sum(ChipChange) as Chip, case Type when 128 then 127 when 140 then 139 else Type end as ChipChangeType 
                  from "+ database3 + @".BG_UserMoneyRecord
                  where Type in (select Type from record.S_Desc where Type_Id in(1,2) and `Type` not in (3,5,26,65,92,114,50,76,99,121)) and CreateTime >= @StartDate and CreateTime < @ExpirationDate and userid = @SearchExt 
                  group by case Type when 128 then 127  when 140 then 139 else Type end
                  order by Chip desc;

                  SELECT sum(case when ChipChange > 0 then ChipChange else 0 end) as ChipAdd, sum(case when ChipChange < 0 then ChipChange else 0 end) ChipDel
                  from "+ database3 + @".BG_UserMoneyRecord
                  where Type in (select Type from "+ database3 + @".S_Desc where Type_Id in(1,2) and `Type` not in (3,5,26,65,92,114,50,76,99,121)) and CreateTime >= @StartDate and CreateTime < @ExpirationDate and userid = @SearchExt ");

                //str.AppendFormat(@"SELECT sum(Chip) as Chip, sum(Diamond) as Diamond, sum(Score) as Score, `RecordType` as ChipChangeType from record.Clearing_UserMoneyRecord where `RecordType` in (select Type from record.S_Desc where Type_Id = 1 or Type_Id = 2) and RecordTime = @StartDate {0} group by `RecordType`;
                //  select systemChip as systemBargainingChip, systemDiamond, systemScore, systemFish as systemFishChip from record.Clearing_UserMoneyStock where RecordTime = @StartDate {0}", vbd.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");

                GameOutputDetail j = new GameOutputDetail();

                using (var multi = cn.QueryMultiple(str.ToString(), vbd, null, null))
                {
                    var i = multi.Read<GameOutputList>().ToList();
                    var systemSum = multi.Read<GameOutput>().ToList();

                    j.date = Convert.ToDateTime(vbd.StartDate);
                    j.ChipAdd = systemSum.Select(x => x.ChipAdd).Sum();
                    j.ChipDel = systemSum.Select(x => x.ChipDel).Sum();
                    j.list = i.ToList();
                }

                cn.Close();
                return j;
            }
        }

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

                str.AppendFormat(@"
                    select hour, sum(RegisterNum) as count from(
                    select CountDate, hour(CountDate) as hour, RegNum as RegisterNum
                    FROM "+ database3 + @".Clearing_Reg
                    where CountDate between @StartDate and @ExpirationDate and CountDate != @ExpirationDate {0}
                    ) a group by a.`hour` ", bdv.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");

                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<Roulette> GetRouletteData(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"select date_add(@StartDate ,interval s.id day) Date, ifnull(sum(abs(CountNum)) / 2 ,0) CountSum
                    , ifnull(sum(case TypeID when 101 then CountNum else 0 end) ,0) CountKey
                    , ifnull(sum(case TypeID when 102 then SumValue else 0 end) ,0) GoldConsume
                    , ifnull(sum(case when TypeID <= 4 then SumValue else 0 end) ,0) GoldIncome
                    , ifnull(sum(case when (TypeID = 5 or TypeID = 6 or TypeID = 7) then SumValue else 0 end) ,0) DHQIncome
                    , ifnull(sum(case when (TypeID = 8 or TypeID = 9 or TypeID = 10) then SumValue else 0 end) ,0) DiamondIncome
                    from "+ database3 + @".S_Ordinal s left join "+ database3 + @".Clearing_Roulette a on a.CountDate = date_add(@StartDate ,interval s.id day) and a.CountDate between @StartDate and @ExpirationDate and a.Agent != 10010 {0}
                    where s.id < datediff(@ExpirationDate ,@StartDate) group by date_add(@StartDate ,interval s.id day) order by Date desc", bdv.Channels == 0 ? "" : "and a.Agent = @Channels");
                IEnumerable<Roulette> i = cn.Query<Roulette>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<Roulette> GetRouletteShop(BaseDataView bdv)
        {
             using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"select a.CreateTime AS Date ,a.UserID ,case a.ItemID when 1  then 'iphone6s plus(64g)' when 2  then 'LG 滚筒洗衣机' when 3  then '美图M4 美颜手机' when 4  then '苏泊尔电饭煲'
                        when 5  then '100元Q币' when 6  then '小米移动电源' when 7  then '50元Q币' when 8  then '10元Q币' when 9  then '蓝色花海*1' when 10 then '香吻*4' when 11 then '游戏币*60000'
                        when 12 then '五币*30'end ItemName ,a.ItemNum ,a.ItemValue ,a.IsGet ,a.TrueName ,a.Tel ,a.Post ,a.Address ,a.QQNum ,a.ID
                    from "+ database3 + @".BG_ExchangeRecord a 
                    where a.CreateTime between @StartDate and @ExpirationDate and a.userid = case @UserID when 0 then a.userid else @UserID end 
                    order by a.CreateTime desc");
                IEnumerable<Roulette> i = cn.Query<Roulette>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }

        internal static int GetRouletteShop(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                string str = "update "+ database3 + @".BG_ExchangeRecord set IsGet = 1 where id =" + id.ToString() + ";";
                int i = cn.Execute(str);
                cn.Close();
                return i;
            }
        }


        /// <summary>
        /// 注册用户 渠道
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<BaseDataInfo> GetRegisteredUsersByChannel(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
                    select a.Agent ChannelID ,b.AgentName ChannelName, sum(RegNum) as count
                    FROM "+ database3 + @".Clearing_Reg a join "+ database2+ @".AgentUsers b on a.Agent = b.Id
                    where CountDate >= @StartDate and CountDate < @ExpirationDate {0}
                    group by a.Agent ,b.AgentName order by count desc ", bdv.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");

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

                str.Append("SELECT DATE_FORMAT( CountDate, '%Y-%m-%d') as date, sum(RegNum) as count FROM "+ database3+ @".Clearing_Reg where CountDate between @StartDate and @ExpirationDate  ");

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



        internal static IEnumerable<CommonIDName> GetModelByBoard(string brand)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append(@"
           select ID,`Name` from "+ database1+ @".MobileModel where BrandName = '"+brand+@"'         
                    
                    ");

           

                IEnumerable<CommonIDName> i = cn.Query<CommonIDName>(str.ToString());

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

                //str.Append("SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, sum(ActiveNum) as count FROM record.Clearing_RoleStatic where CreateTime between @StartDate and @ExpirationDate");

                //if (bdv.Channels > 0)
                //{
                //    str.AppendFormat(" and find_in_set(AgentID, '{0}')", bdv.UserList);
                //}

                //switch (bdv.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );");
                //        break;
                //    case groupby.按月:
                //        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m' );");
                //        break;
                //}


                str.Append("SELECT DATE_FORMAT( CountDate, '%Y-%m-%d') as date, sum(SumValue) as count FROM "+ database3+ @".Clearing_Day where CountDate between @StartDate and @ExpirationDate and TypeId=103 and agent <> 10010 ");

                if (bdv.Channels > 0)
                {
                    str.AppendFormat(" and find_in_set(Agent, '{0}')", bdv.UserList);
                }

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" GROUP BY DATE_FORMAT( CountDate, '%Y-%m-%d' );");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CountDate, '%Y-%m' );");
                        break;
                    default://按周
                        str.Append(" GROUP BY DATE_FORMAT( CountDate, '%x-%v' );");
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
                str.Append("SELECT CreateTime as date, sum(Online) as online, sum(Playing) as playing FROM "+ database1 + @".BG_OnlinePlaying where CreateTime >= date(@StartDate) and CreateTime < date_add(date(@StartDate), interval 1 day) and AgentID<>10010");
                if (bdv.Channels > 0)
                {
                    str.AppendFormat(" and find_in_set(AgentID, '{0}')", bdv.UserList);
                }
                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append("");
                        str.Append(" group by CreateTime ");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );");
                        break;
                    default:
                        str.Append("");
                        str.Append(" group by CreateTime ");
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
                    str.Append("SELECT count(0) FROM V_Role where Agent!=10010;");
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
                        from "+ database3+ @".Clearing_UserKeep
                        where CountDate between @StartDate and @ExpirationDate {0} and UserType = {1}
                        group by CountDate order by date desc;", bdv.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "" ,bdv.TypeID);

                IEnumerable<BaseDataInfoForRetentionRates> i = cn.Query<BaseDataInfoForRetentionRates>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 牌局留存率
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<GameKeep> GetGameRetentionRates(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
select date_add(@StartDate ,interval o.id day) CountDate ,RG ,GC0 ,GC1 ,GC2 ,GC3 ,GC4 ,GC5 ,GC6 ,GC11,GC16,GC21,GC26,GC31,GC36,GC41,GC46,GC51,RU0 ,RU1 ,RU2 
  ,RU3 ,RU4 ,RU5 ,RU6 ,RU11,RU16,RU21,RU26,RU31,RU36,RU41,RU46,RU51,KU0 ,KU1 ,KU2 ,KU3 ,KU4 ,KU5 ,KU6 ,KU11,KU16,KU21,KU26,KU31,KU36,KU41,KU46,KU51 
from "+ database3+ @".S_Ordinal o
left join (
select CountDate ,sum(case GameCount when 0 then 1 else 0 end) GC0,sum(case GameCount when 0 then RegUsers else 0 end) RU0 ,sum(case GameCount when 0 then KeepUsers else 0 end) KU0 
,sum(case GameCount when 1 then 1 else 0 end) GC1,sum(case GameCount when 1 then RegUsers else 0 end) RU1 ,sum(case GameCount when 1 then KeepUsers else 0 end) KU1 
,sum(case GameCount when 2 then 1 else 0 end) GC2,sum(case GameCount when 2 then RegUsers else 0 end) RU2 ,sum(case GameCount when 2 then KeepUsers else 0 end) KU2 
,sum(case GameCount when 3 then 1 else 0 end) GC3,sum(case GameCount when 3 then RegUsers else 0 end) RU3 ,sum(case GameCount when 3 then KeepUsers else 0 end) KU3 
,sum(case GameCount when 4 then 1 else 0 end) GC4,sum(case GameCount when 4 then RegUsers else 0 end) RU4 ,sum(case GameCount when 4 then KeepUsers else 0 end) KU4 
,sum(case GameCount when 5 then 1 else 0 end) GC5,sum(case GameCount when 5 then RegUsers else 0 end) RU5 ,sum(case GameCount when 5 then KeepUsers else 0 end) KU5 
,sum(case when GameCount>5 and GameCount<11 then 1 else 0 end) GC6,sum(case when GameCount>5 and GameCount<11 then RegUsers else 0 end) RU6 ,sum(case when GameCount>5 and GameCount<11 then KeepUsers else 0 end) KU6 
,sum(case when GameCount>10 and GameCount<16 then 1 else 0 end) GC11,sum(case when GameCount>10 and GameCount<16 then RegUsers else 0 end) RU11 ,sum(case when GameCount>10 and GameCount<16 then KeepUsers else 0 end) KU11
,sum(case when GameCount>15 and GameCount<21 then 1 else 0 end) GC16,sum(case when GameCount>15 and GameCount<21 then RegUsers else 0 end) RU16 ,sum(case when GameCount>15 and GameCount<21 then KeepUsers else 0 end) KU16
,sum(case when GameCount>20 and GameCount<26 then 1 else 0 end) GC21,sum(case when GameCount>20 and GameCount<26 then RegUsers else 0 end) RU21 ,sum(case when GameCount>20 and GameCount<26 then KeepUsers else 0 end) KU21
,sum(case when GameCount>25 and GameCount<31 then 1 else 0 end) GC26,sum(case when GameCount>25 and GameCount<31 then RegUsers else 0 end) RU26 ,sum(case when GameCount>25 and GameCount<31 then KeepUsers else 0 end) KU26
,sum(case when GameCount>30 and GameCount<36 then 1 else 0 end) GC31,sum(case when GameCount>30 and GameCount<36 then RegUsers else 0 end) RU31 ,sum(case when GameCount>30 and GameCount<36 then KeepUsers else 0 end) KU31
,sum(case when GameCount>35 and GameCount<41 then 1 else 0 end) GC36,sum(case when GameCount>35 and GameCount<41 then RegUsers else 0 end) RU36 ,sum(case when GameCount>35 and GameCount<41 then KeepUsers else 0 end) KU36
,sum(case when GameCount>40 and GameCount<46 then 1 else 0 end) GC41,sum(case when GameCount>40 and GameCount<46 then RegUsers else 0 end) RU41 ,sum(case when GameCount>40 and GameCount<46 then KeepUsers else 0 end) KU41
,sum(case when GameCount>45 and GameCount<51 then 1 else 0 end) GC46,sum(case when GameCount>45 and GameCount<51 then RegUsers else 0 end) RU46 ,sum(case when GameCount>45 and GameCount<51 then KeepUsers else 0 end) KU46
,sum(case when GameCount>50 then GameCount else 0 end) GC51,sum(case when GameCount>50 then RegUsers else 0 end) RU51 ,sum(case when GameCount>50 then KeepUsers else 0 end) KU51 
from "+ database3+ @".Clearing_GameKeep a 
where a.CountDate >= @StartDate and a.CountDate < @ExpirationDate {0} group by CountDate 
) a on date_add(@StartDate ,interval o.id day) = a.CountDate 
left join (
select date(CountDate) CountDate ,sum(RegNum) RG from "+ database1 + @".Clearing_Reg 
where CountDate >= @StartDate and CountDate < @ExpirationDate {0} group by date(CountDate) 
)b on date_add(@StartDate ,interval o.id day) = b.CountDate 
where o.id < datediff(@ExpirationDate ,@StartDate) order by 1 desc; 
                ", bdv.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");

                IEnumerable<GameKeep> i = cn.Query<GameKeep>(str.ToString(), bdv);
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

                str.Append(@""+ database3+ @".sys_get_ruin_rate");

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

                str.Append(@""+ database3 + @".sys_get_charge_rate");

                //call sys_get_charge_rate('2015-11-01', '2015-11-10');
                IEnumerable<QQZoneRechargeCount> i = cn.Query<QQZoneRechargeCount>(str.ToString(), param: new { begin_date = vbd.StartDate, end_date = vbd.ExpirationDate, agent_id = vbd.Channels > 0 ? vbd.UserList : "0" }, commandType: CommandType.StoredProcedure);

                


                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 注册人数，预留
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeRegisterDetail(BaseDataView vbd) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"

select * from "+ database1+ @".Role where CreateTime  between @StartDate and @ExpirationDate and Agent=if(@Channels=0,Agent,@Channels) ;"


                      );

                IEnumerable<QQZoneRechargeCountDetail> i = cn.Query<QQZoneRechargeCountDetail>(str.ToString(), vbd);
                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 首冲详细信息
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeFirstChargeDetail(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
          select a.UserID ,b.nickname ,sum(a.money)/100 Total
from "+ database1 + @".QQZoneRecharge a 
  join "+ database1 + @".Role b on a.UserID = b.id and b.Agent = if(@Channels=0,b.Agent,@Channels)
where a.CreateTime >= @StartDate and a.CreateTime < @ExpirationDate and a.IsFirst = 1
group by a.UserID ,b.nickname  UNION all
 select aa.UserID ,bb.nickname ,sum(aa.money)/100 Total
from "+ database1 + @".QQZoneRechargeFisrst aa 
  join "+ database1 + @".Role bb on aa.UserID = bb.id and bb.Agent = if(@Channels=0,bb.Agent,@Channels)
where aa.CreateTime >= @StartDate and aa.CreateTime < @ExpirationDate and aa.IsFirst = 1
group by aa.UserID ,bb.nickname;      "
                      );

                IEnumerable<QQZoneRechargeCountDetail> i = cn.Query<QQZoneRechargeCountDetail>(str.ToString(), vbd);
                cn.Close();
                return i;
            }

           
         }
        /// <summary>
        /// 再次付费人数
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeReChargeDetail(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"

create temporary table if not exists c_user(userid int primary key,countdate datetime);
delete from c_user;

insert into c_user (userid ,countdate)
select userid ,min(createtime) from "+ database1 + @".QQZoneRecharge group by userid;    
select a.UserID ,b.nickname ,sum(a.money)/100 Total
from "+ database1 + @".QQZoneRecharge a 
  join "+ database1 + @".Role b on a.UserID = b.id and b.Agent = if(@Channels=0,b.Agent,@Channels)
  join c_user c on a.userid = c.userid and c.countdate < date(a.CreateTime) 
where a.CreateTime >= @StartDate and a.CreateTime <@ExpirationDate
group by a.UserID ,b.nickname;
  "


                      );

                IEnumerable<QQZoneRechargeCountDetail> i = cn.Query<QQZoneRechargeCountDetail>(str.ToString(), vbd);
                cn.Close();
                return i;
            }


        }
        /// <summary>
        /// 当日注册且充值玩家
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeCurReChaDetail(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                //str.AppendFormat(@"
                //        select a.UserID ,b.nickname ,sum(a.money)/100 Total
                //        from gamedata.QQZoneRecharge a 
                //            join gamedata.Role b on a.UserID = b.id and b.Agent = if(@Channels=0,b.Agent,@Channels) and date(b.CreateTime) = date(a.CreateTime)
                //        where a.CreateTime >= @StartDate and a.CreateTime <@ExpirationDate
                //        group by a.UserID ,b.nickname;"
                //);
                str.AppendFormat(@"
select a.UserID,a.nickname,sum(a.Total) as Total from (
select a.UserID ,b.nickname ,sum(a.money)/100 Total
from "+ database1 + @".QQZoneRechargeFisrst a 
join "+ database1 + @".Role b on a.UserID = b.id and b.Agent = if(@Channels=0,b.Agent,@Channels) and date(b.CreateTime) = date(a.CreateTime)
where a.CreateTime >= @StartDate and a.CreateTime <@ExpirationDate
group by a.UserID ,b.nickname 
UNION all
select aa.UserID ,bb.nickname ,sum(aa.money)/100 Total
from "+ database1 + @".QQZoneRecharge aa 
join "+ database1 + @".Role bb on aa.UserID = bb.id and bb.Agent = if(@Channels=0,bb.Agent,@Channels) and date(bb.CreateTime) = date(aa.CreateTime)
where aa.CreateTime >= @StartDate and aa.CreateTime <@ExpirationDate
group by aa.UserID ,bb.nickname
) a
GROUP BY a.UserID,a.nickname
");


                IEnumerable<QQZoneRechargeCountDetail> i = cn.Query<QQZoneRechargeCountDetail>(str.ToString(), vbd);
                cn.Close();
                return i;
            }


        }

        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeAllReChaDetail(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
                        select a.UserID ,b.nickname ,sum(a.money)/100 Total
                        from "+ database1 + @".QQZoneRecharge a 
                            join "+ database1 + @".Role b on a.UserID = b.id and b.Agent = if(@Channels=0,b.Agent,@Channels) /*and date(b.CreateTime) = date(a.CreateTime)*/
                        where a.CreateTime >= @StartDate and a.CreateTime <@ExpirationDate
                        group by a.UserID ,b.nickname;"
                );

                IEnumerable<QQZoneRechargeCountDetail> i = cn.Query<QQZoneRechargeCountDetail>(str.ToString(), vbd);
                cn.Close();
                return i;
            }


        }

        public static GameOutAccurate GetGameOutAccurateFirst()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
select NOW() as CurTime,sum(systemChip) as Gold1 ,sum(systemFish) as Gold2 ,sum(ChipGame) as Gold3,(
select SUM(External) from "+ database1 + @".GamePotData 
) as Gold4
  from(
    select sum(Gold+SafeBox) systemChip,0 systemFish ,0 ChipGame 
    from "+ database1 + @".Role a 
    where IsFreeze = 0  
  union all
    SELECT 0 ,sum(case a.Type when 3 then (FishPrice * -1) else b.FishPrice end) ,0 
    FROM "+ database1 + @".FishInfoRecord a
      join "+ database3+ @".S_FishInfo b on a.FishID = b.FishID 
      join "+ database1 + @".Role c on a.UserID = c.id and IsFreeze = 0 
    where a.Type in (1,3,2) and a.Flag = 1
  union all 
    SELECT 0 ,0 ,sum(GoldGame) 
    FROM "+ database1 + @".RoleGame a
      join "+ database1 + @".Role c on a.UserID = c.id and IsFreeze = 0 
  )t


                  "
                );

                IEnumerable<GameOutAccurate> i = cn.Query<GameOutAccurate>(str.ToString());
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        public static GameOutAccurate GetGameOutAccurate(string bTime,string eTime)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
select 
sum(case b.Type_Id when 1 then t.gold else 0 end) as OutPut ,
sum(case b.Type_Id when 2 then t.gold else 0 end) as InPut 
from (
SELECT a.type,SUM(ChipChange) as gold,UserID from "+ database3+ @".BG_UserMoneyRecord  a
 join "+ database1 + @".Role c on c.id = a.UserID and IsFreeze = 0 
where a.CreateTime>'"+ bTime +@"' and a.CreateTime<='"+ eTime + @"'
group by a.type
union ALL
select a.type,SUM(ChipChange),0 from  "+ database3+ @".BG_UserMoneyRecord a 
where  a.CreateTime>'"+ bTime + @"' and a.CreateTime<='"+ eTime + @"' and a.UserID = 0
group by a.type
) t  join "+ database3 + @".S_Desc b on t.type = b.Type 
                  "
                );

                IEnumerable<GameOutAccurate> i = cn.Query<GameOutAccurate>(str.ToString());
                cn.Close();
                return i.FirstOrDefault();
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
                str.AppendFormat(@"
   select date(RecordTime) as date
                  ,sum(case b.Type_Id when 1 then a.Chip else 0 end) as ChipOutput 
                  ,sum(case b.Type_Id when 2 then a.Chip else 0 end) as ChipConsume 
                  ,sum(case a.RecordType when 5 then a.Chip else 0 end) as ChipRecharge
                  ,sum(case when a.Diamond > 0 then a.Diamond else 0 end) as DiamondOutput
                  ,sum(case when a.Diamond < 0 then a.Diamond else 0 end) as DiamondConsume
                  ,sum(case a.RecordType when 6 then a.Diamond else 0 end) as DiamondRecharge 
                from "+ database3 + @".Clearing_UserMoneyRecord a
                  join "+ database3 + @".S_Desc b on a.RecordType = b.Type
                where RecordTime >= @StartDate and RecordTime < @ExpirationDate and Agent!=10010  {0}
                group by date(RecordTime);

 select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(systemChip) as systemBargainingChip, sum(systemDiamond) as systemDiamond, systemScore, sum(systemFish) as systemFishChip ,sum(ChipGame) ChipGame
from "+ database3 + @".Clearing_UserMoneyStock where RecordTime >= @StartDate and RecordTime < @ExpirationDate and Agent!=10010 {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');

select date(CountDate) as date ,
sum(case TypeID when 100 then DayValue else 0 end) chip007 ,
sum(case TypeID when 101 then DayValue else 0 end) chipPot ,
sum(case TypeID when 11 then DayValue else 0 end) ChipKuCun 
from "+ database3 + @".Clearing_DayValue a where CountDate >= @StartDate and CountDate < @ExpirationDate and TypeID in (100,101,11)
group by date(CountDate); 
", vbd.Channels > 0 ? "and find_in_set(Agent, @UserList)" : ""


                      );//ChipKuCun
                IEnumerable<GameOutput> i;
                using (var multi = cn.QueryMultiple(str.ToString(), vbd))
                {
                    var ChipOutput = multi.Read<GameOutput>().ToList();
                    //var ChipConsume = multi.Read<GameOutput>().ToList();
                    //var ChipRecharge = multi.Read<GameOutput>().ToList();
                    //var DiamondOutput = multi.Read<GameOutput>().ToList();
                    //var DiamondConsume = multi.Read<GameOutput>().ToList();
                    //var DiamondRecharge = multi.Read<GameOutput>().ToList();
                    var systemSum = multi.Read<GameOutput>().ToList();
                    var ChipOther = multi.Read<GameOutput>().ToList();

                    i = ChipOutput.Union(systemSum).GroupBy(n => n.date).Select(g => new GameOutput
                    {
                        date = g.Select(s => s.date).First(),
                        ChipOutput = g.Select(s => s.ChipOutput).Sum(), //产出
                        ChipConsume = g.Select(s => s.ChipConsume).Sum(), //消耗
                        ChipRecharge = g.Select(s => s.ChipRecharge).Sum(),
                        DiamondOutput = g.Select(s => s.DiamondOutput).Sum(),
                        DiamondConsume = g.Select(s => s.DiamondConsume).Sum(),
                        DiamondRecharge = g.Select(s => s.DiamondRecharge).Sum(),
                        systemBargainingChip = g.Select(s => s.systemBargainingChip).Sum(),   //身上+保险箱钱
                        systemDiamond = g.Select(s => s.systemDiamond).Sum(),
                        systemFishChip = g.Select(s => s.systemFishChip).Sum(), //水族馆鱼
                        ChipGame = g.Select(s => s.ChipGame).Sum()  //游戏携带
                    }).OrderBy(x => x.date).ToList();

                    i = i.Union(ChipOther).GroupBy(n => n.date).Select(g => new GameOutput
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
                        systemFishChip = g.Select(s => s.systemFishChip).Sum(),
                        ChipGame = g.Select(s => s.ChipGame).Sum(),
                        chip007 = g.Select(s => s.chip007).Sum(),
                        chipPot = g.Select(s => s.chipPot).Sum(), //外部彩池
                         ChipKuCun = g.Select(s => s.ChipKuCun).Sum()
                    }).OrderBy(x => x.date).ToList();

                }


                cn.Close();
                return i;




                //str.AppendFormat(@"select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipOutput from record.Clearing_UserMoneyRecord where `RecordType` in (select Type from record.S_Desc where Type_Id = 1 ) and RecordTime between @StartDate and @ExpirationDate and Agent!=10010 {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                //select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipConsume from record.Clearing_UserMoneyRecord where RecordTime between @StartDate and @ExpirationDate and `RecordType` in (select Type from record.S_Desc where Type_Id = 2) and Agent!=10010 {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                //select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Chip) as ChipRecharge from record.Clearing_UserMoneyRecord where RecordType = 5 and RecordTime between @StartDate and @ExpirationDate and Agent!=10010 {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                //select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondOutput from record.Clearing_UserMoneyRecord where Diamond > 0 and RecordTime between @StartDate and @ExpirationDate and Agent!=10010 {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                //select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondConsume from record.Clearing_UserMoneyRecord where Diamond < 0 and RecordTime between @StartDate and @ExpirationDate and Agent!=10010 {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                //select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(Diamond) as DiamondRecharge from record.Clearing_UserMoneyRecord where RecordType = 6 and RecordTime between @StartDate and @ExpirationDate and Agent!=10010 {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
 //select DATE_FORMAT(RecordTime, '%Y-%m-%d') as date, sum(systemChip) as systemBargainingChip, sum(systemDiamond) as systemDiamond, systemScore, sum(systemFish) as systemFishChip from record.Clearing_UserMoneyStock where RecordTime between @StartDate and @ExpirationDate and Agent!=10010 {0} group by DATE_FORMAT(RecordTime, '%Y-%m-%d');
                //   ", vbd.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");


                //IEnumerable<GameOutput> i;
                //using (var multi = cn.QueryMultiple(str.ToString(), vbd))
                //{
                //    var ChipOutput = multi.Read<GameOutput>().ToList();
                //    var ChipConsume = multi.Read<GameOutput>().ToList();
                //    var ChipRecharge = multi.Read<GameOutput>().ToList();
                //    var DiamondOutput = multi.Read<GameOutput>().ToList();
                //    var DiamondConsume = multi.Read<GameOutput>().ToList();
                //    var DiamondRecharge = multi.Read<GameOutput>().ToList();
                //    var systemSum = multi.Read<GameOutput>().ToList();
                //    //var FishChip = multi.Read<GameOutput>().ToList();

                //    i = ChipOutput.Union(ChipConsume).Union(ChipRecharge).Union(DiamondOutput).Union(DiamondConsume).Union(DiamondRecharge).Union(systemSum).GroupBy(n => n.date).Select(g => new GameOutput
                //    {
                //        date = g.Select(s => s.date).First(),
                //        ChipOutput = g.Select(s => s.ChipOutput).Sum(),
                //        ChipConsume = g.Select(s => s.ChipConsume).Sum(),
                //        ChipRecharge = g.Select(s => s.ChipRecharge).Sum(),
                //        DiamondOutput = g.Select(s => s.DiamondOutput).Sum(),
                //        DiamondConsume = g.Select(s => s.DiamondConsume).Sum(),
                //        DiamondRecharge = g.Select(s => s.DiamondRecharge).Sum(),
                //        systemBargainingChip = g.Select(s => s.systemBargainingChip).Sum(),
                //        systemDiamond = g.Select(s => s.systemDiamond).Sum(),
                //        systemFishChip = g.Select(s => s.systemFishChip).Sum()
                //    }).OrderBy(x => x.date).ToList();

                //}


                //cn.Close();
                //return i;
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

                str.AppendFormat(@"SELECT sum(Chip) as Chip, sum(Diamond) as Diamond, sum(Score) as Score, case RecordType 
when 151 then 37 
when 128 then 127 
when 140 then 139 
when 146 then 61  
when 147 then 61  
when 204 then 999 
when 220 then 998 
else RecordType end as ChipChangeType 
                  from "+ database3 + @".Clearing_UserMoneyRecord
                  where RecordType in (select Type from "+ database3 + @".S_Desc where Type_Id >0) and RecordTime = @StartDate and Agent!=10010 {0} 
                  group by case RecordType 
when 151 then 37
when 128 then 127 
when 140 then 139
when 146 then 61 
when 147 then 61 
when 204 then 999 
when 220 then 998 
else RecordType end
                  order by Chip desc;
                  select systemChip as systemBargainingChip, systemDiamond, systemScore, systemFish as systemFishChip 
                  from " + database3 + @".Clearing_UserMoneyStock where RecordTime = @StartDate and  Agent!=10010 {0}", vbd.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");
                
                //str.AppendFormat(@"SELECT sum(Chip) as Chip, sum(Diamond) as Diamond, sum(Score) as Score, `RecordType` as ChipChangeType from record.Clearing_UserMoneyRecord where `RecordType` in (select Type from record.S_Desc where Type_Id = 1 or Type_Id = 2) and RecordTime = @StartDate {0} group by `RecordType`;
                //  select systemChip as systemBargainingChip, systemDiamond, systemScore, systemFish as systemFishChip from record.Clearing_UserMoneyStock where RecordTime = @StartDate {0}", vbd.Channels > 0 ? "and find_in_set(Agent, @UserList)" : "");

                GameOutputDetail j = new GameOutputDetail();

                using (var multi = cn.QueryMultiple(str.ToString(), vbd,null,null))
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



        internal static List<GameOutputRecursion>  GetGameOutputRecursion(BaseDataView vbd) {
            return null;
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
                str.AppendFormat(@"call "+ database3 + @".sys_get_gold_rate({0},{1});" ,bdv.Channels ,bdv.TypeID);
                //IEnumerable<BaseDataInfoForUsersGoldDistributionRatio> i = cn.Query<BaseDataInfoForUsersGoldDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure, param: new { agentids = bdv.Channels > 0 ? bdv.UserList : "0" });
                IEnumerable<BaseDataInfoForUsersGoldDistributionRatio> i = cn.Query<BaseDataInfoForUsersGoldDistributionRatio>(str.ToString());

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
                str.AppendFormat(@"call "+ database3 + @".sys_get_diamond_rate({0},{1});", bdv.Channels ,bdv.TypeID);
                //IEnumerable<BaseDataInfoForUsersDiamondDistributionRatio> i = cn.Query<BaseDataInfoForUsersDiamondDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure, param: new { agentids = bdv.Channels > 0 ? bdv.UserList : "0" });
                IEnumerable<BaseDataInfoForUsersDiamondDistributionRatio> i = cn.Query<BaseDataInfoForUsersDiamondDistributionRatio>(str.ToString());
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
                str.AppendFormat(@"call "+ database3 + @".sys_get_vip_rate({0},{1});", bdv.Channels ,bdv.TypeID);
                //IEnumerable<BaseDataInfoForVIPDistributionRatio> i = cn.Query<BaseDataInfoForVIPDistributionRatio>(str.ToString(), commandType: CommandType.StoredProcedure, param: new { agentids = bdv.Channels > 0 ? bdv.UserList : "0" });
                IEnumerable<BaseDataInfoForVIPDistributionRatio> i = cn.Query<BaseDataInfoForVIPDistributionRatio>(str.ToString());
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

        public static IEnumerable<CommonIDName> GetPhoneBoard()
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("SELECT * from MobileBrand");

                IEnumerable<CommonIDName> i = cn.Query<CommonIDName>(str.ToString());


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


                str.Append(@"select CreateTime as date, ChipChange as Chip from "+ database3 + @".BG_UserMoneyRecord where Type = 49 and CreateTime between @StartDate and @ExpirationDate");


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
                                                                                                                                                                                                        //select f.*,r.Agent from FinishTask f, Role r where r.ID = f.UserID
                                                                                                                                                                                                        //(grv.Channels > 0 ? " and find_in_set(Role.Agent, {5})" : "" )
            pq.RecordCount = DAL.PagedListDAL<BaseDataInfo>.GetRecordCount(string.Format(@" select count(0) from ( SELECT DATE_FORMAT(f.FinishTime, '%Y-%m-%d') as date, count(0) as count FROM FinishTask f,Role r where r.ID = f.UserID and f.FinishTime BETWEEN '{0}'  and '{1}' "+ (vbd.Channels > 0 ? " and find_in_set(r.Agent, {2})" : "") + " group by DATE_FORMAT(f.FinishTime, '%Y-%m-%d') ) a ", vbd.StartDate, vbd.ExpirationDate,vbd.UserList), sqlconnectionString);


                                                                                                                    //select f.*,r.Agent from FinishTask f, Role r where r.ID = f.UserID
                                                                                                                    //(grv.Channels > 0 ? " and find_in_set(Role.Agent, {5})" : "" )
            pq.Sql = string.Format(@"SELECT DATE_FORMAT(f.FinishTime, '%Y-%m-%d') as date, count(0) as count FROM FinishTask f,Role r where  r.ID = f.UserID  and f.FinishTime BETWEEN '{0}'  and '{1}' "+ (vbd.Channels > 0 ? " and find_in_set(r.Agent, {4})" : "") + " group by DATE_FORMAT(f.FinishTime, '%Y-%m-%d')  limit {2}, {3}", vbd.StartDate, vbd.ExpirationDate, pq.StartRowNumber, pq.PageSize,vbd.UserList);

            PagedList<BaseDataInfo> obj = new PagedList<BaseDataInfo>(PagedListDAL<BaseDataInfo>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }



        public static SignDraw GetSignDraw(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
           
                str.AppendFormat(@"select  IFNULL(sum(case TypeID when 0 then SignValue else 0 end),0) as SignDrawManPerDay,
        IFNULL(sum(case TypeID when 1 then SignValue else 0 end),0) as DiffColorMan,
        IFNULL(sum(case TypeID when 2 then SignValue else 0 end),0) as Club,
        IFNULL(sum(case TypeID when 3 then SignValue else 0 end),0) as Block,
        IFNULL(sum(case TypeID when 4 then SignValue else 0 end),0) as Hearts,
        IFNULL(sum(case TypeID when 5 then SignValue else 0 end),0) as Spade,
        IFNULL(sum(case TypeID when 8 then SignValue else 0 end),0) as Three,
        IFNULL(sum(case TypeID when 6 then SignValue else 0 end),0) as Five,
        IFNULL(sum(case TypeID when 7 then SignValue else 0 end),0) as Seven,
        IFNULL(sum(case TypeID when 9 then SignValue else 0 end),0) as LoginCoinPerDay,
        IFNULL(sum(case TypeID when 10 then SignValue else 0 end),0) as ConLoginCoinPerDay
from "+ database3 + @".Clearing_SignReward  a
    where a.CountDate between @StartDate and @ExpirationDate  ");

                if (vbd.Channels > 0)
                {
                    str.AppendFormat(" and find_in_set(a.agent, '{0}')", vbd.UserList);
                }
                else {
                    str.AppendFormat(" and  a.agent!=10010  ");
                }




                IEnumerable<SignDraw> i = cn.Query<SignDraw>(str.ToString(), vbd);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        public static PagedList<BaseDataInfo> GetUsreProfit(int page, BaseDataView vbd)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;

            if(vbd.SearchExt == "")
            {

                pq.RecordCount = DAL.PagedListDAL<BaseDataInfo>.GetRecordCount(string.Format(@"
                select ifnull(case when Profit_add>Profit_del then Profit_add else Profit_del end ,0) from (
                    select sum(case when Profit>0 then 1 else 0 end) Profit_add,sum(case when Profit<0 then 1 else 0 end) Profit_del from (
                        select sum(q.Profit) Profit 
                        from "+ database3 + @".Clearing_User q join "+ database1+ @".Role r on q.userid = r.id and r.Agent = case {2} when 0 then r.Agent else {2} end and r.agent <> 10010 
                        where q.CountDate between '{0}' and '{1}' group by q.userid) t where Profit <> 0 )tt",
               vbd.StartDate, vbd.ExpirationDate, vbd.Channels), sqlconnectionString);
                pq.Sql = string.Format(@"call "+ database3 + @".sys_get_user_profit('{0}' ,'{1}' ,{2} ,{3} ,{4});",
                            vbd.StartDate, vbd.ExpirationDate, vbd.Channels, pq.StartRowNumber, pq.PageSize);
            }
            else
            {
                pq.RecordCount = 1;
                pq.Sql = string.Format(@"select a.UserID as UserIDAdd ,b.Nickname as NicknameAdd ,sum(a.Profit) ProfitAdd
                            from "+ database3 + @".Clearing_User a 
                                join "+ database1+ @".Role b on a.UserID = b.ID and b.Agent = case {0} when 0 then b.Agent else {0} end 
                            where a.CountDate >= '{1}' and a.CountDate < '{2}' and b.id = {3} 
                            group by a.UserID ,b.Nickname;",
                            vbd.Channels, vbd.StartDate, vbd.ExpirationDate, vbd.SearchExt);
            }            

            PagedList<BaseDataInfo> obj = new PagedList<BaseDataInfo>(DAL.PagedListDAL<BaseDataInfo>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        /// <summary>
        /// 破产统计
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static IEnumerable<Ruin> GetRuinUsers(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                if(bdv.Channels == 0)
                {
                    str.AppendFormat(@"select date(a.createtime) CountDate, count(distinct a.userid) RuinUsers, count(1) RuinCount, sum(ChipChange) RuinSum
                        from "+ database3+ @".BG_UserMoneyRecord a
                        join "+ database1+ @".Role b on a.Userid = b.id and b.agent <> 10010
                        where a.createtime >= @StartDate and a.createtime < @ExpirationDate and a.type = 148
                        group by date(a.createtime);"
                    , bdv.StartDate, bdv.ExpirationDate);
                }
                else
                {
                    str.AppendFormat(@"select date(a.createtime) CountDate, count(distinct a.userid) RuinUsers, count(1) RuinCount, sum(ChipChange) RuinSum
                        from "+ database3+ @".BG_UserMoneyRecord a
                        join "+ database1+ @".Role b on a.Userid = b.id and b.agent <> 10010 and b.agent = @Channels 
                        where a.createtime >= @StartDate and a.createtime < @ExpirationDate and a.type = 148
                        group by date(a.createtime);"
                    , bdv.StartDate, bdv.ExpirationDate, bdv.Channels);
                }

                IEnumerable<Ruin> i = cn.Query<Ruin>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }



        internal static IEnumerable<VersionSum> GetVersionSum(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
            
                    str.AppendFormat(@"
select ifnull(VersionNo ,'') VersionNo ,ifnull(sum(DayAdd) ,0) AS DayAdd ,ifnull((case when sum(DayAdd) = 0 then 0 else sum(NewBoard)/sum(DayAdd) end) * 100 ,0) AS NewBoardRate
  ,ifnull(sum(DayActive) ,0) AS DayActive ,ifnull((case when sum(LoginUser) = 0 then 0 else sum(PayUser)/sum(LoginUser) end) * 100 ,0) AS PayST
  ,ifnull(sum(PayUser) ,0) AS PayUserCount ,ifnull((case when sum(PayUser) = 0 then 0 else sum(Money)/sum(PayUser)/100 end) ,0) AS Arppu 
  ,ifnull((case when sum(LoginUser) = 0 then 0 else sum(Money)/sum(LoginUser)/100 end) ,0) AS Arpu ,ifnull(sum(Money)/100 ,0) AS TotalPay 
from (
/*版本号 新增 新增玩牌 活跃 登陆人数 付费人数 付费钱*/
  select a.VersionInfo AS VersionNo,count(distinct a.ID) AS DayAdd ,count(distinct b.UserID) NewBoard ,0 DayActive ,0 LoginUser ,0 PayUser ,0 Money
  from "+ database1 + @".Role a 
    left join "+ database3+ @".Clearing_Game b on b.CountDate >= '{0}' and b.CountDate < date_add('{0}' ,interval 1 day) and a.ID = b.UserID 
  where a.CreateTime >= '{0}' and a.CreateTime < date_add('{0}' ,interval 1 day) and agent <> 10010 and isfreeze = 0
  group by a.VersionInfo
  union all 
  /*活跃 登陆*/
  select a.VersionInfo ,0 ,0 ,count(distinct case when b.CreateTime < '{0}' then a.UserID else null end) ,count(distinct a.UserID) ,0 ,0 
  from "+ database1 + @".BG_LoginRecord a 
      join "+ database1 + @".Role b on a.UserID = b.ID and b.agent <> 10010 and b.isfreeze = 0 
  where LoginTime >= '{0}' and LoginTime < date_add('{0}' ,interval 1 day) group by a.VersionInfo 
  union all
  /*付费人数 付费钱*/
  select a.VersionInfo ,0 ,0 ,0 ,0 ,count(distinct a.UserID) ,sum(a.Money) 
  from "+ database1 + @".QQZoneRecharge a 
      join "+ database1 + @".Role b on a.UserID = b.ID and b.agent <> 10010 and b.isfreeze = 0 
  where a.CreateTime >= '{0}' and a.CreateTime < date_add('{0}' ,interval 1 day) group by a.VersionInfo
)t group by ifnull(VersionNo ,'') having ifnull(sum(DayActive) ,0) > 10;

"
                    , bdv.StartDate);
              
                IEnumerable<VersionSum> i = cn.Query<VersionSum>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<DayReport> GetDayReportList(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                string chan = "";
                if (vbd.Channels > 0)
                {
                    chan =  string.Format(" and find_in_set(a.agent, '{0}')", vbd.UserList);
                }

                str.AppendFormat(@"
select date_add('{0}' ,interval o.id day) CreateTime ,ifnull(sum(DayAdd) ,0) DayAdd ,ifnull(sum(DAU) ,0) DAU 
  ,ifnull(sum(DayLoginCount) ,0) DayLoginCount ,ifnull(sum(CiRiRate) ,0) CiRiRate ,ifnull(sum(QiRiRate) ,0) QiRiRate 
  ,ifnull(sum(NewAddCount) ,0) NewAddCount ,ifnull(sum(NewAddSum) ,0) NewAddSum ,ifnull(sum(DayPayCount) ,0) DayPayCount 
  ,ifnull(sum(DayPaySum) ,0) DayPaySum ,ifnull(sum(PlayRate) ,0) PlayRate ,ifnull(sum(TotalCount) ,0) TotalCount 
  ,ifnull(sum(AvgPlay) ,0) AvgPlay ,ifnull(sum(AvgOnLine) ,0) AvgOnLine ,ifnull(sum(DaySend) ,0) DaySend 
  ,ifnull(sum(DayUser) ,0) DayUser ,ifnull(sum(SystemSave) ,0) SystemSave 
from "+ database3+ @".S_Ordinal o left join (
  select a.CountDate ,sum(case a.TypeID when 1 then SumValue else 0 end ) AS DayAdd 
    ,sum(case a.TypeID when 2 then SumValue else 0 end ) AS DAU 
    ,sum(case a.TypeID when 3 then SumValue else 0 end ) AS DayLoginCount 
    ,0 AS CiRiRate ,0 AS QiRiRate 
    ,sum(case a.TypeID when 101 then SumValue else 0 end ) AS NewAddCount 
    ,sum(case a.TypeID when 102 then SumValue else 0 end ) AS NewAddSum 
    ,sum(case a.TypeID when 103 then SumValue else 0 end ) AS DayPayCount 
    ,sum(case a.TypeID when 104 then SumValue else 0 end ) AS DayPaySum 
    ,sum(case a.TypeID when 5 then SumValue else 0 end ) AS PlayRate ,0 as TotalCount 
    ,sum(case a.TypeID when 6 then SumValue else 0 end ) AS AvgPlay 
    ,sum(case a.TypeID when 7 then SumValue else 0 end ) AS AvgOnLine 
    ,0 AS DaySend ,0 AS DayUser ,0 AS SystemSave 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (1,2,3,5,6,7,101,102,103,104) and a.CountDate >= '{0}' and a.CountDate < '{1}' {2}
  group by a.CountDate  union all 
  /*总牌局不用处理渠道*/
  select a.CountDate ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,sum(SumValue) ,0 ,0 ,0 ,0 ,0  from "+ database3 + @".Clearing_Report a 
  where a.CountDate >= '{0}' and a.CountDate < '{1}' and a.TypeID = 4
  group by a.CountDate union all 
  /*总存量*/
  select RecordTime ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,sum(systemChip + systemFish + ChipGame)
  from "+ database3 + @".Clearing_UserMoneyStock a where RecordTime >= '{0}' and RecordTime < '{1}'  {2} 
  group by RecordTime union all 
  /*总消耗*/
  select RecordTime ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,sum(Chip) ,0 
  from "+ database3 + @".Clearing_UserMoneyRecord a join record.S_Desc b on a.RecordType = b.Type_Id and b.Type = 2 
  where a.RecordTime >= '{0}' and a.RecordTime < '{1}' {2} 
  group by RecordTime union all 
  /*总发放*/
  select RecordTime ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,sum(Chip) ,0 ,0 
  from "+ database3 + @".Clearing_UserMoneyRecord a 
  where a.RecordTime >= '{0}' and a.RecordTime < '{1}'  and a.RecordType in (1,2) 
  group by RecordTime union all 
  select CountDate ,0 ,0 ,0 ,case when sum(RegNum) = 0 then 0 else sum(OneDay)/sum(RegNum) * 100 end 
    ,case when sum(RegNum) = 0 then 0 else sum(SevenDay)/sum(RegNum) * 100 end ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 
  from "+ database3 + @".Clearing_UserKeep a where CountDate >= '{0}' and CountDate < '{1}' and UserType = 2  {2}
  group by CountDate 

)b on CountDate = date_add('{0}' ,interval o.id day)
where o.id < datediff('{1}' ,'{0}') 
group by date_add('{0}' ,interval o.id day) order by 1 desc ;
", vbd.StartDate,vbd.ExpirationDate, chan
                );
                //and a.agent=1;
              
              

                IEnumerable<DayReport> i = cn.Query<DayReport>(str.ToString());
                cn.Close();
                return i;
            }
        }



        public static WeekReport GetWeekReport(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                string chan = "";
                if (vbd.Channels > 0)
                {
                    chan = string.Format(" and find_in_set(a.agent, '{0}')", vbd.UserList);
                }

                str.AppendFormat(@"
select a.WeekTime ,ifnull(sum(WeekAdd) ,0) AS WeekAdd ,ifnull(sum(WeekLogin) ,0) AS WeekLogin 
  ,ifnull(sum(WeekPayCount) ,0) AS WeekPayCount ,ifnull(sum(WeekPaySum) ,0) AS WeekPaySum 
from (select '{0}' AS WeekTime 
  union all select date_add('{0}' ,interval -7 day) 
  union all select date_add('{0}' ,interval -1 month)
) a left join (
  select a.CountDate AS WeekTime ,0 AS WeekAdd ,sum(case a.TypeID when 1003 then SumValue else 0 end) AS WeekLogin 
    ,sum(case a.TypeID when 1103 then SumValue else 0 end) AS WeekPayCount ,0 AS WeekPaySum 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (1103,1003) and (a.CountDate = '{0}' or a.CountDate = date_add('{0}' ,interval -7 day) 
  or a.CountDate = date_add('{0}' ,interval -1 month)) {1}
  group by a.CountDate 
  union all 
  select '{0}' ,sum(case a.TypeID when 1 then SumValue else 0 end) ,0 ,0 ,sum(case a.TypeID when 104 then SumValue else 0 end) 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (1,104) and a.CountDate < date_add('{0}' ,interval 1 day) and a.CountDate >= date_add('{0}' ,interval -6 day)  {1}
  union all 
  select date_add('{0}' ,interval -7 day) ,sum(case a.TypeID when 1 then SumValue else 0 end) ,0 ,0 
    ,sum(case a.TypeID when 104 then SumValue else 0 end) 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (1,104) and a.CountDate < date_add('{0}' ,interval -6 day) and a.CountDate >= date_add('{0}' ,interval -13 day)  {1}
  union all 
  select date_add('{0}' ,interval -1 month) ,sum(case a.TypeID when 1 then SumValue else 0 end) ,0 ,0 
    ,sum(case a.TypeID when 104 then SumValue else 0 end) 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (1,104) and a.CountDate < date_add(date_add('{0}' ,interval -1 month) ,interval 1 day) and a.CountDate >= date_add(date_add('{0}' ,interval -1 month) ,interval -6 day) {1}
)b on a.WeekTime = b.WeekTime group by a.WeekTime order by WeekTime asc; ",vbd.StartDate, chan);


                IEnumerable<WeekTempReport> i = cn.Query<WeekTempReport>(str.ToString());


                cn.Close();
                WeekReport wr = new WeekReport();
                if (i.Count() == 3)
                {
                     List<WeekTempReport> li = i.ToList();
                     var col1 = li[0];
                     var col2 = li[1];
                     var col3 = li[2];
                     wr.WeekAdd = col3.WeekAdd;
                    wr.WeekLogin = col3.WeekLogin;
                    wr.WeekPayCount = col3.WeekPayCount;
                    wr.WeekPaySum = col3.WeekPaySum;
                    wr.WeekTime = col3.WeekTime.AddDays(-6).ToString().Replace("0:00:00", "") + " : "+ col3.WeekTime.ToString().Replace("0:00:00", "");
                
                    wr.LwWeekAdd = col2.WeekAdd;
                    wr.LwWeekLogin = col2.WeekLogin;
                    wr.LwWeekPayCount = col2.WeekPayCount;
                    wr.LwWeekPaySum = col2.WeekPaySum;
                    wr.LwWeekTime = col2.WeekTime.AddDays(-6).ToString().Replace("0:00:00", "") + " : " + col2.WeekTime.ToString().Replace("0:00:00", "");


                    wr.LmWeekAdd = col1.WeekAdd;
                    wr.LmWeekLogin = col1.WeekLogin;
                    wr.LmWeekPayCount = col1.WeekPayCount;
                    wr.LmWeekPaySum = col1.WeekPaySum;
                    wr.LmWeekTime = col1.WeekTime.AddDays(-6).ToString().Replace("0:00:00", "") + " : " + col1.WeekTime.ToString().Replace("0:00:00", "");
                }
              
                return wr;
            }
        }


        public static MonthReport GetMonthReport(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                string chan = "";
                if (vbd.Channels > 0)
                {
                    chan = string.Format(" and find_in_set(a.agent, '{0}')", vbd.UserList);
                }

                str.AppendFormat(@"
select a.MonthTime ,ifnull(sum(MonthAdd) ,0) AS MonthAdd ,ifnull(sum(MonthLogin) ,0) AS MonthLogin 
  ,ifnull(sum(MonthPayCount) ,0) AS MonthPayCount ,ifnull(sum(MonthPaySum) ,0) AS MonthPaySum 
from (select '{0}' AS MonthTime 
  union all select date_add('{0}' ,interval -1 month) 
  union all select date_add('{0}' ,interval -1 year)
) a left join (
  select a.CountDate AS MonthTime ,0 AS MonthAdd ,sum(case a.TypeID when 10003 then SumValue else 0 end) AS MonthLogin 
    ,sum(case a.TypeID when 10103 then SumValue else 0 end) AS MonthPayCount ,0 AS MonthPaySum 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (10103,10003) and (a.CountDate = '{0}' or a.CountDate = date_add('{0}' ,interval -7 day) 
  or a.CountDate = date_add('{0}' ,interval -1 month)) {1}
  group by a.CountDate 
  union all 
  select '{0}' ,sum(case a.TypeID when 1 then SumValue else 0 end) ,0 ,0 ,sum(case a.TypeID when 104 then SumValue else 0 end) 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (1,104) and a.CountDate < date_add('{0}' ,interval 1 month) and a.CountDate >= '{0}'  {1}
  union all 
  select date_add('{0}' ,interval -1 month) ,sum(case a.TypeID when 1 then SumValue else 0 end) ,0 ,0 
    ,sum(case a.TypeID when 104 then SumValue else 0 end) 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (1,104) and a.CountDate < '{0}'  and a.CountDate >= date_add('{0}' ,interval -1 month)  {1}
  union all 
  select date_add('{0}' ,interval -1 year) ,sum(case a.TypeID when 1 then SumValue else 0 end) ,0 ,0 
    ,sum(case a.TypeID when 104 then SumValue else 0 end) 
  from "+ database3 + @".Clearing_Report a 
  where a.TypeID in (1,104) and a.CountDate >= date_add('{0}' ,interval -1 year) and a.CountDate < date_add(date_add('{0}' ,interval -1 year) ,interval 1 month)  {1}
)b on a.MonthTime = b.MonthTime group by a.MonthTime order by MonthTime asc; ", vbd.StartDate, chan);


                IEnumerable<MonthTempReport> i = cn.Query<MonthTempReport>(str.ToString());


                cn.Close();
                MonthReport wr = new MonthReport();
                if (i.Count() == 3)
                {
                    List<MonthTempReport> li = i.ToList();
                    var col1 = li[0];
                    var col2 = li[1];
                    var col3 = li[2];
                    wr.MonthAdd = col3.MonthAdd;
                    wr.MonthLogin = col3.MonthLogin;
                    wr.MonthPayCount = col3.MonthPayCount;
                    wr.MonthPaySum = col3.MonthPaySum;
                    wr.MonthTime = col3.MonthTime.Year+"-"+ col3.MonthTime.Month;

                    wr.LmMonthAdd = col2.MonthAdd;
                    wr.LmMonthLogin = col2.MonthLogin;
                    wr.LmMonthPayCount = col2.MonthPayCount;
                    wr.LmMonthPaySum = col2.MonthPaySum;
                    wr.LmMonthTime = col2.MonthTime.Year + "-" + col2.MonthTime.Month;


                    wr.LyMonthAdd = col1.MonthAdd;
                    wr.LyMonthLogin = col1.MonthLogin;
                    wr.LyMonthPayCount = col1.MonthPayCount;
                    wr.LyMonthPaySum = col1.MonthPaySum;
                    wr.LyMonthTime = col1.MonthTime.Year + "-" + col1.MonthTime.Month;
                }

                return wr;
            }
        }




        internal static IEnumerable<ChangleSum> GetChangleSum(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"

/*渠道统计*/
select ifnull(AgentName,'') AgentName , Agent AS Chanlle ,ifnull(sum(DayAdd) ,0) AS DayAdd ,ifnull((case when sum(DayAdd) = 0 then 0 else sum(NewBoard)/sum(DayAdd) end) * 100 ,0) AS NewBoardRate
  ,ifnull(sum(DayActive) ,0) AS DayActive ,ifnull((case when sum(LoginUser+DayActive) = 0 then 0 else sum(PayUser)/sum(LoginUser+DayActive) end) * 100 ,0) AS PayST
  ,ifnull(sum(PayUser) ,0) AS PayUserCount ,ifnull((case when sum(PayUser) = 0 then 0 else sum(Money)/sum(PayUser)/100 end) ,0) AS Arppu 
  ,ifnull((case when sum(LoginUser) = 0 then 0 else sum(Money)/sum(LoginUser)/100 end) ,0) AS Arpu ,ifnull(sum(Money)/100 ,0) AS TotalPay ,MAX(VersionNo) AS MaxVersionNo 
from (
/*渠道 版本号 新增 新增玩牌 活跃 登陆人数 付费人数 付费钱*/
  select a.Agent ,a.VersionInfo AS VersionNo,count(distinct a.ID) AS DayAdd ,count(distinct b.UserID) NewBoard ,0 DayActive ,0 LoginUser ,0 PayUser ,0 Money
  from "+ database1+ @".Role a 
    left join "+ database3+ @".Clearing_Game b on b.CountDate >= '{0}' and b.CountDate < date_add('{0}' ,interval 1 day)  and a.ID = b.UserID 
  where a.CreateTime >= '{0}' and a.CreateTime < date_add('{0}' ,interval 1 day) and agent <> 10010 and isfreeze = 0
  group by a.VersionInfo ,a.Agent 
  union all 
  /*活跃 登陆*/
  select b.Agent ,a.VersionInfo ,0 ,0 ,count(distinct case when b.CreateTime < '{0}' then a.UserID else null end) ,count(distinct a.UserID) ,0 ,0 
  from "+ database1 + @".BG_LoginRecord a 
      join "+ database1 + @".Role b on a.UserID = b.ID and b.agent <> 10010 and b.isfreeze = 0 
  where LoginTime >= '{0}' and LoginTime < date_add('{0}' ,interval 1 day) group by a.VersionInfo ,b.Agent 
  union all
  /*付费人数 付费钱*/
  select b.Agent ,a.VersionInfo ,0 ,0 ,0 ,0 ,count(distinct a.UserID) ,sum(a.Money) 
  from "+ database1 + @".QQZoneRecharge a 
      join "+ database1 + @".Role b on a.UserID = b.ID and b.agent <> 10010 and b.isfreeze = 0 
  where a.CreateTime >= '{0}' and a.CreateTime < date_add('{0}' ,interval 1 day) group by b.Agent ,a.VersionInfo
)t left join "+ database2 + @".AgentUsers a on a.id = t.Agent group by t.Agent ,ifnull(AgentName,'');
"
                , bdv.StartDate);

                IEnumerable<ChangleSum> i = cn.Query<ChangleSum>(str.ToString(), bdv);
                cn.Close();
                return i;
            }
        }

        public static Int64 GetChipSumFromUserMoneyRecord(string types,string startTime,string endTime)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
select SUM(Chip) as Obj  from "+ database3+ @".clearing_usermoneyrecord 
where RecordType in (" + types+@")
 and RecordTime>='"+ startTime + "' and  RecordTime<'"+ endTime + "' ");  
                IEnumerable<SingleData> i = cn.Query<SingleData>(str.ToString());
                cn.Close();
                SingleData data = i.FirstOrDefault();
                if (data == null)
                {
                    return 0;
                }
                else {
                    return Convert.ToInt64(data.Obj);
                }
            }
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