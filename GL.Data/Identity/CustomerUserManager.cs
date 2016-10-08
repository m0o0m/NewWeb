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

namespace GL.Data.OWeb.Identity
{

    // 配置此应用程序中使用的应用程序用户管理器。UserManager 在 ASP.NET Identity 中定义，并由此应用程序使用。
    public class CustomerUserManager : UserManager<CustomerUser, string>
    {
        public CustomerUserManager(IUserStore<CustomerUser, string> store)
            : base(store)
        {
        }

        public static CustomerUserManager Create(IdentityFactoryOptions<CustomerUserManager> options, IOwinContext context)
        {
            var manager = new CustomerUserManager(new CustomerUserStore<CustomerUser, CustomerRole, string, CustomerUserLogin, CustomerUserRole, CustomerUserClaim>(context.Get<CustomerDbContext>()));
            // 配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<CustomerUser, string>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = false
            };

            // 配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // 配置用户锁定默认值
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<CustomerUser, string>
            {
                MessageFormat = "你的安全代码是 {0}"
            });
            manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<CustomerUser, string>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<CustomerUser, string>(dataProtectionProvider.Create("ASP.NET Identity OWeb"));
            }
            return manager;
        }
    }


    //public class SimpleUserValidator<TUser, TKey> : IIdentityValidator<TUser>
    //    where TUser : class, IUser<TKey>
    //    where TKey : IEquatable<TKey>
    //{
    //    private readonly UserManager<TUser, TKey> _manager;

    //    public SimpleUserValidator(UserManager<TUser, TKey> manager)
    //    {
    //        _manager = manager;
    //    }

    //    public async Task<IdentityResult> ValidateAsync(TUser item)
    //    {
    //        var errors = new List<string>();
    //        if (string.IsNullOrWhiteSpace(item.UserName))
    //            errors.Add("Username is required");

    //        if (_manager != null)
    //        {
    //            var otherAccount = await _manager.FindByNameAsync(item.UserName);
    //            if (otherAccount != null && !otherAccount.Id.Equals(item.Id))
    //                errors.Add("Select a different username. An account has already been created with this username.");
    //        }

    //        return errors.Any()
    //            ? IdentityResult.Failed(errors.ToArray())
    //            : IdentityResult.Success;
    //    }
    //}
}
