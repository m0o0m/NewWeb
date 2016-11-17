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
        public static DailiKuCun GetDaiLiKuCun() {
            return DaiLiDAL.GetDaiLiKuCun();
        }
        public static DailiKuCun GetDaiLiKuCun(int no)
        {
            return DaiLiDAL.GetDaiLiKuCun(no);
        }

        public static int UpdateDaiLiKuCun(int no, Int64 Gold)
        {
            return DaiLiDAL.UpdateDaiLiKuCun( no,Gold);
        }





        public static IEnumerable<DaiLiUsers> GetDaiLiUsers()
        {
            return DaiLiDAL.GetDaiLiUsers();
        }


        public static IEnumerable<S_Desc> GetFlowDesc(int no)
        {
            return DaiLiDAL.GetFlowDesc(no);
        }

        public static int UpdateFlowDesc(int no, string flowNos) {
            return DaiLiDAL.UpdateFlowDesc(no,flowNos);
        }
    }
}
