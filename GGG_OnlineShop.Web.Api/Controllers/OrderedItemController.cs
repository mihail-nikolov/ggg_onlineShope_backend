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
        public IHttpActionResult Order(OrderedItemRequestModel model)
        {
            try
            {
                IHttpActionResult result;

                var userId = User.Identity.GetUserId();

                // info
                // if registered user - default: user company deliveryAddress
                // else if UseAlternativeAddress and registered -> use passed fullAddress
                // if nonRegisteredUser -> fullAddres (required)
                if (!string.IsNullOrEmpty(userId))
                {
                    model.UserId = userId;

                    var user = this.users.GetById(userId);
                    if (!model.UseAlternativeAddress)
                    {
                        model.FullAddress = $"{user.DeliveryCountry}; {user.DeliveryTown}; {user.DeliveryAddress}";
                    }
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = this.Mapper.Map<OrderedItem>(model);

                if (this.orders.ValidateOrder(order))
                {
                    this.orders.Add(order);
                    result = this.Ok();
                }
                else
                {
                    result = this.BadRequest("Error while valditing userInfo and/or paid price");
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
