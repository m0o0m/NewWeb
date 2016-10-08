using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using LitJson;


namespace GL.Pay.WxPay
{
    public class AppPay
    {

        /// <summary>
        /// openid用于调用统一下单接口
        /// </summary>
        //public string openid { get; set; }

        /// <summary>
        /// access_token用于获取收货地址js函数入口参数
        /// </summary>
        //public string access_token { get; set; }

        /// <summary>
        /// 商品描述，用于统一下单 String(32)
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }

        /// <summary>
        /// 附加数据 非必填 String(127)
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 商品金额，用于统一下单
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayData unifiedOrderResult { get; set; }

        public AppPay()
        {
        }

        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult()
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", body);
            data.SetValue("attach", attach);
            data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());
            data.SetValue("total_fee", total_fee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", "XXX");
            data.SetValue("trade_type", "APP");
            data.SetValue("spbill_create_ip", spbill_create_ip);

            
            //data.SetValue("openid", openid);

            WxPayData result = WxPayApi.UnifiedOrder2(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }

            unifiedOrderResult = result;
            return result;
        }

    }
}
