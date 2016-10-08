using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WOHAIWeb.Models;
using Webdiyer.WebControls.Mvc;
using GL.Data.Model;
using GL.Data.BLL;
using System.Collections.Generic;

namespace WOHAIWeb.Controllers
{
    [UserAuthentication]
    public class ManageController : Controller
    {

        [QueryValues]
        public ActionResult Management(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Management_PageList", ManagerInfoBLL.GetListByPage(page));
            }


            PagedList<ManagerInfo> model = ManagerInfoBLL.GetListByPage(page);
            return View(model);
        }



        [QueryValues]
        public ActionResult ManagementForAdd(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            ManagerInfo mi = new ManagerInfo();

            return View(mi);
        }
        [QueryValues]
        public ActionResult ManagementForUpdate(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;



            ManagerInfo mi = ManagerInfoBLL.GetModelByID(new ManagerInfo() { AdminID = id });
            if (mi == null)
            {
                return RedirectToAction("Management");
            }


            return View(mi);

        }



    }
}