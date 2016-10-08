using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class SimulatorRecharge
    {
        public int UserID { get; set; }

        public decimal Money { get; set; }

        public int Type { get; set; }

        public double Discounted { get; set; }

        public string ProductID { get; set; }

        public string URL { get; set; }
    }
}
