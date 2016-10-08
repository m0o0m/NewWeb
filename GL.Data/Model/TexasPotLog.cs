using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class TexasPotLog
    {
        public DateTime Time { get; set; }
        public ctrlType Type { get; set; }
        public gameID GameType { get; set; }
        public string Content { get; set; }

        public double Value { get; set; }
    }


    public enum ctrlType
    {
        增加 = 1,
        减少 = 2
    }
}
