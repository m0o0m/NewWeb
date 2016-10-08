using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class Recharge
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
        /// <summary>
        /// 
        /// </summary>
        public long Chip { get; set; }
        public long Diamond { get; set; }
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
        
        public string VersionInfo { get; set; }


        public long ActualMoney { get; set; }

        public string ProductNO { get; set; }

        public int Num { get; set; }
    }


}
