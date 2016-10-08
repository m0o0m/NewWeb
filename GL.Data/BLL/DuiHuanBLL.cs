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
    public class DuiHuanBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static PagedList<DuiHuan> GetListByPage(GameRecordView grv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = grv.Page;
            pq.PageSize = 10;

            if (!string.IsNullOrEmpty(grv.SearchExt))
            {
                pq.RecordCount = DAL.PagedListDAL<DuiHuan>.GetRecordCount(string.Format(@"select count(0) from DuiHuan,Role where DuiHuan.UserID=Role.ID and ( Role.ID ='{0}' or Role.NickName='{0}' or Role.Account='{0}' ) and DuiHuan.CreateTime between '{1}' and '{2}' "+ (grv.Channels > 0 ? " and find_in_set(Role.Agent, {3})" : ""), grv.SearchExt, grv.StartDate, grv.ExpirationDate,grv.UserList), sqlconnectionString);
                pq.Sql = string.Format(@"select  Role.NickName, DuiHuan.* from  DuiHuan,Role where DuiHuan.UserID=Role.ID and DuiHuan.CreateTime between '{3}' and '{4}' and  ( Role.ID ='{2}' or Role.NickName='{2}' or Role.Account='{2}' )"+ (grv.Channels > 0 ? " and find_in_set(Role.Agent, {5})" : "" )+ " order by DuiHuan.CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.SearchExt, grv.StartDate, grv.ExpirationDate,grv.UserList);
            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<DuiHuan>.GetRecordCount(string.Format(@"select count(0) from DuiHuan,Role where DuiHuan.UserID=Role.ID and DuiHuan.CreateTime between '{1}' and '{2}'" + (grv.Channels > 0 ? " and find_in_set(Role.Agent, {3})" : ""), grv.UserID, grv.StartDate, grv.ExpirationDate,grv.UserList), sqlconnectionString);
                pq.Sql = string.Format(@"select Role.NickName, DuiHuan.* from DuiHuan,Role  where DuiHuan.UserID=Role.ID and  DuiHuan.CreateTime between '{3}' and '{4}' "+ (grv.Channels > 0 ? " and find_in_set(Role.Agent, {5})" : "") + " order by DuiHuan.CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, grv.UserID, grv.StartDate, grv.ExpirationDate,grv.UserList);
            }

            PagedList<DuiHuan> obj = new PagedList<DuiHuan>(DAL.PagedListDAL<DuiHuan>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        public static int Update(DuiHuan model)
        {
            return DuiHuanDAL.Update(model);
        }



        public static object GetSumDuiHuan(GameRecordView model)
        {
            return DuiHuanDAL.GetSumDuiHuan(model);
        }
    }
}
