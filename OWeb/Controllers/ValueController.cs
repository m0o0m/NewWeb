using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OWeb.Controllers
{
    public class ValueController : ApiController
    {

        public object Post([FromBody]CustomerServCenter model)
        {



            var a = model.CSCMainID;



            model.GUName = "客服";
            model.GUType = 2;
            model.GUUserID = 0;


            model.CSCSubId = model.CSCMainID;
            model.CSCState = 1;
            model.CSCTime = DateTime.Now;




            int result = CustomerServCenterBLL.Insert(model);

            if (result > 0)
            {
                CustomerServCenter c = new CustomerServCenter { CSCMainID = model.CSCMainID, CSCState = 2 };
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
