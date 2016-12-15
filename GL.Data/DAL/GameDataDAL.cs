using GL.Command.DBUtility;
using GL.Dapper;
using GL.Data.Model;
using GL.Data.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.DAL
{
    public class GameDataDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static int Add(string  sql)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(sql);
                cn.Close();
                return i;
            }
        }



        internal static IEnumerable<TexasGameRecord> GetListForTexas(GameRecordView vbd)
        {
          
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from BG_TexasGameRecord where CreateTime between '"+vbd.StartDate+"' and '"+vbd.ExpirationDate+ "' and CreateTime!='"+ vbd.ExpirationDate + "' order by CreateTime desc");

                IEnumerable<TexasGameRecord> i = cn.Query<TexasGameRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }


        //     public static IEnumerable<ShuihuGameRecord> GetListForShuihu(GameRecordView vbd) {

        internal static IEnumerable<ShuihuGameRecord> GetListForShuihu(GameRecordView vbd)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append(@"
                    select * from BG_ShuiHuRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc");

                IEnumerable<ShuihuGameRecord> i = cn.Query<ShuihuGameRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }

        public static IEnumerable<ThanksRank> GetThanksRankList(GameRecordView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append(@"
select UserID,r.NickName,
sum(
case a.ModeluID 
when 4 then 150000  *ClickCount  
 when 5 then 360000  *ClickCount  
 when 6 then 750000  *ClickCount  
 when 7 then 1980000 *ClickCount  
else 0 
end 
) AS TotalMoney
from "+ database3+ @".BG_ClickRecord as a,"+ database1 + @".Role as r
where ModeluID in (4,5,6,7) and a.UserID = r.ID 
and a.CreateTime>='" + model.StartDate+@"' and a.CreateTime<'"+model.ExpirationDate+@"'
GROUP BY UserID
ORDER BY TotalMoney desc
limit 100
;


");

                IEnumerable< ThanksRank > i = cn.Query<ThanksRank>(str.ToString());
                cn.Close();
                return i;
            }
        }





        public static IEnumerable<ThanksRank> GetMFestivalRankList(GameRecordView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append(@"
select UserID,r.NickName,
sum(
case a.ModeluID 
when 26 then 204000  *ClickCount  
 when 27 then 512000  *ClickCount  
 when 28  then 1312000  *ClickCount  
 when 29  then 3240000 *ClickCount  
else 0 
end 
) AS TotalMoney
from "+ database3+ @".BG_ClickRecord as a,"+ database1+ @".Role as r
where ModeluID in (26,27,28,29) and a.UserID = r.ID 
and a.CreateTime>='" + model.StartDate + @"' and a.CreateTime<'" + model.ExpirationDate + @"'
GROUP BY UserID
ORDER BY TotalMoney desc
limit 100
;


");

                IEnumerable<ThanksRank> i = cn.Query<ThanksRank>(str.ToString());
                cn.Close();
                return i;
            }
        }


        internal static TexasGameRecord GetTexasModelForRound(decimal round)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from BG_TexasGameRecord where Round ="+round);

                IEnumerable<TexasGameRecord> i = cn.Query<TexasGameRecord>(str.ToString());
                cn.Close();
                return i.FirstOrDefault();
            }
        }



        internal static IEnumerable<ScaleGameRecord> GetListForScale(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from BG_ScaleGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");

                IEnumerable<ScaleGameRecord> i = cn.Query<ScaleGameRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<BaccaratGameRecord> GetListForBaiJiaLe(GameRecordView vbd)
        {
           
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from BG_BaccaratGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");

                IEnumerable<BaccaratGameRecord> i = cn.Query<BaccaratGameRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }


        //
        internal static IEnumerable<ScaleGameRecord> GetListForTexPro(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from "+ database3+ @".BG_TexProGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");

                IEnumerable<ScaleGameRecord> i = cn.Query<ScaleGameRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<ZodiacGameRecord> GetListForZodiac(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from BG_ZodiacGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");




                IEnumerable<ZodiacGameRecord> i = cn.Query<ZodiacGameRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<HorseGameRecord> GetListForHorse(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from BG_HorseGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");
               
                IEnumerable<HorseGameRecord> i = cn.Query<HorseGameRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<CarGameRecord> GetListForCar(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("select * from BG_CarGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");

                IEnumerable<CarGameRecord> i = cn.Query<CarGameRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }




        internal static DateTime GetDataBaseTime()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("select now() as CurTime");

                IEnumerable<Time> i = cn.Query<Time>(str.ToString());
                cn.Close();
                return i.FirstOrDefault().CurTime;
            }
        }


        internal static string GetBeginTimeForGame(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from S_Update where UpdateTable = '"+ vbd.SearchExt + "'");

                IEnumerable<SUpdate> i = cn.Query<SUpdate>(str.ToString());
                cn.Close();
                SUpdate mode = i.FirstOrDefault();
                if (mode == null) {
                    return vbd.ExpirationDate;
                }
                return mode.id_date.ToString();
            }
        }

     

        internal static int UpdateBeginTimeForGame(GameRecordView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update S_Update set id_date='"+vbd.ExpirationDate+@"'
where UpdateTable = '"+vbd.SearchExt+"'");
                cn.Close();
                return i;
            }
        }


        internal static int GetSwitchIsOpen(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select ISOpen as Obj  from "+ database1 + @".S_Switch where ID = " + id + "");

                IEnumerable<SingleData> i = cn.Query<SingleData>(str.ToString());
                cn.Close();
                SingleData mode = i.FirstOrDefault();
                if (mode == null)
                {
                    return -1;
                }
                return Convert.ToInt32( mode.Obj);
            }
        }


        internal static SSwitch GetSwitchModel(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select *  from "+ database1 + @".S_Switch where ID = " + id + "");

                IEnumerable<SSwitch> i = cn.Query<SSwitch>(str.ToString());
                SSwitch mode = i.FirstOrDefault();
                cn.Close();

                return mode;
            }
        }

        


        internal static int UpdateSwitchIsOpen(int id,int isOpen)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update "+ database1 + @".S_Switch set ISOpen=" + isOpen + @" 
where ID = " + id + "");
                cn.Close();
                return i;
            }
        }

        internal static int DeletePopUpData(PopUpInfo model) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
delete from "+ database1 + @".PopUpControl where id = "+model.id+@"
");
                cn.Close();
                return i;
            }
        }

        internal static int AddPopUpData(PopUpInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
insert into "+ database1 + @".PopUpControl(Position,Platform,JumpPage,OpenWinNo,StartTime,EndTime,IsOpen)
values("+(int)model.Position + @",'"+model.Platform + @"',"+(int)model.JumpPage+@","+model.OpenWinNo+@",'"+model.StartTime+@"','"+model.EndTime+@"',"+model.IsOpen+@")
");
                cn.Close();
                return i;
            }
        }


        internal static int UpdatePopUpData(PopUpInfo model,int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
update "+ database1 + @".PopUpControl set 
Position=" + (int)model.Position + @",Platform='" + model.Platform + @"',JumpPage=" + (int)model.JumpPage + @",OpenWinNo=" + model.OpenWinNo + @",StartTime='" + model.StartTime + @"',EndTime='" + model.EndTime + @"',IsOpen=" + model.IsOpen + @"
where id = "+id+@"
");
                cn.Close();
                return i;
            }
        }
        public static IEnumerable<PopUpInfo> GetPopUpDataList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@"
select * from  "+ database1 + @".PopUpControl 
");

                IEnumerable<PopUpInfo> i = cn.Query<PopUpInfo>(str.ToString());
                cn.Close();
                return i;
            }
        }


        public static PopUpInfo GetPopUpData(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@"
select * from  "+ database1 + @".PopUpControl where id = "+id+@"
");

                IEnumerable<PopUpInfo> i = cn.Query<PopUpInfo>(str.ToString());
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        //GetPopUpDataByPosition

        public static PopUpInfo GetPopUpDataByPosition(int position)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@"
select * from  "+ database1 + @".PopUpControl where Position = " + position + @"
");

                IEnumerable<PopUpInfo> i = cn.Query<PopUpInfo>(str.ToString());
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        public static IEnumerable<BoardDetail_Left> GetBoardDetail_LeftList(GameRecordView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();



                IEnumerable<BoardDetail_Left> i = cn.Query<BoardDetail_Left>(@"sys_get_game_total1", param: new { v_date1 = model.StartDate, v_date2 = model.ExpirationDate }, commandType: CommandType.StoredProcedure);


             
                cn.Close();
                return i;
            }
        }

        public static IEnumerable<BoardDetail_Right> GetBoardDetail_RightList(GameRecordView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@"
select a.CreateTime ,"+model.Gametype+@" as GameID ,a.BoardID RoomCategory ,ifnull(sum(BoardNum),0) BoardNum ,ifnull(sum(BoardCount),0) BoardCount 
  ,ifnull(sum(BoardTime),0) BoardTime ,ifnull(sum(BetVal),0) BetVal ,ifnull(sum(CallBack),0) CallBack
from (
  select date_add('"+model.StartDate+@"' ,interval a.id day) CreateTime ,BoardID from "+ database3+ @".S_Ordinal a 
    join (select distinct BoardID from "+ database3 + @".S_Board where BoardID in ("+ model.SearchExt + @")) t on 1 = 1 
  where a.id < datediff('"+model.ExpirationDate+@"' ,'"+model.StartDate+@"')
)a  left join (
select a.CountDate AS CreateTime ,GameType AS GameID ,BoardID AS RoomCategory ,sum(BoardVal) AS BoardNum ,sum(CountVal) AS BoardCount ,
  sum(TimeVal) AS BoardTime ,sum(BetVal) AS BetVal ,sum(RakeVal) AS CallBack 
from "+ database3 + @".Clearing_GameTotal a join "+ database3 + @".S_Board b on b.ChildBoard = a.BetType and b.BoardID in ("+model.SearchExt+@")
where a.CountDate >='"+model.StartDate+@"' and a.CountDate < '"+model.ExpirationDate+@"' and a.GameType = "+model.Gametype+@" and a.UserType = "+model.UserType+@" and a.agent = a.agent 
group by a.CountDate,GameType,BoardID
)b on b.CreateTime = a.CreateTime  and a.BoardID = RoomCategory  group by a.CreateTime,a.BoardID ; 

");

                IEnumerable<BoardDetail_Right> i = cn.Query<BoardDetail_Right>(str.ToString());
                cn.Close();
                return i;
            }
        }


    }
}
