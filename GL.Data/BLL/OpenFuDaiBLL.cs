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
    public class OpenFuDaiBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static PagedList<OpenFuDai> GetListByPage(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;

            if (grv.UserID > 0)
            {
                pq.RecordCount = DAL.PagedListDAL<OpenFuDai>.GetRecordCount(string.Format(@"select count(0) from OpenFuDai where UserID = {0} and CreateTime between '{1}' and '{2}'", grv.UserID, grv.StartDate, grv.ExpirationDate), sqlconnectionString);
                pq.Sql = string.Format(@"select * from OpenFuDai where UserID = {2} and CreateTime between '{3}' and '{4}' order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.UserID, grv.StartDate, grv.ExpirationDate);
            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<OpenFuDai>.GetRecordCount(string.Format(@"select count(0) from OpenFuDai where CreateTime between '{1}' and '{2}'", grv.UserID, grv.StartDate, grv.ExpirationDate), sqlconnectionString);
                pq.Sql = string.Format(@"select * from OpenFuDai where CreateTime between '{3}' and '{4}' order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.UserID, grv.StartDate, grv.ExpirationDate);
            }

            PagedList<OpenFuDai> obj = new PagedList<OpenFuDai>(DAL.PagedListDAL<OpenFuDai>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static IEnumerable<OpenFuDai> GetOpenFuDai(GameRecordView grv)
        {
            return OpenFuDaiDAL.GetOpenFuDai(grv);
        }

    }
}
