using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class BaseDataInfo
    {
        public DateTime date { get; set; }
        public int count { get; set; }

        public string Hour { get; set; }

        public string ChannelName { get; set; }

        public string ChannelID { get; set; }
        public string UserIDAdd { get; set; }
        public string UserIDDel { get; set; }
        public string ProfitAdd { get; set; }
        public string ProfitDel { get; set; }
        public string NicknameAdd { get; set; }
        public string NicknameDel { get; set; }
        public int list { get; set; }
        public long ProfitAdd1 { get; set; }
        public long ProfitDel1 { get; set; }
        public long ProfitAdd2 { get; set; }
        public long ProfitDel2 { get; set; }
        public long ProfitAdd3 { get; set; }
        public long ProfitDel3 { get; set; }
        public long ProfitAdd4 { get; set; }
        public long ProfitDel4 { get; set; }
        public long ProfitAdd5 { get; set; }
        public long ProfitDel5 { get; set; }
        public long ProfitAdd6 { get; set; }
        public long ProfitDel6 { get; set; }
        public long ProfitAdd7 { get; set; }
        public long ProfitDel7 { get; set; }


        public long FishType0 { get; set; }
        public long FishType1 { get; set; }
        public long FishType2 { get; set; }
        public long FishType3 { get; set; }
        public long FishType4 { get; set; }
        public long ChipChange { get; set; }
        public long ProfitScale { get; set; }
        public long ProfitHorse { get; set; }
        public long ProfitZodiac { get; set; }
        public long ProfitCar { get; set; }
        public long ProfitHundred { get; set; }

        public long ProfitBaiJiaLe { get; set; }
    }


    public class BaseDataInfoForOnlinePlay
    {
        public DateTime date { get; set; }
        public int online { get; set; }
        public int playing { get; set; }
    }

    public class BaseDataInfoForBankruptcyRate
    {
        public DateTime date { get; set; }
        public int count { get; set; }
        public int activeuser { get; set; }
    }

    public class BaseDataInfoForUsersGoldDistributionRatio
    {
        public int Channels { get; set; }
        public int UserType { get; set; }
        public int a0 { get; set; }
        public int a1 { get; set; }
        public int a2 { get; set; }
        public int a3 { get; set; }
        public int a4 { get; set; }
        public int a5 { get; set; }
        public int a6 { get; set; }
        public int a7 { get; set; }
        public int a8 { get; set; }
        public int a9 { get; set; }
        public int a10 { get; set; }
        public int a11 { get; set; }
        public int a12 { get; set; }
        public int a13 { get; set; }
        public int a14 { get; set; }
        public int a15 { get; set; }
    }

    public class BaseDataInfoForUsersDiamondDistributionRatio
    {
        public int Channels { get; set; }
        public int UserType { set; get; }
        public int a0 { get; set; }
        public int a1 { get; set; }
        public int a2 { get; set; }
        public int a3 { get; set; }
        public int a4 { get; set; }
        public int a5 { get; set; }
        public int a6 { get; set; }
        public int a7 { get; set; }
        public int a8 { get; set; }
        public int a9 { get; set; }
        public int a10 { get; set; }
        public int a11 { get; set; }
        public int a12 { get; set; }
        public int a13 { get; set; }
        public int a14 { get; set; }
        public int a15 { get; set; }
    }

    public class BaseDataInfoForVIPDistributionRatio
    {
        public int Channels { get; set; }
        public int UserType { get; set; }
        public int a0 { get; set; }
        public int a1 { get; set; }
        public int a2 { get; set; }
        public int a3 { get; set; }
        public int a4 { get; set; }
        public int a5 { get; set; }
        public int a6 { get; set; }
        public int a7 { get; set; }
        public int a8 { get; set; }
        public int a9 { get; set; }
        public int a10 { get; set; }
        public int a11 { get; set; }
        public int a12 { get; set; }
        public int a13 { get; set; }
        public int a14 { get; set; }
        public int a15 { get; set; }
    }
    public class BaseDataInfoForLevelDistributionRatio
    {
        public int a0 { get; set; }
        public int a1 { get; set; }
        public int a2 { get; set; }
        public int a3 { get; set; }
        public int a4 { get; set; }
        public int a5 { get; set; }
        public int a6 { get; set; }
        public int a7 { get; set; }
        public int a8 { get; set; }
        public int a9 { get; set; }
        public int a10 { get; set; }
        public int a11 { get; set; }
        public int a12 { get; set; }
        public int a13 { get; set; }
        public int a14 { get; set; }
        public int a15 { get; set; }
        public int a16 { get; set; }
        public int a17 { get; set; }
        public int a18 { get; set; }
    }


    public class BaseDataInfoForRetentionRates
    {
        public DateTime date { get; set; }
        public int newuser { get; set; }
        public decimal oneday { get; set; }
        public decimal twoday { get; set; }
        public decimal threeday { get; set; }
        public decimal fiveday { get; set; }
        public decimal sevenday { get; set; }
        public decimal tenday { get; set; }
        public decimal fifteenday { get; set; }
        public decimal thirtyday { get; set; }
    }


    public class BaseDataView
    {
        public long UserID { get; set; }
        public int Page { get; set; }
        public int Channels { get; set; }
        public string UserList { get; set; }
        public string StartDate { get; set; }
        public string ExpirationDate { get; set; }
        public groupby Groupby { get; set; }
        public object BaseDataList { get; set; }
        public int TypeID { get; set; }
        public int? RaType { get; set; }
        /// <summary>
        /// 查询扩展字段，用于非单个字段查询
        /// </summary>
        public string SearchExt { get; set; }

        public string SearchExt2 { get; set; }

    }


    public class BasePopOutDataView {
        public int Position { get; set; } 

        public string Platform { get; set; }

        public int JumpPage { get; set; }

        public int OpenWinNo { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int IsOpen { get; set; }

        public object DataList { get; set; }
    }


    public class BaseDataInfoForPotRakeback
    {
        public DateTime date { get; set; }
        public decimal Chip { get; set; }
    }


    public enum groupby
    {
        按日 = 1,
        按月 = 2
    }

    public enum terminals
    {
        所有终端 = 0,
        页游 = 1,
        安卓 = 2,
        IOS = 3,
    }

    public class Roulette
    {
        public DateTime Date { get; set; }
        public int CountSum { get; set; }       //抽奖总次数
        public int CountKey { get; set; }       //金钥匙抽奖总次数
        public long GoldConsume { get; set; }   //抽奖总消耗游戏币
        public long GoldIncome { get; set; }    //抽奖总产出游戏币
        public int DHQIncome { get; set; }      //抽奖总产出兑换劵
        public int DiamondIncome { get; set; }  //抽奖总产出五币
        public int ID { get; set; }             //奖励编号
        public string IDDesc { get; set; }         //奖励描述
        public int UserID { get; set; }
        public string ItemName { get; set; }
        public int ItemNum { get; set; }
        public int ItemValue { get; set; }
        public isGet IsGet { get; set; }
        public string TrueName { get; set; }
        public string Tel { get; set; }
        public int Post { get; set; }
        public string Address { get; set; }
        public long QQNum { get; set; }
    }

    public enum isGet
    {
        是 = 1 ,
        否 = 0
    }

    public class GameKeep
    {
        public DateTime CountDate { get; set; }  //日期
        public int RG { get; set; }     //新增
        public int GC0 { get; set; }    //玩牌0局
        public int GC1 { get; set; }
        public int GC2 { get; set; }
        public int GC3 { get; set; }
        public int GC4 { get; set; }
        public int GC5 { get; set; }
        public int GC6 { get; set; }
        public int GC11 { get; set; }
        public int GC16 { get; set; }
        public int GC21 { get; set; }
        public int GC26 { get; set; }
        public int GC31 { get; set; }
        public int GC36 { get; set; }
        public int GC41 { get; set; }
        public int GC46 { get; set; }
        public int GC51 { get; set; }
        public int RU0 { get; set; }    //玩牌0局的新注册用户
        public int RU1 { get; set; }
        public int RU2 { get; set; }
        public int RU3 { get; set; }
        public int RU4 { get; set; }
        public int RU5 { get; set; }
        public int RU6 { get; set; }
        public int RU11 { get; set; }
        public int RU16 { get; set; }
        public int RU21 { get; set; }
        public int RU26 { get; set; }
        public int RU31 { get; set; }
        public int RU36 { get; set; }
        public int RU41 { get; set; }
        public int RU46 { get; set; }
        public int RU51 { get; set; }
        public int KU0 { get; set; }        //玩牌0局的玩家的次日留存玩家数量
        public int KU1 { get; set; }
        public int KU2 { get; set; }
        public int KU3 { get; set; }
        public int KU4 { get; set; }
        public int KU5 { get; set; }
        public int KU6 { get; set; }
        public int KU11 { get; set; }
        public int KU16 { get; set; }
        public int KU21 { get; set; }
        public int KU26 { get; set; }
        public int KU31 { get; set; }
        public int KU36 { get; set; }
        public int KU41 { get; set; }
        public int KU46 { get; set; }
        public int KU51 { get; set; }
    }

    public class Ruin
    {
        public DateTime CountDate { get; set; }
        public int RuinCount { get; set; } //总人次
        public int RuinUsers { get; set; }//总人数
        public long RuinSum { get; set; }//总额
    }
}
