using System;
using System.Collections.Generic;
using System.Web.Http;
using Data.View;
using Data.Model;

namespace A3.Web.Controllers
{
    public class a111Controller : ApiController
    {
        public IEnumerable<CSCModel> Get([FromUri]int GUUserID)
        {


            return Data.BLL.CustomerServCenterBLL.GetList(GUUserID);
        }



        public object Post([FromBody]CustomerServCenter cust)
        {
            if (cust.GUUserID == 0)
            {
                return new { result = 1 };
            }
            if (string.IsNullOrWhiteSpace(cust.GUName))
            {
                return new { result = 1 };
            }
            if (string.IsNullOrWhiteSpace(cust.CSCTitle))
            {
                return new { result = 1 };
            }
            if (string.IsNullOrWhiteSpace(cust.CSCContent))
            {
                return new { result = 1 };
            }
            if (cust.CSCTitle.Length < 4 || cust.CSCTitle.Length > 20)
            {
                return new { result = 1 };
            }
            if (cust.CSCContent.Length < 20 || cust.CSCContent.Length > 500)
            {
                return new { result = 1 };
            }

            cust.GUType = 0;
            cust.CSCSubId = 0;
            cust.CSCState = 1;
            cust.CSCTime = DateTime.Now;

            int result = Data.BLL.CustomerServCenterBLL.Insert(cust);
            if (result > 0)
            {
                return new { result = 0 };
            }
            else
            {
                return new { result = 1 };
            }

        }


        public object Post(int id, [FromBody]CustomerServCenter cust)
        {
            if (cust.GUUserID == 0)
            {
                return new { result = 1 };
            }
            if (string.IsNullOrWhiteSpace(cust.GUName))
            {
                return new { result = 1 };
            }
            if (string.IsNullOrWhiteSpace(cust.CSCContent))
            {
                return new { result = 1 };
            }
            if (cust.CSCContent.Length < 20 || cust.CSCContent.Length > 500)
            {
                return new { result = 1 };
            }

            //cust.CSCMainID = id;
            cust.GUType = 0;
            cust.CSCSubId = id;
            cust.CSCState = 1;
            cust.CSCTime = DateTime.Now;



            int result = Data.BLL.CustomerServCenterBLL.Insert(cust);
            if (result > 0)
            {
                CustomerServCenter c = new CustomerServCenter { CSCMainID = id, CSCState = 1, GUUserID = cust.GUUserID };
                int r = Data.BLL.CustomerServCenterBLL.Update(c);

                return new { result = 0 };
            }
            else
            {
                return new { result = 1 };
            }

        }



    }
}
