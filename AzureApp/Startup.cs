using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureApp.Startup))]
namespace AzureApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
