using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Sychev.DocumentDB.Api;

[assembly: OwinStartup(typeof(Startup))]

namespace Sychev.DocumentDB.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseWebApi(GlobalConfiguration.Configuration);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
        }
    }
}
