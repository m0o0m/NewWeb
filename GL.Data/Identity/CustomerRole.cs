using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GL.Data.OWeb.Identity
{


    public class CustomerUserLogin : IdentityUserLogin<string> { }
    public class CustomerUserClaim : IdentityUserClaim<string> { }
    public class CustomerUserRole : IdentityUserRole<string> { }
    public class CustomerRole : IdentityRole<string, CustomerUserRole>, IRole<string> { }

    public class CustomerUserStore<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim> : UserStore<CustomerUser, CustomerRole, string, CustomerUserLogin, CustomerUserRole, CustomerUserClaim>, IUserStore<CustomerUser, string>, IDisposable
    {
        public CustomerUserStore(DbContext context)
        : base(context)
        { }
        public CustomerUserStore()
        : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }
    }

}
