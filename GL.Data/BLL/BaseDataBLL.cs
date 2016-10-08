using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Common;
using GL.Data.DAL;
using Webdiyer.WebControls.Mvc;
using GL.Command.DBUtility;

namespace GL.Data.BLL
{
    public class BaseDataBLL
    {
        public static IEnumerable<BaseDataInfo> GetGameProfit(BaseDataView bdv)
        {
            return DAL.BaseDataDAL.GetGameProfit(bdv);
        }
        public static IEnumerable<BaseDataInfo> GetRegisteredUsers(BaseDataView bdv)
        {
            return DAL.BaseDataDAL.GetRegisteredUsers(bdv);
        }
        public static GameOutputDetail GetGameOutputDetailUser(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetGameOutputDetailUser(vbd);
        }
        public static IEnumerable<BaseDataInfo> GetUsreProfit(int page ,BaseDataView bdv)
        {
            return DAL.BaseDataDAL.GetUsreProfit(page,bdv);
        }
        //GetRegisteredUsersOnHour
        public static IEnumerable<BaseDataInfo> GetRegisteredUsersOnHour(BaseDataView bdv)
        {
            return DAL.BaseDataDAL.GetRegisteredUsersOnHour(bdv);
        }

        public static IEnumerable<BaseDataInfo> GetRegisteredUsersByChannel(BaseDataView bdv)
        {
            return DAL.BaseDataDAL.GetRegisteredUsersByChannel(bdv);
        }

        public static IEnumerable<BaseDataInfo> GetActiveUsers(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetActiveUsers(vbd);
        }

        public static IEnumerable<BaseDataInfoForOnlinePlay> GetOnlinePlay(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetOnlinePlay(vbd);
        }
        //public static int GetAllPlayCount(BaseDataView vbd)
        //{
        //    return DAL.BaseDataDAL.GetAllPlayCount(vbd);
        //}
        public static object GetAllUser(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetAllUser(vbd);
        }

        public static IEnumerable<Roulette> GetRouletteData(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetRouletteData(vbd);
        }

        public static IEnumerable<Roulette> GetRouletteShop(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetRouletteShop(vbd);
        }

        public static int GetRouletteShop(int id)
        {
            return DAL.BaseDataDAL.GetRouletteShop(id);
        }

        //public static IEnumerable<BaseDataInfo> GetPlayCount(BaseDataView vbd)
        //{
        //    return DAL.BaseDataDAL.GetPlayCount(vbd);
        //}
        public static IEnumerable<BaseDataInfoForRetentionRates> GetRetentionRates(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetRetentionRates(vbd);
        }

        public static IEnumerable<GameKeep> GetGameRetentionRates(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetGameRetentionRates(vbd);
        }
        

        public static IEnumerable<BaseDataInfoForBankruptcyRate> GetBankruptcyRate(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetBankruptcyRate(vbd);
        }


        public static IEnumerable<QQZoneRechargeCount> GetQQZoneRechargeCount(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetQQZoneRechargeCount(vbd);
        }

        /// <summary>
        /// 当日注册且充值玩家
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeCurReChaDetail(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetQQZoneRechargeCurReChaDetail(vbd);
        }

        /// <summary>
        /// 充值总人数
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeAllReChaDetail(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetQQZoneRechargeAllReChaDetail(vbd);
        }
        /// <summary>
        /// 首冲详细信息
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeFirstChargeDetail(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetQQZoneRechargeFirstChargeDetail(vbd);
        }

        /// <summary>
        /// 再次付费人数
        /// </summary>
        /// <param name="vbd"></param>
        /// <returns></returns>
        public static IEnumerable<QQZoneRechargeCountDetail> GetQQZoneRechargeReChargeDetail(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetQQZoneRechargeReChargeDetail(vbd);
        }

        //public static IEnumerable<GameOutput> GetGameOutput(BaseDataView vbd)
        //{
        //    return DAL.BaseDataDAL.GetGameOutput(vbd);
        //}


        //public static GameOutputDetail GetGameOutputDetail(BaseDataView vbd)
        //{
        //    return DAL.BaseDataDAL.GetGameOutputDetail(vbd);
        //}
        public static IEnumerable<GameOutput> GetGameOutput2(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetGameOutput2(vbd);
        }


        public static GameOutAccurate GetGameOutAccurate(string btime,string etime) {
            return DAL.BaseDataDAL.GetGameOutAccurate(btime,etime);
        }
        public static GameOutAccurate GetGameOutAccurateFirst() {
            return DAL.BaseDataDAL.GetGameOutAccurateFirst();
        }

        public static GameOutputDetail GetGameOutputDetail2(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetGameOutputDetail2(vbd);
        }

        public static List<GameOutputRecursion> GetGameOutputRecursion(BaseDataView vbd) {
            return DAL.BaseDataDAL.GetGameOutputRecursion(vbd);
        }



        public static IEnumerable<PotRecord> GetPotRecord()
        {
            return DAL.BaseDataDAL.GetPotRecord();
        }
        public static IEnumerable<PotRecord> GetPotRecord(int Num)
        {
            return DAL.BaseDataDAL.GetPotRecord(Num);
        }

        public static IEnumerable<JiFen> GetScoreboard(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetScoreboard(vbd);
        }

        public static PagedList<JiFen> GetNowScoreboard(int page)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 20;
            pq.RecordCount = 200;
            pq.Sql = string.Format(@"
select "+ ((page-1)*pq.PageSize+1) + @" as No, ID as playerID, Account as UserName,Zicard as Jifen ,NickName
from 515game.Role 
order by Zicard desc limit {0}, {1}
", pq.StartRowNumber, pq.PageSize);

            PagedList<JiFen> obj = new PagedList<JiFen>(DAL.PagedListDAL<JiFen>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }



        public static PagedList<MonitorLog> GetMonitorLogList(BaseDataView bdv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<MonitorLog>.GetRecordCount(string.Format(@"select count(0) from GServerInfo.MonitorLog where MonitorID=28 and CreateTime>='"+bdv.StartDate+"' and CreateTime<'"+bdv.ExpirationDate+"' "));
         
            pq.Sql = string.Format(@"
select * from GServerInfo.MonitorLog where MonitorID=28   and CreateTime>='" + bdv.StartDate + "' and CreateTime<'" + bdv.ExpirationDate + @"' 
order by CreateTime desc limit {0}, {1};
", pq.StartRowNumber, pq.PageSize);


            PagedList<MonitorLog> obj = new PagedList<MonitorLog>(DAL.PagedListDAL<MonitorLog>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        
        public static IEnumerable<CommonIDName> GetPhoneBoard() {

            return DAL.BaseDataDAL.GetPhoneBoard();
        }


        public static IEnumerable<CommonIDName> GetModelByBoard(string brand) {
            return DAL.BaseDataDAL.GetModelByBoard(brand);
        }



        public static IEnumerable<OpenFuDai> GetOpenFuDai(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetOpenFuDai(vbd);
        }



        public static BaseDataInfoForUsersDiamondDistributionRatio GetUsersDiamondDistributionRatio(BaseDataView bdv)
        {
            return DAL.BaseDataDAL.GetUsersDiamondDistributionRatio(bdv);
        }

        public static BaseDataInfoForUsersGoldDistributionRatio GetUsersGoldDistributionRatio(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetUsersGoldDistributionRatio(vbd);
        }




        public static IEnumerable<BaseDataInfoForPotRakeback> GetPotRakeback(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetPotRakeback(vbd);
        }

        public static BaseDataInfoForVIPDistributionRatio GetVIPDistributionRatio(BaseDataView bdv)
        {
            return DAL.BaseDataDAL.GetVIPDistributionRatio(bdv);
        }

        public static IEnumerable<BaseDataInfo> GetNoviceTask(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetNoviceTask(vbd);
        }

        public static PagedList<BaseDataInfo> GetNoviceTaskPage(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetNoviceTaskPage(vbd);

        }

        public static SignDraw GetSignDraw(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetSignDraw(vbd);
        }
        public static IEnumerable<Ruin> GetRuinUsers(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetRuinUsers(vbd);
        }


        //    internal static IEnumerable<VersionSum> GetVersionSum(BaseDataView bdv)

        public static IEnumerable<VersionSum> GetVersionSum(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetVersionSum(vbd);
        }


        public static IEnumerable<ChangleSum> GetChangleSum(BaseDataView vbd)
        {

            return DAL.BaseDataDAL.GetChangleSum(vbd);
        }


        //DayReport
        public static IEnumerable<DayReport> GetDayReportList(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetDayReportList(vbd);
        }


        public static WeekReport GetWeekReport(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetWeekReport(vbd);
        }


        public static MonthReport GetMonthReport(BaseDataView vbd)
        {
            return DAL.BaseDataDAL.GetMonthReport(vbd);
        }

    }
}
