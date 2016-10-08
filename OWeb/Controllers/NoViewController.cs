using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GL.Data.Model;

namespace OWeb.Controllers
{
    public class NoViewController : Controller
    {
        public ActionResult Error()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new { result = Result.Err404 });
            }
            return View("Error404");
        }
        public ActionResult Error404()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new { result = Result.Err404 });
            }
            return View();
        }
        public ActionResult Error403()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new { result = Result.Err403 });
            }
            return View();
        }
        public ActionResult Error405()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new { result = Result.Err405 });
            }
            return View();
        }
        public ActionResult Error500()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new { result = Result.Err500 });
            }
            return View();
        }


    }
}