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
    public class GoodsServiceTests
    {
        [TestMethod]
        public void GetAllByCode_ShouldReturnNeededGoods_WhenCodeSend()
        {
            string testCode = "2233AGN";

            var goods = new List<Good>()
            {
                new Good() { ID = 1, Code = testCode },
                new Good() { ID = 2, Code = "2222AGN" },
                new Good() { ID = 3, Code = testCode }
            }.AsQueryable();

            var repositoryMock = new Mock<IExternalDbRepository<Good>>();
            repositoryMock.Setup(x => x.All()).Returns(() => goods);

            GoodsService service = new GoodsService(repositoryMock.Object);

            List<Good> response = service.GetAllByCode(testCode).ToList();
            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Code, testCode);
            Assert.AreEqual(response[1].Code, testCode);

            repositoryMock.VerifyAll();
        }
    }
}
