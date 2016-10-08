using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GL;

namespace A1.Web.Models
{
    public class MCustomerServCenter
    {
        /// 
        /// </summary>
        [DbColumn(ColumnName = "CSCMainID")]
        public int CSCMainID { set; get; }
       
        
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "CSCSubId")]
        public int CSCSubId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "GUUserID")]
        public int GUUserID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "CSCTime")]
        public long CSCTime { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "CSCTitle")]
        public string CSCTitle { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "CSCType")]
        public int? CSCType { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "CSCContent")]
        public string CSCContent { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "CSCState")]
        public int? CSCState { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "GUName")]
        public string GUName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn(ColumnName = "GUType")]
        public int GUType { set; get; }


    }

}