namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehicleMakesServiceTests
    {
        [TestMethod]
        public void GetByName_ShouldReturnNeededMake()
        {
            string testName = "FoRd";
            var makes = new List<VehicleMake>()
            {
                new VehicleMake() { Name = "Fiat" },
                new VehicleMake() { Name = "Mercedes" },
                new VehicleMake() { Name = "FORD", Id = 1 },
                new VehicleMake() { Name = "Audi" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleMake>>();
            repositoryMock.Setup(x => x.All()).Returns(() => makes);

            VehicleMakesService service = new VehicleMakesService(repositoryMock.Object);

            VehicleMake response = service.GetByName(testName);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }
    }
}
