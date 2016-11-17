using GL.Data.JWeb.BLL;
using GL.Data.JWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JWeb.Controllers
{
    /// <summary>
    /// 开心水族馆
    /// </summary>
    public class FishBlockController : Controller
    {
        /// <summary>
        /// 水族馆首页
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult Index(Dictionary<string, string> queryvalues)
        {
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : -1;

            return View();
        }

        /// <summary>
        /// 根据用户ID查询出用户昵称，送鱼的时候用
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult GetCatchID(Dictionary<string, string> queryvalues) {
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : -1;

            string name = RoleBLL.GetNickNameByID(userid);
            if (name == null)//说明无此人
            {
                return Json(new
                {
                    Result = 0,
                    name = ""
                });
            }
            else {//此人存在,还需要把名字给前端
                return Json(new
                {
                    Result = 1,
                    name = name
                });
            }

        }

        /// <summary>
        /// 得到鱼日志
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult GetMyFishLogsPage(Dictionary<string, string> queryvalues) {
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : -1;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            IEnumerable<FishInfoRecord> data = FishBLL.GetFishInfoRecord(userid, page);

            return Json(new
            {
                data = data
            });
        }


    }
}