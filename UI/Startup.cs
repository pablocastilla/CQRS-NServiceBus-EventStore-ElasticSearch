using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UI.Startup))]
namespace UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
