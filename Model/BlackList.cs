using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Model
{
    /// <summary>
    /// BlackList:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BlackList
    {
        public BlackList()
        { }
        #region Model
        private int _bid;
        private string _realname;
        private string _account;
        private string _mobilephone;
        private string _email;
        private string _qq;
        private string _loginip;
        private string _bankinformation;
        private int _vipId;

        public int ViPId
        {
            get { return _vipId; }
            set { _vipId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int bId
        {
            set { _bid = value; }
            get { return _bid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RealName
        {
            set { _realname = value; }
            get { return _realname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Account
        {
            set { _account = value; }
            get { return _account; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MobilePhone
        {
            set { _mobilephone = value; }
            get { return _mobilephone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QQ
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginIP
        {
            set { _loginip = value; }
            get { return _loginip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BankInformation
        {
            set { _bankinformation = value; }
            get { return _bankinformation; }
        }
        #endregion Model

    }
}
