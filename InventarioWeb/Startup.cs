using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InventarioWeb.Startup))]
namespace InventarioWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
