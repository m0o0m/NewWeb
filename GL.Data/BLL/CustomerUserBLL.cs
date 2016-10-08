using GL.Common;
using GL.Data.DAL;
using GL.Data.OWeb.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class CustomerUserBLL
    {
        public static PagedList<CustomerUser> GetListByPage(int page)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<CustomerUser>.GetRecordCount(@"select count(0) from CustomerUsers");
            pq.Sql = string.Format(@"SELECT * FROM CustomerUsers order by Id desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<CustomerUser> obj = new PagedList<CustomerUser>(DAL.PagedListDAL<CustomerUser>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }


    }
}
