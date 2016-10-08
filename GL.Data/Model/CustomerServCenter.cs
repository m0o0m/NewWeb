using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GL.Data.Model
{
    public class CustomerServCenter
    {
        public int CSCMainID { set; get; }
        public int CSCSubId { set; get; }
        public int GUUserID { set; get; }
        public DateTime CSCTime { set; get; }
        public string CSCTitle { set; get; }
        public cscType CSCType { set; get; }
        public string CSCContent { set; get; }
        public cscState CSCState { set; get; }
        public string GUName { set; get; }
        public int GUType { set; get; }

        public DateTime CSCUpdateTime { get; set; }
    }

    public enum cscType
    {
        程序错误 = 1,
        改进意见 = 2,
        账号问题 = 3,
        支付问题 = 4,
        活动问题 = 5
    }
    public enum cscState
    {
        未处理 = 1,
        已回复 = 2,
        已解决 = 3
    }

}