using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class DownLevel
    {
        public DateTime CreateTime { get; set; }
        public Int64 PLess500 { get; set; }

        public Int64 P500_1K { get; set; }

        public Int64 P1K_2K { get; set; }
        public Int64 P2K_5K { get; set; }
        public Int64 P5K_1W { get; set; }

        public Int64 P1W_5W { get; set; }

        public Int64 P5W_10W { get; set; }

        public Int64 P10W_25W { get; set; }

        public Int64 P25W_50W { get; set; }

        public Int64 P50W_100W { get; set; }

        public Int64 P100W_500W { get; set; }

        public Int64 P500W_2000W { get; set; }

        public Int64 PMore2000W { get; set; }
    }
}
