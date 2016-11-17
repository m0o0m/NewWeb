using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class IAPProduct
    {
        /// <summary>
        /// 首冲赠送币
        /// </summary>
        public int attach_5b {get;set;} 
        /// <summary>
        /// 首冲赠送筹码
        /// </summary>
        public int attach_chip { get; set; } 
        /// <summary>
        /// 金额
        /// </summary>
        public decimal price { get; set; } 
        /// <summary>
        /// 数量
        /// </summary>
        public int goods { get; set; } 
        /// <summary>
        /// 1:游戏币 2:币 
        /// </summary>
        public int goodsType { get; set; } 
        /// <summary>
        /// 
        /// </summary>
        public string product_id {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public string productname { get; set; }


        public string attach_props { get; set; }
    }
}
