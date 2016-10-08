using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.DAL;
using OWeb.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Webdiyer.WebControls.Mvc;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Principal;
using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.Owin.Security;
using GL.Data.OWeb.Identity;

namespace OWeb.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {


        [QueryValues]
        public ActionResult FAQ(Dictionary<string, string> queryvalues)
        {

            IEnumerable<FAQ> model = FAQBLL.GetList();
            return View(model);

        }


        [QueryValues]
        public ActionResult OnlineProblem(Dictionary<string, string> queryvalues)
        {

            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            if (Request.IsAjaxRequest())
            {
                return PartialView("OnlineProblem_PageList", CustomerServCenterBLL.GetListByPage(page));
            }
            PagedList<CustomerServCenter> model = CustomerServCenterBLL.GetListByPage(page);

            return View(model);
        }

        [HttpPost]
        [QueryValues]
        public ActionResult DeleteProblem(Dictionary<string, string> queryvalues)
        {

            int value = queryvalues.ContainsKey("value") ? Convert.ToInt32(queryvalues["value"]) : 0;
            if (value > 0)
            {
                int result = CustomerServCenterBLL.Delete(new CustomerServCenter { CSCMainID = value });
                if (result > 0)
                {
                    return Json(
                        new { result = Result.Normal }
                    );
                }
                else
                {
                    return Json(
                        new { result = Result.Lost }
                    );
                }
            }
            return Json(new {
                result = Result.UnknownError
            });
        }

        [QueryValues]
        public ActionResult PopupboxForReply(Dictionary<string, string> queryvalues)
        {

            int CSCMainID = queryvalues.ContainsKey("CSCMainID") ? Convert.ToInt32(queryvalues["CSCMainID"]) : 0;
            if (CSCMainID > 0)
            {

                //CustomerServCenter res = CustomerServCenterBLL.GetModel(new CustomerServCenter { CSCMainID = CSCMainID });

                IEnumerable<CustomerServCenter> result = CustomerServCenterBLL.GetListWithCSCSubId(new CustomerServCenter { CSCMainID = CSCMainID });
                CustomerServCenter res = result.Where(x => x.CSCMainID == CSCMainID).FirstOrDefault();

                ViewData["res"] = res;
                return View(result);

            }
            return View();
        }



        [HttpPost]
        [QueryValues]
        public ActionResult PopupboxForReply(CustomerServCenter model, Dictionary<string, string> queryvalues)
        {

            var a = model.CSCMainID;


            model.GUName = "客服";
            model.GUType = 2;
            model.GUUserID = 0;

            model.CSCSubId = model.CSCMainID;
            model.CSCState = cscState.未处理;
            model.CSCTime = DateTime.Now;


            int result = CustomerServCenterBLL.Insert(model);

            if (result > 0)
            {
                CustomerServCenter c = new CustomerServCenter { CSCUpdateTime=DateTime.Now, CSCMainID = model.CSCMainID, CSCState = cscState.已回复 };
                int r = CustomerServCenterBLL.UpdateForManage(c);

                return Json(new { result = 0 });
            }
            else
            {
                return Json(new { result = 1 });
            }
        }




    }
}
