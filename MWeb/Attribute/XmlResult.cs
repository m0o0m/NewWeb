using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace MWeb
{
    public class XmlResult : ActionResult
    {
        public XmlResult(Object data)
        {
            this.Data = data;
        }
        public Object Data
        {
            get;
            set;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (Data == null)
            {
                new EmptyResult().ExecuteResult(context);
                return;
            }
            context.HttpContext.Response.ContentType = "application/xml";
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(Data.GetType());
                xs.Serialize(ms, Data);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    context.HttpContext.Response.Output.Write(sr.ReadToEnd());
                }
            }
        }
    }
}