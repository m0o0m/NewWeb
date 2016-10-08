using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class LoginRecord
    {
        public long UserID { get; set; }
        public DateTime LoginTime { get; set; }
        public string IP { get; set; }
        public string Area { get; set; }
        public string Mac { get; set; }
        public string NickName { get; set; }
        public long Gold { get; set; }
        public int Diamond { get; set; }

        public string CreateIP { get; set; }//注册IP
        public int Acc { get; set; }

        public string UserIDS { get; set; }

        public int Flag { get; set; }

        public string AgentName { get; set; }
        public string VersionInfo { get; set; }
        public string PhoneInfo { get; set; }

        public AccountType AccountType { get; set; }
    }

    public class LoginRepeat
    {
        public string IP { get; set; }

        public int Account { get; set; }
        public DateTime LoginTime { get; set; }

        public string UserIDS { get; set; }
    }

    public enum AccountType {
        旧数据无登录方式 = -1,
        QQ = 0,
        微信 =1,
        游客 = 2,
        ID登录 = 3,
        五一五账号 = 4,
        第三方渠道 = 5

    }
}

