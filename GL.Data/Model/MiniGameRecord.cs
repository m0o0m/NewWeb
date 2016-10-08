using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public  class MiniGameRecord
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>

        public string NickName { get; set; }
        /// <summary>
        /// 盲注
        /// </summary>
        public int nBlind { get; set; }
        /// <summary>
        /// 牌类型
        /// </summary>
        public int nCardType { get; set; }
        /// <summary>
        /// 牌,以下划线隔开
        /// </summary>
        public string Cards { get; set; }
        /// <summary>
        /// 翻牌前游戏币
        /// </summary>
        public decimal ChipBefore { get; set; }
        /// <summary>
        /// 消耗游戏币
        /// </summary>
        public decimal ChipUse { get; set; }
        /// <summary>
        /// 赢得游戏币
        /// </summary>
        public decimal ChipWin { get; set; }


    }


    public enum MiniType {
        高牌 = 0,
        对子 =1,
        顺子 =2,
        同花 = 3,
        同花顺 = 4,
        豹子 = 5,
        豹子A = 6
    }


    public class MiniGameSum {
        
        public DateTime CreateTime { get; set; }

        public decimal M0_100 { get; set; }

        public decimal M101_1000 { get; set; }

        public decimal M1001_5000 { get; set; }

        public decimal M5001_2W { get; set; }

        public decimal M2W01_10W { get; set; }

        public decimal M10W01_100W { get; set; }

        public decimal MMore100W { get; set; }

        public string XiaoHao { get; set; }

        public string ChanChu { get; set; }

        public string ClickNum { get; set; }

        public string ClickCount { get; set; }

        public string Active { get; set; }

    }

}
