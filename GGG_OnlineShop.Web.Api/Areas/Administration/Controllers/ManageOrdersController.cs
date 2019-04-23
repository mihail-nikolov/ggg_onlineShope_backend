using System.Threading.Tasks;

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
        public IHttpActionResult Get(bool pending = false, bool ordered = false, bool done = false)
        {
            try
            {
                List<OrderResponseModelWIthUserInfo> orders;

                if (pending)
                {
                    orders = this.orders.GetNewOrders()
                                        .To<OrderResponseModelWIthUserInfo>()
                                        .ToList();
                }
                else if (ordered)
                {
                    orders = this.orders.GetOrderedProducts()
                                        .To<OrderResponseModelWIthUserInfo>()
                                        .ToList();
                }
                else if (done)
                {
                    orders = this.orders.GetDoneOrders()
                                        .To<OrderResponseModelWIthUserInfo>()
                                        .ToList();
                }
                else
                {
                    orders = this.orders.GetAll()
                                        .To<OrderResponseModelWIthUserInfo>()
                                        .ToList();
                }

                orders = orders.OrderBy(x => x.Status)
                               .ThenByDescending(x => x.CreatedOn)
                               .ThenBy(x => x.Id)
                               .ToList();

                return this.Ok(orders);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", controllerName);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("update")]
        public  async Task<IHttpActionResult> Update(OrderRequestUpdateStatusModel model)
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
