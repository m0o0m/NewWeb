using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;


namespace GL.Data.OWeb.Identity
{

    public class CustomerRoleManager : RoleManager<CustomerRole>
    {
        public CustomerRoleManager(IRoleStore<CustomerRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static CustomerRoleManager Create(IdentityFactoryOptions<CustomerRoleManager> options, IOwinContext context)
        {
            return new CustomerRoleManager(new RoleStore<CustomerRole>());

        }
    }


}
