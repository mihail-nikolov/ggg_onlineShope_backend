using System.Security.Claims;
using System.Security.Principal;

namespace GGG_OnlineShop.Web.Api.Tests
{
    using Common;
    using Controllers;
    using Data.Services.Contracts;
    using Data.Services.ExternalDb.Models;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Results;

    [TestClass]
    public class ProductsControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();
        private readonly Mock<ILogsService> mockedLogger = new Mock<ILogsService>();
        private readonly string controllerName = "ProductsController";

        public ProductsControllerTests()
        {
            mockedLogger.Setup(x => x.LogError(It.IsAny<Exception>(), "", controllerName, It.IsAny<string>()));
        }

        [TestMethod]
        public void Get_ShouldReturnInternalServerErrorAndLogError_WhenVehicleGlassesServiceIsNull()
        {

            var controller = new ProductsController(null, null, null, null, null, mockedLogger.Object);

            var result = controller.Get("1234");

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "Get"));
        }

        [TestMethod]
        public void GetPriceAndQuantities_ShouldReturnInternalServerErrorAndLogError_WhenVehicleGlassesServiceIsNull()
        {
            var controller = new ProductsController(null, null, null, null, null, mockedLogger.Object);

            var result = controller.GetPriceAndQuantities(2);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "GetPriceAndQuantities"));
        }

        [TestMethod]
        public void GetProductTypes_ShouldReturnInternalServerErrorAndLogError_WhenVehiclesServiceIsNull()
        {
            var controller = new ProductsController(null, null, null, null, null, mockedLogger.Object);

            var result = controller.GetPositions(null);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "GetPositions"));
        }

        [TestMethod]
        public void FindByVehicleInfo_ShouldReturnInternalServerErrorAndLogError_WhenVehiclesServiceIsNull()
        {
            var controller = new ProductsController(null, null, null, null, null, mockedLogger.Object);

            var result = controller.FindByVehicleInfo(null);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "FindByVehicleInfo"));
        }

        [TestMethod]
        public void Get_ShouldReturnBadRequest_WhenNoArgumentsPassed()
        {
            var controller = new ProductsController(null, null, null, null, null, null);
            var result = controller.Get(null);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;

            Assert.IsTrue(responseMessage.Contains(GlobalConstants.NeededCodesErrorMessage));
        }

        [TestMethod]
        public void Get_ShouldReturnBadRequest_WhenEurocodeLessThan4Symbols()
        {
            string testEurocode = "445";

            var controller = new ProductsController(null, null, null, null, null, null);
            var result = controller.Get(null, testEurocode);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var responseContent = ((BadRequestErrorMessageResult)result).Message;

            Assert.AreEqual(responseContent, GlobalConstants.CodeMinLengthErrorMessage);
        }

        [TestMethod]
        public void Get_ShouldReturnBadRequest_WhenOesCodeLessThan4Symbols()
        {
            string testCode = "445";

            var controller = new ProductsController(null, null, null, null, null, null);
            var result = controller.Get(null, null, testCode);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var responseContent = ((BadRequestErrorMessageResult)result).Message;

            Assert.AreEqual(responseContent, GlobalConstants.CodeMinLengthErrorMessage);
        }

        [TestMethod]
        public void Get_ShouldReturnBadRequest_WhenCodeLessThan4Symbols()
        {
            string testEurocode = "445";
            var controller = new ProductsController(null, null, null, null, null, null);

            var result = controller.Get(null, null, testEurocode);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var responseContent = ((BadRequestErrorMessageResult)result).Message;

            Assert.AreEqual(responseContent, GlobalConstants.CodeMinLengthErrorMessage);
        }

        [TestMethod]
        public void GetDetailedInfo_ShouldReturnGlass_WhenIdPassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            string testCode = "AC22AGNBL1C";
            string testNumber = "testNumber";
            var glass = new VehicleGlass()
            {
                Id = 1,
                EuroCode = testCode,
                VehicleGlassAccessories = new List<VehicleGlassAccessory>()
                {
                    new VehicleGlassAccessory() {IndustryCode = testCode, MaterialNumber = testNumber }
                },
                VehicleGlassInterchangeableParts = new List<VehicleGlassInterchangeablePart>()
                {
                    new VehicleGlassInterchangeablePart() {EuroCode = $"{testCode};CHEVROLET ASTRO VAN 1985 93;", MaterialNumber = testNumber }
                }
            };

            glassesMock.Setup(v => v.GetById(1)).Returns(() => glass);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.GetDetailedInfo(1);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<VehicleGlassResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<VehicleGlassResponseModel>)result).Content;

            Assert.AreEqual(responseContent.Id, 1);
            Assert.AreEqual(responseContent.Accessories.ToList()[0].MaterialNumber, testNumber);
            Assert.AreEqual(responseContent.Accessories.ToList()[0].IndustryCode, testCode);
            Assert.AreEqual(responseContent.InterchangeableParts.ToList()[0].MaterialNumber, testNumber);
            glassesMock.VerifyAll();
        }

        [TestMethod]
        public void GetDetailedInfo_ShouldReturnGlassAndMapInterchangeablePartCorrectly()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            string testCode = "AC22AGNBL1C;CHEVROLET ASTRO VAN 1985 93;";
            string testNumber = "testNumber";
            var glass = new VehicleGlass()
            {
                Id = 1,
                EuroCode = testCode,
                VehicleGlassAccessories = new List<VehicleGlassAccessory>()
                {
                    new VehicleGlassAccessory() {IndustryCode = testCode, MaterialNumber = testNumber }
                },
                VehicleGlassInterchangeableParts = new List<VehicleGlassInterchangeablePart>()
                {
                    new VehicleGlassInterchangeablePart() {EuroCode = testCode, MaterialNumber = testNumber }
                }
            };

            glassesMock.Setup(v => v.GetById(1)).Returns(() => glass);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.GetDetailedInfo(1);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<VehicleGlassResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<VehicleGlassResponseModel>)result).Content;

            Assert.AreEqual(responseContent.InterchangeableParts.ToList()[0].EuroCode, testCode);
            Assert.AreEqual(responseContent.InterchangeableParts.ToList()[0].CleanEurocode, "AC22AGNBL1C");
            glassesMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnGlass_WhenEurocodePassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            var glassesList = new List<VehicleGlass>()
            {
                new VehicleGlass() {Id = 1 }, new VehicleGlass(){ Id = 2 }
            }.AsQueryable();
            string testEurocode = "4455EC";

            glassesMock.Setup(v => v.GetGlassesByEuroCode(testEurocode)).Returns(() => glassesList);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.Get(testEurocode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 2);
            Assert.AreEqual(responseContent.First().Id, 1);
            Assert.AreEqual(responseContent.Last().Id, 2);

            glassesMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnGlass_WhenOescodePassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            var glassesList = new List<VehicleGlass>()
            {
                new VehicleGlass() {Id = 1 }, new VehicleGlass(){ Id = 2 }
            }.AsQueryable();
            string testOesCode = "testOesCode";

            glassesMock.Setup(v => v.GetByOesCode(testOesCode)).Returns(() => glassesList);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.Get(null, testOesCode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 2);
            Assert.AreEqual(responseContent.First().Id, 1);
            Assert.AreEqual(responseContent.Last().Id, 2);

            glassesMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnGlass_WhenCodePassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            var glassesList = new List<VehicleGlass>()
            {
                new VehicleGlass() {Id = 1 }, new VehicleGlass(){ Id = 2 }
            }.AsQueryable();
            string testCode = "testCode";
            glassesMock.Setup(v => v.GetByRandomCode(testCode)).Returns(() => glassesList);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.Get(null, null, testCode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 2);
            Assert.AreEqual(responseContent.First().Id, 1);
            Assert.AreEqual(responseContent.Last().Id, 2);

            glassesMock.VerifyAll();
        }

        [TestMethod]
        public void GetDetailedInfo_ShouldReturnGlass_WhenEurocodePassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            var glass = new VehicleGlass() { Id = 2 };
            string testCode = "testCode";
            glassesMock.Setup(v => v.GetByEuroCode(testCode)).Returns(() => glass);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.GetDetailedInfo(null, testCode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<VehicleGlassResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<VehicleGlassResponseModel>)result).Content;

            Assert.AreEqual(responseContent.Id, 2);
            glassesMock.VerifyAll();
            glassesMock.Verify(x => x.GetByIndustryCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByMaterialNumber(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByLocalCode(It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void GetDetailedInfo_ShouldReturnGlass_WhenMaterialNumberPassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            var glass = new VehicleGlass() { Id = 2 };
            string testCode = "testCode";
            glassesMock.Setup(v => v.GetByMaterialNumber(testCode)).Returns(() => glass);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.GetDetailedInfo(null, "", testCode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<VehicleGlassResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<VehicleGlassResponseModel>)result).Content;

            Assert.AreEqual(responseContent.Id, 2);
            glassesMock.VerifyAll();
            glassesMock.Verify(x => x.GetByIndustryCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByEuroCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByLocalCode(It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void GetDetailedInfo_ShouldReturnGlass_WhenLocalCodePassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            var glass = new VehicleGlass() { Id = 2 };
            string testCode = "testCode";

            glassesMock.Setup(v => v.GetByLocalCode(testCode)).Returns(() => glass);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.GetDetailedInfo(null, null, null, null, testCode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<VehicleGlassResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<VehicleGlassResponseModel>)result).Content;

            Assert.AreEqual(responseContent.Id, 2);
            glassesMock.VerifyAll();
            glassesMock.Verify(x => x.GetByIndustryCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByEuroCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByMaterialNumber(It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void GetDetailedInfo_ShouldReturnGlass_WhenIndustrycodePassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            var glass = new VehicleGlass() { Id = 2 };
            string testCode = "testCode";

            glassesMock.Setup(v => v.GetByIndustryCode(testCode)).Returns(() => glass);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.GetDetailedInfo(null, null, null, testCode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<VehicleGlassResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<VehicleGlassResponseModel>)result).Content;

            Assert.AreEqual(responseContent.Id, 2);
            glassesMock.VerifyAll();
            glassesMock.Verify(x => x.GetByLocalCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByEuroCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByMaterialNumber(It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void GetDetailedInfo_ShouldReturnGlassByLocalCode_WhenByEuroCodeNotFoundAndLocalCodePassed()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            string testCode = "testCode";
            string testLocalCode = "testLocalCode";
            var glass = new VehicleGlass() { Id = 2, LocalCode = testLocalCode };

            glassesMock.Setup(v => v.GetByEuroCode(testCode)).Returns(() => null);
            glassesMock.Setup(v => v.GetByLocalCode(testLocalCode)).Returns(() => glass);

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.GetDetailedInfo(null, testCode, null, null, testLocalCode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<VehicleGlassResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<VehicleGlassResponseModel>)result).Content;

            Assert.AreEqual(responseContent.Id, 2);
            Assert.AreEqual(responseContent.LocalCode, testLocalCode);
            glassesMock.VerifyAll();
            glassesMock.Verify(x => x.GetByIndustryCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByMaterialNumber(It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void GetDetailedInfo_ShouldReturnBadRequest_WhenNoCodeSend()
        {
            mapper.Execute();

            var glassesMock = new Mock<IVehicleGlassesService>();

            var controller = new ProductsController(null, null, glassesMock.Object, null, null, null);
            var result = controller.GetDetailedInfo(null);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var responseContent = ((BadRequestErrorMessageResult)result).Message;

            Assert.AreEqual(responseContent, "No code passed");
            glassesMock.VerifyAll();
            glassesMock.Verify(x => x.GetByLocalCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByIndustryCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByEuroCode(It.IsAny<string>()), Times.Exactly(0));
            glassesMock.Verify(x => x.GetByMaterialNumber(It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void GetPriceAndQuantities_ShouldReturnProductsQuantitiesAndPriceInfo_WhenIdPassed()
        {
            mapper.Execute();
            int testId = 1;
            string testCode = "2021AGGN";
            string testUserId = "userId";
            VehicleGlass testProduct = new VehicleGlass() { EuroCode = testCode, OesCode = "test" };

            var glassesMock = new Mock<IVehicleGlassesService>();
            glassesMock.Setup(v => v.GetById(testId)).Returns(testProduct);
            glassesMock.Setup(v => v.GetCode(testProduct)).Returns(testCode);

            var testUser = new User() { Bulstat = "12345", Id = testUserId };
            var usersMock = new Mock<IUsersService>();
            // TODO doubleckeck later
            //usersMock.Setup(v => v.GetByEmail(It.IsAny<string>())).Returns(() => testUser);

            string nordglassGroup = "Nordglass";
            string yesglassGroup = "Yesglass";

            List<ProductInfoResponseModel> productQuantitiesAndPriceInfo = new List<ProductInfoResponseModel>()
            {
                new ProductInfoResponseModel() { Group = nordglassGroup},
                new ProductInfoResponseModel() { Group = yesglassGroup}
            };

            // moq the user
            var claim = new Claim("test", testUserId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

            // moq identity and role
            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity);

            var productPriceAndQuantitiesMock = new Mock<IProductQuantitiesService>();
            productPriceAndQuantitiesMock.Setup(x => x.GetPriceAndQuantitiesByCode(testCode, It.IsAny<User>())).Returns(productQuantitiesAndPriceInfo);

            var controller = new ProductsController(null, null, glassesMock.Object, productPriceAndQuantitiesMock.Object, usersMock.Object, null)
            {
                User = mockPrincipal.Object
            };
            var result = controller.GetPriceAndQuantities(testId);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IEnumerable<ProductInfoResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<IEnumerable<ProductInfoResponseModel>>)result).Content;

            Assert.AreEqual(2, responseContent.ToList().Count);
            Assert.AreEqual(nordglassGroup, responseContent.First().Group);
            Assert.AreEqual(yesglassGroup, responseContent.Last().Group);

            glassesMock.VerifyAll();
            productPriceAndQuantitiesMock.VerifyAll();
            usersMock.VerifyAll();
        }

        [TestMethod]
        public void GetProductTypes_ShouldReturnProductsWithouRepeatables()
        {
            mapper.Execute();

            VehicleGlassRequestModel request = new VehicleGlassRequestModel() { MakeId = 1 };
            Vehicle vehicle = new Vehicle() { MakeId = 1 };
            var glasses = new List<VehicleGlass>()
            {
                new VehicleGlass() {Position = "WS" },
                new VehicleGlass() {Position = "WS" },
                new VehicleGlass() {Position = "WS" },
                new VehicleGlass() {Position = "BL" },
                new VehicleGlass() {Position = "BL" },
                new VehicleGlass() {Position = "LQF" }
            }.AsQueryable();

            var vehiclesMock = new Mock<IVehiclesService>();
            vehiclesMock.Setup(x => x.GetVehicleByMakeModelAndBodyTypeIds(request.MakeId, null, null)).Returns(vehicle);
            vehiclesMock.Setup(x => x.GetApplicableGLasses(vehicle)).Returns(glasses);

            var controller = new ProductsController(null, vehiclesMock.Object, null, null, null, null);
            var result = controller.GetPositions(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<string>>));
            var responseContent = ((OkNegotiatedContentResult<List<string>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 3);
            Assert.AreEqual(responseContent[0], "WS");
            Assert.AreEqual(responseContent[1], "BL");
            Assert.AreEqual(responseContent[2], "LQF");

            vehiclesMock.VerifyAll();
        }

        [TestMethod]
        public void FindByVehicleInfo_ShouldReturnCorrectProducts()
        {
            mapper.Execute();

            string testProductType = "windscreen";
            VehicleGlassRequestModel request = new VehicleGlassRequestModel() { MakeId = 1, ProductType = testProductType };
            Vehicle vehicle = new Vehicle() { MakeId = 1 };
            var glasses = new List<VehicleGlass>()
            {
                new VehicleGlass() {Id = 1 },
                new VehicleGlass() {Id = 2 },
                new VehicleGlass() {Id = 3 }
            }.AsQueryable();

            var vehiclesMock = new Mock<IVehiclesService>();
            vehiclesMock.Setup(x => x.GetVehicleByMakeModelAndBodyTypeIds(request.MakeId, null, null)).Returns(vehicle);
            vehiclesMock.Setup(x => x.GetApplicableGLassesByProductType(vehicle, testProductType)).Returns(glasses);

            var controller = new ProductsController(null, vehiclesMock.Object, null, null, null, null);
            var result = controller.FindByVehicleInfo(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 3);
            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);

            vehiclesMock.VerifyAll();
            vehiclesMock.Verify(x => x.GetApplicableGLasses(It.IsAny<Vehicle>()), Times.Never);
        }

        public void FindByVehicleInfo_ShouldReturnCorrectProducts_WhenProductTypeEmpty()
        {
            mapper.Execute();

            VehicleGlassRequestModel request = new VehicleGlassRequestModel() { MakeId = 1 };
            Vehicle vehicle = new Vehicle() { MakeId = 1 };
            var glasses = new List<VehicleGlass>()
            {
                new VehicleGlass() {Id = 1 },
                new VehicleGlass() {Id = 2 },
                new VehicleGlass() {Id = 3 }
            }.AsQueryable();

            var vehiclesMock = new Mock<IVehiclesService>();
            vehiclesMock.Setup(x => x.GetVehicleByMakeModelAndBodyTypeIds(request.MakeId, null, null)).Returns(vehicle);
            vehiclesMock.Setup(x => x.GetApplicableGLasses(vehicle)).Returns(glasses);

            var controller = new ProductsController(null, vehiclesMock.Object, null, null, null, null);
            var result = controller.FindByVehicleInfo(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 3);
            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);

            vehiclesMock.VerifyAll();
            vehiclesMock.Verify(x => x.GetApplicableGLassesByProductType(It.IsAny<Vehicle>(), It.IsAny<string>()), Times.Never);
        }
    }
}
