using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalR2.Startup))]
namespace SignalR2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
