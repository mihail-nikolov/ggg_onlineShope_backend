namespace GGG_OnlineShop.Web.Api.Controllers
{
    using AutoMapper;
    using Data.Services.Contracts;
    using Infrastructure;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.Http;

    public abstract class BaseController : ApiController
    {
        //public ICacheService Cache { get; set; }

        public BaseController(ILogsService dbLogger)
        {
            Logger = dbLogger;
        }

        protected ILogsService Logger { get; set; }

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }

        protected void HandlExceptionLogging(Exception exc, string comment, string controller, [CallerMemberName]string action = "")
        {
            try
            {
                Logger.LogError(exc, comment, controller, action);
            }
            catch (Exception e)
            {
                // if no connection to Db - no sense to write info to file
            }
        }
    }
}
