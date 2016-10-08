using GL.Command.DBUtility;
using System.Collections.Generic;
using GL.Dapper;
using MySql.Data.MySqlClient;
using GL.Data.Model;
using System;
using System.Linq;

namespace GL.Data.DAL
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

                IEnumerable<CustomerServCenter> csc = cn.Query<CustomerServCenter>(@"select * from CustomerServCenter a, (SELECT CSCMainID FROM CustomerServCenter where GUUserID = @GUUserID limit 0, 20) b where a.CSCMainID = b.CSCMainID or a.CSCSubId = b.CSCMainID", new { GUUserID = GUUserID });

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




        internal static int Delete(CustomerServCenter cust)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"Delete from CustomerServCenter where CSCMainID = @CSCMainID or CSCSubId = @CSCMainID", cust);

                cn.Close();

                return i;
            }


        }

        internal static IEnumerable<CustomerServCenter> GetListWithCSCSubId(CustomerServCenter cust)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<CustomerServCenter> csc = cn.Query<CustomerServCenter>(@"select * from CustomerServCenter where CSCMainID = @CSCMainID or CSCSubId = @CSCMainID order by CSCMainID asc", cust);
                cn.Close();

                return csc;
            }
        }

        internal static CustomerServCenter GetModel(CustomerServCenter model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<CustomerServCenter> csc = cn.Query<CustomerServCenter>(@"select * from CustomerServCenter where CSCMainID = @CSCMainID", model);
                cn.Close();

                return csc.FirstOrDefault();
            }
        }

        internal static int UpdateForManage(CustomerServCenter c)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"update CustomerServCenter set CSCUpdateTime='"+DateTime.Now.ToString()+"', CSCState = @CSCState where CSCMainID = @CSCMainID", c);

                cn.Close();

                return i;
            }
        }

    }
}