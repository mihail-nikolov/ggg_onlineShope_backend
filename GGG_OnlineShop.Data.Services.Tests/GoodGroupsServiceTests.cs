namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using SkladProDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ExternalDb;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class GoodGroupsServiceTests
    {
        [TestMethod]
        public void GetGoodGroupsByIds_ShouldReturnNeededGoodGroups_WhenEnumerableOfGoodGroupIdsSend()
        {
            int? testId1 = 1;
            int? testId2 = 2;
            int? testId3 = 3;

            List<int?> goodGroupIds = new List<int?>() { testId1, testId2, testId3 };

            GoodsGroup testGoodGroup1 = new GoodsGroup() { ID = 1, Name = "nordglass" };
            GoodsGroup testGoodGroup2 = new GoodsGroup() { ID = 2, Name = "AGC" };
            GoodsGroup testGoodGroup3 = new GoodsGroup() { ID = 3, Name = "Lamex" };

            var repositoryMock = new Mock<IExternalDbRepository<GoodsGroup>>();
            repositoryMock.Setup(x => x.GetById(testId1)).Returns(() => testGoodGroup1);
            repositoryMock.Setup(x => x.GetById(testId2)).Returns(() => testGoodGroup2);
            repositoryMock.Setup(x => x.GetById(testId3)).Returns(() => testGoodGroup3);

            GoodGroupsService service = new GoodGroupsService(repositoryMock.Object);

            List<GoodsGroup> response = service.GetGoodGroupsByIds(goodGroupIds).ToList();
            Assert.AreEqual(response.Count, 3);
            Assert.AreEqual(response[0], testGoodGroup1);
            Assert.AreEqual(response[1], testGoodGroup2);
            Assert.AreEqual(response[2], testGoodGroup3);

            repositoryMock.VerifyAll();
        }
    }
}
