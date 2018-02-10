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

        protected IHttpActionResult HandlExceptionLogging(Exception exc, string comment, string controller, [CallerMemberName]string action = "")
        {
            try
            {
                Logger.LogError(exc, comment, controller, action);
                return InternalServerError();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }
        // uncomment if needed
        //protected void HandlExceptionLogging(Exception exc, string info, string controller, [CallerMemberName]string action = "")
        //{
        //    string errorMessage = exc.Message;
        //    string innerErrorMessage = string.Empty;
        //    string innerInnerErrorMessage = string.Empty;
        //    if (exc.InnerException != null)
        //    {
        //        innerErrorMessage = exc.InnerException.Message;

        //        if (exc.InnerException.InnerException != null)
        //        {
        //            innerInnerErrorMessage = exc.InnerException.InnerException.Message;
        //        }
        //    }

        //    DbLogger.LogError(errorMessage, $"inner: {innerErrorMessage}------ innerInner: {innerInnerErrorMessage}", info, $"{controller}.{action}", DateTime.UtcNow);
        //    // if no connection to Db - no sense to write info to file
        //    //catch
        //    //{
        //    //    FileLogger.LogError($"{controller}.{action}:  exceptionMessage: {errorMessage}; innerExceptionMessage: {innerErrorMessage} ; innerInnerErrorMessage: {innerInnerErrorMessage}", controller);
        //    //}
        //}
    }
}
