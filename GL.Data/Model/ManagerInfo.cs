using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GL.Data.Model
{
    public class ManagerInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public int AdminID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AdminAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AdminPasswd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? AdminMasterRight { get; set; }

    }

    public enum userRole
    {
        管理员 = 1,
        代理 = 2,
        会员 = 3,
        客服 = 4
    }



}