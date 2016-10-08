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
using OWeb.Models;

namespace OWeb
{


    // 配置要在此应用程序中使用的应用程序登录管理器。
    public class CustomerSignInManager : SignInManager<CustomerUser, string>
    {
        public CustomerSignInManager(CustomerUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(CustomerUser user)
        {
            return user.GenerateUserIdentityAsync((CustomerUserManager)UserManager);
        }

        public static CustomerSignInManager Create(IdentityFactoryOptions<CustomerSignInManager> options, IOwinContext context)
        {
            return new CustomerSignInManager(context.GetUserManager<CustomerUserManager>(), context.Authentication);
        }
    }
}