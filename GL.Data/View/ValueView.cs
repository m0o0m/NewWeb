using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.View
{
    public class ValueView
    {
        public int Page { get; set; }
        public string Target { get; set; }
        public string Value { get; set; }

        public seachType Type { get; set; }

        public object DataList { get; set; }
        public string StartDate { get; set; }
        public string ExpirationDate { get; set; }

        public object Data { get; set; }

    }
}
