﻿namespace GGG_OnlineShop.Web.Api.Tests
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
    public class ModelsControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();
        private readonly Mock<ILogsService> mockedLogger = new Mock<ILogsService>();
        private readonly string controllerName = "ModelsController";

        public ModelsControllerTests()
        {
            mockedLogger.Setup(x => x.LogError(It.IsAny<Exception>(), "", controllerName, It.IsAny<string>()));
        }

        [TestMethod]
        public void GetVehicleModelByMake_ShouldReturnInternalServerErrorAndLogError_WhenVehiclesServiceisNull()
        {
            var modelsMock = new Mock<IVehicleModelsService>();
            var controller = new ModelsController(modelsMock.Object, null, mockedLogger.Object);

            var result = controller.GetVehicleModelByMake(1);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "GetVehicleModelByMake"));
        }

        [TestMethod]
        public void GetVehicleModelByMake_ShouldReturnAllModels()
        {
            mapper.Execute();

            var vehiclesMock = new Mock<IVehiclesService>();
            List<int?> modelIds = new List<int?>() { 1, 2, null };

            vehiclesMock.Setup(v => v.GetModelIdsByMakeId(1))
                    .Returns(modelIds);

            var modelsMock = new Mock<IVehicleModelsService>();
            VehicleModel model1 = new VehicleModel() { Id = 1, Name = "A5"};
            modelsMock.Setup(v => v.GetById(1)).Returns(model1);

            VehicleModel model2 = new VehicleModel() { Id = 2, Name = "A4" };
            modelsMock.Setup(v => v.GetById(2)).Returns(model2);

            var controller = new ModelsController(modelsMock.Object, vehiclesMock.Object, null);

            var result = controller.GetVehicleModelByMake(1);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IOrderedEnumerable<VehicleModelResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<IOrderedEnumerable<VehicleModelResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.ToList().Count, 2);
            Assert.AreEqual(responseContent.First().Id, 2);
            Assert.AreEqual(responseContent.Last().Id, 1);

            vehiclesMock.VerifyAll();
            modelsMock.VerifyAll();
        }
    }
}
