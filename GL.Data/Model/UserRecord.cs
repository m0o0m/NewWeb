using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class UserRecord
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 牌局号
        /// </summary>
        public long Board { get; set; }
        /// <summary>
        /// 德州扑克白赢
        /// </summary>
        public long TexasWin { get; set; }
        /// <summary>
        ///  德州扑克输
        /// </summary>
        public long TexasLost { get; set; }
        /// <summary>
        ///  德州扑克做庄盈亏
        /// </summary>
        public long TexasZyk { get; set; }



        /// <summary>
        /// 中发白赢
        /// </summary>
        public long ScaleWin { get; set; }
        /// <summary>
        /// 中发白输
        /// </summary>
        public long ScaleLost { get; set; }
        /// <summary>
        /// 中发白做庄盈亏
        /// </summary>
        public long ScaleZyk { get; set; }



        /// <summary>
        /// 十二生肖赢
        /// </summary>
        public long ZodiacWin { get; set; }
        /// <summary>
        /// 十二生肖输
        /// </summary>
        public long ZodiacLost { get; set; }
        /// <summary>
        /// 十二生肖做庄盈亏
        /// </summary>
        public long ZodiacZyk { get; set; }


        /// <summary>
        /// 奔驰宝马赢
        /// </summary>
        public long CarWin { get; set; }
        /// <summary>
        /// 奔驰宝马输
        /// </summary>
        public long CarLost { get; set; }
        /// <summary>
        ///奔驰宝马做庄盈亏
        /// </summary>
        public long CarZyk { get; set; }



        /// <summary>
        /// 百人德州赢
        /// </summary>
        public long HundredWin { get; set; }
        /// <summary>
        /// 百人德州输
        /// </summary>
        public long HundredLost { get; set; }
        /// <summary>
        ///百人德州做庄盈亏
        /// </summary>
        public long HundredZyk { get; set; }

    }
}
