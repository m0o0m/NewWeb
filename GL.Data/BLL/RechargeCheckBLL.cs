using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;
using GL.Command.DBUtility;


namespace GL.Data.BLL
{
    public class RechargeCheckBLL
    {

        public static int Delete(RechargeCheck model)
        {
            return RechargeCheckDAL.Delete(model);
        }

        public static int Add(RechargeCheck model)
        {
            return RechargeCheckDAL.Add(model);
        }

        public static RechargeCheck GetModelByID(RechargeCheck mi)
        {
            return RechargeCheckDAL.GetModelByID(mi);
        }

        public static IEnumerable<RechargeCheck> GetModelByUserID(RechargeCheck mi)
        {
            return RechargeCheckDAL.GetModelByUserID(mi);
        }

        public static int Update(RechargeCheck model)
        {
            return RechargeCheckDAL.Update(model);
        }


        public static RechargeCheck GetModelBySerialNo(RechargeCheck model)
        {
            return RechargeCheckDAL.GetModelBySerialNo(model);
        }

        public static int AddOrderIP(UserIpInfo model)
        {
            return RechargeCheckDAL.AddOrderIP(model);
        }

        public static int AddCallBackOrderIP(UserIpInfo model)
        {
            return RechargeCheckDAL.AddCallBackOrderIP(model);
        }

        public static List<UserIpInfo> GetCallBackOrder(string orderID)
        {
            var data = RechargeCheckDAL.GetCallBackOrder(orderID);
            if (data.Count() == 0) {
                return new List<UserIpInfo>();
            }
            return data.ToList();
        }

        public static PagedList<UserIpInfo> GetListByPage(int page, UserIpInfo model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<UserIpInfo>.GetRecordCount(string.Format(@"
select count(*) from record.Charge_OrderIPRecord 
where  CreateTime BETWEEN '"+model.StartTime + "' and '"+model.EndTime+@"' {0} {1} {2} {3}",
      string.IsNullOrEmpty(model.OrderID)?"": " and OrderID ='"+model.OrderID+"'",
      model.UserID<=0?"": " and  UserID="+model.UserID,
      model.ChargeType<0?"": " and ChargeType="+model.ChargeType,
       string.IsNullOrEmpty(model.OrderIP) ? "" : " and OrderIP ='" + model.OrderIP + "'"
     
      ));

            pq.Sql = string.Format(@"
select * from record.Charge_OrderIPRecord 
where  CreateTime BETWEEN '" + model.StartTime + "' and '" + model.EndTime + @"' {0} {1} {2} {3}   limit {4}, {5}",
      string.IsNullOrEmpty(model.OrderID) ? "" : " and OrderID ='" + model.OrderID + "'",
      model.UserID <= 0 ? "" : " and  UserID=" + model.UserID,
      model.ChargeType < 0 ? "" : " and ChargeType=" + model.ChargeType,
       string.IsNullOrEmpty(model.OrderIP) ? "" : " and OrderIP ='" + model.OrderIP + "'",
       pq.StartRowNumber, pq.PageSize
      );
    PagedList<UserIpInfo> obj = new PagedList<UserIpInfo>(DAL.PagedListDAL<UserIpInfo>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
     return obj;

        }

        public static List<CallBackRechargeIP> GetCallBackIP(UserIpInfo model) {
            var data = RechargeCheckDAL.GetCallBackIP(model);
            if (data == null)
            {
                return new List<CallBackRechargeIP>();
            }
            else {
                return data.ToList();
            }
        }


        public static bool AddChargeIP(string ip, string type) {
            return RechargeCheckDAL.AddChargeIP(ip, type);
        }

        public static bool DeleteChargeIP(string ip, string type)
        {
            return RechargeCheckDAL.DeleteChargeIP(ip, type);
        }


        public static bool IsTrustIp(string ip, string type) {
            return RechargeCheckDAL.IsTrustIp(ip, type);
        }


        public static int AddAppTreInfo(AppTreasure model)
        {
            return RechargeCheckDAL.AddAppTreInfo(model);
        }

        public static AppTreasure GetModelByUserID(string userid)
        {
            return RechargeCheckDAL.GetModelByUserID(userid);
        }
    }
}
