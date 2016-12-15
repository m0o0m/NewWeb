using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class BaccaratPot
    {
        public int ID { get; set; }

        public DateTime CreateTime { get; set; }

        public string BoardNo { get; set; }

        public decimal ChipBefor { get; set; }

        public decimal ChipAfter { get; set; }

        public string UserData { get; set; }

    }
}
