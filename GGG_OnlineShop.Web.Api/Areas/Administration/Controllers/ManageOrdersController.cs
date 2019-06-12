using System.Threading.Tasks;
using GGG_OnlineShop.InternalApiDB.Models.Enums;

namespace GGG_OnlineShop.Web.Api.Areas.Administration.Controllers
{
    using Common;
    using Api.Controllers;
    using Data.Services.Contracts;
    using System.Web.Http;
    using System;
    using System.Linq;
    using Models.OrderedItems;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Reflection;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [RoutePrefix("api/Administration/ManageOrders")]
    public class ManageOrdersController : BaseController
    {
        private readonly IOrdersService orders;
        private readonly IEmailsService emails;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public ManageOrdersController(IOrdersService orders, IEmailsService emails, ILogsService dbLogger)
            : base(dbLogger)
        {
            this.orders = orders;
            this.emails = emails;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(string status = null)
        {
            try
            {
                var allOrders = this.orders.GetAll();
                List<OrderResponseModelWIthUserInfo> ordersToReturn;
                if (!string.IsNullOrWhiteSpace(status))
                {
                    DeliveryStatus statusEnum = (DeliveryStatus)Enum.Parse(typeof(DeliveryStatus), status, true);
                    ordersToReturn = allOrders.Where(x => x.Status == statusEnum)
                        .OrderByDescending(x => x.CreatedOn)
                        .ThenBy(x => x.Id)
                        .To<OrderResponseModelWIthUserInfo>()
                        .ToList();
                }
                else
                {
                    ordersToReturn = allOrders.OrderBy(x => x.Status)
                        .ThenByDescending(x => x.CreatedOn)
                        .ThenBy(x => x.Id)
                        .To<OrderResponseModelWIthUserInfo>()
                        .ToList();
                }

                return this.Ok(ordersToReturn);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", controllerName);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> Update(OrderRequestUpdateStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = this.orders.GetById(model.Id);
                product.Status = model.Status;
                this.orders.Save();

                var updatedOrder = this.Mapper.Map<OrderResponseModelWIthUserInfo>(product);
                await emails.SendEmail(product.UserЕmail, string.Format(GlobalConstants.OrderUpdated, product.Id),
                                  $"Нов статус на поръчка {model}", GlobalConstants.SMTPServer,
                                  GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);

                return this.Ok(updatedOrder);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", controllerName);
                return InternalServerError();
            }
        }
    }
}
