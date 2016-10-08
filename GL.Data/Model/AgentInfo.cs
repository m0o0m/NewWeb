using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class AgentInfo
    {

        public int AgentID { get; set; }
        public string AgentAccount { get; set; }
        public string AgentName { get; set; }
        public string AgentPasswd { get; set; }
        public agentLv AgentLv { get; set; }
        public string AgentQQ { get; set; }
        public string AgentTel { get; set; }
        public string AgentEmail { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal? Deposit { get; set; }
        /// <summary>
        /// 初始金额
        /// </summary>
        public decimal? InitialAmount { get; set; }
        /// <summary>
        /// 可用金额
        /// </summary>
        public decimal? AmountAvailable { get; set; }
        /// <summary>
        /// 已用金额
        /// </summary>
        public decimal? HavaAmount { get; set; }
        public int? HigherLevel { get; set; }
        public int? LowerLevel { get; set; }
        public int? AgentState { get; set; }
        public int? OnlineState { get; set; }
        public DateTime? RegisterTime { get; set; }
        public string LoginIP { get; set; }
        public DateTime? LoginTime { get; set; }
        /// <summary>
        /// 充值
        /// </summary>
        public decimal? Recharge { get; set; }
        /// <summary>
        /// 安全
        /// </summary>
        public decimal? Drawing { get; set; }
        /// <summary>
        /// 安全密码
        /// </summary>
        public string DrawingPasswd { get; set; }
        /// <summary>
        /// 收益模式
        /// </summary>
        public revenueModel RevenueModel { get; set; }
        /// <summary>
        /// 收益比例
        /// </summary>
        public int? EarningsRatio { get; set; }
        /// <summary>
        /// 佣金比例
        /// </summary>
        public int? RebateRate { get; set; }

        public string JurisdictionID { get; set; }
        /// <summary>
        /// 是否为主站
        /// </summary>
        public bool Extend_isDefault { get; set; }


    }

    public class AgentInfoGroup{
        public int ID { get; set; }
        public string AgentName{get;set;}

        public int TypeList { get; set; }
    }

}
