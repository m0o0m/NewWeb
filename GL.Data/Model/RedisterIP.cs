using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class RedisterIP
    {
          public string IP { get; set; }
          public int Num { get; set; }
          public int Total { get; set; }
          public string Reason { get; set; }
          public DateTime ModifyTime { get; set; }
    }
}
