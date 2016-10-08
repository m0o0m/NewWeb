using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Pay
{
    public class log4netRegister
    {
        public static void Register() {
            var path = HttpContext.Current.Server.MapPath("~/log4net.xml");
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
        }
    }
}