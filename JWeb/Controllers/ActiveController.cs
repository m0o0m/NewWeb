using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JWeb.Controllers
{
    /// <summary>
    /// 活动界面
    /// </summary>
    public class ActiveController : Controller
    {
        [QueryValues]
        // GET: Active
        public ActionResult Index(Dictionary<string, string> queryvalues)
        {
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : -1;
            return View();
        }
    }
}