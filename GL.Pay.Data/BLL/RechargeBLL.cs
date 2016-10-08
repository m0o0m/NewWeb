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
            return RechargeDAL.Add(model);
        }

    }
}
