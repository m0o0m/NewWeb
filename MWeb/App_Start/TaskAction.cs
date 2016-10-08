using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Web.Caching;
using GL.Data;
using StackExchange.Redis;

using log4net;
using Newtonsoft.Json;
using System.IO;
using GL.Data.BLL;
using GL.Data.View;
using Webdiyer.WebControls.Mvc;
using GL.Data.Model;
using System.Threading;
using MWeb.Models;

/// <summary>
///Action 的摘要说明
/// </summary>
public static class TaskAction
{

    public static void TaskRegister()
    {

        /*  
            http://localhost:25664/api/OnLinePlay/GetOnLineUser?pageSize=10&pageIndex=1
        */
        Thread t = new Thread(new ThreadStart(TaskAction.SetContent));
        t.Start();
    }

  


    /// <summary>
    /// 定时器委托任务 调用的方法
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public static void SetContent()
    {
     //   ILog log = LogManager.GetLogger("TaskAction");
       // log.Info("牌局解析:进入SetContent方法" + DateTime.Now.ToString());


       

        while (true)
        {
           // log.Info("牌局解析:执行 while (true)" + DateTime.Now.ToString());

            try
            {
                DateTime endTime = GetDataBaseTime();
                //德州扑克数据解析
                TexasGameLog(endTime);
                //中发白数据解析
               ScaleGameLog(endTime);
                //十二生肖数据解析
               ZodiacGameLog(endTime);
                //小马快跑数据解析
               HorseGameLog(endTime);
                //奔驰宝马数据解析
               CarGameLog(endTime);
                //百人德州扑克数据解析
               TexProGameLog(endTime);
            }
            catch
            {
              //  log.Info("牌局解析:异常" + DateTime.Now.ToString());
            }


            ////int curm2 = DateTime.Now.Minute;
            ////int j = 0;
            ////if (curm2 >= 30)
            ////{
            ////    j = Math.Abs(60 - curm2);
            ////}
            ////else
            ////{
            ////    j = Math.Abs(30 - curm2);
            ////}
            //log.Info("牌局解析:while(true)牌局解析暂停" + j+"分钟");
            ////暂停j分钟
            //Thread.Sleep(j * 60 * 1000);

          //  log.Info("牌局解析:PaiJuAfter" + DateTime.Now.ToString());
            SystemPayBLL.PaiJuAfter();
          //  log.Info("牌局解析:PaiJuAfter" + DateTime.Now.ToString());

            Thread.Sleep(10 * 60 * 1000);

        }

    }

    private static DateTime GetDataBaseTime() {
        DateTime dt = GameDataBLL.GetDataBaseTime();
        return dt.AddSeconds(-1);
    }

    private static void TexasGameLog(DateTime endTime)
    {
        //查询开始时间

      //  ILog log = LogManager.GetLogger("TaskAction");
      //  log.Info("开始德州扑克牌局分析#################");

        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Texas", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

     //   log.Info("开始时间:" + grv.StartDate);
      //  log.Info("结束时间:" + grv.ExpirationDate);

        IEnumerable<TexasGameRecord> data = GameDataBLL.GetListForTexas(grv);

     //   log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


        List<CommonGameData> resdata = new List<CommonGameData>();



        foreach (TexasGameRecord m in data)
        {

            var userList = m.UserData.Split('_').ToList();
            userList.RemoveAt(userList.Count - 1);
            var j = userList.Count;
            for (int i = 0; i < j; i++)
            {
                CommonGameData com = new CommonGameData();
                var userData = userList[i].Split(',').ToList();
                int tem = 0;

                DateTime t = m.CreateTime;
                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
                //int 房间ID = m.RoomID;
                //decimal 牌局号 = m.Round;
                string 盲注 = m.BaseScore;
                com.DownType = m.BaseScore;
                com.TotalTime = m.BoardTime;
                string[] s = m.BaseScore.Split('/');
                int v = int.Parse(s[0]);
                tem = v;
                //int 服务费 = m.Service;


                var wj = userData[2];
                com.UserID = int.Parse(wj);

                decimal d = Convert.ToDecimal(userData[4]);
                if (tem <= 100) { com.Initial = d; com.InitialCount = 1; }
                else if (tem >= 5000) { com.HighRank = d; com.HighRankCount = 1; }
                else { com.Secondary = d; com.SecondaryCount = 1; }

                com.Key = com.UserID + com.CountDate.ToString();

                com.Key2 = com.UserID + com.CountDate.ToString()+com.DownType;

                resdata.Add(com);
            }
        }
        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
        List<CommonGameData> sumData = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
            co.InitialCount = sl.Sum(m => m.InitialCount);
            co.HighRankL = sl.Where(m => m.HighRank < 0).Sum(m => m.HighRank);
            co.HighRankW = sl.Where(m => m.HighRank > 0).Sum(m => m.HighRank);
            co.HighRankCount = sl.Sum(m => m.HighRankCount);
            co.SecondaryL = sl.Where(m => m.Secondary < 0).Sum(m => m.Secondary);
            co.SecondaryW = sl.Where(m => m.Secondary > 0).Sum(m => m.Secondary);
            co.SecondaryCount = sl.Sum(m => m.SecondaryCount);

            sumData.Add(co);
        }
        string sql = "";
        foreach (CommonGameData comdata in sumData)
        {


            sql = sql + @"replace into record.Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W
)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0) + " + comdata.InitialCount + @" ,ifnull(b.Texas_LAward_L,0)+ " + comdata.InitialL + @",ifnull(b.Texas_LAward_W,0)+ " + comdata.InitialW + @" ,
ifnull(b.Texas_MCount,0)+ " + comdata.SecondaryCount + @",ifnull(b.Texas_MAward_L,0) + " + comdata.SecondaryL + @",ifnull(b.Texas_MAward_W,0)+ " + comdata.SecondaryW + @" ,
ifnull(b.Texas_HCount,0)+ " + comdata.HighRankCount + @",ifnull(b.Texas_HAward_L,0)+ " + comdata.HighRankL + @",ifnull(b.Texas_HAward_W,0)+ " + comdata.HighRankW + @",
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0)  ,ifnull(b.Zodiac_Award_W,0)  ,
ifnull(b.Horse_Count,0)   ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0) ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0),ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0) 
from (select " + comdata.UserID + @" userid ) a left join (
select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
";
        }









        IEnumerable<IGrouping<string, CommonGameData>> query2 = resdata.GroupBy(m => m.Key2);
        List<CommonGameData> sumData2 = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info2 in query2)
        {
            List<CommonGameData> sl = info2.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;

            co.InitialCount = sl.Sum(m => m.InitialCount) + sl.Sum(m => m.HighRankCount) + sl.Sum(m => m.SecondaryCount);
            co.TotalTime = sl.Sum(m => m.TotalTime);
            co.DownType = sl[0].DownType;

            sumData2.Add(co);
        }
     
        foreach (CommonGameData comdata in sumData2)
        {
            sql = sql + @"

replace into Clearing_GameDesc(UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime)
select a.UserID ,date('"+comdata.CountDate+@"') ,15 ,'"+comdata.DownType+@"' ,"+comdata.InitialCount+@" + ifnull(b.GameCount ,0) ,"+comdata.TotalTime+@" + ifnull(b.GameTime ,0)
from (select "+comdata.UserID+ @" UserID)a 
  left join (
    select UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime from Clearing_GameDesc 
    where UserID = " + comdata.UserID+@" and CountDate = date('"+comdata.CountDate+@"') and GameType = 15 and BetType = '"+comdata.DownType+@"'
  )b on a.UserID = b.UserID ;

";
        }






        if (sql != "")
        {

            GameDataBLL.Add(sql);
        }

        GameDataBLL.UpdateBeginTimeForGame(grv);
        //修改时间



    }


    private static void ScaleGameLog(DateTime endTime)
    {
        //查询开始时间

     //   ILog log = LogManager.GetLogger("TaskAction");
      //  log.Info("开始中发白牌局分析#################");

        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Scale", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

     //   log.Info("开始时间:" + grv.StartDate);
    //    log.Info("结束时间:" + grv.ExpirationDate);

        IEnumerable<ScaleGameRecord> data = GameDataBLL.GetListForScale(grv);

     //   log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


        List<CommonGameData> resdata = new List<CommonGameData>();



        foreach (ScaleGameRecord m in data)
        {

            var userList = m.UserData.Split('_').ToList();
            userList.RemoveAt(userList.Count - 1);
            var j = userList.Count;
            for (int i = 0; i < j; i++)
            {
                CommonGameData com = new CommonGameData();
                var userData = userList[i].Split(',').ToList();
                int tem = 0;

                DateTime t = m.CreateTime;
                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
                //int 房间ID = m.RoomID;
                //decimal 牌局号 = m.Round;

                //int 服务费 = m.Service;


                var wj = userData[0];
                com.UserID = int.Parse(wj);

                decimal d = 0;

                if (userData.Count() >= 5)
                {
                    d = Convert.ToDecimal(userData[5].Trim(')'));
                }
                else
                {
                    d = 0;
                }


                com.Initial = d; com.InitialCount = 1;
                com.Key = com.UserID + com.CountDate.ToString();
                com.Key2 = com.UserID + com.CountDate.ToString() + com.DownType;

                com.TotalTime = m.BoardTime;
                resdata.Add(com);
            }
        }
        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
        List<CommonGameData> sumData = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
            co.InitialCount = sl.Sum(m => m.InitialCount);

            sumData.Add(co);
        }
        string sql = "";
        foreach (CommonGameData comdata in sumData)
        {
            //             sql = sql+ @"
            //replace into record.Clearing_Game(UserID ,CountDate ,Scale_Count ,Scale_Award_L ,Scale_Award_W )
            //select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Scale_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Scale_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Scale_Award_W,0) + " + comdata.InitialW + @" 
            //from (select " + comdata.UserID + @" userid ) a left join (
            //select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
            //";


            sql = sql + @"replace into record.Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) + " + comdata.InitialL + @" ,ifnull(b.Scale_Award_W,0) + " + comdata.InitialW + @" ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0) ,ifnull(b.Zodiac_Award_W,0) ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0) 
from (select " + comdata.UserID + @" userid ) a left join (
select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
";
        }




        IEnumerable<IGrouping<string, CommonGameData>> query2 = resdata.GroupBy(m => m.Key2);
        List<CommonGameData> sumData2 = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query2)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
            //co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
            //co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
            co.InitialCount = sl.Sum(m => m.InitialCount);
            co.DownType = "13";
            co.TotalTime = sl.Sum(m => m.TotalTime);
            sumData2.Add(co);
        }
        foreach (CommonGameData comdata in sumData2)
        {
            sql = sql + @"
replace into Clearing_GameDesc(UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime)
select a.UserID ,date('" + comdata.CountDate + @"') ,13 ," + comdata.DownType + @" ," + comdata.InitialCount + @" + ifnull(b.GameCount ,0) ," + comdata.TotalTime + @" + ifnull(b.GameTime ,0)
from (select " + comdata.UserID + @" UserID)a 
  left join (
    select UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime from Clearing_GameDesc 
    where UserID = " + comdata.UserID + @" and CountDate = date('" + comdata.CountDate + @"') and GameType = 13 and BetType = '" + comdata.DownType + @"'
  )b on a.UserID = b.UserID ;

";
        }




        if (sql != "")
        { 
            bool res = GameDataBLL.Add(sql);

        }

        GameDataBLL.UpdateBeginTimeForGame(grv);
        //修改时间


    }

    private static void ZodiacGameLog(DateTime endTime)
    {
        //查询开始时间

     //   ILog log = LogManager.GetLogger("TaskAction");
     //   log.Info("开始十二生肖牌局分析#################");

        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Zodiac", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

     //   log.Info("开始时间:" + grv.StartDate);
      //  log.Info("结束时间:" + grv.ExpirationDate);

        IEnumerable<ZodiacGameRecord> data = GameDataBLL.GetListForZodiac(grv);

      //  log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


        List<CommonGameData> resdata = new List<CommonGameData>();



        foreach (ZodiacGameRecord m in data)
        {

            string tempStr = m.UserData.Replace("_0,0,0,(", "");

            var tempStr2 = tempStr.Split(new string[] { ",(_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string wjxy = "";
            string zj = "";
            int k = 0;
            if (tempStr2.Count == 2)
            {//如果是庄家
                zj = tempStr2[0];
                k = -1;
                wjxy = tempStr2[1];
                tempStr = tempStr2[0] + "_" + tempStr2[1];
            }
            else
            {
                wjxy = tempStr2[0];
                k = 0;
            }
            var userList = tempStr.Split('_').ToList();
            userList.RemoveAt(userList.Count - 1);
            var j = userList.Count;



            for (int i = 0; i < j; i++)
            {
                CommonGameData com = new CommonGameData();
                var userData1 = userList[i].Split(new string[] { ",(" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var userData = userList[i].Split(',').ToList();


                DateTime t = m.CreateTime;
                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
                //int 房间ID = m.RoomID;
                //decimal 牌局号 = m.Round;

                //int 服务费 = m.Service;


                var wj = userData[0];
                com.UserID = int.Parse(wj);

                decimal d = Convert.ToDecimal(userData[2]);
                com.Initial = d; com.InitialCount = 1;
                com.Key = com.UserID + com.CountDate.ToString();
                com.Key2 = com.UserID + com.CountDate.ToString() + com.DownType;
                com.TotalTime = m.BoardTime;
                resdata.Add(com);
            }
        }
        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
        List<CommonGameData> sumData = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
            co.InitialCount = sl.Sum(m => m.InitialCount);

            sumData.Add(co);
        }
        string sql = "";
        foreach (CommonGameData comdata in sumData)
        {
            //             sql = sql+ @"
            //replace into record.Clearing_Game(UserID ,CountDate ,Zodiac_Count ,Zodiac_Award_L ,Zodiac_Award_W )
            //select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Zodiac_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Zodiac_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Zodiac_Award_W,0) + " + comdata.InitialW + @" 
            //from (select " + comdata.UserID + @" userid ) a left join (
            //select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
            //";

            sql = sql + @"replace into record.Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0)+ " + comdata.InitialCount + @"  ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0)+ " + comdata.InitialL + @"  ,ifnull(b.Zodiac_Award_W,0) + " + comdata.InitialW + @" ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0) 
from (select " + comdata.UserID + @" userid ) a left join (
select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
";

        }





        IEnumerable<IGrouping<string, CommonGameData>> query2 = resdata.GroupBy(m => m.Key2);
        List<CommonGameData> sumData2 = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query2)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
      
            co.InitialCount = sl.Sum(m => m.InitialCount);
            co.DownType = "14";
            co.TotalTime = sl.Sum(m => m.TotalTime);
            sumData.Add(co);
        }
        foreach (CommonGameData comdata in sumData2)
        {
            sql = sql + @"
replace into Clearing_GameDesc(UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime)
select a.UserID ,date('" + comdata.CountDate + @"') ,14 ," + comdata.DownType + @" ," + comdata.InitialCount + @" + ifnull(b.GameCount ,0) ," + comdata.TotalTime + @" + ifnull(b.GameTime ,0)
from (select " + comdata.UserID + @" UserID)a 
  left join (
    select UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime from Clearing_GameDesc 
    where UserID = " + comdata.UserID + @" and CountDate = date('" + comdata.CountDate + @"') and GameType = 14 and BetType = '" + comdata.DownType + @"'
  )b on a.UserID = b.UserID ;

";
        }




        if (sql != "")
        {
            GameDataBLL.Add(sql);
        }
        GameDataBLL.UpdateBeginTimeForGame(grv);
        //修改时间



    }

    private static void HorseGameLog(DateTime endTime)
    {
        //查询开始时间

     //   ILog log = LogManager.GetLogger("TaskAction");
     //   log.Info("开始小马快跑牌局分析#################");

        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Horse", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

       // log.Info("开始时间:" + grv.StartDate);
      //  log.Info("结束时间:" + grv.ExpirationDate);

        IEnumerable<HorseGameRecord> data = GameDataBLL.GetListForHorse(grv);

       // log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


        List<CommonGameData> resdata = new List<CommonGameData>();



        foreach (HorseGameRecord m in data)
        {

            string tempStr = m.UserData.Replace("_0,0,0,(", "");

            var tempStr2 = tempStr.Split(new string[] { ",(_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string wjxy = "";
            string zj = "";
            int k = 0;
            if (tempStr2.Count == 2)
            {//如果是庄家
                zj = tempStr2[0];
                k = -1;
                wjxy = tempStr2[1];
                tempStr = tempStr2[0] + "_" + tempStr2[1];
            }
            else
            {
                wjxy = tempStr2[0];
                k = 0;
            }
            var userList = tempStr.Split('_').ToList();
            userList.RemoveAt(userList.Count - 1);
            var j = userList.Count;



            for (int i = 0; i < j; i++)
            {
                CommonGameData com = new CommonGameData();
                var userData1 = userList[i].Split(new string[] { ",(" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var userData = userList[i].Split(',').ToList();


                DateTime t = m.CreateTime;
                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
                //int 房间ID = m.RoomID;
                //decimal 牌局号 = m.Round;

                //int 服务费 = m.Service;


                var wj = userData[0];
                com.UserID = int.Parse(wj);

                decimal d = Convert.ToDecimal(userData[2]);
                com.Initial = d; com.InitialCount = 1;
                com.Key = com.UserID + com.CountDate.ToString();
                resdata.Add(com);
            }
        }
        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
        List<CommonGameData> sumData = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
            co.InitialCount = sl.Sum(m => m.InitialCount);

            sumData.Add(co);
        }
        string sql = "";
        foreach (CommonGameData comdata in sumData)
        {
            //             sql = sql+ @"
            //replace into record.Clearing_Game(UserID ,CountDate ,Horse_Count ,Horse_Award_L ,Horse_Award_W )
            //select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Horse_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Horse_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Horse_Award_W,0) + " + comdata.InitialW + @" 
            //from (select " + comdata.UserID + @" userid ) a left join (
            //select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
            //";

            sql = sql + @"replace into record.Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0)  ,ifnull(b.Zodiac_Award_W,0)  ,
ifnull(b.Horse_Count,0) + " + comdata.InitialCount + @"  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0) + " + comdata.InitialL + @" ,ifnull(b.Horse_Award_W,0)+ " + comdata.InitialW + @"  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0)
from (select " + comdata.UserID + @" userid ) a left join (
select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
";

        }
        if (sql != "")
        {
            GameDataBLL.Add(sql);
        }
        GameDataBLL.UpdateBeginTimeForGame(grv);
        //修改时间

    }

    private static void CarGameLog(DateTime endTime)
    {

        //查询开始时间

       // ILog log = LogManager.GetLogger("TaskAction");
       // log.Info("开始奔驰宝马牌局分析#################");

        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Car", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

       // log.Info("开始时间:" + grv.StartDate);
       // log.Info("结束时间:" + grv.ExpirationDate);

        IEnumerable<CarGameRecord> data = GameDataBLL.GetListForCar(grv);

        //log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


        List<CommonGameData> resdata = new List<CommonGameData>();



        foreach (CarGameRecord m in data)
        {

            string tempStr = m.UserData.Replace("_0,0,0,(", "");

            var tempStr2 = tempStr.Split(new string[] { ",(_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string wjxy = "";
            string zj = "";
            int k = 0;
            if (tempStr2.Count == 2)
            {//如果是庄家
                zj = tempStr2[0];
                k = -1;
                wjxy = tempStr2[1];
                tempStr = tempStr2[0] + "_" + tempStr2[1];
            }
            else
            {
                wjxy = tempStr2[0];
                k = 0;
            }
            var userList = tempStr.Split('_').ToList();
            userList.RemoveAt(userList.Count - 1);
            var j = userList.Count;



            for (int i = 0; i < j; i++)
            {
                CommonGameData com = new CommonGameData();
                var userData1 = userList[i].Split(new string[] { ",(" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var userData = userList[i].Split(',').ToList();


                DateTime t = m.CreateTime;
                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
                //int 房间ID = m.RoomID;
                //decimal 牌局号 = m.Round;

                //int 服务费 = m.Service;


                var wj = userData[0];
                com.UserID = int.Parse(wj);

                decimal d = Convert.ToDecimal(userData[2]);
                com.Initial = d; com.InitialCount = 1;
                com.Key = com.UserID + com.CountDate.ToString();
                com.Key2 = com.UserID + com.CountDate.ToString() + com.DownType;

                com.TotalTime = m.BoardTime;
                resdata.Add(com);
            }
        }
        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
        List<CommonGameData> sumData = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
            co.InitialCount = sl.Sum(m => m.InitialCount);

            sumData.Add(co);
        }
        string sql = "";
        foreach (CommonGameData comdata in sumData)
        {
            //             sql = sql+ @"
            //replace into record.Clearing_Game(UserID ,CountDate ,Car_Count ,Car_Award_L ,Car_Award_W )
            //select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Car_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Car_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Car_Award_W,0) + " + comdata.InitialW + @" 
            //from (select " + comdata.UserID + @" userid ) a left join (
            //select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
            //";

            sql = sql + @"replace into record.Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0)  ,ifnull(b.Zodiac_Award_W,0)  ,
ifnull(b.Horse_Count,0)   ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0) ,
ifnull(b.Car_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0)+ " + comdata.InitialL + @" ,ifnull(b.Car_Award_W,0) + " + comdata.InitialW + @", 
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0)
from (select " + comdata.UserID + @" userid ) a left join (
select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
";



        }




        IEnumerable<IGrouping<string, CommonGameData>> query2 = resdata.GroupBy(m => m.Key2);
        List<CommonGameData> sumData2 = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query2)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
          
            co.InitialCount = sl.Sum(m => m.InitialCount);
            co.DownType = "17";
            co.TotalTime = sl.Sum(m => m.TotalTime);
            sumData2.Add(co);
           
        }
        foreach (CommonGameData comdata in sumData2)
        {
            sql = sql + @"
replace into Clearing_GameDesc(UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime)
select a.UserID ,date('" + comdata.CountDate + @"') ,17 ," + comdata.DownType + @" ," + comdata.InitialCount + @" + ifnull(b.GameCount ,0) ," + comdata.TotalTime + @" + ifnull(b.GameTime ,0)
from (select " + comdata.UserID + @" UserID)a 
  left join (
    select UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime from Clearing_GameDesc 
    where UserID = " + comdata.UserID + @" and CountDate = date('" + comdata.CountDate + @"') and GameType = 17 and BetType = '" + comdata.DownType + @"'
  )b on a.UserID = b.UserID ;

";
        }

        if (sql != "")
        {
          //  log.Info(sql);
            GameDataBLL.Add(sql);
        }
        GameDataBLL.UpdateBeginTimeForGame(grv);
        //修改时间


    }

    private static void TexProGameLog(DateTime endTime)
    {

        //查询开始时间

    //    ILog log = LogManager.GetLogger("TaskAction");
    //    log.Info("开始百人德州牌局分析#################");

        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_TexPro", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };
     //   log.Info("结束时间:" + grv.ExpirationDate);
        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

      //  log.Info("开始时间:" + grv.StartDate);
      //  log.Info("结束时间:" + grv.ExpirationDate);

        IEnumerable<ScaleGameRecord> data = GameDataBLL.GetListForTexPro(grv);

      //  log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


        List<CommonGameData> resdata = new List<CommonGameData>();



        foreach (ScaleGameRecord m in data)
        {

            var userList = m.UserData.Split('_').ToList();
            userList.RemoveAt(userList.Count - 1);
            var j = userList.Count;
            for (int i = 0; i < j; i++)
            {
                CommonGameData com = new CommonGameData();
                var userData = userList[i].Split(',').ToList();
                int tem = 0;

                DateTime t = m.CreateTime;
                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
                //int 房间ID = m.RoomID;
                //decimal 牌局号 = m.Round;

                //int 服务费 = m.Service;


                var wj = userData[0];
                com.UserID = int.Parse(wj);

              
                decimal  d = Convert.ToDecimal(userData[2].Trim(')'));

             



                com.Initial = d; com.InitialCount = 1;
                com.Key = com.UserID + com.CountDate.ToString();
                com.Key2 = com.UserID + com.CountDate.ToString() + com.DownType;
                com.TotalTime = m.BoardTime;
                resdata.Add(com);
            }
        }
        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
        List<CommonGameData> sumData = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
            co.InitialCount = sl.Sum(m => m.InitialCount);

            sumData.Add(co);
        }
        string sql = "";
        foreach (CommonGameData comdata in sumData)
        {
            //             sql = sql+ @"
            //replace into record.Clearing_Game(UserID ,CountDate ,Scale_Count ,Scale_Award_L ,Scale_Award_W )
            //select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Scale_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Scale_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Scale_Award_W,0) + " + comdata.InitialW + @" 
            //from (select " + comdata.UserID + @" userid ) a left join (
            //select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
            //";


            sql = sql + @"replace into record.Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0) ,ifnull(b.Zodiac_Award_W,0) ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0)+ " + comdata.InitialL + @" ,ifnull(b.Hundred_Award_W,0) + " + comdata.InitialW+@" 
from (select " + comdata.UserID + @" userid ) a left join (
select * from record.Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
";
        }


        IEnumerable<IGrouping<string, CommonGameData>> query2 = resdata.GroupBy(m => m.Key2);
        List<CommonGameData> sumData2 = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query2)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;
       
            co.InitialCount = sl.Sum(m => m.InitialCount);
            co.DownType = "18";
            co.TotalTime = sl.Sum(m => m.TotalTime);
            sumData2.Add(co);
        }
        foreach (CommonGameData comdata in sumData2)
        {
            sql = sql + @"
replace into Clearing_GameDesc(UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime)
select a.UserID ,date('" + comdata.CountDate + @"') ,18 ," + comdata.DownType + @" ," + comdata.InitialCount + @" + ifnull(b.GameCount ,0) ," + comdata.TotalTime + @" + ifnull(b.GameTime ,0)
from (select " + comdata.UserID + @" UserID)a 
  left join (
    select UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime from Clearing_GameDesc 
    where UserID = " + comdata.UserID + @" and CountDate = date('" + comdata.CountDate + @"') and GameType = 18 and BetType = '" + comdata.DownType + @"'
  )b on a.UserID = b.UserID ;

";
        }







       // log.Info("百人德州sql:" + sql);
        if (sql != "")
        {
            bool res = GameDataBLL.Add(sql);

        }

        GameDataBLL.UpdateBeginTimeForGame(grv);
        //修改时间

    }


}







public class CommonGameData
{
    public string Key { get; set; }

    public string Key2 { get; set; }
    public int UserID { get; set; }

    public DateTime CountDate { get; set; }

    public decimal Initial { get; set; }

    public int InitialCount { get; set; }
    public decimal InitialL { get; set; }
    public decimal InitialW { get; set; }

    public decimal Secondary { get; set; }
    public decimal SecondaryL { get; set; }
    public decimal SecondaryW { get; set; }
    public int SecondaryCount { get; set; }

    public decimal HighRank { get; set; }
    public decimal HighRankL { get; set; }
    public decimal HighRankW { get; set; }
    public int HighRankCount { get; set; }


    public string DownType { get; set; }//下注类型

    public Int64 TotalTime { get; set; }
}





