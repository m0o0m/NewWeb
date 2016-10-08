using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GL.Data.MWeb.Identity
{

    public class ApplicationUserLogin : IdentityUserLogin<string> { }
    public class ApplicationUserClaim : IdentityUserClaim<string> { }
    public class ApplicationUserRole : IdentityUserRole<string> { }
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>, IRole<string> {

        public ApplicationRole() {
            this.Id = Guid.NewGuid().ToString();
        }
        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }
        public ApplicationRole(string name, string description)
            : this(name)
        {
            this.Description = description;
        }

        public virtual string Description { get; set; }

    }

    public class ApplicationUserStore<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim> : UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUserStore<ApplicationUser, string>, IDisposable
    {
        public ApplicationUserStore(DbContext context)
        : base(context)
        { }
        public ApplicationUserStore()
        : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }
    }


    public class ApplicationRoleStore<TRole> : RoleStore<ApplicationRole, string, ApplicationUserRole>, IQueryableRoleStore<ApplicationRole>, IQueryableRoleStore<ApplicationRole, string>, IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore(DbContext context) : base(context)
        { }
        public ApplicationRoleStore() : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }


    }

}