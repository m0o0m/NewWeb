using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class RobotOutPut
    {
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal systemChip { get; set; }
        /// <summary>
        /// 添加金额
        /// </summary>
        public decimal AddMoney { get; set; }
        /// <summary>
        /// 服务费
        /// </summary>
        public decimal ServiceMoney { get; set; }
        public decimal DelMoney { get; set; }

        public string RecordTime { get; set; }

        /// <summary>
        /// 盈利
        /// </summary>
        public decimal Profit { get; set; }

    }
}
