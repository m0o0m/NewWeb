using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class TexasGameRecord
    {
        public DateTime CreateTime { get; set; }
        public int RoomID { get; set; }
        public decimal Round { get; set; }
        public string BaseScore { get; set; }
        public int Service { get; set; }
        public string BankCard { get; set; }
        public string UserData { get; set; }

        public string Account { get; set; }

        public string GiveUP { get; set; }

        public byte[] Record { get; set; }

        public Int64 BoardTime { get; set; }
    }

    public enum OperateRecordKind {
        无 = 0,
        加注 = 1,
        看牌 = 2,
        跟注 = 3,
        ALL_IN = 4,
        弃牌 = 5,
        小盲 = 6,
        大盲 = 7
    }

    public enum CardType {
        错误 = 0,
        高牌 = 1,
        一对 = 2,
        两对 = 3,
        三条 = 4,
        顺子 = 5,
        同花  =6,
        葫芦 = 7,
        四条 = 8,
        同花顺 = 9,
        皇家同花顺 = 10
    }


}
