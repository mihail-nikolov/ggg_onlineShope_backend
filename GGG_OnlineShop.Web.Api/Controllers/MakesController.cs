namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Data.Services.Contracts;
    using Infrastructure;
    using Models;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;

    [RoutePrefix("api/Makes")]
    public class MakesController : BaseController
    {
        private readonly IVehicleMakesService makes;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public MakesController(IVehicleMakesService makes, ILogsService dbLogger)
            : base(dbLogger)
        {
            this.makes = makes;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var vehicleMakes = this.makes.GetAll().To<VehicleMakeResponseModel>().OrderBy(x => x.Name).ToList();
                return this.Ok(vehicleMakes);
            }
            catch (Exception e)
            {
               HandlExceptionLogging(e, "", controllerName);
               return InternalServerError(); 
            }
        }
    }
}
