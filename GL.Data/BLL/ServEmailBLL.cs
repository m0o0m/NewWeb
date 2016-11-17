using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class ServEmailBLL
    {



        public static PagedList<ServEmail> GetListByPage(BaseDataView bdv)
        {


            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;
            string where = " where ServEmailTime BETWEEN '"+bdv.StartDate+"' and '"+bdv.ExpirationDate+"' ";
            pq.RecordCount = DAL.PagedListDAL<ServEmail>.GetRecordCount(@"select count(0) from ServEmail "+ where);
            pq.Sql = string.Format(@"select * from ServEmail "+ where + " order by ServEmailID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<ServEmail> obj = new PagedList<ServEmail>(DAL.PagedListDAL<ServEmail>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;



        }

        public static int Insert(ServEmail servEmail)
        {
            return ServEmailDAL.Insert(servEmail);
        }

        public static int Delete(ServEmail servEmail)
        {
            return ServEmailDAL.Delete(servEmail);
        }

        //UserStock
        public static PagedList<UserStock> GetListByUserStock(BaseDataView bdv)
        {


            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;
         
//            pq.RecordCount = DAL.PagedListDAL<UserStock>.GetRecordCount(
//                string.Format(@" 
//select count(0) from gserverinfo.e_userstock {0}",
//bdv.SearchExt==""? "": " where UserName='"+bdv.SearchExt+@"' "
//));


            pq.RecordCount = DAL.PagedListDAL<UserStock>.GetRecordCount(
              string.Format(@" 

SELECT COUNT(*) from (
select c.id,c.GroupName,c.Value,group_concat(c.UserName separator ',') as UserName
from 
(select b.*,a.UserName from e_userstockgroup as b  LEFT JOIN e_userstock as a on a.groupid = b.id
WHERE 1=1  {0}   {1}

) as c
group by c.id,c.GroupName,c.Value
) as d
",
bdv.SearchExt == "" ? "" : "  and a.groupid in (SELECT groupid from e_userstock where UserName = @UserName)",
bdv.SearchExt2 == "" ? "" : "   and GroupName = @GroupName "
), new { UserName=bdv.SearchExt, GroupName = bdv.SearchExt2 });




            pq.Sql = string.Format(@" 
select c.UpdateTime,c.id as GroupID,c.GroupName,c.Value,group_concat(c.UserName separator ',') as UserName
from 
(select b.*,a.UserName from e_userstockgroup as b  LEFT JOIN e_userstock as a on a.groupid = b.id
WHERE 1=1 {2}   {3}
) as c
group by c.id,c.GroupName,c.Value,c.UpdateTime
order by c.UpdateTime desc 
limit {0}, {1}", pq.StartRowNumber, pq.PageSize,
bdv.SearchExt == "" ? "" : "  and a.groupid in (SELECT groupid from e_userstock where UserName = @UserName) ",
bdv.SearchExt2 == "" ? "" : "   and GroupName = @GroupName "
);

            PagedList<UserStock> obj = new PagedList<UserStock>(
                DAL.PagedListDAL<UserStock>.GetListByPage(pq,new { UserName=bdv.SearchExt, GroupName = bdv.SearchExt2 }),
                pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;



        }


        //public static List<UserStock> GetUserStock
        public static int AddStock(int id,Int64 addStock)
        {
            return ServEmailDAL.AddStock(id, addStock);
        }


        public static int AddStock(string username, Int64 addStock)
        {
            return ServEmailDAL.AddStock(username, addStock);
        }


        public static int AddStockGroup(string groupName, Int64 value) {
            return ServEmailDAL.AddStockGroup(groupName, value);
        }

        //

        public static int AddStockUser(int id, string UserName)
        {
            return ServEmailDAL.AddStockUser(id, UserName);
        }

        public static UserStock GetModelStock(int id) {
            return ServEmailDAL.GetModelStock(id);
        }


        public static IEnumerable<UserStock> GetModelStockByUserName(string userName)
        {
            return ServEmailDAL.GetModelStockByUserName(userName);
        }

        /// <summary>
        /// 查询其他组的有这些用户的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IEnumerable<UserStock> GetOtherUsers(int id,string userName)
        {
            string[] uses = userName.Split(',');
            string s = "";
            for (int i = 0; i < uses.Length; i++) {

                if (i == uses.Length - 1)
                {
                    s += "'" + uses[i].Trim() + "'";
                }
                else {
                    s += "'" + uses[i].Trim() + "',";
                }

              
            }
           
            return ServEmailDAL.GetOtherUsers(id, s);
        }

        public static UserStock GetModelStockAll(int id) {
            return ServEmailDAL.GetModelStockAll(id);
        }
    }
}
