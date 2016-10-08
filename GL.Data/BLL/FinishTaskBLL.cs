using GL.Command.DBUtility;
using GL.Common;
using GL.Data.DAL;
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
    public class FinishTaskBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static PagedList<FinishTask> GetListByPage(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
                                                                                                                //select f.*,r.Agent from FinishTask f, Role r where r.ID = f.UserID
                                                                                                                //(grv.Channels > 0 ? " and find_in_set(Role.Agent, {5})" : "" )
            pq.RecordCount = DAL.PagedListDAL<FinishTask>.GetRecordCount(string.Format(@"select count(0) from FinishTask f, Role r where r.ID = f.UserID and f.FinishTime between '{0}' and '{1}' "+ (grv.Channels > 0 ? " and find_in_set(r.Agent, {2})" : ""), grv.StartDate, grv.ExpirationDate,grv.UserList), sqlconnectionString);
            pq.Sql = string.Format(@"select f.* from FinishTask f, Role r where r.ID = f.UserID and  f.FinishTime between '{2}' and '{3}' "+ (grv.Channels > 0 ? " and find_in_set(r.Agent, {4})" : "") + " order by f.id desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate,grv.UserList);

            PagedList<FinishTask> obj = new PagedList<FinishTask>(PagedListDAL<FinishTask>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }
    }
}
