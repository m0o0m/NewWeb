using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWeb
{
    public class BarAndLine
    {
        public string ID { get; set; }//id

        public string Name { get; set; }//name

        public string Style { get; set; }//样式

        public string Title { get; set; }

        public string URL { get; set; }

        public string Unit { get; set; }

        public int BandRate { get; set; }

        public string BindXField { get; set; }

        public List<GraphItem> GraphItems { get; set; }

        public List<GirdsItem> GirdsItems { get; set; }

    }

    public class GirdsItem {
        public string ViewName { get; set; }
        public string BindYField { get; set; }

        public bool Display { get; set; }
    }

    public class GraphItem {
        public string ViewName { get; set; }

        public string BindYField { get; set; }

     

        public bool Display { get; set; }
    }
}