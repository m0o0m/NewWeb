using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Common
{
    public class PagerQuery
    {
        private int _currentPageIndex = 1;
        private int _pageSize = 20;
        private int _recordCount;
        private int _startItemIndex;

        private string _sql;

        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPageIndex; }
            set { _currentPageIndex = value; }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        /// <summary>
        /// 查询语句
        /// </summary>
        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }


        public int StartRowNumber
        {
            get { return (CurrentPage - 1) * PageSize; }
        }

        public int EndRowNumber
        {
            get { return (StartRowNumber + PageSize); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount { get { return (int)Math.Ceiling(RecordCount / (double)PageSize); } }
        /// <summary>
        /// 
        /// </summary>
        public int StartItemIndex {
            get { return _startItemIndex; }
            set { _startItemIndex = value; }
        }

        public int EndItemIndex { get { return RecordCount > CurrentPage * PageSize ? CurrentPage * PageSize : RecordCount; } }


    }
}
