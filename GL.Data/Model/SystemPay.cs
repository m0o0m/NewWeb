using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class SystemPay
    {
        
        public string CountDate { get; set; }
        /// <summary>
        /// 系统金币总量
        /// </summary>
        public string SystemChip { get; set; }
        /// <summary>
        /// 金币涨跌量
        /// </summary>
        public string ChipChange { get; set; }
        /// <summary>
        /// 金币涨跌比
        /// </summary>
        public string ChipRate { get; set; }
        /// <summary>
        /// 当日活跃金币总量
        /// </summary>
        public string ActiveChip { get; set; }
        /// <summary>
        /// 水族馆养鱼总额
        /// </summary>
        public string SystemFish { get; set; }
        /// <summary>
        /// 系统金币赠送
        /// </summary>
        public string SendChip { get; set; }
        /// <summary>
        /// 系统金币消耗
        /// </summary>
        public string ConsumeChip { get; set; }
        /// <summary>
        /// 系统五币总量
        /// </summary>
        public string SystemCoin { get; set; }

      
        /// <summary>
        /// 五币涨跌量
        /// </summary>
        public string CoinChnage { get; set; }
        /// <summary>
        /// 五币涨跌比
        /// </summary>
        public string CoinRate { get; set; }
        /// <summary>
        /// 当日活跃五币总量
        /// </summary>
        public string ActiveCoin { get; set; }
        /// <summary>
        /// 系统五币赠送
        /// </summary>
        public string SendCoin { get; set; }
        /// <summary>
        /// 系统五币消耗
        /// </summary>
        public string ConsumeCoin { get; set; }
    }
}
