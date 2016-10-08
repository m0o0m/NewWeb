using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class OpenFuDai
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public decimal Count { get; set; }
        public decimal NeedGold { get; set; }
        public fudaiType FuDaiType { get; set; }
        public DateTime Createtime { get; set; }

        public string RewardName { get; set; }
    }


    public enum fudaiType
    {
        免费福袋 = 1,
        收费福袋 = 2
    }

}
