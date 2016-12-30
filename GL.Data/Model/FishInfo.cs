using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class UserFishInfo
    {
        public int Type { get; set; }
        public string Oper { get; set; }
        public long FishPrice { get; set; }
        public int Fish1 { get; set; }
        public int Fish2 { get; set; }
        public int Fish3 { get; set; }
        public int Fish4 { get; set; }
        public int Fish5 { get; set; }

        public int Fish6 { get; set; }
    }
    public class FishInfo
    {

        public long UserID { get; set; }
        public string NickName { get; set; }
        public int GiveID { get; set; }
        public string GiveName { get; set; }
        public int Guid { get; set; }
        public int FishID { get; set; }
        public string FishName { get; set; }
        public DateTime CreateTime { get; set; }
        public ntype Type { get; set; }

        public string ajax { get; set; }
    }
    public class FishCount
    {
        public int Num { get; set; }
        public int FishID { get; set; }
        public ntype Type { get; set; }

    }
    

    public enum ntype
    {
        赠送给 = 0,
        赠送 = 1,
        购买 = 2,
        放生 = 3
    }

}
