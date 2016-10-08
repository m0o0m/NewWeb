using GL.Common;
using GL.Data.AWeb.Identity;
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
    public class AgentUserBLL
    {

        public static PagedList<AgentUser> GetListByPage(int page, int id)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<AgentUser>.GetRecordCount(string.Format(@"select count(0) from AgentUsers where HigherLevel = {0}", id));
            pq.Sql = string.Format(@"select * from AgentUsers where HigherLevel = {2} order by Id desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, id);

            PagedList<AgentUser> obj = new PagedList<AgentUser>(DAL.PagedListDAL<AgentUser>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static int GetRecordCount(int id)
        {
            return DAL.PagedListDAL<AgentUser>.GetRecordCount(string.Format(@"select count(0) from AgentUsers where HigherLevel = {0}", id));
        }


    }
}
