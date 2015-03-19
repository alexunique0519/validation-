using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YYoec.Startup))]
namespace YYoec
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
