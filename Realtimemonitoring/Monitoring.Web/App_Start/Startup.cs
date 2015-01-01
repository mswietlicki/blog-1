using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Sychev.Monitoring.Web;
using Sychev.Monitoring.Web.Code.Repository;
using Sychev.Monitoring.Web.Hubs;

[assembly: OwinStartup(typeof(Startup))]

namespace Sychev.Monitoring.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
            GlobalHost.DependencyResolver.Register(typeof(DiagramHub), () => new DiagramHub(new PointsRepository()));

            //http://www.asp.net/signalr/overview/signalr-20/extensibility/dependency-injection
            //если надо будет сделать нормальный DI, то в статье описано. Пока это сильно много кода, ради одного hub
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true,  
            };
            app.MapSignalR(hubConfiguration);
        }
    }
}
