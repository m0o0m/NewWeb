using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ZodiacGameRecord
    {
        public DateTime CreateTime { get; set; }
        public int RoomID { get; set; }
        public decimal Round { get; set; }
        public int Sieve { get; set; }
        public string UserData { get; set; }

        public string Account { get; set; }

        public Int64 BoardTime { get; set; }
    }

    public enum zodiac
    {
        单注0,
        单注1,
        单注2,
        单注3,
        单注4,
        单注5,
        单注6,
        单注7,
        单注8,
        单注9,
        单注10,
        单注11,
        单注12,
        单注13,
        单注14,
        单注15,
        单注16,
        单注17,
        单注18,
        单注19,
        单注20,
        单注21,
        单注22,
        单注23,
        单注24,
        单注25,
        单注26,
        单注27,
        单注28,
        单注29,
        单注30,
        单注31,
        单注32,
        单注33,
        单注34,
        单注35,
        单注36,
        红色,
        黑色,
        双数,
        单数,
        低注,
        高注,
        列注1st12,
        列注2st12,
        列注3st12,
        列注1_34,
        列注2_35,
        列注3_36,
        鼠,
        牛,
        虎,
        兔,
        龙,
        蛇,
        马,
        羊,
        猴,
        鸡,
        狗,
        猪

    }


}
