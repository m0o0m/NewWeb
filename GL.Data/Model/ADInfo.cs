using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ADInfo
    {

        public int ChannlID{ get; set; }
        public string IP { get; set; }

        public string Url { get; set; }
    }


    public class DMModel {

        public int Id { get; set; }
        public string Appkey { get; set; }
        public string Ifa { get; set; }
        public string Ifamd5 { get; set; }
        public string Mac { get; set; }
        public string MacMD5 { get; set; }
        public string Source { get; set; }

        public long Iddate { get; set; }

        public DateTime CreateTime { get; set; }

        public int Flag { get; set; }
    }
}
