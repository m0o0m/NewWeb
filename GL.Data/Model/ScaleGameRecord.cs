using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ScaleGameRecord
    {
        public DateTime CreateTime { get; set; }
        public int RoomID { get; set; }
        public decimal Round { get; set; }
        public string ResultCard { get; set; }
        public string UserData { get; set; }

        public string Account { get; set; }

        public Int64 BoardTime { get; set; }
    }
}
