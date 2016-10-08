using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class Festival515
    {
        public string DateTime { get; set; }
        public string DAU { get; set; }
        /// <summary>
        /// 点击人数
        /// </summary>
        public int IconPCount { get; set; }
        /// <summary>
        /// 点击人次
        /// </summary>
        public int IconPNum { get; set; }
        /// <summary>
        /// 大厅点击人数
        /// </summary>
        public int DIconPCount { get; set; }
        /// <summary>
        /// 大厅点击人次
        /// </summary>
        public int DIconPNum { get; set; }
        /// <summary>
        /// 房间点击人数
        /// </summary>
        public int FIconPCount { get; set; }
        /// <summary>
        /// 房间点击人次
        /// </summary>
        public int FIconPNum { get; set; }

    }

    public class FestivalLogin {
        public string LoginTime { get; set; }

        public string DAU { get; set; }

        public int UserCount { get; set; }

        public int UserPlay { get; set; }

        public int UserArawd { get; set; }
    }

    public class AllFesLogin {
        public List<FestivalLogin> OneDay { get; set; }

        public List<FestivalLogin> SecDay { get; set; }

        public List<FestivalLogin> thirdDay { get; set; }
    }

    public class LoginFes {
        public int LoginCount { get; set; }

        public int One { get; set; }

        public int Account { get; set; }
    }

    public class FestivalVIP {
        public string CreateTime { get; set; }

        public string DAU { get; set; }

        public int Vip1 { get; set; }
        public int Vip2 { get; set; }
        public int Vip3 { get; set; }
        public int Vip4 { get; set; }
        public int Vip5 { get; set; }
        public int Vip6 { get; set; }
        public int Vip7 { get; set; }
        public int Vip8 { get; set; }
        public int Vip9 { get; set; }
        public int Vip10 { get; set; }

    }


    public class FestivalBaseData
    {
        public string StartTime { get; set; }

        public string ExpirationDate { get; set; }

        public int ClientType { get; set; }
        public List<object> objects { get; set; }
    }

}
