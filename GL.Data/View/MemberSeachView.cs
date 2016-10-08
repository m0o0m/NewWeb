using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.View
{
    public class MemberSeachView
    {
        public object Value { get; set; }
        public int PageIndex { get; set; }
        public long UpperLimit { get; set; }
        public long LowerLimit { get; set; }
        public seachType SeachType { get; set; }
        public int Lv { get; set; }
    }


}
