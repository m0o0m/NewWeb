using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace CMeb.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult ClubList()
        {

            PagedList<CLoginUserClub>  data =ClubBLL.GetCLoginUserClubListByPage(1, 35, -1);

            //将数据展示到前端

            return View();
        }
    }
}