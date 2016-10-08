using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.YYH
{
    public class TransData
    {
        /// <summary>
        /// 外部订单号
        /// </summary>
        public string exorderno { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string transid { get; set; }
        /// <summary>
        /// 支付编号
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int waresid { get; set; }
        /// <summary>
        /// 计费方式
        /// </summary>
        public int feetype { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public int money { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 交易结果 0成功   1失败
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public int transtype { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string transtime { get; set; }
        /// <summary>
        /// 私有密匙
        /// </summary>
        public string cpprivate { get; set; }
    }
}
