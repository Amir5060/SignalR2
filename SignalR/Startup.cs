using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using SignalR.DAL;
using SignalR.Hubs;

[assembly: OwinStartupAttribute(typeof(SignalR2.Startup))]
namespace SignalR2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(
        typeof(ChatHub2),
        () => new ChatHub2(new DataLayer()));

            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
