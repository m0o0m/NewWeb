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

    public class BaseDataInfoForDailyOutput
    {
        public DateTime date { get; set; }
        public decimal Chip { get; set; }
        public decimal Diamond { get; set; }
        public decimal Score { get; set; }
    }


    public class BaseDataView
    {
        public int UserID { get; set; }
        public int Page { get; set; }
        public int Channels { get; set; }
        public terminals Terminals { get; set; }
        public string StartDate { get; set; }
        public string ExpirationDate { get; set; }
        public groupby Groupby { get; set; }
        public object BaseDataList { get; set; }

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
