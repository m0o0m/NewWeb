using GL.Command.DBUtility;
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
    public class IPWhiteListBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static bool Add(Role model)
        {
            return DAL.IPWhiteListDAL.Add(model) > 0;
        }

        public static bool Delete(Role model)
        {
            return DAL.IPWhiteListDAL.Delete(model) > 0;
        }


        public static bool CheckWhiteIp(string ip) {
            return DAL.IPWhiteListDAL.CheckWhiteIp(ip);
        }

        //WhiteList

        public static PagedList<WhiteList> GetListByPage(BaseDataView bdv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;


            if (!string.IsNullOrEmpty(bdv.SearchExt))
            {
                pq.RecordCount = DAL.PagedListDAL<WhiteList>.GetRecordCount(string.Format(@"select count(0) from Role where CreateIP = '{0}'", bdv.SearchExt), sqlconnectionString);
                pq.Sql = string.Format(@"select DISTINCT CreateIP as IP,(select count(*) from IPWhiteList where IP='{2}') as Status from Role where CreateIP='{2}'  limit {0}, {1}", pq.StartRowNumber, pq.PageSize,bdv.SearchExt);

            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<WhiteList>.GetRecordCount(@"select count(0) from IPWhiteList   ",sqlconnectionString);
                pq.Sql = string.Format(@"select  IP,1 as Status from IPWhiteList  limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
            }

            PagedList<WhiteList> obj = new PagedList<WhiteList>(DAL.PagedListDAL<WhiteList>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


    }
}
