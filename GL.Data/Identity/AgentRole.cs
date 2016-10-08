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

    public class AgentUserLogin : IdentityUserLogin<int> { }
    public class AgentUserClaim : IdentityUserClaim<int> { }
    public class AgentUserRole : IdentityUserRole<int> { }
    public class AgentRole : IdentityRole<int, AgentUserRole>, IRole<int>
    {

        public AgentRole()
        {

        }
        public AgentRole(string name)
            : this()
        {
            this.Name = name;
        }
        public AgentRole(string name, string description)
            : this(name)
        {
            this.Description = description;
        }

        public virtual string Description { get; set; }

    }
    public class AgentUserStore<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim> : UserStore<AgentUser, AgentRole, int, AgentUserLogin, AgentUserRole, AgentUserClaim>, IUserStore<AgentUser, int>, IDisposable
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
    public class AgentRoleStore<TRole, TKey, TUserRole> : RoleStore<AgentRole, int, AgentUserRole>, IQueryableRoleStore<AgentRole, int>, IRoleStore<AgentRole, int>, IDisposable
    {
        public AgentRoleStore(DbContext context) : base(context)
        { }
        public AgentRoleStore() : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }
    }


}