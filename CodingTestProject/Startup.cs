using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodingTestProject.Startup))]
namespace CodingTestProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
