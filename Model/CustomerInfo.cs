using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Model
{
    /// <summary>
    /// CustomerInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CustomerInfo
    {
        public CustomerInfo()
        { }
        #region Model
        private int _customerid;
        private string _customeraccount;
        private string _customerpwd;
        private int? _customerstate;
        private DateTime? _createdate;
        private string _customername;
        /// <summary>
        /// 
        /// </summary>
        public int CustomerID
        {
            set { _customerid = value; }
            get { return _customerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerAccount
        {
            set { _customeraccount = value; }
            get { return _customeraccount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerPwd
        {
            set { _customerpwd = value; }
            get { return _customerpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CustomerState
        {
            set { _customerstate = value; }
            get { return _customerstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerName
        {
            set { _customername = value; }
            get { return _customername; }
        }
        #endregion Model

    }
}
