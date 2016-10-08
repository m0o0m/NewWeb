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
    public class ScaleRecordBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");

        public static PagedList<ScaleRecord> GetListByPage(int page)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;

            pq.RecordCount = PagedListDAL<ScaleRecord>.GetRecordCount(string.Format(@"select count(0) from BG_ScaleRecord"), sqlconnectionString);
            pq.Sql = string.Format(@"select * from BG_ScaleRecord order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);


            PagedList<ScaleRecord> obj = new PagedList<ScaleRecord>(PagedListDAL<ScaleRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }


        public static PagedList<ScaleRecord> GetListByPage(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
      
            pq.RecordCount = PagedListDAL<ScaleRecord>.GetRecordCount(string.Format(@"
select count(*) from (
select UserID, GameID, Proj,Content,CreateTime 
from record.BG_ScaleRecord  
where CreateTime>='" + grv.StartDate + @"' and CreateTime<'" + grv.ExpirationDate + @"'  
) as a 
where a.CreateTime>='" + grv.StartDate + @"' and a.CreateTime<'" + grv.ExpirationDate + @"' {2} {3} 
", pq.StartRowNumber, pq.PageSize, grv.UserID <= 0 ? "" : " and a.UserID=" + grv.UserID, grv.SearchExt == "" ? "" : " and a.Proj='" + grv.SearchExt + "'"), sqlconnectionString);
            pq.Sql = string.Format(@"
select a.*,b.NickName as UserName from (
select  UserID, GameID, Proj,Content,CreateTime,UpdateValue
from record.BG_ScaleRecord  
where CreateTime>='" + grv.StartDate+@"' and CreateTime<'"+grv.ExpirationDate+ @"'  

) as a LEFT JOIN 515game.Role as b on a.UserID = b.ID 
where a.CreateTime>='" + grv.StartDate+ @"' and a.CreateTime<'" + grv.ExpirationDate+@"' {2} {3}
ORDER BY a.CreateTime  DESC
limit {0}, {1}
", pq.StartRowNumber, pq.PageSize, grv.UserID<=0?"":" and a.UserID="+grv.UserID,grv.SearchExt==""?"":" and a.Proj='"+grv.SearchExt+"'");


            PagedList<ScaleRecord> obj = new PagedList<ScaleRecord>(PagedListDAL<ScaleRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }


        public static LoginRecord GetLoginRecord(int userid, string startTime, string endTime) {
            return DAL.LoginRecordDAL.GetModel( userid,  startTime,  endTime);
        }



        public static PagedList<ModelBaseData> GetModelByPage(int page ,string name)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 50;

            string CountSql = string.Format(@"select count(0) from S_DataModel where locate('{0}' ,ModelName) > 0", name);
            pq.RecordCount = PagedListDAL<ModelBaseData>.GetRecordCount(CountSql, sqlconnectionString);
            pq.Sql = string.Format(@"select * from S_DataModel where locate('{2}' ,ModelName) > 0 order by checkcount desc ,id desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize ,name);

            PagedList<ModelBaseData> obj = new PagedList<ModelBaseData>(PagedListDAL<ModelBaseData>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

        public static PagedList<ModelBaseData> GetModelByID(int id)
        {
            PagerQuery pq = new PagerQuery();
            pq.Sql = string.Format(@"select ID ,ModelName ,Model ,Para from S_DataModel where id = {0}", id);

            PagedList<ModelBaseData> obj = new PagedList<ModelBaseData>(PagedListDAL<ModelBaseData>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

        public static IEnumerable<object> GetModelData(string sql ,int id)
        {
            return PagedListDAL<object>.GetModelData(sql,id, sqlconnectionString);
        }



        public static IEnumerable<CommonIDName> GetMasterOper()
        {
            return DAL.Console.GetMasterOper();
        }


        public static int AddModel(ModelBaseData model)
        {
            return DAL.Console.AddModel(model);
        }
    }
}
