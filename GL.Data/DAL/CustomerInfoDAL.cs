using System.Collections.Generic;
using System.Linq;
using GL.Dapper;
using MySql.Data.MySqlClient;
using GL.Data.Model;
using GL.Command.DBUtility;

namespace GL.Data.DAL
{
    public class CustomerInfoDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        internal static CustomerInfo GetModel(CustomerInfo ci)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<CustomerInfo> i = cn.Query<CustomerInfo>(@"select * from CustomerInfo where CustomerAccount = @CustomerAccount", ci);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static CustomerInfo GetModelByID(CustomerInfo ci)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<CustomerInfo> i = cn.Query<CustomerInfo>(@"select * from CustomerInfo where CustomerID = @CustomerID", ci);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static int UpdateState(CustomerInfo ci)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update CustomerInfo set CustomerState = @CustomerState where CustomerID = @CustomerID", ci);
                cn.Close();
                return i;
            }
        }


        internal static int Delete(CustomerInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"delete from CustomerInfo where CustomerID = @CustomerID", model);
                cn.Close();
                return i;
            }
        }

        internal static int Add(CustomerInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"INSERT INTO customerinfo (CustomerAccount, CustomerPwd, CustomerState, CreateDate, CustomerName) VALUES (@CustomerAccount, @CustomerPwd, @CustomerState, @CreateDate, @CustomerName) ", model);
                cn.Close();
                return i;
            }
        }
        internal static int Update(CustomerInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update CustomerInfo set CustomerAccount =@CustomerAccount, CustomerPwd =@CustomerPwd, CustomerState =@CustomerState, CreateDate =@CreateDate, CustomerName =@CustomerName where CustomerID = @CustomerID", model);
                cn.Close();
                return i;
            }
        }

    }
}