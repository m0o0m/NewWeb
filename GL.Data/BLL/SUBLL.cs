using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using GL.Data.MWeb.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class SUBLL
    {
      
        public static PagedList<ApplicationUser> GetListByPage(int page)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<ApplicationUser>.GetRecordCount(@"select count(0) from AspNetUsers where UserName != 'admin'");
            pq.Sql = string.Format(@"SELECT Id,Email,EmailConfirmed,
                PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,
                LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,NickName FROM AspNetUsers where UserName != 'admin' order by Id desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<ApplicationUser> obj = new PagedList<ApplicationUser>(DAL.PagedListDAL<ApplicationUser>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }


        public static IEnumerable<Resource> GetResourceList(string roleId, int level)
        {
            return DAL.SUDAL.GetResourceList(roleId, level);
        }

        public static IEnumerable<Resource> GetResourceListByRoleId(string roleid)
        {
            return DAL.SUDAL.GetResourceListByRoleId(roleid);
        }

        public static IEnumerable<Resource> GetResourceListByUserId(string userid)
        {
            return DAL.SUDAL.GetResourceListByUserId(userid);
        }

        public static List<Resource> GetAllResourceListByUserId(string userid) {
            IEnumerable<Resource> res = DAL.SUDAL.GetAllResourceListByUserId(userid);
            if (res.Count() != 0)
            {
                return res.ToList();
            }
            else {
                return new List<Resource>();
            }
        }


        public static IEnumerable<Resource> GetResourceListByRoleName(string roleName)
        {
            AspNetRole role = DAL.SUDAL.GetAspNetRoleByRoleName(roleName);
            if (role == null)
            {
                return new List<Resource>();
            }


            return DAL.SUDAL.GetResourceListByRoleId(role.Id);
        }

        public static List<Resource> GetUserRoleResourceListByUserId(string userName)
        {

            AspNetUser user = DAL.SUDAL.GetAspNetUserByUserName(userName);
            if (user == null)
            {
                return new List<Resource>();
            }
            IEnumerable<Resource> res = DAL.SUDAL.GetUserRoleResourceListByUserId(user.Id);
            if (res.Count() != 0)
            {
                return res.ToList();
            }
            else
            {
                return new List<Resource>();
            }
        }


        public static List<Resource> GetAllResourceListByUserName(string userName)
        {

            AspNetUser user = DAL.SUDAL.GetAspNetUserByUserName(userName);
            if (user == null) {
                return new List<Resource>();
            }
            IEnumerable<Resource> res = DAL.SUDAL.GetAllResourceListByUserId(user.Id);
            if (res.Count() != 0)
            {
                return res.ToList();
            }
            else
            {
                return new List<Resource>();
            }
        }



        public static List<Resource> GetAllResourceList() {
            IEnumerable<Resource> res = DAL.SUDAL.GetAllResourceList();
            return res.ToList();
        }

        public static List<Resource> GetAdminResourceList()
        {
            IEnumerable<Resource> res = DAL.SUDAL.GetAdminResourceList();
            return res.ToList();
        }

        public static int AddRoleResource(string roleid, string resourceNo)
        {
            return DAL.SUDAL.AddRoleResource(roleid, resourceNo);
        }

        public static int AddUserResource(string userid, string resourceNo)
        {
            return DAL.SUDAL.AddUserResource(userid, resourceNo);
        }

        public static bool CheckUserAction(string userid,string action) {
            return DAL.SUDAL.CheckUserAction(userid, action);
        }


        public static Resource GetModelByFirstUrl(string url)
        {
            url = url.Trim('/');
            url = "/" + url;
            return DAL.SUDAL.GetModelByFirstUrl(url);
        }


        //============================================
        public static bool AddUserLimit(string userid, string LimitNo, int category)
        {
            return DAL.SUDAL.AddUserLimit(userid, LimitNo, category)>0;
        }


        public static UserLimit GetLimitModel(UserLimit limit) {
            return DAL.SUDAL.GetLimitModel(limit) ;
        }



        public static PagedList<LogInfo> GetLogListByPage(BaseDataView bdv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;
            if (bdv.SearchExt == "")
            {
                pq.RecordCount = DAL.PagedListDAL<LogInfo>.GetRecordCount(@"select count(0) from record.Log where CreateTime BETWEEN '" + bdv.StartDate + "' and '" + bdv.ExpirationDate + "' ");
                //pq.Sql = string.Format(@"SELECT * FROM record.Log where CreateTime BETWEEN '{3}' and '{4}'  order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, bdv.SearchExt, bdv.StartDate, bdv.ExpirationDate);

                pq.Sql = string.Format(@"  SELECT l.*,u.NickName FROM record.Log as l left join GServerInfo.AspNetUsers as u
                on l.UserAccount = u.UserName where
 l.CreateTime BETWEEN '{3}' and '{4}'  order by l.CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, bdv.SearchExt, bdv.StartDate, bdv.ExpirationDate);




            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<LogInfo>.GetRecordCount(@"select count(0) from record.Log where UserAccount='" + bdv.SearchExt + "' and CreateTime BETWEEN '" + bdv.StartDate + "' and '" + bdv.ExpirationDate + "' ");
                pq.Sql = string.Format(@"  SELECT l.*,u.NickName FROM record.Log as l left join GServerInfo.AspNetUsers as u
                on l.UserAccount = u.UserName where l.UserAccount = '" + bdv.SearchExt + @"'
and l.CreateTime BETWEEN '{3}' and '{4}'  order by l.CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, bdv.SearchExt, bdv.StartDate, bdv.ExpirationDate);

            }

            PagedList<LogInfo> obj = new PagedList<LogInfo>(DAL.PagedListDAL<LogInfo>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }


        public static bool AddLog(LogInfo log)
        {
            return DAL.SUDAL.AddLog(log) > 0;
        }

        public static IEnumerable<AspNetUser> GetAspNetUsers()
        {
            return DAL.SUDAL.GetAspNetUsers();
        }



    }
}
