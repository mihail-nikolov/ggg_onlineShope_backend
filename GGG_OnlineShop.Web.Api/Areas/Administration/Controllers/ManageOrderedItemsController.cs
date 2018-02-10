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
    [RoutePrefix("api/Administration/ManageOrderedItems")]
    public class ManageOrderedItemsController : BaseController
    {
        private readonly IOrderedItemsService orders;
        private readonly IEmailsService emails;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public ManageOrderedItemsController(IOrderedItemsService orders, IEmailsService emails, ILogsService dbLogger)
            :base(dbLogger)
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
                List<OrderedItemResponseModelWIthUserInfo> orders;

                if (pending)
                {
                    orders = this.orders.GetNewOrders()
                                        .To<OrderedItemResponseModelWIthUserInfo>()
                                        .ToList();
                }
                else if (ordered)
                {
                    orders = this.orders.GetOrderedProducts()
                                        .To<OrderedItemResponseModelWIthUserInfo>()
                                        .ToList();
                }
                else if (done)
                {
                    orders = this.orders.GetDoneOrders()
                                        .To<OrderedItemResponseModelWIthUserInfo>()
                                        .ToList();
                }
                else
                {
                    orders = this.orders.GetAll()
                                        .To<OrderedItemResponseModelWIthUserInfo>()
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
        public IHttpActionResult Update(OrderedItemRequestUpdateStatusModel model)
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

                var updatedOrder = this.Mapper.Map<OrderedItemResponseModelWIthUserInfo>(product);
                string emailTo = !string.IsNullOrEmpty(product.AnonymousUserЕmail) ? product.AnonymousUserЕmail : product.User.Email;
                emails.SendEmail(emailTo, string.Format(GlobalConstants.OrderUpdated, product.Id),
                                 $"Нов статус на поръчка {model.ToString()}", GlobalConstants.SMTPServer,
                                 GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword);

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
