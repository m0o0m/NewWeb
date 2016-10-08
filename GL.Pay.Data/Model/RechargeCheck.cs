using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.Data.Model
{
    public class RechargeCheck
    {
        public int ID { get; set; }
        public string SerialNo { get; set; }
        public int UserID { get; set; }
        public string ProductID { get; set; }
        public int Money { get; set; }
        public ulong CreateTime { get; set; }

    }
}
