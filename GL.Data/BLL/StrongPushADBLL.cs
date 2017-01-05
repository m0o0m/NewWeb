using GL.Command.DBUtility;
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
    public class StrongPushADBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");
        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        public static IEnumerable<StrongPushADRecord> GetStrongPushADRecord(BaseDataView bdv)
        {
            return StrongPushADDAL.GetStrongPushADRecord(bdv);
        }


        public static PagedList<StrongPushADRecord> GetStrongPushADRecord(LoginRegisterDataView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

            pq.RecordCount = DAL.PagedListDAL<FreezeLog>.GetRecordCount(string.Format(@"
select count(0) from  "+database3+@".strongpushadrecord"
), sqlconnectionString);


            pq.Sql = string.Format(@"
select * from " + database3 + @".strongpushadrecord ORDER BY CreateTime DESC
limit {0}, {1}
",
pq.StartRowNumber,
pq.PageSize);


            PagedList<StrongPushADRecord> obj = new PagedList<StrongPushADRecord>(DAL.PagedListDAL<StrongPushADRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

    }
}
