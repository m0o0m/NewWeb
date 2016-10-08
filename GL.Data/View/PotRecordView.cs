using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.View
{
    public class PotRecordView
    {
        public string Context { get; set; }
        public double ChipPot { get; set; }
        public int Flag { get; set; }
        public gameID GameID { get; set; }

    }
}
