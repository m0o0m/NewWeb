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
    public class TexasPotLogBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static PagedList<TexasPotLog> GetListByPage(GameRecordView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;



            pq.RecordCount = PagedListDAL<TexasPotLog>.GetRecordCount(@"select count(0) from TexasPotLog ", sqlconnectionString);
            pq.Sql = string.Format(@"select * from TexasPotLog order by Time desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate);



            PagedList<TexasPotLog> obj = new PagedList<TexasPotLog>(PagedListDAL<TexasPotLog>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

        public static void Add(TexasPotLog model) {
            TexasPotLogDAL.Add(model);
        }

    }
}
