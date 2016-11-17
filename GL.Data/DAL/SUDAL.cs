using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;

namespace GL.Data.DAL
{
    public class SUDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionString");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        public static IEnumerable<Resource> GetResourceList(string roleId,int level)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
               
                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"select * from "+ database2 + @".AspNetResource where Level = @Level", new Resource {  Level=level});
                cn.Close();
                return i;
            }
        }
        public static IEnumerable<Resource> GetResourceListByUserId(string userid)
        {
            
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"select  re.*,
(CASE WHEN rr.Id is null THEN false ELSE true END) as Checked from AspNetResource as re
left JOIN AspNetAuthority  as rr
on rr.Id = '" + userid + "' and rr.ResourceNo =re.`No` order by re.`Group` asc,re.OrderIndex asc");
                cn.Close();
                return i;
            }
        }

        public static IEnumerable<Resource> GetResourceListByRoleId(string roleId)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"select  re.*,
(CASE WHEN rr.Id is null THEN false ELSE true END) as Checked from AspNetResource as re
left JOIN AspNetAuthority  as rr
on rr.Id = '"+roleId+ "' and rr.ResourceNo =re.`No` order by re.`Group` asc,re.OrderIndex asc");
                cn.Close();
                return i;
            }
        }

        public static IEnumerable<Resource> GetUserRoleResourceListByUserId(string userid) {


                  using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"
            select re.*,
(CASE WHEN rr.Id is null THEN false ELSE true END) as Checked from AspNetResource as re
left JOIN AspNetAuthority as rr
on rr.Id in (
  select RoleId from AspNetUserRoles where UserId = '"+ userid + @"'
    union ALL
    select '"+ userid + @"'
) and rr.ResourceNo = re.`No` order by re.`Group` asc,re.No asc
");
                cn.Close();
                return i;
            }

        }


        public static AspNetUser GetAspNetUserByUserName(string userName)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<AspNetUser> i = cn.Query<AspNetUser>(@"select * from AspNetUsers where UserName='"+ userName + "'");
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        public static AspNetRole GetAspNetRoleByRoleName(string userName)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<AspNetRole> i = cn.Query<AspNetRole>(@"select * from AspNetRoles where Name='" + userName + "'");
                cn.Close();
                return i.FirstOrDefault();
            }
        }



        public static IEnumerable<Resource> GetAllResourceListByUserId(string userid)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"
select DISTINCT * from (
select OrderIndex,`No`,`Name`,PNo,Action,`Level`,`Group`,LiId from AspNetAuthority ,AspNetResource  where AspNetAuthority.id IN
(
select RoleId from AspNetUserRoles where UserId = @Id
union ALL
select @Id
) and AspNetAuthority.ResourceNo = AspNetResource.`No`
union all
SELECT DISTINCT OrderIndex,`No`,`Name`,PNo,Action,`Level`,`Group`,LiId from AspNetResource,(
  SELECT CONCAT(',',ResourceNo) as ResourceNo  from AspNetAuthority where AspNetAuthority.id IN
	(
	select RoleId from AspNetUserRoles where UserId = @Id
	union ALL
	select @Id
	)
) as pr
where LOCATE(CONCAT(',',No),pr.ResourceNo)>0 and CONCAT(',',No)!=pr.ResourceNo
) as t
order by t.Group asc, t.OrderIndex  desc
", new Resource {  Id = userid });
                cn.Close();
                return i;
            }
        }


        public static IEnumerable<Resource> GetAllResourceList()
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"select * from "+ database2 + @".AspNetResource as ar order by ar.Group asc, ar.OrderIndex desc", new Resource {  });
                cn.Close();
                return i;
            }
        }

        public static IEnumerable<Resource> GetAdminResourceList()
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"select *,1 as Checked from "+ database2 + @".AspNetResource as ar order by ar.Group asc ,ar.No asc", new Resource { });
                cn.Close();
                return i;
            }
        }


        internal static int AddRoleResource(string roleid,string resourceNo)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                string sql = " delete from AspNetAuthority where Id = '" + roleid + "';";
                string[] strs = resourceNo.Split(',');
                if (strs.Length > 0) {
                    sql += "insert into AspNetAuthority(Id,ResourceNo,Type) values";
                    for (int i = 0; i < strs.Length; i++)
                    {
                        sql += " ('"+roleid+"','"+strs[i]+"',1),";
                    }
                    sql = sql.Trim(',');
                }
               
              
                cn.Open();
              
                cn.Query<Resource>(sql, param: new {  });

                return 1;
            }



        }

        internal static int AddUserResource(string userid, string resourceNo)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                string sql = " delete from AspNetAuthority where Id = '" + userid + "';";
                string[] strs = resourceNo.Split(',');
                if (strs.Length > 0)
                {
                    sql += "insert into AspNetAuthority(Id,ResourceNo,Type) values";
                    for (int i = 0; i < strs.Length; i++)
                    {
                        sql += " ('" + userid + "','" + strs[i] + "',2),";
                    }
                    sql = sql.Trim(',');
                }


                cn.Open();

                cn.Query<Resource>(sql, param: new { });

                return 1;
            }



        }

        public static bool CheckUserAction(string userid, string action)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"
select * from (
select `No`,`Name`,PNo,Action,`Level`,`Group`,LiId from AspNetAuthority ,AspNetResource  where AspNetAuthority.id IN
(
select RoleId from AspNetUserRoles where UserId = @Id
union ALL
select @Id
) and AspNetAuthority.ResourceNo = AspNetResource.`No`
union all
SELECT DISTINCT `No`,`Name`,PNo,Action,`Level`,`Group`,LiId from AspNetResource,(
  SELECT CONCAT(',',ResourceNo) as ResourceNo  from AspNetAuthority where AspNetAuthority.id IN
	(
	select RoleId from AspNetUserRoles where UserId = @Id
	union ALL
	select @Id
	)
) as pr
where LOCATE(CONCAT(',',No),pr.ResourceNo)>0 and CONCAT(',',No)!=pr.ResourceNo
) as t
where t.Action like '%"+action+@"%'
order by t.Group asc,t.No desc
", new Resource { Id = userid});
                cn.Close();
                if (i.Count() > 0)
                {
                    return true;
                }
                else {
                    return false;
                }
              
            }
        }


        public static Resource GetModelByFirstUrl(string url)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<Resource> i = cn.Query<Resource>(@"select * from AspNetResource where CONCAT('#',Action)  like '%#"+url+"%'");
                cn.Close();
                return i.FirstOrDefault();
            }
        }




        //============================================================================

        
         public static UserLimit GetLimitModel(UserLimit limit)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<UserLimit> i = cn.Query<UserLimit>(@"
  select * from AspNetLimit where UserId =@UserId and Category=@Category", limit);
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static int AddUserLimit(string userid, string LimitNo,int category)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                string sql = " delete from AspNetLimit where UserId = '" + userid + "' and category="+ category+";";
             
                sql += "insert into AspNetLimit(UserId,AccessNo,Category) values";
                   
                sql += " ('" + userid + "','" + LimitNo + "',"+ category + ")";
                



                cn.Open();

                cn.Query<Resource>(sql, param: new { });

                return 1;
            }



        }


        public static int AddLog(LogInfo log)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                string sql = "insert into "+ database3+ @".Log(UserAccount,CreateTime,LoginIP,OperModule,Content,Detail) values";

                sql += " ('" + log.UserAccount + "','" + log.CreateTime + "','" + log.LoginIP + "','" + log.OperModule + "','" + log.Content + "','" + log.Detail + "')";




                cn.Open();

                cn.Query<Resource>(sql, param: new { });

                return 1;
            }
        }


        public static IEnumerable<AspNetUser> GetAspNetUsers()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<AspNetUser> i = cn.Query<AspNetUser>(@"
  select * from "+ database2 + @".AspNetUsers");
                cn.Close();
                return i;
            }
        }

        //GetAspNetUsersByUserName
        public static IEnumerable<AspNetUser> GetAspNetUsersByUserName(string userName)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                IEnumerable<AspNetUser> i = cn.Query<AspNetUser>(@"
  select * from "+ database2 + @".AspNetUsers where UserName in ("+ userName + @")");
                cn.Close();
                return i;
            }
        }
    }
}
