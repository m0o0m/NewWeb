using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class Yeepay
    {
        public string orderAmount { get; set; }
        public string traderOrderID { get; set; }
        public string identityID { get; set; }
        public string identityType { get; set; }
        public string userIP { get; set; }
        public string userUA { get; set; }
        public string terminalType { get; set; }
        public string terminalID { get; set; }
        public string productCatalog { get; set; }
        public string productName { get; set; }
        public string productDesc { get; set; }
        public string fcallbackURL { get; set; }
        public string callbackURL { get; set; }

    }
}
