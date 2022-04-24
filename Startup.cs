using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Course_Store.Startup))]
namespace Course_Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
