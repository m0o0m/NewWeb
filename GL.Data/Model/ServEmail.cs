using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ServEmail
    {
        public int ServEmailID { get; set; }
        public DateTime ServEmailTime { get; set; }
        public string ServEmailTitle { get; set; }
        public string ServEmailContent { get; set; }
        public string ServEmailAuthor { get; set; }
    }
}
