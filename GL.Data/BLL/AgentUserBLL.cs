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
            pq.RecordCount = DAL.PagedListDAL<AgentUser>.GetRecordCount(string.Format(@"select count(0) from AgentUsers where HigherLevel = {0} or Id = {0}", id));
            pq.Sql = string.Format(@"select * from AgentUsers where HigherLevel = {2} or Id = {2} order by HigherLevel asc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, id);

            PagedList<AgentUser> obj = new PagedList<AgentUser>(DAL.PagedListDAL<AgentUser>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">页号</param>
        /// <param name="mid">上级ID</param>
        /// <param name="id">查询ID</param>
        /// <param name="pq">分页信息</param>
        /// <returns></returns>
        public static IEnumerable<AgentUser> GetListByPage(int pageid, int pagesize, int mid, int id, out PagerQuery pq)
        {
            pq = new PagerQuery();
            pq.StartItemIndex = pageid;
            pq.PageSize = pagesize;
            pq.RecordCount = GetRecordCount(id);
            pq.Sql = string.Format(@"select * from AgentUsers where HigherLevel = @ID or Id = @ID order by HigherLevel asc limit @StartRowNumber, @PageSize");
            //pq.Sql = string.Format(@"select * from AgentUsers order by Id desc limit @StartRowNumber, @PageSize");
            return AgentUserDAL.GetListByPage(pq, mid, id);
        }

        public static int GetRecordCount(int id)
        {
            return AgentUserDAL.GetRecordCount(id);
            
        }

        public static bool CheckUser(int mid, int id)
        {
            return AgentUserDAL.CheckUser(mid, id);
        }

        public static IEnumerable<AgentUser> GetUserList(string idlist)
        {
            return AgentUserDAL.GetUserList(idlist);
        }

        public static AgentUser GetModel(int agentID) {
            return AgentUserDAL.GetModel(agentID);
        }

        public static string GetUserListString(int masterID)
        {
            return AgentUserDAL.GetUserListString(masterID);
        }

    }
}
