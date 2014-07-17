using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(rssSpider.Startup))]
namespace rssSpider
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
