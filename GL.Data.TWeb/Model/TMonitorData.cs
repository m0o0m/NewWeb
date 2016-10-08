using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.TWeb.Model
{
   public  class TMonitorData
    {
        public int MonitorID { get; set; }

        public string MonitorName { get; set; }

        public bool IsOpen { get; set; }

        public int ExecType { get; set; }

        public string ExecSQL { get; set; }

        public string ExecDesc { get; set; }

        public string UpdateTable { get; set; }
    }
}
