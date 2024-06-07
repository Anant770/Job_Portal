using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Job_Portal.Startup))]
namespace Job_Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
