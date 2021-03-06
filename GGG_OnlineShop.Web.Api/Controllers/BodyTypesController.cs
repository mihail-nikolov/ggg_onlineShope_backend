﻿namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Data.Services.Contracts;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;

    [RoutePrefix("api/BodyTypes")]
    public class BodyTypesController : BaseController
    {
        private readonly IVehiclesService vehicles;
        private readonly IVehicleBodyTypesService bodyTypes;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public BodyTypesController(IVehicleBodyTypesService bodyTypes,
                                   IVehiclesService vehicles, ILogsService dbLogger)
            : base(dbLogger)
        {
            this.vehicles = vehicles;
            this.bodyTypes = bodyTypes;
        }

        [HttpPost]
        [Route("GetByMakeAndModelIds")]
        public IHttpActionResult GetVehicleBodyTypeByMakeAndModelIds(VehicleBodyTypesRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var bodyTypeIds = this.vehicles.GetBodyTypeIdsByModelIdAndMakeId(requestModel.MakeId, requestModel.ModelId);

                List<VehicleBodyTypeResponseModel> bodyTypes = new List<VehicleBodyTypeResponseModel>();
                foreach (var bodyTypeId in bodyTypeIds)
                {
                    if (bodyTypeId != null)
                    {
                        var bodyType = this.Mapper.Map<VehicleBodyTypeResponseModel>(this.bodyTypes.GetById(bodyTypeId));
                        bodyTypes.Add(bodyType);
                    }
                }

                return this.Ok(bodyTypes.OrderBy(x => x.Code));
            }
            catch (Exception e)
            {
               HandlExceptionLogging(e, "", controllerName);
               return InternalServerError(); 
            }
        }
    }
}
