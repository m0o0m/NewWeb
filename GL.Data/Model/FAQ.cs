using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class FAQ
    {
        public int Id {get;set;}
        public int? faqtype { get; set; }
        public string faqtitle { get; set; }
        public string faqcontent { get; set; }
        public DateTime? operdate { get; set; }

    }
}
