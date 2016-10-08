using GL.Pay.Data.DAL;
using GL.Pay.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.Data.BLL
{
    public class RechargeCheckBLL
    {

        public static int Delete(RechargeCheck model)
        {
            return RechargeCheckDAL.Delete(model);
        }

        public static int Add(RechargeCheck model)
        {
            return RechargeCheckDAL.Add(model);
        }

        public static RechargeCheck GetModelByID(RechargeCheck mi)
        {
            return RechargeCheckDAL.GetModelByID(mi);
        }

        public static int Update(RechargeCheck model)
        {
            return RechargeCheckDAL.Update(model);
        }


        public static RechargeCheck GetModelBySerialNo(RechargeCheck model)
        {
            return RechargeCheckDAL.GetModelBySerialNo(model);
        }
    }
}
