using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sports.Startup))]
namespace Sports
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
