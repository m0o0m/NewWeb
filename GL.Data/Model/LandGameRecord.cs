using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class LandGameRecord
    {
        public DateTime CreateTime { get; set; }
        public int RoomID { get; set; }
        public decimal Round { get; set; }
        public int BaseScore { get; set; }
        public int Service { get; set; }
        public int Rate { get; set; }
        public string BankCard { get; set; }
        public string UserData { get; set; }
    }
}
