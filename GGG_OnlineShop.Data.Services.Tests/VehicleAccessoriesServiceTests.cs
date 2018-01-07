namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehicleAccessoriesServiceTests
    {
        [TestMethod]
        public void GetByMaterialNumber_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var images = new List<VehicleGlassAccessory>()
            {
                new VehicleGlassAccessory() { MaterialNumber = "2233a", IndustryCode = testCode },
                new VehicleGlassAccessory() { MaterialNumber = "2244B" },
                new VehicleGlassAccessory() { MaterialNumber = "2353aA", Id = 1 },
                new VehicleGlassAccessory() { MaterialNumber = "2728b" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassAccessory>>();
            repositoryMock.Setup(x => x.All()).Returns(() => images);

            VehicleAccessoriesService service = new VehicleAccessoriesService(repositoryMock.Object);

            VehicleGlassAccessory response = service.GetByMaterialNumber(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByIndustryCode_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var images = new List<VehicleGlassAccessory>()
            {
                new VehicleGlassAccessory() { MaterialNumber = "2233a", IndustryCode = "2223A" },
                new VehicleGlassAccessory() { MaterialNumber = testCode, IndustryCode = "2222A" },
                new VehicleGlassAccessory() { MaterialNumber = "23539A", Id = 1 , IndustryCode = testCode },
                new VehicleGlassAccessory() { MaterialNumber = "2728b", IndustryCode = "2224A" },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassAccessory>>();
            repositoryMock.Setup(x => x.All()).Returns(() => images);

            VehicleAccessoriesService service = new VehicleAccessoriesService(repositoryMock.Object);

            VehicleGlassAccessory response = service.GetByIndustryCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAccessory_ShouldReturnNeededItemByMaterialCode_WhenByIndustryCodeNotFound()
        {
            string testCode = "2353AA";
            var images = new List<VehicleGlassAccessory>()
            {
                new VehicleGlassAccessory() { MaterialNumber = "2233a", IndustryCode = "2223A" },
                new VehicleGlassAccessory() { MaterialNumber = "2244B", IndustryCode = "2222A" },
                new VehicleGlassAccessory() { MaterialNumber = testCode, Id = 1 , IndustryCode = "2222A" },
                new VehicleGlassAccessory() { MaterialNumber = "2728b", IndustryCode = "2224A" },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassAccessory>>();
            repositoryMock.Setup(x => x.All()).Returns(() => images);

            VehicleAccessoriesService service = new VehicleAccessoriesService(repositoryMock.Object);

            VehicleGlassAccessory response = service.GetAccessory("0000", testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAccessory_ShouldReturnNeededItemByIndustryCode()
        {
            string testCode = "2353AA";
            var images = new List<VehicleGlassAccessory>()
            {
                new VehicleGlassAccessory() { MaterialNumber = "2233a", IndustryCode = "2223A" },
                new VehicleGlassAccessory() { MaterialNumber = "2244B", IndustryCode = "2222A" },
                new VehicleGlassAccessory() { MaterialNumber = "2359A", Id = 1 , IndustryCode = testCode },
                new VehicleGlassAccessory() { MaterialNumber = "2728b", IndustryCode = "2224A" },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassAccessory>>();
            repositoryMock.Setup(x => x.All()).Returns(() => images);

            VehicleAccessoriesService service = new VehicleAccessoriesService(repositoryMock.Object);

            VehicleGlassAccessory response = service.GetAccessory(testCode, "0000");

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }
    }
}
