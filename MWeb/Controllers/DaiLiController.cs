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
            string checkboxs = queryvalues.ContainsKey("checkbox") ? queryvalues["checkbox"].Trim(',') : "";
            string isPostBack = queryvalues.ContainsKey("isPostBack") ? queryvalues["isPostBack"] : "";
            //isPostBack
            if (!string.IsNullOrEmpty(isPostBack)) {//保存
                int res = DaiLiBLL.UpdateFlowDesc(_no, checkboxs);
                return Content(res.ToString());
            }
            BaseDataView bd = new BaseDataView();

            IEnumerable<S_Desc> modelList = DaiLiBLL.GetFlowDesc(_no);

            bd.BaseDataList = modelList;
            bd.SearchExt = _no.ToString();

            return View(bd);
        }
        [QueryValues]
        public ActionResult GetUpdateGoldByRMB(Dictionary<string, string> queryvalues)
        {
            int _no = queryvalues.ContainsKey("no") ? Convert.ToInt32(queryvalues["no"]) : -1;
            Int64 _gold = queryvalues.ContainsKey("gold") ? Convert.ToInt64(queryvalues["gold"]) : 0;
            DaiLiUsers daliuser = DaiLiBLL.GetDaiLiUsers(_no);

            string fen = daliuser.FenChenRate;
            string goldFen = daliuser.GoldRate;

            string[] fens = fen.Split(':');
            int fen1 = Convert.ToInt32(fens[0].Trim());
            int fen2 = Convert.ToInt32(fens[1].Trim());
            double fenLast = fen2 / fen1;

            string[] goldFens = goldFen.Split(':');
            int goldFen1 = Convert.ToInt32(goldFens[0].Trim());
            int goldFen2 = Convert.ToInt32(goldFens[1].Trim());
            double goldFenLast = goldFen2 / goldFen1;

            //将人民币转换为金币
            _gold = Convert.ToInt64(_gold * (fenLast + 1) * goldFenLast);
         


            return Content(_gold.ToString());
        }


        [QueryValues]
        public ActionResult UpdateKuCun(Dictionary<string, string> queryvalues) {

            int _no = queryvalues.ContainsKey("no") ? Convert.ToInt32(queryvalues["no"]) : -1;
            Int64 _gold = queryvalues.ContainsKey("gold")? Convert.ToInt64(queryvalues["gold"]) : 0;
            Int64 rmb = _gold;
            DaiLiUsers daliuser = DaiLiBLL.GetDaiLiUsers(_no);

            string fen = daliuser.FenChenRate;
            string goldFen = daliuser.GoldRate;

            string[] fens = fen.Split(':');
            int fen1 = Convert.ToInt32(fens[0].Trim());
            int fen2 = Convert.ToInt32(fens[1].Trim());
            double fenLast = fen2 / fen1;

            string[] goldFens = goldFen.Split(':');
            int goldFen1 = Convert.ToInt32(goldFens[0].Trim());
            int goldFen2 = Convert.ToInt32(goldFens[1].Trim());
            double goldFenLast = goldFen2 / goldFen1;

            //将人民币转换为金币
            _gold = Convert.ToInt64(_gold * (fenLast + 1) * goldFenLast);



            if (_no < 0)
            {
                return Content("-1");
            }
            else {
                int i =  DaiLiBLL.UpdateDaiLiKuCun(_no, _gold, DaiLiType.充值库存);
                if (i < 0) {
                    return Content("-2");
                }
                else {


                    DaiLiBLL.InsertKuCunFlow(new GL.Data.Model.KuCunFlow() {
                        CreateTime = DateTime.Now.ToString(),
                        Operation = "添加",
                        OperGold = _gold,
                        OperUserName = User.Identity.Name,
                        DaiLiNo = _no,
                         FenChenRate = fen,
                          GoldRate = goldFen,
                           Rmb = rmb
                    });


              

                    return Content("1");
                }
            }
           
        }


        [QueryValues]
        public ActionResult KuCunFlow(Dictionary<string, string> queryvalues) {
            int tab = queryvalues.ContainsKey("RaType") ? Convert.ToInt32(queryvalues["RaType"]) : -1;
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
          
          

            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
        

            BaseDataView vbd = new BaseDataView
            {
            
                StartDate = _StartDate,
                ExpirationDate = _ExpirationDate,
             
                RaType = tab
            };

        

            if (Request.IsAjaxRequest())
            {
                return PartialView("KuCunFlow_PageList", DaiLiBLL.GetKuCunListByPage(page, Convert.ToInt32(vbd.RaType)));
            }


            vbd.BaseDataList = DaiLiBLL.GetKuCunListByPage(page,Convert.ToInt32( vbd.RaType));

            return View(vbd);
        }

    }
}