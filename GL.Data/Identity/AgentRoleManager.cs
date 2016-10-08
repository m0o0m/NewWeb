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


namespace GL.Data.AWeb.Identity
{

    public class AgentRoleManager : RoleManager<AgentRole, int>
    {
        public AgentRoleManager(IRoleStore<AgentRole, int> roleStore)
            : base(roleStore)
        {
        }

        public static AgentRoleManager Create(IdentityFactoryOptions<AgentRoleManager> options, IOwinContext context)
        {
            return new AgentRoleManager(new AgentRoleStore<AgentRole, int, AgentUserRole>(context.Get<AgentDbContext>()));

        }
    }


}
