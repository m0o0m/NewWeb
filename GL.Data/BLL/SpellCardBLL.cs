using GL.Command.DBUtility;
using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using GL.Data.View;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class SpellCardBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");

        public static PagedList<SpellCard> GetListByPage(GameRecordView bdv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;

            pq.RecordCount = PagedListDAL<SpellCard>.GetRecordCount(string.Format(@"select count(0) from 515game.SpellCardRecord where  Time>='"+bdv.StartDate+"' and Time<'"+bdv.ExpirationDate+"' {0} ", string.IsNullOrEmpty(bdv.SearchExt)?"": " and  PlayerID='" + bdv.SearchExt + "'"), sqlconnectionString);
            pq.Sql = string.Format(@"select * from 515game.SpellCardRecord where Time>='"+bdv.StartDate+"' and Time<'"+bdv.ExpirationDate+"'  {2}  order by Time desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, string.IsNullOrEmpty(bdv.SearchExt) ? "" : " and PlayerID='" + bdv.SearchExt + "'");


            PagedList<SpellCard> obj = new PagedList<SpellCard>(PagedListDAL<SpellCard>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }
    }
}
