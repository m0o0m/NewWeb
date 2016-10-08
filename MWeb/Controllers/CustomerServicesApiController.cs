using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MWeb.Controllers
{
    [Authorize]
    public class CustomerServicesApiController : ApiController
    {
        public object Delete([FromBody]CustomerServCenter model)
        {
            int result = CustomerServCenterBLL.Delete(model);
            if (result > 0)
            {
                return new { result = 0 };
            }
            return new { result = 1 };

        }

        public object Post([FromBody]CustomerServCenter model)
        {

            //var a = model.CSCMainID;

            model.GUName = "管理员";
            model.GUType = 1;
            model.GUUserID = 0;
            model.CSCSubId = model.CSCMainID;
            model.CSCState = cscState.未处理;
            model.CSCTime = DateTime.Now;

            int result = CustomerServCenterBLL.Insert(model);

            if (result > 0)
            {
                CustomerServCenter c = new CustomerServCenter { CSCMainID = model.CSCMainID, CSCState = cscState.已回复 };
                int r = CustomerServCenterBLL.UpdateForManage(c);

                return new { result = 0 };
            }
            else
            {
                return new { result = 1 };
            }

        }



    }
}
