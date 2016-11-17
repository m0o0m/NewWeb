using MySql.Data.MySqlClient;
using GL.Dapper;
using GL.Command.DBUtility;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GL.Data.Model;
using System;
using System.Data;

namespace GL.Data.DAL
{
    public class QQZoneRechargeDAL
    {

        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static string GetSumRecharge(Model.BaseDataView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                string str = string.Empty;

                if (!string.IsNullOrEmpty(model.SearchExt))
                {
                    str = @"select IFNULL(sum(Money) /100,0) from QQZoneRecharge a join "+ database1 + @".Role b on a.userid = b.id and (b.id=@SearchExt or b.account=@SearchExt or b.nickname=@SearchExt)
                            where a.CreateTime between @StartDate and @ExpirationDate and b.agent = case @Channels when 0 then b.agent else @Channels end ";

                }
                else
                {
                    str = @"select IFNULL(sum(Money) /100,0) from QQZoneRecharge a join "+ database1 + @".Role b on a.userid = b.id 
                            where a.CreateTime between @StartDate and @ExpirationDate and b.agent = case @Channels when 0 then b.agent else @Channels end";
                }
                //if (model.RaType != null )
                //{
                //    if ( Convert.ToInt32(model.RaType) > 0) {
                //        str += " and PF=@RaType ";
                //    }
                //}
                string i = cn.ExecuteScalar(str, model).ToString();
                cn.Close();
                return i;
            }
        }


        internal static string GetFirstSumRecharge(Model.BaseDataView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                string str = @"select IFNULL(sum(Money) /100,0) 
                               from QQZoneRecharge 
                               where CreateTime between @StartDate and @ExpirationDate and IsFirst=1 ";


                string i = cn.ExecuteScalar(str, model).ToString();
                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<FirstChargeItem> GetFirstRechargeItemCount(Model.BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append("SELECT DATE_FORMAT(curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold > 10) as a, (SELECT count(0) as count FROM Role where Gold <= 10) as b;");
                str.Append(@" select count(*) as Count,PayItem from "+ database1 + @".QQZoneRecharge 
                                  where PayItem in ('firstCharge_1', 'firstCharge_2', 'firstCharge_3', 'firstCharge_4')
                                        and CreateTime between @StartDate and @ExpirationDate
                                  GROUP BY PayItem
                           ");




                IEnumerable<FirstChargeItem> i = cn.Query<FirstChargeItem>(str.ToString(), vbd);


                cn.Close();
                return i;
            }
        }

        /// <summary>
        ///  德州玩牌领大奖 
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        internal static IEnumerable<TexasGameGetAward> GetTexasGameGetAwardItemCount(Model.BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append("SELECT DATE_FORMAT(curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold > 10) as a, (SELECT count(0) as count FROM Role where Gold <= 10) as b;");


                if (vbd.StartDate == "")
                {
                    str.Append(@" 
select date(CreateTime) CreateTime 
  ,sum(case GiftID when 0 then 1 else 0 end) as Position_18
  ,sum(case GiftID when 1 then 1 else 0 end) as Position_58
  ,sum(case GiftID when 2 then 1 else 0 end) as Position_118
  ,sum(case GiftID when 3 then 1 else 0 end) as Position_238
  ,sum(case GiftID when 4 then 1 else 0 end) as Position_388  
from "+ database1 + @".BG_ActiveRecord
where  ActiveID=1
GROUP BY date(CreateTime)
                           ");
                }
                else
                {
                    str.Append(@" 
select date(CreateTime) CreateTime 
  ,sum(case GiftID when 0 then 1 else 0 end) as Position_18
  ,sum(case GiftID when 1 then 1 else 0 end) as Position_58
  ,sum(case GiftID when 2 then 1 else 0 end) as Position_118
  ,sum(case GiftID when 3 then 1 else 0 end) as Position_238
  ,sum(case GiftID when 4 then 1 else 0 end) as Position_388  
from "+ database1 + @".BG_ActiveRecord
where CreateTime >= @StartDate and  CreateTime< @ExpirationDate and ActiveID=1
GROUP BY date(CreateTime)
                           ");
                }




                IEnumerable<TexasGameGetAward> i = cn.Query<TexasGameGetAward>(str.ToString(), vbd);


                cn.Close();
                return i;
            }
        }


        public static ActiveTime GetGameActiveTime(ActiveType type)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append("SELECT DATE_FORMAT(curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold > 10) as a, (SELECT count(0) as count FROM Role where Gold <= 10) as b;");
                str.Append(@" select BeginDate as StartTime,EndDate as EndTime from  S_Active where ActiveID =  " + (int)type);




                IEnumerable<ActiveTime> i = cn.Query<ActiveTime>(str.ToString());


                cn.Close();
                return i.FirstOrDefault();
            }
        }



        /// <summary>
        /// 新年充值活动 
        /// </summary>
        internal static IEnumerable<NewYearRechargeSum> GetNewYearCharge(Model.BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append("SELECT DATE_FORMAT(curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold > 10) as a, (SELECT count(0) as count FROM Role where Gold <= 10) as b;");
                str.Append(@" select count(*) as Count,GiftID as Postion from "+ database1 + @".BG_ActiveRecord
                              where CreateTime >= @StartDate and  CreateTime< @ExpirationDate and ActiveID=2 
                              GROUP BY GiftID;
 
                           ");

                IEnumerable<NewYearRechargeSum> i = cn.Query<NewYearRechargeSum>(str.ToString(), vbd);


                cn.Close();
                return i;
            }
        }

        /// <summary>
        /// 新年充值活动-充值排行前10 
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        internal static IEnumerable<RechargeRank> NewYearChargeRank(Model.BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append("SELECT DATE_FORMAT(curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold > 10) as a, (SELECT count(0) as count FROM Role where Gold <= 10) as b;");
                str.Append(@" 
select t.*,r.NickName from (
select UserID , sum(Money)/100 as Money from "+ database1 + @".QQZoneRecharge 
 where CreateTime >= @StartDate and  CreateTime< @ExpirationDate
GROUP BY UserID
order by Money desc 
limit 10 ) t,Role r
where t.UserID = r.ID


                           ");

                IEnumerable<RechargeRank> i = cn.Query<RechargeRank>(str.ToString(), vbd);


                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<Festival515> GetFestival515(FestivalBaseData fbd)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //str.Append("SELECT DATE_FORMAT(curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM Role where Gold > 10) as a, (SELECT count(0) as count FROM Role where Gold <= 10) as b;");
                str.AppendFormat(@" 

SELECT (select COUNT(DISTINCT UserID) from "+ database1 + @".BG_LoginRecord
where LoginTime >= t1.CreateTime and LoginTime<DATE_ADD(t1.CreateTime,INTERVAL 1 DAY)
and LoginAgent!=10010) as DAU,
t1.CreateTime as DateTime,
COUNT(t1.UserID) as IconPCount,
SUM(t1.clickcount) as IconPNum,
SUM(t1.dtcount) as DIconPCount,
SUM(t1.dtnum) as DIconPNum,
SUM(t1.fjcount) as FIconPCount,
SUM(t1.fjnum) as FIconPNum
from (
select t.CreateTime,t.UserID,sum(t.ClickCount) as clickcount,SUM( DISTINCT t.dtcount) as dtcount,SUM(t.dtnum) as dtnum,
SUM( DISTINCT t.fjcount ) as fjcount,SUM(t.fjnum) as fjnum from (
select  DATE_FORMAT(CreateTime,'%Y-%m-%d') as CreateTime,UserID,ClickCount,
case SiteID
when 1 then 1
else  0
end as dtcount,
case SiteID
when 1 then ClickCount
else  0
end as dtnum,
case SiteID
when 2 then 1
else  0
end as fjcount,
case SiteID
when 2 then ClickCount
else  0
end as fjnum
from "+ database3+ @".BG_ClickRcord
where CreateTime BETWEEN @StartTime and @ExpirationDate  {0}
) as t
GROUP BY t.CreateTime,t.UserID
) as t1
GROUP BY t1.CreateTime;
", fbd.ClientType > 0 ? " and PlatType=@ClientType" : "");

                IEnumerable<Festival515> i = cn.Query<Festival515>(str.ToString(), fbd);


                cn.Close();
                return i;
            }
        }

        internal static AllFesLogin GetFestivalLogin(FestivalBaseData fbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                AllFesLogin i ;
                using (var multi = cn.QueryMultiple(""+ database3 + @".sys_get_515", new { },null,null,CommandType.StoredProcedure))
                {
                    var thirdDay = multi.Read<FestivalLogin>().ToList();
                 
                    var second = multi.Read<FestivalLogin>().ToList();

                    var one = multi.Read<FestivalLogin>().ToList();

                    i = new AllFesLogin();

                    i.OneDay = one;
                    i.SecDay = second;
                    i.thirdDay = thirdDay;
                }


                cn.Close();
                return i;



            }

        }

        internal static FestivalVIP GetFestivalVIP(FestivalBaseData fbd)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                IEnumerable<FestivalVIP> i = cn.Query<FestivalVIP>(@""+ database3 + @".sys_get_active_vip
", new { },null,true,null,CommandType.StoredProcedure);


                cn.Close();
                return i.FirstOrDefault();
            }

        }


    }
}
