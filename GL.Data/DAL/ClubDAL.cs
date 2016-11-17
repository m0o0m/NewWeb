using System.Collections.Generic;
using System.Linq;
using GL.Dapper;
using MySql.Data.MySqlClient;
using GL.Data.Model;
using GL.Command.DBUtility;
using System.Data;
using GL.Data.View;
using System;
using Webdiyer.WebControls.Mvc;
using GL.Common;

namespace GL.Data.DAL
{
    public class ClubDAL
    {

        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static IEnumerable<UserClub> GetRebate(GameRecordView model)
        {

            if (model.UserID >0)
            {
                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();
                    IEnumerable<UserClub> i = cn.Query<UserClub>(@"S_ClubRebate", param: new { I_ClubID = model.UserID, I_CountDate = model.StartDate }, commandType: CommandType.StoredProcedure);
                    cn.Close();
                    return i;
                }
            }
            else
            {
                return new List<UserClub>();
            }
        }

        public static PagedList<RebateUser> GetRebateuser(int page, BaseDataView vbd)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;

            pq.RecordCount = 1;
            pq.Sql = string.Format(@"select a.UserID ,d.LoginTime ,c.TexasCount ,c.ScaleCount ,c.ZodiacCount ,c.HorseCount ,c.CarCount ,b.ServiceSum ,b.GiveSum 
                        from (select ID UserID ,NickName from "+ database1 + @".Role where id = {0}) a
                          left join (
                            select a.UserID ,sum(ServiceSum) ServiceSum ,sum(GiveSum) GiveSum 
                            from "+ database2+ @".C_RebateGive a 
                            where a.CountDate >= '{1}' and a.CountDate <'{2}' and a.UserID = {0} group by a.UserID
                          )b on a.UserID = b.UserID 
                          left join (
                            select userid ,sum(Texas_LCount+Texas_MCount+Texas_HCount) TexasCount ,sum(Scale_Count) ScaleCount
                                ,sum(Zodiac_Count) ZodiacCount ,sum(Horse_Count) HorseCount ,sum(Car_Count) CarCount 
                            from "+ database3+ @".Clearing_Game where userid = {0} and CountDate >= '{1}' and CountDate < '{2}' group by userid  
                          )c on a.UserID = c.UserID 
                          left join (
                            select userid ,max(LoginTime) LoginTime from "+ database1 + @".BG_LoginRecord where userid = {0} group by userid 
                          )d on a.UserID = d.UserID ;",
                vbd.SearchExt, vbd.StartDate, vbd.ExpirationDate);

            PagedList<RebateUser> obj = new PagedList<RebateUser>(DAL.PagedListDAL<RebateUser>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }
        internal static IEnumerable<UserClubDetail> GetRebateGroupDetail(GameRecordView model)
        {

            if (model.UserID > 0)
            {
                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();
                    string sql = string.Format(@"
                        select a.UserID ,c.NickName UserName ,ifnull(sum(b.ServiceSum) ,0) ServiceSum ,ifnull(sum(b.GiveSum) ,0) GiveSum ,a.CreateTime  
                        from "+ database2+ @".C_RebateUser a 
                            left join "+ database1 + @".Role c on a.UserID = c.ID 
                            left join "+ database2+ @".C_RebateGive b on a.UserID = b.UserID and date(a.CreateTime) <= b.CountDate  
                        where a.GroupID = @UserID group by a.UserID ,c.NickName ,a.CreateTime; ");
                    IEnumerable<UserClubDetail> i = cn.Query<UserClubDetail>(sql, model);
                    cn.Close();
                    return i;
                }
            }
            else
            {
                return new List<UserClubDetail>();
            }
        }
        internal static IEnumerable<UserClub> GetRebateGroup(GameRecordView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                string sql = string.Format(@"
                    select a.GroupID ClubID ,a.GroupName ,ifnull(sum(c.ServiceSum) ,0) ServiceSum ,ifnull(sum(c.GiveSum) ,0) GiveSum ,a.CreateTime ClubUpdate ,a.RebatePer 
                    from "+ database2 + @".C_RebateGroup a 
                        left join "+ database2 + @".C_RebateUser b on a.GroupID = b.GroupID 
                        left join "+ database2 + @".C_RebateGive c on b.UserID = c.UserID and date(b.CreateTime) <= c.CountDate 
                    where a.GroupID = case @UserID when 0 then a.GroupID else @UserID end group by a.GroupID ,a.GroupName ,a.CreateTime ; ");
                IEnumerable<UserClub> i = cn.Query<UserClub>(sql, model);
                cn.Close();
                return i;
            }
        }
        
        internal static IEnumerable<UserClub> GetMyRebate(GameRecordView model)
        {
            /*
            
            
select *,
(
case when Give_LastWeek >= 5000000000 then Give_LastWeek * 0.5 
                    when Give_LastWeek >= 3000000000  then Give_LastWeek * 0.4 
                    when Give_LastWeek >= 1000000000  then Give_LastWeek * 0.3
when Give_LastWeek >= 500000000  then Give_LastWeek * 0.25
               else Give_LastWeek * 0.2 end 
)as Rebate_LastWeek,

(   
 select IFNULL(ClubID,0)   from gamedata.ClubUser where userid = a.ClubId limit 1 
) as HighClub
 from (

select cc.ClubId ,
( select  count(1)  from gamedata.ClubUser where ClubID = cc.ClubId)as ClubCount ,
(  select IFNULL(sum(Gold),0) 
           from gamedata.ClubGive
          where ClubID = cc.ClubId  
) as Give_LastWeek 



 from GServerInfo.C_LoginUserClub as cc
where cc.UserId = 38
) a



            */
          
                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();
                    string sql = string.Format(@"


select *,
(
case when Give_LastWeek >= 5000000000 then Give_LastWeek * 0.5 
                    when Give_LastWeek >= 3000000000  then Give_LastWeek * 0.4 
                    when Give_LastWeek >= 1000000000  then Give_LastWeek * 0.3
when Give_LastWeek >= 500000000  then Give_LastWeek * 0.25
               else Give_LastWeek * 0.2 end 
)as Rebate_LastWeek,
(   
 select IFNULL(ClubID,0)   from "+ database1+ @".ClubUser where userid = a.ClubId limit 1 
) as HighClub
 from (

select cc.ClubId ,
( select  count(1)  from "+ database1 + @".ClubUser where ClubID = cc.ClubId)as ClubCount ,
(  select IFNULL(sum(Gold),0) 
           from "+ database1 + @".ClubGive
          where ClubID = cc.ClubId  and CreateTime < subdate(@StartDate,weekday(@StartDate)) and ClubType = 2 
                 and CreateTime >= date_sub(subdate(@StartDate,weekday(@StartDate)) ,interval 7 day) 

) as Give_LastWeek 
 from "+ database2+ @".C_LoginUserClub as cc,"+ database2 + @".C_LoginUser as cu
where cc.UserId=cu.UserId and cu.UserAccount =@SearchExt   {0}
) a
", model.SearchExter<=0?"": " and cc.ClubId = @SearchExter");
                    IEnumerable<UserClub> i = cn.Query<UserClub>(sql, model);
                    cn.Close();
                    return i;
                }
          
        }

        internal static IEnumerable<ClubData> GetClubData(GameRecordView model) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<ClubData> i = cn.Query<ClubData>(@"S_ClubSum", param: new {  }, commandType: CommandType.StoredProcedure);
                cn.Close();
                return i;
            }
        }

        

        internal static IEnumerable<UserClubDetail> GetRebateDetail(GameRecordView model)
        {

            if (model.UserID > 0)
            {
                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();
                    IEnumerable<UserClubDetail> i = cn.Query<UserClubDetail>(@"S_ClubRebate_Detail", param: new { I_ClubID = model.UserID, I_CountDate = model.StartDate }, commandType: CommandType.StoredProcedure);
                    cn.Close();
                    return i;
                }
            }
            else
            {
                return new List<UserClubDetail>();
            }
        }

        ///call S_ClubRebate_Detail(12163 ,'2015-10-30'); 俱乐部明细查询 
        ///查询汇总信息 call S_ClubRebate(12163 ,'2015-10-30');

        internal static IEnumerable<ClubDataDetail> GetClubDataDetail(GameRecordView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<ClubDataDetail> i = cn.Query<ClubDataDetail>(@"S_ClubSumDetail", param: new { }, commandType: CommandType.StoredProcedure);
                cn.Close();
                return i;
            }
        }



        internal static int AddCLoginUser(CLoginUser loginUser)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
               
                string sql = "insert into "+ database2 + @".C_LoginUser(UserAccount,UserPassword,DateTime) values(@UserAccount,@UserPassword,'"+DateTime.Now+"')";
                 

                cn.Open();

                cn.Query<Resource>(sql,loginUser);

                return 1;
            }



        }
        
        internal static int AddCRebateUser(CLoginUser loginUser)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                string sql = "";
                int i = 0;
                cn.Open();
                if (loginUser.Name != "")
                {
                    sql = "insert into "+ database2 + @".C_RebateGroup(GroupName,GroupDesc,RebatePer) values(@Name,@Desc,@Num)";
                    i = cn.Execute(sql, loginUser);
                }
                else
                {
                    sql = "call "+ database2 + @".f_split(@GroupID ,@ClubIds)";
                    //i = Convert.ToInt32(cn.Query(sql, loginUser).First());
                    i = cn.Execute(sql, loginUser);
                }
                
                return i;
            }
        }
        internal static int CancleCRebateUser(CLoginUser loginUser)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                string sql = "";
                if(loginUser.Num == 1)
                {
                    sql = @"delete from "+ database2 + @".C_RebateUser where GroupID=@GroupID;delete from "+ database2 + @".C_RebateGroup where GroupID = @GroupID;";
                }
                if(loginUser.Num == 2)
                {
                    sql = @"delete from "+ database2 + @".C_RebateUser where UserID=@GroupID;";
                }

                cn.Open();
                int i = cn.Execute(sql, loginUser);
                return i;
            }
        }


        internal static int DeleteCLoginUser(CLoginUser loginUser)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                string sql = @"delete from "+ database2 + @".C_LoginUser where UserId =@UserId;
                               delete from "+ database2 + @".C_LoginUserClub where UserId =@UserId;  
                    ";


                cn.Open();

                cn.Query<Resource>(sql, loginUser);

                return 1;
            }



        }


        internal static int DeleteCLoginUserClub(CLoginUserClub userClub)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                string sql = "delete from "+ database2 + @".C_LoginUserClub where UserId =@UserId and ClubId=@ClubId";


                cn.Open();

                cn.Query<Resource>(sql, userClub);

                return 1;
            }



        }



        internal static int AddCLoginUserClub(CLoginUserClub userClub)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                string sql = "insert into "+ database2 + @".C_LoginUserClub(UserId,ClubId,JoinDate) values(@UserId,@ClubId,'"+DateTime.Now+"')";


                cn.Open();

                cn.Query<Resource>(sql, userClub);

                return 1;
            }



        }


        public static CLoginUser GetCLoginUserByLoginName(CLoginUser user)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<CLoginUser> i = cn.Query<CLoginUser>(@"select * from "+ database2 + @".C_LoginUser where UserAccount = @UserAccount", user);
                cn.Close();
                return i.FirstOrDefault();
            }
        }



        internal static IEnumerable<ClubInfo> GetClubInfo(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                string sql = string.Format(@"
               select * from "+ database1+ @".ClubInfo where ID=@ID;    
");
                IEnumerable<ClubInfo> i = cn.Query<ClubInfo>(sql, new {
                    ID = id
                });
                cn.Close();
                return i;
            }
        }



    }
}
