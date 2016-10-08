using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MWeb.Startup))]
namespace MWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
