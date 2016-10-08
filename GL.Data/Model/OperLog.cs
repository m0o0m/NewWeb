using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    /// <summary>
    /// 后台操作记录
    /// </summary>
    public class OperLog
    {
        /// <summary>
        /// 操作时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 左边菜单栏
        /// </summary>
        public string LeftMenu { get; set; }
        /// <summary>
        /// 操作类型：添加。。。。
        /// </summary>
        public string OperType { get; set; }
        /// <summary>
        /// 操作详细，具体参数
        /// </summary>
        public string OperDetail { get; set; }

        public string IP { get; set; }


    }
    /// <summary>
    /// 后台配置 
    /// </summary>
    public class OperConfig {
        /// <summary>
        /// URL请求链接
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// URL请求描述
        /// </summary>
        public string ActionName { get; set; }
        public string ActionOper { get; set; }
        public string ActionDesc { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active { get; set; }

    }

    public class FreezeLog {
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 封号用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 被封的IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 被封的IMei
        /// </summary>
        public string IMei { get; set; }

        /// <summary>
        /// 封号时长
        /// </summary>
        public string TimeSpan { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperUserName { get; set; }
        /// <summary>
        /// 封号原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

    }

    /// <summary>
    /// 封号原因
    /// </summary>
    public enum FreezeStatus {
         刷游戏币 = 2,
         注册账号已经超过5个 = 1,
         发布恶意虚假信息 = 5,
         恶意攻击官服 = 6  
    }

    public enum FreezeTimeSpanStatus {
        _30分钟 = 30,
        _1小时 = 60,
        _3小时 = 180,
        _6小时 = 360,
        _12小时 = 720,
        _1天 = 1440,
        _2天 = 2880,
        _3天 = 4320,
        _7天= 10080,
        永久 = 5256000
    }

    public enum SpeakStatus {
        恶意刷屏 = 1,
        发布恶意辱骂信息 = 2,
        发布恶意虚假信息 = 3,
        言论与国家法律法规冲突 = 4

    }


     
}
