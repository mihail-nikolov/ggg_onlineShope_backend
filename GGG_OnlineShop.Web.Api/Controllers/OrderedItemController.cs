namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Common;
    using Data.Services.Contracts;
    using InternalApiDB.Models;
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [RoutePrefix("api/OrderedItems")]
    public class OrderedItemController : BaseController
    {
        private readonly IOrderedItemsService orders;
        private readonly IUsersService users;
        private readonly IEmailsService emails;

        public OrderedItemController(IOrderedItemsService orders, IUsersService users, IEmailsService emails)
        {
            this.orders = orders;
            this.users = users;
            this.emails = emails;
        }

        [HttpPost]
        [Route("order")]
        // think about how to send an email with order info to the user
        public IHttpActionResult Order(OrderedItemRequestModel model)
        {
            try
            {
                IHttpActionResult result;

                var userId = User.Identity.GetUserId();
                User user = null;
                OrderedItem order = new OrderedItem();

                // info
                // if registered user - default: user company deliveryAddress
                // else if UseAlternativeAddress and registered -> use passed fullAddress
                // if nonRegisteredUser -> fullAddress (required)
                if (!string.IsNullOrEmpty(userId))
                {
                    user = this.users.GetById(userId);

                    if (!model.UseAlternativeAddress) // else - will use the passed address
                    {
                        model.FullAddress = $"{user.DeliveryCountry}; {user.DeliveryTown}; {user.DeliveryAddress}";
                    }
                }

                model.Status = DeliveryStatus.New;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                order = this.Mapper.Map<OrderedItem>(model);
                order.User = user;
                order.UserId = userId;

                if (this.orders.IsValidOrder(order))
                {
                    this.orders.Add(order);
                    // TODO  Use everywhere the ID of just added entity!

                    string emailTo = !string.IsNullOrEmpty(order.AnonymousUserЕmail) ? order.AnonymousUserЕmail : order.User.Email;
                    emails.SendEmail(emailTo, string.Format(GlobalConstants.OrderMade, order.Id),
                                     $"направена поръчка: {model.ToString()}", GlobalConstants.SMTPServer,
                                     GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword);
                    result = this.Ok();
                }
                else
                {
                    result = this.BadRequest("Error while valditing order");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
