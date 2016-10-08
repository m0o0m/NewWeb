using GL.Command.DBUtility;
using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class RechargeBLL
    {
       
        public static int Add(Recharge model)
        {

            string ver = RechargeDAL.GetVersion(model);

           

            model.VersionInfo = ver;
            return RechargeDAL.Add(model);
        }

        public static int UpdateReachTime(string billNO)
        {

            return RechargeDAL.UpdateReachTime(billNO);
        }


        public static int Delete(Recharge model)
        {
            if (string.IsNullOrEmpty(model.BillNo)) {
                return -1;
            }
            return RechargeDAL.Delete(model);
        }

        public static Recharge GetModelByBillNo(Recharge model)
        {
            return RechargeDAL.GetModelByBillNo(model);
        }

        public static Recharge GetFirstModelListByUserID(Recharge model)
        {
            return RechargeDAL.GetFirstModelListByUserID(model);
        }

    }
}
