using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class BaccaratGameRecord
    {
        public DateTime CreateTime { get; set; }
        public int RoomID { get; set; }
        public long Round { get; set; }
        public string ResultCard { get; set; }
        public string UserData { get; set; }
        public int BoardTime { get; set; }

    }
}
