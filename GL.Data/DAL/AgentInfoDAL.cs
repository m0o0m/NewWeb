using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;
using System.Data;

namespace GL.Data.DAL
{
    public class AgentInfoDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        internal static AgentInfo GetModelByID(AgentInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AgentInfo> i = cn.Query<AgentInfo>(@"select * from AgentInfo where AgentID = @AgentID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static AgentInfo GetModelByIDForLower(AgentInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AgentInfo> i = cn.Query<AgentInfo>(@"select * from AgentInfo where HigherLevel = @AgentID order by EarningsRatio desc limit 0,1", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        internal static AgentInfo GetModelByIDForLowerR(AgentInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AgentInfo> i = cn.Query<AgentInfo>(@"select * from AgentInfo where HigherLevel = @AgentID order by RebateRate desc limit 0,1", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static AgentInfo GetModel(AgentInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AgentInfo> i = cn.Query<AgentInfo>(@"select * from AgentInfo where AgentAccount = @AgentAccount", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        internal static int Update(AgentInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"UPDATE AgentInfo SET AgentAccount =@AgentAccount, AgentPasswd =@AgentPasswd, AgentLv=@AgentLv, AgentName =@AgentName, AgentQQ =@AgentQQ, AgentTel =@AgentTel, AgentEmail =@AgentEmail, Deposit =@Deposit, InitialAmount =@InitialAmount, AmountAvailable =@AmountAvailable, HavaAmount =@HavaAmount, HigherLevel =@HigherLevel, LowerLevel =@LowerLevel, AgentState =@AgentState, OnlineState =@OnlineState, RegisterTime =@RegisterTime,LoginTime  =@LoginTime, Recharge =@Recharge, LoginIP =@LoginIP, Drawing =@Drawing, DrawingPasswd =@DrawingPasswd, RevenueModel =@RevenueModel, EarningsRatio =@EarningsRatio, RebateRate = @RebateRate, JurisdictionID =@JurisdictionID, Extend_isDefault =@Extend_isDefault where AgentID = @AgentID", model);
                cn.Close();
                return i;
            }
        }



        internal static int Delete(AgentInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"delete from AgentInfo where AgentID = @AgentID", model);
                cn.Close();
                return i;
            }
        }

        internal static int Add(AgentInfo model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"INSERT INTO AgentInfo (AgentAccount, AgentName, AgentPasswd, AgentLv, AgentQQ, AgentTel, AgentEmail, Deposit, InitialAmount, AmountAvailable, HavaAmount, HigherLevel, LowerLevel, AgentState, OnlineState, RegisterTime, LoginIP, LoginTime, Recharge, Drawing, DrawingPasswd, RevenueModel, EarningsRatio, RebateRate, JurisdictionID, Extend_isDefault) VALUES (@AgentAccount, @AgentName, @AgentPasswd, @AgentLv, @AgentQQ, @AgentTel, @AgentEmail, @Deposit, @InitialAmount, @AmountAvailable, @HavaAmount, @HigherLevel, @LowerLevel, @AgentState, @OnlineState, @RegisterTime, @LoginIP, @LoginTime, @Recharge, @Drawing, @DrawingPasswd, @RevenueModel, @EarningsRatio, @RebateRate, @JurisdictionID, @Extend_isDefault)", model);
                cn.Close();
                return i;
            }
        }


        internal static List<AgentInfo> GetAgentList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<AgentInfo> i = cn.Query<AgentInfo>(@"select * from AgentInfo where AgentLv = @AgentLv", new AgentInfo { AgentLv = agentLv.代理 });
                cn.Close();
                return i.ToList();
            }
        }

        internal static int ResetDifaultMain()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"UPDATE AgentInfo SET Extend_isDefault = false");
                cn.Close();
                return i;
            }
        }

        internal static List<AgentInfoGroup> GetAgentGroupList() {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
              
                IEnumerable<AgentInfoGroup> i = cn.Query<AgentInfoGroup>("GServerInfo.sp_get_allchannel", param: new { }, commandType: CommandType.StoredProcedure);

                cn.Close();
                return i.ToList();
            }
        }
    }
}
