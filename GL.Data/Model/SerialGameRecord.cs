using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class SerialGameRecord
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 牌局号
        /// </summary>
        public long Board { get; set; }
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
        /// <summary>
        /// 当前输赢游戏币
        /// </summary>
        public long RmainBet { get; set; }
        /// <summary>
        /// 彩池详情
        /// </summary>
        public string PotDetail { get; set; }
        /// <summary>
        /// 消除宝石
        /// </summary>
        public string Xiaochu { get; set; }
        /// <summary>
        /// 线数
        /// </summary>
        public int CountLine { get; set; }
        /// <summary>
        /// 当前关卡
        /// </summary>
        public int Tollgate { get; set; }
        /// <summary>
        /// 砖头数
        /// </summary>
        public int ZuantouCount { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public int Goal { get; set; }
        /// <summary>
        /// 龙珠探宝
        /// </summary>
        public string Longzhu { get; set; }
    }
    public enum SerialRoundID
    {
        加勒比游轮 = 1,
        迪拜塔 = 2,
        巴黎凯旋门 = 3,
        蒙特卡洛夜色 = 4,
        拉斯维加斯 = 5,
        澳门金沙城 = 6
    }


}
