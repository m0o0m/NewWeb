using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class QQZoneRecharge
    {

        /// <summary>
        /// 腾讯流水号
        /// </summary>
        public string BillNo { get; set; }
        /// <summary>
        /// 腾讯角色OpenID
        /// </summary>
        public string OpenID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PayItem { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long Money { get; set; }
        public decimal GetMoney { get { return Money / (decimal)100.0; } }


        public long ActualMoney { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long Chip { get; set; }
        public chipType ChipType { get; set; }
        public isFirst IsFirst { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public raType PF { get; set; }
        
        public string AgentName { get; set; }

        public string VersionInfo { get; set; }


        public int? Num { get; set; }

    }

    public enum chipType
    {
        金币 = 1,
        钻石 = 2,
        首冲礼包 = 3
    }
    public enum isFirst
    {
        否 = 0,
        是 = 1
    }

    public enum raType
    {
        腾讯 = 1,
        AppStore = 2,
        易宝 = 3,
        微信 = 4,
        支付宝 = 5,
        百度 = 6,
        XY助手 = 7,
        联通 = 8,
        应用宝 = 9,
        海马玩 = 10,
        移动 = 11,
        卓悠 = 12,
        应用汇 = 13,
        魅族 = 14,
        华为 = 15
    }

    public class raTypeHelper {
        public static string GetraType(int type) {
            switch (type) {
                case 1: return "腾讯";
                case 2: return "AppStore";
                case 3: return "易宝";
                case 4: return "微信";
                case 5: return "支付宝";
                case 6: return "百度";
                case 7: return "XY助手";
                case 8: return "联通";
                case 9: return "应用宝";
                case 10: return "海马玩";
                case 11: return "移动";
                case 12: return "卓悠";
                case 13: return "应用汇";
                case 14: return "魅族";
                case 15: return "华为";
                default: return "";

            }
        }

    }

    public class FirstChargeItem {
        public string PayItem { get; set;}
        public int Count { get; set; }
    }

    public class TexasGameGetAward {
        public DateTime  CreateTime { get; set; }
        public int Position_18 { get; set; }
        public int Position_58 { get; set; }
        public int Position_118 { get; set; }
        public int Position_238 { get; set; }

        public int Position_388 { get; set; }
    }


    public class NewYearRechargeSum {
        public int Count { get; set; }

        public int Postion { get; set; }
    }

    public class RechargeRank {
        public int UserID { get; set; }
        public string NickName { get; set; }

        public decimal Money { get; set; }
    }


    public class RechargeIP {
        public string IP { get; set; }
        public string Method { get; set; }
    }


    public class RechargeOpen {
        public string CreateTime { get; set; }

        public int RechargeID { get; set; }

        public string RechargeName { get; set; }

        public bool IsOpen { get; set; }

    }



}
