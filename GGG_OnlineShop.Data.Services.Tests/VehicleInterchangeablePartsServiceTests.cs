namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class VehicleInterchangeablePartsServiceTests
    {
        [TestMethod]
        public void GetByEuroCode_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { MaterialNumber = "2233a", EuroCode = "2233a", OesCode = testCode },
                new VehicleGlassInterchangeablePart() { MaterialNumber = "2244B", EuroCode = "2353Aa", Id = 1 },
                new VehicleGlassInterchangeablePart() { MaterialNumber = "2353aA", EuroCode = "2233a" },
                new VehicleGlassInterchangeablePart() { MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            VehicleGlassInterchangeablePart response = service.GetByEuroCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByMaterialNumber_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { LocalCode = testCode, NagsCode = testCode, ScanCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = testCode },
                new VehicleGlassInterchangeablePart() { LocalCode = testCode, NagsCode = testCode, ScanCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1 },
                new VehicleGlassInterchangeablePart() { LocalCode = testCode, NagsCode = testCode, ScanCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a" },
                new VehicleGlassInterchangeablePart() { LocalCode = testCode, NagsCode = testCode, ScanCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            VehicleGlassInterchangeablePart response = service.GetByMaterialNumber(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByLocalCode_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { LocalCode = "2233a", NagsCode = testCode, ScanCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = testCode },
                new VehicleGlassInterchangeablePart() { LocalCode = testCode, NagsCode = testCode, ScanCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1 },
                new VehicleGlassInterchangeablePart() { LocalCode = "2233a", NagsCode = testCode, ScanCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a" },
                new VehicleGlassInterchangeablePart() { LocalCode = "2728b", NagsCode = testCode, ScanCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            VehicleGlassInterchangeablePart response = service.GetByLocalCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByScanCode_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { ScanCode = "2233aB", NagsCode = testCode, LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = testCode },
                new VehicleGlassInterchangeablePart() { ScanCode = testCode, NagsCode = testCode, LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1 },
                new VehicleGlassInterchangeablePart() { ScanCode = "2233aC", NagsCode = testCode, LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a" },
                new VehicleGlassInterchangeablePart() { ScanCode = "2728bD", NagsCode = testCode, LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            VehicleGlassInterchangeablePart response = service.GetByScanCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByNagsCode_ShouldReturnNeededItem()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = testCode },
                new VehicleGlassInterchangeablePart() { NagsCode = testCode, ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1 },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aC", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bD", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            VehicleGlassInterchangeablePart response = service.GetByNagsCode(testCode);

            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetByOesCode_ShouldReturnNeededItems()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353аA", Id = 1, OesCode = "2353Aa" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aC", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bD", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            var response = service.GetByOesCode(testCode).ToList();

            Assert.AreEqual(response.Count, 2);
            Assert.AreEqual(response[0].Id, 1);
            Assert.AreEqual(response[1].Id, 2);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetInterchangeablePart_ShouldReturnNeededItem_WhenEurocodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2353aA", Id = 1, OesCode = "2353Aa" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aC", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bD", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            var response = service.GetInterchangeablePart(testCode, testCode, testCode, testCode, testCode);
            
            Assert.AreEqual(response.Id, 1);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetInterchangeablePart_ShouldReturnNeededItem_WhenMaterialNumberSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2233a", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2353aA", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aC", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2728b", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bD", ScanCode = testCode, LocalCode = testCode, MaterialNumber = "2728b" , EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            var response = service.GetInterchangeablePart(string.Empty, testCode, testCode, testCode, testCode);

            Assert.AreEqual(response.Id, 3);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetInterchangeablePart_ShouldReturnNeededItem_WhenScanCodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = "2233aE", LocalCode = testCode, MaterialNumber = testCode, EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aB", ScanCode = "2353aA", LocalCode = testCode, MaterialNumber = "2233aB", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aC", ScanCode = "2728bR", LocalCode = testCode, MaterialNumber = "2233aB", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bD", ScanCode = "2728bY", LocalCode = testCode, MaterialNumber = testCode, EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            var response = service.GetInterchangeablePart(string.Empty, string.Empty, testCode, testCode, testCode);

            Assert.AreEqual(response.Id, 3);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetInterchangeablePart_ShouldReturnNeededItem_WhenNagsCodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aE", ScanCode = "2233aB", LocalCode = "2233aB", MaterialNumber = "2233aB", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2353aA", ScanCode = "2233aB", LocalCode = "2233aB", MaterialNumber = "2233aB", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bR", ScanCode = "2233aC", LocalCode = "2233aC", MaterialNumber = "2233aC", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bY", ScanCode = "2728bD", LocalCode = "2728bD", MaterialNumber = "2728bD", EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            var response = service.GetInterchangeablePart(string.Empty, string.Empty, string.Empty, string.Empty, testCode);

            Assert.AreEqual(response.Id, 3);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetInterchangeablePart_ShouldReturnNeededItem_WhenLocalCodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aE", ScanCode = "2233aB", LocalCode = "2233aB", MaterialNumber = "2233aB", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2353aAB", ScanCode = "2233aB", LocalCode = "2353aA", MaterialNumber = "2233aB", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bR", ScanCode = "2233aC", LocalCode = "2233aC", MaterialNumber = "2233aC", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bY", ScanCode = "2728bD", LocalCode = "2728bD", MaterialNumber = "2728bD", EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            var response = service.GetInterchangeablePart(string.Empty, string.Empty, testCode, string.Empty, string.Empty);

            Assert.AreEqual(response.Id, 3);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetInterchangeablePart_ShouldReturnNull_WhenNoCodeSend()
        {
            string testCode = "2353AA";
            var items = new List<VehicleGlassInterchangeablePart>()
            {
                new VehicleGlassInterchangeablePart() { NagsCode = "2233aE", ScanCode = "2233aB", LocalCode = "2233aB", MaterialNumber = "2233aB", EuroCode = "2233a", OesCode = "2233aB" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2353aAB", ScanCode = "2233aB", LocalCode = "2233aA", MaterialNumber = "2233aB", EuroCode = "2233a", Id = 3, OesCode = "2353Aa" },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bR", ScanCode = "2233aC", LocalCode = "2233aC", MaterialNumber = "2233aC", EuroCode = "2233a", Id = 2, OesCode = testCode },
                new VehicleGlassInterchangeablePart() { NagsCode = "2728bY", ScanCode = "2728bD", LocalCode = "2728bD", MaterialNumber = "2728bD", EuroCode = "2233a", OesCode = "2233aB" }
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<VehicleGlassInterchangeablePart>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            VehicleInterchangeablePartsService service = new VehicleInterchangeablePartsService(repositoryMock.Object);

            var response = service.GetInterchangeablePart(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            Assert.IsNull(response);
            repositoryMock.VerifyAll();
        }
    }
}
