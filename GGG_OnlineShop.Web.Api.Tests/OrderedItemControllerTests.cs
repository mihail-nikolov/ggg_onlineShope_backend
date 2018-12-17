using System.Collections.Generic;

namespace GGG_OnlineShop.Web.Api.Tests
{
    using System;
    using Moq;
    using Data.Services.Contracts;
    using Controllers;
    using Models;
    using System.Web.Http.Results;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using InternalApiDB.Models;
    using System.Reflection;
    using System.Security.Principal;
    using System.Security.Claims;
    using Common;
    using InternalApiDB.Models.Enums;

    [TestClass]
    public class OrderedItemsControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();
        private readonly Mock<ILogsService> mockedLogger = new Mock<ILogsService>();
        private readonly string controllerName = "OrderedItemController";

        public OrderedItemsControllerTests()
        {
            mockedLogger.Setup(x => x.LogError(It.IsAny<Exception>(), "", controllerName, It.IsAny<string>()));
        }

        [TestMethod]
        public void Order_ShouldReturnInternalServerErrorAndLogError_WhenUsersServiceIsNull()
        {
            var usersMock = new Mock<IUsersService>();
            var controller = new OrderedItemController(null, usersMock.Object, null, mockedLogger.Object);

            var model = new OrderRequestModel()
            {
                IsInvoiceNeeded = true
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "Order"));
        }

        [TestMethod]
        public void Order_ShouldAddNewOrderWithUserInfo_WhenAuthorized()
        {
            mapper.Execute();

            var testId = "testId";
            var testUser = new User() { DeliveryCountry = "BG", DeliveryTown = "SF", DeliveryAddress = "Liulin" };
            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(v => v.GetById(testId)).Returns(() => testUser);

            var ordersMock = new Mock<IOrdersService>();
            OrderedItem orderedItem = new OrderedItem()
            {
                Manufacturer = "nordglass",
                Price = 1,
                CreatedOn = DateTime.MinValue,
                DeletedOn = null,
                Id = 0,
                IsDeleted = false,
                ModifiedOn = null,
                OtherCodes = null,
            };

            var modelToAdd = new Order
            {
                UserInfo = null,
                UserЕmail = "testEmail",
                Status = DeliveryStatus.Unpaid,
                FullAddress = "BG; SF; Liulin",
                WithInstallation = false,
                DeliveryNotes = "DeliveryNotes",
                PaidPrice = 1,
                Price = 1,
                CreatedOn = DateTime.MinValue,
                DeletedOn = null,
                Id = 0,
                IsDeleted = false,
                IsInvoiceNeeded = false,
                ModifiedOn = null,
                User = testUser,
                UserId = testId,
                OrderedItems = new List<OrderedItem>
                {
                    orderedItem
                }
            };

            ordersMock.Setup(v => v.Add(It.IsAny<Order>()));
            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<Order>())).Returns(true);
            var emailsMock = new Mock<IEmailsService>();

            // moq the user
            var claim = new Claim("test", testId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, emailsMock.Object, null)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var orderedItemRequest = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass",
                Description = "Description",
                Price = 1
            };

            var model = new OrderRequestModel
            {
                DeliveryNotes = "DeliveryNotes",
                Price = 1,
                PaidPrice = 1,
                UserЕmail = "testEmail",
                UserId = testId,
                FullAddress = "BG; SF; Liulin",
                OrderedItems = new List<OrderedItemRequestModel>
                {
                    orderedItemRequest
                }
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            ordersMock.Verify(m => m.IsValidOrder(It.Is<Order>(x => AreObjectsEqual(x, modelToAdd))));
            ordersMock.Verify(m => m.Add(It.Is<Order>(x => AreObjectsEqual(x, modelToAdd))));
        }

        [TestMethod]
        public void Order_ShouldSendEmailToUser_WhenValidOrder()
        {
            mapper.Execute();

            var testId = "testId";
            var testUser = new User() { DeliveryCountry = "BG", DeliveryTown = "SF", DeliveryAddress = "Liulin", Email = "testEmail" };
            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(v => v.GetById(testId)).Returns(() => testUser);

            var ordersMock = new Mock<IOrdersService>();

            ordersMock.Setup(v => v.Add(It.IsAny<Order>()));
            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<Order>())).Returns(true);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(testUser.Email, string.Format(GlobalConstants.OrderMade, It.IsAny<string>()),
                                  It.IsAny<string>(), GlobalConstants.SMTPServer,
                                  GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword));

            // moq the user
            var claim = new Claim("test", testId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, emailsMock.Object, null)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var orderedItemRequest = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass",
                Description = "Description",
                Price = 1
            };

            var model = new OrderRequestModel
            {
                DeliveryNotes = "DeliveryNotes",
                Price = 1,
                PaidPrice = 1,
                UserЕmail = "testEmail",
                UserId = testId,
                FullAddress = "BG; SF; Liulin",
                OrderedItems = new List<OrderedItemRequestModel>
                {
                    orderedItemRequest
                }
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            emailsMock.VerifyAll();
        }

        [TestMethod]
        public void Order_ShouldReturnBadRequest_WhenOrderNotValid()
        {
            mapper.Execute();

            var usersMock = new Mock<IUsersService>();

            var ordersMock = new Mock<IOrdersService>();
            OrderedItem orderedItem = new OrderedItem()
            {
                Manufacturer = "nordglass",
                Price = 1,
                CreatedOn = DateTime.MinValue,
                DeletedOn = null,
                Id = 0,
                IsDeleted = false,
                ModifiedOn = null,
                OtherCodes = null,
            };

            var modelToAdd = new Order
            {
                UserInfo = null,
                UserЕmail = "testEmail",
                Status = DeliveryStatus.Unpaid,
                FullAddress = "BG; SF; Liulin",
                WithInstallation = false,
                DeliveryNotes = "DeliveryNotes",
                PaidPrice = 1,
                Price = 1,
                CreatedOn = DateTime.MinValue,
                DeletedOn = null,
                Id = 0,
                IsDeleted = false,
                IsInvoiceNeeded = false,
                ModifiedOn = null,
                OrderedItems = new List<OrderedItem>
                {
                    orderedItem
                }
            };

            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<Order>())).Returns(false);

            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, null, null);

            var orderedItemRequest = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass",
                Description = "Description",
                Price = 1
            };

            var model = new OrderRequestModel
            {
                DeliveryNotes = "DeliveryNotes",
                Price = 1,
                PaidPrice = 1,
                UserЕmail = "testEmail",
                FullAddress = "BG; SF; Liulin",
                OrderedItems = new List<OrderedItemRequestModel>
                {
                    orderedItemRequest
                }
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;

            Assert.IsTrue(responseMessage.Contains("Error while valditing order"));
            ordersMock.Verify(m => m.IsValidOrder(It.Is<Order>(x => AreObjectsEqual(x, modelToAdd))));
        }

        private bool AreObjectsEqual(Order first, Order second)
        {
            if (first == null && second == null)
            {
                return true;
            }
            if (first == null || second == null)
            {
                return false;
            }
            Type firstType = first.GetType();
            if (second.GetType() != firstType)
            {
                return false; // Or throw an exception
            }
            // This will only use public properties. Is that enough?
            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object firstValue = propertyInfo.GetValue(first, null);
                    object secondValue = propertyInfo.GetValue(second, null);
                    if (!object.Equals(firstValue, secondValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
