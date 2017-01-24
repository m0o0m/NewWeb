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
using GL.Command.DBUtility;

/// <summary>
///Action 的摘要说明
/// </summary>
public static class TaskAction
{
    public static string database3 = PubConstant.GetConnectionString("database3");
    public static void TaskRegister()
    {
        //timerTask

        string timerTask = PubConstant.GetConnectionString("timerTask");

        if (timerTask == "1")
        {
            Thread t = new Thread(new ThreadStart(TaskAction.SetContent));
            t.Start();



            //Thread t2 = new Thread(new ThreadStart(TaskAction.SetContentShuihu));
            //t2.Start();


        }

    }




    /// <summary>
    /// 定时器委托任务 调用的方法
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public static void SetContent()
    {
         ILog log = LogManager.GetLogger("TaskAction");
        // log.Info("牌局解析:进入SetContent方法" + DateTime.Now.ToString());




        while (true)
        {
            // log.Info("牌局解析:执行 while (true)" + DateTime.Now.ToString());

            try
            {
                DateTime endTime = GetDataBaseTime();
                log.Info("开始德州扑克");
                  // //德州扑克数据解析
                  TexasGameLog(endTime);
                log.Info("开始中发白");
                // 中发白数据解析
                ScaleGameLog(endTime);
                log.Info("开始十二生肖数据解析");
                //十二生肖数据解析
                ZodiacGameLog(endTime);
                log.Info("开始小马快跑数据解析");
                //小马快跑数据解析
                HorseGameLog(endTime);
                log.Info("开始奔驰宝马数据解析");
                //奔驰宝马数据解析
                CarGameLog(endTime);
                log.Info("开始百人德州扑克数据解析");
                //百人德州扑克数据解析
                TexProGameLog(endTime);
                //水浒传数据解析
                //  ShuihuGameLog(endTime);
                log.Info("开始百家乐数据解析");
                //百家乐数据解析
                BaiJiaLeGameLog(endTime);
                //水果机数据解析
              //  ShuiguojiGameLog(endTime);
                //连环夺宝
               // SerialGameLog(endTime);
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
            log.Info("开始PaiJuAfter解析");
            SystemPayBLL.PaiJuAfter();
            //  log.Info("牌局解析:PaiJuAfter" + DateTime.Now.ToString());

            Thread.Sleep(5 * 60 * 1000);

        }

    }


    public static void SetContentShuihu()
    {







        while (true)
        {
            DateTime endTime = GetDataBaseTime();


            GetShuihuLog(endTime);




            Thread.Sleep(5 * 60 * 1000);

        }
    }

    public static void GetShuihuLog(DateTime endTime)
    {
        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Shuihu", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };
        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);
        //2016-10-13 10:22:51

        DateTime bCheckTime = TransStringToDateTime(grv.StartDate);
        DateTime eCheckTime = endTime;


        if (DateTime.Compare(bCheckTime, eCheckTime) >= 0)
        {
            //开始时间比结束时间大
            return;
        }

        DateTime chEnd = bCheckTime.AddHours(2);
        if (endTime >= chEnd)
        {
            endTime = chEnd;
        }

        //开始时间StartDate   结束时间endTime
        //判断开始时间和结束时间是不是跨天了，如果跨天了，那么就吧结束时间修改成零点

        if (bCheckTime.Year != eCheckTime.Year || bCheckTime.Month != eCheckTime.Month || bCheckTime.Day != eCheckTime.Day)
        {
            //说明不是同一天,修改时间为开始时间的第二天的零点
            endTime = TransStringToDate(bCheckTime.AddDays(1).ToString());
            grv.ExpirationDate = endTime.ToString();
        }

        //查询数据
        IEnumerable<ShuihuGameRecord> data = GameDataBLL.GetListForShuihu(grv);

        if (data.Count() >= 1000)
        {

        }



        List<ShuihuPot> ShuihuPotList = new List<ShuihuPot>();
        List<ShuihuMary> ShuihuMaryList = new List<ShuihuMary>();
        foreach (ShuihuGameRecord item in data)
        {
            //彩池解析
            ShuihuPot shuihuPot = new ShuihuPot();
            string potDetail = item.PotDetail;
            if (!string.IsNullOrEmpty(potDetail))
            {
                string[] potDetails = potDetail.Split(',');
                shuihuPot.BeforeGold = Convert.ToInt64(potDetails[0]);
                shuihuPot.Board = item.Board;
                shuihuPot.CreateTime = item.CreateTime;
                shuihuPot.GetGold = Convert.ToInt64(potDetails[1]);
                shuihuPot.RoundID = item.RoundID;
                shuihuPot.UserID = item.UserID;
                ShuihuPotList.Add(shuihuPot);
            }




            string maryDetail = item.MaryDetail;

            if (!string.IsNullOrEmpty(maryDetail))
            {
                string[] maryTemp = maryDetail.Split('|');
                for (int j = 1; j < maryTemp.Length; j++)
                {
                    string maryLun = maryTemp[j];
                    string[] marys = maryLun.Split(',');
                    ShuihuMary shuihuMary = new ShuihuMary();
                    shuihuMary.CreateTime = item.CreateTime;
                    shuihuMary.Board = item.Board;
                    shuihuMary.UserID = item.UserID;
                    shuihuMary.RoundID = item.RoundID;
                    shuihuMary.InitGold = Convert.ToInt64(marys[2]);
                    shuihuMary.WinGold = Convert.ToInt64(marys[3]);
                    shuihuMary.PayGold = item.Bet;
                    ShuihuMaryList.Add(shuihuMary);
                }
            }

        }




        string sql = "";
        if (ShuihuPotList.Count() > 0)
        {
            string potSql = @"
insert into " + database3 + ".Clearing_Shuihupot(CreateTime,Board,UserID,RoundID,BeforeGold,GetGold) ";
            for (var i = 0; i < ShuihuPotList.Count(); i++)
            {
                ShuihuPot itemPot = ShuihuPotList[i];
                if (i == ShuihuPotList.Count() - 1)
                {
                    potSql += "select '" + itemPot.CreateTime + "'," + itemPot.Board + "," + itemPot.UserID + "," + itemPot.RoundID + "," + itemPot.BeforeGold + "," + itemPot.GetGold + " ; ";
                }
                else
                {
                    potSql += "select '" + itemPot.CreateTime + "'," + itemPot.Board + "," + itemPot.UserID + "," + itemPot.RoundID + "," + itemPot.BeforeGold + "," + itemPot.GetGold + "  union all  ";
                }
            }

            sql += potSql;
        }


        if (ShuihuMaryList.Count() > 0)
        {
            string marySql = @"
insert into " + database3 + ".Clearing_Shuihumary(CreateTime,Board,UserID,RoundID,InitGold,WinGold,PayGold) ";
            for (var i = 0; i < ShuihuMaryList.Count(); i++)
            {
                ShuihuMary itemMary = ShuihuMaryList[i];
                if (i == ShuihuMaryList.Count() - 1)
                {
                    marySql += "select '" + itemMary.CreateTime + "'," + itemMary.Board + "," + itemMary.UserID + "," +
                                           itemMary.RoundID + "," + itemMary.InitGold + "," + itemMary.WinGold + "," + itemMary.PayGold + "; ";
                }
                else
                {
                    marySql += "select '" + itemMary.CreateTime + "'," + itemMary.Board + "," + itemMary.UserID + "," +
                                                                itemMary.RoundID + "," + itemMary.InitGold + "," + itemMary.WinGold + "," + itemMary.PayGold + " union all ";
                }
            }

            sql += marySql;
        }




        if (sql != "")
        {

            GameDataBLL.Add(sql);

            SystemPayBLL.ShuihuAfter(grv.StartDate, grv.ExpirationDate);
            //ShuihuAfter
        }

        GameDataBLL.UpdateBeginTimeForGame(grv);
        //修改时间






    }


    private static DateTime GetDataBaseTime()
    {
        DateTime dt = GameDataBLL.GetDataBaseTime();
        return dt.AddSeconds(-1);
    }

    private static DateTime TransStringToDate(string time)
    {
        string[] s = time.Replace("/", "-").Split(' ');
        string[] s1s = s[0].Split('-');
        string[] s2s = s[1].Split(':');
        return new DateTime(Convert.ToInt32(s1s[0]), Convert.ToInt32(s1s[1]), Convert.ToInt32(s1s[2]),
           0, 0, 0);
    }
    private static DateTime TransStringToDateTime(string time)
    {
        string[] s = time.Replace("/", "-").Split(' ');
        string[] s1s = s[0].Split('-');
        string[] s2s = s[1].Split(':');
        return new DateTime(Convert.ToInt32(s1s[0]), Convert.ToInt32(s1s[1]), Convert.ToInt32(s1s[2]),
            Convert.ToInt32(s2s[0]), Convert.ToInt32(s2s[1]), Convert.ToInt32(s2s[2]));
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

                com.Key2 = com.UserID + com.CountDate.ToString() + com.DownType;

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


            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0) + " + comdata.InitialCount + @" ,ifnull(b.Texas_LAward_L,0)+ " + comdata.InitialL + @",ifnull(b.Texas_LAward_W,0)+ " + comdata.InitialW + @" ,
ifnull(b.Texas_MCount,0)+ " + comdata.SecondaryCount + @",ifnull(b.Texas_MAward_L,0) + " + comdata.SecondaryL + @",ifnull(b.Texas_MAward_W,0)+ " + comdata.SecondaryW + @" ,
ifnull(b.Texas_HCount,0)+ " + comdata.HighRankCount + @",ifnull(b.Texas_HAward_L,0)+ " + comdata.HighRankL + @",ifnull(b.Texas_HAward_W,0)+ " + comdata.HighRankW + @",
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0)  ,ifnull(b.Zodiac_Award_W,0)  ,
ifnull(b.Horse_Count,0)   ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0) ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0),ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0),
ifnull(b.BaiJiaLe_Count,0)  ,ifnull(b.BaiJiaLe_Banker,0),ifnull(b.BaiJiaLe_Award_L,0),ifnull(b.BaiJiaLe_Award_W,0),  
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0)
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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
select a.UserID ,date('" + comdata.CountDate + @"') ,15 ,'" + comdata.DownType + @"' ," + comdata.InitialCount + @" + ifnull(b.GameCount ,0) ," + comdata.TotalTime + @" + ifnull(b.GameTime ,0)
from (select " + comdata.UserID + @" UserID)a 
  left join (
    select UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime from Clearing_GameDesc 
    where UserID = " + comdata.UserID + @" and CountDate = date('" + comdata.CountDate + @"') and GameType = 15 and BetType = '" + comdata.DownType + @"'
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
            //  0,989380,0,0,0,0)          10006,0,3000,4610,3010,10088
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


                com.Initial = d;
                com.InitialCount = 1;
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


            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) + " + comdata.InitialL + @" ,ifnull(b.Scale_Award_W,0) + " + comdata.InitialW + @" ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0) ,ifnull(b.Zodiac_Award_W,0) ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0) ,
ifnull(b.BaiJiaLe_Count,0)  ,ifnull(b.BaiJiaLe_Banker,0),ifnull(b.BaiJiaLe_Award_L,0),ifnull(b.BaiJiaLe_Award_W,0),
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0)
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W

from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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


    private static void SerialGameLog(DateTime endTime)
    {
        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_BaiJiaLe", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };
        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);
        IEnumerable<SerialGameRecord> data = GameDataBLL.GetListForSerial(grv);
        List<CommonGameData> resdata = new List<CommonGameData>();
        foreach (SerialGameRecord m in data)
        {
            CommonGameData com = new CommonGameData();
            com.CountDate = new DateTime(m.CreateTime.Year, m.CreateTime.Month, m.CreateTime.Day);
            com.UserID = m.UserID;
            decimal yinkui = 0;
            if (!string.IsNullOrEmpty(m.Xiaochu))
            {
                List<string> listBS = m.Xiaochu.Split('|').ToList();   //宝石
                listBS.RemoveAt(0);     //消除第一个  因为第一个是空的  
                for (int i = 0; i < listBS.Count; i++)
                {
                    string[] arrBS = new string[3];
                    decimal result = 0;
                    decimal.TryParse(arrBS[2], out result);
                    yinkui = yinkui + (result - m.CountLine * m.Goal);
                    com.Initial = result;
                }
            }
            if (!string.IsNullOrEmpty(m.Longzhu) && m.Longzhu.Trim() != "0,0,0")
            {
                string[] arrLongzhu = new string[3];
                arrLongzhu = m.Longzhu.Split(',');
                decimal result = 0;
                decimal xiazhu = 0;
                decimal.TryParse(arrLongzhu[2], out result);
                decimal.TryParse(arrLongzhu[1], out xiazhu);
                yinkui = yinkui + result - xiazhu;
                com.Initial = result;
            }
            com.InitialCount = 1;
            com.Key = com.UserID + com.CountDate.ToString();
            com.Key2 = com.UserID + com.CountDate.ToString() + com.DownType;
            //  com.TotalTime = m.BoardTime;  // 牌局时常
            resdata.Add(com);
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
            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0),ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0) ,ifnull(b.Zodiac_Award_W,0) ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0) ,
ifnull(b.BaiJiaLe_Count, 0)  ,ifnull(b.BaiJiaLe_Banker, 0),ifnull(b.BaiJiaLe_Award_L, 0),ifnull(b.BaiJiaLe_Award_W, 0),
ifnull(b.Serial_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0) + " + comdata.InitialL + @" ,ifnull(b.Serial_Award_W,0) + " + comdata.InitialW + @",
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0) 
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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
            co.DownType = "21";
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


    private static void BaiJiaLeGameLog(DateTime endTime)
    {
        //查询开始时间
        //   ILog log = LogManager.GetLogger("TaskAction");
        //  log.Info("开始百家乐牌局分析#################");
        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_BaiJiaLe", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };
        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);
        //   log.Info("开始时间:" + grv.StartDate);
        //    log.Info("结束时间:" + grv.ExpirationDate);
        IEnumerable<BaccaratGameRecord> data = GameDataBLL.GetListForBaiJiaLe(grv);
        //   log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());
        List<CommonGameData> resdata = new List<CommonGameData>();
        foreach (BaccaratGameRecord m in data)
        {
            //0,88782000,0,(_37,0,0,(0:500,2:500,3:500,4:500,_10009,0,95000,(4:100000,_
            List<string> userList = m.UserData.Trim('_').Split('_').ToList();
            userList.Remove(userList[0]);
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
                decimal.TryParse(userData[2].Trim('('), out d);
                com.Initial = d;
                com.InitialCount = 1;
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


            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W

)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0),ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0) ,ifnull(b.Zodiac_Award_W,0) ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0) ,
ifnull(b.BaiJiaLe_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.BaiJiaLe_Banker,0),ifnull(b.BaiJiaLe_Award_L,0) + " + comdata.InitialL + @" ,ifnull(b.BaiJiaLe_Award_W,0) + " + comdata.InitialW + @",
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0)
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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
            co.DownType = "21";
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

            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W

)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0)+ " + comdata.InitialCount + @"  ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0)+ " + comdata.InitialL + @"  ,ifnull(b.Zodiac_Award_W,0) + " + comdata.InitialW + @" ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0) ,
ifnull(b.BaiJiaLe_Count, 0)  ,ifnull(b.BaiJiaLe_Banker, 0),ifnull(b.BaiJiaLe_Award_L, 0),ifnull(b.BaiJiaLe_Award_W, 0),
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0)
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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

            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0)  ,ifnull(b.Zodiac_Award_W,0)  ,
ifnull(b.Horse_Count,0) + " + comdata.InitialCount + @"  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0) + " + comdata.InitialL + @" ,ifnull(b.Horse_Award_W,0)+ " + comdata.InitialW + @"  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0),
ifnull(b.BaiJiaLe_Count, 0)  ,ifnull(b.BaiJiaLe_Banker, 0),ifnull(b.BaiJiaLe_Award_L, 0),ifnull(b.BaiJiaLe_Award_W, 0),
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0)
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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

            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0)  ,ifnull(b.Zodiac_Award_W,0)  ,
ifnull(b.Horse_Count,0)   ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0) ,
ifnull(b.Car_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0)+ " + comdata.InitialL + @" ,ifnull(b.Car_Award_W,0) + " + comdata.InitialW + @", 
ifnull(b.Hundred_Count,0)  ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0),ifnull(b.Hundred_Award_W,0),
ifnull(b.BaiJiaLe_Count, 0)  ,ifnull(b.BaiJiaLe_Banker, 0),ifnull(b.BaiJiaLe_Award_L, 0),ifnull(b.BaiJiaLe_Award_W, 0),
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0)
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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


                decimal d = Convert.ToDecimal(userData[2].Trim(')'));





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


            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W

)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0) ,ifnull(b.Zodiac_Award_W,0) ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Hundred_Banker,0),ifnull(b.Hundred_Award_L,0)+ " + comdata.InitialL + @" ,ifnull(b.Hundred_Award_W,0) + " + comdata.InitialW + @" ,
ifnull(b.BaiJiaLe_Count, 0)  ,ifnull(b.BaiJiaLe_Banker, 0),ifnull(b.BaiJiaLe_Award_L, 0),ifnull(b.BaiJiaLe_Award_W, 0),
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0)
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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

    private static void ShuihuGameLog(DateTime endTime)
    {
        //查询开始时间

        //    ILog log = LogManager.GetLogger("TaskAction");
        //    log.Info("开始水浒传牌局分析#################");

        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Shuihuzhuan", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);



        IEnumerable<ShuihuGameRecord> data = GameDataBLL.GetListForShuihu(grv);

        //  log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


        List<CommonGameData> resdata = new List<CommonGameData>();



        foreach (ShuihuGameRecord m in data)
        {
            CommonGameData com = new CommonGameData();
            string time = m.CreateTime.Replace("/", "-");
            string[] times = time.Split(new char[] { '-', ' ' });

            com.CountDate = new DateTime(
                Convert.ToInt32(times[0]),
                Convert.ToInt32(times[1]),
                Convert.ToInt32(times[2])
            );
            com.UserID = m.UserID;

            if (m.Pay != 0)
            {//赔付-押注
                com.Initial = m.Pay - m.Bet;
            }
            else
            {
                string bibeis = m.TimesDetail;
                if (string.IsNullOrEmpty(bibeis))//比倍是空值
                {
                    com.Initial = m.Pay - m.Bet;
                }
                else
                {
                    string[] timesDetails = m.TimesDetail.Trim('|').Split('|');
                    long before = Convert.ToInt64(timesDetails[0].Split(',')[0]);
                    long after = Convert.ToInt64(timesDetails[timesDetails.Length - 1].Split(',')[1]);
                    com.Initial = after - before - m.Bet;
                }
            }


            com.InitialCount = 1;
            com.Key = com.UserID + com.CountDate.ToString();
            com.Key2 = com.UserID + com.CountDate.ToString() + com.DownType;
            com.TotalTime = 0;
            resdata.Add(com);
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


            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0) ,ifnull(b.Zodiac_Award_W,0) ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count, 0)  ,ifnull(b.Hundred_Banker, 0),ifnull(b.Hundred_Award_L, 0),ifnull(b.Hundred_Award_W, 0),
ifnull(b.BaiJiaLe_Count, 0)  ,ifnull(b.BaiJiaLe_Banker, 0),ifnull(b.BaiJiaLe_Award_L, 0),ifnull(b.BaiJiaLe_Award_W, 0),
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Shuihu_Award_L,0)+ " + comdata.InitialL + @" ,ifnull(b.Shuihu_Award_W,0) + " + comdata.InitialW + @" ,
ifnull(b.Shuiguoji_Count, 0) ,ifnull(b.Shuiguoji_Award_L, 0),ifnull(b.Shuiguoji_Award_W, 0)
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
";
        }


        IEnumerable<IGrouping<string, CommonGameData>> query2 = resdata.GroupBy(m => m.Key2);
        List<CommonGameData> sumData2 = new List<CommonGameData>();
        foreach (IGrouping<string, CommonGameData> info in query2)
        {
            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合sl
           
            CommonGameData co = new CommonGameData();
            co.UserID = sl[0].UserID;
            co.CountDate = sl[0].CountDate;

            co.InitialCount = sl.Sum(m => m.InitialCount);
            co.DownType = "19";
            co.TotalTime = sl.Sum(m => m.TotalTime);
            sumData2.Add(co);
        }
        foreach (CommonGameData comdata in sumData2)
        {
            sql = sql + @"
replace into Clearing_GameDesc(UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime)
select a.UserID ,date('" + comdata.CountDate + @"') ,19 ," + comdata.DownType + @" ," + comdata.InitialCount + @" + ifnull(b.GameCount ,0) ," + comdata.TotalTime + @" + ifnull(b.GameTime ,0)
from (select " + comdata.UserID + @" UserID)a 
  left join (
    select UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime from Clearing_GameDesc 
    where UserID = " + comdata.UserID + @" and CountDate = date('" + comdata.CountDate + @"') and GameType = 19 and BetType = '" + comdata.DownType + @"'
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



    private static void ShuiguojiGameLog(DateTime endTime)
    {
        //查询开始时间

        ILog log = LogManager.GetLogger("TaskAction");
        log.Info("开始水果机牌局分析#################");

        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Shuiguo", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);
        log.Info("开始水果机牌局分析grv.StartDate=" + grv.StartDate);
        grv.SearchExt = "";

        IEnumerable<FruitGameRecord> data = GameDataBLL.GetListByPageForShuiguo(grv);
        grv.SearchExt = "Analyse_Shuiguo";
        log.Info("开始水果机牌局分析data.Count()=" + data.Count());
        //  log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


        List<CommonGameData> resdata = new List<CommonGameData>();



        foreach (FruitGameRecord m in data)
        {
            CommonGameData com = new CommonGameData();
            string time = m.CreateTime.Replace("/", "-");
            string[] times = time.Split(new char[] { '-', ' ' });

            com.CountDate = new DateTime(
                Convert.ToInt32(times[0]),
                Convert.ToInt32(times[1]),
                Convert.ToInt32(times[2])
            );
            com.UserID = m.UserID;
            com.Initial = m.WinGold;

            decimal wingold = m.WinGold;
            decimal betgold = m.BetGold;

            string bibeis = m.SbInfo;
            if (!string.IsNullOrEmpty(bibeis))
            {
                string[] sbinfos = bibeis.Trim('_').Split('_');
                decimal inDecimal = m.BetGold;
                decimal outDecimal = m.WinGold;
                for (int z = 0; z < sbinfos.Length; z++)
                {
                    string[] tempSbs = sbinfos[z].Split(',');
                    string yazhu = tempSbs[0];
                    string yazhuquyu = tempSbs[1];
                    int kaijiangjieguo = Convert.ToInt32(tempSbs[2]);
                    inDecimal += Convert.ToInt64(yazhu);

                    if (yazhuquyu == "1" && kaijiangjieguo <= 7)
                    {
                        outDecimal += Convert.ToInt64(yazhu) * 2;
                    }
                    else if (yazhuquyu == "2" && kaijiangjieguo > 7)
                    {
                        outDecimal += Convert.ToInt64(yazhu) * 2;
                    }
                    else
                    {
                        outDecimal += 0;
                    }

                }
                com.Initial = outDecimal - inDecimal;
            }
            else
            {//没有比倍
                com.Initial = m.WinGold - m.BetGold;
            }



            com.InitialCount = 1;
            com.Key = com.UserID + com.CountDate.ToString();
            com.Key2 = com.UserID + com.CountDate.ToString() + com.DownType;
            com.TotalTime = 0;
            resdata.Add(com);
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
            sql = sql + @"replace into " + database3 + @".Clearing_Game(
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
)
select " + comdata.UserID + @" ,'" + comdata.CountDate + @"' ,
ifnull(b.Texas_LCount,0),ifnull(b.Texas_LAward_L,0),ifnull(b.Texas_LAward_W,0),
ifnull(b.Texas_MCount,0),ifnull(b.Texas_MAward_L,0),ifnull(b.Texas_MAward_W,0),
ifnull(b.Texas_HCount,0),ifnull(b.Texas_HAward_L,0),ifnull(b.Texas_HAward_W,0),
ifnull(b.Scale_Count,0) ,ifnull(b.Scale_Banker,0),ifnull(b.Scale_Award_L,0) ,ifnull(b.Scale_Award_W,0) ,
ifnull(b.Zodiac_Count,0) ,ifnull(b.Zodiac_Banker,0),ifnull(b.Zodiac_Award_L,0) ,ifnull(b.Zodiac_Award_W,0) ,
ifnull(b.Horse_Count,0)  ,ifnull(b.Horse_Banker,0),ifnull(b.Horse_Award_L,0)  ,ifnull(b.Horse_Award_W,0)  ,
ifnull(b.Car_Count,0)  ,ifnull(b.Car_Banker,0),ifnull(b.Car_Award_L,0) ,ifnull(b.Car_Award_W,0) ,
ifnull(b.Hundred_Count, 0)  ,ifnull(b.Hundred_Banker, 0),ifnull(b.Hundred_Award_L, 0),ifnull(b.Hundred_Award_W, 0),
ifnull(b.BaiJiaLe_Count, 0)  ,ifnull(b.BaiJiaLe_Banker, 0),ifnull(b.BaiJiaLe_Award_L, 0),ifnull(b.BaiJiaLe_Award_W, 0),
ifnull(b.Serial_Count,0)  ,ifnull(b.Serial_Banker,0),ifnull(b.Serial_Award_L,0),ifnull(b.Serial_Award_W,0),
ifnull(b.Shuihu_Count, 0) ,ifnull(b.Shuihu_Award_L, 0),ifnull(b.Shuihu_Award_W, 0),
ifnull(b.Shuiguoji_Count,0) + " + comdata.InitialCount + @" ,ifnull(b.Shuiguoji_Award_L,0)+ " + comdata.InitialL + @" ,ifnull(b.Shuiguoji_Award_W,0) + " + comdata.InitialW + @" 
from (select " + comdata.UserID + @" userid ) a left join (
select 
UserID ,CountDate ,
 Texas_LCount, Texas_LAward_L ,Texas_LAward_W,
Texas_MCount,Texas_MAward_L ,Texas_MAward_W,
Texas_HCount,Texas_HAward_L,Texas_HAward_W,
Scale_Count,Scale_Banker,Scale_Award_L,Scale_Award_W,
Zodiac_Count,Zodiac_Banker,Zodiac_Award_L,Zodiac_Award_W,
Horse_Count,Horse_Banker,Horse_Award_L,Horse_Award_W,
Car_Count,Car_Banker,Car_Award_L,Car_Award_W,
Hundred_Count,Hundred_Banker,Hundred_Award_L,Hundred_Award_W,
BaiJiaLe_Count,BaiJiaLe_Banker,BaiJiaLe_Award_L,BaiJiaLe_Award_W,
Serial_Count,Serial_Banker,Serial_Award_L,Serial_Award_W,
Shuihu_Count,Shuihu_Award_L,Shuihu_Award_W,
Shuiguoji_Count,Shuiguoji_Award_L,Shuiguoji_Award_W
from " + database3 + @".Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
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
            co.DownType = "20";
            co.TotalTime = sl.Sum(m => m.TotalTime);
            sumData2.Add(co);
        }
        foreach (CommonGameData comdata in sumData2)
        {
            sql = sql + @"
replace into Clearing_GameDesc(UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime)
select a.UserID ,date('" + comdata.CountDate + @"') ,20 ," + comdata.DownType + @" ," + comdata.InitialCount + @" + ifnull(b.GameCount ,0) ," + comdata.TotalTime + @" + ifnull(b.GameTime ,0)
from (select " + comdata.UserID + @" UserID)a 
  left join (
    select UserID ,CountDate ,GameType ,BetType ,GameCount ,GameTime from Clearing_GameDesc 
    where UserID = " + comdata.UserID + @" and CountDate = date('" + comdata.CountDate + @"') and GameType = 20 and BetType = '" + comdata.DownType + @"'
  )b on a.UserID = b.UserID ;

";
        }

        if (sql != "")
        {
            log.Info("开始水果机牌局分析sql=" + sql);

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





