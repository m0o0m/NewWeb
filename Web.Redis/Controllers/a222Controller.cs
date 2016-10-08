using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Web.Redis.Controllers
{
    public class a222Controller : ApiController
    {
        // GET: a222
        public object Post(int id, [FromBody]CustomerServCenter cust)
        {

            if (cust.CSCState != cscState.已解决)
            {
                return new { result = 1 };
            }
            CustomerServCenter c = new CustomerServCenter { CSCMainID = id, CSCState = cust.CSCState, GUUserID = cust.GUUserID };
            int result = CustomerServCenterBLL.Update(c);
            if (result > 0)
            {
                return new { result = 0 };
            }
            else
            {
                return new { result = 1 };
            }

        }
    }
}