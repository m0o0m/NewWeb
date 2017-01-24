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

        public static int AddStrongPushADRecord(StrongPushADRecord model)
        {
            return StrongPushADDAL.AddStrongPushADRecord(model);
        }

        public static PagedList<StrongPushADRecord> GetStrongPushADRecord(LoginRegisterDataView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

            pq.RecordCount = DAL.PagedListDAL<FreezeLog>.GetRecordCount(string.Format(@"
select count(0) from  "+database3+ @".strongpushadrecord where  Plat ={0} and Agent={1} ",
model.Platform, model.Channels
), sqlconnectionString);


            pq.Sql = string.Format(@"
select * from " + database3 + @".strongpushadrecord 
where  Plat ={2} and Agent={3}
 ORDER BY CreateTime DESC
limit {0}, {1}
",
pq.StartRowNumber,
pq.PageSize,

model.Platform,model.Channels);


            PagedList<StrongPushADRecord> obj = new PagedList<StrongPushADRecord>(DAL.PagedListDAL<StrongPushADRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        public static StrongPushAD GetStrongPushAD(LoginRegisterDataView model)
        {
            return StrongPushADDAL.GetStrongPushAD(model);
        }

        public static int AddStrongPushAD(LoginRegisterDataView model)
        {
            return StrongPushADDAL.AddStrongPushAD(model);
        }

        public static int UpdateStrongPushAD(LoginRegisterDataView model)
        {
            
            StrongPushAD ad = GetStrongPushAD(model);
            if (ad == null)
            {
                return StrongPushADDAL.AddStrongPushAD(model);
            }
            else {
                return StrongPushADDAL.UpdateStrongPushAD(model);
            }

        }
    }
}
