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
    public class ManageApiController : ApiController
    {

        public object Delete([FromBody]ManagerInfo model)
        {
            int result = ManagerInfoBLL.Delete(model);
            if (result > 0)
            {
                return new { result = 0 };
            }
            return new { result = 1 };

        }
        public object Put([FromBody]ManagerInfo model)
        {

            if (string.IsNullOrWhiteSpace(model.AdminAccount))
            {
                return new { result = Result.ValueCanNotBeNull };
            }
            if (string.IsNullOrWhiteSpace(model.AdminPasswd))
            {
                return new { result = Result.ValueCanNotBeNull };
            }

            //if (Utils.IsValidate(model.AdminAccount))
            //{
            //    return new { result = Result.ValueCanNotBeNull };
            //}

            if (!Regex.IsMatch(model.AdminAccount, @"^[a-zA-Z_]\w{3,16}"))
            {
                return new { result = Result.AccountOnlyConsistOfLettersAndNumbers };
            }

            ManagerInfo mi = ManagerInfoBLL.GetModel(model);
            if (mi != null)
            {
                return new { result = Result.AccountHasBeenRegistered };
            }

            model.AdminMasterRight = 0;
            model.AdminPasswd = Utils.MD5(model.AdminPasswd);
            model.CreateDate = DateTime.Now;

            int result = ManagerInfoBLL.Add(model);
            if (result > 0)
            {
                return new { result = 0 };
            }

            return new { result = 4 };



        }
        public object Options([FromBody]ManagerInfo model)
        {

            if (string.IsNullOrWhiteSpace(model.AdminPasswd))
            {
                return new { result = Result.ValueCanNotBeNull };
            }

            ManagerInfo mi = ManagerInfoBLL.GetModelByID(model);
            if (mi.AdminPasswd == Utils.MD5(model.AdminPasswd))
            {
                return new { result = Result.Normal };
            }

            model.AdminMasterRight = mi.AdminMasterRight;
            model.AdminAccount = mi.AdminAccount;
            model.AdminPasswd = Utils.MD5(model.AdminPasswd);

            int result = ManagerInfoBLL.Update(model);
            if (result > 0)
            {
                return new { result = Result.Normal };
            }

            return new { result = 4 };



        }

    }
}
