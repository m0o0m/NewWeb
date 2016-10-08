using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Redis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Layout = null;
#if DEBUG
            ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
#endif

            return View();
        }
    }
}
