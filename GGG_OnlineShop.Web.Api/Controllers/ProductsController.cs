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
    using System.Reflection;
    using System.Web.Http;

    [RoutePrefix("api/Products")]
    public class ProductsController : BaseController
    {
        private readonly IVehiclesService _vehicles;
        private readonly IVehicleGlassesService _glasses;
        private readonly IProductQuantitiesService _productQuantities;
        private readonly IUsersService _users;
        private readonly string _controllerName = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public ProductsController(IVehiclesService vehicles, IVehicleGlassesService glasses,
                                  IProductQuantitiesService productQuantities, IUsersService users, ILogsService dbLogger)
            : base(dbLogger)
        {
            this._vehicles = vehicles;
            this._glasses = glasses;
            this._productQuantities = productQuantities;
            this._users = users;
        }

        [HttpGet]
        [Route("GetItemByFullCode")]
        public IHttpActionResult GetItemByFullCode(string eurocode = "", string materialNumber = "", string industryCode = "", string localCode = "")
        {
            IHttpActionResult result = null;
            VehicleGlassResponseModel glass = null;
            if (!string.IsNullOrEmpty(eurocode))
            {
                glass = this.Mapper.Map<VehicleGlassResponseModel>(this._glasses.GetByEuroCode(eurocode));
            }

            if (glass == null && !string.IsNullOrEmpty(materialNumber))
            {
                glass = this.Mapper.Map<VehicleGlassResponseModel>(this._glasses.GetByMaterialNumber(materialNumber));
            }

            if (glass == null && !string.IsNullOrEmpty(industryCode))
            {
                glass = this.Mapper.Map<VehicleGlassResponseModel>(this._glasses.GetByIndustryCode(industryCode));
            }

            if (glass == null && !string.IsNullOrEmpty(localCode))
            {
                glass = this.Mapper.Map<VehicleGlassResponseModel>(this._glasses.GetByLocalCode(localCode));
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
                        var glass = this.Mapper.Map<VehicleGlassResponseModel>(this._glasses.GetById(id));
                        return this.Ok(glass);
                    }
                    else
                    {
                        List<VehicleGlassShortResponseModel> glassesList;

                        if (!string.IsNullOrEmpty(eurocode))
                        {
                            if (eurocode.Length < GlobalConstants.CodeMinLength)
                            {
                                result = this.BadRequest(GlobalConstants.CodeMinLengthErrorMessage);
                            }
                            else
                            {
                                glassesList = this._glasses.GetGlassesByEuroCode(eurocode).To<VehicleGlassShortResponseModel>().ToList();
                                result = this.Ok(glassesList);
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
                                glassesList = this._glasses.GetByOesCode(oescode).To<VehicleGlassShortResponseModel>().ToList();
                                result = this.Ok(glassesList);
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
                                glassesList = this._glasses.GetByRandomCode(code).To<VehicleGlassShortResponseModel>()
                                                                                       .ToList();
                                result = this.Ok(glassesList);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetPriceAndQUantities/{productId}")]
        public IHttpActionResult GetPriceAndQuantities(int productId)
        {
            try
            {
                var product = this._glasses.GetById(productId);
                var code = this._glasses.GetCode(product);

                var isUserLogged = this.User.Identity.IsAuthenticated;
                User user = null;
                if (isUserLogged)
                {
                    user = this._users.GetByEmail(this.User.Identity.Name);
                }

                var quantities = this._productQuantities.GetPriceAndQuantitiesByCode(code, user);
                return this.Ok(quantities);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
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
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
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
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
            }
        }

        private IQueryable<VehicleGlassShortResponseModel> FindGlassesByVehicleInfo(VehicleGlassRequestModel requestModel)
        {
            var vehicle = this._vehicles.GetVehicleByMakeModelAndBodyTypeIds(requestModel.MakeId, requestModel.ModelId, requestModel.BodyTypeId);
            IQueryable<VehicleGlassShortResponseModel> glasses = new List<VehicleGlassShortResponseModel>().AsQueryable();

            if (!string.IsNullOrEmpty(requestModel.ProductType))
            {
                var applicableGlasses =
                    this._vehicles.GetApplicableGLassesByProductType(vehicle, requestModel.ProductType);

                if (applicableGlasses != null)
                {
                    glasses = applicableGlasses?.To<VehicleGlassShortResponseModel>().OrderBy(x => x.Description);
                }
            }
            else
            {
                var applicableGlasses =
                    this._vehicles.GetApplicableGLasses(vehicle);

                if (applicableGlasses != null)
                {
                    glasses = applicableGlasses?.To<VehicleGlassShortResponseModel>().OrderBy(x => x.Description);
                }
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
        //        
        //                                         
        //    }
        //}
    }
}
