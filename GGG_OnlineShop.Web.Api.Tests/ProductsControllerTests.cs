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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_ShouldThrowException_WhenVehicleGlassesServiceIsNull()
        {
            var controller = new ProductsController(null, null, null, null);

            var result = controller.Get(2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPriceAndQUantities_ShouldThrowException_WhenVehicleGlassesServiceIsNull()
        {
            var controller = new ProductsController(null, null, null, null);

            var result = controller.GetPriceAndQuantities(2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetProductTypes_ShouldThrowException_WhenVehiclesServiceIsNull()
        {
            var controller = new ProductsController(null, null, null, null);

            var result = controller.GetProductTypes(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindByVehicleInfo_ShouldThrowException_WhenVehiclesServiceIsNull()
        {
            var controller = new ProductsController(null, null, null, null);

            var result = controller.FindByVehicleInfo(null);
        }

        [TestMethod]
        public void Get_ShouldReturnBadRequest_WhenNoArgumentsPassed()
        {
            var controller = new ProductsController(null, null, null, null);
            var result = controller.Get(null);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;

            Assert.IsTrue(responseMessage.Contains(GlobalConstants.NeededCodesErrorMessage));
        }

        [TestMethod]
        public void Get_ShouldReturnBadRequest_WhenEurocodeLessThan4Symbols()
        {
            string testEurocode = "445";

            var controller = new ProductsController(null, null, null, null);
            var result = controller.Get(null, testEurocode);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var responseContent = ((BadRequestErrorMessageResult)result).Message;

            Assert.AreEqual(responseContent, GlobalConstants.CodeMinLengthErrorMessage);
        }

        [TestMethod]
        public void Get_ShouldReturnBadRequest_WhenOesCodeLessThan4Symbols()
        {
            string testCode = "445";

            var controller = new ProductsController(null, null, null, null);
            var result = controller.Get(null, null, testCode);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var responseContent = ((BadRequestErrorMessageResult)result).Message;

            Assert.AreEqual(responseContent, GlobalConstants.CodeMinLengthErrorMessage);
        }

        [TestMethod]
        public void Get_ShouldReturnBadRequest_WhenCodeLessThan4Symbols()
        {
            string testEurocode = "445";
            var controller = new ProductsController(null, null, null, null);

            var result = controller.Get(null, null, null, testEurocode);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var responseContent = ((BadRequestErrorMessageResult)result).Message;

            Assert.AreEqual(responseContent, GlobalConstants.CodeMinLengthErrorMessage);
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

            var controller = new ProductsController(null, glassesMock.Object, null, null);
            var result = controller.Get(null, testEurocode);

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

            var controller = new ProductsController(null, glassesMock.Object, null, null);
            var result = controller.Get(null, null, testOesCode);

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

            var controller = new ProductsController(null, glassesMock.Object, null, null);
            var result = controller.Get(null, null, null, testCode);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 2);
            Assert.AreEqual(responseContent.First().Id, 1);
            Assert.AreEqual(responseContent.Last().Id, 2);

            glassesMock.VerifyAll();
        }

        [TestMethod]
        public void GetPriceAndQUantities_ShouldReturnProductsQuantitiesAndPriceInfo_WhenIdPassed()
        {
            mapper.Execute();
            int testId = 1;
            string testCode = "2021AGGN";
            VehicleGlass testProduct = new VehicleGlass() { EuroCode = testCode, OesCode = "test" };

            var glassesMock = new Mock<IVehicleGlassesService>();
            glassesMock.Setup(v => v.GetById(testId)).Returns(testProduct);
            glassesMock.Setup(v => v.GetCode(testProduct)).Returns(testCode);

            var testUser = new User() { Bulstat = "12345" };
            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(v => v.GetByEmail(It.IsAny<string>())).Returns(() => testUser);

            string nordglassGroup = "Nordglass";
            string yesglassGroup = "Yesglass";

            List<ProductInfoResponseModel> productQuantitiesAndPriceInfo = new List<ProductInfoResponseModel>()
            {
                new ProductInfoResponseModel() { Group =  nordglassGroup},
                new ProductInfoResponseModel() { Group = yesglassGroup}
            };

            var productPriceAndQuantitiesMock = new Mock<IProductQuantitiesService>();
            productPriceAndQuantitiesMock.Setup(x => x.GetPriceAndQuantitiesByCode(testCode, testUser)).Returns(productQuantitiesAndPriceInfo);

            var controller = new ProductsController(null, glassesMock.Object, productPriceAndQuantitiesMock.Object, usersMock.Object);
            var result = controller.GetPriceAndQuantities(testId);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IEnumerable<ProductInfoResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<IEnumerable<ProductInfoResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 2);
            Assert.AreEqual(responseContent.First().Group, nordglassGroup);
            Assert.AreEqual(responseContent.Last().Group, yesglassGroup);

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
                new VehicleGlass() {ProductType = "windscreen" },
                new VehicleGlass() {ProductType = "windscreen" },
                new VehicleGlass() {ProductType = "windscreen" },
                new VehicleGlass() {ProductType = "test1" },
                new VehicleGlass() {ProductType = "test2" },
                new VehicleGlass() {ProductType = "test1" }
            }.AsQueryable();

            var vehiclesMock = new Mock<IVehiclesService>();
            vehiclesMock.Setup(x => x.GetVehicleByMakeModelAndBodyTypeIds(request.MakeId, null, null)).Returns(vehicle);
            vehiclesMock.Setup(x => x.GetApplicableGLasses(vehicle)).Returns(glasses);

            var controller = new ProductsController(vehiclesMock.Object, null, null, null);
            var result = controller.GetProductTypes(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<string>>));
            var responseContent = ((OkNegotiatedContentResult<List<string>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 3);
            Assert.AreEqual(responseContent[0], "windscreen");
            Assert.AreEqual(responseContent[1], "test1");
            Assert.AreEqual(responseContent[2], "test2");

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

            var controller = new ProductsController(vehiclesMock.Object, null, null, null);
            var result = controller.FindByVehicleInfo(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<VehicleGlassShortResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 3);
            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);

            vehiclesMock.VerifyAll();
        }
    }
}
