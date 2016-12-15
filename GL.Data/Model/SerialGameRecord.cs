using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class SerialGameRecord
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }

        public long Board { get; set; }

        public int UserID { get; set; }

        public int RoundID { get; set; }

        public int RoomID { get; set; }

        public long BeforeGold { get; set; }

        public long AfterGold { get; set; }

        public long BeforeDiamond { get; set; }

        public long AfterDiamond { get; set; }

        public long Bet { get; set; }

        public long Pay { get; set; }

        public long RmainBet { get; set; }

        public string PotDetail { get; set; }

        public string Xiaochu { get; set; }
        public string Longzhu { get; set; }
    }
}
