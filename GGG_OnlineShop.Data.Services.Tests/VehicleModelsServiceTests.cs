namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehicleModelsServiceTests
    {
        [TestMethod]
        public void GetByName_ShouldReturnNeededModel()
        {
            string testName = "Focus";
            var models = new List<VehicleModel>()
            {
                new VehicleModel() { Name = "A6" },
                new VehicleModel() { Name = "Fiesta" },
                new VehicleModel() { Name = "FOCUS", Id = 1 },
                new VehicleModel() { Name = "ML" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleModel>>();
            repositoryMock.Setup(x => x.All()).Returns(() => models);

            VehicleModelsService service = new VehicleModelsService(repositoryMock.Object);

            VehicleModel response = service.GetByName(testName);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }
    }
}