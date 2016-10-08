using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class DuiHuan
    {
        public int ID { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }

        public string NickName { get; set; }
        public int GoodsID { get; set; }
        public string GoodsName { get; set; }
        public int ItemNum { get; set; }
        public string TelNum { get; set; }
        public string qqNum { get; set; }
        public string addr { get; set; }
        public string email { get; set; }
        public decimal NeedZiKa { get; set; }
        public giveOut GiveOut { get; set; }
        public DateTime Createtime { get; set; }


    }

    public enum giveOut
    {
         没有发放 = 0, 发放了 = 1 
    }

}
