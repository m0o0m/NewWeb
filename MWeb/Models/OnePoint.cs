using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWeb.Models
{
    public class OnePoint
    {
        public string SessionId { get; set; }
        public string LoginName { get; set; }

        public DateTime UserTime { get; set; }
    }
}