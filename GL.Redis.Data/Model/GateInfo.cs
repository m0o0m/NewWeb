using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class GateInfo
    {
        public int ID { get; set; }
        public string GIP { get; set; }
        public string GPort { get; set; }
        public string ZIP { get; set; }
        public string ZPort { get; set; }
        public string HIP { get; set; }
        public string HPort { get; set; }
        public int Limit { get; set; }
        public string Description { get; set; }
        public gateType Type { get; set; }
        public int Num { get; set; }
    }

    public class Gate
    {
        public string G { get; set; }
        public string Z { get; set; }
        public string H { get; set; }
    }

    public enum gateType
    {
        内网测试 = 0,
        外网测试 = 1,
        正式 = 2
    }
}
