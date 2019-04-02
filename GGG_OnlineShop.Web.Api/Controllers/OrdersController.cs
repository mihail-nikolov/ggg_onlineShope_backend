using Newtonsoft.Json;

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
    using System.Web.Http.Results;

    [RoutePrefix("api/Orders")]
    public class OrdersController : BaseController
    {
        private readonly IOrdersService orders;
        private readonly IUsersService users;
        private readonly IEmailsService emails;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public OrdersController(IOrdersService orders, IUsersService users, IEmailsService emails, ILogsService dbLogger)
            : base(dbLogger)
        {
            this.orders = orders;
            this.users = users;
            this.emails = emails;
        }

        [HttpPost]
        public IHttpActionResult Order(OrderRequestModel orderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                IHttpActionResult result = Ok();

                var userId = User.Identity.GetUserId();
                User user = null;

                if (!string.IsNullOrEmpty(userId))
                {
                    user = this.users.GetById(userId);
                }

                var order = this.Mapper.Map<Order>(orderRequest);
                order.User = user;
                order.Status = DeliveryStatus.Unpaid;

                if (this.orders.IsValidOrder(order))
                {
                    this.orders.Add(order);
                }
                else
                {
                    result = this.BadRequest("Грешка при валидацията на поръчката");
                }

                if (result is OkResult)
                {
                    string body = $"Направена поръчка: \n\n{orderRequest}";
                    string emailTo = order.UserЕmail;
                    emails.SendEmail(emailTo, string.Format(GlobalConstants.OrderMade, order.Id),
                        body, GlobalConstants.SMTPServer,
                        GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);

                    emails.SendEmail(GlobalConstants.EmailPrimaryProduction, string.Format(GlobalConstants.OrderMade, order.Id),
                        body, GlobalConstants.SMTPServer,
                        GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);

                    var installationRuse = orderRequest.InstallationRuse;
                    var installationSofia = orderRequest.InstallationSofia;

                    if (installationSofia)
                    {
                        emails.SendEmail(GlobalConstants.EmailSofia,
                            string.Format(GlobalConstants.OrderMade, order.Id),
                            body, GlobalConstants.SMTPServer,
                            GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                    }
                    else if (installationRuse)
                    {
                        emails.SendEmail(GlobalConstants.EmailRuse,
                            string.Format(GlobalConstants.OrderMade, order.Id),
                            body, GlobalConstants.SMTPServer,
                            GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                    }
                }

                var orderResponse = Mapper.Map<OrderResponseModel>(order);
                result = Ok(orderResponse);

                return result;
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", controllerName);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult UpdateOrder(OrderUpdateStatus updateOrder)
        {
            EpayResponse response = new EpayResponse {Status = ShopResponse.Ok};
            Logger.LogInfo(updateOrder.ToJson(), "incoming message", controllerName, "Order");

            if (!ModelState.IsValid)
            {
                response.Status = ShopResponse.Error;
                response.Error = JsonConvert.SerializeObject(ModelState, Formatting.Indented);
                response.ErrorMessage = "Грешен модел";

                return Ok(response);
            }

            response.Invoice = updateOrder.Invoice;
            try
            {
                IHttpActionResult result = Ok(response);
                // for now for invoice we use the ID
                var order = orders.GetById(updateOrder.Invoice);
                if (order == null)
                {
                    response.Status = ShopResponse.NotFound;
                    result = Ok(response);
                }
                else
                {
                    switch (updateOrder.Status)
                    {
                        case EpayStatus.Paid: order.Status = DeliveryStatus.Paid; break;
                        case EpayStatus.Denied: order.Status = DeliveryStatus.Denied; break;
                    }

                    string body = $"Обновена поръчка с No: {updateOrder.Invoice}\n";
                    switch (order.Status)
                    {
                        case DeliveryStatus.Paid: body += "статус: платена"; break;
                        case DeliveryStatus.Denied: body += "статус: отказана"; break;
                    }

                    try
                    {
                        if (updateOrder.Status != EpayStatus.Expired)
                        {
                            emails.SendEmail(GlobalConstants.EmailPrimary,
                                string.Format(GlobalConstants.OrderMade, order.Id),
                                body, GlobalConstants.SMTPServer,
                                GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);

                            emails.SendEmail(order.UserЕmail,
                                string.Format(GlobalConstants.OrderUpdated, order.Id),
                                body, GlobalConstants.SMTPServer,
                                GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                        }
                    }
                    catch (Exception e)
                    {
                        HandlExceptionLogging(e, "error while sending update emails", controllerName);
                    }
                    // TODO ???
                    // INVOICE=123456:STATUS=OK
                    // INVOICE=123457:STATUS=ERR
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
