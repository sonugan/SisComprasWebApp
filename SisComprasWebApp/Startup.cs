using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SisComprasWebApp.Startup))]
namespace SisComprasWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
