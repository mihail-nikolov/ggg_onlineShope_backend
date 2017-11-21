﻿namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Data.Services.Contracts;
    using Infrastructure;
    using Models;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class MakesController : BaseController
    {
        private readonly IVehicleMakesService makes;

        public MakesController(IVehicleMakesService makes)
        {
            this.makes = makes;
        }

        [HttpGet]
        [Route("api/Makes")]
        public IHttpActionResult GetVehicleMakes()
        {
            try
            {
                var vehicleMakes = this.makes.GetAll().To<VehicleMakeResponseModel>().OrderBy(x => x.Name).ToList();
                return this.Ok(vehicleMakes);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
