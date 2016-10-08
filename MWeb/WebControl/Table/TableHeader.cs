using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWeb
{
    public class TableHeader
    {
        /// <summary>
        /// 表格 表头列名称
        /// </summary>
        public string HeaderName { get; set; }
        /// <summary>
        /// 表格 表头关联数据列
        /// </summary>
        public string ConName { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public string Width { get; set; }
        /// <summary>
        /// 样式
        /// </summary>
        public string Style { get; set; }
        /// <summary>
        /// 是否可现
        /// </summary>
        public bool IsVisiable { get; set; }

    }
}
