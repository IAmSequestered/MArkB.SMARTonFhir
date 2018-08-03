using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MArkB.SMARTonFhir.Client.Startup))]
namespace MArkB.SMARTonFhir.Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
