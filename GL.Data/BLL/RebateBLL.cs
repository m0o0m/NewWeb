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
    public class RebateBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");

        public static PagedList<Rebate> GetListByPage(GameRecordView bdv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;

            if (string.IsNullOrEmpty(bdv.SearchExt))
            {
                bdv.SearchExt = "0";
            }
            pq.RecordCount = PagedListDAL<Rebate>.GetRecordCount(string.Format(@"
                select count(0) from record.BG_PointRecord where (Type = 1 or Type=2) and CreateTime>='" + bdv.StartDate + "' and CreateTime<'" + bdv.ExpirationDate + "' and  UserID=" + bdv.SearchExt
            ), sqlconnectionString);
            //pq.Sql = string.Format(@"select * from 515game.SpellCardRecord where Time>='" + bdv.StartDate + "' and Time<'" + bdv.ExpirationDate + "'  {2}  order by Time desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, string.IsNullOrEmpty(bdv.SearchExt) ? "" : " and PlayerID='" + bdv.SearchExt + "'");


            //pq.RecordCount = 20;
            pq.Sql = string.Format(@"
                select b.CreateTime ,a.id as UserID ,NickName ,b.PointNum TotalRebate ,b.PointChange ChangeRebate ,case b.Type when 1 then '德州获取' when 2 then '德州消耗' else '' end as Oper
                from (select id ,nickname from 515game.Role a where a.id = {2}) a
                    join record.BG_PointRecord b on a.id = b.UserID and b.CreateTime >= '{3}' and b.CreateTime < '{4}' and (b.Type = 1 or b.Type = 2 )
                order by CreateTime desc limit {0}, {1}"
            , pq.StartRowNumber, pq.PageSize ,bdv.SearchExt ,bdv.StartDate ,bdv.ExpirationDate);


            PagedList<Rebate> obj = new PagedList<Rebate>(PagedListDAL<Rebate>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

    }
}
