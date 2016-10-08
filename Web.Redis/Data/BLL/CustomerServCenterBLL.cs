using Data.Model;
using Data.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.BLL
{
    public class CustomerServCenterBLL
    {

        internal static IEnumerable<CSCModel> GetList(int GUUserID)
        {
            IEnumerable<CustomerServCenter> csc = Data.DAL.CustomerServCenterDAL.GetList(GUUserID);
            IEnumerable<CSCModel> cm = from cc in csc
                                       where cc.CSCSubId == 0
                                       orderby cc.CSCTime descending
                                       select new CSCModel
                                       {
                                           CSCMainID = cc.CSCMainID,
                                           CSCSubId = cc.CSCSubId,
                                           CSCTitle = cc.CSCTitle,
                                           CSCContent = cc.CSCContent,
                                           CSCType = cc.CSCType,
                                           CSCState = cc.CSCState,
                                           CSCTime = cc.CSCTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                           GUName = cc.GUName,
                                           CSC =
                                             (from dd in csc
                                             where dd.CSCSubId == cc.CSCMainID
                                              orderby cc.CSCTime ascending
                                             select new CSCLower
                                             {
                                                 CSCTime = dd.CSCTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                 GUName = dd.GUName,
                                                 CSCContent = dd.CSCContent
                                             }).ToList()
                                       };

            List<CSCModel> siss = cm.ToList();

            return cm;
        }

        internal static int Insert(CustomerServCenter cust)
        {
            return Data.DAL.CustomerServCenterDAL.Insert(cust);
        }

        internal static int Update(CustomerServCenter c)
        {
            return Data.DAL.CustomerServCenterDAL.Update(c);
        }
    }
}