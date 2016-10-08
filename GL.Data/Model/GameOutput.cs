using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class GameOutput
    {
        public DateTime date { get; set; }
        /// <summary>
        /// 保险箱+身上的钱
        /// </summary>
        public decimal systemBargainingChip { get; set; }
        /// <summary>
        /// 水族馆鱼钱
        /// </summary>
        public decimal systemFishChip { get; set; }
        public decimal systemDiamond { get; set; }
        public decimal systemScore { get; set; }
        /// <summary>
        /// 当日产出
        /// </summary>
        public decimal ChipOutput { get; set; }
        /// <summary>
        /// 当日消耗
        /// </summary>
        public decimal ChipConsume { get; set; }
        public decimal ChipRecharge { get; set; }

        public decimal DiamondOutput { get; set; }
        public decimal DiamondConsume { get; set; }
        public decimal DiamondRecharge { get; set; }
        public decimal ChipAdd { get; set; }
        public decimal ChipDel { get; set; }
        /// <summary>
        /// 德州007盈利
        /// </summary>
        public long chip007 { get; set; }
        /// <summary>
        /// 外部彩池
        /// </summary>
        public long chipPot { get; set; }
        /// <summary>
        /// 游戏携带
        /// </summary>
        public long ChipGame { get; set; }
    }


    public class GameOutAccurate {
        public long TotoalGold { get; set; }//系统总游戏

        public long Gold1 { get; set; } //身上+保险箱


        public long Gold2 { get; set; }//水族馆的鱼

        public long Gold3 { get; set; }//进游戏携带

        public long Gold4 { get; set; }//外部彩池


        public string CurTime { get; set; } //当前时间

        public long OutPut { get; set; } //产出

        public long InPut { get; set; }//消耗
    }



    public class GameOutputList
    {
        public chipChangeType ChipChangeType { get; set; }
        public decimal Chip { get; set; }
        public decimal Diamond { get; set; }
        public decimal Score { get; set; }
    }
    public class GameOutputDetail
    {
        public DateTime date { get; set; }
        public List<GameOutputList> list { get; set; }
        public decimal systemBargainingChip { get; set; }
        public decimal systemDiamond { get; set; }
        public decimal systemScore { get; set; }
        public decimal ChipAdd { get; set; }
        public decimal ChipDel { get; set; }
    }

    public enum gameID
    {
        斗地主 = 12,
        中发白 = 13,
        十二生肖 = 14,
        德州扑克 = 15,
        小马快跑 = 16,
        奔驰宝马 = 17,
        百人德州 = 18
    }

    public enum itemName
    {
        金钥匙 = 1,
        兑换券 = 2

    }

    public enum chipChangeType
    {
        系统 = 0,
        /// <summary>
        /// 签到 + UCR_SIGNIN 
        /// </summary>
        签到奖励 = 1,
        /// <summary>
        /// 领取免费筹码(游戏时长) + UCR_FREEGOLD
        /// </summary>
        游戏时长奖励 = 2,
        /// <summary>
        /// 购买筹码 + UCR_BUYCHIP
        /// </summary>
        购买游戏币 = 3,
        /// <summary>
        /// 扣五币买游戏币 - UCR_BUYCHIP_BYDIA
        /// </summary>
        五币购买游戏币 = 4,
        /// <summary>
        /// 充值游戏币 + UCR_CHARGECHIP
        /// </summary>
        充值游戏币 = 5,
        /// <summary>
        /// 充值五币 UCR_CHARGEDIA
        /// </summary>
        充值五币 = 6,
        /// <summary>
        /// 购买礼物金币 - UCR_BUGGIFT_GOLD
        /// </summary>
        金币购买礼物 = 7,
        /// <summary>
        /// 购买礼物五币 - UCR_BUGGIFT_DIA
        /// </summary>
        五币购买礼物 = 8,
        /// <summary>
        /// 出售礼物 + UCR_SELLGIFT
        /// </summary>
        出售礼物 = 9,
        /// <summary>
        /// 购买权贵五币 - UCR_BUGNoble_DIA
        /// </summary>
        购买权贵五币 = 10,
        /// <summary>
        /// 购买权贵金币 + UCR_BUGNoble_GOLD
        /// </summary>
        购买权贵金币 = 11,
        /// <summary>
        /// 游戏分享 + UCR_GAMESHARE
        /// </summary>
        游戏分享 = 12,
        /// <summary>
        /// 邀请好友 + UCR_INVATEUSER
        /// </summary>
        邀请好友 = 13,
        /// <summary>
        /// 开心水族馆 - UCR_HAPPY_AQUARIUM_BUY
        /// </summary>
        购买开心水族馆 = 14,
        /// <summary>
        /// 开心水族馆 + UCR_HAPPY_AQUARIUM_SELL
        /// </summary>
        出售开心水族馆 = 15,
        /// <summary>
        /// 黄钻礼包 + UCR_YELLOWGIFT
        /// </summary>
        黄钻礼包 = 16,
        /// <summary>
        /// 福袋活动 - UCR_LUCKLY_BAG_ACTIVE
        /// </summary>
        福袋活动 = 17,
        /// <summary>
        /// 开启福袋 + UCR_LUCKLY_BAG_OPEN
        /// </summary>
        开启福袋 = 18,
        /// <summary>
        /// 德州比赛报名 - UCR_TEXAS_MATCH_REG
        /// </summary>
        德州比赛报名 = 19,
        /// <summary>
        /// 德州比赛赢取 + UCR_TEXAS_MATCH_WIN
        /// </summary>
        德州比赛赢取 = 20,
        /// <summary>
        /// 使用喇叭 - UCR_HORN
        /// </summary>
        使用喇叭 = 21,
        /// <summary>
        /// 使用表情(互动道具) - UCR_EXP
        /// </summary>
        互动道具 = 22,
        /// <summary>
        /// 牌桌小费 - UCR_TIP
        /// </summary>
        牌桌小费 = 23,
        /// <summary>
        /// 斗地主服务费 - UCR_LAND_SERVICEFEE
        /// </summary>
        斗地主服务费 = 24,
        /// <summary>
        /// 德州服务费 - UCR_TEXAS_SERVICEFEE
        /// </summary>
        德州服务费 = 25,
        /// <summary>
        /// 中发白服务费 - UCR_SCALE_SERVICEFEE
        /// </summary>
        中发白服务费 = 26,
        ///// <summary>
        ///// 斗地主结算 UCR_LAND_RESULT
        ///// </summary>
        斗地主结算 = 27,
        /// <summary>
        /// 德州结算 UCR_TEXAS_RESULT
        /// </summary>
        德州结算 = 28,
        /// <summary>
        /// 中发白结算 UCR_SCALE_RESULT
        /// </summary>
        中发白结算 = 29,
        /// <summary>
        /// 斗地主单局结算 UCR_LAND_SINGLE_RES
        /// </summary>
        斗地主单局结算 = 30,
        /// <summary>
        /// 中发白下庄 UCR_SCALE_LEAVEBANK
        /// </summary>
        中发白下庄 = 31,
        /// <summary>
        /// 中发白上庄 UCR_SCALE_GOBANK
        /// </summary>
        中发白上庄 = 32,
        /// <summary>
        /// 中发白补庄 UCR_SCALE_ADDBANK
        /// </summary>
        中发白补庄 = 33,
        /// <summary>
        /// 斗地主携带 UCR_LAND_CARRY
        /// </summary>
        斗地主携带 = 34,
        /// <summary>
        /// 德州携带 UCR_TEXAS_CARRY
        /// </summary>
        德州携带 = 35,
        /// <summary>
        /// 中发白下注 UCR_SCALE_BET
        /// </summary>
        中发白下注 = 36,
        /// <summary>
        /// 邮件金币 UCR_MAIL_GOLD
        /// </summary>
        邮件金币 = 37,
        /// <summary>
        /// 邮件五币 UCR_MAIL_DIA
        /// </summary>
        邮件五币 = 38,
        /// <summary>
        /// 创建角色金币 + UCR_CREATEROLE_GOLD
        /// </summary>
        创建角色金币 = 39,
        /// <summary>
        /// 创建角色五币 + UCR_CREATEROLE_DIA
        /// </summary>
        创建角色五币 = 40,
        /// <summary>
        /// 玩牌给积分 + UCR_PLAYGIVE_SCORE
        /// </summary>
        玩牌给积分 = 41,
        /// <summary>
        /// 充值获得积分 + UCR_RECHAREG_SCORE
        /// </summary>
        充值获得积分 = 42,
       /// <summary>
        /// 开福袋给积分 + UCR_LUCKEYBAG_SCORE
        /// </summary>
        开福袋给积分 = 43,
        /// <summary>
        /// 积分兑换物品 + UCR_DUIHUAN_SCORE
        /// </summary>
        玩就送扣积分 = 44,
        /// <summary>
        /// 首冲特惠 + UCR_FRISTCHARGE
        /// </summary>
        首冲特惠 = 45,
        /// <summary>
        /// 开心水族馆 卖鱼收益 + UCR_HAPPY_AQUARIUM_IN
        /// </summary>
        开心水族馆收益 = 46,
        /// <summary>
        /// 中发白提前下庄服务费 - UCR_SCALE_LEAVE_SERVICEFEE
        /// </summary>
        中发白提前下庄服务费 = 47,
        /// <summary>
        /// 苹果五星好评 + UCR_APPLE_REVIEW
        /// </summary>
        苹果五星好评 = 48,
        /// <summary>
        /// 彩池返奖扣除百分之十 - UCR_SCALE_RETURNHIDEPOT
        /// </summary>
        彩池返奖扣除百分之十 = 49,
        /// <summary>
        /// 中发白系统庄家服务费 - UCR_SCALE_SYSBANK_SERVICEFEE
        /// </summary>
        中发白庄家服务费 = 50,
        /// <summary>
        /// vip每日奖励 - UCR_SCALE_SYSBANK_SERVICEFEE
        /// </summary>
        vip每日尊享奖励 = 51,
        /// <summary>
        /// 德州获得积分 -UCR_TEXAS_JIFEN
        /// </summary>
        德州获得积分 = 52,               
        /// <summary>
        /// 斗地主获得积分 UCR_LAND_JIFEN
        /// </summary>
        斗地主获得积分 = 53,               
        /// <summary>
        ///  中发白获得积分 UCR_SCALE_JIFEN
        /// </summary>
        中发白获得积分 = 54,
        /// <summary>
        ///  德州玩游戏领经验 UCR_TEXAS_EXP
        /// </summary>
        德州玩游戏领经验 = 56,
        /// <summary>
        /// 斗地主游戏领经验 UCR_LAND_EXP
        /// </summary>
        斗地主游戏领经验 = 57,                
        /// <summary>
        /// 中发白游戏领经验 UCR_SCALE_EXP
        /// </summary>
        中发白游戏领经验 = 58,              
        /// <summary>
        /// 经验升级领金币 UCR_EXPUPLEVEL
        /// </summary>
        经验升级领金币 = 59,
        /// <summary>
        /// 新手任务领金币 UCR_TASK_COIN
        /// </summary>
        新手任务领金币 = 60,
        /// <summary>
        /// 每日任务领金币  UCR_DAYTASK_COIN
        /// </summary>
        每日任务领金币 = 61,              
        /// <summary>
        /// 每周任务领金币 UCR_WEEKTASK_COIN
        /// </summary>
        每周任务领金币 = 62,
        /// <summary>
        ///  宝石领金币
        /// </summary>
        宝石领金币 = 63,
        /// <summary>
        /// 五币买宝石 UCR_BUYGEM_BYDIA
        /// </summary>
        五币买宝石 = 64	,
        /// <summary>
        /// 十二生肖服务费 UCR_ZODIAC_SERVICEFEE
        /// </summary>
        十二生肖服务费 = 65,
        /// <summary>
        /// 十二生肖结算 UCR_ZODIAC_RESULT
        /// </summary>
        十二生肖结算 = 66,
        /// <summary>
        /// 积分换豪礼 UCR_SCORE_AWARD
        /// </summary>
        积分换豪礼 = 67,
        /// <summary>
        /// 拼手牌消耗 - UCR_SPELLCARD_CARRY
        /// </summary>
        拼手牌消耗 = 68,
        /// <summary>
        /// 十二生肖下注 UCR_ZODIAC_BET
        /// </summary>
        十二生肖下注 = 69,
        /// <summary>
        /// 拼中手牌得游戏币 + UCR_SPELLCARD_WIN
        /// </summary>
        拼中手牌得游戏币 = 70,
        /// <summary>
        /// 拼手牌开宝箱领金币+ UCR_SPELLCARDBOX_COIN
        /// </summary>
        拼手牌开宝箱领金币 = 71,
        /// <summary>
        /// 拼手牌开宝箱领经验+ UCR_SPELLCARDBOX_EXP
        /// </summary>
        拼手牌开宝箱领经验 = 72,
        /// <summary>
        /// 拼手牌开宝箱领宝石 UCR_SPELLCARDBOX_GEM
        /// </summary>
        拼手牌开宝箱领宝石 = 73,
        /// <summary>
        /// 拼手牌抢按钮领金币+ UCR_SPELLCARDBUTTON_COIN
        /// </summary>
        拼手牌抢按钮领金币 = 74,
        /// <summary>
        /// 拼手牌抢按钮领经验+ UCR_SPELLCARDBUTTON_EXP
        /// </summary>
        拼手牌抢按钮领经验 = 75,
        /// <summary>
        /// 十二生肖系统服务费- UCR_ZODIAC_SYSBANK_SERVICEFEE
        /// </summary>
        十二生肖庄家服务费 = 76,	
        /// <summary>
        /// 十二生肖获得积分 UCR_ZODIAC_GET_JIFEN
        /// </summary>
        十二生肖获得积分 = 77,
        /// <summary>
        /// 十二生肖获得经验 UCR_ZODIAC_EXP
        /// </summary>
        十二生肖获得经验 = 78,
        /// <summary>
        /// 德州大彩池获得金币 + UCR_TEXASPOT_COIN
        /// </summary>
        德州大彩池获得金币 = 79,
        /// <summary>
        /// 宝盆功能获得金币+ UCR_GOLD_POT_COIN
        /// </summary>
        金宝盆功能获得金币 = 80,          
        /// <summary>
        /// 中发白抢红包获得金币+  UCR_ROBEVENLOPE_COIN
        /// </summary>
        中发白抢红包获得金币 = 81,
        /// <summary>
        /// 中发白抢红包获得五币+ UCR_ROBEVENLOPE_DIA
        /// </summary>
        中发白抢红包获得五币 = 82,
        /// <summary>
        /// 银宝盆功能获得金币+ URC_YIN_POT_COIN
        /// </summary>
        银宝盆功能获得金币 = 83,
        /// <summary>
        /// 中发白大彩池获得金币 + UCR_SCALEPOT_COIN
        /// </summary>
        中发白大彩池获得金币 = 84,
        /// <summary>
        /// 俱乐部奖励 + UCR_CLUBAWARD
        /// </summary>
        俱乐部奖励 = 85,
        /// <summary>
        ///  中发玩游戏领大奖获得金币+ UCR_PLAYGAME_SCALE_COIN
        /// </summary>
        中发白之王 = 86,
        /// <summary>
        /// 中发玩游戏领大奖获得宝石 + UCR_PLAYGAME_SCALE_GEM
        /// </summary>
        中发玩游戏领大奖获得宝石 = 87,
        /// <summary>
        /// 十二生肖游戏领大奖获得金币 + UCR_PLAYGAME_ZODIC_COIN
        /// </summary>
        十二生肖之王 = 88,
        /// <summary>
        /// 十二生肖玩游戏领大奖获得宝石 + UCR_PLAYGAME_ZODIC_GEM
        /// </summary>
        十二生肖玩游戏领大奖获得宝石 = 89,
        /// <summary>
        /// 浇花灌溉得金币 + UCR_FLOWER_SCALE_COIN
        /// </summary>
        浇花灌溉得金币 = 90,
        /// <summary>
        /// 小马快跑服务费
        /// </summary>
        小马快跑服务费 = 92,
        /// <summary>
        /// 积分争霸获得金币 + UCR_JIFEN_SCRAMBLE_COIN
        /// </summary>
        积分争霸获得金币 = 102,

        小马结算 = 91,

        小马下庄 = 93,

        小马上庄 = 94,

        小马补庄 = 95,

        小马下注 = 96,

        小马提前下庄服务费 = 97,

        小马彩池返奖扣除百分之十 = 98,

        小马庄家服务费 = 99,

        小马获得积分 = 100,

        小马游戏领经验 = 101,

        十二生肖下庄 = 103,

        十二生肖上庄 = 104,

        十二生肖补庄 = 105,


        十二生肖提前下庄服务费 = 106,
        /// <summary>
        /// UCR_ACHIEVE_COIN = 107,		
        /// </summary>
        成就奖励=107,

        /// <summary>
        /// UCR_TEXAW_COIN = 108,		
        /// </summary>
        德州玩牌赢大礼 = 108 ,

        /// <summary>
        /// UCR_CHARGE_REBATE_COIN = 109,	
        /// </summary>
        充值返利 = 109,

        /// <summary>
        /// UCR_CHARGE_RANK_COIN = 110,				
        /// </summary>
        充值排名奖励 = 110,

        小马快跑之王 = 111 ,

        小马快跑领大奖获得宝石 = 112,

        //奔驰宝马
        /*
        UCR_CAR_RESULT = 113,				//奔驰宝马结算
	    UCR_CAR_SERVICEFEE = 114,				//奔驰宝马服务费
	    UCR_CAR_LEAVEBANK = 115,					//奔驰宝马下庄
	    UCR_CAR_GOBANK = 116,					//奔驰宝马上庄
	    UCR_CAR_ADDBANK = 117,					//奔驰宝马补庄
	    UCR_CAR_BET = 118,										//奔驰宝马下注
	    UCR_CAR_LEAVE_SERVICEFEE = 119,				//奔驰宝马提前下庄服务费-
	    UCR_CAR_RETURNHIDEPOT = 120,					//奔驰宝马彩池返奖扣除10%
	    UCR_CAR_SYSBANK_SERVICEFEE=121,			//奔驰宝马系统庄家服务费
	    UCR_CAR_JIFEN = 122,					//奔驰宝马获得积分+
	    UCR_CAR_EXP= 123,						//奔驰宝马游戏领经验

        */

        /// <summary>
        /// UCR_CAR_RESULT
        /// </summary>
        奔驰宝马结算 = 113,
        /// <summary>
        /// UCR_CAR_SERVICEFEE
        /// </summary>
        奔驰宝马服务费 = 114,
        /// <summary>
        /// UCR_CAR_LEAVEBANK
        /// </summary>
        奔驰宝马下庄 = 115,
        /// <summary>
        /// UCR_CAR_GOBANK
        /// </summary>
        奔驰宝马上庄 = 116,
        /// <summary>
        /// UCR_CAR_ADDBANK
        /// </summary>
        奔驰宝马补庄 = 117,
        /// <summary>
        /// UCR_CAR_BET
        /// </summary>
        奔驰宝马下注 = 118,
        /// <summary>
        /// UCR_CAR_LEAVE_SERVICEFEE
        /// </summary>
        奔驰宝马提前下庄服务费 = 119,
        /// <summary>
        /// UCR_CAR_RETURNHIDEPOT
        /// </summary>
        奔驰宝马彩池返奖扣除百分之十 = 120,  //用%替代_515_
        /// <summary>
        /// UCR_CAR_SYSBANK_SERVICEFEE
        /// </summary>
        奔驰宝马庄家服务费 = 121,
        /// <summary>
        /// UCR_CAR_JIFEN
        /// </summary>
        奔驰宝马获得积分 = 122,
        /// <summary>
        /// UCR_CAR_EXP
        /// </summary>
        奔驰宝马游戏领经验 = 123,

        领取奔驰宝马之王奖励 = 124,

        签到领取游戏币 = 127,
        连续签到5天 = 129,
        保险箱取 = 131,
        保险箱存 = 132,
        轮盘抽奖获取金币 = 133,
        轮盘抽奖获取五币 = 134,
        兑换券兑换金币 = 135,
        兑换券兑换五币 = 136,
        轮盘抽奖消耗金币 = 137,

        棋牌日领游戏币 = 139,

        棋牌日VIP奖励 = 140,

        首充五币奖励 = 141,

        任务五币奖励 = 142,

        玩就送兑换游戏币 = 154,

        修改昵称扣游戏币 = 143,

        修改昵称奖励 = 144,

        修改头像奖励 = 145,

        任务达人奖励 = 146,

        超级任务奖励 = 147,

        领取破产奖励 = 148,

        任务达人奖励五币 = 149,

        超级任务奖励五币 = 150,

        后台赠送 = 151,
        加群奖励 = 152,

        数据回滚 = 153,

        德州大返利 = 155,
     
        德州百人结算 = 156,
        德州百人服务费 = 157,
        德州百人下庄 = 158,
        德州百人上庄 = 159,
        德州百人补庄 = 160,
        德州百人下注 = 161,
        德州百人系统庄家服务费 = 162,
        德州百人获得积分 = 163,
        德州百人游戏领经验 = 164,
        邮件领积分 = 165,
        百人德州大彩池获得金币 = 166,//百人德州大彩池

        商品返利游戏币 = 167,
        免费红包游戏币 = 168,

        数据回滚_扣游戏币成功后人物离线 = 169,
        数据回滚_扣五币成功后人物离线 = 170,
        数据回滚_奔驰宝马下注 = 171,
        数据回滚_奔驰宝马重复下注 = 172,
        数据回滚_奔驰宝马上庄总数量上限超过30个 = 173,
        数据回滚_奔驰宝马上庄个人数量上限超过5个 = 174,
        数据回滚_奔驰宝马补庄成功后超时 = 175,
        数据回滚_德州自动加入 = 176,
        数据回滚_德州主动坐下抢最后一个位置失败 = 177,
        数据回滚_德州主动坐下同一IP两个号同时坐下 = 178,
        数据回滚_百人德州使用互动道具扣五币 = 179,
        数据回滚_百人德州互动道具扣游戏币 = 180,
        数据回滚_百人德州下注超过庄家可下注金额 = 181,
        数据回滚_百人德州下注超过自己可下注金额 = 182,
        数据回滚_百人德州重复下注超过庄家可下注金额 = 183,
        数据回滚_百人德州重复下注超过自己可下注金额 = 184,
        数据回滚_百人德州上庄总数量上限超过30个 = 185,
        数据回滚_百人德州上庄个人数量上限超过5个 = 186,
        数据回滚_中发白上庄个人数量上限超过5个 = 187,
        数据回滚_中发白上庄总数量超过30个 = 188,
        数据回滚_中发白下注 = 189,
        数据回滚_中发白重复下注 = 190,
        数据回滚_中发白补庄 = 191,
        数据回滚_中发白使用互动道具扣五币 = 192,
        数据回滚_中发白互动道具扣游戏币 = 193,
        数据回滚_十二生肖上庄总数量上限超过30个 = 194,
        数据回滚_十二生肖上庄个人数量上限超过5个 = 195,
        数据回滚_十二生肖下注 = 196,
        数据回滚_十二生肖重复下注 = 197,
        数据回滚_十二生肖补庄 = 198,

        翻翻乐消耗游戏币 = 199,
        翻翻乐获得游戏币 = 200,

        中秋节充值返利 = 201,
        中秋节领取月饼 = 202,



        转盘使用金钥匙 = 10001,
        签到得到金钥匙 = 10002,
        转盘获得兑换券 = 10003,
        使用兑换券兑换物品 = 10004,

        棋牌日领取宝石 = 10005,
        购买宝石 = 10006,
        宝石过期 = 10007,

        任务获取 = 10008,

        拼手牌获取 = 10009,
        签到领奖 = 10010,
        邮件领取 = 10011,

        首冲赠送 = 10012,

        任务达人奖励宝石 = 10013,

        超级任务奖励宝石 = 10014

    }
}
