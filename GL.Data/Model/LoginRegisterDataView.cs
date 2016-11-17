using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class LoginRegisterDataView
    {
        /// <summary>
        /// 手机系统平台
        /// </summary>
        public int Platform { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public int Channels { get; set; }
        /// <summary>
        /// 手机品牌
        /// </summary>
        public string PhoneBoard { get; set; }
        /// <summary>
        /// 手机品牌机型
        /// </summary>
        public string PhoneModels{get;set;}

        public int Page { get; set; }

        public object Data { get; set; }
    }

    public class LoginRegisterDetail {
        /// <summary>
        /// 游客注册开关
        /// </summary>
        public int RYouKe { get; set; }
        /// <summary>
        /// qq注册开关
        /// </summary>
        public int RQQ { get; set; }
        /// <summary>
        /// 微信注册开关
        /// </summary>
        public int RWeiXin { get; set; }
        /// <summary>
        /// 注册开关
        /// </summary>
        public int R515 { get; set; }
        /// <summary>
        /// 游客登录开关
        /// </summary>
        public int LYouKe { get; set; }
        /// <summary>
        /// qq登录开关
        /// </summary>
        public int LQQ { get; set; }
        /// <summary>
        /// 微信登录开关
        /// </summary>
        public int  LWeiXin{get;set;}
        /// <summary>
        /// 515登录开关
        /// </summary>
        public int L515 { get; set; }

        public int LID { get; set; }
        public int RID { get; set; }

        public int LOther { get; set; }
        public int ROther { get; set; }

        public RLFlow RLFlow { get; set;}
    }

    public class RLFlow {

        public int ID { get; set; }
        public int Agent { get; set; }
        public string AgentName { get; set; }

        public int Platform { get; set; }

        public string PlatformName { get; set; }


        public int PhoneBoardID { get; set; }
        public string PhoneBoardName { get; set; }

        public int PhoneModelsID { get; set; }
        public string PhoneModelsName { get; set; }

        public int Page { get; set; }
        public string OperType { get; set; }

    }


    public enum PlatformL {
        所有 = -1,
        Android = 1,
        IOS = 2
    }

    public enum RLOperType {
        游客注册 = 1,
        QQ注册 = 2,
        微信注册 = 3,
        _515注册 = 4,
        游客登录 = 5,
        QQ登录 =6,
        微信登录 = 7,
        _515登录 = 8
    }

    public enum LoginType {
        所有  =-1,
        QQ  =0,
        微信 = 1,
        游客 =2,
        ID = 3,
        五一五账号 = 4,
        第三方渠道 = 5


    }
}
