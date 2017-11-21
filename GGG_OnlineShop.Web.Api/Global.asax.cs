namespace GGG_OnlineShop.Web.Api
{
    using App_Start;
    using System.Web.Http;
    using System.Web.Mvc;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DataBaseConfig.Initialize();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
