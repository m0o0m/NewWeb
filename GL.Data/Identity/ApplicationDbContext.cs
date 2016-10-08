using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;

namespace GL.Data.MWeb.Identity
{
    //// 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    //public class ApplicationUser : IdentityUser
    //{
    //    public string NickName { get; set; }

    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // 在此处添加自定义用户声明
    //        return userIdentity;
    //    }
    //}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        /// <summary>
        /// Some database fixup / model constraints
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", "GServerInfo");
            modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles", "GServerInfo");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("AspNetUserLogins", "GServerInfo");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("AspNetUserClaims", "GServerInfo");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("AspNetUserRoles", "GServerInfo");



            //// Keep this:
            //modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers", "GServerInfo");

            //// Change TUser to ApplicationUser everywhere else - IdentityUser and ApplicationUser essentially 'share' the AspNetUsers Table in the database:
            //EntityTypeConfiguration<ApplicationUser> table = modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", "GServerInfo");

            //table.Property((ApplicationUser u) => u.UserName).IsRequired();

            //// EF won't let us swap out IdentityUserRole for ApplicationUserRole here:
            //modelBuilder.Entity<ApplicationUser>().HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);
            //modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles", "GServerInfo");

            //// Leave this alone:
            //EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
            //    modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
            //        new { UserId = l.UserId, LoginProvider = l.LoginProvider, ProviderKey = l.ProviderKey }).ToTable("AspNetUserLogins", "GServerInfo");

            //entityTypeConfiguration.HasRequired<IdentityUser>((IdentityUserLogin u) => u.User);
            //EntityTypeConfiguration<IdentityUserClaim> table1 = modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims", "GServerInfo");
            //table1.HasRequired<IdentityUser>((IdentityUserClaim u) => u.User);

            //// Add this, so that IdentityRole can share a table with ApplicationRole:
            //modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles", "GServerInfo");

            //// Change these from IdentityRole to ApplicationRole:
            //EntityTypeConfiguration<ApplicationRole> entityTypeConfiguration1 = modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles", "GServerInfo");
            //entityTypeConfiguration1.Property((ApplicationRole r) => r.Name).IsRequired();



        }




    }
}