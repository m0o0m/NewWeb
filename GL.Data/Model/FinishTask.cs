using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class FinishTask
    {
        public int id { get; set; }
        public DateTime FinishTime { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
    }
}
