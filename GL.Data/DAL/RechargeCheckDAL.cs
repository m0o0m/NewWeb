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
    public class RechargeCheckDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


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

        internal static IEnumerable<RechargeCheck> GetModelByUserID(RechargeCheck model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<RechargeCheck> i = cn.Query<RechargeCheck>(@"
select r.* from "+ database2+ @".RechargeCheck as r left JOIN "+ database1+ @".QQZoneRecharge as q  on  r.SerialNo = q.BillNo
where r.UserID = @UserID and r.SerialNo like 'AppTreasure%' and q.BillNo is NULL", model);
                cn.Close();
                return i;
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
                int i = cn.Execute(@"insert into RechargeCheck(SerialNo,UserID,ProductID,Money,CreateTime,CheckInfo,AgentID) values (@SerialNo,@UserID,@ProductID,@Money,@CreateTime,@CheckInfo,@AgentID);", model);
                cn.Close();
                return i;
            }
        }



        internal static int AddOrderIP(UserIpInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"insert into "+ database3+ @".Charge_OrderIPRecord(OrderID,CreateTime,UserID,ChargeType) values 
                                    (@OrderID,@CreateTime,@UserID,@ChargeType);", model);
                cn.Close();
                return i;
            }
        }


        internal static int AddCallBackOrderIP(UserIpInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"insert into "+ database3 + @".Charge_CallBackIPRecord(OrderID,CallBackIP,CallBackTime,CallBackChargeType,CallBackUserID,Method) values 
                                    (@OrderID,@CallBackIP,@CallBackTime,@CallBackChargeType,@CallBackUserID,@Method);", model);
                cn.Close();
                return i;
            }
        }

        public static IEnumerable<UserIpInfo> GetCallBackOrder(string orderID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<UserIpInfo> i = cn.Query<UserIpInfo>(@"select * from "+ database3 + @".Charge_CallBackIPRecord where OrderID ='"+ orderID+"'");
                cn.Close();
                return i;
            }
        }


        public static IEnumerable<CallBackRechargeIP> GetCallBackIP(UserIpInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<CallBackRechargeIP> i = cn.Query<CallBackRechargeIP>(string.Format(@"
select CallBackIP,count(*) as Num from (
select *  from "+ database3 + @".Charge_CallBackIPRecord 
where 1=1   {0} {1}
GROUP BY CallBackUserID,CallBackIP,CallBackChargeType
)as a
GROUP BY CallBackIP
", model.ChargeType < 0?"":" and CallBackChargeType="+model.ChargeType, string.IsNullOrEmpty(model.OrderIP)?"": " and CallBackIP = '" + model.OrderIP + "'"));
                cn.Close();
                return i;
            }
        }


        public  static bool AddChargeIP(string ip, string type)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"

insert into   "+ database3 + @".Charge_IP(IP,ChargeType)
values('"+ip+"',"+type+@")

");
                cn.Close();
                return i>0;
            }
        }


        public static bool DeleteChargeIP(string ip, string type)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"

delete from  "+ database3 + @".Charge_IP
where  IP='"+ ip + @"' and ChargeType="+ type +@"

");
                cn.Close();
                return i > 0;
            }
        }


        public static bool IsTrustIp(string ip, string type)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<dynamic> i = cn.Query(@"

select * from  "+ database3 + @".Charge_IP
where  IP='" + ip + @"' and ChargeType=" + type + @"

");
              
                cn.Close();
                
              
                return i.Count() > 0;
            }
        }



        internal static int AddAppTreInfo(AppTreasure model)
        {
         
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
            
                int i = cn.Execute(@"
insert into "+ database1+ @".RechargeBanlance(
Appid,Balance,Openid,Openkey,Pay_token,Pf,
Pfkey,Session_id,Session_type,Userid,CreateTime
) values 
(
@Appid,@Balance,@Openid,@Openkey,@Pay_token,@Pf,
@Pfkey,@Session_id,@Session_type,@Userid,@CreateTime
);", model);
                cn.Close();
                return i;
            }
        }

        internal static AppTreasure GetModelByUserID(string userid)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AppTreasure> i = cn.Query<AppTreasure>(@"select * from "+ database1 + @".RechargeBanlance where Userid=@userId ORDER BY CreateTime desc limit 1", new { userId = userid });
                cn.Close();
                return i.FirstOrDefault();
            }
        }


    }
}
