using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CWeb.Startup))]
namespace CWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
