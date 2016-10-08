using System;
using System.Collections.Generic;
using System.Web;

namespace GL.Pay.WxPay
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}