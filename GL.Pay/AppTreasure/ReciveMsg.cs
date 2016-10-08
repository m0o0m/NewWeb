using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Pay.AppTreasure
{

    public class BalanceReciveMsg
    {
        public int ret { get; set; }
        public int balance { get; set; }

        public int gen_balance { get; set; }

        public int first_save { get; set; }

        public int save_amt { get; set; }

        public int gen_expire { get; set; }
    }

    public class InPayReciveMsg
    {
        public int ret { get; set; }
        public string billno { get; set; }

        public int balance { get; set; }

        public int used_gen_amt { get; set; }
    }
}
