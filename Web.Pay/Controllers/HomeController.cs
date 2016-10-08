using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Pay.Controllers
{
    public class HomeController : Controller
    {

        //private ILog log = LogManager.GetLogger("Moon Test");
        // GET: Home
        public ActionResult Index()
        {
            //log.Error("Moons");
            return View();
        }


        [QueryValues]
        public ActionResult success(Dictionary<string, string> queryvalues)
        {

            //orderid = m.orderid, yborderid = m.yborderid 
            string orderid = queryvalues.ContainsKey("orderid") ? queryvalues["orderid"] : string.Empty;
            string yborderid = queryvalues.ContainsKey("yborderid") ? queryvalues["yborderid"] : string.Empty;


           // return Redirect("mobilecall://success");


            return Redirect("mobilecall://success?orderid="+orderid);
        }

        [QueryValues]
        public ActionResult fail(Dictionary<string, string> queryvalues)
        {
            string orderid = queryvalues.ContainsKey("orderid") ? queryvalues["orderid"] : string.Empty;
            string yborderid = queryvalues.ContainsKey("yborderid") ? queryvalues["yborderid"] : string.Empty;


           // return Redirect("mobilecall://fail");

            return Redirect("mobilecall://fail?orderid = "+orderid);
        }

    }
}