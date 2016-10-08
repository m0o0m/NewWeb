using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class DotSum
    {
        /// <summary>
        /// 类型
        /// </summary>
        public ClientType ClientType { get; set; }
        /// <summary>
        /// 每日活跃
        /// </summary>
        public double Active { get; set; }
        /// <summary>
        /// 中发白人数
        /// </summary>
        public double ZFBNum { get; set; }
        /// <summary>
        /// 中发白次数
        /// </summary>
        public double ZFBCount { get; set; }
        /// <summary>
        /// 点击人数(去重)
        /// </summary>
        public double ClickNum { get; set; }
        /// <summary>
        /// 点击次数
        /// </summary>
        public double ClickCount { get; set; }

        public DateTime CreateTime { get; set; }

    }

    public enum ClientType {
        所有 = 0,
        Web端 = 1,
        安卓端 = 3,
        IOS端 = 2
    }
}
