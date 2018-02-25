namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using InternalApiDB.Models.Enums;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class OrderedItemsServiceTests
    {
        [TestMethod]
        public void GetDoneOrders_ShouldReturnNeededItem()
        {
            var items = new List<OrderedItem>()
            {
                new OrderedItem() {Id = 1,  Status = DeliveryStatus.New},
                new OrderedItem() {Id = 2,  Status = DeliveryStatus.Ordered},
                new OrderedItem() {Id = 3,  Status = DeliveryStatus.Done},
                new OrderedItem() {Id = 4,  Status = DeliveryStatus.Done},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<OrderedItem>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new OrderedItemsService(repositoryMock.Object, null);

            var result = service.GetDoneOrders().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].Id);
            Assert.AreEqual(4, result[1].Id);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetNewOrders_ShouldReturnNeededItem()
        {
            var items = new List<OrderedItem>()
            {
                new OrderedItem() {Id = 1,  Status = DeliveryStatus.Done},
                new OrderedItem() {Id = 2,  Status = DeliveryStatus.Ordered},
                new OrderedItem() {Id = 3,  Status = DeliveryStatus.New},
                new OrderedItem() {Id = 4,  Status = DeliveryStatus.New},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<OrderedItem>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new OrderedItemsService(repositoryMock.Object, null);

            var result = service.GetNewOrders().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].Id);
            Assert.AreEqual(4, result[1].Id);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetOrderedProducts_ShouldReturnNeededItem()
        {
            var items = new List<OrderedItem>()
            {
                new OrderedItem() {Id = 1,  Status = DeliveryStatus.New},
                new OrderedItem() {Id = 2,  Status = DeliveryStatus.Done},
                new OrderedItem() {Id = 3,  Status = DeliveryStatus.Ordered},
                new OrderedItem() {Id = 4,  Status = DeliveryStatus.Ordered},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<OrderedItem>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new OrderedItemsService(repositoryMock.Object, null);

            var result = service.GetOrderedProducts().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].Id);
            Assert.AreEqual(4, result[1].Id);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenAbsolutelyAllNeededConditionsOk()
        {
            var order = new OrderedItem()
            {
                AnonymousUserInfo = "test",
                AnonymousUserЕmail = "test",
                UserId = "test",
                EuroCode = "test",
                OtherCodes = "test",
                //IsDepositNeeded = true,
                Price = 20,
                //PaidPrice = 20,
                User = new User() { IsDeferredPaymentAllowed = true},
            };

            var service = new OrderedItemsService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }

        // TODO check later if needed
        //[TestMethod]
        //public void IsValidOrder_SouldReturnFalse_WhenNeededPriceNotEnoughAndUserAndNoDeferredPayment()
        //{
        //    var order = new OrderedItem()
        //    {
        //        AnonymousUserInfo = "test",
        //        AnonymousUserЕmail = "test",
        //        UserId = "test",
        //        EuroCode = "test",
        //        OtherCodes = "test",
        //        //IsDepositNeeded = true,
        //        Price = 20,
        //        //PaidPrice = 5,
        //        User = new User() { IsDeferredPaymentAllowed = false },
        //    };

        //    var service = new OrderedItemsService(null, null);

        //    var result = service.IsValidOrder(order);

        //    Assert.IsFalse(result);
        //}

        // TODO check later if needed
        //[TestMethod]
        //public void IsValidOrder_SouldReturnFalse_WhenNeededPriceAndNoUser()
        //{
        //    var order = new OrderedItem()
        //    {
        //        AnonymousUserInfo = "test",
        //        AnonymousUserЕmail = "test",
        //        EuroCode = "test",
        //        OtherCodes = "test",
        //        //IsDepositNeeded = true,
        //        Price = 20,
        //        //PaidPrice = 5,
        //    };

        //    var service = new OrderedItemsService(null, null);

        //    var result = service.IsValidOrder(order);

        //    Assert.IsFalse(result);
        //}

        [TestMethod]
        public void IsValidOrder_SouldReturnFalse_WhenNoDepositNeededAndNoAnonymousUserInfoAndNoUserInfo()
        {
            var order = new OrderedItem()
            {
                EuroCode = "test",
                OtherCodes = "test",
                //IsDepositNeeded = false,
                Price = 20,
                //PaidPrice = 20
            };

            var service = new OrderedItemsService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenOnlyEuroCodeSendNoDepositeNeededAndAnonymousInfoSend()
        {
            var order = new OrderedItem()
            {
                AnonymousUserInfo = "test",
                AnonymousUserЕmail = "test",
                EuroCode = "test",
                //IsDepositNeeded = false,
                Price = 20,
                //PaidPrice = 20,
            };

            var service = new OrderedItemsService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenOnlyOtherCodesSendNoDepositeNeededAndAnonymousInfoSend()
        {
            var order = new OrderedItem()
            {
                AnonymousUserInfo = "test",
                AnonymousUserЕmail = "test",
                OtherCodes = "test",
                //IsDepositNeeded = false,
                Price = 20,
                //PaidPrice = 20,
            };

            var service = new OrderedItemsService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenDepositNeededAndPaidPriceEnough()
        {
            var order = new OrderedItem()
            {
                AnonymousUserInfo = "test",
                AnonymousUserЕmail = "test",
                OtherCodes = "test",
                //IsDepositNeeded = true,
                Price = 20,
                //PaidPrice = 20,
            };

            var service = new OrderedItemsService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenDepositNeededAndPaidPriceNotEnoughButDeferredPaymentAllowed()
        {
            var order = new OrderedItem()
            {
                UserId = "test",
                User = new User() { IsDeferredPaymentAllowed = true, Id = "test" },
                OtherCodes = "test",
                //IsDepositNeeded = true,
                Price = 20,
                //PaidPrice = 4
            };

            var service = new OrderedItemsService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }
    }
}
