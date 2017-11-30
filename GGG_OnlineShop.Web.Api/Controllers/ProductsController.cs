namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Data.Services.Contracts;
    using Infrastructure;
    using Models;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [RoutePrefix("api/Products")]
    public class ProductsController : BaseController
    {
        private readonly IVehiclesService vehicles;
        private readonly IVehicleGlassesService glasses;
        private readonly IProductQuantitiesService productQuantities;

        public ProductsController(IVehiclesService vehicles, IVehicleGlassesService glasses,
                                    IProductQuantitiesService productQuantities)
        {
            this.vehicles = vehicles;
            this.glasses = glasses;
            this.productQuantities = productQuantities;
        }

        // TODO crossing elements between DBs via GetGlass nad GetInterchangeableParts
        // be really careful with nulls! model and bodyType could be nulls => the Ids will be nulls.. (double check!!!)
        // probably service returns it OK, but showing models etc is NOK in the fronted
        // how to pass code (some codes have non alphanumeric symbol
        [HttpPost]
        [Route("GetByVehicleInfoAndProductType")]
        public IHttpActionResult GetByMakeModelBodyTypeIdsAndProductType(VehicleGlassRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var vehicle = this.vehicles.GetVehicleByMakeModelAndBodyTypeIds(requestModel.MakeId, requestModel.ModelId, requestModel.BodyTypeId);
                var glasses = this.vehicles.GetApplicableGLassesByProductType(vehicle, requestModel.ProductType)
                                                                             .To<VehicleGlassShortResponseModel>()
                                                                             .OrderBy(x => x.Description)
                                                                             .ToList();
                return this.Ok(glasses);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpPost]
        [Route("GetByVehicleInfo")]
        public IHttpActionResult GetByMakeModelBodyTypeIds(VehicleGlassRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var vehicle = this.vehicles.GetVehicleByMakeModelAndBodyTypeIds(requestModel.MakeId, requestModel.ModelId, requestModel.BodyTypeId);
                var glasses = this.vehicles.GetApplicableGLasses(vehicle).To<VehicleGlassShortResponseModel>()
                                                                         .OrderBy(x => x.Description)
                                                                         .ToList();
                return this.Ok(glasses);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpGet]
        [Route("GetByEuroCode/{eurocode}")]
        public IHttpActionResult GetByEuroCode(string eurocode)
        {
            try
            {
                var glass = this.Mapper.Map<VehicleGlassResponseModel>(this.glasses.GetByEuroCode(eurocode));
                return this.Ok(glass);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpGet]
        [Route("GetByOesCode/{oescode}")]
        public IHttpActionResult GetByOesCode(string oescode)
        {
            try
            {
                var glass = this.Mapper.Map<VehicleGlassResponseModel>(this.glasses.GetByOesCode(oescode));
                return this.Ok(glass);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public IHttpActionResult GetByCode(string code)
        {
            try
            {
                if (code.Length < 4)
                {
                    return this.BadRequest("enter at least 4 symbols");
                }

                var glasses = this.glasses.GetByRandomCode(code).To<VehicleGlassResponseModel>()
                                                                         .ToList();
                return this.Ok(glasses);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var glass = this.Mapper.Map<VehicleGlassResponseModel>(this.glasses.GetById(id));
                return this.Ok(glass);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpGet]
        [Route("GetQuantities/{code}")]
        public IHttpActionResult GetQuantities(string code)
        {
            try
            {

                var quantities = this.productQuantities.GetQuantitiesByCode(code);
                return this.Ok(quantities);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        //[HttpGet]
        //[Route("api/Products/GetAccessoriesByGlassId/{glassId}")]
        //public IHttpActionResult GetAccessoryByGlassId(int glassId)
        //{
        //    try
        //    {
        //        var accessories = this.glasses.GetAccessories(glassId).To<VehicleGlassAccessoryResponseModel>()
        //                                                                 .ToList();

        //        return this.Ok(accessories);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
        //                                         e.Message));
        //    }
        //}
    }
}
