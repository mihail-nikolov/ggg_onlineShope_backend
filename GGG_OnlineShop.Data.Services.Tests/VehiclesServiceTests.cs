namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehiclesServiceTests
    {
        [TestMethod]
        public void GetAllByMakeId_ShouldReturnNeededItems()
        {
            int id = 2;
            var items = new List<Vehicle>()
            {
                new Vehicle() { MakeId = 3, ModelId = id, BodyTypeId = id},
                new Vehicle() { Id = 1, MakeId = id, ModelId = 2, BodyTypeId = 2},
                new Vehicle() { Id = 2, MakeId = id, ModelId = 1, BodyTypeId = 1},
                new Vehicle() { MakeId = 10, ModelId = 2, BodyTypeId = 2},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Vehicle>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehiclesService(repositoryMock.Object);

            var response = service.GetAllByMakeId(id).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Id, 1);
            Assert.AreEqual(response[1].Id, 2);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllByMakeAndModelIds_ShouldReturnNeededItems()
        {
            int id = 2;
            var items = new List<Vehicle>()
            {
                new Vehicle() { MakeId = id, ModelId = 4, BodyTypeId = 10 },
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = 3, Id = 1 },
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = id, Id = 2 },
                new Vehicle() { MakeId = id, ModelId = 3, BodyTypeId = id },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Vehicle>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehiclesService(repositoryMock.Object);

            var response = service.GetAllByMakeAndModelIds(id, id).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Id, 1);
            Assert.AreEqual(response[1].Id, 2);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetVehicleByMakeModelAndBodyTypeIds_ShouldReturnNeededItem()
        {
            int id = 2;
            var items = new List<Vehicle>()
            {
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = 10 },
                new Vehicle() { MakeId = id, ModelId = 3, BodyTypeId = 3, Id = 1 },
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = id, Id = 2 },
                new Vehicle() { MakeId = id, ModelId = 3, BodyTypeId = id },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Vehicle>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehiclesService(repositoryMock.Object);

            var response = service.GetVehicleByMakeModelAndBodyTypeIds(id, id, id);

            Assert.AreEqual(response.Id, 2);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetModelIdsByMakeId_ShouldReturnNeededItems()
        {
            int id = 2;
            var items = new List<Vehicle>()
            {
                new Vehicle() { MakeId = id, ModelId = 1, BodyTypeId = 10 },
                new Vehicle() { MakeId = id, ModelId = 1, BodyTypeId = 3, Id = 1 },
                new Vehicle() { MakeId = id, ModelId = 2, BodyTypeId = id, Id = 2 },
                new Vehicle() { MakeId = id, ModelId = 3, BodyTypeId = id },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Vehicle>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehiclesService(repositoryMock.Object);

            var response = service.GetModelIdsByMakeId(id);

            Assert.AreEqual(response.Count, 3);
            Assert.AreEqual(response[0], 1);
            Assert.AreEqual(response[1], 2);
            Assert.AreEqual(response[2], 3);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetBodyTypeIdsByModelIdAndMakeId_ShouldReturnNeededItems()
        {
            int id = 2;
            var items = new List<Vehicle>()
            {
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = 1 },
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = 2 },
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = 2 },
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = null },
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = null },
                new Vehicle() { MakeId = id, ModelId = id, BodyTypeId = 2, Id = 1 },
                new Vehicle() { MakeId = id, ModelId = 10, BodyTypeId = id, Id = 2 },
                new Vehicle() { MakeId = 3, ModelId = id, BodyTypeId = id },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Vehicle>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehiclesService(repositoryMock.Object);

            var response = service.GetBodyTypeIdsByModelIdAndMakeId(id, id);

            Assert.AreEqual(response.Count, 3);
            Assert.AreEqual(response[0], 1);
            Assert.AreEqual(response[1], 2);
            Assert.AreEqual(response[2], null);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetBodyTypeIdsByModelIdAndMakeId_ShouldReturnNeededItems_WhenModelIdIsNull()
        {
            int id = 2;
            var items = new List<Vehicle>()
            {
                new Vehicle() { MakeId = id, ModelId = null, BodyTypeId = 1 },
                new Vehicle() { MakeId = id, ModelId = null, BodyTypeId = 2 },
                new Vehicle() { MakeId = id, ModelId = null, BodyTypeId = 2 },
                new Vehicle() { MakeId = id, ModelId = null, BodyTypeId = null },
                new Vehicle() { MakeId = id, ModelId = null, BodyTypeId = null },
                new Vehicle() { MakeId = id, ModelId = null, BodyTypeId = 2, Id = 1 },
                new Vehicle() { MakeId = id, ModelId = 10, BodyTypeId = id, Id = 2 },
                new Vehicle() { MakeId = 3, ModelId = id, BodyTypeId = id },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<Vehicle>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehiclesService(repositoryMock.Object);

            var response = service.GetBodyTypeIdsByModelIdAndMakeId(id, null);

            Assert.AreEqual(response.Count, 3);
            Assert.AreEqual(response[0], 1);
            Assert.AreEqual(response[1], 2);
            Assert.AreEqual(response[2], null);
            repositoryMock.VerifyAll();
        }


        [TestMethod]
        public void GetApplicableGLasses_ShouldReturnNeededItems()
        {
            var vehicle = new Vehicle()
            {
                MakeId = 1, ModelId = 1, BodyTypeId = 1,
                VehicleGlasses = new List<VehicleGlass>()
                {
                    new VehicleGlass() {Id = 1 }, new VehicleGlass() {Id = 2 }
                }
            };

            var repositoryMock = new Mock<IInternalDbRepository<Vehicle>>();

            var service = new VehiclesService(repositoryMock.Object);

            var response = service.GetApplicableGLasses(vehicle).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Id, 1);
            Assert.AreEqual(response[1].Id, 2);
        }

        [TestMethod]
        public void GetApplicableGLassesByProductType_ShouldReturnNeededItems()
        {
            string productType = "windscreen";
            var vehicle = new Vehicle()
            {
                MakeId = 1,
                ModelId = 1,
                BodyTypeId = 1,
                VehicleGlasses = new List<VehicleGlass>()
                {
                    new VehicleGlass() {Id = 1, ProductType = productType },
                    new VehicleGlass() {Id = 2, ProductType = "test" },
                    new VehicleGlass() {Id = 3, ProductType = productType }
                }
            };

            var repositoryMock = new Mock<IInternalDbRepository<Vehicle>>();

            var service = new VehiclesService(repositoryMock.Object);

            var response = service.GetApplicableGLassesByProductType(vehicle, productType).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Id, 1);
            Assert.AreEqual(response[1].Id, 3);
        }
    }
}
