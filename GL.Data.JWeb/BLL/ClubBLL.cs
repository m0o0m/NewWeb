using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.JWeb.BLL
{
    public class ClubBLL
    {
        public static IEnumerable<ClubGive> GetClubGive(int clubID)
        {
            return ClubDAL.GetClubGive(clubID);
        }

        public static int GetClubUserCount(int clubID)
        {
            return ClubDAL.GetClubUserCount(clubID);
        }

        public static int GetCommonClubCount(int clubID)
        {
            return ClubDAL.GetCommonClubCount(clubID);
        }

        public static int GetHYClubCount(int clubID)
        {
            return ClubDAL.GetHYClubCount(clubID);
        }

        public static IEnumerable<MemberMender> GetMemberMender(int clubID, DateTime CreateTime,int page)
        {
            return ClubDAL.GetMemberMender(clubID,CreateTime,page);
        }

        public static Int64 GetClubWeekTotal(int clubID)
        {
            return ClubDAL.GetClubWeekTotal(clubID);

        }
    }
}
