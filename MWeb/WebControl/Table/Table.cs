
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWeb
{
    public class Table:Control
    {
       
        /// <summary>
        /// 表格名称：用户列表
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 是否分页
        /// </summary>
        public bool IsPaging { get; set; }
        /// <summary>
        /// 表格表头信息
        /// </summary>
        public List<TableHeader> TableHeaders { get; set; }
        /// <summary>
        /// 表格数据
        /// </summary>
        public List<dynamic> TableData { get; set; }
        /// <summary>
        /// 显示第几页
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 每页显示的数据数量
        /// </summary>
        public int CountPerPage { get; set; }
        /// <summary>
        /// 数据总数量
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage {
            get {
                return Convert.ToInt32(Math.Ceiling(Total * 1.0 / CountPerPage));
            }
        }

        public string Url { get; set; }

    
        /// <summary>
        /// 事件
        /// </summary>
        public List<Event> Events { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public List<Event> EventsLine { get; set; }

        public override ControlType ControlType
        {
            get { return ControlType.Table; }
        }

    }

  

   
}
