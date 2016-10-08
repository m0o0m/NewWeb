using GL.Data.TWeb.DAL;
using GL.Data.TWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.TWeb.BLL
{
    public class TimerBLL
    {
        public static IEnumerable<TMonitorData> GetTimeHasSQL( )
        {
           return  TimerDAL.GetTimeHasSQL();
        }

        public static IEnumerable<TMonitorData> GetTimeNoSQL( )
        {
            return TimerDAL.GetTimeNoSQL();
        }

        public static TMonitorData GetTimeByID(int id) {
            return TimerDAL.GetTimeByID(id);
        }
        public static SUpdate GetTimeForGame(string SearchExt)
        {
            return TimerDAL.GetTimeForGame(SearchExt);
        }
        public static int ExecuteSql(string sql) {
            return TimerDAL.ExecuteSql(sql);
        }


        public static IEnumerable<TMonitorLog> GetTMonitorLog()
        {
            return TimerDAL.GetTMonitorLog();
        }

        //UpdateTMonitorLog


        public static int UpdateTMonitorLog(string ids)
        {
            return TimerDAL.UpdateTMonitorLog(ids);
        }

        public static int AddTMonitorLog(int monitorID,int userid) {
            return TimerDAL.AddTMonitorLog(monitorID, userid);
        }

        public static int AddTMonitorLog(int monitorID)
        {
            return TimerDAL.AddTMonitorLog(monitorID);
        }


        public static DateTime GetDataBaseTime()
        {
            return TimerDAL.GetDataBaseTime();
        }
    }
}
