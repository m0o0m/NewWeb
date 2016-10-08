using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ClubGive
    {
        public string CreateTime { get; set; }

        public decimal Gold { get; set; }

        public string GoldStr { get; set; }
    }

    public class CountData {
        public int Con { get; set; }
    }

    public class CountData64
    {
        public Int64 Con { get; set; }
    }

    public class MemberMender{
        public string NickName { get; set; }

        public decimal Gold { get; set; }

        public string GoldStr { get; set; }
        public int LastLogin { get; set; }

        public int ID { get; set; }

        public string BeforeLogin { get; set; }
    }

    public class ClubInit {
        public Int16 V1 { get; set; }
        public Int16 V2 { get; set; }
        public Int32 C1 { get; set; }
        public Int32 C2 { get; set; }
        public Int32 C3 { get; set; }
        public string C4 { get; set; }
        public Int16 C5 { get; set; }

        public string Mark { get; set; }

        public int UID { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public Int64 WeekTotal { get; set; }

        public string WeekTotalStr { get; set; }

        public string FanLi { get; set; }

        public string NextTotal { get; set; }

        public string NextFanLi { get; set; }

        public string Code { get; set; }
    }
}
