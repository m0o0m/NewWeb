using MySql.Data.MySqlClient;
using GL.Dapper;
using GL.Command.DBUtility;
using System.Linq;
using GL.Data.Model;
using System.Collections.Generic;

namespace GL.Data.DAL
{
    public class RechargeDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static Recharge GetModelByID(Recharge model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Recharge> i = cn.Query<Recharge>(@"select * from QQZoneRecharge where AdminID = @AdminID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }



        internal static int Add(Recharge model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"insert into QQZoneRecharge(BillNo,OpenID,PayItem,UserID,Money,Chip,ChipType,IsFirst,CreateTime,PF) values (@BillNo,@OpenID,@PayItem,@UserID,@Money,@Chip,@ChipType,@IsFirst,@CreateTime,@PF);", model);
                cn.Close();
                return i;
            }
        }


    }
}
