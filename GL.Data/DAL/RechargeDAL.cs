using MySql.Data.MySqlClient;
using GL.Dapper;
using GL.Command.DBUtility;
using System.Linq;
using GL.Data.Model;
using System.Collections.Generic;
using System;

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

        internal static Recharge GetModelByBillNo(Recharge model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Recharge> i = cn.Query<Recharge>(@"select * from QQZoneRecharge where BillNo = @BillNo", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static Recharge GetFirstModelListByUserID(Recharge model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Recharge> i = cn.Query<Recharge>(@"select * from QQZoneRecharge where UserID = @UserID and IsFirst=1", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static int Add(Recharge model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"replace into QQZoneRecharge(Num,BillNo,OpenID,PayItem,UserID,Money,Chip,ChipType,IsFirst,CreateTime,PF,VersionInfo,ActualMoney,ProductNO) values (@Num,@BillNo,@OpenID,@PayItem,@UserID,@Money,@Chip,@ChipType,@IsFirst,@CreateTime,@PF,'" + model.VersionInfo+ "',@ActualMoney,@ProductNO);", model);
                cn.Close();
                return i;
            }
        }

        internal static int UpdateReachTime(string billNO)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update  QQZoneRecharge set ReachTime=@ReachTime where BillNO=@BillNO",
                    new { ReachTime = DateTime.Now,BillNO  = billNO  }
                    );
                cn.Close();
                return i;
            }
        }


        public static string GetVersion(Recharge model) {

            string sql = @"
select if(ifnull(a.VersionInfo ,'')='' ,b.VersionInfo ,a.VersionInfo) VersionInfo 
from (select VersionInfo from BG_LoginRecord where UserID = " + model.UserID + @" order by LoginTime desc limit 1 ) a cross join (
select VersionInfo from Role where id = " + model.UserID + @" ) b ;
";
            try
            {
                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();

                    IEnumerable<VersionData> obj = cn.Query<VersionData>(sql);
                    VersionData o = obj.FirstOrDefault();

                    string versioninfo = "";
                    if (o != null)
                    {
                        versioninfo = o.VersionInfo;
                    }
                    cn.Close();
                    return versioninfo;
                }

            }
            catch {
            

                return model.UserID.ToString();
            }
                
           
         
        }

        internal static int Delete(Recharge model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {

                cn.Open();
                int i = cn.Execute(@"delete from QQZoneRecharge where BillNo = @BillNo and UserID=@UserID);", model);
                cn.Close();
                return i;
            }
        }


    }
}
