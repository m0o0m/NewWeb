using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class PopUpInfo
    {
        public int id { get; set; }
        public PopPosition Position { get; set; }

        public string Platform { get; set; }

        public JumpPage JumpPage { get; set; }

        public int OpenWinNo { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int IsOpen { get; set; }

     
    }


    public enum Platform {
        Web = 0,
        Android = 1,
        IOS = 2
    }

    public enum PopPosition
    {
         首登首弹=0,
         首登后弹=1,
         返回首弹=2,
         进房首弹=3
    }

    public enum JumpPage {
        感恩 = 0
    }

}
