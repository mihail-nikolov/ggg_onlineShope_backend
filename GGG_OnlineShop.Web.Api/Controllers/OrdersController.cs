﻿using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

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

        // TODO add route for update order manually
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> UpdateOrder()
        {
            string method = nameof(UpdateOrder);
            var content = await Request.Content.ReadAsStringAsync();
            Logger.LogInfo(content, "incoming message", controllerName, method);
            try
            {
                var updateOrder = DecodeUpdateInput(content);

                string response = "INVOICE=" + updateOrder.Invoice + ":STATUS={0}";
                IHttpActionResult result;
                // for now for invoice we use the ID
                var order = orders.GetById(updateOrder.Invoice);
                if (order == null)
                {
                    response = string.Format(response, ShopResponse.ERR);
                    response += $"=не е намерен продукт с ID:{updateOrder.Invoice}";
                    Logger.LogInfo(response, "не е намерен продукт", controllerName, method);

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

                    response = string.Format(response, ShopResponse.OK);
                    result = Ok(response);
                }

                return result;
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, content, controllerName);
                return Ok("INVOICE=0:STATUS=ERR=InternalServerError");
            }
        }

        private OrderUpdateStatus DecodeUpdateInput(string input)
        {
            // TODO probably can send more than 1 invoice
            var splitted = input.Split('&');
            int startIndexData = splitted[0].IndexOf("=", StringComparison.Ordinal) + 1;
            string dataPart = splitted[0].Substring(startIndexData);

            // TODO enable later
            //int startIndexCheckSum = splitted[1].IndexOf("=", StringComparison.Ordinal) + 1;
            //string checkSumPart = splitted[1].Substring(startIndexCheckSum);

            //string controlCheckSum = HashString(dataPart);
            //if (controlCheckSum != checkSumPart)
            //{
            //    return null;
            //}

            dataPart = dataPart.Replace("%3D", "="); ;

            string data = Base64UrlEncoder.Decode(dataPart).Replace("\n", "");
            Logger.LogInfo(data, "split data Input", controllerName, nameof(DecodeUpdateInput));
            var dataDictionary = new Dictionary<string, string>();
            var dataSplit = data.Split(':');
            foreach (var dataArgument in dataSplit)
            {
                var splittedArgument = dataArgument.Split('=');
                dataDictionary.Add(splittedArgument[0], splittedArgument[1]);
            }

            var model = new OrderUpdateStatus
            {
                Invoice = int.Parse(dataDictionary["INVOICE"]),
                Status = (EpayStatus)Enum.Parse(typeof(EpayStatus), dataDictionary["STATUS"], true)
            };

            return model;
        }

        public static string HashString(string stringToHash)
        {
            UTF8Encoding myEncoder = new UTF8Encoding();
            byte[] Key = myEncoder.GetBytes(GlobalConstants.EpayUserKey);
            byte[] Text = myEncoder.GetBytes(stringToHash);

            HMACSHA1 myHMACSHA1 = new HMACSHA1(Key);

            byte[] HashCode = myHMACSHA1.ComputeHash(Text);
            string hash = BitConverter.ToString(HashCode).Replace("-", "");

            return hash.ToLower();
        }
    }
}
