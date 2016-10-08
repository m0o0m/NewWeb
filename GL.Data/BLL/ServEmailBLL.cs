using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class ServEmailBLL
    {



        public static PagedList<ServEmail> GetListByPage(BaseDataView bdv)
        {


            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;
            string where = " where ServEmailTime BETWEEN '"+bdv.StartDate+"' and '"+bdv.ExpirationDate+"' ";
            pq.RecordCount = DAL.PagedListDAL<ServEmail>.GetRecordCount(@"select count(0) from ServEmail "+ where);
            pq.Sql = string.Format(@"select * from ServEmail "+ where + " order by ServEmailID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<ServEmail> obj = new PagedList<ServEmail>(DAL.PagedListDAL<ServEmail>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;



        }

        public static int Insert(ServEmail servEmail)
        {
            return ServEmailDAL.Insert(servEmail);
        }

        public static int Delete(ServEmail servEmail)
        {
            return ServEmailDAL.Delete(servEmail);
        }
    }
}
