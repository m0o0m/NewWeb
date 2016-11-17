using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class GameUserInfo
    {

       public int ID { get; set; }			
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
       /// <summary>
       /// 密码
       /// </summary>
       public string Password { get; set; }		
       /// <summary>
       /// 金币
       /// </summary>
       public decimal Gold { get; set; }			
       /// <summary>
       /// 币
       /// </summary>
       public decimal Diamond { get; set; }			
       /// <summary>
       /// 紫卡
       /// </summary>
       public int Zicard { get; set; }			
       /// <summary>
       /// 话费
       /// </summary>
       public int Telfare { get; set; }			
       /// <summary>
       /// 最大权贵
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



    }
}
