using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Model
{
    public class CustomerServCenter
    {
        public int CSCMainID { set; get; }
        public int CSCSubId { set; get; }
        public int GUUserID { set; get; }
        public DateTime CSCTime { set; get; }
        public string CSCTitle { set; get; }
        public int? CSCType { set; get; }
        public string CSCContent { set; get; }
        public int? CSCState { set; get; }
        public string GUName { set; get; }
        public int GUType { set; get; }
    }
}