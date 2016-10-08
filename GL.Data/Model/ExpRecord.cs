using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class ExpRecord
    {
        public DateTime CreateTime { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public int ExpLevel { get; set; }
        public decimal ExpPoint { get; set; }
        public chipChangeType Type { get; set; }
        public decimal AddExp { get; set; }

    }

    public class ItemRecord
    {
        public DateTime CreateTime { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string ChangeType { get; set; }      //改变类型
        public itemName ItemName { get; set; }  //物品名称
        public int NowNum { get; set; }         //现在拥有个数
        public int OldNum { get; set; }         //之前拥有个数}

        public string TemplateName { get; set; }
    }

    public class ItemGroup
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int TypeList { get; set; }
    }
}
