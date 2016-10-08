using GL.Common;
using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using GL.Data.DAL;
using GL.Command.DBUtility;
using GL.Data.View;


namespace MWeb.Controllers
{
    [Authorize]
    public class ConsoleController : Controller
    {
        // GET: Console
        public ActionResult ProbabilityGames()
        {

#if Debug
            ViewData["version"] = Utils.GetTimeStamp();
#endif
#if TEST
            ViewData["version"] = Utils.GetTimeStamp();
#endif
#if R17
            ViewData["version"] = Utils.GetTimeStamp();
#endif
#if Release
            ViewData["version"] = Utils.GetTimeStamp();
#endif



            return View();
        }

        [QueryValues]
        public ActionResult PotCtrlList(Dictionary<string, string> queryvalues)
        {

     
            int _id = queryvalues.ContainsKey("UserID") ? string.IsNullOrWhiteSpace(queryvalues["UserID"]) ? 0 : Convert.ToInt32(queryvalues["UserID"]) : 0;
           
          
            string _StartDate = queryvalues.ContainsKey("StartDate") ? queryvalues["StartDate"] : DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            string _ExpirationDate = queryvalues.ContainsKey("ExpirationDate") ? queryvalues["ExpirationDate"] : DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
          
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;

            string _search = queryvalues.ContainsKey("SearchExt") ? queryvalues["SearchExt"].ToString() : "";

            GameRecordView grv = new GameRecordView { Page=page, SearchExt=_search ,UserID=_id, StartDate = _StartDate, ExpirationDate = _ExpirationDate };


            if (Request.IsAjaxRequest())
            {

               
                return PartialView("PotCtrlList_PageList", ScaleRecordBLL.GetListByPage(grv));
              

            }


            PagedList<ScaleRecord> model = ScaleRecordBLL.GetListByPage(grv);

            grv.DataList = model;

            return View(grv);





        }

        [ActionName("config.xml")]
        public ActionResult config()
        {


            return new XmlResult(new Config
            {
                debugMode = true,
                port = 5622,

#if Debug
                serverIp = "192.168.1.17",
#endif
#if TEST
                              //  serverIp = "115.159.57.23",
                                 serverIp = "182.254.148.116",
#endif
#if R17
                                serverIp = "192.168.1.17",
#endif
#if Release
                                serverIp = "115.159.3.167",
#endif
                version = "1.0",
                clientVer = "1.004",
                agent = 208
            });



        }

        [QueryValues]
        public ActionResult Model(Dictionary<string, string> queryvalues)
        {
            int page = queryvalues.ContainsKey("page") ? Convert.ToInt32(queryvalues["page"]) : 1;
            string name = queryvalues.ContainsKey("Value") ? queryvalues["Value"] : "";
            ViewData["Value"] = name;
            PagedList<ModelBaseData> model = ScaleRecordBLL.GetModelByPage(page , name);
            return View(model);
        }
        
        [QueryValues]
        public ActionResult DataModelView(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            int isFirst = queryvalues.ContainsKey("isFirst") ? Convert.ToInt32(queryvalues["isFirst"]) : 0;
            ModelBaseData model = new ModelBaseData();
            //根据ID获取模版数据
            PagedList<ModelBaseData> t = ScaleRecordBLL.GetModelByID(id); 
            if (t.Count > 0)
            {
                ViewData["id"] = id;
                model.ID = id;
                model.ModelName = t[0].ModelName;
                //根据参数生成查询控件
                model.Para = t[0].Para;
                //不是首次查询
                if (isFirst == 0) {
                    string[] parameter = new string[queryvalues.Count];
                    foreach (string key in queryvalues.Keys)
                    {
                        //使用选择的值(value)替换脚本中的变量({para})
                        t[0].Model = t[0].Model.Replace("{" + key + "}" , queryvalues[key]);
                        ViewData[key] = queryvalues[key];
                    }
                    model.Model = t[0].Model;
                    try
                    {
                        model.DataList = new List<object>(ScaleRecordBLL.GetModelData(model.Model ,model.ID));
                    }
                    catch(Exception ex) {
                        model.isError = ex.Message; 
                    }
                }  
            }
            else {
                ViewData["id"] = 0;
                model.ID = 0;
                model.Para = "";
                model.DataList = "";
            }                     
            return View(model);
        }

        [QueryValues]
        public ActionResult DataModelEdit(Dictionary<string, string> queryvalues)
        {
            int id = queryvalues.ContainsKey("id") ? Convert.ToInt32(queryvalues["id"]) : 0;
            ModelBaseData model = new ModelBaseData();
            if (Request.IsAjaxRequest())
            {
                //1:begindate:开始时间_1:enddate:结束时间_2:agent:代理ID
                model.Para = "";
                foreach (string key in queryvalues.Keys)
                {
                    switch (key)
                    {
                        case "ID":
                            model.ID = Convert.ToInt32(queryvalues[key]);
                            continue;
                        case "ModelName":
                            model.ModelName = queryvalues[key];
                            continue;
                        case "Model":
                            model.Model = queryvalues[key];
                            continue;
                        default:
                            string[] sArray = queryvalues[key].ToString().Split(',');
                            if(sArray.Length !=2 )
                            {
                                return Json(new { result = Result.ParaFormError });
                            }
                            else
                            {
                                model.Para += "_" + sArray[1] + ":" + key + ":" + ((sArray[0] == "")? key: sArray[0]);
                                continue;
                            }
                    }
                }
                int result = ScaleRecordBLL.AddModel(model);
                if (result == 1)
                {
                    return Json(new { result = Result.Normal });
                }
                else if (result == 0)
                {
                    return Json(new { result = Result.ResultError });
                }
                else
                {
                    return Json(new { result = Result.ResultExcept });
                }
            }            
            //根据ID获取模版数据
            PagedList<ModelBaseData> t = ScaleRecordBLL.GetModelByID(id);
            if(t.Count > 0) {
                model = t[0];
            }
            return View(model);
        }

    }
}