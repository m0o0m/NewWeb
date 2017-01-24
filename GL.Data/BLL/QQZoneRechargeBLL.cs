using GL.Command.DBUtility;
using GL.Common;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;


namespace GL.Data.BLL
{
    public class QQZoneRechargeBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");
        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        public static PagedList<QQZoneRecharge> GetListByPage(int page)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<QQZoneRecharge>.GetRecordCount(@"select count(0) from QQZoneRecharge", sqlconnectionString);
            pq.Sql = string.Format(@"select q.*, Role.NickName as NickName, Role.Account as UserAccount from QQZoneRecharge as q,Role where q.UserID = Role.ID order by q.CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<QQZoneRecharge> obj = new PagedList<QQZoneRecharge>(DAL.PagedListDAL<QQZoneRecharge>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }
        public static PagedList<QQZoneRecharge> GetListByPageLoginAgent(int page, BaseDataView vbd) {

            var arr = vbd.StartDate.Split('-');
            DateTime dts = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), Convert.ToInt32(vbd.StartDate.Substring(8, 2)), 0, 0, 0);
            dts = dts.AddDays(-7);


            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            if (!string.IsNullOrEmpty(vbd.SearchExt))
            {
                string myWhere = @" 
(q.UserID = BINARY '{2}' or l.NickName=BINARY '{2}' or l.UserAccount=BINARY '{2}' )  and                
q.UserID = l.UserID and q.CreateTime BETWEEN '{0}' and '{1}'  ";
                if (vbd.RaType != null && Convert.ToInt32(vbd.RaType) > 0)
                {
                    myWhere += " and q.PF=" + vbd.RaType + " ";
                }

                pq.RecordCount = DAL.PagedListDAL<QQZoneRecharge>.GetRecordCount(string.Format(@"
select count(*) from QQZoneRecharge as q,(
    select a.* from (
        select UserID,LoginAgent,AgentName,NickName,UserAccount from "+database1+@".V_LoginRecord where LoginTime BETWEEN '"+ dts + @"' and '{1}'
        ORDER BY LoginTime desc
) as a
GROUP BY a.UserID) as l
where " + myWhere + " {3}",
                    vbd.StartDate, vbd.ExpirationDate, vbd.SearchExt, vbd.Channels == 0 ? "" : string.Format(" and l.LoginAgent in ({0})", vbd.Channels)), sqlconnectionString);

                pq.Sql = string.Format(@"
select q.*,l.LoginAgent,l.AgentName,l.NickName,l.UserAccount from QQZoneRecharge as q,(
    select a.* from (
        select UserID,LoginAgent,AgentName,NickName,UserAccount from "+ database1 + @".V_LoginRecord where LoginTime BETWEEN '"+ dts + @"' and '{1}'
        ORDER BY LoginTime desc
) as a
GROUP BY a.UserID) as l
where " + myWhere + "{5} order by q.CreateTime desc limit {3}, {4}",
                         vbd.StartDate, vbd.ExpirationDate, vbd.SearchExt, pq.StartRowNumber, pq.PageSize, vbd.Channels == 0 ? "" : string.Format(" and l.LoginAgent in ({0})", vbd.Channels));

            }
            else
            {
                string myWhere = @"  q.UserID = l.UserID and q.CreateTime BETWEEN '{0}' and '{1}'  ";
                if (vbd.RaType != null && Convert.ToInt32(vbd.RaType) > 0)
                {
                    myWhere += " and q.PF=" + vbd.RaType + " ";
                }

                pq.RecordCount = DAL.PagedListDAL<QQZoneRecharge>.GetRecordCount(string.Format(@"
select count(*) from QQZoneRecharge as q,(
    select a.* from (
        select UserID,LoginAgent,AgentName,NickName,UserAccount from "+ database1 + @".V_LoginRecord where LoginTime BETWEEN '"+ dts + @"' and '{1}'
        ORDER BY LoginTime desc
) as a
GROUP BY a.UserID) as l
where " + myWhere + " {2}",
                    vbd.StartDate, vbd.ExpirationDate, vbd.Channels == 0 ? "" : string.Format(" and l.LoginAgent in ({0})", vbd.Channels)), sqlconnectionString);

                pq.Sql = string.Format(@"
select q.*,l.LoginAgent,l.AgentName,l.NickName,l.UserAccount from QQZoneRecharge as q,(
    select a.* from (
        select UserID,LoginAgent,AgentName,NickName,UserAccount from "+ database1 + @".V_LoginRecord where LoginTime BETWEEN '"+ dts + @"' and '{1}'
        ORDER BY LoginTime desc
) as a
GROUP BY a.UserID) as l
where " + myWhere + "{4} order by q.CreateTime desc limit {2}, {3}",
                         vbd.StartDate, vbd.ExpirationDate, pq.StartRowNumber, pq.PageSize, vbd.Channels == 0 ? "" : string.Format(" and l.LoginAgent in ({0})", vbd.Channels));

            }


            PagedList<QQZoneRecharge> obj = new PagedList<QQZoneRecharge>(DAL.PagedListDAL<QQZoneRecharge>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

        public static PagedList<QQZoneRecharge> GetListByPage(int page, BaseDataView vbd)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;



            if (!string.IsNullOrEmpty(vbd.SearchExt) )
            {

                string myWhere = "q.UserID = r.ID  and ( r.ID = BINARY '{2}' or r.NickName= BINARY '{2}' or r.Account= BINARY '{2}' ) {3} and q.CreateTime between '{0}' and '{1}'";
                if (vbd.RaType != null && Convert.ToInt32(vbd.RaType) > 0)
                {
                    myWhere += " and q.PF=" + vbd.RaType + " ";
                }




                pq.RecordCount = DAL.PagedListDAL<QQZoneRecharge>.GetRecordCount(
                                  string.Format(@"select count(0) from QQZoneRecharge as q ,Role as r  where " + myWhere  , 
                                  vbd.StartDate, vbd.ExpirationDate, vbd.SearchExt, vbd.Channels == 0 ? "" : string.Format(" and r.Agent in ({0})", vbd.Channels))
                                  , sqlconnectionString);

                pq.Sql = string.Format(@"select q.*, r.NickName as NickName, r.Account as UserAccount,a.AgentName from QQZoneRecharge as q ,Role as r,GServerInfo.AgentUsers as a
                                         where r.Agent =  a.Id and " + myWhere + @" order by q.CreateTime desc limit {4},{5}", 
                    vbd.StartDate, vbd.ExpirationDate,vbd.SearchExt, vbd.Channels == 0 ? "" : string.Format(" and r.Agent in ({0})", vbd.Channels), pq.StartRowNumber, pq.PageSize);
            }
            else
            {
                string myWhere = " q.CreateTime between '{0}' and '{1}' ";
                if (vbd.RaType != null && Convert.ToInt32(vbd.RaType) > 0)
                {
                    myWhere += " and q.PF=" + vbd.RaType + " ";
                }

                pq.RecordCount = DAL.PagedListDAL<QQZoneRecharge>.GetRecordCount(string.Format(@"select count(0) from QQZoneRecharge as q join Role r on q.userid = r.id where " + myWhere + " {2}", 
                    vbd.StartDate, vbd.ExpirationDate, vbd.Channels == 0 ? "" : string.Format(" and r.Agent in ({0})", vbd.Channels)), sqlconnectionString);

                pq.Sql = string.Format(@"select q.*, r.NickName as NickName, r.Account as UserAccount,a.AgentName from QQZoneRecharge as q,Role as r,GServerInfo.AgentUsers as a 
                         where q.UserID = r.ID and r.Agent =  a.Id and " + myWhere + "{4} order by q.CreateTime desc limit {2}, {3}", 
                         vbd.StartDate, vbd.ExpirationDate, pq.StartRowNumber, pq.PageSize, vbd.Channels == 0 ? "" : string.Format(" and r.Agent in ({0})", vbd.Channels));
            }


            PagedList<QQZoneRecharge> obj = new PagedList<QQZoneRecharge>(DAL.PagedListDAL<QQZoneRecharge>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static string GetSumRecharge(BaseDataView model)
        {
            return DAL.QQZoneRechargeDAL.GetSumRecharge(model);
        }


        public static string GetFirstSumRecharge(BaseDataView model) {
            return DAL.QQZoneRechargeDAL.GetFirstSumRecharge(model);
        }

        public static IEnumerable<FirstChargeItem> GetFirstRechargeItemCount(BaseDataView bdv) {

            return DAL.QQZoneRechargeDAL.GetFirstRechargeItemCount(bdv);

        }


        public static IEnumerable<TexasGameGetAward> GetTexasGameGetAwardItemCount(BaseDataView bdv)
        {

            return DAL.QQZoneRechargeDAL.GetTexasGameGetAwardItemCount(bdv);

        }

        public static IEnumerable<NewYearRechargeSum> GetNewYearCharge(BaseDataView bdv)
        {

            return DAL.QQZoneRechargeDAL.GetNewYearCharge(bdv);

        }

        public static IEnumerable<RechargeRank> NewYearChargeRank(BaseDataView bdv)
        {

            return DAL.QQZoneRechargeDAL.NewYearChargeRank(bdv);

        }

        public static ActiveTime GetGameActiveTime(ActiveType type)
        {
            return DAL.QQZoneRechargeDAL.GetGameActiveTime(type);
        }


        public static IEnumerable<Festival515> GetFestival515(FestivalBaseData fbd)
        {

            return DAL.QQZoneRechargeDAL.GetFestival515(fbd);

        }


        public static AllFesLogin GetFestivalLogin(FestivalBaseData fbd)
        {

            return DAL.QQZoneRechargeDAL.GetFestivalLogin(fbd);

        }

        public static FestivalVIP GetFestivalVIP(FestivalBaseData fbd)
        {

            return DAL.QQZoneRechargeDAL.GetFestivalVIP(fbd);

        }


        public static IEnumerable<RechargeOpen> GetRechargeOpen() {
            return DAL.QQZoneRechargeDAL.GetRechargeOpen();
        }

        public static int  SetRechargeOpen(RechargeOpen model)
        {
            return DAL.QQZoneRechargeDAL.SetRechargeOpen(model);
        }


        public static bool GetRechargeOpen(int rechargeID)
        {
            RechargeOpen model = DAL.QQZoneRechargeDAL.GetRechargeOpen(rechargeID);
            if (model == null)
            {
                return false;
            }
            else {
                return model.IsOpen;
            }
        }
    }
}
