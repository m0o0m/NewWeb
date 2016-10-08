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
    public class RedisterIPBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static PagedList<RedisterIP> GetListByPage(int page, ValueView model, string StartDate, string ExpirationDate)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;

            if (string.IsNullOrWhiteSpace(model.Value))
            {
                pq.RecordCount = DAL.PagedListDAL<RedisterIP>.GetRecordCount(string.Format(@"select count(0) from BG_RedisterIP"), sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_RedisterIP order by ModifyTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<RedisterIP>.GetRecordCount(string.Format(@"select count(0) from BG_RedisterIP where IP = '{0}'", model.Value), sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_RedisterIP where IP = '{2}' order by ModifyTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, model.Value);
            }

            PagedList<RedisterIP> obj = new PagedList<RedisterIP>(DAL.PagedListDAL<RedisterIP>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

    }
}
