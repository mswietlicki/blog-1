using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace Sychev.Monitoring.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
         
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(30);
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //NinjectHttpContainer.RegisterModules(NinjectModules.Modules);
            //NinjectContainer.RegisterModules(NinjectModules.Modules);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}