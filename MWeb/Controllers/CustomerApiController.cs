using GL.Common;
using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace MWeb.Controllers
{
    [Authorize]
    public class CustomerApiController : ApiController
    {
        public object Delete([FromBody]CustomerInfo model)
        {


            int result = CustomerInfoBLL.Delete(model);
            if (result > 0)
            {
                return new { result = 0 };
            }
            return new { result = 1 };

        }
        public object Put([FromBody]CustomerInfo model)
        {

            if (string.IsNullOrWhiteSpace(model.CustomerAccount) || string.IsNullOrWhiteSpace(model.CustomerName) || string.IsNullOrWhiteSpace(model.CustomerPwd) )
            {
                return new { result = Result.ValueCanNotBeNull };
            }

            if (!Regex.IsMatch(model.CustomerAccount, @"^[a-zA-Z_]\w{3,16}"))
            {
                return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            }
            if (!Regex.IsMatch(model.CustomerName, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,16}"))
            {
                return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            }


            CustomerInfo mi = CustomerInfoBLL.GetModel(model);
            if (mi != null)
            {
                return new { result = Result.AccountHasBeenRegistered };
            }


            model.CustomerState = 0;
            model.CustomerPwd = Utils.MD5(model.CustomerPwd);
            model.CreateDate = DateTime.Now;



            int result = CustomerInfoBLL.Add(model);
            if (result > 0)
            {
                return new { result = 0 };
            }

            return new { result = 4 };



        }
        public object Options([FromBody]CustomerInfo model)
        {

            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                return new { result = Result.ValueCanNotBeNull };
            }


            if (!Regex.IsMatch(model.CustomerName, @"^[\u0391-\uFFE5a-zA-Z_]\w{3,16}"))
            {
                return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            }


            CustomerInfo mi = CustomerInfoBLL.GetModelByID(model);

            if (!string.IsNullOrWhiteSpace(model.CustomerPwd) && mi.CustomerPwd != Utils.MD5(model.CustomerPwd))
            {
                mi.CustomerPwd = Utils.MD5(model.CustomerPwd);
            }




            mi.CustomerName = model.CustomerName;




            int result = CustomerInfoBLL.Update(mi);
            if (result > 0)
            {
                return new { result = 0 };
            }

            return new { result = 4 };



        }



    }
}
