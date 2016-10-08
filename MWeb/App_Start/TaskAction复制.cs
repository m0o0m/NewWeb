//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Timers;
//using System.Web.Caching;
//using GL.Data;
//using StackExchange.Redis;

//using log4net;
//using Newtonsoft.Json;
//using System.IO;
//using GL.Data.BLL;
//using GL.Data.View;
//using Webdiyer.WebControls.Mvc;
//using GL.Data.Model;

///// <summary>
/////Action 的摘要说明
///// </summary>
//public static class TaskAction
//{

//    public static void TaskRegister()
//    {

//        /*  
//            http://localhost:25664/api/OnLinePlay/GetOnLineUser?pageSize=10&pageIndex=1
//        */
//        SetContent(null, null);
//        System.Timers.Timer myTimer = new System.Timers.Timer(1000 * 60 * 2);

//        myTimer.Elapsed += new System.Timers.ElapsedEventHandler(TaskAction.SetContent);
//        myTimer.Enabled = true;
//        myTimer.AutoReset = true;
//    }
//    /// <summary>
//    /// 定时器委托任务 调用的方法
//    /// </summary>
//    /// <param name="source"></param>
//    /// <param name="e"></param>
//    public static void SetContent(object source, ElapsedEventArgs e)
//    {
//        ILog log = LogManager.GetLogger("TaskAction");

//        DateTime endTime = DateTime.Now;
//        //德州扑克数据解析
//       TexasGameLog(endTime);
//        //中发白数据解析
//       ScaleGameLog(endTime);
//        //十二生肖数据解析
//       ZodiacGameLog(endTime);
//        //小马快跑数据解析
//        HorseGameLog(endTime);
//        //奔驰宝马数据解析
//        CarGameLog(endTime);
//    }
//    /// <summary>
//    /// 应用池回收的时候调用的方法
//    /// </summary>
//    public static void SetContent()
//    {
      
//    }

//    private static void TexasGameLog(DateTime endTime)
//    {
//        //查询开始时间

//        ILog log = LogManager.GetLogger("TaskAction");
//        log.Info("开始德州扑克牌局分析#################");

//        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Texas", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

//        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

//        log.Info("开始时间:"+grv.StartDate);
//        log.Info("结束时间:" + grv.ExpirationDate);

//        IEnumerable<TexasGameRecord> data = GameDataBLL.GetListForTexas(grv);
      
//        log.Info("数据查询结果集数量:" + data==null?0:data.Count());


//        List<CommonGameData> resdata = new List<CommonGameData>();



//        foreach (TexasGameRecord m in data)
//        {
          
//            var userList = m.UserData.Split('_').ToList();
//            userList.RemoveAt(userList.Count - 1);
//            var j = userList.Count;
//            for (int i = 0; i < j; i++)
//            {
//                CommonGameData com = new CommonGameData();
//                var userData = userList[i].Split(',').ToList();
//                int tem = 0;
               
//                    DateTime t = m.CreateTime;
//                    com.CountDate = new DateTime(t.Year, t.Month, t.Day);
//                    //int 房间ID = m.RoomID;
//                    //decimal 牌局号 = m.Round;
//                    string 盲注 = m.BaseScore;

//                    string[] s = m.BaseScore.Split('/');
//                    int v = int.Parse(s[0]);
//                    tem = v;
//                    //int 服务费 = m.Service;
                
             
//                var wj = userData[2];
//                com.UserID = int.Parse( wj);

//                decimal d = Convert.ToDecimal(userData[4]);
//                if (tem <= 100) { com.Initial = d; com.InitialCount = 1; }
//                else if (tem >= 5000) { com.HighRank = d;com.HighRankCount = 1; }
//                else { com.Secondary = d; com.SecondaryCount = 1; }

//                com.Key = com.UserID + com.CountDate.ToString();
//                resdata.Add(com);
//            }
//        }
//        IEnumerable<IGrouping<string, CommonGameData>> query  = resdata.GroupBy(m => m.Key);
//        List<CommonGameData> sumData = new List<CommonGameData>();
//        foreach (IGrouping<string, CommonGameData> info in query)
//        {
//            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
//            CommonGameData co = new CommonGameData();
//            co.UserID = sl[0].UserID;
//            co.CountDate = sl[0].CountDate;
//            co.InitialL = sl.Where(m=>m.Initial<0).Sum(m => m.Initial);
//            co.InitialW= sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
//            co.InitialCount = sl.Sum(m => m.InitialCount);
//            co.HighRankL = sl.Where(m => m.HighRank < 0).Sum(m => m.HighRank);
//            co.HighRankW = sl.Where(m => m.HighRank > 0).Sum(m => m.HighRank);
//            co.HighRankCount = sl.Sum(m => m.HighRankCount);
//            co.SecondaryL = sl.Where(m => m.Secondary < 0).Sum(m => m.Secondary);
//            co.SecondaryW = sl.Where(m => m.Secondary > 0).Sum(m => m.Secondary);
//            co.SecondaryCount = sl.Sum(m => m.SecondaryCount);
//            sumData.Add(co);
//        }

//        foreach (CommonGameData comdata in sumData)
//        {
//            string sql = @"
//replace into Clearing_Game(UserID ,CountDate ,Texas_LCount ,Texas_LAward_L ,Texas_LAward_W ,Texas_MCount ,Texas_MAward_L ,Texas_MAward_W ,Texas_HCount ,Texas_HAward_L ,Texas_HAward_W)
//select "+ comdata.UserID + " ,'"+ comdata.CountDate + "' ,ifnull(b.Texas_LCount,0) + "+comdata.InitialCount+" ,ifnull(b.Texas_LAward_L,0) + "+comdata.InitialL +" ,ifnull(b.Texas_LAward_W,0) + "+comdata.InitialW+@" 
//,ifnull(b.Texas_MCount,0) + "+ comdata.SecondaryCount + @" ,ifnull(b.Texas_MAward_L,0) + "+comdata.SecondaryL+@" ,ifnull(b.Texas_MAward_W,0) + "+comdata.SecondaryW+@" 
//,ifnull(b.Texas_HCount,0) + "+ comdata.HighRankCount + @" ,ifnull(b.Texas_HAward_L,0) + "+ comdata.HighRankL + @" ,ifnull(b.Texas_HAward_W,0) + "+comdata.HighRankW+@"
//from (select "+comdata.UserID+@" userid ) a left join (
//select * from Clearing_Game where UserID = "+comdata.UserID+@" and CountDate = '"+comdata.CountDate+@"') b on 1 = 1;
//";


//            GameDataBLL.Add(sql);
//        }

//        GameDataBLL.UpdateBeginTimeForGame(grv);
//        //修改时间



//    }


//    private static void ScaleGameLog(DateTime endTime) {
//        //查询开始时间

//        ILog log = LogManager.GetLogger("TaskAction");
//        log.Info("开始中发白牌局分析#################");

//        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Scale", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

//        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

//        log.Info("开始时间:" + grv.StartDate);
//        log.Info("结束时间:" + grv.ExpirationDate);

//        IEnumerable<ScaleGameRecord> data = GameDataBLL.GetListForScale(grv);

//        log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


//        List<CommonGameData> resdata = new List<CommonGameData>();



//        foreach (ScaleGameRecord m in data)
//        {

//            var userList = m.UserData.Split('_').ToList();
//            userList.RemoveAt(userList.Count - 1);
//            var j = userList.Count;
//            for (int i = 0; i < j; i++)
//            {
//                CommonGameData com = new CommonGameData();
//                var userData = userList[i].Split(',').ToList();
//                int tem = 0;

//                DateTime t = m.CreateTime;
//                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
//                //int 房间ID = m.RoomID;
//                //decimal 牌局号 = m.Round;
         
//                //int 服务费 = m.Service;


//                var wj = userData[0];
//                com.UserID = int.Parse(wj);

//                decimal d = Convert.ToDecimal(userData[5].Trim(')'));
//                com.Initial = d; com.InitialCount = 1;
//                com.Key = com.UserID + com.CountDate.ToString();
//                resdata.Add(com);
//            }
//        }
//        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
//        List<CommonGameData> sumData = new List<CommonGameData>();
//        foreach (IGrouping<string, CommonGameData> info in query)
//        {
//            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
//            CommonGameData co = new CommonGameData();
//            co.UserID = sl[0].UserID;
//            co.CountDate = sl[0].CountDate;
//            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
//            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
//            co.InitialCount = sl.Sum(m => m.InitialCount);
          
//            sumData.Add(co);
//        }

//        foreach (CommonGameData comdata in sumData)
//        {
//            string sql = @"
//replace into Clearing_Game(UserID ,CountDate ,Scale_Count ,Scale_Award_L ,Scale_Award_W )
//select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Scale_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Scale_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Scale_Award_W,0) + " + comdata.InitialW + @" 
//from (select " + comdata.UserID + @" userid ) a left join (
//select * from Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
//";


//            GameDataBLL.Add(sql);
//        }

//        GameDataBLL.UpdateBeginTimeForGame(grv);
//        //修改时间


//    }

//    private static void ZodiacGameLog(DateTime endTime) {
//        //查询开始时间

//        ILog log = LogManager.GetLogger("TaskAction");
//        log.Info("开始十二生肖牌局分析#################");

//        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Zodiac", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

//        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

//        log.Info("开始时间:" + grv.StartDate);
//        log.Info("结束时间:" + grv.ExpirationDate);

//        IEnumerable<ZodiacGameRecord> data = GameDataBLL.GetListForZodiac(grv);

//        log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


//        List<CommonGameData> resdata = new List<CommonGameData>();



//        foreach (ZodiacGameRecord m in data)
//        {

//            string tempStr = m.UserData.Replace("_0,0,0,(", "");

//            var tempStr2 = tempStr.Split(new string[] { ",(_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
//            string wjxy = "";
//            string zj = "";
//            int k = 0;
//            if (tempStr2.Count == 2)
//            {//如果是庄家
//                zj = tempStr2[0];
//                k = -1;
//                wjxy = tempStr2[1];
//                tempStr = tempStr2[0] + "_" + tempStr2[1];
//            }
//            else
//            {
//                wjxy = tempStr2[0];
//                k = 0;
//            }
//            var userList = tempStr.Split('_').ToList();
//            userList.RemoveAt(userList.Count - 1);
//            var j = userList.Count;



//            for (int i = 0; i < j; i++)
//            {
//                CommonGameData com = new CommonGameData();
//                var userData1 = userList[i].Split(new string[] { ",(" }, StringSplitOptions.RemoveEmptyEntries).ToList();
//                var userData = userList[i].Split(',').ToList();


//                DateTime t = m.CreateTime;
//                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
//                //int 房间ID = m.RoomID;
//                //decimal 牌局号 = m.Round;

//                //int 服务费 = m.Service;


//                var wj = userData[0];
//                com.UserID = int.Parse(wj);

//                decimal d = Convert.ToDecimal(userData[2]);
//                com.Initial = d; com.InitialCount = 1;
//                com.Key = com.UserID + com.CountDate.ToString();
//                resdata.Add(com);
//            }
//        }
//        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
//        List<CommonGameData> sumData = new List<CommonGameData>();
//        foreach (IGrouping<string, CommonGameData> info in query)
//        {
//            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
//            CommonGameData co = new CommonGameData();
//            co.UserID = sl[0].UserID;
//            co.CountDate = sl[0].CountDate;
//            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
//            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
//            co.InitialCount = sl.Sum(m => m.InitialCount);

//            sumData.Add(co);
//        }

//        foreach (CommonGameData comdata in sumData)
//        {
//            string sql = @"
//replace into Clearing_Game(UserID ,CountDate ,Zodiac_Count ,Zodiac_Award_L ,Zodiac_Award_W )
//select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Zodiac_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Zodiac_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Zodiac_Award_W,0) + " + comdata.InitialW + @" 
//from (select " + comdata.UserID + @" userid ) a left join (
//select * from Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
//";


//            GameDataBLL.Add(sql);
//        }

//        GameDataBLL.UpdateBeginTimeForGame(grv);
//        //修改时间



//    }

//    private static void HorseGameLog(DateTime endTime) {
//        //查询开始时间

//        ILog log = LogManager.GetLogger("TaskAction");
//        log.Info("开始小马快跑牌局分析#################");

//        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Horse", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

//        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

//        log.Info("开始时间:" + grv.StartDate);
//        log.Info("结束时间:" + grv.ExpirationDate);

//        IEnumerable<HorseGameRecord> data = GameDataBLL.GetListForHorse(grv);

//        log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


//        List<CommonGameData> resdata = new List<CommonGameData>();



//        foreach (HorseGameRecord m in data)
//        {

//            string tempStr = m.UserData.Replace("_0,0,0,(", "");

//            var tempStr2 = tempStr.Split(new string[] { ",(_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
//            string wjxy = "";
//            string zj = "";
//            int k = 0;
//            if (tempStr2.Count == 2)
//            {//如果是庄家
//                zj = tempStr2[0];
//                k = -1;
//                wjxy = tempStr2[1];
//                tempStr = tempStr2[0] + "_" + tempStr2[1];
//            }
//            else
//            {
//                wjxy = tempStr2[0];
//                k = 0;
//            }
//            var userList = tempStr.Split('_').ToList();
//            userList.RemoveAt(userList.Count - 1);
//            var j = userList.Count;



//            for (int i = 0; i < j; i++)
//            {
//                CommonGameData com = new CommonGameData();
//                var userData1 = userList[i].Split(new string[] { ",(" }, StringSplitOptions.RemoveEmptyEntries).ToList();
//                var userData = userList[i].Split(',').ToList();


//                DateTime t = m.CreateTime;
//                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
//                //int 房间ID = m.RoomID;
//                //decimal 牌局号 = m.Round;

//                //int 服务费 = m.Service;


//                var wj = userData[0];
//                com.UserID = int.Parse(wj);

//                decimal d = Convert.ToDecimal(userData[2]);
//                com.Initial = d; com.InitialCount = 1;
//                com.Key = com.UserID + com.CountDate.ToString();
//                resdata.Add(com);
//            }
//        }
//        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
//        List<CommonGameData> sumData = new List<CommonGameData>();
//        foreach (IGrouping<string, CommonGameData> info in query)
//        {
//            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
//            CommonGameData co = new CommonGameData();
//            co.UserID = sl[0].UserID;
//            co.CountDate = sl[0].CountDate;
//            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
//            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
//            co.InitialCount = sl.Sum(m => m.InitialCount);
          
//            sumData.Add(co);
//        }
//        foreach (CommonGameData comdata in sumData)
//        {
//            string sql = @"
//replace into Clearing_Game(UserID ,CountDate ,Horse_Count ,Horse_Award_L ,Horse_Award_W )
//select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Horse_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Horse_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Horse_Award_W,0) + " + comdata.InitialW + @" 
//from (select " + comdata.UserID + @" userid ) a left join (
//select * from Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
//";


//            GameDataBLL.Add(sql);
//        }

//        GameDataBLL.UpdateBeginTimeForGame(grv);
//        //修改时间

//    }

//    private static void CarGameLog(DateTime endTime) {

//        //查询开始时间

//        ILog log = LogManager.GetLogger("TaskAction");
//        log.Info("开始奔驰宝马牌局分析#################");

//        GameRecordView grv = new GameRecordView { Gametype = 1, Data = 0, UserID = 0, SearchExt = "Analyse_Car", StartDate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = endTime.ToString(), Page = 1, SeachType = (seachType)0 };

//        grv.StartDate = GameDataBLL.GetBeginTimeForGame(grv);

//        log.Info("开始时间:" + grv.StartDate);
//        log.Info("结束时间:" + grv.ExpirationDate);

//        IEnumerable<CarGameRecord> data = GameDataBLL.GetListForCar(grv);

//        log.Info("数据查询结果集数量:" + data == null ? 0 : data.Count());


//        List<CommonGameData> resdata = new List<CommonGameData>();



//        foreach (CarGameRecord m in data)
//        {

//            string tempStr = m.UserData.Replace("_0,0,0,(", "");

//            var tempStr2 = tempStr.Split(new string[] { ",(_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
//            string wjxy = "";
//            string zj = "";
//            int k = 0;
//            if (tempStr2.Count == 2)
//            {//如果是庄家
//                zj = tempStr2[0];
//                k = -1;
//                wjxy = tempStr2[1];
//                tempStr = tempStr2[0] + "_" + tempStr2[1];
//            }
//            else
//            {
//                wjxy = tempStr2[0];
//                k = 0;
//            }
//            var userList = tempStr.Split('_').ToList();
//            userList.RemoveAt(userList.Count - 1);
//            var j = userList.Count;



//            for (int i = 0; i < j; i++)
//            {
//                CommonGameData com = new CommonGameData();
//                var userData1 = userList[i].Split(new string[] { ",(" }, StringSplitOptions.RemoveEmptyEntries).ToList();
//                var userData = userList[i].Split(',').ToList();


//                DateTime t = m.CreateTime;
//                com.CountDate = new DateTime(t.Year, t.Month, t.Day);
//                //int 房间ID = m.RoomID;
//                //decimal 牌局号 = m.Round;

//                //int 服务费 = m.Service;


//                var wj = userData[0];
//                com.UserID = int.Parse(wj);

//                decimal d = Convert.ToDecimal(userData[2]);
//                com.Initial = d; com.InitialCount = 1;
//                com.Key = com.UserID + com.CountDate.ToString();
//                resdata.Add(com);
//            }
//        }
//        IEnumerable<IGrouping<string, CommonGameData>> query = resdata.GroupBy(m => m.Key);
//        List<CommonGameData> sumData = new List<CommonGameData>();
//        foreach (IGrouping<string, CommonGameData> info in query)
//        {
//            List<CommonGameData> sl = info.ToList<CommonGameData>();//分组后的集合
//            CommonGameData co = new CommonGameData();
//            co.UserID = sl[0].UserID;
//            co.CountDate = sl[0].CountDate;
//            co.InitialL = sl.Where(m => m.Initial < 0).Sum(m => m.Initial);
//            co.InitialW = sl.Where(m => m.Initial > 0).Sum(m => m.Initial);
//            co.InitialCount = sl.Sum(m => m.InitialCount);

//            sumData.Add(co);
//        }
//        foreach (CommonGameData comdata in sumData)
//        {
//            string sql = @"
//replace into Clearing_Game(UserID ,CountDate ,Car_Count ,Car_Award_L ,Car_Award_W )
//select " + comdata.UserID + " ,'" + comdata.CountDate + "' ,ifnull(b.Car_Count,0) + " + comdata.InitialCount + " ,ifnull(b.Car_Award_L,0) + " + comdata.InitialL + " ,ifnull(b.Car_Award_W,0) + " + comdata.InitialW + @" 
//from (select " + comdata.UserID + @" userid ) a left join (
//select * from Clearing_Game where UserID = " + comdata.UserID + @" and CountDate = '" + comdata.CountDate + @"') b on 1 = 1;
//";


//            GameDataBLL.Add(sql);
//        }

//        GameDataBLL.UpdateBeginTimeForGame(grv);
//        //修改时间


//    }

//}







//public class CommonGameData {
//    public string Key { get; set; }
//    public int UserID { get; set; }

//    public DateTime CountDate { get; set; }

//    public decimal Initial { get; set; }

//    public int InitialCount { get; set; }
//    public decimal InitialL { get; set; }
//    public decimal InitialW { get; set; }

//    public decimal Secondary { get; set; }
//    public decimal SecondaryL { get; set; }
//    public decimal SecondaryW { get; set; }
//    public int SecondaryCount { get; set; }

//    public decimal HighRank { get; set; }
//    public decimal HighRankL { get; set; }
//    public decimal HighRankW { get; set; }
//    public int HighRankCount { get; set; }
//}





