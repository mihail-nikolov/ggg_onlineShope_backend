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

    [RoutePrefix("api/OrderedItems")]
    public class OrderedItemController : BaseController
    {
        private readonly IOrdersService orders;
        private readonly IUsersService users;
        private readonly IEmailsService emails;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public OrderedItemController(IOrdersService orders, IUsersService users, IEmailsService emails, ILogsService dbLogger)
            : base(dbLogger)
        {
            this.orders = orders;
            this.users = users;
            this.emails = emails;
        }

        [HttpPost]
        [Route("order")]
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
                    string body = $"направена поръчка: \n\n, {orderRequest})";
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
