using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class RechargeCheck
    {
        public int ID { get; set; }
        public string SerialNo { get; set; }
        public int UserID { get; set; }
        public string ProductID { get; set; }
        public int Money { get; set; }
        public ulong CreateTime { get; set; }
        public string CheckInfo { get; set; }

        public int AgentID { get; set; }


    }

    public class UserIpInfo {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 订单生成时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 生成订单的IP
        /// </summary>
        public string OrderIP { get; set; }


        public int ChargeType { get; set; }

        /// <summary>
        /// 回调IP
        /// </summary>
        public string CallBackIP { get; set; }


        public string Method { get; set; }
        /// <summary>
        /// 回调时间
        /// </summary>
        public DateTime CallBackTime { get; set; }



        public int CallBackChargeType { get; set; }


        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public object Data { get; set; }

        public int CallBackUserID { get; set; }
    }
}
