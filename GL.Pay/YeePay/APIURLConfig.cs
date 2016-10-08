using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.YeePay
{
    public class APIURLConfig
    {
        static APIURLConfig()
        {
#if Debug
            //一键支付移动终端网页收银台前缀
            payMobilePrefix = "http://mobiletest.yeepay.com/paymobile";//测试环境

            //商户通用接口前缀
            merchantPrefix = "http://mobiletest.yeepay.com/merchant";//测试环境
#endif

#if P17
            //一键支付移动终端网页收银台前缀
            payMobilePrefix = "http://mobiletest.yeepay.com/paymobile";//测试环境

            //商户通用接口前缀
            merchantPrefix = "http://mobiletest.yeepay.com/merchant";//测试环境
#endif
#if Test
            //一键支付移动终端网页收银台前缀
            payMobilePrefix = "https://ok.yeepay.com/paymobile";//生产环境

            //商户通用接口前缀
            merchantPrefix = "https://ok.yeepay.com/merchant";//生产环境
#endif
#if Release
            //一键支付移动终端网页收银台前缀
            payMobilePrefix = "https://ok.yeepay.com/paymobile";//生产环境

            //商户通用接口前缀
            merchantPrefix = "https://ok.yeepay.com/merchant";//生产环境
#endif
            //移动终端网页收银台支付地址
            webpayURI = "/api/pay/request";

            //支付结果查询接口
            queryPayResultURI = "/api/query/order";

            //直接退款
            directFundURI = "/query_server/direct_refund";

            //交易记录查询
            queryOrderURI = "/query_server/pay_single";

            //退款订单查询
            queryRefundURI = "/query_server/refund_single";

            //获取消费清算对账单
            clearPayDataURI = "/query_server/pay_clear_data";

            //获取退款清算对账单
            clearRefundDataURI = "/query_server/refund_clear_data";


        }

        public static string payMobilePrefix
        { get; set; }

        public static string merchantPrefix
        { get; set; }

        public static string webpayURI
        { get; set; }

        public static string queryPayResultURI
        { get; set; }

        public static string directFundURI
        { get; set; }

        public static string queryOrderURI
        { get; set; }

        public static string queryRefundURI
        { get; set; }

        public static string clearPayDataURI
        { get; set; }

        public static string clearRefundDataURI
        { get; set; }
    }
}
