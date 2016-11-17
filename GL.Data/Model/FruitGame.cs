using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class FruitGameExplodeConfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 爆灯编号
        /// </summary>
        public int ExplodeNO { get; set; }
        /// <summary>
        /// 小苹果
        /// </summary>
        public int SmallApple { get; set; }
        /// <summary>
        /// 小橘子
        /// </summary>
        public int SmallOrange { get; set; }
        /// <summary>
        /// 小芒果
        /// </summary>
        public int SmallMango { get; set; }
        /// <summary>
        /// 小铃铛
        /// </summary>
        public int SmallRing { get; set; }
        /// <summary>
        /// 小西瓜
        /// </summary>
        public int SmallWatermalon { get; set; }
        /// <summary>
        /// 小双星
        /// </summary>
        public int SmallDoubleStar { get; set; }
        /// <summary>
        /// 小双七
        /// </summary>
        public int SmallDoubleSeven { get; set; }
        /// <summary>
        /// 苹果
        /// </summary>
        public int Apple { get; set; }
        /// <summary>
        /// 橘子
        /// </summary>
        public int Orange { get; set; }
        /// <summary>
        /// 芒果
        /// </summary>
        public int Mango { get; set; }
        /// <summary>
        /// 铃铛
        /// </summary>
        public int Ring { get; set; }
        /// <summary>
        /// 西瓜
        /// </summary>
        public int Watermalon { get; set; }
        /// <summary>
        /// 双星
        /// </summary>
        public int DoubleStar { get; set; }
        /// <summary>
        /// 双七
        /// </summary>
        public int DoubleSeven { get; set; }
        /// <summary>
        /// 小Bar
        /// </summary>
        public int SmallBar { get; set; }
        /// <summary>
        /// 大Bar
        /// </summary>
        public int BigBar { get; set; }
        /// <summary>
        /// 普通
        /// </summary>
        public int Normal { get; set; }
        /// <summary>
        /// 幸运奖励
        /// </summary>
        public int Lucky { get; set; }
        /// <summary>
        /// 随机奖励
        /// </summary>
        public int Random { get; set; }
        /// <summary>
        /// 小三元
        /// </summary>
        public int SmallThree { get; set; }
        /// <summary>
        /// 大三元
        /// </summary>
        public int BigThree { get; set; }
        /// <summary>
        /// 大四喜
        /// </summary>
        public int Bigfour { get; set; }
        /// <summary>
        /// 纵横四海
        /// </summary>
        public int Zong { get; set; }
        /// <summary>
        /// 天女散花
        /// </summary>
        public int TianNv { get; set; }
        /// <summary>
        /// 天龙八部
        /// </summary>
        public int TianLong { get; set; }
        /// <summary>
        /// 九宝莲灯
        /// </summary>
        public int JiuBao { get; set; }
        /// <summary>
        /// 大满贯
        /// </summary>
        public int GrandSlam { get; set; }
        /// <summary>
        /// 开火车
        /// </summary>
        public int OpenTrain { get; set; }
        /// <summary>
        /// 方案
        /// </summary>
        public int Type { get; set; }

        public string CreateTime { get; set; }
    }


    public class FruitBibeiConfig {
        /// <summary>
        /// 场次
        /// </summary>
        public int SeasonID { get; set; }
        /// <summary>
        /// 第一局
        /// </summary>
        public int GameNum1 { get; set; }
        /// <summary>
        /// 第2局
        /// </summary>
        public int GameNum2 { get; set; }
        /// <summary>
        /// 第3局
        /// </summary>
        public int GameNum3 { get; set; }
        /// <summary>
        /// 第n局
        /// </summary>
        public int GameNumn { get; set; }
    }

    public class FruitPotConfig {
        public int Id { get; set; }
        public int PlazeID { get; set; }

        public int Open { get; set; }

        public Int64 CurPot { get; set; }

        public Int64 Critical { get; set; }

    }


    public class FruitGameRecord {
        public string CreateTime { get; set; }
        /// <summary>
        /// 牌局ID
        /// </summary>
        public Int64 RoundID { get; set; }
        /// <summary>
        /// 场次ID
        /// </summary>
        public int PlazeID { get; set; }
        /// <summary>
        /// 房间ID
        /// </summary>
        public int RoomID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 变化前游戏币
        /// </summary>
        public decimal BeforeGold { get; set; }
        /// <summary>
        /// 变化后游戏币
        /// </summary>
        public decimal AfterGold { get; set; }
        /// <summary>
        /// 下注游戏币
        /// </summary>
        public decimal BetGold { get; set; }
        /// <summary>
        /// 赢取还是赔付
        /// </summary>
        public decimal WinGold { get; set; }
        /// <summary>
        /// 游戏类型 1爆灯 2比倍
        /// </summary>
        public int GameType { get; set; }
        /// <summary>
        /// 爆灯下注
        /// </summary>
        public string LampBet { get; set; }
        /// <summary>
        /// 爆灯的类型
        /// </summary>
        public int LampAwardType { get; set; }
        /// <summary>
        /// 爆了哪些灯
        /// </summary>
        public string LampInfo { get; set; }
        /// <summary>
        /// 彩池
        /// </summary>
        public Int64 BigPot { get; set; }
        /// <summary>
        /// 比倍信息
        /// </summary>
        public string SbInfo { get; set; }
    }

    public class FruitDataSum {
        /// <summary>
        /// 场次
        /// </summary>
        public int RoundID { get; set; }
         /// <summary>
         /// 时间
         /// </summary>
        public string CountDate { get; set; }
        /// <summary>
        /// 押注玩局人数
        /// </summary>
        public int LCount { get; set; }
        /// <summary>
        /// 押注玩局数
        /// </summary>
        public int LNum { get; set; }
        /// <summary>
        /// 比倍玩局人数
        /// </summary>
        public int BCount { get; set; }
        /// <summary>
        /// 比倍玩局数
        /// </summary>
        public int BNum { get; set; }
        /// <summary>
        /// 随机奖励人数
        /// </summary>
        public int Random { get; set; }
        /// <summary>
        /// ,幸运奖励
        /// </summary>
        public int Lucky { get; set; }
        /// <summary>
        /// 小三元
        /// </summary>
        public int SmallThree { get; set; }
        /// <summary>
        /// 大三元
        /// </summary>
        public int BigThree { get; set; }
        /// <summary>
        /// 大四喜
        /// </summary>
        public int Bigfour { get; set; }
        /// <summary>
        /// 开火车
        /// </summary>
        public int OpenTrain { get; set; }
        /// <summary>
        /// 纵横四海
        /// </summary>
        public int Zong { get; set; }
        /// <summary>
        /// 天女散花 
        /// </summary>
        public int TianNv { get; set; }
        /// <summary>
        /// 天龙八部
        /// </summary>
        public int TianLong { get; set; }
        /// <summary>
        /// 九莲宝灯
        /// </summary>
        public int JiuBao { get; set; }
        /// <summary>
        /// 大满贯
        /// </summary>
        public int GrandSlam { get; set; }
        /// <summary>
        /// 奖池发放次数
        /// </summary>
        public int PotNum { get; set; }
        /// <summary>
        /// 奖池发放总额
        /// </summary>
        public Int64 PotSum { get; set; }
    }

    public class FruitChangguiDataSum {
        public string CreateTime { get; set; }

        public Int64 YeatPlayCount { get; set; }

        public Int64 YeatNowPlayCount { get; set; }

        public Int64 ZhongCount { get; set; }

        public Int64 AllPlay { get; set; }
    }

    public enum FruitPlatze {
        澳门金沙城 = 1,
        拉斯维加斯 = 2,
        加勒比游轮 = 3,
        埃菲尔铁塔 = 4,
        巴黎凯旋门 = 5,
        蒙特卡洛 = 6
    }

    public enum FruitLampAwardType {
        普通 = 0,
        幸运 =1,
        随机=2,
        小三元=3,
        大三元=4,
        大四喜=5,
        纵横四海=6,
        天女散花=7,
        天龙八部=8,
        九宝连灯=9,
        大满贯=10,
        开火车=11

    }

    public enum FruitlampInfo {
        小苹果 = 0,
        小橙子 = 1,
        小芒果 = 2,
        小铃铛 = 3,
        小西瓜 = 4,
        小双星 = 5,
        小双7 = 6,
        大苹果 = 7,
        大橙子 = 8,
        大芒果 = 9,
        大铃铛 = 10,
        大西瓜 = 11,
        大双星 = 12,
        大双7 = 13,
        小BAR = 14,
        大BAR = 15,
        luck=16
    }
}
