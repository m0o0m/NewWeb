using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.YeePay
{
    ///
    ///一键支付API返回结果对象
    ///
    public class RespondJson
    {
        /// <summary>
        ///加密的响应结果
        /// </summary>
        public string data;

        /// <summary>
        /// 加密的密文
        /// </summary>
        public string encryptkey;
    }
}
