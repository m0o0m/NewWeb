using GL.Command.DBUtility;
using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Linq;
using System.Web;
using GL.Dapper;
using GL.Dapper.Extensions;
using MySql.Data.MySqlClient;
using Data.Model;

namespace Data.DAL
{
    public class CustomerServCenterDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        internal static IEnumerable<CustomerServCenter> GetList(int GUUserID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                //var predicate = Predicates.Field<CustomerServCenter>(f => f.GUUserID, Operator.Eq, true);
                //IEnumerable<CustomerServCenter> csc = cn.GetList<CustomerServCenter>(predicate);

                IEnumerable<CustomerServCenter> csc = cn.Query<CustomerServCenter>(@"select * from gserverinfo.customerservcenter a, (SELECT CSCMainID FROM gserverinfo.customerservcenter where GUUserID = @GUUserID limit 0, 20) b where a.CSCMainID = b.CSCMainID or a.CSCSubId = b.CSCMainID", new { GUUserID = GUUserID });

                cn.Close();

                return csc;
            }
        }

        internal static int Insert(CustomerServCenter cust)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"insert into CustomerServCenter(CSCSubId,GUUserID,CSCTime,CSCTitle,CSCType,CSCContent,CSCState,GUName,GUType)values (@CSCSubId,@GUUserID,@CSCTime,@CSCTitle,@CSCType,@CSCContent,@CSCState,@GUName,@GUType)", cust);

                cn.Close();

                return i;
            }


        }

        internal static int Update(CustomerServCenter c)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"update CustomerServCenter set CSCState = @CSCState where CSCMainID = @CSCMainID and GUUserID = @GUUserID", c);

                cn.Close();

                return i;
            }
        }
    }
}