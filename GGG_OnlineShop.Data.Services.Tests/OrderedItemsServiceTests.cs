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
            var order = new List<Order>()
            {
                new Order() {Id = 1,  Status = DeliveryStatus.Unpaid},
                new Order() {Id = 2,  Status = DeliveryStatus.Ordered},
                new Order() {Id = 3,  Status = DeliveryStatus.Done},
                new Order() {Id = 4,  Status = DeliveryStatus.Done},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Order>>();
            repositoryMock.Setup(x => x.All()).Returns(() => order);

            var service = new OrdersService(repositoryMock.Object, null);

            var result = service.GetDoneOrders().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].Id);
            Assert.AreEqual(4, result[1].Id);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetNewOrders_ShouldReturnNeededItem()
        {
            var order = new List<Order>()
            {
                new Order() {Id = 1,  Status = DeliveryStatus.Unpaid},
                new Order() {Id = 2,  Status = DeliveryStatus.Ordered},
                new Order() {Id = 3,  Status = DeliveryStatus.Done},
                new Order() {Id = 4,  Status = DeliveryStatus.Done},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Order>>();
            repositoryMock.Setup(x => x.All()).Returns(() => order);

            var service = new OrdersService(repositoryMock.Object, null);

            var result = service.GetNewOrders().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].Id);
            Assert.AreEqual(4, result[1].Id);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetOrderedProducts_ShouldReturnNeededItem()
        {
            var order = new List<Order>()
            {
                new Order() {Id = 1,  Status = DeliveryStatus.Unpaid},
                new Order() {Id = 2,  Status = DeliveryStatus.Ordered},
                new Order() {Id = 3,  Status = DeliveryStatus.Done},
                new Order() {Id = 4,  Status = DeliveryStatus.Done},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Order>>();
            repositoryMock.Setup(x => x.All()).Returns(() => order);

            var service = new OrdersService(repositoryMock.Object, null);

            var result = service.GetOrderedProducts().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].Id);
            Assert.AreEqual(4, result[1].Id);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenAbsolutelyAllNeededConditionsOk()
        {
            var order = new Order()
            {
                UserInfo = "test",
                UserЕmail = "test",
                UserId = "test",
                Price = 20,
                PaidPrice = 20,
                User = new User() { IsDeferredPaymentAllowed = true},
            };

            var service = new OrdersService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenOnlyEuroCodeSendNoAnonymousInfoSend()
        {
            var order = new Order()
            {
                UserInfo = "test",
                UserЕmail = "test",
                Price = 20,
                PaidPrice = 20,
            };

            var service = new OrdersService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenOnlyOtherCodesSendNoAnonymousInfoSend()
        {
            var order = new Order()
            {
                UserInfo = "test",
                UserЕmail = "test",
                Price = 20,
                PaidPrice = 20,
            };

            var service = new OrdersService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidOrder_SouldReturnTrue_WhenPaidPriceNotEnoughButDeferredPaymentAllowed()
        {
            var order = new Order()
            {
                UserId = "test",
                User = new User() { IsDeferredPaymentAllowed = true, Id = "test" },
                Price = 20,
                PaidPrice = 0
            };

            var service = new OrdersService(null, null);

            var result = service.IsValidOrder(order);

            Assert.IsTrue(result);
        }
    }
}
