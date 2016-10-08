using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace A2.Web.Models
{
    public class NewsItem
    {
        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string SourceURL { get; set; }
        public string Content { get; set; }

    }
}