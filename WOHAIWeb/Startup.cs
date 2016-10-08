using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WOHAIWeb.Startup))]
namespace WOHAIWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
