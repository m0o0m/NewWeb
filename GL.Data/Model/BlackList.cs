using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class BlackList
    {
        public string IP { get; set; }
        public string Mac { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
