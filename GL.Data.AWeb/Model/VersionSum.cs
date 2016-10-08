using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class VersionSum
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo { get; set; }
        /// <summary>
        /// 日新增
        /// </summary>
        public Int64 DayAdd { get; set; }
        /// <summary>
        /// 新增玩牌率
        /// </summary>
        public float NewBoardRate { get; set; }
        /// <summary>
        /// 日活跃
        /// </summary>
        public Int64 DayActive { get; set; }
        /// <summary>
        /// 付费渗透率
        /// </summary>
        public float PayST { get; set; }
        /// <summary>
        /// 付费用户数
        /// </summary>
        public Int64 PayUserCount { get; set; }
        public float Arppu { get; set; }
        public float Arpu { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        public decimal TotalPay { get; set; }
    }


    public class ChangleSum
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string MaxVersionNo { get; set; }
        /// <summary>
        /// 日新增
        /// </summary>
        public Int64 DayAdd { get; set; }
        /// <summary>
        /// 新增玩牌率
        /// </summary>
        public float NewBoardRate { get; set; }
        /// <summary>
        /// 日活跃
        /// </summary>
        public Int64 DayActive { get; set; }
        /// <summary>
        /// 付费渗透率
        /// </summary>
        public float PayST { get; set; }
        /// <summary>
        /// 付费用户数
        /// </summary>
        public Int64 PayUserCount { get; set; }
        public float Arppu { get; set; }
        public float Arpu { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        public decimal TotalPay { get; set; }

        public int Chanlle { get; set;}


        public string AgentName { get; set; }
    }
}
