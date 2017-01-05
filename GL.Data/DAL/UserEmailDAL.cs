using GL.Command.DBUtility;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;
using MySql.Data.MySqlClient;

namespace GL.Data.DAL
{
    public class UserEmailDAL
    {

        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        internal static UserEmail GetModelByID(UserEmail model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<UserEmail> i = cn.Query<UserEmail>(@"select * from UserEmail where UEID = @UEID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static int Update(UserEmail model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update UserEmail set UEUserID = @UEUserID, UETitle = @UETitle, UEContent = @UEContent, UEAuthor = @UEAuthor, UEItemType = @UEItemType, UEItemValue = @UEItemValue, UEItemNum = @UEItemNum where UEID = @UEID", model);
                cn.Close();
                return i;
            }
        }


        internal static List<UEUser> GetUserGroupList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                IEnumerable<UEUser> i = cn.Query<UEUser>("select UserName ,concat(UserName ,'(' ,NickName,')') NickName from AspNetUsers order by ID;");

                cn.Close();
                return i.ToList();
            }
        }

        internal static IEnumerable<UEUser> GetUserTotal(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select ifnull(sum(case UEItemType when 1 then UEItemValue*ifnull(PeopleNum,1) else 0 end) ,0) MailGold 
  ,ifnull(sum(case UEItemType when 2 then UEItemValue*ifnull(PeopleNum,1) else 0 end) ,0) MailDimoad 
  ,ifnull(sum(case UEItemType when 3 then UEItemValue*ifnull(PeopleNum,1) else 0 end) ,0) MailJifen 
from UserEmail 
where UEItemType in (1 ,2 ,3) and UEAuthor = case '{0}' when '' then UEAuthor else '{0}' end 
  and UETime >= '{1}' and UETime < '{2}' ;
                ", bdv.SearchExt, bdv.StartDate, bdv.ExpirationDate);

                IEnumerable<UEUser> i = cn.Query<UEUser>(str.ToString());
                cn.Close();
                return i;
            }
        }

        internal static int Delete(UserEmail model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"delete from UserEmail where UEID = @UEID", model);
                cn.Close();
                return i;
            }
        }

        internal static int Add(UserEmail model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"insert into UserEmail(UEUserID,UETitle,UEContent,UEAuthor,UETime,UEItemType,UEItemValue,UEItemNum,IsGlobal,PeopleNum) values (@UEUserID,@UETitle,@UEContent,@UEAuthor,@UETime,@UEItemType,@UEItemValue,@UEItemNum,@IsGlobal,@PeopleNum);", model);
                cn.Close();
                return i;
            }
        }

    }
}
