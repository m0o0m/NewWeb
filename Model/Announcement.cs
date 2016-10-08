using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Model
{
    /// <summary>
    /// Announcement:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Announcement
    {
        public Announcement()
        { }
        #region Model
        private int _aid;
        private long _atime = 0;
        private string _atitle = "无";
        private string _acontent = "空";
        private int _atype = 0;
        /// <summary>
        /// 
        /// </summary>
        public int AID
        {
            set { _aid = value; }
            get { return _aid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long ATime
        {
            set { _atime = value; }
            get { return _atime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ATitle
        {
            set { _atitle = value; }
            get { return _atitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AContent
        {
            set { _acontent = value; }
            get { return _acontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AType
        {
            set { _atype = value; }
            get { return _atype; }
        }
        #endregion Model

    }
}
