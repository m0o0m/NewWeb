using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CMeb.Startup))]
namespace CMeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
