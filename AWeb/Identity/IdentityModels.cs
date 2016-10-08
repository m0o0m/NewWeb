using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AWeb.Models
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

    public enum agentLv : uint
    {
        公司 = 0,
        股东 = 1,
        总代 = 2,
        代理 = 3
    }
    public enum revenueModel : uint
    {
        占成 = 0,
        返水 = 1
    }


    public class AgentUserLogin : IdentityUserLogin<int> { }
    public class AgentUserClaim : IdentityUserClaim<int> { }
    public class AgentUserRole : IdentityUserRole<int> { }
    public class AgentRole : IdentityRole<int, AgentUserRole>, IRole<int> { }

    public class AgentUserStore : UserStore<AgentUser, AgentRole, int, AgentUserLogin, AgentUserRole, AgentUserClaim>, IUserStore<AgentUser, int>, IDisposable
    {
        public AgentUserStore(DbContext context)
        : base(context)
        { }
        public AgentUserStore()
        : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }
    }
    public class AgentRoleStore : RoleStore<AgentRole, int, AgentUserRole>, IQueryableRoleStore<AgentRole, int>, IRoleStore<AgentRole, int>, IDisposable
    {
        public AgentRoleStore() : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }
    }

    public class AgentDbContext : IdentityDbContext<AgentUser, AgentRole, int, AgentUserLogin, AgentUserRole, AgentUserClaim>
    {
        public AgentDbContext() : base("DefaultConnection")
        {
        }

        public static AgentDbContext Create()
        {
            return new AgentDbContext();
        }

        ///// <summary>
        ///// Some database fixup / model constraints
        ///// </summary>
        ///// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AgentUser>().ToTable("AgentUsers", "GServerInfo");
            modelBuilder.Entity<AgentRole>().ToTable("AgentRoles", "GServerInfo");
            modelBuilder.Entity<AgentUserLogin>().ToTable("AgentUserLogins", "GServerInfo");
            modelBuilder.Entity<AgentUserClaim>().ToTable("AgentUserClaims", "GServerInfo");
            modelBuilder.Entity<AgentUserRole>().ToTable("AgentUserRoles", "GServerInfo");

            //var user = modelBuilder.Entity<IdentityUser>().HasKey(u => u.Id).ToTable("CustomerUsers", "GServerInfo");

            //var user = modelBuilder.Entity<AgentUser>().HasKey(u => u.Id).ToTable("AgentUsers", "GServerInfo");
            //user.Property(iu => iu.Id).HasColumnName("Id");
            //user.Property(iu => iu.Email).HasColumnName("Email").HasMaxLength(256).IsOptional();
            //user.Property(iu => iu.EmailConfirmed).HasColumnName("EmailConfirmed");
            //user.Property(iu => iu.PasswordHash).HasColumnName("PasswordHash");
            //user.Property(iu => iu.SecurityStamp).HasColumnName("SecurityStamp");
            //user.Property(iu => iu.PhoneNumber).HasColumnName("PhoneNumber");
            //user.Property(iu => iu.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
            //user.Property(iu => iu.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
            //user.Property(iu => iu.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            //user.Property(iu => iu.LockoutEnabled).HasColumnName("LockoutEnabled");
            //user.Property(iu => iu.AccessFailedCount).HasColumnName("AccessFailedCount");
            //user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            //user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            //user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            //user.Property(iu => iu.UserName).HasColumnName("UserName").HasMaxLength(256).IsRequired().HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex")
            //{
            //    IsUnique = true
            //}));




            //modelBuilder.Entity<IdentityUser>().ToTable("CustomerUsers", "game");
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //var CustomerUser = modelBuilder.Entity<CustomerUser>().HasKey(au => au.Id).ToTable("CustomerUsers", "GServerInfo");

            //CustomerUser.Property(au => au.Id).HasColumnName("Id");
            //CustomerUser.Property(au => au.Email).HasColumnName("Email").HasMaxLength(256).IsRequired();
            //CustomerUser.Property(au => au.EmailConfirmed).HasColumnName("EmailConfirmed");
            //CustomerUser.Property(au => au.PasswordHash).HasColumnName("PasswordHash");
            //CustomerUser.Property(au => au.SecurityStamp).HasColumnName("SecurityStamp");
            //CustomerUser.Property(au => au.PhoneNumber).HasColumnName("PhoneNumber");
            //CustomerUser.Property(au => au.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
            //CustomerUser.Property(au => au.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
            //CustomerUser.Property(au => au.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            //CustomerUser.Property(au => au.LockoutEnabled).HasColumnName("LockoutEnabled");
            //CustomerUser.Property(au => au.AccessFailedCount).HasColumnName("AccessFailedCount");
            //CustomerUser.Property(au => au.NickName).HasColumnName("NickName").HasMaxLength(256).IsOptional();
            //CustomerUser.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            //CustomerUser.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            //CustomerUser.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            //CustomerUser.Property(au => au.UserName).HasColumnName("UserName").HasMaxLength(256).IsRequired().HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex")
            //{
            //    IsUnique = true
            //}));

            //var role = modelBuilder.Entity<IdentityRole>().HasKey(ir => ir.Id).ToTable("CustomerRoles", "GServerInfo");
            //role.Property(ir => ir.Id).HasColumnName("Id");
            //role.Property(ir => ir.Name).HasColumnName("Name").HasMaxLength(256).IsRequired().HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex")
            //{
            //    IsUnique = true
            //}));

            //var claim = modelBuilder.Entity<IdentityUserClaim>().HasKey(iuc => iuc.Id).ToTable("CustomerUserClaims", "GServerInfo");
            //claim.Property(iuc => iuc.Id).HasColumnName("Id");
            //claim.Property(iuc => iuc.UserId).HasColumnName("UserId");
            //claim.Property(iuc => iuc.ClaimType).HasColumnName("ClaimType");
            //claim.Property(iuc => iuc.ClaimValue).HasColumnName("ClaimValue");


            //var login = modelBuilder.Entity<IdentityUserLogin>().HasKey(iul => new { iul.UserId, iul.LoginProvider, iul.ProviderKey }).ToTable("CustomerUserLogins", "GServerInfo"); //Used for third party OAuth providers
            //login.Property(iul => iul.LoginProvider).HasColumnName("LoginProvider");
            //login.Property(iul => iul.ProviderKey).HasColumnName("ProviderKey");
            //login.Property(iul => iul.UserId).HasColumnName("UserId");

            //var userRole = modelBuilder.Entity<IdentityUserRole>().HasKey(iur => new { iur.UserId, iur.RoleId }).ToTable("CustomerUserRoles", "GServerInfo");
            //userRole.Property(ur => ur.UserId).HasColumnName("UserId");
            //userRole.Property(ur => ur.RoleId).HasColumnName("RoleId");

        }

    }
}