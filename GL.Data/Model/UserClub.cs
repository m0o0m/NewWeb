using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class UserClub
    {
        public int ClubID { get; set; }
        public decimal Rebate_Yesterday { get; set; }
        public decimal Rebate_LastWeek { get; set; }
        public int ClubCount { get; set; }
        public clubType ClubType { get; set; }
        public int HighClub { get; set; }
        public int RebatePer { get; set; }
        public DateTime ClubUpdate { get; set; }

        public decimal Give_LastWeek { get; set; }
        public long ServiceSum { get; set; }
        public long GiveSum { get; set; }
        public string GroupName { get; set; }

        public string TGUrl { get; set; }
    }

    public enum clubType
    {
        未激活俱乐部 = 0,
        普通模式俱乐部 = 1,
        会员模式俱乐部 = 2
    }

    public class UserClubDetail
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal Gold_LastWeek { get; set; }
        public decimal Gold_Yesterday { get; set; }
        public long ServiceSum { get; set; }
        public long GiveSum { get; set; }
    }


    public class ClubData {
        /// <summary>
        /// 俱乐部总数
        /// </summary>
        public int ClubCount { get; set; }
        /// <summary>
        /// 昨日总贡献
        /// </summary>
        public double GiveYes { get; set; }
        /// <summary>
        /// 上周总贡献
        /// </summary>
        public double GiveLast { get; set; }
        /// <summary>
        /// 本周已领取总返利
        /// </summary>
        public double Rebate { get; set; }
    }

    public class ClubDataDetail {
        /// <summary>
        /// 俱乐部id
        /// </summary>
        public int Club { get; set; }
        /// <summary>
        /// 俱乐部下家人数
        /// </summary>
        public int ClubCount { get; set; }
        /// <summary>
        /// 下家昨日总贡献
        /// </summary>
        public double GiveYes { get; set; }
        /// <summary>
        /// 下家上周总贡献
        /// </summary>
        public double GiveLast { get; set; }
        /// <summary>
        /// 昨日活跃人数
        /// </summary>
        public int Login { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
    }

    public class ClubInfo {
        public int ID { get; set; }

        public DateTime CreateTime { get; set; }

        public int ClubType { get; set; }
    }
}
