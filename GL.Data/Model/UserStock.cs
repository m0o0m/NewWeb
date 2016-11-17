using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class UserStock
    {
        /// <summary>
        /// 组ID
        /// </summary>
        public int GroupID { get; set; }
        /// <summary>
        ///  组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 组ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 上次的库存值
        /// </summary>
        public string LastValue { get; set; }

        /// <summary>
        /// 库存值
        /// </summary>
        public string Value { get; set; }

    }
}
