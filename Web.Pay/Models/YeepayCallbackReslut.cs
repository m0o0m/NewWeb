using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Pay.JsonMapper;

namespace Web.Pay.Models
{
    public class YeepayCallbackReslut
    {
        //{ "amount":2,
        //"bank":"工商银行",
        //"bankcode":"ICBC",
        //"cardtype":1,
        //"lastno":"1148",
        //"merchantaccount":"YB01000000144",
        //"orderid":"1234567536150700",
        //"sign":"VSslUIRmXkdpHa7qOkVKXHyiXL31XPXTispL5RvxREsopWsA3WNimEMmFoL9Xa1uzXgphNWh3kZXIVeIfPStJ987hX7fSQi/fvrDm8JJwc/DbLjGtc4PtNPXIyP/LR9RZvOC7Brwrc885luA4b29rwTqPmb9qnAIcoubCXZBIls=",
        //"status":1,
        //"yborderid":411508144289284291 }

        //"amount":100,"bank":"工商银行","bankcode":"ICBC","cardtype":1,"lastno":"1148","merchantaccount":"YB01000000144","orderid":"yeepay00001439560941","sign":"Eot30zCJnYq9C9N2Ho+egZQq+le2HelJNkWH+0OqA9XxlIfEmVYvk9xGLde6PcZzHyS0tbC2trzJbLdnUN8CcAwh/mX2qE7ibkFYcIaHPCdhIR6M2RqsjFa/7lSgugSnLrqRdNQCFyaPFWxy9jdWaT9sK0+uu82lS3M8KJk8yJ4=","status":1,"yborderid":411508147933533420

        [JsonField(Name = "amount")]
        public long amount { get; set; }

        [JsonField(Name = "bank")]
        public string bank { get; set; }

        [JsonField(Name = "bankcode")]
        public string bankcode { get; set; }

        [JsonField(Name = "cardtype")]
        public int cardtype { get; set; }
        [JsonField(Name = "lastno")]
        public string lastno { get; set; }
        [JsonField(Name = "merchantaccount")]
        public string merchantaccount { get; set; }
        [JsonField(Name = "orderid")]
        public string orderid { get; set; }
        [JsonField(Name = "sign")]
        public string sign { get; set; }
        [JsonField(Name = "status")]
        public int status { get; set; }
        [JsonField(Name = "yborderid")]
        public string yborderid { get; set; }
    }
}