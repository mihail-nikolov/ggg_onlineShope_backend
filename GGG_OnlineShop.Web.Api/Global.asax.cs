namespace GGG_OnlineShop.Web.Api
{
    using App_Start;
    using Data.Common;
    using Data.Services;
    using Data.Services.Contracts;
    using InternalApiDB.Data;
    using InternalApiDB.Models;
    using System;
    using System.Web.Http;
    using System.Web.Mvc;

    public class WebApiApplication : System.Web.HttpApplication
    {
        public ILogsService logger = new LogsService(new InternalDbRepository<Log>(new InternalApiDbContext()));

        protected void Application_Start()
        {
            DataBaseConfig.Initialize();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            try
            {
                logger.LogError(exc, "unhandled ", "WebApplication-unhandled");
            }
            catch (Exception)
            {
            }
        }
    }
}
