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
    public class OperLogBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");


        public static IEnumerable<OperLog> GetModelByIDList()
        {
            return OperLogDAL.GetModelByIDList();
        }


        //InsertOperLog
        public static int InsertOperLog(OperLog model)
        {
            return OperLogDAL.InsertOperLog(model);
        }


        //FreezeLog

        public static int InsertFreezeLog(FreezeLog model)
        {
            return OperLogDAL.InsertFreezeLog(model);
        }



        public static OperConfig GetOperConfigExist(string url, string param)
        {
            return OperLogDAL.GetOperConfigExist(url,param);
        }


        public static void WriteOperLog(string url, string param,string UserAccount,string IP)
        {
            OperConfig config = OperLogDAL.GetOperConfigExist(url, param);
            if (config != null)
            {
             

                if (config.Action == "/Home/Login") {
                    UserAccount = "";
                }
             
                OperLogBLL.InsertOperLog(new OperLog()
                {
                    CreateTime = DateTime.Now.ToString(),
                    LeftMenu = config.ActionName,
                    OperDetail = "",
                    OperType = config.ActionOper,
                    UserAccount = UserAccount,
                     UserName = UserAccount,
                     IP = IP
                });
            }
         

        }


        public static PagedList<OperLog> GetListByPageForOperLog(OperLogView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

            pq.RecordCount = DAL.PagedListDAL<OperLog>.GetRecordCount(string.Format(@"
select count(0) from Q_OperLog where CreateTime >= '{0}' and CreateTime<'{1}'  {2} ", 
model.StartTime, 
model.EndTime, 
model.UserAccount == "" ? " " : " and  UserAccount = '" + model.UserAccount + "'"
), sqlconnectionString);

            pq.Sql = string.Format(@"
select * from Q_OperLog
where CreateTime >= '{2}' and CreateTime< '{3}' {4}  
order by CreateTime desc 
limit {0}, {1}
",
pq.StartRowNumber, 
pq.PageSize, model.StartTime, model.EndTime, model.UserAccount == "" ? " " : " and  UserAccount = '" + model.UserAccount + "'");


            PagedList<OperLog> obj = new PagedList<OperLog>(DAL.PagedListDAL<OperLog>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }




        public static PagedList<FreezeLog> GetListByPageForFreezeLog(FreezeLogView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

            pq.RecordCount = DAL.PagedListDAL<FreezeLog>.GetRecordCount(string.Format(@"
select count(0) from (
select * from Q_FreezeLog
union all
select ID,CreateTime,UserID,'','','永久','系统',
case when MonitorID=27 then '凭空出现的游戏币' else '非法协议' end ,'封号' 
from GServerInfo.MonitorLog where MonitorID in (27,28)

) as a
where a.CreateTime >= '{0}' and a.CreateTime<'{1}'  {2} ",
model.StartTime,
model.EndTime,
model.Search =="" ? " " : " and  a.UserID = '" + model.Search + "'  or a.IP='"+model.Search+"'  or a.IMei='"+model.Search+"'  "
), sqlconnectionString);

            pq.Sql = string.Format(@"
select a.* from (
select * from Q_FreezeLog
union all
select ID,CreateTime,UserID,'','','永久','系统',
case when MonitorID=27 then '凭空出现的游戏币' else '非法协议' end ,'封号' 
from GServerInfo.MonitorLog where MonitorID in (27,28)
) as a
where a.CreateTime >= '{2}' and a.CreateTime< '{3}' {4}  
order by a.CreateTime desc 
limit {0}, {1}
",
pq.StartRowNumber,
pq.PageSize, model.StartTime, model.EndTime,
model.Search=="" ? " " : " and  a.UserID = '" + model.Search + "'   or  a.IP='"+model.Search+"' or a.IMei='"+model.Search+"'  ");


            PagedList<FreezeLog> obj = new PagedList<FreezeLog>(DAL.PagedListDAL<FreezeLog>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


    }
}
