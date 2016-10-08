using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class UserMoneyRecord
    {
        public DateTime CreateTime { get; set; }
        public long UserID { get; set; }
        public long ChipNum { get; set; }
        public long ChipChange { get; set; }
        public long Diamond { get; set; }
        public long DiamondChange { get; set; }
        public long Score { get; set; }
        public long ScoreChange { get; set; }
        public chipChangeType Type { get; set; }
        public string UserOper { get; set; }
        public object UserName { get; set; }

        public string RoundID { get; set; }
   
    }

    public class UserInfo
    {
        public DateTime CreateTime { get; set; }  
        public string CreateIP { get; set; }
        public decimal recharge { get; set; }
        public decimal rechargecount { get; set; }
        public decimal money { get; set; }

        public decimal Score { get; set; }

        public int ID { get; set; }
        public long ServiceMoney { get; set; } //服务费
    }


}
