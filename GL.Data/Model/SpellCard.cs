using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class SpellCard
    {
        public int ID { get; set; }
        /// <summary>
        /// 玩家ID
        /// </summary>
        public int PlayerID { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 购买拼手牌
        /// </summary>
        public decimal Chip { get; set; }
        /// <summary>
        /// 自定义手牌1
        /// </summary>
        public string CardOne { get; set; }
        /// <summary>
        /// 自定义手牌2
        /// </summary>
        public string CardTwo { get; set; }
        /// <summary>
        /// 奖励手牌
        /// </summary>
        public decimal ChipAward { get; set; }

        /// <summary>
        /// 手牌牌型1
        /// </summary>
        public string AwardOne { get; set; }
        /// <summary>
        /// 手牌牌型2
        /// </summary>
        public string AwardTwo { get; set; }
        /// <summary>
        /// 筹码数
        /// </summary>
        public decimal ChipTotal { get; set; }

    }
}
