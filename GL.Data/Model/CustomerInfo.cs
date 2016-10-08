using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GL.Data.Model
{
    public class CustomerInfo
    {
        public int CustomerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerPwd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CICustomerState CustomerState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerName { get; set; }

    }

    public enum CICustomerState
    {
        离线 = 0,
        在线 = 1
    }


}