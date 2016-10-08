using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OWeb.Startup))]
namespace OWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
