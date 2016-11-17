using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ShuihuGameRecord
    {
        public int ID { get; set; }

        public string CreateTime { get; set; }
        /// <summary>
        /// 牌局号
        /// </summary>
        public Int64 Board { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 场次
        /// </summary>
        public int RoundID { get; set; }
        /// <summary>
        /// 房间ID
        /// </summary>
        public int RoomID { get; set; }
        /// <summary>
        /// 变化前游戏币
        /// </summary>
        public long BeforeGold { get; set; }
        /// <summary>
        /// 变化后游戏币
        /// </summary>
        public long AfterGold { get; set; }
        /// <summary>
        /// 变化前五币 
        /// </summary>
        public long BeforeDiamond { get; set; }
        /// <summary>
        /// 变化后五币
        /// </summary>
        public long AfterDiamond { get; set; }

        /// <summary>
        /// 押注
        /// </summary>
        public long Bet { get; set; }
        /// <summary>
        /// 赔付
        /// </summary>
        public long Pay { get; set; }

      
        public int IsFree { get; set; }
     
        /// <summary>
        /// 彩池详细
        /// </summary>
        public string PotDetail { get; set; }
        /// <summary>
        /// 比倍详细
        /// </summary>
        public string TimesDetail { get; set; }
        /// <summary>
        /// 小玛丽详细
        /// </summary>
        public string MaryDetail { get; set; }

        public string Patterns { get; set; }

    }


    public enum ShuihuRound
    {
        梁山泊 =1,
        五台山 = 2,
        二龙山 = 3,
        景阳冈=4,
        祝家庄 = 5,
        乌龙岭 = 6
    }

    public enum ShuihuPatterns {
        斧头 =1,
        银枪 = 2,
        金刀 = 3,
        鲁智深 = 4,
        林冲  =5,
        宋江 = 6,
        替天行道 = 7,
        忠义堂 = 8,
        水浒传 = 9,
        美酒 = 10
    }


    public class ShuihuDataSum {
        /// <summary>
        /// 时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 系统消耗
        /// </summary>
        public decimal SystemConsume { get; set; }
        /// <summary>
        /// 拉霸玩局人数
        /// </summary>
        public Int64 LCount { get; set; }
        /// <summary>
        /// 拉霸玩局数
        /// </summary>
        public Int64 LNum { get; set; }
        /// <summary>
        /// 拉霸中奖次数
        /// </summary>
        public Int64 LZhongCount { get; set; }
        /// <summary>
        /// 拉霸回报率
        /// </summary>
        public string LReturnRate { get; set; }
        /// <summary>
        /// 比倍玩局人数
        /// </summary>
        public Int64 BCount { get; set; }
        /// <summary>
        /// 比倍玩局数
        /// </summary>
        public Int64 BNum { get; set; }
        /// <summary>
        /// 比倍回报率
        /// </summary>
        public string BReturnRate { get; set; }
        /// <summary>
        /// 免费拉霸触发次数5次
        /// </summary>
        public Int64 MLNum5 { get; set; }
        /// <summary>
        /// 免费拉霸触发次数10次
        /// </summary>
        public Int64 MLNum10 { get; set; }
        /// <summary>
        /// 免费拉霸触发次数15次
        /// </summary>
        public Int64 MLNum15 { get; set; }
        /// <summary>
        /// 小玛丽触发总次数
        /// </summary>
        public Int64 MaryNum { get; set; }
        /// <summary>
        /// 作弊触发次数
        /// </summary>
        public Int64 MaryZuobiNum { get; set; }
        /// <summary>
        /// 平均中奖倍数
        /// </summary>
        public float MaryAverageNum { get; set; }
        /// <summary>
        /// 最大中奖倍数
        /// </summary>
        public Int32 MaryMax { get; set; }

        /// <summary>
        /// 全盘奖次数-武器
        /// </summary>
        public Int64 QWuqi { get; set; }
        /// <summary>
        /// 全盘奖次数-任务
        /// </summary>
        public Int64 QRenwu { get; set; }
        /// <summary>
        /// 全盘奖次数-铁斧
        /// </summary>
        public Int64 QTiefu { get; set; }
        /// <summary>
        /// 全盘奖次数-银枪
        /// </summary>
        public Int64 QYinQiang { get; set; }
        /// <summary>
        /// 全盘奖次数-金刀
        /// </summary>
        public Int64 QJinDao { get; set; }
        /// <summary>
        /// 全盘奖次数-鲁智深
        /// </summary>
        public Int64 QLu { get; set; }
        /// <summary>
        /// 全盘奖次数-林冲
        /// </summary>
        public Int64 QLin { get; set; }
        /// <summary>
        /// 全盘奖次数-宋江
        /// </summary>
        public Int64 QSong { get; set; }
        /// <summary>
        /// 全盘奖次数-替天行道
        /// </summary>
        public Int64 QTi { get; set; }
        /// <summary>忠义堂武器
        /// </summary>
        public Int64 QZhong { get; set; }
        /// <summary>
        /// 全盘奖次数-全盘龙
        /// </summary>
        public Int64 QQuan { get; set; }
        /// <summary>
        /// 奖池发放次数
        /// </summary>
        public Int64 PotNum { get; set; }
        /// <summary>
        /// 奖池发放总额
        /// </summary>
        public Int64 QSum { get; set; }


    }


    public class ShuihuChangguiDataSum {
        public string CreateTime { get; set; }

        public Int64 YeatPlayCount { get; set; }

        public Int64 YeatNowPlayCount { get; set; }

        public Int64 ZhongCount { get; set; }

        public Int64 AllPlay { get; set; }
    }

    public class ShuihuPot {
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 牌局号
        /// </summary>
        public Int64 Board { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 场次
        /// </summary>
        public int RoundID { get; set; }
        /// <summary>
        /// 中奖前身上游戏币
        /// </summary>
        public Int64 BeforeGold { get; set; }
        /// <summary>
        /// 获得的奖池
        /// </summary>
        public Int64 GetGold { get; set; }

    }


    public class ShuihuMary {
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 牌局号
        /// </summary>
        public Int64 Board { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Int32 UserID { get; set; }
        /// <summary>
        /// 场次 
        /// </summary>
        public int RoundID { get; set; }
        /// <summary>
        /// 初始得分
        /// </summary>
        public Int64 InitGold { get; set; }
        /// <summary>
        /// 赢分
        /// </summary>
        public Int64 WinGold { get; set; }
        /// <summary>
        /// 压住分
        /// </summary>
        public Int64 PayGold { get; set; }

    }
}
