using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GL.Data.AWeb.Identity
{
    // 可以通过向 AgentUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class AgentUser : IdentityUser<int, AgentUserLogin, AgentUserRole, AgentUserClaim>, IUser<int>
    {

        //public int Id { get; set; }
        //public string Email { get; set; }
        //public int EmailConfirmed { get; set; }
        //public string SecurityStamp { get; set; }
        //public string PhoneNumber { get; set; }
        //public int PhoneNumberConfirmed { get; set; }
        //public int TwoFactorEnabled { get; set; }
        //public DateTime LockoutEndDateUtc { get; set; }
        //public int LockoutEnabled { get; set; }
        //public int AccessFailedCount { get; set; }
        
        public string AgentName { get; set; }
        public agentLv AgentLv { get; set; }
        public string AgentQQ { get; set; }
        public decimal Deposit { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal AmountAvailable { get; set; }
        public decimal HavaAmount { get; set; }
        public int HigherLevel { get; set; }
        public int LowerLevel { get; set; }
        public int AgentState { get; set; }
        public int OnlineState { get; set; }
        public DateTime RegisterTime { get; set; }
        public string LoginIP { get; set; }
        public DateTime LoginTime { get; set; }
        public decimal Recharge { get; set; }
        public decimal Drawing { get; set; }
        public string DrawingPasswd { get; set; }
        public revenueModel RevenueModel { get; set; }
        public int EarningsRatio { get; set; }
        public int RebateRate { get; set; }
        public string JurisdictionID { get; set; }
        public int Extend_isDefault { get; set; }




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AgentUser, int> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }



}