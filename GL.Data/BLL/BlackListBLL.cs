using GL.Command.DBUtility;
using GL.Common;
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
    public class BlackListBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static PagedList<BlackList> GetListByPageFor(int page, ValueView vv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;

            if (vv.Value != null)
            {
                pq.RecordCount = DAL.PagedListDAL<BlackList>.GetRecordCount(string.Format(@"select count(0) from BG_BanIPList where IP = '{0}'", vv.Value), sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_BanIPList where IP = '{2}'order by CreateTime limit {0}, {1}", pq.StartRowNumber, pq.PageSize, vv.Value);
            
            }else
            {
                pq.RecordCount = DAL.PagedListDAL<BlackList>.GetRecordCount(@"select count(0) from BG_BanIPList", sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_BanIPList order by CreateTime limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
            }


            PagedList<BlackList> obj = new PagedList<BlackList>(DAL.PagedListDAL<BlackList>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static PagedList<BlackList> GetListByPageForMac(int page, ValueView vv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;

            if (vv.Value != null)
            {
                pq.RecordCount = DAL.PagedListDAL<BlackList>.GetRecordCount(string.Format(@"select count(0) from BG_BanMacList where Mac = '{0}'", vv.Value), sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_BanMacList where Mac = '{2}'order by CreateTime limit {0}, {1}", pq.StartRowNumber, pq.PageSize, vv.Value);

            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<BlackList>.GetRecordCount(@"select count(0) from BG_BanMacList", sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_BanMacList order by CreateTime limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
            }


            PagedList<BlackList> obj = new PagedList<BlackList>(DAL.PagedListDAL<BlackList>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


    }
}
