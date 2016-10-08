using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OWeb.Models
{
    //// 可以通过向 CustomerUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    //public class CustomerUser : IdentityUser
    //{
    //    public string NickName { get; set; }

    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<CustomerUser> manager)
    //    {
    //        // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // 在此处添加自定义用户声明
    //        return userIdentity;
    //    }
    //}

    public class CustomerDbContext : IdentityDbContext<CustomerUser>
    {
        public CustomerDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static CustomerDbContext Create()
        {
            return new CustomerDbContext();
        }


        ///// <summary>
        ///// Some database fixup / model constraints
        ///// </summary>
        ///// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerUser>().ToTable("CustomerUsers", "GServerInfo");

            //var user = modelBuilder.Entity<IdentityUser>().HasKey(u => u.Id).ToTable("CustomerUsers", "GServerInfo");
            //var user = modelBuilder.Entity<IdentityUser>().HasKey(u => u.Id);
            //user.Property(iu => iu.Id).HasColumnName("Id");
            //user.Property(iu => iu.Email).HasColumnName("Email").HasMaxLength(256).IsRequired();
            //user.Property(iu => iu.EmailConfirmed).HasColumnName("EmailConfirmed");
            //user.Property(iu => iu.PasswordHash).HasColumnName("PasswordHash");
            //user.Property(iu => iu.SecurityStamp).HasColumnName("SecurityStamp");
            //user.Property(iu => iu.PhoneNumber).HasColumnName("PhoneNumber");
            //user.Property(iu => iu.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
            //user.Property(iu => iu.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
            //user.Property(iu => iu.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            //user.Property(iu => iu.LockoutEnabled).HasColumnName("LockoutEnabled");
            //user.Property(iu => iu.AccessFailedCount).HasColumnName("AccessFailedCount");
            ////user.Property(iu => iu.UserName).HasColumnName("UserName").HasMaxLength(256).IsRequired();
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