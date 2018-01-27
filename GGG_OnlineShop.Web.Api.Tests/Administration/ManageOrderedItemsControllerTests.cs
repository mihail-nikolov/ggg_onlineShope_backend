namespace GGG_OnlineShop.Web.Api.Tests.Administration
{
    using Areas.Administration.Controllers;
    using Areas.Administration.Models.OrderedItems;
    using Common;
    using Data.Services.Contracts;
    using InternalApiDB.Models;
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_ShouldThrowException_WhenOrdersServiceIsNull()
        {
            var controller = new ManageOrderedItemsController(null, null);

            controller.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_ShouldThrowException_WhenOrdersServiceIsNull()
        {
            var usersMock = new Mock<IUsersService>();

            OrderedItemRequestUpdateStatusModel request = new OrderedItemRequestUpdateStatusModel();
            var controller = new ManageOrderedItemsController(null, null);

            controller.Update(request);
        }

        [TestMethod]
        public void Get_ShouldReturnPendingOrdersAndOrderedCorrectly_WhenPendingSend()
        {
            mapper.Execute();

            var orders = new List<OrderedItem>()
            {
                new OrderedItem()
                {
                    Status = DeliveryStatus.New,
                    Id = 2,
                    CreatedOn = DateTime.MinValue
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.New,
                    Id = 3,
                    CreatedOn = DateTime.MinValue
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.New,
                    Id = 1,
                    CreatedOn = DateTime.Now
                }
            }.AsQueryable();

            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(v => v.GetNewOrders()).Returns(orders);

            var controller = new ManageOrderedItemsController(ordersMock.Object, null);

            var result = controller.Get(pending: true);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnOrderedProducts_WhenOrderedSend()
        {
            mapper.Execute();

            var orders = new List<OrderedItem>()
            {
                new OrderedItem()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 2
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 1
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 3
                }
            }.AsQueryable();

            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(v => v.GetOrderedProducts()).Returns(orders);

            var controller = new ManageOrderedItemsController(ordersMock.Object, null);

            var result = controller.Get(ordered: true);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnDoneOrders_WhenDoneSend()
        {
            mapper.Execute();

            var orders = new List<OrderedItem>()
            {
                new OrderedItem()
                {
                    Status = DeliveryStatus.Done,
                    Id = 2
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.Done,
                    Id = 1
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.Done,
                    Id = 3
                }
            }.AsQueryable();

            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(v => v.GetDoneOrders()).Returns(orders);

            var controller = new ManageOrderedItemsController(ordersMock.Object, null);

            var result = controller.Get(done: true);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnAllOrdersAndOrderedCorrectly_WhenNoParameterSend()
        {
            mapper.Execute();

            var orders = new List<OrderedItem>()
            {
                new OrderedItem()
                {
                    Status = DeliveryStatus.New,
                    Id = 2,
                    CreatedOn = DateTime.MinValue
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.New,
                    Id = 1,
                    CreatedOn = DateTime.MinValue
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 4,
                    CreatedOn = DateTime.MinValue
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.Ordered,
                    Id = 3,
                    CreatedOn = DateTime.Now
                }
                ,
                new OrderedItem()
                {
                    Status = DeliveryStatus.Done,
                    Id = 5,
                    CreatedOn = DateTime.MinValue
                }
            }.AsQueryable();

            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(v => v.GetAll()).Returns(orders);

            var controller = new ManageOrderedItemsController(ordersMock.Object, null);

            var result = controller.Get();

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>)result).Content;

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

            var orders = new List<OrderedItem>()
            {
                new OrderedItem()
                {
                    Status = DeliveryStatus.New,
                    Id = 1,
                    UserId = "1",
                    User = new User() {IsCompany = true, Bulstat = "Bulstat", Name = "CompanyName", Email = "testEmail", PhoneNumber = "0088"}
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.New,
                    Id = 2,
                    UserId = "2",
                    User = new User() {IsCompany = false, Bulstat = "Bulstat", Name = "UserName", Email = "testEmail1", PhoneNumber = "0099"}
                },
                new OrderedItem()
                {
                    Status = DeliveryStatus.New,
                    Id = 3,
                    AnonymousUserInfo = "AnonymousUserInfo",
                    AnonymousUserЕmail = "AnonymousUserЕmail"
                },
            }.AsQueryable();

            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(v => v.GetNewOrders()).Returns(orders);

            var controller = new ManageOrderedItemsController(ordersMock.Object, null);

            var result = controller.Get(pending: true);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderedItemResponseModelWIthUserInfo>>)result).Content;

            Assert.AreEqual(responseContent[0].UserInfo, "Bulstat; CompanyName; testEmail; 0088");
            Assert.AreEqual(responseContent[0].AnonymousUserInfo, null);
            Assert.AreEqual(responseContent[0].AnonymousUserЕmail, null);

            Assert.AreEqual(responseContent[1].UserInfo, "UserName; testEmail1; 0099");
            Assert.AreEqual(responseContent[1].AnonymousUserInfo, null);
            Assert.AreEqual(responseContent[1].AnonymousUserЕmail, null);

            Assert.AreEqual(responseContent[2].UserInfo, string.Empty);
            Assert.AreEqual(responseContent[2].AnonymousUserInfo, "AnonymousUserInfo");
            Assert.AreEqual(responseContent[2].AnonymousUserЕmail, "AnonymousUserЕmail");

            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Update_ShouldUpdateOrderCorrectly()
        {
            mapper.Execute();

            int testId = 1;
            var order = new OrderedItem()
            {
                Status = DeliveryStatus.New,
                Id = testId,
                AnonymousUserЕmail = "testEmail"
            };

            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(v => v.GetById(testId)).Returns(order);
            ordersMock.Setup(v => v.Save());

            var emailsMock = new Mock<IEmailsService>();

            OrderedItemRequestUpdateStatusModel request = new OrderedItemRequestUpdateStatusModel() { Id = 1, Status = DeliveryStatus.Done };

            var controller = new ManageOrderedItemsController(ordersMock.Object, emailsMock.Object);

            var result = controller.Update(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<OrderedItemResponseModelWIthUserInfo>));
            var responseContent = ((OkNegotiatedContentResult<OrderedItemResponseModelWIthUserInfo>)result).Content;

            Assert.AreEqual(responseContent.Id, testId);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public void Update_ShouldSendEmaiToTheAnonymousUser()
        {
            mapper.Execute();

            int testId = 1;
            var order = new OrderedItem()
            {
                Status = DeliveryStatus.New,
                Id = testId,
                AnonymousUserЕmail = "testEmail"
            };

            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(v => v.GetById(testId)).Returns(order);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(order.AnonymousUserЕmail, GlobalConstants.OrderUpdated,
                                 It.IsAny<string>(), GlobalConstants.SMTPServer, // TODO adapt content
                                 GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword));

            OrderedItemRequestUpdateStatusModel request = new OrderedItemRequestUpdateStatusModel() { Id = 1, Status = DeliveryStatus.Done };

            var controller = new ManageOrderedItemsController(ordersMock.Object, emailsMock.Object);

            var result = controller.Update(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<OrderedItemResponseModelWIthUserInfo>));
            var responseContent = ((OkNegotiatedContentResult<OrderedItemResponseModelWIthUserInfo>)result).Content;

            Assert.AreEqual(responseContent.Id, testId);
            emailsMock.VerifyAll();
        }

        [TestMethod]
        public void Update_ShouldSendEmaiToTheRegisteredUser()
        {
            mapper.Execute();

            int testId = 1;
            var testUser = new User { Email = "testEmail" };
            var order = new OrderedItem()
            {
                Status = DeliveryStatus.New,
                Id = testId,
                User = testUser
            };

            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(v => v.GetById(testId)).Returns(order);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(testUser.Email, GlobalConstants.OrderUpdated,
                                 It.IsAny<string>(), GlobalConstants.SMTPServer, // TODO adapt content
                                 GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword));

            OrderedItemRequestUpdateStatusModel request = new OrderedItemRequestUpdateStatusModel() { Id = 1, Status = DeliveryStatus.Done };

            var controller = new ManageOrderedItemsController(ordersMock.Object, emailsMock.Object);

            var result = controller.Update(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<OrderedItemResponseModelWIthUserInfo>));
            var responseContent = ((OkNegotiatedContentResult<OrderedItemResponseModelWIthUserInfo>)result).Content;

            Assert.AreEqual(responseContent.Id, testId);
            emailsMock.VerifyAll();
        }
    }
}
