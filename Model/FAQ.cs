using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Model
{	/// <summary>
    /// FAQ:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class FAQ
    {
        public FAQ()
        { }
        #region Model
        private int _id;
        private int? _faqtype;
        private string _faqtitle;
        private string _faqcontent;
        private DateTime? _operdate;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? faqtype
        {
            set { _faqtype = value; }
            get { return _faqtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string faqtitle
        {
            set { _faqtitle = value; }
            get { return _faqtitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string faqcontent
        {
            set { _faqcontent = value; }
            get { return _faqcontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? operdate
        {
            set { _operdate = value; }
            get { return _operdate; }
        }
        #endregion Model

    }
}
