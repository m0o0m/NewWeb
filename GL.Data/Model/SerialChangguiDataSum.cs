using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class SerialChangguiDataSum
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 昨天玩连环夺宝的人数
        /// </summary>
        public Int64 YestPlayCount { get; set; }

        /// <summary>
        /// 昨天玩连环夺宝的人次
        /// </summary>
        public Int64 YestPlayNum { get; set; }

        /// <summary>
        /// 昨天玩今天玩连环夺宝的人数
        /// </summary>
        public Int64 YestNowPlayCount { get; set; }

    }
}
