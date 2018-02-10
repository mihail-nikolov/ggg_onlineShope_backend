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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Order_ShouldThrowException_WhenUsersServiceIsNull()
        {
            var usersMock = new Mock<IUsersService>();
            var controller = new OrderedItemController(null, usersMock.Object, null, null); // TODO

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
            var emailsMock = new Mock<IEmailsService>();

            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, emailsMock.Object, null); // TODO

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
            var emailsMock = new Mock<IEmailsService>();

            // moq the user
            var claim = new Claim("test", testId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, emailsMock.Object, null) // TODO
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
            var emailsMock = new Mock<IEmailsService>();

            // moq the user
            var claim = new Claim("test", testId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, emailsMock.Object, null) // TODO
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
        public void Order_ShouldSendEmailToRegisteredUser_WhenValidOrder()
        {
            mapper.Execute();

            var testId = "testId";
            var testUser = new User() { DeliveryCountry = "BG", DeliveryTown = "SF", DeliveryAddress = "Liulin", Email = "testEmail" };
            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(v => v.GetById(testId)).Returns(() => testUser);

            var ordersMock = new Mock<IOrderedItemsService>();

            ordersMock.Setup(v => v.Add(It.IsAny<OrderedItem>()));
            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<OrderedItem>())).Returns(true);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(testUser.Email, string.Format(GlobalConstants.OrderMade, It.IsAny<string>()),
                                  It.IsAny<string>(), GlobalConstants.SMTPServer,
                                  GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword));

            // moq the user
            var claim = new Claim("test", testId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, emailsMock.Object, null) // TODO
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
            emailsMock.VerifyAll();
        }

        [TestMethod]
        public void Order_ShouldSendEmailToAnonymousUser_WhenValidOrder()
        {
            mapper.Execute();

            var testId = "testId";
            string eurocode = "2233A";
            string othercodes = "3212; AS1312";
            string anonymousEmail = "anonymousEmail";
            string fullAddress = "Sofia BG";
            string manufacturer = "nordglass";
            bool isInvoiceNeeded = true;
            bool isInstallationNeeded = false;
            string deliveryNotes = "DeliveryNotes";
            string description = "ALFA-ROMEO Windscreen";
            double price = 102;
            double paidPrice = 70;
            DeliveryStatus status = DeliveryStatus.New;

            string isInstallationNeededBG = EnglishBulgarianDictionary.Namings[isInstallationNeeded.ToString()];
            string isInvoiceNeededBG = EnglishBulgarianDictionary.Namings[isInvoiceNeeded.ToString()];
            string statusBG = EnglishBulgarianDictionary.Namings[status.ToString()];

            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(v => v.GetById(testId)).Returns(() => null);

            var ordersMock = new Mock<IOrderedItemsService>();

            ordersMock.Setup(v => v.Add(It.IsAny<OrderedItem>()));
            ordersMock.Setup(v => v.IsValidOrder(It.IsAny<OrderedItem>())).Returns(true);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(
                                  anonymousEmail, string.Format(GlobalConstants.OrderMade, It.IsAny<string>()),
                                  It.Is<string>(y => y.Contains(eurocode) && y.Contains(othercodes) && y.Contains(fullAddress) &&
                                                     y.Contains(manufacturer) && y.Contains(isInvoiceNeededBG) &&
                                                     y.Contains(deliveryNotes) && y.Contains(description) &&
                                                     y.Contains(statusBG) && y.Contains(isInstallationNeededBG) &&
                                                     y.Contains(price.ToString()) && y.Contains(paidPrice.ToString())),
                                  GlobalConstants.SMTPServer,
                                  GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword));

            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, emailsMock.Object, null); // TODO

            var model = new OrderedItemRequestModel()
            {
                FullAddress = fullAddress,
                Manufacturer = manufacturer,
                EuroCode = eurocode,
                OtherCodes = othercodes,
                Status = status,
                DeliveryNotes = deliveryNotes,
                Description = description,
                Price = price,
                PaidPrice = paidPrice,
                UseAlternativeAddress = true,
                IsInvoiceNeeded = isInvoiceNeeded,
                WithInstallation = isInstallationNeeded,
                AnonymousUserЕmail = anonymousEmail
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

            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object, null, null); // TODO

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
