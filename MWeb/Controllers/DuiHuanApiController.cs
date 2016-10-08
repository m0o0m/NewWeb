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
    public class DuiHuanApiController : ApiController
    {
        public object Post([FromBody]DuiHuan model)
        {
            model.GiveOut = giveOut.发放了;
            int result = DuiHuanBLL.Update(model);
            if (result > 0)
            {
                return new { result = 0 };
            }
            return new { result = 1 };
        }

    }
}
