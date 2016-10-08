using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AWeb.Controllers
{
    public class NoViewController : Controller
    {
        // GET: NoView
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GeneralError()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Error403()
        {
            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error405()
        {
            return View();
        }
        public ActionResult Error500()
        {
            return View();
        }
    }
}