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
    using System.Collections.Generic;

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
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
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
                
                var updatedOrder = this.Mapper.Map<OrderedItemResponseModelWIthUserInfo>(this.orders.GetById(model.Id));

                return this.Ok(updatedOrder);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
