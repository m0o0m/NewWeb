using GL.Command.DBUtility;
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
    public class UserEmailBLL
    {
        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


        public static PagedList<UserEmail> GetListByPage(BaseDataView bdv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = bdv.Page;
            pq.PageSize = 10;
            if (bdv.Channels <= 0)
            {
                string where = " where UETime BETWEEN '" + bdv.StartDate + "' and '" + bdv.ExpirationDate + "'";
                pq.RecordCount = DAL.PagedListDAL<UserEmail>.GetRecordCount(@"select count(0) from UserEmail" + where);
                pq.Sql = string.Format(@"select * from UserEmail" + where + " order by UEID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            }
            else {

                /*
                select * from UserEmail,gamedata.Role as r
where UETime BETWEEN '2016-4-25 00:00:00' and '2016-4-26 00:00:00' 
and r.Agent = 10010
 order by UEID desc;


                */



                string where = " where UETime BETWEEN '" + bdv.StartDate + "' and '" + bdv.ExpirationDate + "' and  FIND_IN_SET(r.ID,UserEmail.UEUserID ) >0 and r.Agent = "+ bdv.Channels;
                pq.RecordCount = DAL.PagedListDAL<UserEmail>.GetRecordCount(@" select count(*) from UserEmail,"+ database1 + @".Role as r" + where);
                pq.Sql = string.Format(@"select UserEmail.* from UserEmail,"+ database1 + @".Role as r " + where + " order by UEID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            }

            PagedList<UserEmail> obj = new PagedList<UserEmail>(DAL.PagedListDAL<UserEmail>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;

        }

        public static int Delete(UserEmail model)
        {
            return DAL.UserEmailDAL.Delete(model);
        }

        public static int Add(UserEmail model)
        {
            return DAL.UserEmailDAL.Add(model);
        }

        public static UserEmail GetModelByID(UserEmail mi)
        {
            UserEmail cust = DAL.UserEmailDAL.GetModelByID(mi);
            return cust;
        }

        public static int Update(UserEmail model)
        {
            return DAL.UserEmailDAL.Update(model);
        }

        public static List<UEUser> GetUserGroupList()
        {
            return UserEmailDAL.GetUserGroupList();
        }
        public static IEnumerable<UEUser> GetUserTotal(BaseDataView bdv)
        {
            return UserEmailDAL.GetUserTotal(bdv);
        }
    }
}
