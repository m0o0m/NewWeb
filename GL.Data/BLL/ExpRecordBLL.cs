using GL.Command.DBUtility;
using GL.Common;
using GL.Dapper;
using GL.Data.DAL;
using GL.Data.Model;
using GL.Data.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class ExpRecordBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");
        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        public static PagedList<ItemRecord> GetItemRecordList(GameRecordView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

            pq.RecordCount = PagedListDAL<ItemRecord>.GetRecordCount(string.Format(@"select count(0) from "+ database3 + @".BG_ItemRecord 
                where CreateTime between '{0}' and '{1}' and UserID = case when {2} = 0 then UserID else {2} end and ItemID = case when {3} = 0 then ItemID else {3} end "
            , model.StartDate, model.ExpirationDate, model.SearchExt, model.ItemID), sqlconnectionString);

            pq.Sql = string.Format(@"select aa.CreateTime, aa.UserID , Role.NickName as UserName ,ifnull(c.UserOper ,aa.ChangeType) ChangeType 
  ,ifnull(t.TempalteName ,aa.ItemID) TemplateName ,aa.NowNum ,aa.OldNum ,aa.ItemID ItemName  
from (
    select * from "+ database3 + @".BG_ItemRecord where CreateTime between '{2}' and '{3}' and UserID = case when {4} = 0 then UserID else {4} end 
      and ItemID = case when {5} = 0 then ItemID else {5} end order by CreateTime desc limit {0}, {1}
) as aa 
  join Role on aa.UserID = Role.ID left join "+ database3 + @".S_Desc c on aa.ChangeType = c.Type
  left join "+ database1+ @".S_Template t on aa.ItemID = t.TempalteID and t.TemplateType = 0 ;
"
            , pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate, model.SearchExt ,model.ItemID);

            PagedList<ItemRecord> obj = new PagedList<ItemRecord>(PagedListDAL<ItemRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static PagedList<ExpRecord> GetListByPage(GameRecordView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

            if (!string.IsNullOrEmpty(model.SearchExt))
            {
                pq.RecordCount = PagedListDAL<ExpRecord>.GetRecordCount(string.Format(@"select count(0) from "+ database3 + ".BG_ExpRecord where CreateTime between '{0}' and '{1}' and UserID in (select ID from Role where ID = '{2}' or Account='{2}' or NickName='{2}')", model.StartDate, model.ExpirationDate, model.SearchExt), sqlconnectionString);

                pq.Sql = string.Format(@"select aa.*, Role.NickName as UserName from (select * from "+ database3 + @".BG_ExpRecord where CreateTime between '{2}' and '{3}' and UserID in (select ID from Role where ID = '{4}' or Account='{4}' or NickName='{4}') order by CreateTime desc limit {0}, {1}) as aa inner join Role on aa.UserID = Role.ID", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate, model.SearchExt);

            }
            else
            {
                pq.RecordCount = PagedListDAL<ExpRecord>.GetRecordCount(string.Format(@"select count(0) from "+ database3 + @".BG_ExpRecord where CreateTime between '{0}' and '{1}'", model.StartDate, model.ExpirationDate), sqlconnectionString);
                pq.Sql = string.Format(@"select aa.*, Role.NickName as UserName from (select * from "+ database3 + @".BG_ExpRecord where CreateTime between '{2}' and '{3}' order by CreateTime desc limit {0}, {1}) as aa inner join Role on aa.UserID = Role.ID", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate);
            }



            PagedList<ExpRecord> obj = new PagedList<ExpRecord>(PagedListDAL<ExpRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }


        public static PagedList<ExpRecord> GetListByPageForAgent(GameRecordView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

            if (!string.IsNullOrEmpty(model.SearchExt))
            {
                pq.RecordCount = PagedListDAL<ExpRecord>.GetRecordCount(string.Format(@"select count(0) from "+ database3 + @".BG_ExpRecord where CreateTime between '{0}' and '{1}' and UserID in (select ID from Role where (ID = '{2}' or Account='{2}' or NickName='{2}') and find_in_set(Agent, '{3}'))", model.StartDate, model.ExpirationDate, model.SearchExt, model.UserList), sqlconnectionString);

                pq.Sql = string.Format(@"select aa.*, b.NickName as UserName from "+ database3 + @".BG_ExpRecord aa inner join Role b on aa.UserID = b.ID and (b.ID = '{4}' or b.Account='{4}' or b.NickName='{4}') and find_in_set(b.Agent, '{5}') where aa.CreateTime between '{2}' and '{3}' order by aa.CreateTime desc limit {0}, {1};", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate, model.SearchExt, model.UserList);

            }
            else
            {
                return new PagedList<ExpRecord>(new List<ExpRecord>(), 1, 1); 
            }



            PagedList<ExpRecord> obj = new PagedList<ExpRecord>(PagedListDAL<ExpRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

        public static List<ItemGroup> GetItemGroupList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<ItemGroup> i = cn.Query<ItemGroup>(@"
                    select 410 as ItemID ,'VIP体验卡' as ItemName ,410 as TypeList 
                    union all select 400 ,'绿宝石' ,400 
                    union all select 401 ,'蓝宝石' ,401  ; 
                ");
                cn.Close();
                return i.ToList();
            }
            //return GL.Data.DAL.ExpRecordDAL.GetItemGroupList();
        }
    }

}
