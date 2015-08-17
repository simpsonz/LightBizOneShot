using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LightBizOneShot.Startup))]
namespace LightBizOneShot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
