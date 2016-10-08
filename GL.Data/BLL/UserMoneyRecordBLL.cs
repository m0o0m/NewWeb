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
    public class UserMoneyRecordBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static PagedList<UserMoneyRecord> GetListByPage(GameRecordView grv)
        {
           
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
            //            pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"
            //select count(0) from record.BG_UserMoneyRecord where UserID in (select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}') and CreateTime between '{1}' and '{2}' ", grv.SearchExt, grv.StartDate, grv.ExpirationDate), sqlconnectionString);




            pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"
            select count(0) from record.BG_UserMoneyRecord where UserID = '{0}' and ScoreChange<>0 and CreateTime between '{1}' and '{2}' ", grv.SearchExt, grv.StartDate, grv.ExpirationDate), sqlconnectionString);




            //pq.Sql = string.Format(@"select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper, r.NickName as UserName from (select * from record.BG_UserMoneyRecord where UserID  = (select ID from Role where ID = '{2}' or Account= '{2}' or NickName= '{2}' limit 1) and CreateTime between '{3}' and '{4}' order by CreateTime desc limit {0}, {1}) as u left join record.S_Desc s on u.Type = s.Type left join Role as r on u.UserID = r.ID", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
            pq.Sql = string.Format(@"select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper
                   from (
                        select a.* ,b.NickName as UserName from record.BG_UserMoneyRecord a
                        join (select ID ,Nickname from 515game.Role where ID = '{2}' ) b on a.userid = b.id
                        where CreateTime between '{3}' and '{4}' and ScoreChange<>0 order by CreateTime desc limit {0}, {1}
                    ) as u left join record.S_Desc s on u.Type = s.Type 
                    ", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);


            PagedList<UserMoneyRecord> obj = new PagedList<UserMoneyRecord>(DAL.PagedListDAL<UserMoneyRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static PagedList<UserMoneyRecord> GetListByPage(GameRecordView grv,out int recordCount)
        {

            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
            //            pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"
            //select count(0) from record.BG_UserMoneyRecord where UserID in (select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}') and CreateTime between '{1}' and '{2}' ", grv.SearchExt, grv.StartDate, grv.ExpirationDate), sqlconnectionString);




            pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
            select count(0) from record.BG_UserMoneyRecord where UserID = '{0}' and type not in (41,42, 43, 52, 53, 54, 77,100,122,163) and CreateTime between '{1}' and '{2}' ", grv.SearchExt, grv.StartDate, grv.ExpirationDate), sqlconnectionString);





            //pq.Sql = string.Format(@"select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper, r.NickName as UserName from (select * from record.BG_UserMoneyRecord where UserID  = (select ID from Role where ID = '{2}' or Account= '{2}' or NickName= '{2}' limit 1) and CreateTime between '{3}' and '{4}' order by CreateTime desc limit {0}, {1}) as u left join record.S_Desc s on u.Type = s.Type left join Role as r on u.UserID = r.ID", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
            pq.Sql = string.Format(@"SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;select u.* ,ifnull(u.UserOper ,concat(u.Type,'未定义描述')) UserOper
                    from (
                      select a.* ,b.NickName as UserName ,s.UserOper from record.BG_UserMoneyRecord a 
                        join (select ID ,Nickname from 515game.Role where ID = '{2}') b on a.userid = b.id
                        left join record.S_Desc s on a.Type = s.Type 
                      where a.CreateTime between '{3}' and '{4}' and a.type not in (41,42, 43, 52, 53, 54, 77,100,122,163)  order by a.CreateTime desc ,s.OrderNo desc limit {0}, {1}
                    ) u  
                    ", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
            PagedList<UserMoneyRecord> obj = new PagedList<UserMoneyRecord>(DAL.PagedListDAL<UserMoneyRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            recordCount = pq.RecordCount;
            return obj;
        }

        public static PagedList<UserMoneyRecord> Get007ListByPage(GameRecordView grv, out int recordCount)
        {

            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
            //            pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"
            //select count(0) from record.BG_UserMoneyRecord where UserID in (select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}') and CreateTime between '{1}' and '{2}' ", grv.SearchExt, grv.StartDate, grv.ExpirationDate), sqlconnectionString);




            pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
            select count(0) from record.BG_UserMoneyRecord where UserID in (select ID from 515game.Role where  agent = 10010) and CreateTime between '{1}' and '{2}' and Type in (126,138) ", grv.SearchExt, grv.StartDate, grv.ExpirationDate), sqlconnectionString);





            //pq.Sql = string.Format(@"select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper, r.NickName as UserName from (select * from record.BG_UserMoneyRecord where UserID  = (select ID from Role where ID = '{2}' or Account= '{2}' or NickName= '{2}' limit 1) and CreateTime between '{3}' and '{4}' order by CreateTime desc limit {0}, {1}) as u left join record.S_Desc s on u.Type = s.Type left join Role as r on u.UserID = r.ID", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
            pq.Sql = string.Format(@"SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper
from (
  select a.* ,b.NickName as UserName from record.BG_UserMoneyRecord a
    join 515game.Role b on a.userid = b.id and b.Agent=10010
  where a.CreateTime between '{3}' and '{4}' and Type in(126 ,138) order by CreateTime desc limit {0}, {1}
) as u left join record.S_Desc s on u.Type = s.Type 
                    ", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
            PagedList<UserMoneyRecord> obj = new PagedList<UserMoneyRecord>(DAL.PagedListDAL<UserMoneyRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
          

            recordCount = pq.RecordCount;
            return obj;
        }


        public static PagedList<UserMoneyRecord> GetListByPage(GameRecordView grv,int recordCount)
        {

            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
            //            pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"
            //select count(0) from record.BG_UserMoneyRecord where UserID in (select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}') and CreateTime between '{1}' and '{2}' ", grv.SearchExt, grv.StartDate, grv.ExpirationDate), sqlconnectionString);

            pq.RecordCount = recordCount;

            //pq.Sql = string.Format(@"select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper, r.NickName as UserName from (select * from record.BG_UserMoneyRecord where UserID  = (select ID from Role where ID = '{2}' or Account= '{2}' or NickName= '{2}' limit 1) and CreateTime between '{3}' and '{4}' order by CreateTime desc limit {0}, {1}) as u left join record.S_Desc s on u.Type = s.Type left join Role as r on u.UserID = r.ID", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
            pq.Sql = string.Format(@"SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;select u.* , ifnull(u.UserOper , concat(u.Type,'未定义描述')) UserOper
                      from(
                        select a.* , b.NickName as UserName , s.UserOper from record.BG_UserMoneyRecord a 
                          join (select ID, Nickname from 515game.Role where ID = '{2}') b on a.userid = b.id
                          left join record.S_Desc s on a.Type = s.Type
                        where a.CreateTime between '{3}' and '{4}' and a.type not in (41,42, 43, 52, 53, 54, 77,100,122,163)  order by a.CreateTime desc, s.OrderNo desc limit {0}, {1}
                    ) u
                            ", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
            PagedList<UserMoneyRecord> obj = new PagedList<UserMoneyRecord>(DAL.PagedListDAL<UserMoneyRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }
        

        public static PagedList<UserMoneyRecord> Get007ListByPage(GameRecordView grv, int recordCount)
        {

            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
            //            pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"
            //select count(0) from record.BG_UserMoneyRecord where UserID in (select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}') and CreateTime between '{1}' and '{2}' ", grv.SearchExt, grv.StartDate, grv.ExpirationDate), sqlconnectionString);

            pq.RecordCount = recordCount;

            //pq.Sql = string.Format(@"select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper, r.NickName as UserName from (select * from record.BG_UserMoneyRecord where UserID  = (select ID from Role where ID = '{2}' or Account= '{2}' or NickName= '{2}' limit 1) and CreateTime between '{3}' and '{4}' order by CreateTime desc limit {0}, {1}) as u left join record.S_Desc s on u.Type = s.Type left join Role as r on u.UserID = r.ID", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
            pq.Sql = string.Format(@"select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper
                            from (
                              select a.* ,b.NickName as UserName from record.BG_UserMoneyRecord a
                                join 515game.Role b on b.Agent=10010 and a.userid = b.id
                              where a.CreateTime between '{3}' and '{4}' and Type in(126,138) order by b.CreateTime desc limit {0}, {1}
                            ) as u left join record.S_Desc s on u.Type = s.Type 
                            ", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate);
          
            PagedList<UserMoneyRecord> obj = new PagedList<UserMoneyRecord>(DAL.PagedListDAL<UserMoneyRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }



        public static PagedList<UserMoneyRecord> GetListByPageForAgent(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;


            if (!string.IsNullOrEmpty(grv.SearchExt))
            {
                pq.RecordCount = DAL.PagedListDAL<UserMoneyRecord>.GetRecordCount(string.Format(@"
            select count(0) from record.BG_UserMoneyRecord where UserID = (select ID from Role where ID = '{0}' or Account= '{0}' or NickName= '{0}' and find_in_set(Agent, '{3}') limit 1) and CreateTime between '{1}' and '{2}' ", grv.SearchExt, grv.StartDate, grv.ExpirationDate, grv.UserList), sqlconnectionString);

                pq.Sql = string.Format(@"select u.* ,ifnull(s.UserOper ,concat(u.Type,'未定义描述')) UserOper
                    from (select a.* ,b.NickName as UserName from record.BG_UserMoneyRecord a
                        join (select ID ,Nickname from 515game.Role where find_in_set(Agent, '{5}') and (ID = '{2}' or Account= '{2}' or NickName= '{2}') limit 1) b on a.userid = b.id
                        where CreateTime between '{3}' and '{4}' order by CreateTime desc limit {0}, {1}
                    ) as u left join record.S_Desc s on u.Type = s.Type;", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate, grv.UserList);

            }
            else
            {
                return new PagedList<UserMoneyRecord>(new List<UserMoneyRecord>(), 1, 1);
            }

            PagedList<UserMoneyRecord> obj = new PagedList<UserMoneyRecord>(DAL.PagedListDAL<UserMoneyRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        public static UserInfo GetUserInfo(GameRecordView grv)
        {
            return UserMoneyRecordDAL.GetUserInfo(grv);
        }

        public static UserInfo GetUserInfo007(GameRecordView grv)
        {
            return UserMoneyRecordDAL.GetUserInfo007(grv);
        }

        public static decimal GetUserInfoScore(GameRecordView grv)
        {
            return UserMoneyRecordDAL.GetUserInfoScore(grv);
        }

        public static long GetUserInfoService(GameRecordView grv)
        {
            return UserMoneyRecordDAL.GetUserInfoService(grv);
        }
    }
}
