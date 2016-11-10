using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SugarSite.Startup))]
namespace SugarSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
