using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class SignDraw
    {
        /// <summary>
        /// 每日登录抽奖总人数
        /// </summary>
        public int SignDrawManPerDay { get; set; }
        /// <summary>
        /// 花色不同牌型总人数
        /// </summary>
        public int DiffColorMan { get; set; }

        /// <summary>
        /// 三个梅花牌型总人数
        /// </summary>
        public int Club { get; set; }
        /// <summary>
        /// 三个方块牌型总人数
        /// </summary>
        public int Block { get; set; }

        /// <summary>
        /// 三个红桃牌型总人数
        /// </summary>
        public int Hearts { get; set; }
        /// <summary>
        /// 三个黑桃牌型总人数
        /// </summary>
        public int Spade { get; set; }
        /// <summary>
        /// 连续登录3天奖励总人数
        /// </summary>
        public int Three { get; set; }
        /// <summary>
        /// 连续登录5天奖励总人数
        /// </summary>
        public int Five { get; set; }
        /// <summary>
        /// 连续登录7天奖励总人数
        /// </summary>
        public int Seven { get; set; }
        /// <summary>
        /// 每日产出登录抽奖总游戏币
        /// </summary>
        public decimal LoginCoinPerDay { get; set; }

        /// <summary>
        /// 每日产出连续登录总游戏币
        /// </summary>
        public decimal ConLoginCoinPerDay { get; set; }
    }
}
