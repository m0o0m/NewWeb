using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AWeb.Startup))]
namespace AWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
