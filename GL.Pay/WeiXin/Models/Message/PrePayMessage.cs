using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GL.Pay.WxPay.Models.Message
{
    public class PrePayMessage : ErrorMessage
    {
        public string PrePayId { get; set; }
    }
}
