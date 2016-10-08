using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ScaleRecord
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public gameID GameID { get; set; }
        public int TableID { get; set; }
        public string Proj { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }

        public string UpdateValue { get; set; }


        public string IP { get; set; }

        public string Area { get; set; }
    }
}
