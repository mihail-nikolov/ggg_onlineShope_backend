using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using GGG_OnlineShop.Web.Api.App_Start;
using GGG_OnlineShop.Infrastructure;
using System;
using System.Linq;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(GGG_OnlineShop.Web.Api.Startup))]

namespace GGG_OnlineShop.Web.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            var autoMapperConfig = new AutoMapperConfig();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var neededAssemblies = assemblies.Where(a => a.FullName.Contains("GGG_OnlineShop")).ToList();
            autoMapperConfig.Execute(neededAssemblies);

            ConfigureAuth(app);

            var httpConfig = new HttpConfiguration();

            WebApiConfig.Register(httpConfig);
            httpConfig.EnsureInitialized();
            app
               .UseNinjectMiddleware(NinjectConfig.CreateKernel)
               .UseNinjectWebApi(httpConfig);
        }
    }
}
