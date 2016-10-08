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
        public terminals Terminals { get; set; }
        public string StartDate { get; set; }
        public string ExpirationDate { get; set; }
        public groupby Groupby { get; set; }
        public object BaseDataList { get; set; }

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
}
