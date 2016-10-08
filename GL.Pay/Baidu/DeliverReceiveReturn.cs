using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.Baidu
{
    public class DeliverReceiveReturn
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public int AppID { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public int ResultCode { get; set; }
        /// <summary>
        /// 返回值描述
        /// </summary>
        public string ResultMsg { get; set; }
        /// <summary>
        /// 签名值
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Content { get; set; }

    }
}
