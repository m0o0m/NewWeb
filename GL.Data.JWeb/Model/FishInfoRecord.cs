using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.JWeb.Model
{
    public class FishInfoRecord
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string NickName { get; set; }

        public int GiveID { get; set; }

        public string GiveName { get; set; }

        public int Guid { get; set; }

        public int FishID { get; set; }

        public string FishName { get; set; }

        public DateTime CreateTime { get; set; }

        public int Type { get; set; }

        public int Flag { get; set; }
    }
}
