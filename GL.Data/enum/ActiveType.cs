using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data
{
    public enum ActiveType:uint
    {
        德州玩牌领奖 = 1,
        充值礼包奖励 = 2
    }

    public class ActiveTime {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
