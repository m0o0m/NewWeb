using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class DayReport
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 日新增
        /// </summary>
        public Int64 DayAdd { get; set; }
        /// <summary>
        /// DAU
        /// </summary>
        public Int64 DAU { get; set; }
        /// <summary>
        /// 日登陆总量
        /// </summary>
        public Int64 DayLoginCount { get; set; }
        /// <summary>
        /// 次日留存
        /// </summary>
        public double CiRiRate { get; set; }
        /// <summary>
        /// 七日留存
        /// </summary>
        public double QiRiRate { get; set; }
        /// <summary>
        /// 新增付费率
        /// </summary>
        public double AddPayRate { get; set; }
        /// <summary>
        /// 新增付费人数
        /// </summary>
        public double NewAddCount { get; set; }
        /// <summary>
        /// 新增付费
        /// </summary>
        public double Arppu { get; set; }
        /// <summary>
        /// 新增付费总额
        /// </summary>
        public double NewAddSum { get; set; }
        /// <summary>
        /// 日付费率
        /// </summary>
        public double DayPayRate { get; set; }
        /// <summary>
        /// 日付费人数
        /// </summary>
        public double DayPayCount { get; set; }
        /// <summary>
        /// 日付费arppu
        /// </summary>
        public double DayArppu { get; set; }
        /// <summary>
        /// 日付费总额
        /// </summary>
        public double DayPaySum { get; set; }
        /// <summary>
        /// 新增付费arpu
        /// </summary>
        public double AddArpu { get; set; }
        /// <summary>
        /// 日登录arpu
        /// </summary>
        public double DayLoginArpu { get; set; }
        /// <summary>
        /// 玩牌率
        /// </summary>
        public double PlayRate { get; set; }
        /// <summary>
        /// 总牌局数
        /// </summary>
        public double TotalCount { get; set; }
        /// <summary>
        /// 平均在玩
        /// </summary>
        public double AvgPlay { get; set; }
        /// <summary>
        /// 平均在线
        /// </summary>
        public double AvgOnLine { get; set; }
        /// <summary>
        /// 日发放
        /// </summary>
        public double DaySend { get; set; }
        /// <summary>
        /// 日消耗
        /// </summary>
        public double DayUser { get; set; }
        /// <summary>
        /// 日结余
        /// </summary>
        public double DayJieYu { get; set; }
        /// <summary>
        /// 系统存量
        /// </summary>
        public double SystemSave { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public int Agent { get; set; }
    }

    public class WeekTempReport
    {
        public double WeekAdd { get; set; }
        public double WeekLogin { get; set; }
        public double WeekPayCount { get; set; }
        public double WeekPaySum { get; set; }
        public DateTime WeekTime { get; set; }
    }


    public class WeekReport {
        /// <summary>
        /// 周新增
        /// </summary>
        public double WeekAdd { get; set; }
        /// <summary>
        /// 周登录
        /// </summary>
        public double WeekLogin { get; set; }
        /// <summary>
        /// 周付费率
        /// </summary>
        public double WeekPayRate { get; set; }
        /// <summary>
        /// 周累计付费人数
        /// </summary>
        public double WeekPayCount { get; set; }
        /// <summary>
        /// 周付费总额
        /// </summary>
        public double WeekPaySum { get; set; }
        /// <summary>
        /// 周时间
        /// </summary>
        public string WeekTime { get; set; }


        /// <summary>
        /// 上周新增
        /// </summary>
        public double LwWeekAdd { get; set; }
        /// <summary>
        /// 上周登录
        /// </summary>
        public double LwWeekLogin { get; set; }
        /// <summary>
        /// 上周付费率
        /// </summary>
        public double LwWeekPayRate { get; set; }
        /// <summary>
        /// 上周累计付费人数
        /// </summary>
        public double LwWeekPayCount { get; set; }
        /// <summary>
        /// 上周付费总额
        /// </summary>
        public double LwWeekPaySum { get; set; }
        /// <summary>
        /// 上周时间
        /// </summary>
        public string LwWeekTime { get; set; }



        /// <summary>
        /// 上月周新增
        /// </summary>
        public double LmWeekAdd { get; set; }
        /// <summary>
        /// 上月周登录
        /// </summary>
        public double LmWeekLogin { get; set; }
        /// <summary>
        /// 上月周付费率
        /// </summary>
        public double LmWeekPayRate { get; set; }
        /// <summary>
        /// 上月周累计付费人数
        /// </summary>
        public double LmWeekPayCount { get; set; }
        /// <summary>
        /// 上月周付费总额
        /// </summary>
        public double LmWeekPaySum { get; set; }
        /// <summary>
        /// 上月时间
        /// </summary>
        public string LmWeekTime { get; set; }

    }

    public class MonthTempReport
    {
        public double MonthAdd { get; set; }
        public double MonthLogin { get; set; }
        public double MonthPayCount { get; set; }
        public double MonthPaySum { get; set; }
        public DateTime MonthTime { get; set; }
    }

    public class MonthReport
    {
        /// <summary>
        /// 月新增
        /// </summary>
        public double MonthAdd { get; set; }
        /// <summary>
        /// 月登录
        /// </summary>
        public double MonthLogin { get; set; }
        /// <summary>
        /// 月付费率
        /// </summary>
        public double MonthPayRate { get; set; }
        /// <summary>
        /// 月累计付费人数
        /// </summary>
        public double MonthPayCount { get; set; }
        /// <summary>
        /// 月付费总额
        /// </summary>
        public double MonthPaySum { get; set; }
        /// <summary>
        /// 月时间
        /// </summary>
        public string MonthTime { get; set; }


        /// <summary>
        /// 上月新增
        /// </summary>
        public double LmMonthAdd { get; set; }
        /// <summary>
        /// 上月登录
        /// </summary>
        public double LmMonthLogin { get; set; }
        /// <summary>
        /// 上月付费率
        /// </summary>
        public double LmMonthPayRate { get; set; }
        /// <summary>
        /// 上月累计付费人数
        /// </summary>
        public double LmMonthPayCount { get; set; }
        /// <summary>
        /// 上月付费总额
        /// </summary>
        public double LmMonthPaySum { get; set; }
        /// <summary>
        /// 上月时间
        /// </summary>
        public string LmMonthTime { get; set; }



        /// <summary>
        /// 去年月新增
        /// </summary>
        public double LyMonthAdd { get; set; }
        /// <summary>
        /// 去年月登录
        /// </summary>
        public double LyMonthLogin { get; set; }
        /// <summary>
        /// 去年月付费率
        /// </summary>
        public double LyMonthPayRate { get; set; }
        /// <summary>
        /// 去年月累计付费人数
        /// </summary>
        public double LyMonthPayCount { get; set; }
        /// <summary>
        /// 去年月付费总额
        /// </summary>
        public double LyMonthPaySum { get; set; }
        /// <summary>
        /// 去年月时间
        /// </summary>
        public string LyMonthTime { get; set; }

    }
}
