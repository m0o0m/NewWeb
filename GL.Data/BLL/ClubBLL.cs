using GL.Command.DBUtility;
using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using GL.Data.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class ClubBLL
    {
        //public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        //public static PagedList<UserClub> GetListByPage(GameRecordView msv)
        //{
        //    PagerQuery pq = new PagerQuery();
        //    pq.CurrentPage = msv.Page;
        //    pq.PageSize = 10;


        //    pq.RecordCount = DAL.PagedListDAL<UserClub>.GetRecordCount(string.Format(@"select count(0) from Role where ID = {0}", msv.UserID), sqlconnectionString);
        //    pq.Sql = string.Format(@"select r.* from (select ID, NickName, ClubID, Agent from Role where ID = {2} order by ID desc limit {0}, {1}) as r", pq.StartRowNumber, pq.PageSize, msv.UserID);


        //    PagedList<UserClub> obj = new PagedList<UserClub>(DAL.PagedListDAL<UserClub>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
        //    return obj;
        //}
        public static IEnumerable<UserClub> GetRebate(GameRecordView model)
        {
            return ClubDAL.GetRebate(model);
        }

        public static IEnumerable<UserClub> GetRebateGroup(GameRecordView model)
        {
            return ClubDAL.GetRebateGroup(model);
        }
        
        public static PagedList<ClubDataDetail> GetMyRebate(GameRecordView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

         

            pq.RecordCount = DAL.PagedListDAL<ClubDataDetail>.GetRecordCount(string.Format(@"
select count(*) from  GServerInfo.C_LoginUserClub cc,GServerInfo.C_LoginUser cu
 where cc.UserId=cu.UserId and cu.UserAccount='"+model.SearchExt+"' {0}",
 model.SearchExter <= 0 ?"": " and cc.ClubId = "+model.SearchExter));
           

            string sql = string.Format(@"
select a.* from (
select Club ,sum(ClubCount) ClubCount ,sum(GiveYes) GiveYes ,sum(GiveLast) GiveLast ,sum(Login) Login ,s.Nickname
from (
    select a.ClubID Club ,count(distinct a.userid) ClubCount ,0 GiveYes ,0 GiveLast ,ifnull(count(distinct b.userid ) ,0) Login
    from 515game.ClubUser  a
      left join 515game.ClubGive b on a.userid = b.userid and b.CreateTime >= date_add('"+model.StartDate+@"'  ,interval -1 day) and b.CreateTime < '"+ model.StartDate + @"' 
    group by a.ClubID
union all
    select ClubID Club ,0 ,0 ,sum(Gold) GiveLast ,0 
    from 515game.ClubGive 
    where ClubType = 2 and CreateTime < subdate('"+ model.StartDate + @"' ,weekday('"+ model.StartDate + @"' )) and 
      CreateTime >= date_sub(subdate('"+ model.StartDate + @"' ,weekday('"+ model.StartDate + @"' )) ,interval 7 day)
    group by ClubID
union all
    select ClubID ,0 ,sum(Gold) GiveYes ,0 ,0 
    from 515game.ClubGive 
    where ClubType = 2 and CreateTime < '"+ model.StartDate + @"'  and CreateTime >= date_sub('"+ model.StartDate + @"'  ,interval 1 day) 
    group by ClubID
union all
    select ID ,0 ,0 ,0 ,0
    from 515game.ClubInfo
)t join 515game.Role s on t.Club = s.id group by Club ,s.Nickname order by 4 desc
) as a
where a.Club  in (
   select cc.ClubId from   GServerInfo.C_LoginUserClub as cc,GServerInfo.C_LoginUser as cu
   where cc.UserId=cu.UserId and cu.UserAccount ='"+model.SearchExt+@"' 
) {0}
limit {1}, {2}
", model.SearchExter <= 0 ? "" : " and a.Club =  " + model.SearchExter, pq.StartRowNumber, pq.PageSize);


            pq.Sql = sql;
            PagedList<ClubDataDetail> obj = new PagedList<ClubDataDetail>(DAL.PagedListDAL<ClubDataDetail>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static PagedList<UserClubDetail> GetMyRebateDetail(GameRecordView model)
        {

            IEnumerable<UserClubDetail> data = GetRebateDetail(model);
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;
            pq.RecordCount = data.Count();
            data = data.Skip((pq.CurrentPage - 1) * 10).Take(10);
            PagedList<UserClubDetail> obj = new PagedList<UserClubDetail>(
               data, pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        public static IEnumerable<UserClubDetail> GetRebateGroupDetail(GameRecordView model)
        {
            return ClubDAL.GetRebateGroupDetail(model);
        }

        public static IEnumerable<UserClubDetail> GetRebateDetail(GameRecordView model)
        {
            return ClubDAL.GetRebateDetail(model);
        }

        public static IEnumerable<ClubData> GetClubData(GameRecordView model) {
            return ClubDAL.GetClubData(model);
        }

        public static IEnumerable<ClubInfo> GetClubInfo(int id) {
            return ClubDAL.GetClubInfo(id);
        }
        public static IEnumerable<ClubDataDetail> GetClubDataDetail(GameRecordView model) {
            return ClubDAL.GetClubDataDetail(model);
        }


        /// <summary>
        /// 得到登录用户列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PagedList<CLoginUser> GetCLoginUserListByPage(int page, string  userAccount)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<CLoginUser>.GetRecordCount(string.Format(@"select count(0) from GServerInfo.C_LoginUser {0}", string.IsNullOrEmpty(userAccount)==false? "  where UserAccount = '"+ userAccount + "' or UserId= '"+userAccount+"'" : ""));
          //  pq.Sql = string.Format(@"select * from GServerInfo.C_LoginUser  {2}  order by DateTime desc  limit {0}, {1}", pq.StartRowNumber, pq.PageSize, string.IsNullOrEmpty(userAccount) == false  ? "  where UserAccount = '" + userAccount + "' or UserId= '"+ userAccount+"'" : "");

            pq.Sql = string.Format(@"select cu.*,(
  
      select GROUP_CONCAT(ClubId) from GServerInfo.C_LoginUserClub
      where UserId = cu.UserId 
     
)as ClubIds from GServerInfo.C_LoginUser as cu 
{2}  order by cu.DateTime desc limit {0}, {1};
", pq.StartRowNumber, pq.PageSize, string.IsNullOrEmpty(userAccount) == false ? "  where cu.UserAccount = '" + userAccount + "' or cu.UserId= '" + userAccount + "'" : "");



            PagedList<CLoginUser> obj = new PagedList<CLoginUser>(DAL.PagedListDAL<CLoginUser>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        /// <summary>
        /// 添加登录用户
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public static bool AddCLoginUser(CLoginUser loginUser) {
            return ClubDAL.AddCLoginUser(loginUser)>0;
        }
        public static IEnumerable<RebateUser> GetRebateuser(int page, BaseDataView bdv)
        {
            return ClubDAL.GetRebateuser(page, bdv);
        }
        public static int CancleCRebateUser(CLoginUser loginUser)
        {
            return ClubDAL.CancleCRebateUser(loginUser);
        }

        public static int AddCRebateUser(CLoginUser loginUser)
        {
            return ClubDAL.AddCRebateUser(loginUser);
        }
        /// <summary>
        /// 删除登录用户
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public static bool DeleteCLoginUser(CLoginUser loginUser)
        {
            return ClubDAL.DeleteCLoginUser(loginUser) > 0;
        }


        public static bool AddCLoginUserClub(CLoginUserClub userClub)
        {
            return ClubDAL.AddCLoginUserClub(userClub) > 0;
        }

        public static bool DeleteCLoginUserClub(CLoginUserClub userClub)
        {
            return ClubDAL.DeleteCLoginUserClub(userClub) > 0;
        }


        public static PagedList<CLoginUserClub> GetCLoginUserClubListByPage(int page, int userId,int clubid)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            if (clubid < 0)
            {//说明没有查询条件，那么就是查询能看到的
                pq.RecordCount = DAL.PagedListDAL<CLoginUserClub>.GetRecordCount(string.Format(@"select count(0) from GServerInfo.C_LoginUserClub where UserId=" + userId));
                pq.Sql = string.Format(@"select UserId,ClubId,CreateTime,1 as IsViewClub from GServerInfo.C_LoginUserClub,515game.ClubInfo
where UserId = " + userId+ " and C_LoginUserClub.ClubId = ClubInfo.ID order by JoinDate desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            }
            else {

                pq.RecordCount = DAL.PagedListDAL<CLoginUserClub>.GetRecordCount(string.Format(@"select count(0) from 515game.ClubInfo where ID= "+clubid));
                pq.Sql = string.Format(@"select -1 as UserId,ID as ClubId,CreateTime,(
   select count(*) from GServerInfo.C_LoginUserClub
   where UserId = " + userId+@" and ClubId = "+ clubid + @"
) as IsViewClub 
from 515game.ClubInfo 
where ID = " + clubid + @"
   limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            }

            PagedList<CLoginUserClub> obj = new PagedList<CLoginUserClub>(DAL.PagedListDAL<CLoginUserClub>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }



        public static CLoginUser GetCLoginUserByLoginName(CLoginUser user)
        {
            return ClubDAL.GetCLoginUserByLoginName(user) ;
        }





    }
}
