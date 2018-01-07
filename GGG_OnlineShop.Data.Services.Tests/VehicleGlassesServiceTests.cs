namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehicleGlassesServiceTests
    {
        [TestMethod]
        public void GetByEuroCode_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { MaterialNumber = "2233a", EuroCode = "2233a"},
                new VehicleGlass() { MaterialNumber = "2244B", EuroCode = "2353Aa", Id = 1 },
                new VehicleGlass() { MaterialNumber = "2353aA", EuroCode = "2233a" },
                new VehicleGlass() { MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            VehicleGlass response = service.GetByEuroCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetGlassesByEuroCode_ShouldReturnNeededItems()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { MaterialNumber = "2233a", EuroCode = "2233a"},
                new VehicleGlass() { MaterialNumber = "2244B", EuroCode = "2353Aa", Id = 1 },
                new VehicleGlass() { MaterialNumber = "2353aA", EuroCode = "2353Aa", Id = 2 },
                new VehicleGlass() { MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetGlassesByEuroCode(testCode).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Id, 1);
            Assert.AreEqual(response[1].Id, 2);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAccessories_ShouldReturnNeededItems()
        {
            int id = 2;

            var glass = new VehicleGlass()
            {
                Id = 2,
                VehicleGlassAccessories = new List<VehicleGlassAccessory>()
                {
                    new VehicleGlassAccessory() {Id = 3 },
                    new VehicleGlassAccessory() {Id = 4 },
                }
            };

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(() => glass);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetAccessories(id).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Id, 3);
            Assert.AreEqual(response[1].Id, 4);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByMaterialNumber_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = testCode },
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1 },
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a" },
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            VehicleGlass response = service.GetByMaterialNumber(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByLocalCode_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { LocalCode = "2233a", MaterialNumber = "2233a", EuroCode = "2233a", OesCode = testCode },
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1 },
                new VehicleGlass() { LocalCode = "2233a", MaterialNumber = "2728b", EuroCode = "2233a" },
                new VehicleGlass() { LocalCode = "2728b", MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            VehicleGlass response = service.GetByLocalCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByScanCode_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { IndustryCode = "2233aB", LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = testCode },
                new VehicleGlass() { IndustryCode = testCode, LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1 },
                new VehicleGlass() { IndustryCode = "2233aC", LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a" },
                new VehicleGlass() { IndustryCode = "2728bD", LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            VehicleGlass response = service.GetByIndustryCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByOesCode_ShouldReturnNeededItems()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1, OesCode = "2353Aa" },
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlass() { LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetByOesCode(testCode).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Id, 1);
            Assert.AreEqual(response[1].Id, 2);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetGlass_ShouldReturnNeededItem_WhenEurocodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { IndustryCode = "2233aE", LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlass() { IndustryCode = "2353aAB",LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353aA", Id = 1, OesCode = "2353Aa" },
                new VehicleGlass() { IndustryCode = "2728bR", LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlass() { IndustryCode = "2728bY", LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetGlass(testCode, testCode, testCode, testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetGlass_ShouldReturnNeededItem_WhenMaterialNumberSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { IndustryCode = testCode, LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlass() { IndustryCode = testCode, LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlass() { IndustryCode = testCode, LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlass() { IndustryCode = testCode, LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetGlass(string.Empty, testCode, testCode, testCode);

            Assert.AreEqual(response.Id, 3);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetGlass_ShouldReturnNeededItem_WhenScanCodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { IndustryCode = "2233aE", LocalCode = testCode, MaterialNumber = testCode, EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlass() { IndustryCode = "2353aA", LocalCode = testCode, MaterialNumber = "2233aB", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlass() { IndustryCode = "2728bR", LocalCode = testCode, MaterialNumber = "2233aB", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlass() { IndustryCode = "2728bY", LocalCode = testCode, MaterialNumber = testCode, EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetGlass(string.Empty, string.Empty, testCode, testCode);

            Assert.AreEqual(response.Id, 3);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetGlass_ShouldReturnNeededItem_WhenLocalCodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { IndustryCode = "2233aE", LocalCode = "2233aB", MaterialNumber = "2233aB", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlass() { IndustryCode = "2353aAB", LocalCode = "2353aA", MaterialNumber = "2233aB", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlass() { IndustryCode = "2728bR", LocalCode = "2233aC", MaterialNumber = "2233aC", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlass() { IndustryCode = "2728bY", LocalCode = "2728bD", MaterialNumber = "2728bD", EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetGlass(string.Empty, string.Empty, string.Empty, testCode);

            Assert.AreEqual(response.Id, 3);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetGlass_ShouldReturnNull_WhenNoCodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { IndustryCode = "2233aE", LocalCode = "2233aB", MaterialNumber = "2233aB", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlass() { IndustryCode = "2353aAB", LocalCode = "2233aA", MaterialNumber = "2233aB", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlass() { IndustryCode = "2728bR", LocalCode = "2233aC", MaterialNumber = "2233aC", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlass() { IndustryCode = "2728bY", LocalCode = "2728bD", MaterialNumber = "2728bD", EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetGlass(string.Empty, string.Empty, string.Empty, string.Empty);

            Assert.IsNull(response);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByRandomCode_ShouldReturNeededItems()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { IndustryCode = "2353AA", LocalCode = "2233aB", MaterialNumber = "2353AA", EuroCode = "2233a", OesCode = "2233aB", Id = 1 },
                new VehicleGlass() { IndustryCode = "2353aB", LocalCode = "2233aC", MaterialNumber = "2233aB", EuroCode = "2233a", OesCode = "2353Aa", Id = 2 },
                new VehicleGlass() { IndustryCode = "2728bR", LocalCode = "2233aC", MaterialNumber = "2233aC", EuroCode = "2233a", OesCode = testCode, Id = 3 },
                new VehicleGlass() { IndustryCode = "2728bY", LocalCode = "2728bD", MaterialNumber = "2728bD", EuroCode = "2233a", OesCode = "2233aAD", Id = 4 },
                new VehicleGlass() { IndustryCode = "2728bY", LocalCode = "2728bD", MaterialNumber = "2728bD", EuroCode = "2353AA", OesCode = "2233aB", Id = 5 },
                new VehicleGlass() { IndustryCode = "2728bY", LocalCode = "2353AA", MaterialNumber = "2728bD", EuroCode = "", OesCode = "2233aB", Id = 6 },
                new VehicleGlass() { IndustryCode = "2353AA", LocalCode = "2353AA", MaterialNumber = "2353AA", EuroCode = "2353AA", OesCode = "2353AA", Id = 7 },
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetByRandomCode(testCode).ToList();

            Assert.AreEqual(6, response.Count);
            Assert.IsNull(response.Where(x => x.Id == 4).FirstOrDefault()); // does not contain the 6th element
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllUniqueCodesFromDb_ShouldReturNeededCodes()
        {
            var items = new List<VehicleGlass>()
            {
                new VehicleGlass() { IndustryCode = "2353AA", LocalCode = "2233aB", MaterialNumber = "2353AA", EuroCode = "EuroCode", OesCode = "2233aB", Id = 1 },
                new VehicleGlass() { IndustryCode = "2233aC", LocalCode = "2233aC", MaterialNumber = "MaterialNumber", EuroCode = "", OesCode = "OesCode", Id = 2 },
                new VehicleGlass() { IndustryCode = "IndustryCode", LocalCode = "2233aC", MaterialNumber = "", EuroCode = "", OesCode = "", Id = 3 },
                new VehicleGlass() { IndustryCode = "", LocalCode = "LocalCode", MaterialNumber = "", EuroCode = "", OesCode = "", Id = 3 }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlass>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new VehicleGlassesService(repositoryMock.Object);

            var response = service.GetAllUniqueCodesFromDb().ToList();

            Assert.AreEqual(4, response.Count);
            Assert.AreEqual("EuroCode", response[0]);
            Assert.AreEqual("MaterialNumber", response[1]);
            Assert.AreEqual("IndustryCode", response[2]);
            Assert.AreEqual("LocalCode", response[3]);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetCode_ShouldReturnNull_WhenNoCodeAvailable()
        {
            var glass = new VehicleGlass() { IndustryCode = "", LocalCode = "", MaterialNumber = "", EuroCode = "", OesCode = null };

            var service = new VehicleGlassesService(null);

            var response = service.GetCode(glass);

            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetCode_ShouldReturnEuroCode_WhenAvailable()
        {
            string code = "2353A";
            var glass = new VehicleGlass()
            {
                IndustryCode = "2233aE",
                LocalCode = "2233aS",
                MaterialNumber = "2233aB",
                EuroCode = code,
                OesCode = "2233aD"
            };
            var service = new VehicleGlassesService(null);

            var response = service.GetCode(glass);

            Assert.AreEqual(code, response);
        }

        [TestMethod]
        public void GetCode_ShouldReturnMaterialNumber_WhenAvailable()
        {
            string code = "2353A";
            var glass = new VehicleGlass()
            {
                IndustryCode = "2233aE",
                LocalCode = "2233aS",
                MaterialNumber = code,
                EuroCode = null,
                OesCode = "2233aD"
            };
            var service = new VehicleGlassesService(null);

            var response = service.GetCode(glass);

            Assert.AreEqual(code, response);
        }

        [TestMethod]
        public void GetCode_ShouldReturnIndustryCode_WhenAvailable()
        {
            string code = "2353A";
            var glass = new VehicleGlass()
            {
                IndustryCode = "2353A",
                LocalCode = code,
                MaterialNumber = "",
                EuroCode = null,
                OesCode = "2233aD"
            };
            var service = new VehicleGlassesService(null);

            var response = service.GetCode(glass);

            Assert.AreEqual(code, response);
        }

        [TestMethod]
        public void GetCode_ShouldReturnLocalCode_WhenAvailable()
        {
            string code = "2353A";
            var glass = new VehicleGlass()
            {
                IndustryCode = "",
                LocalCode = code,
                MaterialNumber = "",
                EuroCode = null,
                OesCode = "2353A"
            };
            var service = new VehicleGlassesService(null);

            var response = service.GetCode(glass);

            Assert.AreEqual(code, response);
        }

        [TestMethod]
        public void GetCode_ShouldReturnOesCode_WhenAvailable()
        {
            string code = "2353A";
            var glass = new VehicleGlass()
            {
                IndustryCode = null,
                LocalCode = "",
                MaterialNumber = "",
                EuroCode = null,
                OesCode = code
            };
            var service = new VehicleGlassesService(null);

            var response = service.GetCode(glass);

            Assert.AreEqual(code, response);
        }
    }
}
