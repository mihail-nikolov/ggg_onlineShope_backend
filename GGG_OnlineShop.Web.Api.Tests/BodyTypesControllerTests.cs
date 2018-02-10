namespace GGG_OnlineShop.Web.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Data.Services.Contracts;
    using Controllers;
    using Models;
    using System.Web.Http.Results;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using InternalApiDB.Models;

    [TestClass]
    public class BodyTypesControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetVehicleBodyTypeByMakeAndModelIds_ShouldThrowException_WhenVehiclesServiceisNull()
        {
            var bodyTypesMock = new Mock<IVehicleBodyTypesService>();
            var controller = new BodyTypesController(bodyTypesMock.Object, null, null); // TODO

            var model = new VehicleBodyTypesRequestModel()
            {
                MakeId = 1,
                ModelId = 3
            };

            var result = controller.GetVehicleBodyTypeByMakeAndModelIds(model);
        }

        [TestMethod]
        public void GetVehicleBodyTypeByMakeAndModelIds_ShouldReturnAllBodyTypes()
        {
            mapper.Execute();

            var vehiclesMock = new Mock<IVehiclesService>();
            List<int?> bodyTypeIds = new List<int?>() { 1, 2, null };

            vehiclesMock.Setup(v => v.GetBodyTypeIdsByModelIdAndMakeId(1, 3))
                    .Returns(bodyTypeIds);

            var bodyTypesMock = new Mock<IVehicleBodyTypesService>();
            VehicleBodyType bodyType1 = new VehicleBodyType() { Id = 1, Code = "H5", Description = "Hatcback 5dr" };
            bodyTypesMock.Setup(v => v.GetById(1)).Returns(bodyType1);

            VehicleBodyType bodyType2 = new VehicleBodyType() { Id = 2, Code = "H3", Description = "Hatcback 3dr" };
            bodyTypesMock.Setup(v => v.GetById(2)).Returns(bodyType2);

            var controller = new BodyTypesController(bodyTypesMock.Object, vehiclesMock.Object, null); // TODO
            var model = new VehicleBodyTypesRequestModel()
            {
                MakeId = 1,
                ModelId = 3
            };

            var result = controller.GetVehicleBodyTypeByMakeAndModelIds(model);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IOrderedEnumerable<VehicleBodyTypeResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<IOrderedEnumerable<VehicleBodyTypeResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 2);
            Assert.AreEqual(responseContent.First().Id, 2);
            Assert.AreEqual(responseContent.Last().Id, 1);

            vehiclesMock.VerifyAll();
            bodyTypesMock.VerifyAll();
        }
    }
}
