using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class BoardDist
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 新增用户
        /// </summary>
        public Int64 NewAddUser { get; set; }
        /// <summary>
        /// 总局数
        /// </summary>
        public Int64 BoardTotal { get; set; }
        /// <summary>
        /// 0局
        /// </summary>
        public Int64 Board0 { get; set; }
        /// <summary>
        /// 1局
        /// </summary>
        public Int64 Board1 { get; set; }
        /// <summary>
        /// 2局
        /// </summary>
        public Int64 Board2 { get; set; }
        /// <summary>
        /// 3局
        /// </summary>
        public Int64 Board3 { get; set; }
        /// <summary>
        /// 4局
        /// </summary>
        public Int64 Board4 { get; set; }
        /// <summary>
        /// 5局
        /// </summary>
        public Int64 Board5 { get; set; }

        public Int64 Board6_10 { get; set; }

        public Int64 Board11_15 { get; set; }

        public Int64 Board16_20 { get; set; }
        public Int64 Board21_25 { get; set; }
        public Int64 Board26_30 { get; set; }
        public Int64 Board31_35 { get; set; }
        public Int64 Board36_40 { get; set; }
        public Int64 Board41_45 { get; set; }

        public Int64 Board46_50 { get; set; }

        public Int64 BoardMore50 { get; set; }
    }
}
