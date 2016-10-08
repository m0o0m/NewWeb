using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ThanksGiving
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CountTime { get; set; }

        /// <summary>
        /// 点击次数
        /// </summary>
        public Int64 ClickNum { get; set; }
        /// <summary>
        /// 点击人数(点击次数去重)
        /// </summary>
        public Int64 ClickCount { get; set; }
        /// <summary>
        /// 曝光次数
        /// </summary>
        public Int64 BaoGuangNum { get; set; }


        /// <summary>
        /// 每天领取30元返利的人次
        /// </summary>
        public Int64 Num30 { get; set; }
        /// <summary>
        /// 每天领取68元返利的人次
        /// </summary>
        public Int64 Num68 { get; set; }
        /// <summary>
        /// 每天领取128元返利的人次
        /// </summary>
        public Int64 Num128 { get; set; }
        /// <summary>
        /// 每天领取328元返利的人次
        /// </summary>
        public Int64 Num328 { get; set; }
        /// <summary>
        /// 每天领取300游戏币免费红包的人数
        /// </summary>
        public Int64 Num300 { get; set; }
        /// <summary>
        /// 每天领取500游戏币免费红包的人数
        /// </summary>
        public Int64 Num500 { get; set; }
        /// <summary>
        /// 每天领取700游戏币免费红包的人数
        /// </summary>
        public Int64 Num700 { get; set; }
        /// <summary>
        /// 每天领取1000游戏币免费红包的人数
        /// </summary>
        public Int64 Num1000 { get; set; }
        /// <summary>
        /// 每天领取1500游戏币免费红包的人数
        /// </summary>
        public Int64 Num1500 { get; set; }

        /// <summary>
        /// 扩展
        /// </summary>
        public Int64 Other { get; set; }

    }


    public class ThanksBaoGuang {
        public Int64 BaoGuang { get; set; }

        public Int64 Num { get; set;}

        public Int64 Count { get; set; }
    }


    public class ThanksRank {

        public Int64 TotalMoney { get; set; }

        public int UserID { get; set; }

        public string NickName { get; set; }

        public DateTime CountData { get; set; }

    }
}
