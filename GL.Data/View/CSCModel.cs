using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GL;

namespace GL.Data.View
{
    public class CSCModel
    {
        public int CSCMainID { set; get; }
        public int CSCSubId { set; get; }
        public string GUName { set; get; }
        public string CSCTime { set; get; }
        public string CSCTitle { set; get; }
        public int? CSCType { set; get; }
        public string CSCContent { set; get; }
        public int? CSCState { set; get; }
        public IEnumerable<CSCLower> CSC { set; get; }
    }

    public class CSCLower
    {
        public string GUName { set; get; }
        public string CSCContent { set; get; }
        public string CSCTime { set; get; }
    }


}