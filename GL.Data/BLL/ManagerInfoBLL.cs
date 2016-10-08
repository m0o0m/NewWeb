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
    public class ManagerInfoBLL
    {
        public static ManagerInfo GetModel(ManagerInfo mi)
        {
            ManagerInfo cust = DAL.ManagerInfoDAL.GetModel(mi);
            return cust;
        }

        public static PagedList<ManagerInfo> GetListByPage(int page)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<ManagerInfo>.GetRecordCount(@"select count(0) from ManagerInfo");
            pq.Sql = string.Format(@"select * from ManagerInfo order by AdminID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<ManagerInfo> obj = new PagedList<ManagerInfo>(DAL.PagedListDAL<ManagerInfo>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }




        public static int Delete(ManagerInfo model)
        {
            return DAL.ManagerInfoDAL.Delete(model);
        }

        public static int Add(ManagerInfo model)
        {
            return DAL.ManagerInfoDAL.Add(model);
        }

        public static ManagerInfo GetModelByID(ManagerInfo mi)
        {
            ManagerInfo cust = DAL.ManagerInfoDAL.GetModelByID(mi);
            return cust;
        }

        public static int Update(ManagerInfo model)
        {
            return DAL.ManagerInfoDAL.Update(model);
        }

    }
}
