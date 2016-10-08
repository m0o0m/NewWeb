using GL.Data.TWeb.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TWeb.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }



      
        public ActionResult DataControlLog()
        {
            try { 
            int id = Convert.ToInt32( Request["id"]);
               
            int userid = Convert.ToInt32(Request["userid"]);

            int i = TimerBLL.AddTMonitorLog(id,userid);
            return Content(i.ToString());
            }
            catch
            {
                return Content("0");
            }

        }

    }
}