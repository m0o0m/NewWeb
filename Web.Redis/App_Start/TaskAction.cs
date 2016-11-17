using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Web.Caching;
using GL.Data;
using StackExchange.Redis;
using Web.Redis.Models;
using log4net;
using Newtonsoft.Json;
using System.IO;
using GL.Data.BLL;

/// <summary>
///Action 的摘要说明
/// </summary>
public static class TaskAction
{

    public static void TaskRegister() {

        /*  
            http://localhost:25664/api/OnLinePlay/GetOnLineUser?pageSize=10&pageIndex=1
        */
        //SetContent(null,null);
        //System.Timers.Timer myTimer = new System.Timers.Timer(1000 * 60*2);
        ////TaskAction.SetContent 表示要调用的方法
        //myTimer.Elapsed += new System.Timers.ElapsedEventHandler(TaskAction.SetContent);
        //myTimer.Enabled = true;
        //myTimer.AutoReset = true;
    }
    /// <summary>
    /// 定时器委托任务 调用的方法
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public static void SetContent(object source, ElapsedEventArgs e)
    {
         ILog log = LogManager.GetLogger("TaskAction");
         Cache ca = HttpRuntime.Cache;

        try
        {
           
               List<RankModel> rankAll = new List<RankModel>();
              
               var onlineUseralle = ServiceStackManage.Redis.GetDatabase().HashGetAll("UserInfo", CommandFlags.None);//如果可以访问，就加入到缓存
          
               for (int i = 0; i < onlineUseralle.Count(); i++)
               {
                    RankModel model = new RankModel();
                    model.Name = onlineUseralle[i].Name;
                    model.Value = onlineUseralle[i].Value;
                    string[] s = model.Value.Split('_');
                    model.VipGrade = Convert.ToInt32(s[4]);
                    rankAll.Add(model);
                }
                if (onlineUseralle != null) {
                    if (onlineUseralle.Length != 0) { 
                         ca.Insert("onlineList", rankAll);  //这里给数据加缓存
                         string json = JsonConvert.SerializeObject(rankAll.Skip(0).Take(200));

                    OnLineInfoBLL.InsertNewJson(new GL.Data.Model.OnLineInfo { CreateTime = DateTime.Now, OnLineInfoJson = json });

                        
                    }
                }
        }
        catch (Exception ex)
        {//如果连不上redis
            log.Error("TaskAction(定时读取在线人数到缓存):"+ex.Message + "\n");
            List<RankModel> rankAll = ca["onlineList"] as List<RankModel>;
            if (rankAll == null) {
                string rJson = OnLineInfoBLL.GetNewJson();
                rankAll = (List<RankModel>)JsonConvert.DeserializeObject<List<RankModel>>(rJson);
            }
            if (rankAll.Count==0)
            {
                string rJson = OnLineInfoBLL.GetNewJson();
                rankAll = (List<RankModel>)JsonConvert.DeserializeObject<List<RankModel>>(rJson);
            }
            ca.Insert("onlineList", rankAll);  //这里给数据加缓存
        }
    }
    /// <summary>
    /// 应用池回收的时候调用的方法
    /// </summary>
    public static void SetContent()
    {
      
    }
}