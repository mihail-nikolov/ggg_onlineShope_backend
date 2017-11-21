namespace GGG_OnlineShop.Web.Api.Areas.Administration.Controllers
{
    using Common;
    using Api.Controllers;
    using Data.Services.Contracts;
    using System.Web.Http;
    using System;
    using System.Net.Http;
    using System.Linq;
    using ViewModels.OrderedItems;
    using Infrastructure;
    using System.Net;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class ManageOrderedItemsController : BaseController
    {
        private readonly IOrderedItemsService orders;

        public ManageOrderedItemsController(IOrderedItemsService orders)
        {
            this.orders = orders;
        }

        [HttpGet]
        [Route("api/Administration/ManageOrderedItems/all")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var orders = this.orders.GetAll()
                                  .OrderBy(x => x.CreatedOn)
                                  .ThenBy(x => x.UserId)
                                  .ThenBy(x => x.Id)
                                  .To<OrderedItemResponseModelWIthUserInfo>()
                                  .ToList();

                return this.Ok(orders);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpGet]
        [Route("api/Administration/ManageOrderedItems/pending")]
        public IHttpActionResult GetPending()
        {
            try
            {
                var orders = this.orders.GetAllPending()
                                  .OrderBy(x => x.CreatedOn)
                                  .ThenBy(x => x.UserId)
                                  .ThenBy(x => x.Id)
                                  .To<OrderedItemResponseModelWIthUserInfo>()
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
        [Route("api/Administration/ManageOrderedItems/update")]
        public IHttpActionResult Update(OrderedItemRequestUpdateStatusModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = this.orders.GetById(model.Id);
                product.Finished = model.Finished;
                this.orders.Save();

                // TODO - check the nulls (closing the connection when getbyId => cannot read VehicleGlass property
                var updatedOrder = this.Mapper.Map<OrderedItemResponseModelWIthUserInfo>(this.orders.GetById(model.Id)); // should return the whole model
                return this.Ok($"{updatedOrder.Id} updated successfully");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
