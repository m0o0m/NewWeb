using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class Role
    {
        public long ID { get; set; }
        /// <summary>
        /// 帐户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 真实名字
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string Identity { get; set; }
        /// <summary>
        /// 代理商
        /// </summary>
        public int Agent { get; set; }
        public string AgentName { get; set; }
        public string AgentAccount { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 金币
        /// </summary>
        public long Gold { get; set; }
        /// <summary>
        /// 515币
        /// </summary>
        public long Diamond { get; set; }
        /// <summary>
        /// 紫卡
        /// </summary>
        public long Zicard { get; set; }
        /// <summary>
        /// 话费
        /// </summary>
        public long Telfare { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int MaxNoble { get; set; }
        /// <summary>
        /// 显示礼物
        /// </summary>
        public int ShowGift { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 头像URL
        /// </summary>
        public string FigureUrl { get; set; }
        /// <summary>
        /// 是不是黄钻
        /// </summary>
        public int IsYellowVip { get; set; }
        /// <summary>
        /// 是不是年黄钻
        /// </summary>
        public int IsYellowVipYear { get; set; }
        /// <summary>
        /// 黄钻等级
        /// </summary>
        public int YellowVipLevel { get; set; }
        /// <summary>
        /// 是不是至尊黄钻
        /// </summary>
        public int IsYellowHighVip { get; set; }
        /// <summary>
        /// 平台
        /// </summary>
        public string PF { get; set; }
        /// <summary>
        /// QQ空间OPENID
        /// </summary>
        public string OpenID { get; set; }
        /// <summary>
        /// 邀请OPENID
        /// </summary>
        public string IOpenID { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        public string Invkey { get; set; }
        /// <summary>
        /// 邀请时间
        /// </summary>
        public string Itime { get; set; }
        /// <summary>
        /// 登录设备
        /// </summary>
        public int LoginDevice { get; set; }
        /// <summary>
        /// 上一次修改数据的时间
        /// </summary>
        public DateTime LastModify { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }		
        /// <summary>
        /// 创建IP
        /// </summary>
        public string CreateIP { get; set; }		
        /// <summary>
        /// 禁言
        /// </summary>
        public isSwitch NoSpeak { get; set; }		
        /// <summary>
        /// 封号
        /// </summary>
        public isSwitch IsFreeze { get; set; }

        public Byte[] BaseInfo { get; set; }
        public Byte[] ExtInfo { get; set; }


        /// <summary>
        /// 保险箱
        /// </summary>
        public decimal SafeBox { get; set; }
        /// <summary>
        /// 保险箱密码
        /// </summary>
        public string SafePwd { get; set; }

        public string CreateMac { get; set; }
        
        public int ClubID { get; set; }

        public isSwitch MasterLevel { get; set; }

        /// <summary>
        /// VIP等级
        /// </summary>
        public int VipGrade { get; set; }
        public int VipPoint { get; set; }

        /// <summary>
        /// 玩家等级
        /// </summary>
        public int LevelGrade { get; set; }
        public int LevelPoint { get; set; }

        /// <summary>
        /// 昨日盈利
        /// </summary>
        public long lastMoney { get; set; }

        /// <summary>
        /// 当前好友数量
        /// </summary>
        public short Friend { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 最大的金钱记录
        /// </summary>
        public long MaxGold { get; set; }
        public gameinfo GameInfo15 { get; set; }
        public gameinfo GameInfo14 { get; set; }
        public gameinfo GameInfo13 { get; set; }

        public isSwitch SwitchIP { get; set; }

        public isSwitch SwitchMac { get; set; }

        public string LastLoginIP {get; set;}

        /// <summary>
        /// 道具总数量
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// 到期礼物数量
        /// </summary>
        public int GiftExpire { get; set; }

        /// <summary>
        /// 当前礼物数量
        /// </summary>
        public int Gift { get; set; }


        /// <summary>
        /// 修改的属性 1VIP 2等级
        /// </summary>
        public int UpdateProperty { get; set; }


        public int TimeSelect { get; set; }


        public DateTime FreezeTime { get; set; }

        public DateTime SpeakTime { get; set; }

        //金钥匙数量
        public int GoldKey { get; set; }

        //兑换券个数
        public int Exchange { get; set; }

        //邮编
        public int Post { get; set; }

        //玩家详细地址
        public string Address { get; set; }

        //QQ号码
        public long QQNum { get; set; }
        public string VersionInfo { get; set; }

        public IsOnLine IsOnLine { get; set; }

        public int RoomID { get; set; }

        public gameID RoomType { get; set; }

        public string LoginAgent { get; set; }
    }

    public enum IsOnLine {
        离线 = 0,
        在线  = 1
    }

   


    public struct gameinfo
    {
        public int dwWin;          //赢的局数
        public int dwTotal;        //玩的总局数
        public int maxWinChip;     //最高一把赢取
    }

    public enum isSwitch
    {
        开 = 0,
        关 = 1
    }


    public class MD5Flow
    {
        //userid,md5,CreateTime,Category

        public int userid { get; set; }

        public string md5 { get; set; }

        public DateTime CreateTime { get; set; }

        public int Category { get; set; }
    }

}
