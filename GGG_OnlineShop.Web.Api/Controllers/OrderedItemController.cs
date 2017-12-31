namespace GGG_OnlineShop.Web.Api.Controllers
{
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

        public OrderedItemController(IOrderedItemsService orders, IUsersService users)
        {
            this.orders = orders;
            this.users = users;
        }

        [HttpPost]
        [Route("order")]
        // TODO send email to user (also when update order)
        // think about how to send an email with order info to the user
        public IHttpActionResult Order(OrderedItemRequestModel model)
        {
            try
            {
                IHttpActionResult result;

                var userId = User.Identity.GetUserId();
                // info
                // if registered user - default: user company deliveryAddress
                // else if UseAlternativeAddress and registered -> use passed fullAddress
                // if nonRegisteredUser -> fullAddress (required)
                if (!string.IsNullOrEmpty(userId))
                {
                    model.UserId = userId;

                    var user = this.users.GetById(userId);
                    if (!model.UseAlternativeAddress) // else - will use the passed address
                    {
                        model.FullAddress = $"{user.DeliveryCountry}; {user.DeliveryTown}; {user.DeliveryAddress}";
                    }
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = this.Mapper.Map<OrderedItem>(model);
                
                if (this.orders.IsValidOrder(order))
                {
                    this.orders.Add(order);
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
