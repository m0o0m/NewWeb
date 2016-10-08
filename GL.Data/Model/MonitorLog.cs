using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class MonitorLog
    {
        public int ID { get; set; }

        public string MonitorName { get; set; }

        public string ExecDesc { get; set; }

        public int UserID { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
