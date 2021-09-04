using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FreDX.Startup))]
namespace FreDX
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
