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
    public class FAQBLL
    {
        public static PagedList<FAQ> GetListByPage(int page)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 5;
            pq.RecordCount = DAL.PagedListDAL<FAQ>.GetRecordCount(@"select count(0) from FAQ");
            pq.Sql = string.Format(@"select * from FAQ order by Id desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<FAQ> obj = new PagedList<FAQ>(DAL.PagedListDAL<FAQ>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static IEnumerable<FAQ> GetList()
        {
            return FAQDAL.GetList();
        }

        public static int Add(FAQ model)
        {
            return FAQDAL.Add(model);
        }

        public static int Delete(FAQ model)
        {
            return FAQDAL.Delete(model);
        }


        public static FAQ GetModelByID(FAQ model)
        {
            return FAQDAL.GetModelByID(model);
        }

        public static int Update(FAQ model)
        {
            return FAQDAL.Update(model);
        }
    }
}
