using GL.Common;
using GL.Data;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Data.View;
using GL.Protocol;
using MWeb.protobuf.SCmd;
using ProtoCmd.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using GL.Command.DBUtility;

namespace MWeb.Controllers
{
    [Authorize]
    /// <summary>
    /// 代理管理
    /// </summary>
    public class DaiLiController : Controller
    {
        // GET: DaiLi
        public ActionResult Index()
        {

            IEnumerable<DaiLiUsers> modelList = DaiLiBLL.GetDaiLiUsers();

            

            return View(modelList);
        }

        [QueryValues]
        public ActionResult KuCunValueIndex(Dictionary<string, string> queryvalues) {
            int _no = queryvalues.ContainsKey("no") ? Convert.ToInt32(queryvalues["no"]) : -1;

            ViewData["no"] = _no;
           

            return View();
        }
        [QueryValues]
        public ActionResult SetFlowNoIndex(Dictionary<string, string> queryvalues) {
            int _no = queryvalues.ContainsKey("no") ? Convert.ToInt32(queryvalues["no"]) : -1;
            string checkboxs = queryvalues.ContainsKey("checkbox") ? queryvalues["checkbox"] : "";
            string isPostBack = queryvalues.ContainsKey("isPostBack") ? queryvalues["isPostBack"] : "";
            //isPostBack
            if (!string.IsNullOrEmpty(isPostBack)) {//保存
                int res = DaiLiBLL.UpdateFlowDesc(_no, checkboxs);
               

            }
            BaseDataView bd = new BaseDataView();

            IEnumerable<S_Desc> modelList = DaiLiBLL.GetFlowDesc(_no);

            bd.BaseDataList = modelList;
            bd.SearchExt = _no.ToString();

            return View(bd);
        }
      

        [QueryValues]
        public ActionResult UpdateKuCun(Dictionary<string, string> queryvalues) {

            int _no = queryvalues.ContainsKey("no") ? Convert.ToInt32(queryvalues["no"]) : -1;
            Int64 _gold = queryvalues.ContainsKey("gold")? Convert.ToInt64(queryvalues["gold"]) : 0;
            if (_no < 0)
            {
                return Content("-1");
            }
            else {
                int i =  DaiLiBLL.UpdateDaiLiKuCun(_no, _gold);
                if (i < 0) {
                    return Content("-2");
                }
                else {
                    return Content("1");
                }
            }
           
        }
    }
}