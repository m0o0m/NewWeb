using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.BLL
{
    public class NoAuthBLL
    {
        public static int AddSMS(int userid,string sign)
        {
            return DAL.NoAuthDAL.AddSMS(userid,sign);
        }

        public static int UpdateRechargeSum(int hour)
        {
            return DAL.NoAuthDAL.UpdateRechargeSum(hour);
        }


    }
}
