namespace GGG_OnlineShop.Web.Api.Areas.Administration.Controllers
{
    using Common;
    using Api.Controllers;
    using Data.Services.Contracts;
    using System.Web.Http;
    using System;
    using System.Net.Http;
    using System.Linq;
    using Models.OrderedItems;
    using Infrastructure;
    using System.Net;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [RoutePrefix("api/Administration/ManageOrderedItems")]
    public class ManageOrderedItemsController : BaseController
    {
        private readonly IOrderedItemsService orders;

        public ManageOrderedItemsController(IOrderedItemsService orders)
        {
            this.orders = orders;
        }

        [HttpGet]
        [Route("all")]
        public IHttpActionResult Get()
        {
            try
            {
                var orders = this.orders.GetAll()
                                  .OrderBy(x => x.CreatedOn)
                                  .ThenByDescending(x => x.Status)
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

        //[HttpGet]
        //[Route("api/Administration/ManageOrderedItems/pending")]
        //public IHttpActionResult GetPending()
        //{
        //    try
        //    {
        //        var orders = this.orders.GetAllPending()
        //                          .OrderBy(x => x.CreatedOn)
        //                          .ThenBy(x => x.UserId)
        //                          .ThenBy(x => x.Id)
        //                          .To<OrderedItemResponseModelWIthUserInfo>()
        //                          .ToList();

        //        return this.Ok(orders);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
        //                                         e.Message));
        //    }
        //}

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

                var updatedOrder = this.Mapper.Map<OrderedItemResponseModelWIthUserInfo>(this.orders.GetById(model.Id));
                return this.Ok($"{updatedOrder} updated successfully");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
