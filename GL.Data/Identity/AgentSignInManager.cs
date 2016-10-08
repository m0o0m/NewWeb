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
using GL.Data.Identity;

namespace GL.Data.AWeb.Identity
{
    // 配置要在此应用程序中使用的应用程序登录管理器。
    public class AgentSignInManager : SignInManager<AgentUser, int>
    {
        public AgentSignInManager(AgentUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AgentUser user)
        {
            return user.GenerateUserIdentityAsync((AgentUserManager)UserManager);
        }

        public static AgentSignInManager Create(IdentityFactoryOptions<AgentSignInManager> options, IOwinContext context)
        {
            return new AgentSignInManager(context.GetUserManager<AgentUserManager>(), context.Authentication);
        }
    }
}
