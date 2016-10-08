using GL.Command.DBUtility;
using GL.Pay.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;

namespace GL.Pay.Data.DAL
{
    public class RechargeCheckDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;


        internal static RechargeCheck GetModelByID(RechargeCheck model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<RechargeCheck> i = cn.Query<RechargeCheck>(@"select * from RechargeCheck where ID = @ID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        internal static RechargeCheck GetModelBySerialNo(RechargeCheck model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<RechargeCheck> i = cn.Query<RechargeCheck>(@"select * from RechargeCheck where SerialNo = @SerialNo", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static int Update(RechargeCheck model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update RechargeCheck set SerialNo = @SerialNo, UserID = @UserID, ProductID = @ProductID, Money = @Money where ID = @ID", model);
                cn.Close();
                return i;
            }
        }



        internal static int Delete(RechargeCheck model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"delete from RechargeCheck where SerialNo = @SerialNo", model);
                cn.Close();
                return i;
            }
        }

        internal static int Add(RechargeCheck model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"insert into RechargeCheck(SerialNo,UserID,ProductID,Money,CreateTime) values (@SerialNo,@UserID,@ProductID,@Money,@CreateTime);", model);
                cn.Close();
                return i;
            }
        }



    }
}
