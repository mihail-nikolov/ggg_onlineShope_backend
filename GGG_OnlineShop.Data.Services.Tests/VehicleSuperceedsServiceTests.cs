namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using InternalApiDB.Models;

    [TestClass]
    public class VehicleSuperceedsServiceTests
    {
        [TestMethod]
        public void GetByOldEuroCode_ShouldReturnItemWithSameCode_EvenWhenLettersCaseIsNotTheSame()
        {
            string testCode = "2233abA";
            var superceeds = new List<VehicleGlassSuperceed>()
            {
                new VehicleGlassSuperceed() { OldEuroCode = "test" },
                new VehicleGlassSuperceed() { OldEuroCode = "2233ABA" },
                new VehicleGlassSuperceed() { OldEuroCode = "test" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassSuperceed>>();
            repositoryMock.Setup(x => x.All()).Returns(() => superceeds);

            VehicleSuperceedsService service = new VehicleSuperceedsService(repositoryMock.Object);

            VehicleGlassSuperceed response = service.GetByOldEuroCode(testCode);

            Assert.AreEqual(response.OldEuroCode, "2233ABA");
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByOldLocalCode_ShouldReturnItemWithSameCode_EvenWhenLettersCaseIsNotTheSame()
        {
            string testCode = "2233abA";
            var superceeds = new List<VehicleGlassSuperceed>()
            {
                new VehicleGlassSuperceed() { OldLocalCode = "test" },
                new VehicleGlassSuperceed() { OldLocalCode = "2233ABA" },
                new VehicleGlassSuperceed() { OldLocalCode = "test" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassSuperceed>>();
            repositoryMock.Setup(x => x.All()).Returns(() => superceeds);

            VehicleSuperceedsService service = new VehicleSuperceedsService(repositoryMock.Object);

            VehicleGlassSuperceed response = service.GetByOldLocalCode(testCode);

            Assert.AreEqual(response.OldLocalCode, "2233ABA");
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByOldMaterialNumber_ShouldReturnItemWithSameCode_EvenWhenLettersCaseIsNotTheSame()
        {
            string testCode = "2233abA";
            var superceeds = new List<VehicleGlassSuperceed>()
            {
                new VehicleGlassSuperceed() { OldMaterialNumber = "test" },
                new VehicleGlassSuperceed() { OldMaterialNumber = "2233ABA" },
                new VehicleGlassSuperceed() { OldMaterialNumber = "test" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassSuperceed>>();
            repositoryMock.Setup(x => x.All()).Returns(() => superceeds);

            VehicleSuperceedsService service = new VehicleSuperceedsService(repositoryMock.Object);

            VehicleGlassSuperceed response = service.GetByOldMaterialNumber(testCode);

            Assert.AreEqual(response.OldMaterialNumber, "2233ABA");
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByOldOesCode_ShouldReturnItemsWithSameCode_EvenWhenLettersCaseIsNotTheSame()
        {
            string testCode = "2233abA";
            var superceeds = new List<VehicleGlassSuperceed>()
            {
                new VehicleGlassSuperceed() { OldOesCode = "test" },
                new VehicleGlassSuperceed() { OldOesCode = "2233ABA" },
                new VehicleGlassSuperceed() { OldOesCode = "test" },
                new VehicleGlassSuperceed() { OldOesCode = "2233abA" },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassSuperceed>>();
            repositoryMock.Setup(x => x.All()).Returns(() => superceeds);

            VehicleSuperceedsService service = new VehicleSuperceedsService(repositoryMock.Object);

            var response = service.GetByOldOesCode(testCode).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].OldOesCode, "2233ABA");
            Assert.AreEqual(response[1].OldOesCode, "2233abA");
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetSuperceed_ShouldReturnItemsByOldEuroCode_WhenOldEuroCodeSend()
        {
            string testCode = "2233abA";
            var superceeds = new List<VehicleGlassSuperceed>()
            {
                new VehicleGlassSuperceed() { Id = 1, OldEuroCode = testCode },
                new VehicleGlassSuperceed() { Id = 2, OldLocalCode = "2233ABC" },
                new VehicleGlassSuperceed() { Id = 3, OldMaterialNumber = "test" },
                new VehicleGlassSuperceed() { Id = 4, OldOesCode = "TestOes" },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassSuperceed>>();
            repositoryMock.Setup(x => x.All()).Returns(() => superceeds);

            VehicleSuperceedsService service = new VehicleSuperceedsService(repositoryMock.Object);

            var response = service.GetSuperceed(testCode, string.Empty, string.Empty);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }


        [TestMethod]
        public void GetSuperceed_ShouldReturnItemByOldLocalCode_WhenOldLocalCodeSend()
        {
            string testCode = "2233abA";
            var superceeds = new List<VehicleGlassSuperceed>()
            {
                new VehicleGlassSuperceed() { Id = 1, OldLocalCode = "test",  OldMaterialNumber = "test", OldEuroCode = "test" },
                new VehicleGlassSuperceed() { Id = 2, OldLocalCode = testCode,  OldMaterialNumber = "test", OldEuroCode = "test" },
                new VehicleGlassSuperceed() { Id = 3, OldLocalCode = "test",  OldMaterialNumber = "test", OldEuroCode = "test" },
                new VehicleGlassSuperceed() { Id = 4, OldOesCode = "TestOes" },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassSuperceed>>();
            repositoryMock.Setup(x => x.All()).Returns(() => superceeds);

            VehicleSuperceedsService service = new VehicleSuperceedsService(repositoryMock.Object);

            var response = service.GetSuperceed(string.Empty, testCode, string.Empty);

            Assert.AreEqual(response.Id, 2);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetSuperceed_ShouldReturnItemByOldMaterialNumbere_WhenOldMaterialNumberSend()
        {
            string testCode = "2233abA";
            var superceeds = new List<VehicleGlassSuperceed>()
            {
                new VehicleGlassSuperceed() { Id = 1, OldLocalCode = "test",  OldMaterialNumber = "test", OldEuroCode = "test" },
                new VehicleGlassSuperceed() { Id = 2, OldLocalCode = testCode,  OldMaterialNumber = "test", OldEuroCode = "test" },
                new VehicleGlassSuperceed() { Id = 3, OldLocalCode = "test",  OldMaterialNumber = testCode, OldEuroCode = "test" },
                new VehicleGlassSuperceed() { Id = 4, OldOesCode = "TestOes" },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassSuperceed>>();
            repositoryMock.Setup(x => x.All()).Returns(() => superceeds);

            VehicleSuperceedsService service = new VehicleSuperceedsService(repositoryMock.Object);

            var response = service.GetSuperceed(string.Empty, string.Empty, testCode);

            Assert.AreEqual(response.Id, 3);
            repositoryMock.VerifyAll();
        }
    }
}
