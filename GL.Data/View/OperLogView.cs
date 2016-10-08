using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.View
{
    public class OperLogView
    {
        public string  StartTime { get; set; }

        public string EndTime { get; set; }

        public string UserAccount { get; set; }

        public string LeftMenu { get; set; }

        public object DataList { get; set; }

        public int Page { get; set; }

    }



    public class FreezeLogView
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int UserID { get; set; }

        public string OperUserName { get; set; }

        public object DataList { get; set; }

        public string Search { get; set; }
        public int Page { get; set; }

    }

}
