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
    public class DaiLiBLL
    {

        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionString");
        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


        public static DailiKuCun GetDaiLiKuCun() {
            return DaiLiDAL.GetDaiLiKuCun();
        }
        public static DailiKuCun GetDaiLiKuCun(int no)
        {
            return DaiLiDAL.GetDaiLiKuCun(no);
        }

        public static int UpdateDaiLiKuCun(int no, Int64 Gold, DaiLiType type = DaiLiType.充值库存)
        {
            return DaiLiDAL.UpdateDaiLiKuCun( no,Gold,type);
        }


        public static IEnumerable<DaiLiUsers> GetDaiLiUsers()
        {
            return DaiLiDAL.GetDaiLiUsers();
        }

        public static DaiLiUsers GetDaiLiUsers(int no)
        {
            return DaiLiDAL.GetDaiLiUsers(no);
        }

        public static IEnumerable<S_Desc> GetFlowDesc(int no)
        {
            return DaiLiDAL.GetFlowDesc(no);
        }

        public static int UpdateFlowDesc(int no, string flowNos) {
            return DaiLiDAL.UpdateFlowDesc(no,flowNos);
        }

        public static int InsertKuCunFlow(KuCunFlow model)
        {
            return DaiLiDAL.InsertKuCunFlow(model);
        }


        public static int InsertdailiFlowRecord(DailiFlowRecord model)
        {
            return DaiLiDAL.InsertdailiFlowRecord(model);
        }
        public static PagedList<KuCunFlow> GetKuCunListByPage(int page,int dailiNo)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<KuCunFlow>.GetRecordCount(@"select count(0) from dailirecord", sqlconnectionString);
            pq.Sql = string.Format(@"select * from dailirecord where DaiLiNo = {2} order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize,dailiNo);

            PagedList<KuCunFlow> obj = new PagedList<KuCunFlow>(DAL.PagedListDAL<KuCunFlow>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

    }
}
