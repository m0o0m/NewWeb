using GL.Common;
using GL.Data.Model;
using GL.Data.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class CustomerServCenterBLL
    {
        public static IEnumerable<CSCModel> GetList(int GUUserID)
        {
            IEnumerable<CustomerServCenter> csc = DAL.CustomerServCenterDAL.GetList(GUUserID);
            IEnumerable<CSCModel> cm = from cc in csc
                                       where cc.CSCSubId == 0
                                       orderby cc.CSCTime descending
                                       select new CSCModel
                                       {
                                           CSCMainID = cc.CSCMainID,
                                           CSCSubId = cc.CSCSubId,
                                           CSCTitle = cc.CSCTitle,
                                           CSCContent = cc.CSCContent,
                                           CSCType = (int)cc.CSCType,
                                           CSCState = (int)cc.CSCState,
                                           CSCTime = cc.CSCTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                           GUName = cc.GUName,
                                           CSC =
                                             from dd in csc
                                             where dd.CSCSubId == cc.CSCMainID
                                             orderby dd.CSCTime descending
                                             select new CSCLower
                                             {
                                                 CSCTime = dd.CSCTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                 GUName = dd.GUName,
                                                 CSCContent = dd.CSCContent
                                             }
                                       };
            return cm;
        }

        public static PagedList<CustomerServCenter> GetListByPage(int page)
        {

            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<CustomerServCenter>.GetRecordCount(@"select count(0) from CustomerServCenter where CSCSubId = 0");
            pq.Sql = string.Format(@"
select CSCMainID,CSCSubId,GUUserID,CSCTime,CSCTitle,CSCType,CSCContent,CSCState,GUName,GUType,IFNULL(CSCUpdateTime,CSCTime)  as CSCUpdateTime
from GServerInfo.CustomerServCenter where CSCSubId = 0 order by CSCUpdateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<CustomerServCenter> csc = new PagedList<CustomerServCenter>(DAL.PagedListDAL<CustomerServCenter>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return csc;
        }

        public static PagedList<CustomerServCenter> GetListByPage(BaseDataView bdv) {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<CustomerServCenter>.GetRecordCount(@"select count(0) from CustomerServCenter where CSCSubId = 0 and  CSCTime BETWEEN '"+bdv.StartDate+"' and '"+bdv.ExpirationDate+"'");
            pq.Sql = string.Format(@"select * from CustomerServCenter where CSCSubId = 0 and  CSCTime BETWEEN '{2}' and '{3}'  order by CSCTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize,bdv.StartDate,bdv.ExpirationDate);

            PagedList<CustomerServCenter> csc = new PagedList<CustomerServCenter>(DAL.PagedListDAL<CustomerServCenter>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return csc;
        }





        public static CustomerServCenter GetModel(CustomerServCenter model)
        {
            return DAL.CustomerServCenterDAL.GetModel(model);
        }

        public static int Insert(CustomerServCenter model)
        {
            return DAL.CustomerServCenterDAL.Insert(model);
        }

        public static int Update(CustomerServCenter model)
        {
            return DAL.CustomerServCenterDAL.Update(model);
        }

        public static int Delete(CustomerServCenter cust)
        {
            return DAL.CustomerServCenterDAL.Delete(cust);
        }


        public static IEnumerable<CustomerServCenter> GetListWithCSCSubId(CustomerServCenter cust)
        {
            return DAL.CustomerServCenterDAL.GetListWithCSCSubId(cust);
        }

        public static int UpdateForManage(CustomerServCenter c)
        {
            return DAL.CustomerServCenterDAL.UpdateForManage(c);
        }
    }
}
