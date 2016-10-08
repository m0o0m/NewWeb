using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace GL.Pay.YiDong
{


    public class response
    {
        public response()
        {

        }

        public int hRet { get; set; }
        public string message { get; set; }
    }


    public class XmlResult : ActionResult
    {
        // 可被序列化的内容
         response Data { get; set; }

        // Data的类型
        Type DataType { get; set; }

        // 构造器
        public XmlResult(response data, Type type)
        {
            Data = data;
            DataType = type;
        }

        // 主要是重写这个方法
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            // 设置 HTTP Header 的 ContentType
            response.ContentType = "text/xml";

            if (Data != null)
            {
                // 序列化 Data 并写入 Response
                //XmlSerializer serializer = new XmlSerializer(DataType);
                //MemoryStream ms = new MemoryStream();
                //serializer.Serialize(ms, Data);
                //string str = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                //str = str.Replace("\r", "").Replace("\n", "").Replace(">  <","><");
                //str = str.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"","");
                //str = str.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                //str = str.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                //str = str.Replace("?", " encoding=\"UTF - 8\"?");

                string str= "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                    +"<response>"
                    +"<hRet>"+ Data .hRet+ "</hRet>"+
                    "<message>"+ Data.message + "</message>"+
                    "</response>";

                response.Write(str);
            }
        }
    }
}
