namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehicleBodyTypesServiceTests
    {
        [TestMethod]
        public void GetByCode_ShouldReturnNeededBodyType()
        {
            string testCode = "H5";
            var characteristics = new List<VehicleBodyType>()
            {
                new VehicleBodyType() { Code = "S4" },
                new VehicleBodyType() { Code = "h5", Id = 1 },
                new VehicleBodyType() { Code = "h4" },
                new VehicleBodyType() { Code = "V5" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleBodyType>>();
            repositoryMock.Setup(x => x.All()).Returns(() => characteristics);

            VehicleBodyTypesService service = new VehicleBodyTypesService(repositoryMock.Object);

            VehicleBodyType response = service.GetByCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }
    }
}
