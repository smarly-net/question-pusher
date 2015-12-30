using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PushAll.Web.Startup))]
namespace PushAll.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
