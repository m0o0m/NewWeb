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
    public class AgentInfoBLL
    {
        public static PagedList<AgentInfo> GetListByPage(int page)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<AgentInfo>.GetRecordCount(@"select count(0) from AgentInfo where HigherLevel = 0");
            pq.Sql = string.Format(@"select * from AgentInfo where HigherLevel = 0 order by AgentID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<AgentInfo> obj = new PagedList<AgentInfo>(DAL.PagedListDAL<AgentInfo>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }
        public static PagedList<AgentInfo> GetListByPage(int page, int id)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<AgentInfo>.GetRecordCount(string.Format(@"select count(0) from AgentInfo where HigherLevel = {0}", id));
            pq.Sql = string.Format(@"select * from AgentInfo where HigherLevel = {2} order by AgentID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, id);

            PagedList<AgentInfo> obj = new PagedList<AgentInfo>(DAL.PagedListDAL<AgentInfo>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static int GetRecordCount(int id)
        {
            return DAL.PagedListDAL<AgentInfo>.GetRecordCount(string.Format(@"select count(0) from AgentInfo where HigherLevel = {0}", id));
        }

        public static int Add(AgentInfo ai)
        {
            return AgentInfoDAL.Add(ai);
        }

        public static int Delete(AgentInfo model)
        {
            return AgentInfoDAL.Delete(model);
        }

        public static AgentInfo GetModel(AgentInfo model)
        {
            return AgentInfoDAL.GetModel(model);
        }

        public static AgentInfo GetModelByID(AgentInfo model)
        {
            return AgentInfoDAL.GetModelByID(model);
        }

        public static AgentInfo GetModelByIDForLower(AgentInfo model)
        {
            return AgentInfoDAL.GetModelByIDForLower(model);
        }


        public static int Update(AgentInfo model)
        {
            return AgentInfoDAL.Update(model);
        }


        public static List<AgentInfo> GetAgentList()
        {
            return AgentInfoDAL.GetAgentList();
        }

        public static int ResetDifaultMain()
        {
            return AgentInfoDAL.ResetDifaultMain();
        }

        public static List<AgentInfoGroup> GetAgentGroupList() {
            return AgentInfoDAL.GetAgentGroupList();
        }
    }
}
