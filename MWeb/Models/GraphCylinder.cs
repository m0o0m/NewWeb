using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWeb.Models
{
    public class GraphCylinder
    {
        public string Title { get; set; }

        public bool HasGird { get; set; }

        public List<string> GirdTitle { get; set; }
        public string Id { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}