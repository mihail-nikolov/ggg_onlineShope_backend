using System.Threading.Tasks;
using System.Web.Http.Results;
using GGG_OnlineShop.Data.Services.ExternalDb.Models;

namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Common;
    using Data.Services.Contracts;
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
        private readonly IEmailsService _emails;
        private readonly IVehiclesService _vehicles;
        private readonly IVehicleGlassesService _glasses;
        private readonly IProductQuantitiesService _productQuantities;
        private readonly IUsersService _users;
        private readonly string _controllerName = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public ProductsController(IEmailsService emails, IVehiclesService vehicles, IVehicleGlassesService glasses,
                                  IProductQuantitiesService productQuantities, IUsersService users, ILogsService dbLogger)
            : base(dbLogger)
        {
            _vehicles = vehicles;
            _glasses = glasses;
            _productQuantities = productQuantities;
            _users = users;
            _emails = emails;
        }

        [HttpGet]
        [Route("GetDetailedInfo")]
        public IHttpActionResult GetDetailedInfo(int? id, string eurocode = "", string materialNumber = "", string industryCode = "", string localCode = "")
        {
            try
            {
                if (id == null && string.IsNullOrWhiteSpace(eurocode) && string.IsNullOrWhiteSpace(materialNumber) &&
                    string.IsNullOrWhiteSpace(industryCode) && string.IsNullOrWhiteSpace(localCode))
                {
                    return BadRequest("No code passed");
                }

                VehicleGlass glass = null;
                if (id != null)
                {
                    glass = _glasses.GetById(id);
                }

                if (!string.IsNullOrEmpty(eurocode))
                {
                    glass = _glasses.GetByEuroCode(eurocode);
                }

                if (glass == null && !string.IsNullOrEmpty(materialNumber))
                {
                    glass = this._glasses.GetByMaterialNumber(materialNumber);
                }

                if (glass == null && !string.IsNullOrEmpty(industryCode))
                {
                    glass = this._glasses.GetByIndustryCode(industryCode);
                }

                if (glass == null && !string.IsNullOrEmpty(localCode))
                {
                    glass = _glasses.GetByLocalCode(localCode);
                }

                var glassResponse = this.Mapper.Map<VehicleGlassResponseModel>(glass);
                User user = null;
                if (User.Identity.IsAuthenticated)
                {
                    user = _users.GetByEmail(User.Identity.Name);
                }

                glassResponse.ProductInfos = GetProductInfos(glass, user);

                return Ok(glassResponse);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GenerateGlassesPositions")]
        public IHttpActionResult GenerateGlassesPositions()
        {
            try
            {
                this._glasses.CreateGlassesPostions();

                return Ok();
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult Get(string eurocode = "", string oescode = "", string code = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(eurocode) && string.IsNullOrWhiteSpace(oescode) && string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest(GlobalConstants.NeededCodesErrorMessage);
                }

                IHttpActionResult result = Ok();
                IQueryable<VehicleGlass> glassesQuery = new List<VehicleGlass>().AsQueryable();

                if (!string.IsNullOrEmpty(eurocode))
                {
                    if (eurocode.Length < GlobalConstants.CodeMinLength)
                    {
                        result = this.BadRequest(GlobalConstants.CodeMinLengthErrorMessage);
                    }
                    else
                    {
                        glassesQuery = this._glasses.GetGlassesByEuroCode(eurocode);
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
                        glassesQuery = this._glasses.GetByOesCode(oescode);
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
                        glassesQuery = _glasses.GetByRandomCode(code);
                    }
                }

                List<VehicleGlassShortResponseModel> glassesResult = new List<VehicleGlassShortResponseModel>();

                // get quantities for each
                if (result.GetType() == typeof(OkResult))
                {
                    User user = null;
                    if (User.Identity.IsAuthenticated)
                    {
                        user = _users.GetByEmail(User.Identity.Name);
                    }

                    foreach (var glass in glassesQuery)
                    {
                        var glassToAdd = Mapper.Map<VehicleGlassShortResponseModel>(glass);
                        glassToAdd.ProductInfos = GetProductInfos(glass, user);

                        glassesResult.Add(glassToAdd);
                    }

                    result = Ok(glassesResult);
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
                User user = null;
                if (User.Identity.IsAuthenticated)
                {
                    user = this._users.GetByEmail(this.User.Identity.Name);
                }

                var product = _glasses.GetById(productId);
                var quantities = GetProductInfos(product, user);
                return this.Ok(quantities);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
            }
        }

        private IEnumerable<ProductInfoResponseModel> GetProductInfos(VehicleGlass glass, User user)
        {
            var code = this._glasses.GetCode(glass);

            var quantities = this._productQuantities.GetPriceAndQuantitiesByCode(code, user);
            return quantities;
        }

        [HttpPost]
        [Route("GetPositions")]
        public IHttpActionResult GetPositions(VehicleGlassRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var positions = FindProductsByVehicleInfo(model).Select(x => x.Position).Distinct();
                return Ok(positions);
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
                var glasses = FindProductsByVehicleInfo(requestModel);

                return Ok(glasses.ToList());
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
            }
        }

        private List<VehicleGlassShortResponseModel> FindProductsByVehicleInfo(VehicleGlassRequestModel requestModel)
        {
            var vehicle = _vehicles.GetVehicleByMakeModelAndBodyTypeIds(requestModel.MakeId, requestModel.ModelId, requestModel.BodyTypeId);
            var glassesResult = new List<VehicleGlassShortResponseModel>();

            var applicableGlasses = !string.IsNullOrEmpty(requestModel.ProductType) 
                ? _vehicles.GetApplicableGLassesByProductType(vehicle, requestModel.ProductType) 
                : _vehicles.GetApplicableGLasses(vehicle);

            if (applicableGlasses != null)
            {
                User user = null;
                if (User.Identity.IsAuthenticated)
                {
                    user = _users.GetByEmail(User.Identity.Name);
                }

                foreach (var glass in applicableGlasses)
                {
                    var glassToAdd = Mapper.Map<VehicleGlassShortResponseModel>(glass);
                    glassToAdd.ProductInfos = GetProductInfos(glass, user);

                    glassesResult.Add(glassToAdd);
                }
            }

            return glassesResult;
        }

        [HttpPost]
        public async Task<IHttpActionResult> CheckAvailability(EnquiryRequestModel enquiry)
        {
            try
            {
                IHttpActionResult result = Ok();

                string enquiryText = $@"
Мара:                    {enquiry.Make}
Модел:                   {enquiry.Model}
Година:                  {enquiry.ModelYear}
Каросерия и бр. врати:   {enquiry.Body}
Тип Продукт:             {enquiry.ProductType}
Номер на Рама:           {enquiry.VIN}
Допълнителна информация: {enquiry.Description}
Име:                     {enquiry.Name}
Email:                   {enquiry.Email}
Телефон:                 {enquiry.Phone}
";

                if (enquiry.EnquireToRuse)
                {
                    await _emails.SendEmail(GlobalConstants.EmailRuse, "Запитване за продукт", enquiryText, GlobalConstants.SMTPServer,
                         GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                }

                if (enquiry.EnquireToSofia)
                {
                    await _emails.SendEmail(GlobalConstants.EmailSofia, "Запитване за продукт", enquiryText, GlobalConstants.SMTPServer,
                        GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                }

                await _emails.SendEmail(enquiry.Email, "Запитване за продукт", enquiryText, GlobalConstants.SMTPServer,
                    GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);

                return result;
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", _controllerName);
                return InternalServerError();
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
        //        
        //                                         
        //    }
        //}
    }
}
