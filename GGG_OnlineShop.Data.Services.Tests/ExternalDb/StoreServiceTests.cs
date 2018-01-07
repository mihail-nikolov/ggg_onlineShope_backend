namespace GGG_OnlineShop.Data.Services.Tests.ExternalDb
{
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Services.ExternalDb;
    using SkladProDB.Models;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class StoreServiceTests
    {
        [TestMethod]
        public void GetAllByGoodId_ShouldReturnNeededStoreItems_WhenGoodIdSend()
        {
            int testGoodId = 1;

            var stores = new List<Store>()
            {
                new Store() { ID = 1, GoodID = testGoodId },
                new Store() { ID = 2,  GoodID = 2 },
                new Store() { ID = 3,  GoodID = testGoodId },
            }.AsQueryable();

            var repositoryMock = new Mock<IExternalDbRepository<Store>>();
            repositoryMock.Setup(x => x.All()).Returns(() => stores);

            StoreService service = new StoreService(repositoryMock.Object);

            List<Store> response = service.GetAllByGoodId(testGoodId).ToList();
            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].ID, 1);
            Assert.AreEqual(response[1].ID, 3);

            repositoryMock.VerifyAll();
        }
    }
}
