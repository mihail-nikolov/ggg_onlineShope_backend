using System.Threading.Tasks;

namespace GGG_OnlineShop.Web.Api.Tests.Administration
{
    using Areas.Administration.Controllers;
    using Areas.Administration.Models.OrderedItems;
    using Common;
    using Data.Services.Contracts;
    using InternalApiDB.Models;
    using InternalApiDB.Models.Enums;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Results;

    [TestClass]
    public class ManageOrderedItemsControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();
        private readonly Mock<ILogsService> mockedLogger = new Mock<ILogsService>();
        private readonly string controllerName = "ManageOrderedItemsController";

        public ManageOrderedItemsControllerTests()
        {
            mockedLogger.Setup(x => x.LogError(It.IsAny<Exception>(), "", controllerName, It.IsAny<string>()));
        }

        [TestMethod]
        public void Get_ShouldReturnInternalServerErrorAndLogError_WhenOrdersServiceIsNull()
        {
            var controller = new ManageOrdersController(null, null, mockedLogger.Object);

            var result = controller.Get();

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "Get"));
        }

        [TestMethod]
        public void Update_ShouldReturnInternalServerErrorAndLogError_WhenOrdersServiceIsNull()
        {
            var usersMock = new Mock<IUsersService>();
            OrderRequestUpdateStatusModel request = new OrderRequestUpdateStatusModel();
            var controller = new ManageOrdersController(null, null, mockedLogger.Object);

            var result = controller.Update(request);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "Update"));
        }

        [TestMethod]
        public void Get_ShouldReturnPendingOrdersAndOrderedCorrectly_WhenPendingSend()
        {
            mapper.Execute();

            var orders = new List<Order>()
            {
                new Order()
                {
                    Status = DeliveryStatus.Unpaid,
                    Id = 2,
                    CreatedOn = DateTime.MinValue
                },
                new Order()
                {
                    Status = DeliveryStatus.Unpaid,
                    Id = 3,
                    CreatedOn = DateTime.MinValue
                },
                new Order()
                {
                    Status = DeliveryStatus.Unpaid,
                    Id = 1,
                    CreatedOn = DateTime.Now
                }
            }.AsQueryable();

            var ordersMock = new Mock<IOrdersService>();
            ordersMock.Setup(v => v.GetNewOrders()).Returns(orders);

            var controller = new ManageOrdersController(ordersMock.Object, null, null);

            var result = controller.Get(DeliveryStatus.Unpaid.ToString());

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnOrderedProducts_WhenOrderedSend()
        {
            mapper.Execute();

            var orders = new List<Order>()
            {
                new Order()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 2
                },
                new Order()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 1
                },
                new Order()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 3
                }
            }.AsQueryable();

            var ordersMock = new Mock<IOrdersService>();
            ordersMock.Setup(v => v.GetOrderedProducts()).Returns(orders);

            var controller = new ManageOrdersController(ordersMock.Object, null, null);

            var result = controller.Get(DeliveryStatus.Ordered.ToString());

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnDoneOrders_WhenDoneSend()
        {
            mapper.Execute();

            var orders = new List<Order>()
            {
                new Order()
                {
                    Status = DeliveryStatus.Done,
                    Id = 2
                },
                new Order()
                {
                    Status = DeliveryStatus.Done,
                    Id = 1
                },
                new Order()
                {
                    Status = DeliveryStatus.Done,
                    Id = 3
                }
            }.AsQueryable();

            var ordersMock = new Mock<IOrdersService>();
            ordersMock.Setup(v => v.GetDoneOrders()).Returns(orders);

            var controller = new ManageOrdersController(ordersMock.Object, null, null);

            var result = controller.Get(DeliveryStatus.Done.ToString());

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnAllOrdersAndOrderedCorrectly_WhenNoParameterSend()
        {
            mapper.Execute();

            var orders = new List<Order>()
            {
                new Order()
                {
                    Status = DeliveryStatus.Unpaid,
                    Id = 2,
                    CreatedOn = DateTime.MinValue
                },
                new Order()
                {
                    Status = DeliveryStatus.Unpaid,
                    Id = 1,
                    CreatedOn = DateTime.MinValue
                },
                new Order()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 4,
                    CreatedOn = DateTime.MinValue
                },
                new Order()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 3,
                    CreatedOn = DateTime.Now
                }
                ,
                new Order()
                {
                    Status = DeliveryStatus.Done,
                    Id = 5,
                    CreatedOn = DateTime.MinValue
                }
            }.AsQueryable();

            var ordersMock = new Mock<IOrdersService>();
            ordersMock.Setup(v => v.GetAll()).Returns(orders);

            var controller = new ManageOrdersController(ordersMock.Object, null, null);

            var result = controller.Get();

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);
            Assert.AreEqual(responseContent[3].Id, 4);
            Assert.AreEqual(responseContent[4].Id, 5);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldMapUserInfoCorrectly()
        {
            mapper.Execute();

            var orders = new List<Order>()
            {
                new Order()
                {
                    Status = DeliveryStatus.Unpaid,
                    Id = 1,
                    UserId = "1",
                    User = new User() {IsCompany = true, Bulstat = "Bulstat", Name = "CompanyName", Email = "testEmail", PhoneNumber = "0088"}
                },
                new Order()
                {
                    Status = DeliveryStatus.Unpaid,
                    Id = 2,
                    UserId = "2",
                    User = new User() {IsCompany = false, Bulstat = "Bulstat", Name = "UserName", Email = "testEmail1", PhoneNumber = "0099"}
                },
                new Order()
                {
                    Status = DeliveryStatus.Unpaid,
                    Id = 3,
                    UserInfo = "AnonymousUserInfo",
                    UserЕmail = "AnonymousUserЕmail"
                },
            }.AsQueryable();

            var ordersMock = new Mock<IOrdersService>();
            ordersMock.Setup(v => v.GetNewOrders()).Returns(orders);

            var controller = new ManageOrdersController(ordersMock.Object, null, null);

            var result = controller.Get();

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual("Bulstat; CompanyName; testEmail; 0088", responseContent[0].UserInfo);
            Assert.IsNull(responseContent[0].UserInfo);
            Assert.IsNull(responseContent[0].UserЕmail);

            Assert.AreEqual(responseContent[1].UserInfo, "UserName; testEmail1; 0099");
            Assert.IsNull(responseContent[1].UserInfo);
            Assert.IsNull(responseContent[1].UserЕmail);

            Assert.AreEqual(responseContent[2].UserInfo, string.Empty);
            Assert.AreEqual(responseContent[2].UserInfo, "AnonymousUserInfo");
            Assert.AreEqual(responseContent[2].UserЕmail, "AnonymousUserЕmail");

            ordersMock.VerifyAll();
        }

        [TestMethod]
        public async Task Update_ShouldUpdateOrderCorrectly()
        {
            mapper.Execute();

            int testId = 1;
            var order = new Order()
            {
                Status = DeliveryStatus.Unpaid,
                Id = testId,
                UserЕmail = "testEmail"
            };

            var ordersMock = new Mock<IOrdersService>();
            ordersMock.Setup(v => v.GetById(testId)).Returns(order);
            ordersMock.Setup(v => v.Save());

            var emailsMock = new Mock<IEmailsService>();

            OrderRequestUpdateStatusModel request = new OrderRequestUpdateStatusModel() { Id = 1, Status = DeliveryStatus.Done };

            var controller = new ManageOrdersController(ordersMock.Object, emailsMock.Object, null);

            var result = await controller.Update(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<OrderResponseModelWIthUserInfo>));
            var responseContent = ((OkNegotiatedContentResult<OrderResponseModelWIthUserInfo>)result).Content;

            Assert.AreEqual(responseContent.Id, testId);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public async Task Update_ShouldSendEmaiToTheAnonymousUserWithTheCorrectContent()
        {
            mapper.Execute();

            int testId = 1;
            DeliveryStatus status = DeliveryStatus.Done;
            string statusBG = EnglishBulgarianDictionary.Namings[status.ToString()];

            var order = new Order()
            {
                Status = DeliveryStatus.Unpaid,
                Id = testId,
                UserЕmail = "testEmail"
            };

            var ordersMock = new Mock<IOrdersService>();
            ordersMock.Setup(v => v.GetById(testId)).Returns(order);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(
                                              order.UserЕmail, string.Format(GlobalConstants.OrderUpdated, order.Id),
                                              It.Is<string>(y => y.Contains("Нов статус на поръчка") && y.Contains(testId.ToString()) && y.Contains(statusBG)),
                                              GlobalConstants.SMTPServer,
                                              GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword));

            OrderRequestUpdateStatusModel request = new OrderRequestUpdateStatusModel() { Id = testId, Status = status };

            var controller = new ManageOrdersController(ordersMock.Object, emailsMock.Object, null);

            var result = await controller.Update(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<OrderResponseModelWIthUserInfo>));
            var responseContent = ((OkNegotiatedContentResult<OrderResponseModelWIthUserInfo>)result).Content;

            Assert.AreEqual(responseContent.Id, testId);
            emailsMock.VerifyAll();
        }

        [TestMethod]
        public async Task Update_ShouldSendEmailToTheRegisteredUser()
        {
            mapper.Execute();

            int testId = 1;
            var testUser = new User { Email = "testEmail" };
            var order = new Order()
            {
                Status = DeliveryStatus.Unpaid,
                Id = testId,
                User = testUser
            };

            var ordersMock = new Mock<IOrdersService>();
            ordersMock.Setup(v => v.GetById(testId)).Returns(order);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(
                                              testUser.Email, string.Format(GlobalConstants.OrderUpdated, order.Id),
                                              It.Is<string>(y => y.Contains("Нов статус на поръчка") && y.Contains(testId.ToString()) && y.Contains("Завършена")),
                                              GlobalConstants.SMTPServer,
                                              GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword));

            OrderRequestUpdateStatusModel request = new OrderRequestUpdateStatusModel() { Id = testId, Status = DeliveryStatus.Done };

            var controller = new ManageOrdersController(ordersMock.Object, emailsMock.Object, null);

            var result = await controller.Update(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<OrderResponseModelWIthUserInfo>));
            var responseContent = ((OkNegotiatedContentResult<OrderResponseModelWIthUserInfo>)result).Content;

            Assert.AreEqual(responseContent.Id, testId);
            emailsMock.VerifyAll();
        }
    }
}