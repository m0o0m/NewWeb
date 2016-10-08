using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GL.Data.View
{
    public class FuDaiOutPutView
    {
        public long UserID { get; set; }
        public int Page { get; set; }
        public string StartDate { get; set; }
        public string ExpirationDate { get; set; }
        public object DataList { get; set; }
        public decimal everydaySum { get; set; }
        public decimal everydayOutput { get; set; }
    }
}
