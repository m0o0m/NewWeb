using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class LongHuGameRecord
    {
        public DateTime CreateTime { get; set; }
        public int RoomID { get; set; }
        public decimal RoundID { get; set; }
        public CarType CarID { get; set; }



        public string UserData { get; set; }


    }
}
