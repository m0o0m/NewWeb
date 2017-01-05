using GL.Common;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Pay.AliPay;
using GL.Pay.QQPay;
using GL.Pay.WxPay;
using GL.Pay.YeePay;
using GL.Protocol;
using log4net;
using ProtoCmd.BackRecharge;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Pay.JsonMapper;
using Web.Pay.Models;
using Web.Pay.protobuf.SCmd;
using System.Collections;
using GL.Pay.Baidu;
using Newtonsoft.Json;
using GL.Pay.Unicom;
using GL.Pay.XY;
using GL.Pay.Hippocampi;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Data;
using GL.Pay.ZY;
using GL.Pay.YYH;
using System.Web.Script.Serialization;
using GL.Pay.MeiZu;
using GL.Pay.HuaWei;


namespace Web.Pay.Controllers
{
    public class CallbackController : Controller
    {
        private ILog log = LogManager.GetLogger("Callback");

        [QueryValues]
        //[HttpGet]
        public ActionResult YeePay(Dictionary<string, string> queryvalues)
        {
            log.Info("##########################YeePay易宝回调函数###################");
            log.Info("YeePay易宝回调 queryvalues: " + JsonConvert.SerializeObject(queryvalues));

            string _data = queryvalues.ContainsKey("data") ? queryvalues["data"] : string.Empty;
            string _encryptkey = queryvalues.ContainsKey("encryptkey") ? queryvalues["encryptkey"] : string.Empty;

            if (_data == string.Empty || _encryptkey == string.Empty)
            {
                log.Error(this.GetType().ToString() + " 参数不正确 " + Utils.GetUrl());
                Response.Redirect("mobilecall://fail");
                return Content("参数不正确!");
            }

            try
            {
                //商户注意：接收到易宝的回调信息后一定要回写success用以保证握手成功！
                //Response.Write("success");

                YeepayCallback model = new YeepayCallback();
                model.Data = _data;
                model.EncryptKey = _encryptkey;
                model.CallBackResult = YJPayUtil.checkYbCallbackResult(_data, _encryptkey);//解密易宝支付回调结果



                JsonToInstance util = new JsonToInstance();
                YeepayCallbackReslut m = util.ToInstance<YeepayCallbackReslut>(model.CallBackResult);





#if P17
                RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = m.orderid });
                Role user = RoleBLL.GetModelByID(new Role{ ID = rc.UserID});
                IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;

                bool firstGif = iF == isFirst.是;
                uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                if (firstGif)
                {
                    gold = (uint)(iap.goods + iap.attach_chip);
                    dia = (uint)iap.attach_5b;
                }
                   string list = iap.attach_props;
                list = list.Trim(',') +(gold > 0 ? ",10000|"+gold+"," : "");
                list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                list = list.Trim(',') + ",";
                uint rmb = (uint)(rc.Money / 100);
                normal ServiceNormalS = normal.CreateBuilder()
                .SetUserID((uint)rc.UserID)
                .SetList(list)
                .SetRmb(rmb)
                .SetFirstGif(firstGif)
                .Build();

                Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                switch ((CenterCmd)tbind.header.CommandID)
                {
                    case CenterCmd.CS_NORMAL:
                        normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                        if (ServiceNormalC.Suc)
                        {

                            RechargeBLL.Add(new Recharge { BillNo = m.yborderid, OpenID = rc.SerialNo, UserID = rc.UserID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.易宝, UserAccount = user.NickName });
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = m.orderid });
                            Response.Redirect("mobilecall://success");
                            return Content("success");
                        }
                        Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                        break;
                    case CenterCmd.CS_CONNECT_ERROR:
                        break;
                }

                Response.Redirect("mobilecall://fail");
                return Content("支付失败!");
#endif


                if (m.status == 1)
                {
                    log.Info(this.GetType().ToString() + " success ");
                    return RedirectToAction("success", "Home", new { orderid = m.orderid, yborderid = m.yborderid });
                }
                log.Error(this.GetType().ToString() + " fail " + Utils.GetUrl());
                return RedirectToAction("fail", "Home", new { orderid = m.orderid, yborderid = m.yborderid });
            }
            catch (Exception err)
            {
                //Response.Redirect("mobilecall://fail?err=" + err);
                log.Error(this.GetType().ToString() + " 支付失败 " + Utils.GetUrl(), err);
                return RedirectToAction("fail", "Home", new { orderid = "-1", yborderid = "-1" });
                //return Content("支付失败!" + err);
            }
        }
        //[HttpPost]
        [QueryValues]
        public ActionResult fYeePay(Dictionary<string, string> queryvalues)
        {
            log.Info("###################fYeePay易宝回调#####################");

            log.Info("fYeePay易宝回调 queryvalues: " + JsonConvert.SerializeObject(queryvalues));

            string _data = queryvalues.ContainsKey("data") ? queryvalues["data"] : string.Empty;
            string _encryptkey = queryvalues.ContainsKey("encryptkey") ? queryvalues["encryptkey"] : string.Empty;

            if (_data == string.Empty || _encryptkey == string.Empty)
            {
                log.Error("yeepay 参数不正确 : q " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
                Response.Redirect("mobilecall://fail");
                return Content("fail");
            }

            try
            {

                YeepayCallback model = new YeepayCallback();
                model.Data = _data;
                model.EncryptKey = _encryptkey;
                model.CallBackResult = YJPayUtil.checkYbCallbackResult(_data, _encryptkey);//解密易宝支付回调结果

                JsonToInstance util = new JsonToInstance();
                YeepayCallbackReslut m = util.ToInstance<YeepayCallbackReslut>(model.CallBackResult);




                RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = m.orderid });
                Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                
                IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;



                bool firstGif = iF == isFirst.是;

                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                if (recharge != null)//首冲过
                {
                    if (firstGif == true)
                    {//又是首冲，则改变其为不首冲
                        firstGif = false;
                        ct = (chipType)1;
                        iF = isFirst.否;
                        iap.goodsType = 1;
                        log.Error(" 第多次首冲，修改firstGif值");
                    }
                }



                RechargeIP ipmodel = GetClientIp();
                RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.易宝 });



                uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                if (firstGif)
                {
                    gold = (uint)(iap.goods + iap.attach_chip);
                    dia = (uint)iap.attach_5b;
                }
                string list = iap.attach_props;
                list = list.Trim(',') +(gold > 0 ? ",10000|"+gold+"," : "");
                list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                list = list.Trim(',') + ",";
                uint rmb = (uint)rc.Money;
                log.Error("fYeePay易宝回调 传给服务器的钱：" + rmb);

                //解决延迟导致游戏服务器发货并且补单了
                RechargeBLL.Add(new Recharge { BillNo = m.yborderid, OpenID = rc.SerialNo, UserID = rc.UserID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.易宝, UserAccount = user.NickName, ActualMoney= Convert.ToInt64(iap.price * 100), ProductNO=list.Trim(','), AgentID=rc.AgentID });

                normal ServiceNormalS = normal.CreateBuilder()
                .SetUserID((uint)rc.UserID)
                .SetList(list)
                .SetRmb(rmb)
                .SetRmbActual((uint)iap.price * 100)
                .SetFirstGif(firstGif)
                .SetBillNo(m.yborderid)
                .Build();

                Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                switch ((CenterCmd)tbind.header.CommandID)
                {
                    case CenterCmd.CS_NORMAL:
                        normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                        if (ServiceNormalC.Suc)
                        {
                            //true 正常发货  false 补单
                            RechargeBLL.UpdateReachTime(m.yborderid);
                        }
                        log.Info(" fYeePay易宝回调 ServiceResult[" + m.yborderid + "] 服务器发货: " + ServiceNormalC.Suc);
                        RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = m.orderid });
                        return Content("success");
                      
                    case CenterCmd.CS_CONNECT_ERROR:
                        log.Info(" fYeePay易宝回调 ServiceResult =CS_CONNECT_ERROR");
                        break;
                      
                }

                // RechargeBLL.Delete(new Recharge { BillNo = m.yborderid, OpenID = rc.SerialNo, UserID = rc.UserID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.易宝, UserAccount = user.NickName });
                RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = m.orderid });
                return Content("fail");

                //Response.Redirect("mobilecall://fail");
                //log.Error("yeepay 支付失败 : q " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
                //return Content("fail");








            }
            catch (Exception err)
            {
                //Response.Redirect("mobilecall://fail?err=" + err);
                log.Error("yeepay 支付失败 : q " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
                return Content("fail " + err);
            }
        }

        [QueryValues]
        [HttpPost]
        public ActionResult ResultNotifyPageForWxPay(Dictionary<string, string> queryvalues)
        {
            log.Info("###################ResultNotifyPageForWxPay微信回调#####################");

            log.Info("ResultNotifyPageForWxPay微信回调 queryvalues: " + JsonConvert.SerializeObject(queryvalues));

       

            MvcResultNotify resultNotify = new MvcResultNotify(this);
            WxPayData notifyData = resultNotify.ProcessNotify();

            if (!notifyData.IsSet("transaction_id"))
            {
                return Content(notifyData.ToXml(), "text/xml");
            }
            else
            {
                WxPayData res = new WxPayData();
                try
                {
                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = notifyData.GetValue("out_trade_no").ToString() });
                    if (rc == null)
                    {
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", string.Format("订单[{0}]不存在", notifyData.GetValue("out_trade_no")));
                        log.Error(Utils.GetUrl() + " WxPay fail : " + res.ToXml());
                        return Content(res.ToXml(), "text/xml");
                    }
                    Recharge rech = RechargeBLL.GetModelByBillNo(new Recharge { BillNo = notifyData.GetValue("transaction_id").ToString() });
                    if (rech != null)
                    {//说明此流水已经存在
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", string.Format("流水号[{0}]重复", notifyData.GetValue("transaction_id")));
                        log.Error(Utils.GetUrl() + " WxPay fail : " + res.ToXml());
                        return Content(res.ToXml(), "text/xml");
                    }







                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;

                    bool firstGif = iF == isFirst.是;


                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }
                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.微信 });

                  
                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    log.Error("gold" + gold);
                    log.Error("dia" + dia);
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";
                    uint rmb = (uint)rc.Money;
                    log.Error("ResultNotifyPageForWxPay微信回调给游戏服务器传钱：" + rmb);
                    log.Error("transaction_id"+notifyData.GetValue("transaction_id"));
                    log.Error("rc.SerialNo"+rc.SerialNo);
                    log.Error("rc.UserID" + rc.UserID);
                    log.Error("Money" + rc.Money);
                    log.Error("gold" + gold);
                    log.Error("dia" + dia);
                    log.Error("ct" + ct);
                    log.Error("iF" + iF);
                    log.Error("iap.productname" + iap.productname);
                    log.Error("iap.product_id" + iap.product_id);
                    log.Error("user.NickName" + user.NickName);
                    log.Error("iap.price*100" + iap.price * 100);
                    log.Error("list" + list); 
                    RechargeBLL.Add(new Recharge { BillNo = notifyData.GetValue("transaction_id").ToString(), OpenID = rc.SerialNo, UserID = rc.UserID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.微信, UserAccount = user.NickName, ActualMoney= Convert.ToInt64(iap.price*100), ProductNO=list.Trim(','), AgentID=rc.AgentID });
                    normal ServiceNormalS = normal.CreateBuilder()
                    .SetUserID((uint)rc.UserID)
                    .SetList(list)
                    .SetRmb(rmb)
                    .SetRmbActual(rmb)
                    .SetFirstGif(firstGif)
                    .SetBillNo(notifyData.GetValue("transaction_id").ToString())
                    .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info("ResultNotifyPageForWxPay微信回调 " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                RechargeBLL.UpdateReachTime(notifyData.GetValue("transaction_id").ToString());
                                log.Info(" ResultNotifyPageForWxPay微信回调 ServiceResult[" + notifyData.GetValue("transaction_id").ToString() + "] 服务器发货: " + ServiceNormalC.Suc);
                            }
                          
                            res.SetValue("return_code", "SUCCESS");
                            res.SetValue("return_msg", "OK");
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });

                            return Content(res.ToXml(), "text/xml");
                        case CenterCmd.CS_CONNECT_ERROR:
                            log.Info(" ResultNotifyPageForWxPay微信回调 ServiceResult =CS_CONNECT_ERROR");
                            break;
                    }
                    RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });


                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "链接服务器失败");
                    return Content(res.ToXml(), "text/xml");






                }
                catch (Exception err)
                {
                    //Response.Redirect("mobilecall://fail?err=" + err);
                    //return Content("支付失败!" + err);
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "服务器发货失败 : " + err.Message);
                    log.Error(" WxPay fail : " + res.ToXml(), err);
                    return Content(res.ToXml(), "text/xml");
                }


            }

        }


        [QueryValues]
        public ActionResult ResultNotifyPageForAliPay(Dictionary<string, string> queryvalues, SortedDictionary<string, string> sPara)
        {
            log.Info("###################ResultNotifyPageForAliPay易宝回调#####################");
            log.Info("ResultNotifyPageForAliPay易宝回调 queryvalues: " + JsonConvert.SerializeObject(queryvalues));
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                string _notify_id = sPara.ContainsKey("notify_id") ? sPara["notify_id"] : string.Empty;
                string _sign = sPara.ContainsKey("sign") ? sPara["sign"] : string.Empty;

                AliPayNotify aliNotify = new AliPayNotify();
                bool verifyResult = aliNotify.Verify(sPara, _notify_id, _sign);

                if (true)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码
                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    if (sPara["trade_status"] == "WAIT_BUYER_PAY")
                    {
                        log.Error(" AliPay : trade_status[" + sPara["trade_status"] + "] [" + sPara["trade_no"] + "] ");
                        return Content("success");
                    }
                    else if (sPara["trade_status"] == "TRADE_FINISHED")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                        //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                    }
                    else if (sPara["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //付款完成后，支付宝系统发送该交易状态通知
                        //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的

                        // http://pay.515.com:80/Callback/ResultNotifyPageForAliPay?

                        //discount =0.00
                        //payment_type =1
                        //subject =515%e6%b8%b8%e6%88%8f-6480%e4%ba%94%e5%b8%81
                        //trade_no =2016010121001004240002375476
                        //buyer_email=13452000008
                        //gmt_create=2016-01-01+06%3a09%3a25
                        //notify_type=trade_status_sync
                        //quantity=1
                        //out_trade_no=Alipay20160101060812439
                        //seller_id=2088121009887361
                        //notify_time=2016-01-01+06%3a23%3a25
                        //body=515%e6%b8%b8%e6%88%8f-6480%e4%ba%94%e5%b8%81
                        //trade_status=TRADE_SUCCESS
                        //is_total_fee_adjust=N
                        //total_fee=648.00
                        //gmt_payment=2016-01-01+06%3a09%3a26
                        //seller_email=webmaster%40515.com
                        //price=648.00
                        //buyer_id=2088802168419243
                        //notify_id=b9556cfca00092041255f67d213f8b8huo
                        //use_coupon=N
                        //sign_type=RSA
                        //sign=TqO8zPu7UGhbJzilkfZ2Vxn0TbQzGujko62YjpYelQnMOOMZrTJ9UStElNrvmN8XI2Q9rQjpIrW9SleRzI3iy6xCkluW9NQlXNmuDroMuknOV1unrywK6M2Aerd207FSljiObd5cV78y94Hf4tFCCp%2fD7o9aatdudR2n1bqm1d0%3d

                        //[body, 515游戏-1游戏币]
                        //[buyer_email, blue0ban@163.com]
                        //[buyer_id, 2088002341861142]
                        //[discount, 0.00]
                        //[gmt_create, 2015-11-14 14:51:42]
                        //[is_total_fee_adjust, Y]
                        //[notify_id, 77cbabd63b89524a7aab39753df09d622s]
                        //[notify_time, 2015-11-14 14:55:35]
                        //[notify_type, trade_status_sync]
                        //[out_trade_no, Alipay20151114145130442]
                        //[payment_type, 1]
                        //[price, 0.01]
                        //[quantity, 1]
                        //[seller_email, webmaster@515.com]
                        //[seller_id, 2088121009887361]
                        //[sign, XttMw2dh6ZjNh/YLlOuEDGT6mwIiICN5uEJ00H9dDpgiL4wpHi0JULtNEwO5alvXxpmT5QF9naCNK0DGY3xPK3Cs+0lGn1c/HpD7+5BfoijkVKADU1njrsBdG1IPfktZ12okYG1yl5oTqmiYW9TGf/hQ8cmWjOO/vN6iAdSzVqI=]
                        //[sign_type, RSA]
                        //[subject, 515游戏-1游戏币]
                        //[total_fee, 0.01]
                        //[trade_no, 2015111400001000140062745282]
                        //[trade_status, WAIT_BUYER_PAY]
                        //[use_coupon, N]

                        try
                        {

                            //商户订单号                
                            string out_trade_no = queryvalues.ContainsKey("out_trade_no") ? queryvalues["out_trade_no"] : string.Empty;
                            //支付宝交易号                
                            string trade_no = queryvalues.ContainsKey("trade_no") ? queryvalues["trade_no"] : string.Empty;

                            //交易状态
                            string trade_status = queryvalues.ContainsKey("trade_status") ? queryvalues["trade_status"] : string.Empty;

                            RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = out_trade_no });

                            if (rc == null)
                            {
                                log.Error(" ResultNotifyPageForAliPay易宝回调 订单[" + out_trade_no + "]不存在");
                                return Content("fail");
                            }
                            Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                            if (user == null)
                            {
                                log.Error(" ResultNotifyPageForAliPay易宝回调 用户[" + rc.UserID + "]不存在");
                                return Content("fail");
                            }
                            IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                            if (iap == null)
                            {
                                log.Error(" ResultNotifyPageForAliPay易宝回调 支付配表出错");
                                return Content("fail");
                            }
                            Recharge rech = RechargeBLL.GetModelByBillNo(new Recharge { BillNo = trade_no });
                            if (rech != null)
                            {//说明此流水已经存在
                                log.Error(" ResultNotifyPageForAliPay易宝回调 此流水已经存在");
                                return Content("fail");
                            }



                            isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                            chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;

                            bool firstGif = iF == isFirst.是;


                            Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                            if (recharge != null)//首冲过
                            {
                                if (firstGif == true)
                                {//又是首冲，则改变其为不首冲
                                    firstGif = false;
                                    ct = (chipType)1;
                                    iF = isFirst.否;
                                    iap.goodsType = 1;
                                    log.Error(" 第多次首冲，修改firstGif值");
                                }
                            }


                            RechargeIP ipmodel = GetClientIp();
                            RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.支付宝 });



                            uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                            uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                            if (firstGif)
                            {
                                gold = (uint)(iap.goods + iap.attach_chip);
                                dia = (uint)iap.attach_5b;
                            }
                            string list = iap.attach_props;
                            list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                            list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                            list = list.Trim(',') + ",";
                            uint rmb = (uint)rc.Money;
                            log.Error("ResultNotifyPageForAliPay易宝回调 充钱(分)：" + rmb);
                            RechargeBLL.Add(new Recharge { BillNo = trade_no, OpenID = rc.SerialNo, UserID = rc.UserID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.支付宝, UserAccount = user.NickName , ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','), AgentID=rc.AgentID });


                            log.Info("ResultNotifyPageForAliPay易宝回调 订单号=" + trade_no + ",用户ID=" + rc.UserID);

                            normal ServiceNormalS = normal.CreateBuilder()
                            .SetUserID((uint)user.ID)
                            .SetList(list)
                            .SetRmb(rmb)
                             .SetRmbActual((uint)iap.price * 100)
                            .SetFirstGif(firstGif)
                            .SetBillNo(trade_no)
                            .Build();

                            Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                            switch ((CenterCmd)tbind.header.CommandID)
                            {
                                case CenterCmd.CS_NORMAL:
                                    normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                                    log.Info(" ResultNotifyPageForAliPay易宝回调 ServiceResult : " + ServiceNormalC.Suc);
                                    if (ServiceNormalC.Suc)
                                    {
                                        RechargeBLL.UpdateReachTime(trade_no);
                                        log.Info(" ResultNotifyPageForAliPay易宝回调 ServiceResult [" + trade_no + "] : " + ServiceNormalC.Suc);
                                    }
                                    RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                                 
                                    return Content("success");

                                case CenterCmd.CS_CONNECT_ERROR:
                                    log.Info(" ResultNotifyPageForAliPay易宝回调 ServiceResult=CS_CONNECT_ERROR ");
                                    break;
                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });



                            //log.Error(" AliPay fail : "+ (CenterCmd)tbind.header.CommandID  + " q " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
                            //return Content("fail");



                            log.Info("fail");

                            return Content("fail");



                        }
                        catch (Exception err)
                        {
                            log.Error("ResultNotifyPageForAliPay易宝回调 fail : q " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8), err);
                            return Content("fail");
                        }


                    }
                    else
                    {
                    }

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    //Response.Write("success");  //请不要修改或删除
                    log.Error(" ResultNotifyPageForAliPay易宝回调 fail : trade_status[" + sPara["trade_status"] + "] " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
                    return Content("fail");
                    //log.Info(" AliPay success");
                    //return Content("success");

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else
                {
                    //Response.Write("fail");
                    log.Error(" AliPay 签名验证失败 : q " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
                    return Content("fail");
                }
            }
            else
            {
                log.Error("AliPay 无通知参数 : " + Utils.GetUrl());
                return Content("无通知参数");
                //Response.Write("无通知参数");
            }

        }


        [QueryValues]
        public ActionResult ResultNotifyPageForQQ(Dictionary<string, string> queryvalues)
        {
            log.Info("###################ResultNotifyPageForQQ腾讯回调#####################");
            log.Info(" ResultNotifyPageForQQ腾讯回调 Url : " + Utils.GetUrl());

            if (queryvalues.Count > 0)//判断是否有带返回参数
            {

                string method = Request.HttpMethod;
                string url = Utils.GetUrlPath();
                string _sign = queryvalues.ContainsKey("sig") ? queryvalues["sig"] : string.Empty;
                string appid = queryvalues.ContainsKey("appid") ? queryvalues["appid"] : string.Empty;

                bool verifyResult = SnsSigCheck.VerifySig(method, url, queryvalues, GL.Pay.QQPay.Config.GetAppKey(appid) + "&", _sign);

                log.Info(verifyResult);
                if (verifyResult)//验证成功
                {


                    try
                    {

                        ////  http://10.105.2.182:9001/callback/ResultNotifyPageForQQ?amt=1800&appid=1103881749&billno=-APPDJ39416-20160411-0154278078&ibazinga=1&openid=4DE1B1F06382DEEF254532E11EC8DAA0&payamt_coins=0&paychannel=qdqb&paychannelsubid=1&payitem=QQPay20160411015304776*180*1&providetype=0&pubacct_payamt_coins=&token=30CDCF76F7FE3A77D826FE4C229D6AD715090&ts=1460310868&version=v3&zoneid=0&sig=Owx%2F2d8L2wPQFmHgNwBplAn52Cg%3D


                        ////http://10.105.2.182:9001/callback/ResultNotifyPageForQQ?amt=3000&appid=1104965754&billno=-APPDJSX36116-20151114-1625174273&hbazinga=1&openid=A1BAE5D590ACCAC6DCD61063A9387637&payamt_coins=0&paychannel=qqpoint&paychannelsubid=1&payitem=Chip_3*300*1&providetype=0&pubacct_payamt_coins=&token=C105AEC26568412C9BE594DDC231506328390&ts=1447489517&version=v3&zoneid=0&sig=Z7G0X124eXU4CHPrFtmvXHwJtCM%3D


                        string openid = queryvalues.ContainsKey("openid") ? queryvalues["openid"] : string.Empty;
                        //string appid = queryvalues.ContainsKey("appid") ? queryvalues["appid"] : string.Empty;
                        //string ts = queryvalues.ContainsKey("ts") ? queryvalues["ts"] : string.Empty;
                        string payitem = queryvalues.ContainsKey("payitem") ? queryvalues["payitem"] : string.Empty;
                        //string token = queryvalues.ContainsKey("token") ? queryvalues["token"] : string.Empty;
                        string billno = queryvalues.ContainsKey("billno") ? queryvalues["billno"] : string.Empty;
                        //string version = queryvalues.ContainsKey("version") ? queryvalues["version"] : string.Empty;
                        //string zoneid = queryvalues.ContainsKey("zoneid") ? queryvalues["zoneid"] : string.Empty;
                        //string providetype = queryvalues.ContainsKey("providetype") ? queryvalues["providetype"] : string.Empty;
                        uint amt = queryvalues.ContainsKey("amt") ? Convert.ToUInt32(queryvalues["amt"]) : 0;
                        string payamt_coins = queryvalues.ContainsKey("payamt_coins") ? queryvalues["payamt_coins"] : string.Empty;
                        string pubacct_payamt_coins = queryvalues.ContainsKey("pubacct_payamt_coins") ? queryvalues["pubacct_payamt_coins"] : string.Empty;

                        ///50001 游戏币
                        ///50002 五币
                        ///50003 首冲
                        /// 

                        string tradeno = payitem.Split('*')[0];
                        uint price = Convert.ToUInt32(payitem.Split('*')[1]);
                        uint num = Convert.ToUInt32(payitem.Split('*')[2]);


                        RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = tradeno });
                        if (rc == null)
                        {
                            log.Error(" ResultNotifyPageForQQ腾讯回调 订单[" + tradeno + "]不存在");
                            return Content("fail");
                        }

                        Role user = RoleBLL.GetModelByOpenID(new Role { OpenID = openid });
                        if (user == null)
                        {
                            log.Error(" ResultNotifyPageForQQ腾讯回调 OpenID用户不存在" + Utils.GetUrl());
                            return Json(new
                            {
                                ret = 4,
                                msg = "OpenID用户不存在"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                        if (iap == null)
                        {
                            log.Error(" ResultNotifyPageForQQ腾讯回调 支付配表出错" + Utils.GetUrl());
                            return Json(new
                            {
                                ret = 4,
                                msg = "支付配表出错"
                            }, JsonRequestBehavior.AllowGet);
                        }






                        isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                        chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;



                        bool firstGif = iF == isFirst.是;

                        Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                        if (recharge != null)//首冲过
                        {
                            if (firstGif == true)
                            {//又是首冲，则改变其为不首冲
                                firstGif = false;
                                ct = (chipType)1;
                                iF = isFirst.否;
                                iap.goodsType = 1;
                                log.Error(" 第多次首冲，修改firstGif值");
                            }
                        }
                        log.Info("插入数据");
                      RechargeIP ipmodel = GetClientIp();
                        RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.腾讯 });

                        log.Info(" ResultNotifyPageForQQ腾讯回调 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);




                        uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                        uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                        uint rmb = (uint)(iap.price * 100);
                        if (firstGif)
                        {
                            gold = (uint)(iap.goods + iap.attach_chip);
                            dia = (uint)iap.attach_5b;
                        }
                        if (num > 1)
                        {
                            gold = gold * (uint)num;
                            dia = dia * (uint)num;
                            rmb = rmb * (uint)num;
                        }
                        string list = "";
                        if (num > 1)
                        {
                            log.Info(iap.attach_props);

                            string attach_props = iap.attach_props;
                            if (!string.IsNullOrEmpty(attach_props))
                            {
                                string[] attachs = attach_props.Trim(',').Split(',');
                                for (int i = 0; i < attachs.Length; i++) {
                                    string att = attachs[i];
                                    string[] attKeyValue = att.Split('|');
                                    string key = attKeyValue[0];
                                    int value = Convert.ToInt32(attKeyValue[1]);
                                    string temp = key + "|" + value * num;
                                    list = list + temp+",";
                                }
                            }
                            else {
                                list = "";
                            }
                        }
                        else {
                            list = iap.attach_props;
                        }

                        log.Info(list);

                        list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                        list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                        list = list.Trim(',') + ",";
                        //传给波总要传打折之后的数据amt

                        log.Error("ResultNotifyPageForQQ腾讯回调rmb：" + rmb);
                        log.Error("ResultNotifyPageForQQ腾讯回调amt：" + amt);
                        uint oldRmb = rmb;
                        if (amt < rmb)
                        {
                            rmb = amt;
                        }
                        log.Error("ResultNotifyPageForQQ腾讯回调rmb：" + rmb);

                        RechargeBLL.Add(new Recharge { Num= (int)num,BillNo = billno, OpenID = openid, UserID = user.ID, Money = (long)rmb, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.腾讯, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(oldRmb), ProductNO = list.Trim(','),AgentID=rc.AgentID });

                        normal ServiceNormalS = normal.CreateBuilder()
                        .SetUserID((uint)user.ID)
                        .SetList(list)
                        .SetRmb(rmb)   //打折后的总价
                        .SetRmbActual(oldRmb) //没打折的总价
                        .SetFirstGif(firstGif)
                        .SetBillNo(billno)
                        .SetNum(num)
                        .SetUnitPrice(oldRmb/num)   //没打折单价
                        .SetUnitDiscounted(rmb/num)  //打折的单价
                        .Build();

                        Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                        switch ((CenterCmd)tbind.header.CommandID)
                        {
                            case CenterCmd.CS_NORMAL:
                                normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                                log.Info(" ResultNotifyPageForQQ腾讯回调 ServiceResult : " + ServiceNormalC.Suc);
                                if (ServiceNormalC.Suc)
                                {
                                    RechargeBLL.UpdateReachTime(billno);
                                }
                                RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = billno });
                                return Json(new
                                {
                                    ret = 0,
                                    msg = "OK"
                                }, JsonRequestBehavior.AllowGet);
                            case CenterCmd.CS_CONNECT_ERROR:
                                log.Info(" ResultNotifyPageForQQ腾讯回调 ServiceResult =CS_CONNECT_ERROR");
                                break;
                        }
                        RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = billno });
                        log.Info("ResultNotifyPageForQQ腾讯回调 服务器发货失败");
                        return Json(new
                        {
                            ret = 4,
                            msg = "服务器发货失败"
                        }, JsonRequestBehavior.AllowGet);


                    }
                    catch (Exception err)
                    {
                        log.Error(" QQPay 服务器发货失败" + Utils.GetUrl(), err);
                        return Json(new
                        {
                            ret = 4,
                            msg = "服务器发货失败" + err.Message
                        }, JsonRequestBehavior.AllowGet);
                    }


                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    //Response.Write("success");  //请不要修改或删除

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    //Response.Write("fail");
                    log.Error(" QQPay 签名验证失败: " + Utils.GetUrl());
                    return Json(new
                    {
                        ret = 4,
                        msg = "签名验证失败"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                log.Error(" QQPay 无通知参数 : " + Utils.GetUrl());
                return Json(new
                {
                    ret = 4,
                    msg = "无通知参数"
                }, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// 百度支付
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult baiduPay(Dictionary<string, string> queryvalues)
        {
            log.Info("###################baiduPay百度回调#####################");
            log.Info(" baiduPay百度回调 Url : " + Utils.GetUrl());

            //1.获取请求参数 提供两种获取参数方式
            #region 1.Request方式获取请求参数
            //var orderSerial = string.IsNullOrEmpty(Request["OrderSerial"]) ? string.Empty : Request["OrderSerial"];
            //var cooperatorOrderSerial = string.IsNullOrEmpty(Request["CooperatorOrderSerial"]) ? string.Empty : Request["CooperatorOrderSerial"];
            //var content = string.IsNullOrEmpty(Request["Content"]) ? string.Empty : Request["Content"];//Content通过Request读取的数据已经自动解码
            //var sign = string.IsNullOrEmpty(Request["Sign"]) ? string.Empty : Request["Sign"];
            #endregion

            #region 2.读取POST流方式获取请求参数
            var postData = string.Empty;
            using (var br = new System.IO.BinaryReader(Request.InputStream))
            {
                postData = System.Text.Encoding.UTF8.GetString(br.ReadBytes(int.Parse(Request.InputStream.Length.ToString())));
            }
            //解析postData
            Hashtable ht = Utility.ConvertKeyValueStrToHashtable(postData);
            var orderSerial = string.Empty;
            if (ht != null && ht.ContainsKey("OrderSerial"))
                orderSerial = ht["OrderSerial"].ToString();
            var cooperatorOrderSerial = string.Empty;
            if (ht != null && ht.ContainsKey("CooperatorOrderSerial"))
                cooperatorOrderSerial = ht["CooperatorOrderSerial"].ToString();
            var content = string.Empty;
            if (ht != null && ht.ContainsKey("Content"))
                content = HttpUtility.UrlDecode(ht["Content"].ToString());//读取POST流的方式需要进行HttpUtility.UrlDecode解码操作
            var sign = string.Empty;
            if (ht != null && ht.ContainsKey("Sign"))
                sign = ht["Sign"].ToString();
            #endregion

            DeliverReceiveReturn drr = BaiduPayNotify.Verify(orderSerial, cooperatorOrderSerial, content, sign);

            if (drr.ResultCode == 1)
            {

                //3.解析Content 内容  【BASE64解码--》JSON解析】
                var item = JsonConvert.DeserializeAnonymousType(Utility.Base64StringDecode(content), new
                {
                    UID = 0L,
                    MerchandiseName = string.Empty,
                    OrderMoney = 0F,
                    StartDateTime = System.DateTime.Now,
                    BankDateTime = System.DateTime.Now,
                    OrderStatus = 0,
                    StatusMsg = string.Empty,
                    ExtInfo = string.Empty,
                    VoucherMoney = 0 //增加代金券金额获取
                });

                //4.执行业务逻辑处理
                if (item == null)
                {
                    return Json(new
                    {
                        ResultCode = 10002,
                        ResultMsg = "content包校验错误"
                    });
                }
                //业务逻辑代码

                var uid = item.UID;
                var merchandiseName = item.MerchandiseName;
                var orderMoney = item.OrderMoney;
                var startDateTime = item.StartDateTime;
                var bankDateTime = item.BankDateTime;
                var orderStatus = item.OrderStatus;
                var statusMsg = item.StatusMsg;
                var extInfo = item.ExtInfo;
                var voucherMoney = item.VoucherMoney;

                if (orderStatus != 1)
                {
                    return Json(new
                    {
                        ResultCode = 10006,
                        ResultMsg = "  订单[" + orderSerial + "]支付失败"
                    });
                }

                try
                {

                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = cooperatorOrderSerial });

                    if (rc == null)
                    {
                        log.Error(" baiduPay百度回调 订单[" + cooperatorOrderSerial + "]不存在");
                        return Json(new
                        {
                            ResultCode = 10003,
                            ResultMsg = " BaiduPay 订单[" + cooperatorOrderSerial + "]不存在"
                        });
                    }
                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    if (user == null)
                    {
                        log.Error(" baiduPay百度回调 用户[" + rc.UserID + "]不存在");
                        return Json(new
                        {
                            ResultCode = 10005,
                            ResultMsg = " BaiduPay 用户[" + rc.UserID + "]不存在"
                        });
                    }
                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                    if (iap == null)
                    {
                        log.Error(" baiduPay百度回调 支付配表出错");
                        return Json(new
                        {
                            ResultCode = 10007,
                            ResultMsg = " BaiduPay 支付配表出错"
                        });
                    }

                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;

                    bool firstGif = iF == isFirst.是;

                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }

                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.百度 });


                    log.Info(" baiduPay百度回调 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);


                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";
                    uint rmb = (uint)(rc.Money);

                    log.Error("baiduPay百度回调 rmb：" + rmb);
                    RechargeBLL.Add(new Recharge { BillNo = orderSerial, OpenID = rc.SerialNo, UserID = rc.UserID, Money = rmb, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.百度, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });
                    //log.Error("测试分rmb：" + rmb);
                    //log.Error("测试分voucherMoney：" + voucherMoney);
                    //if (voucherMoney < rmb)
                    //{
                    //    rmb = (uint)voucherMoney;
                    //}
                    //log.Error("测试分rmb：" + rmb);

                    normal ServiceNormalS = normal.CreateBuilder()
                    .SetUserID((uint)rc.UserID)
                    .SetList(list)
                    .SetRmb(rmb)
                    .SetRmbActual(rmb)
                    .SetFirstGif(firstGif)
                    .SetBillNo(orderSerial)
                    .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info(" baiduPay百度回调 ServiceResult : " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                log.Info(" baiduPay百度回调 传入的订单号为"+ rc.SerialNo);
                                int ii = RechargeBLL.UpdateReachTime(orderSerial);
                                log.Info(" baiduPay百度回调 ii="+ii);
                                log.Info(" baiduPay百度回调 ServiceResult [" + orderSerial + "]: " + ServiceNormalC.Suc);

                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            return Content(JsonConvert.SerializeObject(drr));
                           
                        case CenterCmd.CS_CONNECT_ERROR:
                            log.Info(" baiduPay百度回调 ServiceResult : CS_CONNECT_ERROR");
                            break;
                    }

                    RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });

                    log.Info(string.Concat(" baiduPay百度回调 Process [", orderSerial, "]: Money price(支付价格): ", iap.price, " voucherMoney(支付金额): ", voucherMoney, " orderMoney(订单金额): ", orderMoney, " rmb(实际花费): ", rmb, " Url ", Utils.GetUrl(), "?", Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8)));

                  
                 
                    return Content(JsonConvert.SerializeObject(drr));

                }
                catch (Exception err)
                {
                    log.Error(" BaiduPay fail : q " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8), err);
                    return Json(new
                    {
                        ResultCode = 10008,
                        ResultMsg = " BaiduPay fail"
                    });
                }




            }
            log.Error(" BaiduPay 校验失败 : " + JsonConvert.SerializeObject(drr));
            return Content(JsonConvert.SerializeObject(drr));
        }

        /// <summary>
        /// 典贷支付
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult diandaiPay(Dictionary<string, string> queryvalues)
        {
            /*
             
记录时间：2016-04-14 11:54:03,562  线程ID:[629]  日志级别：INFO  
错误描述：典贷支付校验notifyData: {"cityid":"","orderid":"UniPay020160414115353973","provinceid":"","return_code":"SUCCESS","signMsg":"7facaff4704647e14a3854181baef174","usercode":"41389e3f30ecb763fb73979824ea0a58"}

 
记录时间：2016-04-14 11:54:03,578  线程ID:[629]  日志级别：INFO  
错误描述：典贷支付校验订单UniPay020160414115353973

 
记录时间：2016-04-14 11:54:03,594  线程ID:[629]  日志级别：ERROR 
错误描述：Application级别错误 http://pay.515.com:80/callback/diandaiPay?serviceid=validateorderid
System.Collections.Generic.KeyNotFoundException: 给定关键字不在字典中。
            */


            log.Info("###################diandaiPay联通回调#####################");
            log.Info(" diandaiPay联通回调 Url : " + Utils.GetUrl());




            string serviceid = queryvalues.ContainsKey("serviceid") ? queryvalues["serviceid"] : string.Empty;

            UnicomResultNotify resultNotify = new UnicomResultNotify(this);
            UnicomPayData notifyData = new UnicomPayData();
            UnicomPayData res = new UnicomPayData();
            if (serviceid == "validateorderid")
            {
                log.Info("diandaiPay联通回调 serviceid==validateorderid");



                notifyData = resultNotify.CheckNotify();

                log.Info("diandaiPay联通回调支付校验notifyData: " + JsonConvert.SerializeObject(notifyData.GetValues()));
                log.Info("diandaiPay联通回调支付校验订单" + notifyData.GetValue("orderid").ToString());
                //<checkOrderIdRsp>0</checkOrderIdRsp>  //0-验证成功 1-验证失败，必填
                //<gameaccount>xxx</gameaccount>    //游戏账号，长度<=64，必填
                //<imei>xxx</imei>                  //设备标识，必填
                //<macaddress >xxx</macaddress >    //MAC地址去掉冒号，必填
                //<ipaddress>xxx</ipaddress>        //IP地址，去掉点号，补零到每地址段3位，如：192168000001，必填
                //<serviceid>xxx</serviceid>        //12位计费点（业务代码），必填
                //<channelid>xxx</channelid>        //渠道ID，必填，如00012243
                //<cpid>xxx</cpid >                 //CPID，必填，账户管理，账户信息中查看。
                //<ordertime>yyyyMMddhhmmss</ordertime> //订单时间戳，14位时间格式    
                //<appversion >xxx</appversion >     //应用版本号，必填，长度<=32
                //gameaccount, ",", imei, ",", macaddress, ",", ipaddress, ",", appversion

                RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = notifyData.GetValue("orderid").ToString() });
                if (rc == null)
                {
                    res.SetValue("checkOrderIdRsp", 1);
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", string.Format("订单[{0}]不存在", notifyData.GetValue("orderid")));
                    log.Error(Utils.GetUrl() + " UnicomPay validateorderid fail : " + res.ToXmlForCheck());
                    return Content(res.ToXmlForCheck(), "text/xml");
                }
                log.Info("diandaiPay联通回调支付订单CheckInfo:" + rc.CheckInfo);
                var checkinfo = rc.CheckInfo.Split(',');
                //185****2154,357458041037465,086361ecbc79,201,1.5
                if (checkinfo.Length > 0)
                {
                    log.Info("diandaiPay联通回调 checkinfo.Length>0 notifyData: " + JsonConvert.SerializeObject(notifyData.GetValues()));

                    notifyData.SetValue("checkOrderIdRsp", 0);
                    log.Info("diandaiPay联通回调 gameaccount: " + checkinfo[0]);

                    notifyData.SetValue("gameaccount", checkinfo[0]);
                    log.Info("diandaiPay联通回调 imei: " + checkinfo[1]);
                    notifyData.SetValue("imei", checkinfo[1]);
                    log.Info("diandaiPay联通回调 macaddress: " + checkinfo[2]);
                    notifyData.SetValue("macaddress", checkinfo[2]);
                    log.Info("diandaiPay联通回调 ipaddress: " + checkinfo[3]);
                    notifyData.SetValue("ipaddress", checkinfo[3]);
                    log.Info("diandaiPay联通回调 serviceid: rc.ProductID " + rc.ProductID);
                    notifyData.SetValue("serviceid", UnicomConfig.GetServiceid(rc.ProductID));
                    notifyData.SetValue("channelid", 0);
                    log.Info("diandaiPay联通回调 cpid " + UnicomConfig.CPID);
                    notifyData.SetValue("cpid", UnicomConfig.CPID);
                    notifyData.SetValue("ordertime", rc.CreateTime.ToString());
                    notifyData.SetValue("appversion", checkinfo[4]);
                    notifyData.SetValue("appversion", checkinfo[4]);
                }
                else
                {
                    notifyData.SetValue("checkOrderIdRsp", 1);
                }
                log.Info("diandaiPay联通回调 validateorderid: " + notifyData.ToXmlForCheck());
                return Content(notifyData.ToXmlForCheck(), "text/xml");
            }

            notifyData = resultNotify.ProcessNotify();
            if (!notifyData.IsSet("orderid"))
            {
                return Content(notifyData.ToXml(), "text/xml");
            }
            else
            {
                try
                {


                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = notifyData.GetValue("orderid").ToString() });
                    if (rc == null)
                    {
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", string.Format("订单[{0}]不存在", notifyData.GetValue("orderid")));
                        log.Error(Utils.GetUrl() + " diandaiPay联通回调 fail : " + res.ToXml());
                        return Content(res.ToXml(), "text/xml");
                    }
                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);


                    //if(UnicomConfig.GetServiceid(rc.ProductID).Equals(notifyData.GetValue("consumeCode")))
                    //{
                    //    res.SetValue("return_code", "FAIL");
                    //    res.SetValue("return_msg", string.Format("金额校验失败"));
                    //    log.Error(Utils.GetUrl() + " UnicomPay fail : " + res.ToXml());
                    //    return Content(res.ToXml(), "text/xml");
                    //}
                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;

                    bool firstGif = iF == isFirst.是;


                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }

                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.联通 });



                    log.Info(" diandaiPay联通回调 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);



                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";
                    uint rmb = (uint)(rc.Money);

                    log.Error("diandaiPay联通回调 rmb：" + rmb);
                    RechargeBLL.Add(new Recharge { BillNo = notifyData.GetValue("orderid").ToString(), OpenID = rc.SerialNo, UserID = rc.UserID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.联通, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });

                    normal ServiceNormalS = normal.CreateBuilder()
                    .SetUserID((uint)rc.UserID)
                    .SetList(list)
                    .SetRmb(rmb)
                    .SetRmbActual(rmb)
                    .SetFirstGif(firstGif)
                    .SetBillNo(notifyData.GetValue("orderid").ToString())
                    .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info(" diandaiPay联通回调 ServiceResult : " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                RechargeBLL.UpdateReachTime(rc.SerialNo);
                                log.Info(" diandaiPay联通回调 ServiceResult[" + notifyData.GetValue("orderid").ToString() + "] 服务器发货: " + ServiceNormalC.Suc);

                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            res.SetValue("callbackRsp", "1");
                            res.SetValue("return_code", "SUCCESS");
                            res.SetValue("return_msg", "OK");
                            log.Info(" diandaiPay联通回调 Process Success[" + notifyData.GetValue("orderid").ToString() + "]:  " + rc.SerialNo + " \n" + res.ToXml());
                            return Content(res.ToXml(), "text/xml");
                        case CenterCmd.CS_CONNECT_ERROR:
                            log.Info(" diandaiPay联通回调 ServiceResult : CS_CONNECT_ERROR");
                            break;
                    }

                    RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                    log.Info("fail");

                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "服务器发货失败");
                    return Content(res.ToXml(), "text/xml");



                }
                catch (Exception err)
                {
                    //Response.Redirect("mobilecall://fail?err=" + err);
                    //return Content("支付失败!" + err);
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "服务器发货失败 : " + err.Message);
                    log.Error(" UnicomPay fail : " + res.ToXml(), err);
                    return Content(res.ToXml(), "text/xml");
                }


            }

        }

      
        /// <summary>
        /// XY苹果助手
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult ResultNotifyPageForXY(Dictionary<string, string> queryvalues)
        {
            log.Info("###################ResultNotifyPageForXY苹果助手回调#####################");
            log.Info(" ResultNotifyPageForXY苹果助手回调 Url : " + Utils.GetUrl());

            if (queryvalues.Count > 0)//判断是否有带返回参数
            {

                string method = Request.HttpMethod;
                string url = Utils.GetUrlPath();
                string _sign = queryvalues.ContainsKey("sign") ? queryvalues["sign"] : string.Empty;
                string _sig = queryvalues.ContainsKey("sig") ? queryvalues["sig"] : string.Empty;
                //appid_orderid
                string extra = queryvalues.ContainsKey("extra") ? queryvalues["extra"] : string.Empty;
                string orderid = queryvalues.ContainsKey("orderid") ? queryvalues["orderid"] : string.Empty;
                string uid = queryvalues.ContainsKey("uid") ? queryvalues["uid"] : string.Empty;
                string serverid = queryvalues.ContainsKey("serverid") ? queryvalues["serverid"] : string.Empty;
                //元
                double amount = queryvalues.ContainsKey("amount") ? Convert.ToDouble(queryvalues["amount"]) : 0;

                string tradeno = extra;
                string appid = orderid.Split('_')[0];

                bool verifyResult = XYSigCheck.VerifySig(queryvalues, GL.Pay.XY.XYConfig.GetKey(appid), _sign, _sig);

                if (verifyResult)//验证成功
                {
                    try
                    {

                        RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = tradeno });
                        if (rc == null)
                        {
                            log.Error(" ResultNotifyPageForXY苹果助手回调 订单[" + tradeno + "]不存在");
                            return Json(new
                            {
                                ret = 5,
                                msg = "订单不存在"
                            }, JsonRequestBehavior.AllowGet);
                        }

                        Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                        if (user == null)
                        {
                            log.Error(" ResultNotifyPageForXY苹果助手回调 用户不存在" + Utils.GetUrl());
                            return Json(new
                            {
                                ret = 2,
                                msg = "用户不存在"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                        if (iap == null)
                        {
                            log.Error(" ResultNotifyPageForXY苹果助手回调 支付配表出错" + Utils.GetUrl());
                            return Json(new
                            {
                                ret = 1,
                                msg = "支付配表出错"
                            }, JsonRequestBehavior.AllowGet);
                        }

                        Recharge rech = RechargeBLL.GetModelByBillNo(new Recharge { BillNo = orderid });
                        if (rech != null)
                        {//说明此流水已经存在
                            log.Error(" ResultNotifyPageForXY苹果助手回调 此流水已经存在");
                            return Json(new
                            {
                                ret = 1,
                                msg = "支付配表出错"
                            }, JsonRequestBehavior.AllowGet);
                        }

                        isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                        chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;
                        bool firstGif = iF == isFirst.是;




                        Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                        if (recharge != null)//首冲过
                        {
                            if (firstGif == true)
                            {//又是首冲，则改变其为不首冲
                                firstGif = false;
                                ct = (chipType)1;
                                iF = isFirst.否;
                                iap.goodsType = 1;
                                log.Error(" 第多次首冲，修改firstGif值");
                            }
                        }

                        RechargeIP ipmodel = GetClientIp();
                        RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.XY助手 });



                        log.Info(" ResultNotifyPageForXY苹果助手回调 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);


                        uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                        uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                        string list = iap.attach_props;
                        list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                        list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                        list = list.Trim(',') + ",";
                        uint rmb = (uint)(rc.Money);
                        if (firstGif)
                        {
                            gold = (uint)(iap.goods + iap.attach_chip);
                            dia = (uint)iap.attach_5b;
                        }
                        //amount
                        log.Error("ResultNotifyPageForXY苹果助手回调rmb：" + rmb);
                        log.Error("ResultNotifyPageForXY苹果助手回调amount(观察amount值是否为打折后的)：" + amount);

                        RechargeBLL.Add(new Recharge { BillNo = orderid, OpenID = rc.SerialNo, UserID = user.ID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.XY助手, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });

                        normal ServiceNormalS = normal.CreateBuilder()
                        .SetUserID((uint)user.ID)
                        .SetList(list)
                        .SetRmb(rmb)
                        .SetRmbActual(rmb)
                        .SetFirstGif(firstGif)
                        .SetBillNo(orderid)
                        .Build();

                        Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                        switch ((CenterCmd)tbind.header.CommandID)
                        {
                            case CenterCmd.CS_NORMAL:
                                normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                                log.Info(" ResultNotifyPageForXY苹果助手回调 ServiceResult : " + ServiceNormalC.Suc);
                                if (ServiceNormalC.Suc)
                                {
                                    RechargeBLL.UpdateReachTime(rc.SerialNo);
                                    log.Info(string.Concat(" ResultNotifyPageForXY苹果助手回调 ServiceResult[" + orderid + "] " + ServiceNormalC.Suc));
                                }
                                RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                                return Json(new
                                {
                                    ret = 0,
                                    msg = "OK"
                                }, JsonRequestBehavior.AllowGet);
                            case CenterCmd.CS_CONNECT_ERROR:
                                log.Info(" ResultNotifyPageForXY苹果助手回调 ServiceResult : CS_CONNECT_ERROR");
                                break;
                        }
                        RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                        log.Info("fail");

                        return Json(new
                        {
                            ret = 8,
                            msg = "服务器发货失败"
                        }, JsonRequestBehavior.AllowGet);


                    }
                    catch (Exception err)
                    {
                        log.Error(" XYPay 服务器发货失败" + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8), err);
                        return Json(new
                        {
                            ret = 8,
                            msg = "服务器发货失败" + err.Message
                        }, JsonRequestBehavior.AllowGet);
                    }

                }
                else//验证失败
                {
                    //Response.Write("fail");
                    log.Error(" XYPay 签名验证失败: " + Utils.GetUrl() + "?" + Core.CreateLinkStringUrlencode(queryvalues, Encoding.UTF8));
                    return Json(new
                    {
                        ret = 6,
                        msg = "签名验证失败"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                log.Error(" XYPay 无通知参数 : " + Utils.GetUrl());
                return Json(new
                {
                    ret = 1,
                    msg = "无通知参数"
                }, JsonRequestBehavior.AllowGet);
            }


        }



        [QueryValues]
        public ActionResult ApplicationTreasureGetBalance(Dictionary<string, string> queryvalues)
        {
            log.Info("##################开始" + DateTime.Now.ToString() + "应用宝ApplicationTreasureGetBalance登录查询余额############## ");
            log.Info("Url: " + Utils.GetUrl());
            log.Info("queryvalues: " + JsonConvert.SerializeObject(queryvalues));

            string openid = queryvalues.ContainsKey("openid") ? queryvalues["openid"] : string.Empty;
            string openkey = queryvalues.ContainsKey("openkey") ? queryvalues["openkey"] : string.Empty;
            string pay_token = queryvalues.ContainsKey("pay_token") ? queryvalues["pay_token"] : string.Empty;
            string appid = queryvalues.ContainsKey("appid") ? queryvalues["appid"] : string.Empty;
            string pf = queryvalues.ContainsKey("pf") ? queryvalues["pf"] : string.Empty;
            string pfkey = queryvalues.ContainsKey("pfkey") ? queryvalues["pfkey"] : string.Empty;
            string session_id = queryvalues.ContainsKey("session_id") ? queryvalues["session_id"] : string.Empty;
            string session_type = queryvalues.ContainsKey("session_type") ? queryvalues["session_type"] : string.Empty;
            string userid = queryvalues.ContainsKey("myuserid") ? queryvalues["myuserid"] : string.Empty;

            GL.Pay.AppTreasure.OpenApiHelper xx = new GL.Pay.AppTreasure.OpenApiHelper(Convert.ToInt32(appid), openid, openkey, pay_token, pf, pfkey, session_id, session_type);


            GL.Pay.AppTreasure.RstArray result = xx.GetBalanceLogin();


            log.Info("ApplicationTreasureGetBalance result:" + JsonConvert.SerializeObject(result));
            string msg = result.Msg;
            GL.Pay.AppTreasure.BalanceReciveMsg brec = JsonConvert.DeserializeObject<GL.Pay.AppTreasure.BalanceReciveMsg>(msg);
            //如果这个人有余额，那么保存起来,======》先全部添加
            if (brec.balance > 0)
            {
                AppTreasure appModel = new AppTreasure()
                {
                    Openid = openid,
                    Openkey = openkey,
                    Pay_token = pay_token,
                    Appid = appid,
                    Pf = pf,
                    Pfkey = pfkey,
                    Session_id = session_id,
                    Session_type = session_type,
                    Userid = userid,
                    Balance = brec.balance,
                    CreateTime = DateTime.Now
                };

                RechargeCheckBLL.AddAppTreInfo(appModel);

            }
           

            log.Info("##################结束" + DateTime.Now.ToString() + "应用宝ApplicationTreasureGetBalance登录查询余额############## ");
            return null;
        }

        [QueryValues]
        public ActionResult AppTrePay(Dictionary<string, string> queryvalues)
        {
            log.Info("##################开始" + DateTime.Now.ToString() + "应用宝AppTrePay手动后台充值############# ");

            string userid = queryvalues.ContainsKey("userid") ? queryvalues["userid"] : string.Empty;
           
            string productID = queryvalues.ContainsKey("productID") ? queryvalues["productID"] : string.Empty;
            IAPProduct iap = IAPProductBLL.GetModelByID(productID);

            AppTreasure model = RechargeCheckBLL.GetModelByUserID(userid);
            string openid = model.Openid;
            string openkey = model.Openkey;
            string pay_token = model.Pay_token;
            string appid = model.Appid;
            string pf = model.Pf;
            string pfkey = model.Pfkey;
            string session_id = model.Session_id;
            string session_type = model.Session_type;
            GL.Pay.AppTreasure.OpenApiHelper xx = new GL.Pay.AppTreasure.OpenApiHelper(Convert.ToInt32(appid), openid, openkey, pay_token, pf, pfkey, session_id, session_type);
            xx.amt = iap.price*100 / 10;
            GL.Pay.AppTreasure.RstArray payRes = xx.InPayLogin(); 
            log.Info("应用宝回调接口支付返回结果payRes:" + JsonConvert.SerializeObject(payRes));
            GL.Pay.AppTreasure.InPayReciveMsg rec = JsonConvert.DeserializeObject<GL.Pay.AppTreasure.InPayReciveMsg>(payRes.Msg);

            log.Info("应用宝回调接口支付返回结果rec:" + JsonConvert.SerializeObject(rec));
            string res = "";
            if (rec.ret == 0)
            {
                //发货
                string serialNo = Utils.GenerateOutTradeNo("AppTreasure_Re");
                isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;
                bool firstGif = iF == isFirst.是;
                Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID =Convert.ToInt64(userid) });
                if (recharge != null)//首冲过
                {
                    if (firstGif == true)
                    {//又是首冲，则改变其为不首冲
                        firstGif = false;
                        ct = (chipType)1;
                        iF = isFirst.否;
                        iap.goodsType = 1;
                        log.Error(" 第多次首冲，修改firstGif值");
                    }
                }
                RechargeIP ipmodel = GetClientIp();
                RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = Convert.ToInt32(userid), OrderID = serialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.应用宝 });
                uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                uint rmb = (uint)iap.price;
                if (firstGif)
                {
                    gold = (uint)(iap.goods + iap.attach_chip);
                    dia = (uint)iap.attach_5b;
                }
                string list = iap.attach_props;
                list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                list = list.Trim(',') + ",";

              
                log.Error("测试分：" + rmb * 100);
               
                RechargeBLL.Add(new Recharge { BillNo = serialNo, OpenID = rec.billno, UserID = Convert.ToInt64(userid), Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.应用宝, UserAccount = "", ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','), AgentID= recharge.AgentID});

                normal ServiceNormalS = normal.CreateBuilder()
                         .SetUserID((uint)Convert.ToInt64(userid))
                         .SetList(list)
                          .SetRmb(rmb * 100)
                         .SetRmbActual(rmb * 100)
                         .SetFirstGif(firstGif)
                         .SetBillNo(serialNo)
                         .Build();

                Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
            
                switch ((CenterCmd)tbind.header.CommandID)
                {
                    case CenterCmd.CS_NORMAL:
                        normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                        log.Info("应用宝回调接口服务器返回 ServiceNormalC.Suc=" + ServiceNormalC.Suc);
                        if (ServiceNormalC.Suc)
                        {
                            RechargeBLL.UpdateReachTime(serialNo);
                            //log.Info(" QQPay ServiceResult : " + ServiceNormalC.Suc);
                            log.Info(string.Concat("应用宝回调接口服务器发货成功:[" + serialNo + "] " + ServiceNormalC.Suc));
                            res = "2";
                        }
                        else {
                            res = "1";
                        }
                        // RechargeBLL.Add(new Recharge { BillNo = item.SerialNo, OpenID = openid, UserID = item.UserID, Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.应用宝, UserAccount = "" });
                        RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = serialNo });

                       
                        //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                        break;
                    case CenterCmd.CS_CONNECT_ERROR:
                        log.Info("应用宝回调接口服务器返回连接失败");

                        res = "0";
                      //  / mpay / cancel_pay_m
                        xx.billno = rec.billno;
                        GL.Pay.AppTreasure.RstArray cres = xx.CancelPayLogin();
                        log.Info("应用宝回调接口取消订单支付结果" + JsonConvert.SerializeObject(cres));

                        break;
                    default:
                        res = "-1";

                        xx.billno = rec.billno;
                        GL.Pay.AppTreasure.RstArray cres2 = xx.CancelPayLogin();
                        log.Info("应用宝回调接口取消订单支付结果" + JsonConvert.SerializeObject(cres2));

                        log.Info("应用宝回调接口服务器返回未知");
                        break;
                }



            }
            else
            {
                res = "-2";
            }

            Response.Clear();


            log.Info("##################结束" + DateTime.Now.ToString() + "应用宝AppTrePay手动后台充值############# ");


            return Content(res);
        }





        [QueryValues]
        public ActionResult AppTrePay2(Dictionary<string, string> queryvalues)
        {
            string userid = queryvalues.ContainsKey("userid") ? queryvalues["userid"] : string.Empty;

            string productID = queryvalues.ContainsKey("productID") ? queryvalues["productID"] : string.Empty;
            IAPProduct iap = IAPProductBLL.GetModelByID(productID);

            AppTreasure model = RechargeCheckBLL.GetModelByUserID(userid);
            string openid = model.Openid;
            string openkey = model.Openkey;
            string pay_token = model.Pay_token;
            string appid = model.Appid;
            string pf = model.Pf;
            string pfkey = model.Pfkey;
            string session_id = model.Session_id;
            string session_type = model.Session_type;
            GL.Pay.AppTreasure.OpenApiHelper xx = new GL.Pay.AppTreasure.OpenApiHelper(Convert.ToInt32(appid), openid, openkey, pay_token, pf, pfkey, session_id, session_type);
            xx.amt = iap.price * 100 / 10;
            GL.Pay.AppTreasure.RstArray payRes = xx.InPayLogin();
            log.Info("应用宝回调接口支付返回结果:" + JsonConvert.SerializeObject(payRes));
            GL.Pay.AppTreasure.InPayReciveMsg rec = JsonConvert.DeserializeObject<GL.Pay.AppTreasure.InPayReciveMsg>(payRes.Msg);
            string res = "";
            if (rec.ret == 0)
            {
                res = "2";
            }
            else
            {
                res = "-2";
            }

            Response.Clear();

            return Content(res);
        }

        /// <summary>
        /// 应用宝回调接口
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult ApplicationTreasure(Dictionary<string, string> queryvalues)
        {

            log.Info("##################开始" + DateTime.Now.ToString() + "ApplicationTreasure应用宝回调接口############## ");
            log.Info("ApplicationTreasure应用宝回调接口 Url: " + Utils.GetUrl());
            log.Info("ApplicationTreasure应用宝回调接口 queryvalues: " + JsonConvert.SerializeObject(queryvalues));



         
            if (queryvalues.Count > 0)//判断是否有带返回参数
            {
                string method = Request.HttpMethod;
                string url = Utils.GetUrlPath();
                if (true)//验证成功
                {
                    try
                    {
                        string billno = queryvalues.ContainsKey("billno") ? queryvalues["billno"] : string.Empty;
                        string openid = queryvalues.ContainsKey("openid") ? queryvalues["openid"] : string.Empty;
                        string openkey = queryvalues.ContainsKey("openkey") ? queryvalues["openkey"] : string.Empty;
                        string pay_token = queryvalues.ContainsKey("pay_token") ? queryvalues["pay_token"] : string.Empty;
                        string appid = queryvalues.ContainsKey("appid") ? queryvalues["appid"] : string.Empty;
                        string pf = queryvalues.ContainsKey("pf") ? queryvalues["pf"] : string.Empty;
                        string pfkey = queryvalues.ContainsKey("pfkey") ? queryvalues["pfkey"] : string.Empty;
                        string session_id = queryvalues.ContainsKey("session_id") ? queryvalues["session_id"] : string.Empty;
                        string session_type = queryvalues.ContainsKey("session_type") ? queryvalues["session_type"] : string.Empty;
                        /*
                        "session_id":"hy_gameid","session_type":"wc_actoken"
                        */

                        RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = billno.Trim() });
                        if (rc == null)
                        {
                            log.Error(" ApplicationTreasure应用宝回调接口 订单[" + billno + "]不存在");
                            return Content("fail");
                        }
                        if (rc.UserID== 578867) {
                            Response.Clear();
                            return Json(new
                            {
                                ret = 0,
                                msg = "ok"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                        if (user == null)
                        {
                            log.Error(" ApplicationTreasure应用宝回调接口 [" + rc.UserID + "]用户不存在" + Utils.GetUrl());
                            Response.Clear();
                            return Json(new
                            {
                                ret = 4,
                                msg = "用户不存在"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        
                        IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                        if (iap == null)
                        {
                            log.Error(" ApplicationTreasure应用宝回调接口 支付配表出错" + Utils.GetUrl());
                            Response.Clear();
                            return Json(new
                            {
                                ret = 4,
                                msg = "支付配表出错"
                            }, JsonRequestBehavior.AllowGet);
                        }




                        GL.Pay.AppTreasure.OpenApiHelper xx = new GL.Pay.AppTreasure.OpenApiHelper(Convert.ToInt32(appid), openid, openkey, pay_token, pf, pfkey, session_id, session_type);


                        GL.Pay.AppTreasure.RstArray result = xx.GetBalance();


                        log.Info("应用宝回调接口查询余额结果:" + JsonConvert.SerializeObject(result));
                        string msg = result.Msg;
                        GL.Pay.AppTreasure.BalanceReciveMsg brec = JsonConvert.DeserializeObject<GL.Pay.AppTreasure.BalanceReciveMsg>(msg);
#if Debug
                        brec.ret = 0; brec.balance = rc.Money / 10 + 100;

#endif
#if P17
           brec.ret = 0; brec.balance = rc.Money / 10 + 100;
#endif
#if Test
           brec.ret = 0; brec.balance = rc.Money / 10 + 100;
#endif

                        //解决扣费延迟问题
                        //int count = 0;
                        //while (brec.balance * 10 < rc.Money) {
                        //    if (count > 7) { break; }
                        //    count++;
                        //    Thread.Sleep(20 * 1000);//20秒之后再查询
                        //    result = xx.GetBalance();
                        //    log.Info("应用宝回调接口查询余额结果扣费延迟第" + count + "次:" + JsonConvert.SerializeObject(result));
                        //    msg = result.Msg;
                        //    brec = JsonConvert.DeserializeObject<GL.Pay.AppTreasure.BalanceReciveMsg>(msg);
                        //}




                        if (brec.ret == 0)
                        {

                            int banl = brec.balance;
                            int money = rc.Money;
                            log.Info("应用宝回调接口支付判断banl * 10 >= money,banl=" + banl + ",money=" + money);
                            //if (banl * 10 >= money)
                            //{
                                xx.amt = money / 10;
                                GL.Pay.AppTreasure.RstArray payRes = xx.InPay();
                                log.Info("应用宝回调接口支付返回结果:" + JsonConvert.SerializeObject(payRes));

                                GL.Pay.AppTreasure.InPayReciveMsg rec = JsonConvert.DeserializeObject<GL.Pay.AppTreasure.InPayReciveMsg>(payRes.Msg);
#if Debug
                                rec.ret = 0;
#endif
#if P17
           rec.ret = 0;
#endif
#if Test
           rec.ret = 0;
#endif
                                if (rec.ret == 0)
                                {
                                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;
                                    bool firstGif = iF == isFirst.是;

                                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                                    if (recharge != null)//首冲过
                                    {
                                        if (firstGif == true)
                                        {//又是首冲，则改变其为不首冲
                                            firstGif = false;
                                            ct = (chipType)1;
                                            iF = isFirst.否;
                                            iap.goodsType = 1;
                                            log.Error(" 第多次首冲，修改firstGif值");
                                        }
                                    }


                                    RechargeIP ipmodel = GetClientIp();
                                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.应用宝 });


                                log.Info(" 应用宝回调接口 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);
                             
                                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                                  
                                uint rmb = (uint)iap.price;
                                    if (firstGif)
                                    {
                                        gold = (uint)(iap.goods + iap.attach_chip);
                                        dia = (uint)iap.attach_5b;
                                    }
                                string list = iap.attach_props;
                                list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                                list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                                list = list.Trim(',') + ",";

                                log.Info("应用宝回调接口通知服务器发货: userID =" + user.ID + ",gold=" + gold + ",dia=" + dia + ",rmb=" + rmb + ",billno=" + billno);
                                    log.Error("应用宝回调接口rmb：" + rmb * 100);


                                //string ver = RechargeDAL.GetVersion(new Recharge { BillNo = billno, OpenID = rc.SerialNo, UserID = user.ID, Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.应用宝, UserAccount = user.NickName });
                                //log.Info("版本号:"+ver);

                                RechargeBLL.Add(new Recharge { BillNo = billno, OpenID = rec.billno, UserID = user.ID, Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.应用宝, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });
                                //20000|300,
                                log.Info("list:" + list);

                                normal ServiceNormalS = normal.CreateBuilder()
                                    .SetUserID((uint)user.ID)
                                    .SetList(list)
                                    .SetRmb(rmb * 100)
                                    .SetRmbActual(rmb* 100)
                                    .SetFirstGif(firstGif)
                                    .SetBillNo(billno)
                                    .Build();

                                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                                    switch ((CenterCmd)tbind.header.CommandID)
                                    {
                                        case CenterCmd.CS_NORMAL:
                                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                                            log.Info("应用宝回调接口服务器返回 ServiceNormalC.Suc=" + ServiceNormalC.Suc);
                                            if (ServiceNormalC.Suc)
                                            {
                                              RechargeBLL.UpdateReachTime(rc.SerialNo);
                                              log.Info(string.Concat("应用宝回调接口服务器发货成功:[" + billno + "] " + ServiceNormalC.Suc));
                                            }
                                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                                            Response.Clear();
                                            return Json(new
                                            {
                                                ret = 0,
                                                msg = result.Msg
                                            }, JsonRequestBehavior.AllowGet);
                                    case CenterCmd.CS_CONNECT_ERROR:
                                            log.Info("应用宝回调接口服务器返回连接失败");
                                        ///mpay/cancel_pay_m
                                        RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                                        break;
                                        default:
                                            log.Info("应用宝回调接口服务器返回未知");
                                            break;
                                    }
                                    xx.billno = rec.billno;
                                    GL.Pay.AppTreasure.RstArray cres = xx.CancelPay();
                                    log.Info("应用宝回调接口取消订单支付结果" + JsonConvert.SerializeObject(cres));

                                



                                    log.Info("AppTreasure  Url 回调方法成功 result.Msg:" + result.Msg);
                                }
                            //}
                              
                            Response.Clear();
                                return Json(new
                                {
                                    ret = 0,
                                    msg = msg
                                }, JsonRequestBehavior.AllowGet);







                        }
                        else if (brec.ret == 1001)
                        {
                            Response.Clear();
                            return Json(new
                            {
                                ret = 1001,
                                msg = msg
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else if (brec.ret == 1018)
                        {
                            Response.Clear();
                            return Json(new
                            {
                                ret = 1018,
                                msg = msg
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Response.Clear();
                            return Json(new
                            {
                                ret = -1,
                                msg = msg
                            }, JsonRequestBehavior.AllowGet);
                        }














                    }
                    catch (Exception err)
                    {
                        log.Error(" AppTreasure 服务器发货失败" + Utils.GetUrl(), err);
                        return Json(new
                        {
                            ret = 4,
                            msg = "服务器发货失败" + err.Message
                        }, JsonRequestBehavior.AllowGet);
                    }


                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    //Response.Write("success");  //请不要修改或删除

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    //Response.Write("fail");
                    log.Error(" AppTreasure 签名验证失败: " + Utils.GetUrl());
                    return Json(new
                    {
                        ret = 4,
                        msg = "签名验证失败"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                log.Error(" AppTreasure 无通知参数 : " + Utils.GetUrl());
                return Json(new
                {
                    ret = 4,
                    msg = "无通知参数"
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 海马支付回调
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult HippocampiPay(Dictionary<string, string> queryvalues)
        {
            log.Info("##################" + DateTime.Now.ToString() + "HippocampiPay海马支付回调接口############## ");
            log.Info("HippocampiPay海马支付回调接口 Url: " + Utils.GetUrl());
            log.Info("HippocampiPay海马支付回调接口 queryvalues: " + JsonConvert.SerializeObject(queryvalues));

            if (queryvalues.Count > 0)
            {
                string billno = queryvalues.ContainsKey("out_trade_no") ? queryvalues["out_trade_no"] : string.Empty;
                string trade_status = queryvalues.ContainsKey("trade_status") ? queryvalues["trade_status"] : string.Empty;

                bool verifyResult = HippocampiSigCheck.VerifySig(queryvalues, "c181d1cb928d82b3377889ad49adb381");




                if (!verifyResult) { log.Info(" HippocampiPay海马支付回调接口  Url 回调方法: md5校验失败"); return Content(""); }


                if (trade_status == "1")
                {
                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = billno.Trim() });
                    if (rc == null)
                    {
                        log.Error(" HippocampiPay海马支付回调接口 订单[" + billno + "]不存在");
                        return Json(new
                        {
                            ret = 5,
                            msg = "订单不存在"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    if (user == null)
                    {
                        log.Error(" HippocampiPay海马支付回调接口 [" + rc.UserID + "]用户不存在" + Utils.GetUrl());
                        return Json(new
                        {
                            ret = 4,
                            msg = "用户不存在"
                        }, JsonRequestBehavior.AllowGet);
                    }

                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                    if (iap == null)
                    {
                        log.Error(" HippocampiPay海马支付回调接口 支付配表出错" + Utils.GetUrl());
                        return Json(new
                        {
                            ret = 4,
                            msg = "支付配表出错"
                        }, JsonRequestBehavior.AllowGet);
                    }

                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;



                    bool firstGif = iF == isFirst.是;


                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }

                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.海马玩 });


                    log.Info(" HippocampiPay海马支付回调接口 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);



                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    uint rmb = (uint)iap.price;
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";

                    log.Info("HippocampiPay海马支付回调接口 user.ID =" + user.ID + ",  gold=" + gold + ",dia=" + dia + ",rmb=" + rmb + ",billno=" + billno);

                    log.Error("HippocampiPay海马支付回调接口 充钱：" + rmb * 100);
                    RechargeBLL.Add(new Recharge { BillNo = billno, OpenID = rc.SerialNo, UserID = user.ID, Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.海马玩, UserAccount = user.NickName,AgentID=rc.AgentID });

                    normal ServiceNormalS = normal.CreateBuilder()
                    .SetUserID((uint)user.ID)
                    .SetList(list)
                    .SetRmb(rmb * 100)
                    .SetRmbActual(rmb * 100)
                    .SetFirstGif(firstGif)
                    .SetBillNo(billno)
                    .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info(" HippocampiPay海马支付回调接口 ServiceResult : " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                log.Info(string.Concat(" HippocampiPay海马支付回调接口 ServiceResult[" + billno + "] " + ServiceNormalC.Suc));
                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = billno });
                            return Content("success");
                        case CenterCmd.CS_CONNECT_ERROR:

                            log.Info(" HippocampiPay海马支付回调接口 ServiceResult : CenterCmd.CS_CONNECT_ERROR");

                            return Content("");

                        default:

                            log.Info(" HippocampiPay海马支付回调接口 ServiceResult :default");
                            return Content("");

                    }



                 
                  
                }
                else
                {
                    log.Info(" HippocampiPay海马支付回调接口 订单失败trade_status:" + trade_status);
                    return Content("");
                }


            }





            log.Info("HippocampiPay海马支付回调接口 queryvalues length<=0");

            return Content("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult yidongL(Dictionary<string, string> queryvalues)
        {
            //log.Info(" yidongL  Url 回调方法: get" + Utils.GetUrl());
            //log.Info(" yidongL  Url 回调方法: post Request.Form " + JsonConvert.SerializeObject(queryvalues));
            //log.Info("queryvalues:request请求 " + JsonConvert.SerializeObject(Request));
            //log.Info("queryvalues:get请求 " + JsonConvert.SerializeObject(Request.QueryString));
            //log.Info("queryvalues:post请求 " + JsonConvert.SerializeObject(Request.Form));
            //log.Info("queryvalues:消息头请求 " + JsonConvert.SerializeObject(Request.Headers));
            //log.Info("queryvalues:消息体请求 " + JsonConvert.SerializeObject(Request.RequestContext));
            //log.Info("queryvalues:请求方式 " + JsonConvert.SerializeObject(Request.HttpMethod));
            /*
                "userId":"1494524781",
                "key":"JS000d16615061780640815702851",
                "cpId":"799087",
                "cpServiceId":"608716051661",
                "channelId":"12064000",
                "p":"",
                "region":"200",
                "Ua":"HTC M8d"
            */
            //string userId = queryvalues.ContainsKey("userId") ? queryvalues["userId"] : string.Empty;//用户伪码
            //string key = queryvalues.ContainsKey("key") ? queryvalues["key"] : string.Empty;//用户登录网游的事务ID
            //string cpId = queryvalues.ContainsKey("cpId") ? queryvalues["cpId"] : string.Empty;//合作方ID
            //string cpServiceId = queryvalues.ContainsKey("cpServiceId") ? queryvalues["cpServiceId"] : string.Empty;//计费代码
            //string channelId = queryvalues.ContainsKey("channelId") ? queryvalues["channelId"] : string.Empty;//8位渠道代码
            //string p = queryvalues.ContainsKey("p") ? queryvalues["p"] : string.Empty;//透传参数
            //string region = queryvalues.ContainsKey("region") ? queryvalues["region"] : string.Empty;//用户归属地信息
            //string Ua = queryvalues.ContainsKey("Ua") ? queryvalues["Ua"] : string.Empty;//用户手机型号

            Response.Clear();
            return Content("0", "string");
        }

        /// <summary>
        /// 获取客户端的ip地址，考虑到代理，只获取实际地址
        /// </summary>
        /// <returns></returns>
        private RechargeIP GetClientIp() {
            RechargeIP mode = new RechargeIP();
            string userIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userIP))
            {
                userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                mode.Method = "REMOTE_ADDR";
            }
            else {
                mode.Method = "HTTP_X_FORWARDED_FOR";
            }
            mode.IP = userIP;
            return mode;

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult yidongFGP(Dictionary<string, string> queryvalues)
        {

            try
            {
                Response.Clear();
                log.Info("##################" + DateTime.Now.ToString() + "yidongFGP移动支付回调############## ");
                log.Info("yidongFGP移动支付回调 Url : " + Utils.GetUrl());

                //获取ip地址
                string ip = GetClientIp().IP;

                log.Info("yidongFGP移动支付回调ip地址：" + ip);
                if (ip == "112.4.3.36") {

                }




                StreamReader reader = new StreamReader(Request.InputStream);
                String xmlData = reader.ReadToEnd();
                log.Info(" yidongFGP移动支付回调输入流：" + xmlData);
                //   xmlData = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?><request><userId>1301881222</userId><contentId>608716051661</contentId><consumeCode>006101039001</consumeCode><cpid>799087</cpid><hRet>0</hRet><status>1800</status><versionId>21150</versionId><cpparam>Y160202170245438</cpparam><packageID /><provinceId>200</provinceId><channelId>12064000</channelId></request>";

                GL.Pay.YiDong.XMLHelp xh = new GL.Pay.YiDong.XMLHelp();

                xh.Document = XDocument.Parse(xmlData);
                DataRow dr = xh.GetEntityRow();

                //把数据重新返回给客户端



                string userId = dr[0].ToString();//用户伪码
                string contentId = dr[1].ToString();//计费代码
                string consumeCode = dr[2].ToString();//道具计费代码
                string cpId = dr[3].ToString();//合作代码
                string hRet = dr[4].ToString();//平台计费结果(0成功，其他失败)
                string status = dr[5].ToString();//返回状态（内码）
                string versionId = dr[6].ToString();//版本号，统一2_0_0
                string cpparam = dr[7].ToString();//cp透传参数
                string packageID = dr[8].ToString();//套餐包ID


                /*
                <request>
                <userId>1301881222</userId>
                <contentId>608716051661</contentId>
                <consumeCode>006101039001</consumeCode>
                <cpid>799087</cpid>
                <hRet>0</hRet>
                <status>1800</status>
                <versionId>21150</versionId>
                <cpparam>Y160202170245438</cpparam>
                <packageID />
                <provinceId>200</provinceId>
                <channelId>12064000</channelId></request>
                */
                //string userId = "1301881222";//用户伪码
                //string contentId = "608716051661";//计费代码
                //string consumeCode = "006101039001";//道具计费代码
                //string cpId = "799087";//合作代码
                //string hRet = "0";//平台计费结果(0成功，其他失败)
                //string status = "1800";//返回状态（内码）
                //string versionId = "21150";//版本号，统一2_0_0
                //string cpparam = "Y160203091708854";//cp透传参数
                //string packageID = "12064000";//套餐包ID

                if (hRet == "0" || status == "1800")
                {


                    //发放道具  cpparam
                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = cpparam.Trim() });
                    if (rc == null)
                    {
                        log.Error(" yidongFGP移动支付回调 订单[" + cpparam + "]不存在");
                        GL.Pay.YiDong.response dmError = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                        // 序列化为XML格式显示
                        GL.Pay.YiDong.XmlResult xResultError = new GL.Pay.YiDong.XmlResult(dmError, dmError.GetType());
                        log.Info(" yidongFGP移动支付回调 结果：" + xResultError);
                        return xResultError;
                    }
                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    if (user == null)
                    {
                        log.Error(" yidongFGP移动支付回调 [" + rc.UserID + "]用户不存在" + Utils.GetUrl());
                        GL.Pay.YiDong.response dmError = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                        // 序列化为XML格式显示
                        GL.Pay.YiDong.XmlResult xResultError = new GL.Pay.YiDong.XmlResult(dmError, dmError.GetType());
                        log.Info(" yidongFGP移动支付回调 结果：" + xResultError);
                        return xResultError;
                    }

                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                    if (iap == null)
                    {
                        log.Error(" yidongFGP移动支付回调 支付配表出错" + Utils.GetUrl());
                        GL.Pay.YiDong.response dmError = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                        // 序列化为XML格式显示
                        GL.Pay.YiDong.XmlResult xResultError = new GL.Pay.YiDong.XmlResult(dmError, dmError.GetType());
                        log.Info("yidongFGP移动支付回调  结果：" + xResultError);
                        return xResultError;


                    }



                    //检测_identityid 是否被封号
                    Role role = RoleBLL.GetModelByID(new Role() { ID = rc.UserID });
                    if (role.IsFreeze == isSwitch.关)
                    {//说明被封号了，不允许回调发货


                        GL.Pay.YiDong.response dmError = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                        // 序列化为XML格式显示
                        GL.Pay.YiDong.XmlResult xResultError = new GL.Pay.YiDong.XmlResult(dmError, dmError.GetType());
                        log.Error("yidongFGP移动支付回调 Callback _identity= " + rc.UserID + ",被封号了，不能回调发货!");
                        return xResultError;


                    }



                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;



                    bool firstGif = iF == isFirst.是;

                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }


                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = cpparam, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.移动 });

                    log.Info(" yidongFGP移动支付回调 订单号= " + cpparam + ",用户ID:  " + rc.UserID);



                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    uint rmb = (uint)iap.price;
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";

                    log.Info(" yidongFGP移动支付回调 user.ID =" + user.ID + ",  gold=" + gold + ",dia=" + dia + ",rmb=" + rmb + ",billno=" + cpparam);
                    log.Error("yidongFGP移动支付回调测试分：" + rmb * 100);
                    log.Info("yidongFGP移动支付回调开始加入数据：BillNo=" + cpparam + ",OpenID=" + rc.SerialNo + ",UserID=" + user.ID + "Money=" + rmb + ",CreateTime=" + DateTime.Now + ",Chip=" + gold + ",Diamond=" + dia + ",ChipType=" + ct + ",IsFirst=" + iF + ",NickName=" + iap.productname + ",PayItem=" + iap.product_id);
                    RechargeBLL.Add(new Recharge { BillNo = cpparam, OpenID = rc.SerialNo, UserID = user.ID, Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.移动, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });

                    normal ServiceNormalS = normal.CreateBuilder()
                        .SetUserID((uint)user.ID)
                        .SetList(list)
                        .SetRmb(rmb * 100)
                        .SetRmbActual(rmb * 100)
                        .SetFirstGif(firstGif)
                        .SetBillNo(cpparam)
                        .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info(" yidongFGP移动支付回调 ServiceResult : " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                RechargeBLL.UpdateReachTime(cpparam);
                                log.Info(string.Concat(" yidongFGP移动支付回调 ServiceResult[" + cpparam + "] " + ServiceNormalC.Suc));
                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = cpparam });
                            break;
                        case CenterCmd.CS_CONNECT_ERROR:

                            log.Info(" yidongFGP移动支付回调 ServiceResult : CenterCmd.CS_CONNECT_ERROR");
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = cpparam });
                            GL.Pay.YiDong.response dmError = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                            // 序列化为XML格式显示
                            GL.Pay.YiDong.XmlResult xResultError = new GL.Pay.YiDong.XmlResult(dmError, dmError.GetType());
                            log.Info("yidongFGP移动支付回调 结果：" + xResultError);
                            return xResultError;

                        default:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = cpparam });
                            log.Info(" yidongFGP移动支付回调 ServiceResult :default");
                            GL.Pay.YiDong.response dmdefault = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                            // 序列化为XML格式显示
                            GL.Pay.YiDong.XmlResult xResultdefault = new GL.Pay.YiDong.XmlResult(dmdefault, dmdefault.GetType());
                            log.Info("yidongFGP移动支付回调 结果：" + xResultdefault);
                            return xResultdefault;

                    }


                    


               

                    GL.Pay.YiDong.response dm = new GL.Pay.YiDong.response { hRet = 0, message = "successful" };
                    GL.Pay.YiDong.XmlResult xResult = new GL.Pay.YiDong.XmlResult(dm, dm.GetType());
                    log.Info("yidongFGP移动支付回调 结果：" + xResult);
                    return xResult;



                }
                else if (hRet == "1" || status != "1800")
                {
                    GL.Pay.YiDong.response dm = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                    // 序列化为XML格式显示
                    GL.Pay.YiDong.XmlResult xResult = new GL.Pay.YiDong.XmlResult(dm, dm.GetType());

                    // return Content(" <? xml version =\"1.0\" encoding=\"UTF - 8\"?>< response >< hRet > 1 </ hRet >< message > failure </ message ></ response > ", "string");
                    log.Info("yidongFGP移动支付回调 结果：" + xResult);
                    return xResult;
                }
                else
                {
                    GL.Pay.YiDong.response dm = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                    // 序列化为XML格式显示
                    GL.Pay.YiDong.XmlResult xResult = new GL.Pay.YiDong.XmlResult(dm, dm.GetType());

                    // return Content(" <? xml version =\"1.0\" encoding=\"UTF - 8\"?>< response >< hRet > 1 </ hRet >< message > failure </ message ></ response > ", "string");
                    log.Info("yidongFGP移动支付回调 结果：" + xResult);
                    return xResult;
                }

            }
            catch (Exception ex)
            {
                log.Info("异常：" + ex.Message);
                Response.Clear();
                GL.Pay.YiDong.response dm = new GL.Pay.YiDong.response { hRet = 1, message = "failure" };

                // 序列化为XML格式显示
                GL.Pay.YiDong.XmlResult xResult = new GL.Pay.YiDong.XmlResult(dm, dm.GetType());


                log.Info("yidongFGP移动支付回调 结果：" + xResult);
                //return Content(" <? xml version =\"1.0\" encoding=\"UTF - 8\"?>< response >< hRet > 1 </ hRet >< message > failure </ message ></ response > ","string");
                return xResult;

            }





        }


        [QueryValues]
        public ActionResult ZYPay(Dictionary<string, string> queryvalues) {
            Response.Clear();
            log.Info("##################" + DateTime.Now.ToString() + "ZYPay卓悠支付回调接口############## ");
            log.Info("ZYPay卓悠支付回调接口 Url: " + Utils.GetUrl());
            log.Info("ZYPay卓悠支付回调接口 queryvalues: " + JsonConvert.SerializeObject(queryvalues));

            if (queryvalues.Count > 0)
            {
                //卓悠订单流水号
                string Recharge_Id = queryvalues.ContainsKey("Recharge_Id") ? queryvalues["Recharge_Id"] : string.Empty;
                //应用Id
                string App_Id = queryvalues.ContainsKey("App_Id") ? queryvalues["App_Id"] : string.Empty;



                //充值用户Id
                string Uin = queryvalues.ContainsKey("Uin") ? queryvalues["Uin"] : string.Empty;
                //CP订单流水号
                string Urecharge_Id = queryvalues.ContainsKey("Urecharge_Id") ? queryvalues["Urecharge_Id"] : string.Empty;
                //附加信息
                string Extra = queryvalues.ContainsKey("Extra") ? queryvalues["Extra"] : string.Empty;
                //订单金额
                string Recharge_Money = queryvalues.ContainsKey("Recharge_Money") ? queryvalues["Recharge_Money"] : string.Empty;
                string Recharge_Gold_Count = queryvalues.ContainsKey("Recharge_Gold_Count") ? queryvalues["Recharge_Gold_Count"] : string.Empty;
                //订单状态：-1取消交易  0未操作 1支付成功  2支付失败
                string Pay_Status = queryvalues.ContainsKey("Pay_Status") ? queryvalues["Pay_Status"] : string.Empty;
                //订单创建时间,时间戳
                string Create_Time = queryvalues.ContainsKey("Create_Time") ? queryvalues["Create_Time"] : string.Empty;
                //对本次所传数据的签名
                string sign = queryvalues.ContainsKey("sign") ? queryvalues["sign"] : string.Empty;





                bool verifyResult = ZYSignCheck.VerifySig(queryvalues, "de75608f7c71ca856436f138880ff0ce");//app_server_key:创建应用时后台获取




                if (!verifyResult) { log.Info(" ZYPay卓悠支付回调接口  Url 回调方法: md5校验失败"); return Content("failure", "string"); }


                if (Pay_Status == "1")
                {
                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = Urecharge_Id.Trim() });
                    if (rc == null)
                    {
                        log.Error(" ZYPay卓悠支付回调接口 订单[" + Urecharge_Id + "]不存在");
                        return Content("failure", "string");
                    }
                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    if (user == null)
                    {
                        log.Error(" ZYPay卓悠支付回调接口 [" + rc.UserID + "]用户不存在" + Utils.GetUrl());
                        return Content("failure", "string");
                    }

                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                    if (iap == null)
                    {
                        log.Error(" ZYPay卓悠支付回调接口 支付配表出错" + Utils.GetUrl());
                        return Content("failure", "string");
                    }

                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;



                    bool firstGif = iF == isFirst.是;


                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }
                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.卓悠 });



                    log.Info(" ZYPay卓悠支付回调接口 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);


                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    uint rmb = (uint)iap.price;
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";

                    log.Info("ZYPay卓悠支付回调接口 user.ID =" + user.ID + ",  gold=" + gold + ",dia=" + dia + ",rmb=" + rmb + ",billno=" + Urecharge_Id);
                    log.Error("ZYPay卓悠支付回调接口 分：" + rmb * 100);
                    RechargeBLL.Add(new Recharge { BillNo = Recharge_Id, OpenID = rc.SerialNo, UserID = user.ID, Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.卓悠, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });

                    normal ServiceNormalS = normal.CreateBuilder()
                    .SetUserID((uint)user.ID)
                    .SetList(list)
                    .SetRmb(rmb * 100)
                    .SetRmbActual(rmb * 100)
                    .SetFirstGif(firstGif)
                    .SetBillNo(Recharge_Id)
                    .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info(" ZYPay卓悠支付回调接口 ServiceResult : " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                RechargeBLL.UpdateReachTime(rc.SerialNo);
                                log.Info(string.Concat(" ZYPay卓悠支付回调接口 ServiceResult[" + Recharge_Id + "] " + ServiceNormalC.Suc));
                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            return Content("success", "string");
                          
                        case CenterCmd.CS_CONNECT_ERROR:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            log.Info(" ZYPay卓悠支付回调接口 ServiceResult : CenterCmd.CS_CONNECT_ERROR");

                            return Content("failure", "string");

                        default:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            log.Info(" ZYPay卓悠支付回调接口 ServiceResult :default");
                            return Content("failure", "string");

                    }


                  

                }
                else
                {
                    log.Info(" ZYPay卓悠支付回调接口 订单失败trade_status:" + Urecharge_Id);
                    return Content("failure", "string");
                }


            }


            return Content("failure", "string");




        }


        [QueryValues]
        public ActionResult YYHPay(Dictionary<string, string> queryvalues) {

            Response.Clear();
            log.Info("##################" + DateTime.Now.ToString() + "YYHPay应用汇支付回调接口############## ");
            log.Info("YYHPay应用汇支付回调接口 Url: " + Utils.GetUrl());
            log.Info("YYHPay应用汇支付回调接口 queryvalues: " + JsonConvert.SerializeObject(queryvalues));






            string jsonStr = "", line;
            string transdata, sign;
            try
            {

                Stream streamResponse = Request.InputStream;

                StreamReader streamRead = new StreamReader(streamResponse, Encoding.UTF8);






                while ((line = streamRead.ReadLine()) != null)
                {
                    jsonStr += line;
                }
                streamResponse.Close();
                streamRead.Close();
                /*
                jsonStr=transdata={"exorderno":"YYHPay20160302185623169","transid":"01416030218562547087","waresid":1,"appid":"5000805433","feetype":0,"money":6,"count":1,"result":0,"transtype":0,"transtime":"2016-03-02 18:57:15","cpprivate":"515娓告垙-60000娓告垙甯?,"paytype":401}&sign=97323cdea977cc5a93c11e4909d976eb 8c6eaad8a7cb66e93d4e3fec004b1267 a7ae55b332fddeabd8029fa23078c639 
                */



                /*
                
                
                
                  transdata = "{\"exorderno\":\"10004200000001100042\",\"transid\":\"02113013118562300203\",\"waresid\":1,\"appid\":\"20004600000001200046\",\"feetype\":0,\"money\":3000,\"count\":1,\"result\":0,\"transtype\":0,\"transtime\":\"2013-01-31 18:57:27\",\"cpprivate\":\"123456\"}";
                   key = "MjhERTEwQkFBRDJBRTRERDhDM0FBNkZBMzNFQ0RFMTFCQTBCQzE3QU1UUTRPRFV6TkRjeU16UTVNRFUyTnpnek9ETXJNVE15T1RRME9EZzROVGsyTVRreU1ETXdNRE0zTnpjd01EazNNekV5T1RJek1qUXlNemN4";
                  sign = "28adee792782d2f723e17ee1ef877e7 166bc3119507f43b06977786376c0434 633cabdb9ee80044bc8108d2e9b3c86e";

                
                */



                // jsonStr = "transdata ={\"exorderno\":\"YYHPay20160302185623169\",\"transid\":\"01416030218562547087\",\"waresid\":1,\"appid\":\"5000805433\",\"feetype\":0,\"money\":6,\"count\":1,\"result\":0,\"transtype\":0,\"transtime\":\"2016-03-02 18:57:15\",\"cpprivate\":\"515娓告垙-60000娓告垙甯\",paytype\":401}&sign=97323cdea977cc5a93c11e4909d976eb 8c6eaad8a7cb66e93d4e3fec004b1267 a7ae55b332fddeabd8029fa23078c639";
                log.Error(" YYHPay应用汇支付回调接口 jsonStr=" + jsonStr);


                //jsonStr = "transdata ={ \"exorderno\":\"YYHPay20160302194522756\",\"transid\":\"01416030219452647215\",\"waresid\":1,\"appid\":\"5000805433\",\"feetype\":0,\"money\":6,\"count\":1,\"result\":0,\"transtype\":0,\"transtime\":\"2016-03-02 19:45:51\",\"cpprivate\":\"515游戏-60000游戏币\",\"paytype\":401}&sign = 7a532a31a1834b57499c4724939f1a16 5c5b1e091f08b714ead4ebf1a9915ae3 2cc8204e3d228da1aea4dace9bc5fdab";






                string[] content = jsonStr.Split('&');
                if (content.Length != 2)
                {
                    log.Error(" YYHPay应用汇支付回调接口 jsonStr参数长度不够2个");
                    return Content("FAILD", "string");
                }


                transdata = content[0].Replace("transdata =", "").Replace("transdata=", "");
                sign = content[1].Replace("sign=", "").Replace("sign =", "");

            }
            catch (Exception ex)
            {
                log.Error(" YYHPay应用汇支付回调接口 异常:" + ex.Message);
                return Content("FAILD", "string");

            }

            /*

             $trans_data = '{"exorderno":"10004200000001100042","transid":"02113013118562300203","waresid":1,"appid":"20004600000001200046","feetype":0,"money":3000,"count":1,"result":0,"transtype":0,"transtime":"2013-01-31 18:57:27","cpprivate":"123456"}';
 $key = 'MjhERTEwQkFBRDJBRTRERDhDM0FBNkZBMzNFQ0RFMTFCQTBCQzE3QU1UUTRPRFV6TkRjeU16UTVNRFUyTnpnek9ETXJNVE15T1RRME9EZzROVGsyTVRreU1ETXdNRE0zTnpjd01EazNNekV5T1RJek1qUXlNemN4';
 $sign = '28adee792782d2f723e17ee1ef877e7 166bc3119507f43b06977786376c0434 633cabdb9ee80044bc8108d2e9b3c86e';

             */

            // log.Info(" transdata:" + transdata);
            //log.Info(" sign:" + sign);

            bool rebool = YYHSignCheck.validSign(transdata.Trim(), sign.Trim(), "OEQxODM1M0Q2QjMwNDAzOUYwRDJEMkE4OUVEMjIyMjgwMUU3QzY1RE1UYzJORFkyT0RJMU1UWXdNVGd5TXpnNU16TXJNekU1T0RrNE9ETTRPRGcyT0RnMk5EY3pOVEUyTXprd01UYzBOekV4TnpVMU5UUTRNakU1");




            if (!rebool)
            {
                log.Error("YYHPay应用汇支付回调接口 签名不通过");
                return Content("FAILD", "string");
            }
            else
            {

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                TransData transdata_obj = jsonSerializer.Deserialize<TransData>(transdata);
                log.Info("YYHPay应用汇支付回调接口 transdata_obj= " + JsonConvert.SerializeObject(transdata_obj));
                if (transdata_obj.result == 0)
                {
                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = transdata_obj.exorderno });
                    if (rc == null)
                    {
                        log.Error(" YYHPay应用汇支付回调接口 订单[" + transdata_obj.exorderno + "]不存在");
                        return Content("FAILD", "string");
                    }
                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    if (user == null)
                    {
                        log.Error(" YYHPay应用汇支付回调接口 [" + rc.UserID + "]用户不存在" + Utils.GetUrl());
                        return Content("FAILD", "string");
                    }
                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                    if (iap == null)
                    {
                        log.Error(" YYHPay应用汇支付回调接口 支付配表出错" + Utils.GetUrl());
                        return Content("FAILD", "string");
                    }

                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;



                    bool firstGif = iF == isFirst.是;



                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }
                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = transdata_obj.exorderno, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.应用汇 });


                    log.Info(" YYHPay应用汇支付回调接口 订单号= " + transdata_obj.exorderno + ",用户ID:  " + rc.UserID);


                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    uint rmb = (uint)iap.price;
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";


                    log.Info("YYHPay应用汇支付回调接口  user.ID =" + user.ID + ",  gold=" + gold + ",dia=" + dia + ",rmb=" + rmb + ",billno=" + transdata_obj.exorderno);

                    log.Error("YYHPay应用汇支付回调接口 分：" + rmb * 100);

                    RechargeBLL.Add(new Recharge { BillNo = transdata_obj.transid, OpenID = transdata_obj.exorderno, UserID = user.ID, Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.应用汇, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });

                    normal ServiceNormalS = normal.CreateBuilder()
                    .SetUserID((uint)user.ID)
                    .SetList(list)
                    .SetRmb(rmb * 100)
                      .SetRmbActual(rmb * 100)
                    .SetFirstGif(firstGif)
                    .SetBillNo(transdata_obj.transid)
                    .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info(" YYHPay应用汇支付回调接口 ServiceResult : " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                RechargeBLL.UpdateReachTime(transdata_obj.exorderno);
                                log.Info(string.Concat(" YYHPay应用汇支付回调接口 ServiceResult[" + transdata_obj.transid + "] " + ServiceNormalC.Suc));
                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = transdata_obj.exorderno });
                            return Content("SUCCESS", "string");

                        case CenterCmd.CS_CONNECT_ERROR:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = transdata_obj.exorderno });
                            log.Info(" YYHPay应用汇支付回调接口 ServiceResult : CenterCmd.CS_CONNECT_ERROR");

                            return Content("FAILD", "string");

                        default:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = transdata_obj.exorderno });
                            log.Info(" YYHPay应用汇支付回调接口 ServiceResult :default");
                            return Content("FAILD", "string");

                    }



                 
                

                }
                else {
                    return Content("FAILD", "string");
                }




            }


        }

        [QueryValues]
        public ActionResult MZPay(Dictionary<string, string> queryvalues)
        {
            Response.Clear();
            log.Info("##################" + DateTime.Now.ToString() + "MZPay魅族支付回调接口############## ");
            log.Info("MZPay魅族支付回调接口 Url: " + Utils.GetUrl());
            log.Info("MZPay魅族支付回调接口 queryvalues: " + JsonConvert.SerializeObject(queryvalues));

            if (queryvalues.Count > 0)
            {
                //通知的发送时间
                string notify_time = queryvalues.ContainsKey("notify_time") ? queryvalues["notify_time"] : string.Empty;
                //通知id
                string notify_id = queryvalues.ContainsKey("notify_id") ? queryvalues["notify_id"] : string.Empty;
                //订单id
                string order_id = queryvalues.ContainsKey("order_id") ? queryvalues["order_id"] : string.Empty;
                //应用id
                string app_id = queryvalues.ContainsKey("app_id") ? queryvalues["app_id"] : string.Empty;
                //用户id
                string uid = queryvalues.ContainsKey("uid") ? queryvalues["uid"] : string.Empty;
                //商户id
                string partner_id = queryvalues.ContainsKey("partner_id") ? queryvalues["partner_id"] : string.Empty;
                //游戏订单id
                string cp_order_id = queryvalues.ContainsKey("cp_order_id") ? queryvalues["cp_order_id"] : string.Empty;
                //产品id
                string product_id = queryvalues.ContainsKey("product_id") ? queryvalues["product_id"] : string.Empty;
                //产品单位
                string product_unit = queryvalues.ContainsKey("product_unit") ? queryvalues["product_unit"] : string.Empty;
                //购买数量
                string buy_amount = queryvalues.ContainsKey("buy_amount") ? queryvalues["buy_amount"] : string.Empty;
                //产品单价
                string product_per_price = queryvalues.ContainsKey("product_per_price") ? queryvalues["product_per_price"] : string.Empty;
                //购买总价
                string total_price = queryvalues.ContainsKey("total_price") ? queryvalues["total_price"] : string.Empty;
                //交易状态：1待支付  2支付中 3已支付  4取消订单 5位置异常取消订单
                string trade_status = queryvalues.ContainsKey("trade_status") ? queryvalues["trade_status"] : string.Empty;
                //订单时间
                string create_time = queryvalues.ContainsKey("create_time") ? queryvalues["create_time"] : string.Empty;
                //支付时间
                string pay_time = queryvalues.ContainsKey("pay_time") ? queryvalues["pay_time"] : string.Empty;
                //用户自定义信息
                string user_info = queryvalues.ContainsKey("user_info") ? queryvalues["user_info"] : string.Empty;
                //参数签名
                string sign = queryvalues.ContainsKey("sign") ? queryvalues["sign"] : string.Empty;
                //签名类型,常量md5
                string sign_type = queryvalues.ContainsKey("sign_type") ? queryvalues["sign_type"] : string.Empty;


                bool verifyResult = MeiZuSignCheck.VerifySig(queryvalues, "Qx2Yhspw5UNJQ7FLa3ieZScYjGl6GO1b");//app_server_key:创建应用时后台获取




                if (!verifyResult) {
                    log.Info(" MZPay魅族支付回调接口  Url 回调方法: md5校验失败");
                    return Json(new
                    {
                        code = 900000,
                        message = "md5校验失败",
                        redirect = "",
                        value = ""
                    }, JsonRequestBehavior.AllowGet);

                }


                if (trade_status == "3")
                {
                    string[] strs = user_info.Split('.');
                    string myNo = strs[0];
                    string myUid = strs[1];


                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = myNo.Trim() });
                    if (rc == null)
                    {
                        log.Error(" MZPay魅族支付回调接口 订单[" + user_info + "]不存在");
                        return Json(new
                        {
                            code = 900000,
                            message = "订单不存在",
                            redirect = "",
                            value = ""
                        }, JsonRequestBehavior.AllowGet);
                    }
                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    if (user == null)
                    {
                        log.Error(" MZPay魅族支付回调接口 [" + rc.UserID + "]用户不存在" + Utils.GetUrl());
                        return Json(new
                        {
                            code = 900000,
                            message = "用户不存在",
                            redirect = "",
                            value = ""
                        }, JsonRequestBehavior.AllowGet);
                    }

                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                    if (iap == null)
                    {
                        log.Error(" MZPay魅族支付回调接口 支付配表出错" + Utils.GetUrl());
                        return Json(new
                        {
                            code = 900000,
                            message = "MZPay 支付配表出错",
                            redirect = "",
                            value = ""
                        }, JsonRequestBehavior.AllowGet);
                    }

                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;



                    bool firstGif = iF == isFirst.是;


                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }
                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.魅族 });


                    log.Info(" MZPay魅族支付回调接口 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);


                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    uint rmb = (uint)iap.price;
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";

                    log.Info(" MZPay魅族支付回调接口 user.ID =" + user.ID + ",  gold=" + gold + ",dia=" + dia + ",rmb=" + rmb + ",billno=" + order_id);
                    log.Error("MZPay魅族支付回调接口 分：" + rmb * 100);
                    RechargeBLL.Add(new Recharge { BillNo = order_id, OpenID = rc.SerialNo, UserID = user.ID, Money = (long)rmb * 100, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.魅族, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });

                    normal ServiceNormalS = normal.CreateBuilder()
                    .SetUserID((uint)user.ID)
                    .SetList(list)
                    .SetRmb(rmb * 100)
                      .SetRmbActual(rmb * 100)
                    .SetFirstGif(firstGif)
                    .SetBillNo(order_id)
                    .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info(" MZPay魅族支付回调接口 ServiceResult : " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                RechargeBLL.UpdateReachTime(rc.SerialNo);
                                log.Info(string.Concat(" MZPay魅族支付回调接口 ServiceResult[" + order_id + "] " + ServiceNormalC.Suc));
                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            break;
                        case CenterCmd.CS_CONNECT_ERROR:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            log.Info(" MZPay魅族支付回调接口 ServiceResult : CenterCmd.CS_CONNECT_ERROR");

                            return Json(new
                            {
                                code = 900000,
                                message = "MZPay 链接服务器失败",
                                redirect = "",
                                value = ""
                            }, JsonRequestBehavior.AllowGet);

                        default:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            log.Info(" MZPay魅族支付回调接口 ServiceResult :default");
                            return Json(new
                            {
                                code = 900000,
                                message = "MZPay魅族支付回调接口 ServiceResult :default ",
                                redirect = "",
                                value = ""
                            }, JsonRequestBehavior.AllowGet);

                    }
                  
                  
                    return Json(new
                    {
                        code = 200,
                        message = "成功",
                        redirect = "",
                        value = ""
                    }, JsonRequestBehavior.AllowGet);



                }
                else
                {
                    log.Info(" MZPay魅族支付回调接口 订单失败trade_status:" + order_id);
                    return Json(new
                    {
                        code = 900000,
                        message = "MZPay 订单失败trade_status ",
                        redirect = "",
                        value = ""
                    }, JsonRequestBehavior.AllowGet);
                }


            }

            log.Info(" MZPay魅族支付回调接口 参数不正确");
            return Json(new
            {
                code = 900000,
                message = "MZPay 参数不正确",
                redirect = "",
                value = ""
            }, JsonRequestBehavior.AllowGet);




        }


        /// <summary>
        /// 华为回调接口
        /// </summary>
        /// <param name="queryvalues"></param>
        /// <returns></returns>
        [QueryValues]
        public ActionResult HuaWei(Dictionary<string, string> queryvalues)
        {
            log.Info("##################" + DateTime.Now.ToString() + "HuaWei华为回调接口############## ");
            log.Info("HuaWei华为回调接口 Url: " + Utils.GetUrl());
            log.Info("HuaWei华为回调接口 queryvalues: " + JsonConvert.SerializeObject(queryvalues));

            if (queryvalues.Count > 0)//判断是否有带返回参数
            {

                try
                {
                    //开始校验签名 

                    //
                    string sercet = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkVqw1DaYlkhBXfWwvNUe4S/k8b6YCfjdng6xmC8ZYbfxKyzgp7vRxAnjyx03iOVwOHCv5RuYWSSX+sUAuEPmdnbqW6wls1G2gU0W3ddpgCC6kOKzK0qr1N/xMG9bDQvEM7qUeoqYSUGs07Su8vH2rp21Qo6saUi45J4hgdFUy63Bi/OPQdN/4d7kvOyWkr1fO6da+vmQ+dWcTnz3E3O3QT1IL6GACDuMgZaVgfrvO8++913ucopjbo1Cc87C3mMsZACb8WpjFNC1a2PRwe/tDyg7q7ccmRgNU1ZW8h8Jw2tu64xNjgF03ya+tzeJGaiu3LbZhAtZpAiABtiEUTmptwIDAQAB";
#if Debug
                    sercet = "MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAIW1g+KAqqOeC1ypte8L3qTDk2nz6jUbM6o6Jg9obvivPnCAm/wZvV3jWbYWfOuO/wrFJygn/jZqf8cR1T1CQa8CAwEAAQ==";
#endif
#if P17
           sercet = "MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAIW1g+KAqqOeC1ypte8L3qTDk2nz6jUbM6o6Jg9obvivPnCAm/wZvV3jWbYWfOuO/wrFJygn/jZqf8cR1T1CQa8CAwEAAQ==";
#endif
#if Test
        //    sercet = "MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAIW1g+KAqqOeC1ypte8L3qTDk2nz6jUbM6o6Jg9obvivPnCAm/wZvV3jWbYWfOuO/wrFJygn/jZqf8cR1T1CQa8CAwEAAQ==";
#endif

                    bool res = HuaWeiSigCheck.VerifySig(queryvalues, sercet);
                    if (res == false) { //签名错误


                        return Json(new
                        {
                            result = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    string result = queryvalues.ContainsKey("result") ? queryvalues["result"] : string.Empty;
                    if (result != "0") {//支付未成功
                        return Json(new
                        {
                            result = 3
                        }, JsonRequestBehavior.AllowGet);
                    }


                  //  "accessMode=0&amount=6.00&notifyTime=1465984111579&orderId=Y2016061517480919646967539&orderTime=2016-06-15 17:48:09&payType=16&productName=515游戏-60五币&requestId=HuaWeiPay20160615174747439&result=0&spending=0.00&tradeTime=2016-06-15 17:48:11&userName=890086000001008842"



                    //我们的订单id
                    string requestId = queryvalues.ContainsKey("requestId") ? queryvalues["requestId"] : string.Empty;
                    //华为订单号
                    string orderId = queryvalues.ContainsKey("orderId") ? queryvalues["orderId"] : string.Empty;
                    //商品支付金额
                    decimal amount = queryvalues.ContainsKey("amount") ? Convert.ToDecimal(queryvalues["amount"]) : 0;
                    if (amount <= 0)
                    {
                        return Json(new  //金额小于等于0，出错
                        {
                            result = 3
                        }, JsonRequestBehavior.AllowGet);
                    }
                    RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = requestId.Trim() });
                    if (rc == null)
                    {
                        return Json(new
                        {
                            result = 3
                        }, JsonRequestBehavior.AllowGet);
                    }
                    Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
                    if (user == null)
                    {
                        log.Error(" HuaWei华为回调接口支付 [" + rc.UserID + "]用户不存在" + Utils.GetUrl());
                        Response.Clear();
                        return Json(new  //用户不存在
                        {
                            result = 3
                        }, JsonRequestBehavior.AllowGet);
                    }
                    IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                    if (iap == null)
                    {
                        log.Error(" HuaWei华为回调接口 支付配表出错" + Utils.GetUrl());
                        Response.Clear();
                        return Json(new  //支付配表出错
                        {
                            result = 3
                        }, JsonRequestBehavior.AllowGet);
                    }

                    isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge") ? isFirst.是 : isFirst.否;
                    chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;
                    bool firstGif = iF == isFirst.是;
                    Recharge recharge = RechargeBLL.GetFirstModelListByUserID(new Recharge { UserID = rc.UserID });
                    if (recharge != null)//首冲过
                    {
                        if (firstGif == true)
                        {//又是首冲，则改变其为不首冲
                            firstGif = false;
                            ct = (chipType)1;
                            iF = isFirst.否;
                            iap.goodsType = 1;
                            log.Error(" 第多次首冲，修改firstGif值");
                        }
                    }
                    RechargeIP ipmodel = GetClientIp();
                    RechargeCheckBLL.AddCallBackOrderIP(new UserIpInfo { CallBackUserID = rc.UserID, OrderID = rc.SerialNo, Method = ipmodel.Method, CallBackIP = ipmodel.IP, CallBackTime = DateTime.Now, CallBackChargeType = (int)raType.华为 });

                    log.Info(" HuaWei华为回调接口 订单号= " + rc.SerialNo + ",用户ID:  " + rc.UserID);



                    //开始发货
                    uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                    uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;
                    uint rmb = (uint)iap.price;//元
                    if (firstGif)
                    {
                        gold = (uint)(iap.goods + iap.attach_chip);
                        dia = (uint)iap.attach_5b;
                    }
                    string list = iap.attach_props;
                    list = list.Trim(',') + (gold > 0 ? ",10000|" + gold + "," : "");
                    list = list.Trim(',') + (dia > 0 ? ",20000|" + dia + "," : "");
                    list = list.Trim(',') + ",";
                    log.Info("HuaWei华为回调接口 user.ID =" + user.ID + ",  gold=" + gold + ",dia=" + dia + ",rmb=" + rmb + ",billno=" + requestId);
                    log.Error("HuaWei华为回调接口 rmb测试分：" + rmb * 100);
                    log.Error("HuaWei华为回调接口 amount测试分：" + rmb * 100);
                    //检查打折问题
                    uint oldrmb = rmb*100;
                    if (amount > 0)
                    {
                        if (amount < rmb)
                        {
                            rmb = (uint)amount * 100;
                        }
                        else
                        {
                            rmb = rmb * 100;
                        }
                    }
                    else {
                        rmb = rmb * 100;
                    }
                    log.Error("HuaWei华为回调接口 rmb测试分：" + rmb );
                    RechargeBLL.Add(new Recharge { BillNo = requestId, OpenID = orderId, UserID = user.ID, Money = (long)rmb, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.华为, UserAccount = user.NickName, ActualMoney = Convert.ToInt64(iap.price * 100), ProductNO = list.Trim(','),AgentID=rc.AgentID });

                    normal ServiceNormalS = normal.CreateBuilder()
                   .SetUserID((uint)user.ID)
                   .SetList(list)
                   .SetRmb(rmb)
                     .SetRmbActual(oldrmb)
                   .SetFirstGif(firstGif)
                   .SetBillNo(requestId)
                   .Build();

                    Bind tbind = Cmd.runClient(new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()));
                    switch ((CenterCmd)tbind.header.CommandID)
                    {
                        case CenterCmd.CS_NORMAL:
                            normalRep ServiceNormalC = normalRep.ParseFrom(tbind.body.ToBytes());
                            log.Info(" HuaWei华为回调接口 ServiceResult : " + ServiceNormalC.Suc);
                            if (ServiceNormalC.Suc)
                            {
                                RechargeBLL.UpdateReachTime(rc.SerialNo);
                                log.Info(string.Concat(" HuaWei华为回调接口 ServiceResult[" + requestId + "] " + ServiceNormalC.Suc));
                            }
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });
                            return Json(new
                            {
                                result = 0
                            }, JsonRequestBehavior.AllowGet);

                        case CenterCmd.CS_CONNECT_ERROR:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });


                            log.Info(" HuaWei华为回调接口 ServiceResult : CenterCmd.CS_CONNECT_ERROR");

                            return Json(new  //CS_CONNECT_ERROR
                            {
                                result = 99
                            }, JsonRequestBehavior.AllowGet);

                        default:
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = rc.SerialNo });


                            log.Info(" HuaWei华为回调接口 ServiceResult :default");
                            return Json(new
                            {
                                result = 99
                            }, JsonRequestBehavior.AllowGet);

                    }

                   


                }
                catch (Exception err)
                {
                    log.Error(" HuaWei华为回调接口 异常" + Utils.GetUrl(), err);
                    return Json(new
                    {
                        result = 94
                    }, JsonRequestBehavior.AllowGet);

                }


            }
            else
            {

                log.Error(" HuaWei华为回调接口 无参数: " + Utils.GetUrl());
                return Json(new
                {
                    result = 98
                }, JsonRequestBehavior.AllowGet);
            }
        }


       

       

        [QueryValues]
        public ActionResult TestPay5(Dictionary<string, string> queryvalues)
        {
            bool res = false;
#if Debug
            res = true;

#endif
#if P17
            res = true;
#endif
#if Test
           res = true;
#endif
            if (res == false)
            {
                return Content("-1");
            }
            string billNO = Utils.GenerateOutTradeNo("HYTest");
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : 1;
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : 1;

            //1 qian  2五币
            uint gold = 0;
            uint dia = 0;
            string t = "";
            chipType c = chipType.金币;
            if (type == 1)
            {
                gold = 60000;
                dia = 0;
                t = "Chip_8";
                c = chipType.金币;
            }
            else
            {
                gold = 0;
                dia = 60;
                t = "5b_7";
                c = chipType.钻石;
            }
           

            string list = "10000|98000,20000|10,60400|1,60410|1,";
            RechargeBLL.Add(new Recharge { BillNo = billNO, OpenID = billNO, UserID = userid, Money = 480, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = c, IsFirst = isFirst.是, NickName = t, PayItem = t, PF = raType.华为, UserAccount = "", ActualMoney =600, ProductNO = list.Trim(',') });

            // 
           
        
         
            
           normal ServiceNormalS = normal.CreateBuilder()
                .SetUserID((uint)userid)
                .SetList(list)
                .SetRmb(600)
                .SetRmbActual(600)
                .SetFirstGif(true)
                .SetBillNo(billNO)
                .Build();
           
              Bind tbind = Cmd.runClient(
                new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()),"192.168.1.60");
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_NORMAL:
                    normalRep ServiceNormalC =
                       normalRep.ParseFrom(tbind.body.ToBytes());
                 
                    if (ServiceNormalC.Suc)
                    {

                        RechargeBLL.UpdateReachTime(billNO);

                        return Content("0");
                    }
                    //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                    break;
                case CenterCmd.CS_CONNECT_ERROR:

                    return Content("3");


                default:
                    return Content("4");


            }


            return Content("1");



        }


        [QueryValues]
        public ActionResult TestPay6(Dictionary<string, string> queryvalues)
        {
            bool res = false;
#if Debug
            res = true;

#endif
#if P17
            res = true;
#endif
#if Test
           res = true;
#endif
            if (res == false)
            {
                return Content("-1");
            }
            string billNO = Utils.GenerateOutTradeNo("HYTest");
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : 1;
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : 1;
            //1 qian  2五币
            uint gold = 0;
            uint dia = 0;
            string t = "";
            chipType c = chipType.金币;
            if (type == 1)
            {
                gold = 60000;
                dia = 0;
                t = "Chip_8";
                c = chipType.金币;
            }
            else
            {
                gold = 0;
                dia = 60;
                t = "5b_7";
                c = chipType.钻石;
            }


            string list = "10000|60000,";
            RechargeBLL.Add(new Recharge { BillNo = billNO, OpenID = billNO, UserID = userid, Money = 480, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = c, IsFirst = isFirst.否, NickName = t, PayItem = t, PF = raType.华为, UserAccount = "", ActualMoney = 600, ProductNO = list.Trim(',') });

            // 




            normal ServiceNormalS = normal.CreateBuilder()
                 .SetUserID((uint)userid)
                 .SetList(list)
                 .SetRmb(600)
                 .SetRmbActual(600)
                 .SetFirstGif(false)
                 .SetBillNo(billNO)
                 .Build();

            Bind tbind = Cmd.runClient(
              new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()), "192.168.1.60");
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_NORMAL:
                    normalRep ServiceNormalC =
                       normalRep.ParseFrom(tbind.body.ToBytes());

                    if (ServiceNormalC.Suc)
                    {
                        RechargeBLL.UpdateReachTime(billNO);

                        return Content("0");
                    }
                    //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                    break;
                case CenterCmd.CS_CONNECT_ERROR:

                    return Content("3");


                default:
                    return Content("4");


            }


            return Content("1");



        }




        [QueryValues]
        public ActionResult TestPay7(Dictionary<string, string> queryvalues)
        {
            bool res = false;
#if Debug
            res = true;

#endif
#if P17
            res = true;
#endif
#if Test
           res = true;
#endif
            if (res == false)
            {
                return Content("-1");
            }
            string billNO = Utils.GenerateOutTradeNo("HYTest");
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : 1;
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : 1;

            //1 qian  2五币
            uint gold = 0;
            uint dia = 0;
            string t = "";
            chipType c = chipType.金币;
            if (type == 1)
            {
                gold = 60000;
                dia = 0;
                t = "Chip_8";
                c = chipType.金币;
            }
            else
            {
                gold = 0;
                dia = 60;
                t = "5b_7";
                c = chipType.钻石;
            }


            string list = "10000|98000,20000|10,60400|1,60410|1,";
            RechargeBLL.Add(new Recharge { BillNo = billNO, OpenID = billNO, UserID = userid, Money = 480, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = c, IsFirst = isFirst.是, NickName = t, PayItem = t, PF = raType.华为, UserAccount = "", ActualMoney = 600, ProductNO = list.Trim(',') });

            // 




            normal ServiceNormalS = normal.CreateBuilder()
                 .SetUserID((uint)userid)
                 .SetList(list)
                 .SetRmb(480)
                 .SetRmbActual(600)
                 .SetFirstGif(true)
                 .SetBillNo(billNO)
                 .Build();

            Bind tbind = Cmd.runClient(
              new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()), "192.168.1.30");
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_NORMAL:
                    normalRep ServiceNormalC =
                       normalRep.ParseFrom(tbind.body.ToBytes());

                    if (ServiceNormalC.Suc)
                    {

                        RechargeBLL.UpdateReachTime(billNO);

                        return Content("0");
                    }
                    //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                    break;
                case CenterCmd.CS_CONNECT_ERROR:

                    return Content("3");


                default:
                    return Content("4");


            }


            return Content("1");



        }


        [QueryValues]
        public ActionResult TestPay8(Dictionary<string, string> queryvalues)
        {
            bool res = false;
#if Debug
            res = true;

#endif
#if P17
            res = true;
#endif
#if Test
           res = true;
#endif
            if (res == false)
            {
                return Content("-1");
            }
            string billNO = Utils.GenerateOutTradeNo("HYTest");
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : 1;
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : 1;
            //1 qian  2五币
            uint gold = 0;
            uint dia = 0;
            string t = "";
            chipType c = chipType.金币;
            if (type == 1)
            {
                gold = 60000;
                dia = 0;
                t = "Chip_8";
                c = chipType.金币;
            }
            else
            {
                gold = 0;
                dia = 60;
                t = "5b_7";
                c = chipType.钻石;
            }


            string list = "10000|60000,";
            RechargeBLL.Add(new Recharge { BillNo = billNO, OpenID = billNO, UserID = userid, Money = 480, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = c, IsFirst = isFirst.否, NickName = t, PayItem = t, PF = raType.华为, UserAccount = "", ActualMoney = 600, ProductNO = list.Trim(',') });

            // 




            normal ServiceNormalS = normal.CreateBuilder()
                 .SetUserID((uint)userid)
                 .SetList(list)
                 .SetRmb(480)
                 .SetRmbActual(600)
                 .SetFirstGif(false)
                 .SetBillNo(billNO)
                 .Build();

            Bind tbind = Cmd.runClient(
              new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()), "192.168.1.30");
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_NORMAL:
                    normalRep ServiceNormalC =
                       normalRep.ParseFrom(tbind.body.ToBytes());

                    if (ServiceNormalC.Suc)
                    {
                        RechargeBLL.UpdateReachTime(billNO);

                        return Content("0");
                    }
                    //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                    break;
                case CenterCmd.CS_CONNECT_ERROR:

                    return Content("3");


                default:
                    return Content("4");


            }


            return Content("1");



        }

        [QueryValues]
        public ActionResult TestPay9(Dictionary<string, string> queryvalues)
        {
            bool res = false;
#if Debug
            res = true;

#endif
#if P17
            res = true;
#endif
#if Test
           res = true;
#endif
            if (res == false)
            {
                return Content("-1");
            }
            string billNO = Utils.GenerateOutTradeNo("HYTest");
            int userid = queryvalues.ContainsKey("userid") ? Convert.ToInt32(queryvalues["userid"]) : 1;
            int type = queryvalues.ContainsKey("type") ? Convert.ToInt32(queryvalues["type"]) : 1;
            //1 qian  2五币
            uint gold = 0;
            uint dia = 0;
            string t = "";
            chipType c = chipType.金币;
            if (type == 1)
            {
                gold = 60000;
                dia = 0;
                t = "Chip_8";
                c = chipType.金币;
            }
            else
            {
                gold = 0;
                dia = 60;
                t = "5b_7";
                c = chipType.钻石;
            }


            string list = "10000|3000000,";
            RechargeBLL.Add(new Recharge { BillNo = billNO, OpenID = billNO, UserID = userid, Money = 600, CreateTime = DateTime.Now, Chip = gold, Diamond = dia, ChipType = c, IsFirst = isFirst.否, NickName = t, PayItem = t, PF = raType.华为, UserAccount = "", ActualMoney = 600, ProductNO = list.Trim(',') });

            // 




            normal ServiceNormalS = normal.CreateBuilder()
                 .SetUserID((uint)userid)
                 .SetList(list)
                 .SetRmb(24000)//打折后的总价
                 .SetRmbActual(30000)//没打折的总价
                 .SetFirstGif(false)
                 .SetNum(10)
                 .SetUnitPrice(3000) //没打折单价30
                 .SetUnitDiscounted(2400)//打折的单价24
                 .SetBillNo(billNO)
                 .Build();

            Bind tbind = Cmd.runClient(
              new Bind(BR_Cmd.BR_NORMAL, ServiceNormalS.ToByteArray()), "192.168.1.60");
            switch ((CenterCmd)tbind.header.CommandID)
            {
                case CenterCmd.CS_NORMAL:
                    normalRep ServiceNormalC =
                       normalRep.ParseFrom(tbind.body.ToBytes());

                    if (ServiceNormalC.Suc)
                    {
                        RechargeBLL.UpdateReachTime(billNO);

                        return Content("0");
                    }
                    //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                    break;
                case CenterCmd.CS_CONNECT_ERROR:

                    return Content("3");


                default:
                    return Content("4");


            }


            return Content("1");



        }


    }






}

