using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class CLoginUser
    {
        public int UserId { get; set; }
        public int Num { get; set; }
        public string UserAccount { get; set; }
        public int GroupID { get; set; }
        public string UserPassword { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime DateTime { get; set; }

        public string ClubIds { get; set; }
    }

    public class CLoginUserClub {
        public int UserId { get; set; }

        public int ClubId { get; set; }

        public string UserAccount { get; set; }

        public string ClubName { get; set; }

        public bool IsViewClub { get; set; }

        public DateTime CreateTime { get; set; }

    }

    public class RebateUser
    {
        public int UserID { get; set; }

        public DateTime LoginTime { get; set; }

        public int TexasCount { get; set; }

        public int ScaleCount { get; set; }

        public long ServiceSum { get; set; }

        public int GiveSum { get; set; }
        public int ZodiacCount { get; set; }
        public int HorseCount { get; set; }
        public int CarCount { get; set; }
    }
}
