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
    public class ToolsUseRecordBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");

        public static PagedList<ToolsUseRecord> GetListByPage(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;
            //(grv.Channels > 0 ? " and find_in_set(Role.Agent, {3})" : "")
            if (!string.IsNullOrEmpty(grv.SearchExt))
            {
                pq.RecordCount = DAL.PagedListDAL<ToolsUseRecord>.GetRecordCount(string.Format(@"select count(0) from ToolsUseRecord as t,515game.Role as r where  t.PlayerID = r.ID and ( r.ID ='{0}' or r.NickName='{0}' or r.Account='{0}' )  and  t.UseTime between '{1}' and '{2}' "+ (grv.Channels > 0 ? " and find_in_set(r.Agent, {3})" : ""), grv.SearchExt, grv.StartDate, grv.ExpirationDate,grv.UserList), sqlconnectionString);
                pq.Sql = string.Format(@"select t.id,t.UseTime,t.PlayerID,r.NickName as UserName,t.ToolsID,t.ToolsName,t.Mobile,t.QQ,t.ExchangeStatus from ToolsUseRecord as t,515game.Role as r where t.PlayerID = r.ID and  ( r.ID ='{2}' or r.NickName='{2}' or r.Account='{2}' ) and t.UseTime between '{3}' and '{4}' " + (grv.Channels > 0 ? " and find_in_set(r.Agent, {5})" : "")  + " order by t.UseTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate,grv.UserList);
            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<ToolsUseRecord>.GetRecordCount(string.Format(@"select count(0) from ToolsUseRecord as t,515game.Role as r where  t.PlayerID = r.ID and t.UseTime between '{0}' and '{1}' "+ (grv.Channels > 0 ? " and find_in_set(r.Agent, {2})" : ""), grv.StartDate, grv.ExpirationDate,grv.UserList), sqlconnectionString);
                pq.Sql = string.Format(@"select t.id,t.UseTime,t.PlayerID,r.NickName as UserName,t.ToolsID,t.ToolsName,t.Mobile,t.QQ,t.ExchangeStatus from ToolsUseRecord as t,515game.Role as r where  t.PlayerID = r.ID and t.UseTime between '{3}' and '{4}' " + (grv.Channels > 0 ? " and find_in_set(r.Agent, {5})" : "") + " order by t.UseTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.UserID, grv.StartDate, grv.ExpirationDate,grv.UserList);
            }

            PagedList<ToolsUseRecord> obj = new PagedList<ToolsUseRecord>(DAL.PagedListDAL<ToolsUseRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        public static int UpdateExchange(GameRecordView model)
        {
            return ToolsUseRecordDAL.UpdateExchange(model);
        }
    }
}
