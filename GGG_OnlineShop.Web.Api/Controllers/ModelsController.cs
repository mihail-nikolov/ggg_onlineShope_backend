namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Data.Services.Contracts;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [RoutePrefix("api/Models")]
    public class ModelsController : BaseController
    {
        private readonly IVehiclesService vehicles;
        private readonly IVehicleModelsService models;

        public ModelsController(IVehicleModelsService models,
                                IVehiclesService vehicles)
        {
            this.models = models;
            this.vehicles = vehicles;
        }

        [HttpGet]
        [Route("GetByMakeId/{vehicleMakeId}")]
        public IHttpActionResult GetVehicleModelByMake(int vehicleMakeId)
        {
            try
            {
                var modelIds = this.vehicles.GetModelIdsByMakeId(vehicleMakeId);
                List<VehicleModelResponseModel> models = new List<VehicleModelResponseModel>();
                foreach (var modeId in modelIds)
                {
                    if (modeId != null)
                    {
                        var model = this.Mapper.Map<VehicleModelResponseModel>(this.models.GetById(modeId));
                        models.Add(model);
                    }
                }

                return this.Ok(models.OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
