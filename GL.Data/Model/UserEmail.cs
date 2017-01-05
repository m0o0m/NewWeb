using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class UserEmail
    {
        public int UEID { get; set; }
        public string UEUserID { get; set; }
        public string UETitle { get; set; }
        public string UEContent { get; set; }
        public string UEAuthor { get; set; }
        public DateTime UETime { get; set; }
        public ueItemType UEItemType { get; set; }
        public decimal UEItemValue { get; set; }
        public int UEItemNum { get; set; }

        public string UENote { get; set; }
        public string NickName { get; set; }

        public bool IsGlobal { get; set; }

        public Int64 PeopleNum { get; set; }

    }


    public enum ueItemType
    {
        无 = 0,
        金币 = 1,
        币 = 2,  //类似Q币
        积分 = 3,
        礼物 = 4,
        权贵 = 5,
        道具 = 6
    }

    public class UEUser
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public long MailGold { get; set; }
        public long MailDimoad { get; set; }
        public long MailJifen { get; set; }
    }
}
