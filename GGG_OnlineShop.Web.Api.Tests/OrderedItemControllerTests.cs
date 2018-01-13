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

    [TestClass]
    public class OrderedItemsControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Order_ShouldThrowException_WhenUsersServiceIsNull()
        {
            var usersMock = new Mock<IUsersService>();
            var controller = new OrderedItemController(null, usersMock.Object);

            var model = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass"
            };

            var result = controller.Order(model);
        }

        [TestMethod]
        public void Order_ShouldAddNewOrderWithAlternativeUserInfo_WhenNotAuthorized()
        {
            mapper.Execute();

            var usersMock = new Mock<IUsersService>();

            var ordersMock = new Mock<IOrderedItemsService>();
            OrderedItem modelToAdd = new OrderedItem()
            {
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                FullAddress = null,
                WithInstallation = false,
                IsDepositNeeded = false,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                PaidPrice = 0,
                Price = 1,
                AnonymousUserInfo = "AnonymousUserInfo",
                AnonymousUserЕmail = "AnonymousUserЕmail",
                CreatedOn = DateTime.MinValue,
                DeletedOn = null,
                Id = 0,
                IsDeleted = false,
                IsInvoiceNeeded = false,
                ModifiedOn = null,
                OtherCodes = null,
                User = null,
                UserId = null
            };

            ordersMock.Setup(v => v.Add(It.IsAny<OrderedItem>()));
            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<OrderedItem>())).Returns(true);

            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object);

            var model = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                Price = 1,
                AnonymousUserInfo = "AnonymousUserInfo",
                AnonymousUserЕmail = "AnonymousUserЕmail",
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            ordersMock.Verify(m => m.IsValidOrder(It.Is<OrderedItem>(x => AreObjectsEqual(x, modelToAdd))));
            ordersMock.Verify(m => m.Add(It.Is<OrderedItem>(x => AreObjectsEqual(x, modelToAdd))));
        }

        [TestMethod]
        public void Order_ShouldAddNewOrderWithUserInfo_WhenAuthorized()
        {
            mapper.Execute();

            var testId = "testId";
            var testUser = new User() { DeliveryCountry = "BG", DeliveryTown = "SF", DeliveryAddress = "Liulin" };
            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(v => v.GetById(testId)).Returns(() => testUser);

            var ordersMock = new Mock<IOrderedItemsService>();
            OrderedItem modelToAdd = new OrderedItem()
            {
                AnonymousUserInfo = null,
                AnonymousUserЕmail = null,
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                FullAddress = "BG; SF; Liulin",
                WithInstallation = false,
                IsDepositNeeded = false,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                PaidPrice = 0,
                Price = 1,
                CreatedOn = DateTime.MinValue,
                DeletedOn = null,
                Id = 0,
                IsDeleted = false,
                IsInvoiceNeeded = false,
                ModifiedOn = null,
                OtherCodes = null,
                User = testUser,
                UserId = testId
            };

            ordersMock.Setup(v => v.Add(It.IsAny<OrderedItem>()));
            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<OrderedItem>())).Returns(true);

            // moq the user
            var claim = new Claim("test", testId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var model = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                Price = 1,
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            ordersMock.Verify(m => m.IsValidOrder(It.Is<OrderedItem>(x => AreObjectsEqual(x, modelToAdd))));
            ordersMock.Verify(m => m.Add(It.Is<OrderedItem>(x => AreObjectsEqual(x, modelToAdd))));
        }

        [TestMethod]
        public void Order_ShouldAddNewOrderWithUserInfoAndAlternativeAddress_WhenAuthorizedAndIsAlternativeAddressTrue()
        {
            mapper.Execute();

            var testId = "testId";
            var testUser = new User() { DeliveryCountry = "BG", DeliveryTown = "SF", DeliveryAddress = "Liulin" };
            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(v => v.GetById(testId)).Returns(() => testUser);

            var ordersMock = new Mock<IOrderedItemsService>();
            OrderedItem modelToAdd = new OrderedItem()
            {
                AnonymousUserInfo = null,
                AnonymousUserЕmail = null,
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                FullAddress = "AlternativeAddress",
                WithInstallation = false,
                IsDepositNeeded = false,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                PaidPrice = 0,
                Price = 1,
                CreatedOn = DateTime.MinValue,
                DeletedOn = null,
                Id = 0,
                IsDeleted = false,
                IsInvoiceNeeded = false,
                ModifiedOn = null,
                OtherCodes = null,
                User = testUser,
                UserId = testId
            };

            ordersMock.Setup(v => v.Add(It.IsAny<OrderedItem>()));
            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<OrderedItem>())).Returns(true);

            // moq the user
            var claim = new Claim("test", testId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var model = new OrderedItemRequestModel()
            {
                FullAddress = "AlternativeAddress",
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                Price = 1,
                UseAlternativeAddress = true
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            ordersMock.Verify(m => m.IsValidOrder(It.Is<OrderedItem>(x => AreObjectsEqual(x, modelToAdd))));
            ordersMock.Verify(m => m.Add(It.Is<OrderedItem>(x => AreObjectsEqual(x, modelToAdd))));
        }

        [TestMethod]
        public void Order_ShouldReturnBadRequest_WhenOrderNotValid()
        {
            mapper.Execute();

            var usersMock = new Mock<IUsersService>();

            var ordersMock = new Mock<IOrderedItemsService>();
            OrderedItem modelToAdd = new OrderedItem()
            {
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                FullAddress = null,
                WithInstallation = false,
                IsDepositNeeded = false,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                PaidPrice = 0,
                Price = 1,
                AnonymousUserInfo = "AnonymousUserInfo",
                AnonymousUserЕmail = "AnonymousUserЕmail",
                CreatedOn = DateTime.MinValue,
                DeletedOn = null,
                Id = 0,
                IsDeleted = false,
                IsInvoiceNeeded = false,
                ModifiedOn = null,
                OtherCodes = null,
                User = null,
                UserId = null
            };

            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<OrderedItem>())).Returns(false);

            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object);

            var model = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                Price = 1,
                AnonymousUserInfo = "AnonymousUserInfo",
                AnonymousUserЕmail = "AnonymousUserЕmail",
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;

            Assert.IsTrue(responseMessage.Contains("Error while valditing order"));
            ordersMock.Verify(m => m.IsValidOrder(It.Is<OrderedItem>(x => AreObjectsEqual(x, modelToAdd))));
        }

        private bool AreObjectsEqual(OrderedItem first, OrderedItem second)
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
