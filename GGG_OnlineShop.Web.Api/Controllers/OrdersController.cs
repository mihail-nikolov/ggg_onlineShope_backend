using System.Collections.Generic;
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
        public async Task<IHttpActionResult> Order(OrderRequestModel orderRequest)
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
                    user = users.GetById(userId);
                }

                var order = this.Mapper.Map<Order>(orderRequest);
                order.User = user;
                order.Status = DeliveryStatus.Unpaid;

                var installationRuse = orderRequest.InstallationRuse;
                var installationSofia = orderRequest.InstallationSofia;
                if (installationSofia)
                {
                    order.BoughtFrom = GlobalConstants.OfficeSofia;
                }
                else if (installationRuse)
                {
                    order.BoughtFrom = GlobalConstants.OfficeRuse;
                }

                if (this.orders.IsValidOrder(order))
                {
                    this.orders.Add(order);
                }
                else
                {
                    result = BadRequest("Грешка при валидацията на поръчката");
                }

                if (result is OkResult)
                {
                    string body = orderRequest.ToString();
                    string emailTo = order.UserЕmail;
                    await emails.SendEmail(emailTo, string.Format(GlobalConstants.OrderMade, order.Id),
                        body, GlobalConstants.SMTPServer,
                        GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);

                    await emails.SendEmail(GlobalConstants.EmailPrimaryProduction, 
                        string.Format(GlobalConstants.OrderMade, order.Id),
                        body, GlobalConstants.SMTPServer,
                        GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);

                    if (installationSofia)
                    {
                        await emails.SendEmail(GlobalConstants.EmailSofia,
                             string.Format(GlobalConstants.OrderMade, order.Id),
                             body, GlobalConstants.SMTPServer,
                             GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                    }
                    else if (installationRuse)
                    {
                        await emails.SendEmail(GlobalConstants.EmailRuse,
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
        public async Task<IHttpActionResult> UpdateOrder()
        {
            string method = nameof(UpdateOrder);
            var content = await Request.Content.ReadAsStringAsync();
            Logger.LogInfo(content, "incoming message", controllerName, method);
            try
            {
                var updateOrders = DecodeUpdateInput(content);

                StringBuilder responseString = new StringBuilder();
                foreach (var updateOrder in updateOrders)
                {
                    string responseOrder = "INVOICE=" + updateOrder.Invoice + ":STATUS={0}";
                    var order = orders.GetById(updateOrder.Invoice);
                    if (order == null)
                    {
                        responseOrder = string.Format(responseOrder, ShopResponse.NO);
                        responseOrder += $"не е намерен продукт с ID:{updateOrder.Invoice}";
                        Logger.LogInfo(responseOrder, "product not found", controllerName, method);
                    }
                    else
                    {
                        string body = $"Обновена поръчка с No: {updateOrder.Invoice}\n";

                        switch (updateOrder.Status)
                        {
                            case EpayStatus.Paid: order.Status = DeliveryStatus.Paid; body += "статус: платена"; break;
                            case EpayStatus.Denied: order.Status = DeliveryStatus.Denied; body += "статус: отказана"; break;
                            case EpayStatus.Expired: order.Status = DeliveryStatus.Expired; body += "статус: изтекла"; break;
                        }
                        orders.Save();

                        try
                        {
                            var boughtFrom = order.BoughtFrom;
                            if (boughtFrom == GlobalConstants.OfficeSofia)
                            {
                                await emails.SendEmail(GlobalConstants.EmailSofia,
                                    string.Format(GlobalConstants.OrderMade, order.Id),
                                    body, GlobalConstants.SMTPServer,
                                    GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                            }
                            if (boughtFrom == GlobalConstants.OfficeRuse)
                            {
                                await emails.SendEmail(GlobalConstants.EmailRuse,
                                    string.Format(GlobalConstants.OrderMade, order.Id),
                                    body, GlobalConstants.SMTPServer,
                                    GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                            }
                            else
                            {
                                await emails.SendEmail(GlobalConstants.EmailPrimary,
                                    string.Format(GlobalConstants.OrderMade, order.Id),
                                    body, GlobalConstants.SMTPServer,
                                    GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                            }


                            await emails.SendEmail(order.UserЕmail,
                                 string.Format(GlobalConstants.OrderUpdated, order.Id),
                                 body, GlobalConstants.SMTPServer,
                                 GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);
                        }
                        catch (Exception e)
                        {
                            HandlExceptionLogging(e, "error while sending update emails", controllerName);
                        }

                        responseOrder = string.Format(responseOrder, ShopResponse.OK);
                        Logger.LogInfo(responseOrder, "product found", controllerName, method);
                    }

                    responseString.AppendFormat("{0}\n", responseOrder);
                }

                var encodedResponse = GenerateEncodedResponse(responseString.ToString());
                Logger.LogInfo(encodedResponse, "response to epay", controllerName, method);

                return Ok(encodedResponse);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, content, controllerName);
                string response = GenerateEncodedResponse("INVOICE=0:STATUS=ERR=InternalServerError");

                return Ok(response);
            }
        }

        private string GenerateEncodedResponse(string data)
        {
            string base64DecodedData = Base64UrlEncoder.Encode(data);
            base64DecodedData = base64DecodedData.PadRight(base64DecodedData.Length + (4 - base64DecodedData.Length % 4) % 4, '=');
            string checksum = "";

            if (!string.IsNullOrWhiteSpace(base64DecodedData))
            {
                checksum = GenerateCheckSum(base64DecodedData);
            }

            return $"ENCODED={base64DecodedData}&CHECKSUM={checksum}";
        }

        private List<OrderUpdateStatus> DecodeUpdateInput(string input)
        {
            var splitted = input.Split('&');
            int startIndexData = splitted[0].IndexOf("=", StringComparison.Ordinal) + 1;
            string dataPart = splitted[0].Substring(startIndexData);

            int startIndexCheckSum = splitted[1].IndexOf("=", StringComparison.Ordinal) + 1;
            string checkSumPart = splitted[1].Substring(startIndexCheckSum);

            dataPart = dataPart.Replace("%3D", "=");
            string controlCheckSum = GenerateCheckSum(dataPart);
            if (controlCheckSum != checkSumPart)
            {
                return new List<OrderUpdateStatus>();
            }

            List<OrderUpdateStatus> orderUpdates = new List<OrderUpdateStatus>();
            var dataArray = Base64UrlEncoder.Decode(dataPart).Split(Environment.NewLine.ToCharArray());
            foreach (var data in dataArray)
            {
                if (!string.IsNullOrWhiteSpace(data) && data.Contains("=") && data.Contains(":"))
                {
                    Logger.LogInfo(data, "split data Input", controllerName, nameof(DecodeUpdateInput));
                    var dataDictionary = new Dictionary<string, string>();
                    var dataSplit = data.Split(':');
                    foreach (var dataArgument in dataSplit)
                    {
                        var splittedArgument = dataArgument.Split('=');
                        dataDictionary.Add(splittedArgument[0], splittedArgument[1]);
                    }

                    orderUpdates.Add(new OrderUpdateStatus
                    {
                        Invoice = int.Parse(dataDictionary["INVOICE"]),
                        Status = (EpayStatus)Enum.Parse(typeof(EpayStatus), dataDictionary["STATUS"], true)
                    });
                }
            }

            return orderUpdates;
        }

        public static string GenerateCheckSum(string stringToHash)
        {
            UTF8Encoding myEncoder = new UTF8Encoding();
            byte[] key = myEncoder.GetBytes(GlobalConstants.EpayUserKey);
            byte[] text = myEncoder.GetBytes(stringToHash);

            HMACSHA1 myHMACSHA1 = new HMACSHA1(key);

            byte[] HashCode = myHMACSHA1.ComputeHash(text);
            string hash = BitConverter.ToString(HashCode).Replace("-", "");

            return hash.ToLower();
        }
    }
}
