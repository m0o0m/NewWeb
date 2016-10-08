using GL.Command.DBUtility;
using GL.Dapper;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.DAL
{
    public class ClubDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");

        public static IEnumerable<ClubGive> GetClubGive(int clubID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"select date( date_sub(curdate(),interval s.id day)) as CreateTime, ifnull(Gold ,0) as Gold 
from record.S_Ordinal s
left join (
  select date(CreateTime) as CreateTime, sum(Gold) as Gold 
  from 515game.ClubGive   
  where ClubID=@ClubID AND date_sub(current_date(), INTERVAL 9 DAY) <= CreateTime and CreateTime < date_add(curdate() ,interval 1 day)
  group by date(CreateTime) 
)a on date(date_sub(curdate(),interval s.id day)) = a.CreateTime 
where s.id < 10 ;");

                IEnumerable<ClubGive> i = cn.Query<ClubGive>(str.ToString(), new { ClubID = clubID });
                cn.Close();
                return i;
            }
        }

        public static int GetClubUserCount(int clubID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select COUNT(distinct UserID) as con 
from 515game.ClubUser 
where ClubID=@ClubID");

                IEnumerable<CountData> i = cn.Query<CountData>(str.ToString(),
                    new { ClubID = clubID });
                cn.Close();
                return i.FirstOrDefault().Con;
            }
        }


        public static int GetCommonClubCount(int clubID) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select COUNT(distinct UserID) as con 
from 515game.ClubGive 
where CreateTime >= date_add(curdate() ,interval -1 day) 
and CreateTime < curdate() AND ClubID=@ClubID");

                IEnumerable<CountData> i = cn.Query<CountData>(str.ToString(),
                    new { ClubID = clubID });
                cn.Close();
                return i.FirstOrDefault().Con;
            }
        }


        public static int GetHYClubCount(int clubID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select COUNT(distinct UserID) as con 
from 515game.ClubGive 
where CreateTime>=date_sub(curdate() ,interval date_format(curdate(),'%w') + 6 day) 
and CreateTime < date_sub(curdate() ,interval date_format(curdate(),'%w') - 1 day) 
AND ClubID=@ClubID");

                IEnumerable<CountData> i = cn.Query<CountData>(str.ToString(),
                    new { ClubID = clubID });
                cn.Close();
                return i.FirstOrDefault().Con;
            }
        }



        public static IEnumerable<MemberMender> GetMemberMender(int clubID,DateTime CreateTime,int page)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                int start = (page - 1) * 20;
                int end = start + 20;


                //str.AppendFormat(@"
                //select b.ID,b.Account, b.nickname as NickName ,ifnull(sum(c.Gold) ,0 ) Gold, datediff(curdate() ,b.LastModify ) as  LastLogin
                //from 515game.ClubUser a
                //    join 515game.Role b on a.userid = b.ID 
                //    left join 515game.ClubGive c on a.userid = c.UserID and c.CreateTime >= date(@CreateTime) 
                //	and c.CreateTime < date_add(date(@CreateTime) ,interval 1 day)
                //where a.ClubID = @ClubID 
                //group by b.nickname  ,datediff(curdate() ,b.LastModify )
                //order by ifnull(sum(c.Gold) ,0 ) desc ,LastLogin limit " + start + "," + 20 + ";");


                str.AppendFormat(@"select b.ID,b.Account, b.nickname as NickName ,ifnull(sum(c.Gold) ,0 ) Gold, datediff(curdate() ,b.LastModify ) as  LastLogin
from 515game.ClubUser a
    join 515game.Role b on a.userid = b.ID 
    left join (select * from 515game.ClubGive c where c.CreateTime >= date(@CreateTime) 
        and c.CreateTime < date_add(date(@CreateTime) ,interval 1 day) and ClubID = @ClubID ) c on b.id = c.UserID 
where a.ClubID = @ClubID  
group by b.nickname  ,datediff(curdate() ,b.LastModify )
order by ifnull(sum(c.Gold) ,0 ) desc ,LastLogin limit " + start + "," + 20 + ";");



                IEnumerable<MemberMender> i = cn.Query<MemberMender>(str.ToString(), 
                    new { ClubID = clubID, CreateTime = CreateTime });
                cn.Close();
                return i;
            }
        }



        public static Int64 GetClubWeekTotal(int clubID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select ifnull(sum(Gold),0) Con
from 515game.ClubGive
where ClubID = @ClubID and CreateTime >= date_add('2014-07-07' ,interval cast(floor(datediff(curdate() ,'2014-07-07')/7) as SIGNED)*7 day) 
");

 //               select ifnull(sum(Gold), 0) Con
 //from 515game.ClubGive
 //where ClubID = @ClubID and CreateTime >= subdate(curdate(), date_format(curdate(), '%w') - 1)

                IEnumerable<CountData64> i = cn.Query<CountData64>(str.ToString(),
                    new { ClubID = clubID });
                cn.Close();
                return i.FirstOrDefault().Con;
            }
        }


    }
}
