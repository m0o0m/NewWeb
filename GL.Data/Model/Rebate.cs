using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class Rebate
    {
        public DateTime CreateTime { get; set; }

        public int UserID { get; set; }

        public string NickName { get; set; }
        /// <summary>
        /// 总返点
        /// </summary>
        public decimal TotalRebate { get; set; }
        /// <summary>
        /// 返点变化
        /// </summary>
        public decimal ChangeRebate { get; set; }
        /// <summary>
        /// 玩家操作项目
        /// </summary>
        public string Oper { get; set; }
    }
}
