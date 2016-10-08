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
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入电子邮件服务可发送电子邮件。
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入 SMS 服务可发送短信。
            return Task.FromResult(0);
        }
    }


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
