using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GL.Pay.Unicom
{
    public class UnicomPay
    {

        public Controller page { get; set; }
        public UnicomPay(Controller page)
        {
            this.page = page;
        }


        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public UnicomPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = page.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            Log.Info(this.GetType().ToString(), "Receive data from Unicom : " + builder.ToString());

            //转换数据格式并验证签名
            UnicomPayData data = new UnicomPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (Exception ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                UnicomPayData res = new UnicomPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Log.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                return res;
            }

            Log.Info(this.GetType().ToString(), "Check sign success");


            data.SetValue("return_code", "SUCCESS");
            return data;
        }

        public UnicomPayData CheckNotifyData()
        {
          

            //接收从微信后台POST过来的数据
            System.IO.Stream s = page.Request.InputStream;
            Log.Info("接收从微信后台POST过来的数据","");
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            Log.Info(this.GetType().ToString(), "Receive data from Unicom : " + builder.ToString());


            UnicomPayData data = new UnicomPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (Exception ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                UnicomPayData res = new UnicomPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Log.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                return res;
            }

            Log.Info(this.GetType().ToString(), "Check sign success");


            data.SetValue("return_code", "SUCCESS");
            return data;


        }

        //派生类需要重写这个方法，进行不同的回调处理
        public virtual UnicomPayData ProcessNotify()
        {
            return null;
        }
        //派生类需要重写这个方法，进行不同的回调处理
        public virtual UnicomPayData CheckNotify()
        {
            return null;
        }

    }



    // public function processValidateOrderId()
    // {
    //  $request = UnicomUtils::getRequestBean();
    // $orderid = $request['orderid'];
    // $signMsg = $request['signMsg'];
    // $usercode = $request['usercode'];
    // $provinceid = $request['provinceid'];
    // $cityid = $request['cityid'];

    // $mySign = md5("orderid=${orderid}&Key=".App::$KEY);

    // $response = array();

    //     //验证签名是否一致
    //     if (strcasecmp($mySign, $signMsg) == 0)
    //     {
    //     //TODO: start 填写校验订单逻辑，需要开发者完成 

    //     /*
    //      * 如果通过校验，将ifpasswd设置为true
    //      * 通过校验的含义为，待校验的订单在开发者系统中为有效订单，可以继续支付
    //      */
    //     $ifpasswd = true;
    //         //TODO: end

    //         if ($ifpasswd) {
    //         //TODO: 通过订单获取必要信息，需开发者完成
    //         $serviceid = "";
    //         $feename = "";
    //         $payfee = 0;
    //         $ordertime = date("YmdHis");
    //         $gameaccount = "";
    //         $macaddress = "";
    //         $ipaddress = "";
    //         $imei = "";
    //         $appversion = "";
    //         //

    //         //0-验证成功 1-验证失败，必填
    //         $response["checkOrderIdRsp"] = '0';

    //         //应用名称，必填
    //         $response["appName"] = App::$APP_NAME;

    //         //计费点名称
    //         $response["feename"] = $feename;

    //         //计费点金额，单位分
    //         $response["payfee"] = $payfee;

    //         //应用开发商名称，必填
    //         $response["appdeveloper"] = App::$APP_DEVELOPER;

    //         //游戏账号，长度<=64，联网支付必填
    //         $response["gameaccount"] = $gameaccount;

    //         //MAC地址去掉冒号，联网支付必填，单机尽量上报
    //         $response["macaddress"] = $macaddress;

    //         //沃商店应用id，必填
    //         $response["appid"] = App::$APP_ID;

    //         //IP地址，去掉点号，补零到每地址段3位, 如：192168000001，联网必填，单机尽量
    //         $response["ipaddress"] = $ipaddress;

    //         //沃商店计费点，必填
    //         $response["serviceid"] = $serviceid;

    //         //渠道ID，必填
    //         $response["channelid"] = App::$CHANNEL_ID;

    //         //沃商店CPID，必填
    //         $response["cpid"] = App::$CPID;

    //         //订单时间戳，14位时间格式，联网必填，单机尽量
    //         $response["ordertime"] = $ordertime;

    //         //设备标识，联网必填，单机尽量上报
    //         $response["imei"] = $imei;

    //         //应用版本号，必填
    //         $response["appversion"] = $appversion;
    //         }

    //     }
    //     else {
    //     $response['checkOrderIdRsp'] = '1';
    //     }

    //     echo UnicomUtils::toResponse($response, "paymessages");
    // }

    // public function processPayNotify()
    // {
    //  //订单是否被处理
    // $fullyProcessed = false;

    // //解析http请求体
    // $request = UnicomUtils::getRequestBean();

    // //cp订单号
    // $orderid = $request["orderid"];
    // //订单时间
    // $ordertime = $request["ordertime"];
    // //沃商店cpid
    // $cpid = $request["cpid"];
    // //应用ID
    // $appid = $request["appid"];
    // //渠道ID
    // $fid = $request["fid"];
    // //计费点ID
    // $consumeCode = $request["consumeCode"];
    // //支付金额，单位分
    // $payfee = $request["payfee"];
    // //0-沃支付，1-支付宝，2-VAC支付，3-神州付 ...
    // $payType = $request["payType"];
    // //支付结果，0代表成功，其他代表失败
    // $hRet = $request["hRet"];
    // //状态码
    // $status = $request["status"];
    // //签名 MD5(orderid=XXX&ordertime=XXX&cpid=XXX&appid=XXX&fid=XXX&consumeCode=XXX&payfee=XXX&payType=XXX&hRet=XXX&status=XXX&Key=XXX)
    // $signMsg = $request["signMsg"];

    ////校验签名是否正确
    // $mySign = md5("orderid=${orderid}&ordertime=${ordertime}&cpid=${cpid}&appid=${appid}&fid=${fid}&consumeCode=${consumeCode}&payfee=${payfee}&payType=${payType}&hRet=${hRet}&status=${status}&Key=".App::$KEY);

    //     if (strcasecmp($mySign, $signMsg) == 0)
    //     {
    //         //TODO： start 开发者处理逻辑, 处理完成后，将$fullyProcessed设为ture

    //         if ("0" == $hRet) {
    //             //TODO: 添加支付成功逻辑
    //         } else {
    //             //TODO: 添加支付失败逻辑
    //         }
    //     //
    //     $fullyProcessed = true;
    //         //
    //         //TODO: end

    //     }

    //     if (!$fullyProcessed) {
    //         header("HTTP/1.0 400 Bad Request");
    //     }

    //     echo UnicomUtils::toResponse($fullyProcessed ? 1 : 0, "callbackRsp");
    // }
}
