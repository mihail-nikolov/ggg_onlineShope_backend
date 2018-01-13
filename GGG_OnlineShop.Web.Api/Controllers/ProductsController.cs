namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Common;
    using Data.Services.Contracts;
    using Infrastructure;
    using InternalApiDB.Models;
    using Models;
    using System;
    using System.Collections.Generic;
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
        private readonly IUsersService users;

        public ProductsController(IVehiclesService vehicles, IVehicleGlassesService glasses,
                                  IProductQuantitiesService productQuantities, IUsersService users)
        {
            this.vehicles = vehicles;
            this.glasses = glasses;
            this.productQuantities = productQuantities;
            this.users = users;
        }


        [HttpGet]
        // TODO unittest
        [Route("GetItemByFullCode")]
        public IHttpActionResult GetItemByFullCode(string eurocode = "", string materialNumber = "", string industryCode = "", string localCode = "")
        {
            IHttpActionResult result = null;
            VehicleGlassResponseModel glass = null;
            if (!string.IsNullOrEmpty(eurocode))
            {
                 glass = this.Mapper.Map<VehicleGlassResponseModel>(this.glasses.GetByEuroCode(eurocode));
            }
             
            if (glass == null && !string.IsNullOrEmpty(materialNumber))
            {
                glass = this.Mapper.Map<VehicleGlassResponseModel>(this.glasses.GetByMaterialNumber(materialNumber));
            }

            if (glass == null && !string.IsNullOrEmpty(industryCode))
            {
                glass = this.Mapper.Map<VehicleGlassResponseModel>(this.glasses.GetByIndustryCode(industryCode));
            }

            if (glass == null && !string.IsNullOrEmpty(localCode))
            {
                glass = this.Mapper.Map<VehicleGlassResponseModel>(this.glasses.GetByLocalCode(localCode));
            }

            result = Ok(glass);

            if (glass == null)
            {
                result = BadRequest("No code passed");
            }

            return result;
        }

        [HttpGet]
        public IHttpActionResult Get(int? id, string eurocode = "", string oescode = "", string code = "")
        {
            try
            {
                IHttpActionResult result;
                if (id == null && string.IsNullOrEmpty(eurocode) && string.IsNullOrEmpty(oescode) && string.IsNullOrEmpty(code))
                {
                    result = this.BadRequest(GlobalConstants.NeededCodesErrorMessage);
                }
                else
                {
                    if (id != null)
                    {
                        var glass = this.Mapper.Map<VehicleGlassResponseModel>(this.glasses.GetById(id));
                        return this.Ok(glass);
                    }
                    else
                    {
                        List<VehicleGlassShortResponseModel> glasses;

                        if (!string.IsNullOrEmpty(eurocode))
                        {
                            if (eurocode.Length < GlobalConstants.CodeMinLength)
                            {
                                result = this.BadRequest(GlobalConstants.CodeMinLengthErrorMessage);
                            }
                            else
                            {
                                glasses = this.glasses.GetGlassesByEuroCode(eurocode).To<VehicleGlassShortResponseModel>().ToList();
                                result = this.Ok(glasses);
                            }
                        }
                        else if (!string.IsNullOrEmpty(oescode))
                        {
                            if (oescode.Length < GlobalConstants.CodeMinLength)
                            {
                                result = this.BadRequest(GlobalConstants.CodeMinLengthErrorMessage);
                            }
                            else
                            {
                                glasses = this.glasses.GetByOesCode(oescode).To<VehicleGlassShortResponseModel>().ToList();
                                result = this.Ok(glasses);
                            }
                        }
                        else
                        {
                            if (code.Length < GlobalConstants.CodeMinLength)
                            {
                                result = this.BadRequest(GlobalConstants.CodeMinLengthErrorMessage);
                            }
                            else
                            {
                                glasses = this.glasses.GetByRandomCode(code).To<VehicleGlassShortResponseModel>()
                                                                                       .ToList();
                                result = this.Ok(glasses);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpGet]
        [Route("GetPriceAndQUantities/{productId}")]
        public IHttpActionResult GetPriceAndQuantities(int productId)
        {
            try
            {
                var product = this.glasses.GetById(productId);
                var code = this.glasses.GetCode(product);

                var isUserLogged = this.User.Identity.IsAuthenticated;
                User user = null;
                if (isUserLogged)
                {
                    user = this.users.GetByEmail(this.User.Identity.Name);
                }

                var quantities = this.productQuantities.GetPriceAndQuantitiesByCode(code, user);
                return this.Ok(quantities);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpPost]
        [Route("GetProductTypes")]
        public IHttpActionResult GetProductTypes(VehicleGlassRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var productTypes = this.FindGlassesByVehicleInfo(model).Select(x => x.ProductType).Distinct().ToList();
                return this.Ok(productTypes);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpPost]
        [Route("FindByVehicleInfo")]
        public IHttpActionResult FindByVehicleInfo(VehicleGlassRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var glasses = this.FindGlassesByVehicleInfo(requestModel);

                return this.Ok(glasses.ToList());
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        private IQueryable<VehicleGlassShortResponseModel> FindGlassesByVehicleInfo(VehicleGlassRequestModel requestModel)
        {
            var vehicle = this.vehicles.GetVehicleByMakeModelAndBodyTypeIds(requestModel.MakeId, requestModel.ModelId, requestModel.BodyTypeId);
            IQueryable<VehicleGlassShortResponseModel> glasses;

            if (!string.IsNullOrEmpty(requestModel.ProductType))
            {
                glasses = this.vehicles.GetApplicableGLassesByProductType(vehicle, requestModel.ProductType)
                                                                         .To<VehicleGlassShortResponseModel>()
                                                                         .OrderBy(x => x.Description);
            }
            else
            {
                glasses = this.vehicles.GetApplicableGLasses(vehicle).To<VehicleGlassShortResponseModel>()
                                                                     .OrderBy(x => x.Description);
            }

            return glasses;
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
