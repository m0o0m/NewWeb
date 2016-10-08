using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GL.Pay.Baidu
{
    public sealed class BaiduPayNotify
    {
        private const int appID = 7302074;//开发者应用ID

        private const string secretkey = "1I9MTGKYqwd5ZASyQF4ZsxdI96iNjymT";//开发者应用秘钥

        public static DeliverReceiveReturn Verify(string orderSerial, string cooperatorOrderSerial, string content, string sign)
        {
            var resultCode = 1;
            var resultMsg = string.Empty;

            //2.先检测请求数据签名是否合法 
            if (sign != Utility.Encrypt_MD5_UTF8(appID + orderSerial + cooperatorOrderSerial + content + secretkey))
            {
                resultCode = 1001;//自定义错误信息
                resultMsg = "签名错误";//自定义错误信息

                return new DeliverReceiveReturn
                {
                    AppID = appID,
                    ResultCode = resultCode,
                    ResultMsg = resultMsg,
                    Sign = Utility.Encrypt_MD5_UTF8(appID + resultCode + secretkey),
                    Content = string.Empty
                };


                //this.page.Response.Write(JsonConvert.SerializeObject(new DeliverReceiveReturn()
                //{
                //    AppID = appID,
                //    ResultCode = resultCode,
                //    ResultMsg = resultMsg,
                //    Sign = Utility.Encrypt_MD5_UTF8(appID + resultCode + secretkey),
                //    Content = string.Empty
                //}
                //    ));


            }



            //5.返回成功结果
            resultCode = 1;
            resultMsg = "成功";
            return new DeliverReceiveReturn
            {
                AppID = appID,
                ResultCode = resultCode,
                ResultMsg = resultMsg,
                Sign = Utility.Encrypt_MD5_UTF8(appID + resultCode + secretkey),//签名
                Content = string.Empty
            };



            //Response.Write(JsonConvert.SerializeObject(new DeliverReceiveReturn()
            //{
            //    AppID = appID,
            //    ResultCode = resultCode,
            //    ResultMsg = resultMsg,
            //    Sign = Utility.Encrypt_MD5_UTF8(appID + resultCode + secretkey),//签名
            //    Content = string.Empty
            //}
            //));
        }

  
    }
}
