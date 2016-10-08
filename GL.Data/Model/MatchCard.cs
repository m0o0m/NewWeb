using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class MatchCard
    {
        public string DateTime { get; set; }

        public int UserID { get; set; }

        public string NickName { get; set; }

        public decimal Chip { get; set; }
     
        public decimal WinHandChip { get; set; }// 中拼手牌金额
        public decimal BuyHandChip { get; set; }//购买拼手牌金额

        //手牌牌型
        public int WinMultiple { get; set; }  //中奖倍数

        public int customshuffle { get; set; }
    }
}
