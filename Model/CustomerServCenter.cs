using System;
using System.Collections.Generic;
using System.Text;
namespace GS.Model
{
    /// <summary>
    /// CustomerServCenter:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CustomerServCenter
    {
        public CustomerServCenter()
        { }
        #region Model
        private int _cscmainid;
        private int _cscsubid = 0;
        private int _guuserid = 0;
        private long _csctime = 0;
        private string _csctitle;
        private int? _csctype = 0;
        private string _csccontent = "无";
        private int? _cscstate = 0;
        private string _guname = "无";
        private int _gutype = 0;
        /// <summary>
        /// 
        /// </summary>

        public int CSCMainID
        {
            set { _cscmainid = value; }
            get { return _cscmainid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CSCSubId
        {
            set { _cscsubid = value; }
            get { return _cscsubid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GUUserID
        {
            set { _guuserid = value; }
            get { return _guuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long CSCTime
        {
            set { _csctime = value; }
            get { return _csctime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CSCTitle
        {
            set { _csctitle = value; }
            get { return _csctitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CSCType
        {
            set { _csctype = value; }
            get { return _csctype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CSCContent
        {
            set { _csccontent = value; }
            get { return _csccontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CSCState
        {
            set { _cscstate = value; }
            get { return _cscstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GUName
        {
            set { _guname = value; }
            get { return _guname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GUType
        {
            set { _gutype = value; }
            get { return _gutype; }
        }
        #endregion Model

    }
}
