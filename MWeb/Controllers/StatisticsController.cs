using GL.Common;
using GL.Data.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using GL.Data.Model;
using log4net;
using System.Web.Mvc;

namespace MWeb.Controllers
{
    public class Ta {
        public string X { get; set; }
        public double Y { get; set; }

        public string Type { get; set; }
    }

    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Terminal()
        {
            
            return View();
        }

        public ActionResult Test() {
            return View();
        }

        [QueryValues]
        public ActionResult SystemPay(Dictionary<string, string> queryvalues) {
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.ToString("yyyy-MM-dd");

            BaseDataView bdv = new BaseDataView()
            {
                 StartDate = _StartDate, ExpirationDate = _ExpirationDate
            };

           
       

            if (Request.IsAjaxRequest()) {

                List<SystemPay> pays = SystemPayBLL.GetSystemPay(_StartDate, _ExpirationDate, 0);
                
                return Json(new
                {
                    datatype = "list",
                    ret = 0,
                    data = pays
                });


            }

            return View(bdv);
        }


        [QueryValues]
        public ActionResult SystemExpend(Dictionary<string, string> queryvalues) {
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.ToString("yyyy-MM-dd");

            BaseDataView bdv = new BaseDataView()
            {
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate
            };




            if (Request.IsAjaxRequest())
            {

                List<SystemExpend> pays = SystemPayBLL.GetSystemSystemExpend(_StartDate, _ExpirationDate, 0);

                return Json(new
                {
                    datatype = "list",
                    ret = 0,
                    data = pays
                });


            }

            return View(bdv);
        }

        public ActionResult Test2() {
            return View();
        }


        [QueryValues]
        public ActionResult SystemPay2(Dictionary<string, string> queryvalues)
        {
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.ToString("yyyy-MM-dd");

            BaseDataView bdv = new BaseDataView()
            {
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate
            };




            if (Request.IsAjaxRequest())
            {

                List<SystemPay> pays = SystemPayBLL.GetSystemPay(_StartDate, _ExpirationDate, 0);

                return Json(new
                {
                    datatype = "list",
                    ret = 0,
                    data = pays
                });


            }

            return View(bdv);
        }


    }
}