using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class AppTreasure
    {
        public string Openid { get; set; }

        public string Openkey { get; set; }

        public string Pay_token { get; set; }
        public string Appid { get; set; }
        public string Pf { get; set; }
        public string Pfkey { get; set; }
        public string Session_id { get; set; }
        public string Session_type { get; set; }
        public string Userid { get; set; }

        public int Balance { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
