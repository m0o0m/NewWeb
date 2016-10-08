using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{

    public class QQZoneRechargeCount
    {
        public DateTime date { get; set; }
        /// <summary>
        /// 注册用户
        /// </summary>
        public decimal registerUser { get; set; }
        /// <summary>
        /// 充值用户
        /// </summary>
        public decimal rechargeUser { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal rechargeCount { get; set; }
        /// <summary>
        /// 当日注册并充值的玩家
        /// </summary>
        public decimal fristRecharge { get; set; }
        /// <summary>
        /// 充值率
        /// </summary>
        public decimal rechargeRate { get; set; }
        public decimal ARPU { get; set; }
        /// <summary>
        /// 再付费玩家
        /// </summary>
        public decimal nextRecharge { get; set; }
        /// <summary>
        /// 每日新增充值玩家
        /// </summary>
        public decimal newRecharge { get; set; }

    }

}
