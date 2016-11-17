using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    /// <summary>
    /// Slot配置
    /// </summary>
    public class WaterMargin
    {
        /// <summary>
        /// 列编号  1,2,3,4,5
        /// </summary>
        public int ColumnNO { get; set; }
        /// <summary>
        /// 铁斧权重
        /// </summary>
        public long Hatchet { get; set; }
        /// <summary>
        /// 银枪权重
        /// </summary>
        public long Gun { get; set; }
        /// <summary>
        /// 金刀权重
        /// </summary>
        public long Knife { get; set; }
        /// <summary>
        /// 鲁智深权重 
        /// </summary>
        public long Lu { get; set; }
        /// <summary>
        /// 林冲权重
        /// </summary>
        public long Lin { get; set; }
        /// <summary>
        /// 宋江权重 
        /// </summary>
        public long Song { get; set; }
        /// <summary>
        /// 替天行道权重
        /// </summary>
        public long God { get; set; }
        /// <summary>
        /// 忠义堂权重
        /// </summary>
        public long Hall { get; set; }
        /// <summary>
        /// 水浒传权重
        /// </summary>
        public long Outlaws { get; set; }
        /// <summary>
        /// 美酒权重
        /// </summary>
        public long Wine { get; set; }
        /// <summary>
        /// 类型   1方案1   2方案2   3方案3
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }


    public class MarryConfig {
        public int ID { get; set; }

        public Int64 Lowerlimit { get; set; }

        public Int64 Uperlimit { get; set; }
    }


    public class BiBeiConfig {
        public int ID { get; set; }
        public Int64 GameRound { get; set; }

        public Int64 GapWeight { get; set; }
    }

    public class ArcadeGameStock {
        public int ID { get; set; }
        /// <summary>
        /// 场次名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 实时库存值
        /// </summary>
        public Int64 StockValue { get; set; }
        /// <summary>
        /// 库存警戒线
        /// </summary>
        public Int64 StockCordon { get; set; }
        /// <summary>
        /// 库存值开关
        /// </summary>
        public int StockIsOpen { get; set; }

        public Int64 Param1 { get; set; }

        public Int64 Param2 { get; set; }

        public Int64 Param3 { get; set; }
        public Int64 Param4 { get; set; }
        public Int64 Param5 { get; set; }
        public Int64 Param6 { get; set; }
        public Int64 Param7 { get; set; }

        public DateTime CreateTime { get; set; }
    }

}
