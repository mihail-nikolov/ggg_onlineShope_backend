namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehicleGlassImagesServiceTests
    {
        [TestMethod]
        public void GetByCaption_ShouldReturnNeededImage()
        {
            string testCaption = "2353AA";
            var images = new List<VehicleGlassImage>()
            {
                new VehicleGlassImage() { Caption = "2353aA", Id = 1 },
                new VehicleGlassImage() { Caption = "2353AA", Id = 2 },
                new VehicleGlassImage() { Caption = "h4" },
                new VehicleGlassImage() { Caption = "V5" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassImage>>();
            repositoryMock.Setup(x => x.All()).Returns(() => images);

            VehicleGlassImagesService service = new VehicleGlassImagesService(repositoryMock.Object);

            VehicleGlassImage response = service.GetByCaption(testCaption);

            Assert.AreEqual(response.Id, 2);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByOriginalId_ShouldReturnNeededImage()
        {
            int testOriginalId = 2;
            var images = new List<VehicleGlassImage>()
            {
                new VehicleGlassImage() { OriginalId = 1, Id = 1 },
                new VehicleGlassImage() { OriginalId = 2, Id = 2 },
                new VehicleGlassImage() { OriginalId = 3 },
                new VehicleGlassImage() { OriginalId = 4 }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassImage>>();
            repositoryMock.Setup(x => x.All()).Returns(() => images);

            VehicleGlassImagesService service = new VehicleGlassImagesService(repositoryMock.Object);

            VehicleGlassImage response = service.GetByOriginalId(testOriginalId);

            Assert.AreEqual(response.Id, 2);
            repositoryMock.VerifyAll();
        }
    }
}
