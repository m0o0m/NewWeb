using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MWeb.Controllers
{
    public class ErrorController : Controller
    {
        [QueryValues]
        // GET: Error
        public ActionResult NoPower(Dictionary<string, string> queryvalues)
        {
            string url = queryvalues.ContainsKey("url") ? queryvalues["url"] :"";
            ViewData["url"] = url;

            //根据url查询其名称
            Resource model = GL.Data.BLL.SUBLL.GetModelByFirstUrl(url);
            if (model == null)
            {
                ViewData["name"] = "";
            }
            else {
                ViewData["name"] = model.Name;
            }
           
            return View();
        }

        public ActionResult NoPager()
        {
            return View();
        }
    }
}