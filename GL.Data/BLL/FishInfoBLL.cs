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
    public class FishInfoBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static PagedList<FishInfo> GetListByPage(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
            if (string.IsNullOrEmpty(grv.SearchExt))
            {
                pq.RecordCount = PagedListDAL<FishInfo>.GetRecordCount(string.Format(@"select count(0) from FishInfoRecord a join 515game.Role r on a.UserID = r.id and r.agent <> 10010 where a.CreateTime between '{0}' and '{1}'", grv.StartDate, grv.ExpirationDate), sqlconnectionString);
                pq.Sql = string.Format(@"select a.* from FishInfoRecord a join 515game.Role r on a.UserID = r.id  and r.agent <> 10010 where a.CreateTime between '{2}' and '{3}' order by a.CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate);
            }
            else
            {
                pq.RecordCount = PagedListDAL<FishInfo>.GetRecordCount(string.Format(@"select count(0) from FishInfoRecord as f,Role as r where  r.agent <> 10010 and f.UserID = r.ID and ( r.ID ='{2}' or r.NickName='{2}' or r.Account='{2}' ) and f.CreateTime between '{0}' and '{1}'", grv.StartDate, grv.ExpirationDate, grv.SearchExt), sqlconnectionString);
                pq.Sql = string.Format(@"select f.* from FishInfoRecord as f,Role as r where  r.agent <> 10010 and f.UserID = r.ID and ( r.ID ='{4}' or r.NickName='{4}' or r.Account='{4}' ) and f.CreateTime between '{2}' and '{3}' order by f.CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate, grv.SearchExt);
            }

            PagedList<FishInfo> obj = new PagedList<FishInfo>(PagedListDAL<FishInfo>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

        public static PagedList<FishInfo> GetListByFish(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;

            if (grv.commandID == 5)
            {
                pq.RecordCount = PagedListDAL<FishInfo>.GetRecordCount(string.Format(@"
                    select count(0) from FishInfoRecord a join 515game.Role r on a.UserID = r.id and  r.agent <> 10010 where a.CreateTime between '{0}' and '{1}' and FishID = {2} and Type < 3 and Flag = 1 and a.UserID = case when '{4}' = '' then a.UserID else '{4}' end "
                    , grv.StartDate, grv.ExpirationDate, grv.ItemID, grv.commandID, grv.SearchExt), sqlconnectionString);
                pq.Sql = string.Format(@"
                    select * from FishInfoRecord a join 515game.Role r on a.UserID = r.id  and r.agent <> 10010 where a.CreateTime between '{2}' and '{3}' and a.FishID = {4} and a.Type < 3 and a.Flag = 1 and a.UserID = case when '{5}' = '' then a.UserID else '{5}' end order by a.CreateTime desc limit {0}, {1}"
                    , pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate, grv.ItemID, grv.SearchExt);
            }
            else
            {
                pq.RecordCount = PagedListDAL<FishInfo>.GetRecordCount(string.Format(@"
                    select count(0) from FishInfoRecord as f join 515game.Role r on f.UserID = r.id and r.agent <> 10010 where f.CreateTime between '{0}' and '{1}' and f.FishID = {3} and f.Type = {4} and UserID = case when '{2}' = '' then UserID else '{2}' end"
                    , grv.StartDate, grv.ExpirationDate, grv.SearchExt, grv.ItemID, grv.commandID), sqlconnectionString);
                pq.Sql = string.Format(@"
                    select f.* from FishInfoRecord as f join 515game.Role r on f.UserID = r.id and r.agent <> 10010 where  UserID = case when '{4}' = '' then UserID else '{4}' end 
                    and f.CreateTime between '{2}' and '{3}' and f.FishID = {5} and f.Type = {6} order by f.CreateTime desc limit {0}, {1}"
                    , pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate, grv.SearchExt, grv.ItemID, grv.commandID);
            }

            //if (string.IsNullOrEmpty(grv.SearchExt))
            //{
            //    pq.RecordCount = PagedListDAL<FishInfo>.GetRecordCount(string.Format(@"
            //        select count(0) from FishInfoRecord where CreateTime between '{0}' and '{1}' and FishID = {2} and Type = {3}"
            //        , grv.StartDate, grv.ExpirationDate ,grv.ItemID ,grv.commandID), sqlconnectionString);
            //    pq.Sql = string.Format(@"
            //        select * from FishInfoRecord where CreateTime between '{2}' and '{3}' and FishID = {4} and Type = {5} order by CreateTime desc limit {0}, {1}"
            //        , pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate, grv.ItemID, grv.commandID);
            //}
            //else
            //{
            //    pq.RecordCount = PagedListDAL<FishInfo>.GetRecordCount(string.Format(@"
            //        select count(0) from FishInfoRecord as f,Role as r where f.UserID = r.ID and ( r.ID ='{2}' or r.NickName='{2}' or r.Account='{2}' ) 
            //        and f.CreateTime between '{0}' and '{1}' and f.FishID = {3} and f.Type = {4}"
            //        , grv.StartDate, grv.ExpirationDate, grv.SearchExt, grv.ItemID, grv.commandID), sqlconnectionString);
            //    pq.Sql = string.Format(@"
            //        select f.* from FishInfoRecord as f,Role as r where f.UserID = r.ID and ( r.ID ='{4}' or r.NickName='{4}' or r.Account='{4}' ) 
            //        and f.CreateTime between '{2}' and '{3}' and f.FishID = {5} and f.Type = {6} order by f.CreateTime desc limit {0}, {1}"
            //        , pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate, grv.SearchExt ,grv.ItemID, grv.commandID);
            //}

            PagedList<FishInfo> obj = new PagedList<FishInfo>(PagedListDAL<FishInfo>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }


        public static string GetGiveAcc(int id)
        {
            return DAL.RoleDAL.GetGiveAcc(id);
        }
        public static IEnumerable<FishCount> GetFishCount(GameRecordView vbd)
        {
            return DAL.FishInfoDAL.GetFishCount(vbd);
        }
        public static IEnumerable<UserFishInfo> GetUserInfo(GameRecordView vbd)
        {
            return DAL.FishInfoDAL.GetUserInfo(vbd);
        }

        public static PagedList<FishInfo> FishCountOnUser(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
            if (string.IsNullOrEmpty(grv.SearchExt))
            {
                pq.RecordCount = PagedListDAL<FishInfo>.GetRecordCount(string.Format(@"select count(0) from FishInfoRecord where Type < 3 and Flag = 1 and CreateTime between '{0}' and '{1}'", grv.StartDate, grv.ExpirationDate), sqlconnectionString);
                //pq.Sql = string.Format(@"select * from FishInfoRecord where Type < 3 and Flag = 1 and  CreateTime between '{2}' and '{3}' order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate);


                pq.Sql = string.Format(@"select * from (
select *,
         CASE FishID
            WHEN 4 THEN 10
            WHEN 3 THEN 20
            WHEN 2 THEN 30
            WHEN 1 THEN 40
            WHEN 5 THEN 50
            ELSE 60
         END FishNameOrder from FishInfoRecord 
where Type < 3 and Flag = 1 and  CreateTime between '{2}' and '{3}'
) t
order by t.FishNameOrder,t.CreateTime desc  limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate);
            }
            else
            {
                pq.RecordCount = PagedListDAL<FishInfo>.GetRecordCount(string.Format(@"select count(0) from FishInfoRecord as f,Role as r where f.UserID = r.ID and f.Type < 3 and f.Flag = 1 and ( r.ID ='{2}' or r.NickName='{2}' or r.Account='{2}' ) and f.CreateTime between '{0}' and '{1}'", grv.StartDate, grv.ExpirationDate, grv.SearchExt), sqlconnectionString);
               // pq.Sql = string.Format(@"select * from FishInfoRecord where Type < 3 and Flag = 1 and UserID = {4} and CreateTime between '{2}' and '{3}' order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate, grv.UserID);

                pq.Sql = string.Format(@"select * from (
select f.*,
         CASE f.FishID
            WHEN 4 THEN 10
            WHEN 3 THEN 20
            WHEN 2 THEN 30
            WHEN 1 THEN 40
            WHEN 5 THEN 50
            ELSE 60
         END FishNameOrder from FishInfoRecord as f,Role as r
where f.UserID = r.ID and f.Type < 3 and f.Flag = 1 and ( r.ID ='{4}' or r.NickName='{4}' or r.Account='{4}' ) and f.CreateTime between '{2}' and '{3}'
) t
order by t.FishNameOrder,t.CreateTime desc  limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.StartDate, grv.ExpirationDate, grv.SearchExt);

            }

            PagedList<FishInfo> obj = new PagedList<FishInfo>(PagedListDAL<FishInfo>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

    }
}
