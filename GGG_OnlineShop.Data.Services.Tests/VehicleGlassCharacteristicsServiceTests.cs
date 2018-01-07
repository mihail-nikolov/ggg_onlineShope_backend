namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehicleGlassCharacteristicsServiceTests
    {
        [TestMethod]
        public void GetByName_ShouldReturnNeededCharacteristic()
        {
            string testName = "Laminated";
            var characteristics = new List<VehicleGlassCharacteristic>()
            {
                new VehicleGlassCharacteristic() { Name = "Alarm Wire" },
                new VehicleGlassCharacteristic() { Name = "Center Part" },
                new VehicleGlassCharacteristic() { Name = "LAMINATED", Id = 1 },
                new VehicleGlassCharacteristic() { Name = "Top extrusion" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassCharacteristic>>();
            repositoryMock.Setup(x => x.All()).Returns(() => characteristics);

            VehicleGlassCharacteristicsService service = new VehicleGlassCharacteristicsService(repositoryMock.Object);

            VehicleGlassCharacteristic response = service.GetByName(testName);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }
    }
}
