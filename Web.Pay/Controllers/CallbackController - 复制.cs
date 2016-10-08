 using GL.Common;
using GL.Data.BLL;
using GL.Data.Model;
using GL.Pay.Data.BLL;
using GL.Pay.Data.Model;
using GL.Pay.YeePay;
using GL.Protocol;
using ProtoCmd.BackRecharge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Pay.JsonMapper;
using Web.Pay.Models;
using Web.Pay.protobuf.SCmd;

namespace Web.Pay.Controllers
{
    public class CallbackController : Controller
    {
       
        [QueryValues]
        public ActionResult YeePay(Dictionary<string, string> queryvalues)
        {
            string _data = queryvalues.ContainsKey("data") ? queryvalues["data"] : string.Empty;
            string _encryptkey = queryvalues.ContainsKey("encryptkey") ? queryvalues["encryptkey"] : string.Empty;

            if (_data == string.Empty || _encryptkey == string.Empty)
            {
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
                RechargeCheck rc = RechargeCheckBLL.GetModelBySerialNo(new RechargeCheck { SerialNo = m.orderid });
#if Debug
                Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
#endif

#if P17
                Role user = RoleBLL.GetModelByID(new Role{ ID = rc.UserID});
#endif
#if Release
                Role user = RoleBLL.GetModelByID(new Role{ ID = rc.UserID});
#endif
                IAPProduct iap = IAPProductBLL.GetModelByID(rc.ProductID);
                isFirst iF = iap.product_id.Split('_')[0].Equals("firstCharge")?isFirst.是:isFirst.否;
                chipType ct = iF == isFirst.是 ? chipType.首冲礼包 : (chipType)iap.goodsType;





                bool firstGif = iF == isFirst.是;

                uint gold = iap.goodsType == 1 ? (uint)iap.goods : 0;
                uint dia = iap.goodsType == 2 ? (uint)iap.goods : 0;

                if (firstGif)
                {
                    gold = (uint)(iap.goods + iap.attach_chip);
                    dia = (uint)iap.attach_5b;
                }

                uint rmb = (uint)(rc.Money /100);

                normal ServiceNormalS = normal.CreateBuilder()
                .SetUserID((uint)rc.UserID)
                .SetGold(gold)
                .SetDia(dia)
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

                            RechargeBLL.Add(new Recharge { BillNo = m.yborderid, OpenID = rc.SerialNo, UserID = rc.UserID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.易宝, UserAccount = user.NickName });

                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = m.orderid });



                            //Response.Redirect("mobilecall://success");
                            return RedirectToAction("success","Home");
                        }
                        //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                        break;
                    case CenterCmd.CS_CONNECT_ERROR:
                        break;
                }



                //Response.Redirect("mobilecall://fail");
                return RedirectToAction("fail", "Home");
            }
            catch (Exception err)
            {
                //Response.Redirect("mobilecall://fail?err=" + err);
                return RedirectToAction("fail", "Home");
                //return Content("支付失败!" + err);
            }
        }
        [QueryValues]
        public ActionResult fYeePay(Dictionary<string, string> queryvalues)
        {
            string _data = queryvalues.ContainsKey("data") ? queryvalues["data"] : string.Empty;
            string _encryptkey = queryvalues.ContainsKey("encryptkey") ? queryvalues["encryptkey"] : string.Empty;

            if (_data == string.Empty || _encryptkey == string.Empty)
            {
                Response.Redirect("mobilecall://fail");
                return Content("参数不正确!");
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
#if Debug
                Role user = RoleBLL.GetModelByID(new Role { ID = rc.UserID });
#endif

#if P17
                Role user = RoleBLL.GetModelByID(new Role{ ID = rc.UserID});
#endif
#if Release
                Role user = RoleBLL.GetModelByID(new Role{ ID = rc.UserID});
#endif
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

                uint rmb = (uint)(rc.Money / 100);

                normal ServiceNormalS = normal.CreateBuilder()
                .SetUserID((uint)rc.UserID)
                .SetGold(gold)
                .SetDia(dia)
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

                            RechargeBLL.Add(new Recharge { BillNo = m.yborderid, OpenID = rc.SerialNo, UserID = rc.UserID, Money = rc.Money, CreateTime = DateTime.Now, Chip = gold, ChipType = ct, IsFirst = iF, NickName = iap.productname, PayItem = iap.product_id, PF = raType.易宝, UserAccount = user.NickName });
                            RechargeCheckBLL.Delete(new RechargeCheck { SerialNo = m.orderid });
                            //Response.Redirect("mobilecall://success");
                            return Content("success");
                        }
                        //Response.Redirect("mobilecall://fail?suc=" + ServiceNormalC.Suc);
                        break;
                    case CenterCmd.CS_CONNECT_ERROR:
                        break;
                }



                //Response.Redirect("mobilecall://fail");
                return Content("支付失败!");
            }
            catch (Exception err)
            {
                //Response.Redirect("mobilecall://fail?err=" + err);
                return Content("支付失败!" + err);
            }
        }


        
    }
}