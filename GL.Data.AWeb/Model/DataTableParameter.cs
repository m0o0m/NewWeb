using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class jQueryDataTableParamModel
    {
        private int _iDisplayLength = 0;
        /// <summary>
        /// DataTable请求服务器端次数
        /// </summary> 
        public string sEcho { get; set; }

        /// <summary>
        /// 过滤文本
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// 每页显示的数量
        /// </summary>
        public int iDisplayLength {
            get
            {
                return _iDisplayLength > 50 ? 50 : _iDisplayLength;
            }
            set { _iDisplayLength = value; }
        }

        /// <summary>
        /// 分页时每页跨度数量
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// 排序列的数量
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// 逗号分割所有的列
        /// </summary>
        public string sColumns { get; set; }
    }


}
