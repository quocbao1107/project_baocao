using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(doan.Startup))]
namespace doan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
