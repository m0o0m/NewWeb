using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.MeiZu
{
    public class MeiZuModel
    {
        public int app_id { get; set; }

        public int buy_amount { get; set; }

        public long cp_order_id { get; set; }

        public long create_time { get; set; }

        public int pay_type { get; set; }

        public string product_body { get; set; }


        public string product_id { get; set; }

        public string product_per_price { get; set; }

        public string product_subject { get; set; }

        public string product_unit { get; set; }
        public string sign { get; set; }

        public string sign_type { get; set; }
        public string total_price { get; set; }
        public string uid { get; set; }
        public string user_info { get; set; }
        

    }


    public class MeiZuRecModel {


            public int code { get; set; }

        public string message { get; set; }

        public string redirect { get; set; }

        public MeiZuModel value { get; set; }
    }

}
