using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RemusixWeb.Startup))]
namespace RemusixWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
