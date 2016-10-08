using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GL.Common.Data
{
    /// <summary>
    /// PageList 的摘要说明。
    /// </summary>
    public class PagerQueryMySQL
    {
        private int _currentPageIndex = 1;
        private int _pageSize = 20;
        private int _pageCount;
        private int _recordCount;

        private string _id;
        private string _fromClause;
        private string _groupClause;
        private string _selectClause = "*";
        private string _sortClause;
        private int _orderType;
        private int _distClause;
        private StringBuilder _whereClause;

        public PagerQueryMySQL()
        {
            _whereClause = new StringBuilder();
        }

        /// <summary>
        /// 主表的主键
        /// </summary>
        public string IDClause
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 显示的字段列表
        /// </summary>
        public string SelectClause
        {
            get { return _selectClause; }
            set { _selectClause = value; }
        }

        /// <summary>
        /// 要显示的表或多个表的连接
        /// </summary>
        public string FromClause
        {
            get { return _fromClause; }
            set { _fromClause = value; }
        }

        /// <summary>
        /// 查询条件,不需where
        /// </summary>
        public StringBuilder WhereClause
        {
            get { return _whereClause; }
            set { _whereClause = value; }
        }

        /// <summary>
        /// 聚合查询
        /// </summary>
        public string GroupClause
        {
            get { return _groupClause; }
            set { _groupClause = value; }
        }

        /// <summary>
        /// 排序字段列表或条件(如果是多字段排列Sort指代最后一个排序字段的排列顺序(最后一个排序字段不加排序标记)--程序传参如：' SortA Asc,SortB Desc,SortC '
        /// </summary>
        public string SortClause
        {
            get { return _sortClause; }
            set { _sortClause = value; }
        }

        /// <summary>
        /// 排序方法，0为升序，1为降序
        /// </summary>
        public int OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        /// <summary>
        /// 是否添加查询字段的 DISTINCT 默认0不添加/1添加
        /// </summary>
        public int DistClause
        {
            get { return _distClause; }
            set { _distClause = value; }
        }

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
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
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
        /// 生成缓存Key
        /// </summary>
        /// <returns></returns>
        public string GetCacheKey()
        {
            const string keyFormat = "Sqler-SC:{0}-FC:{1}-WC:{2}-GC:{3}-SC:{4}||Pager-CP:{5}-PS:{6}-PC:{7}-RC:{8}";
            return string.Format(keyFormat, SelectClause, FromClause, WhereClause, GroupClause, SortClause, CurrentPage, PageSize, PageCount, RecordCount);
        }

        /// <summary>
        /// 分页查询数据记录总数获取
        /// </summary>
        /// <param name="FromClause">----要显示的表或多个表的连接</param>
        /// <param name="IDClause">----主表的主键</param>
        /// <param name="WhereClause">----查询条件,不需where</param>        
        /// <param name="DistClause">----是否添加查询字段的 DISTINCT 默认0不添加/1添加</param>
        /// <returns></returns>
        protected string getPageListCounts()
        {
            //---存放取得查询结果总数的查询语句                    
            //---对含有DISTINCT的查询进行SQL构造
            //---对含有DISTINCT的总数查询进行SQL构造

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(" from {0}", FromClause);
            if (WhereClause.Length > 0)
                sb.AppendFormat(" where 1=1 {0}", WhereClause);

            if (!string.IsNullOrEmpty(GroupClause))
                sb.AppendFormat(" group by {0}", GroupClause);

            //return string.Format("Select count(0) {0}", sb);
            if (DistClause == 0)
            {
                return string.Format("Select count(0) {0}", sb);
            }
            else
            {
                return string.Format("Select DISTINCT count(DISTINCT {1}) {0}", sb, IDClause);
            }
        }


        protected string getPageListSql()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(SelectClause))
                SelectClause = "*";

            if (string.IsNullOrEmpty(SortClause))
                SortClause = IDClause;

            int start_row_num = (CurrentPage - 1) * PageSize + 1;

            sb.AppendFormat(" from {0}", FromClause);
            if (WhereClause.Length > 0)
                sb.AppendFormat(" where 1=1 {0}", WhereClause);

            if (!string.IsNullOrEmpty(GroupClause))
                sb.AppendFormat(" group by {0}", GroupClause);

            //strSql.Append("SELECT * FROM CustomerServCenter ");
            //if (!string.IsNullOrEmpty(strWhere.Trim()))
            //{
            //    strSql.Append(" WHERE " + strWhere);
            //}
            //if (!string.IsNullOrEmpty(orderby.Trim()))
            //{
            //    strSql.Append(" order by " + orderby);
            //}
            //else
            //{
            //    strSql.Append(" order by CSCMainID desc");
            //}
            //int sIndex = startIndex > 0 ? startIndex - 1 : startIndex;
            //strSql.AppendFormat("  limit {0} , {1}", sIndex, 15);

            return
              string.Format("SELECT {1} {2} order by {0} {5} limit {3} , {4}",
                //"WITH t AS (SELECT ROW_NUMBER() OVER(ORDER BY {0} {5}) as rownumber,{1}{2}) Select * from t where rownumber BETWEEN {3} and {4}",
                SortClause, SelectClause, sb, start_row_num, (start_row_num + PageSize - 1), OrderType == 0 ? "ASC" : "DESC");
            //return
            //  string.Format(
            //    "WITH t AS (SELECT ROW_NUMBER() OVER(ORDER BY {0} {5}) as rownumber,{1}{2}) Select * from t where rownumber BETWEEN {3} and {4}",
            //    SortClause, SelectClause, sb, start_row_num, (start_row_num + PageSize - 1), OrderType == 0 ? "ASC" : "DESC");

        }
    }

}
