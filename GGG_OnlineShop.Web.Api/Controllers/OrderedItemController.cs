using System.Collections.Generic;
using System.Text;
using System.Web.Http.Results;

namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Common;
    using Data.Services.Contracts;
    using InternalApiDB.Models;
    using InternalApiDB.Models.Enums;
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Reflection;
    using System.Web.Http;

    [RoutePrefix("api/OrderedItems")]
    public class OrderedItemController : BaseController
    {
        private readonly IOrderedItemsService orders;
        private readonly IUsersService users;
        private readonly IEmailsService emails;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public OrderedItemController(IOrderedItemsService orders, IUsersService users, IEmailsService emails, ILogsService dbLogger)
            : base(dbLogger)
        {
            this.orders = orders;
            this.users = users;
            this.emails = emails;
        }

        [HttpPost]
        [Route("order")]
        public IHttpActionResult Order(List<OrderedItemRequestModel> items)
        {
            try
            {
                IHttpActionResult result = Ok();

                var userId = User.Identity.GetUserId();
                User user = null;

                if (!string.IsNullOrEmpty(userId))
                {
                    user = this.users.GetById(userId);
                }

                string anonymousUserEmail = items[0].AnonymousUserЕmail;
                StringBuilder orderItemIds = new StringBuilder();
                // info
                // if registered user - default: in fullAddress will write down the user company deliveryAddress
                // else if UseAlternativeAddress and registered -> use passed fullAddress
                // if nonRegisteredUser -> fullAddress (required)
                foreach (var item in items)
                {
                    if (user != null)
                    {
                        if (!item.UseAlternativeAddress) // else - will use the passed address
                        {
                            item.FullAddress = $"{user.DeliveryCountry}; {user.DeliveryTown}; {user.DeliveryAddress}";
                        }
                    }

                    OrderedItem order = new OrderedItem();

                    item.Status = DeliveryStatus.New;
                    if (!ModelState.IsValid)
                    {
                        result = BadRequest(ModelState);
                        break;
                    }

                    order = this.Mapper.Map<OrderedItem>(item);
                    order.User = user;
                    order.UserId = userId;

                    if (this.orders.IsValidOrder(order))
                    {
                        this.orders.Add(order);
                        // TODO Probably best approach is to have Orders table -> OrderedItems

                        orderItemIds.Append($"{order.Id}, ");
                    }
                    else
                    {
                        result = this.BadRequest("Error while valditing order");
                        break;
                    }
                }

                if (result is OkResult)
                {
                    string body = $"направена поръчка: {string.Join("\n\n", items)}";
                    string emailTo = !string.IsNullOrEmpty(anonymousUserEmail) ? anonymousUserEmail : user?.Email;
                    emails.SendEmail(emailTo, string.Format(GlobalConstants.OrderMade, orderItemIds.ToString().TrimEnd(',', ' ')),
                        body, GlobalConstants.SMTPServer,
                        GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword);
                }

                return result;
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", controllerName);
                return InternalServerError();
            }
        }
    }
}
