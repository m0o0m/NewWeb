using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.View
{
    public class OMModel
    {
        public string strActiveTime { get; set; }

        public string strAccount { get; set; }
        public int dwUserID { get; set; }
        public string strIP { get; set; }
        public string strLoginTime { get; set; }
        public string strGame { get; set; }
        public string online { get; set; }

        public int Reason { get; set; }

        public string Ext { get; set; }

        public int Minu { get; set; }
    }
}
