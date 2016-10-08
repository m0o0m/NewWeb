using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class TexasProAccording
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 变化前游戏币
        /// </summary>
        public decimal ChipNow { get; set; }
        /// <summary>
        /// 变化后游戏币
        /// </summary>
        public decimal ChipAfter { get; set; }
        /// <summary>
        /// 改变的值
        /// </summary>
        public decimal ChipChange { get; set; }
      
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 牌局号
        /// </summary>
        public string RoundID { get; set; }


    }



    public enum TexProOper {
        中发白申请上庄 = 1,
        中发白取消上庄 = 2,
        中发白正式上庄 = 3,
        中发白庄家结算 = 4,
        中发白下庄 = 5,


        十二生肖申请上庄 = 6,
        十二生肖取消上庄 = 7,
        十二生肖正式上庄 = 8,
        十二生肖庄家结算 = 9,
        十二生肖下庄 = 10,


        奔驰宝马申请上庄 = 11,
        奔驰宝马取消上庄 = 12,
        奔驰宝马正式上庄 = 13,
        奔驰宝马庄家结算 = 14,
        奔驰宝马下庄 = 15,



        百人德州申请上庄 = 16,
        百人德州取消上庄 = 17,
        百人德州正式上庄 = 18,
        百人德州庄家结算 = 19,
        百人德州下庄 = 20

    }
}
