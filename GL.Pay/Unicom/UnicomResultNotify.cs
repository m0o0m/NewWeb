using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GL.Pay.Unicom
{
    public class UnicomResultNotify : UnicomPay
    {
        public UnicomResultNotify(Controller page):base(page)
        {
        }

        public override UnicomPayData ProcessNotify()
        {
            UnicomPayData notifyData = GetNotifyData();

            if (notifyData.GetValue("return_code").ToString() == "FAIL")
            {
                return notifyData;
            }

            //检查支付结果中transaction_id是否存在
            //if (!notifyData.IsSet("hRet"))
            //{
            //    //若transaction_id不存在，则立即返回结果给微信支付后台
            //    UnicomPayData res = new UnicomPayData();
            //    res.SetValue("return_code", "FAIL");
            //    res.SetValue("return_msg", "支付结果中微信订单号不存在");
            //    Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
            //    return res;
            //}

            //string transaction_id = notifyData.GetValue("transaction_id").ToString();

            return notifyData;
        }
        public override UnicomPayData CheckNotify()
        {
            Log.Info("CheckNotifyData():", "before");
            UnicomPayData notifyData = CheckNotifyData();
            Log.Info("CheckNotifyData():after return_code", notifyData.GetValue("return_code").ToString());
            if (notifyData.GetValue("return_code").ToString() == "FAIL")
            {
                return notifyData;
            }

            return notifyData;
        }

    }
}
