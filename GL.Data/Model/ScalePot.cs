using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ScalePot
    {
        public int ID { get; set; }

        public DateTime CreateTime { get; set; }

        public string BoardNo { get; set; }

        public decimal ChipBefor { get; set; }

        public decimal ChipAfter { get; set; }

        public string UserData { get; set; }
    }

    public enum AwardPosit {
        庄家 = 0,
        红中 =1,
        发财 = 2,
        白板 = 3

    }

    public enum TexProPosit
    {
        方块 = 0,
        梅花 = 1,
        红桃 = 2,
        黑桃 = 3,
        庄家 = 4
    }

}
