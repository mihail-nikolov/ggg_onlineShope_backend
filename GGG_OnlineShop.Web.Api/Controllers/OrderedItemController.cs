namespace GGG_OnlineShop.Web.Api.Controllers
{
    using Data.Services.Contracts;
    using Infrastructure;
    using InternalApiDB.Models;
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    [Authorize]
    public class OrderedItemController : BaseController
    {
        private readonly IOrderedItemsService orders;
        private readonly IVehicleGlassesService glasses;

        public OrderedItemController(IOrderedItemsService orders,
                                     IVehicleGlassesService glasses)
        {
            this.orders = orders;
            this.glasses = glasses;
        }

        [HttpGet]
        [Route("api/OrderedItems/ShowMyOrders")]
        public IHttpActionResult ShowMyOrders()
        {
            try
            {
                var orders = this.orders.GetAllByUser(User.Identity.GetUserId())
                                        .OrderBy(x => x.Finished)
                                        .ThenBy(x => x.CreatedOn)
                                        .ThenBy(x => x.Id)
                                        .To<OrderedItemResponseModel>()
                                        .ToList();
                return this.Ok(orders);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpPost]
        [Route("api/OrderedItems/order/{productId}")]
        public IHttpActionResult OrderItem(int productId)
        {
            try
            {
                var order = new OrderedItem() { VehicleGlassId = productId, UserId = User.Identity.GetUserId() };
                this.orders.Add(order);
                var product = this.Mapper.Map<OrderedItemResponseModel>(this.glasses.GetById(productId));
                return this.Ok(product);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
