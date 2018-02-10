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
    public class MakesControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_ShouldThrowException_WhenMakesServiceisNull()
        {
            var controller = new MakesController(null, null); // TODO
            var result = controller.Get();
        }

        [TestMethod]
        public void Get_ShouldReturnAllMakes()
        {
            mapper.Execute();

            var makesMock = new Mock<IVehicleMakesService>();
            IQueryable<VehicleMake> makes = new List<VehicleMake>()
            {
                new VehicleMake() {Id = 1, Name = "Audi" },
                new VehicleMake() {Id = 2, Name = "Ford" }
            }.AsQueryable();

            makesMock.Setup(v => v.GetAll())
                    .Returns(makes);

            var controller = new MakesController(makesMock.Object, null); // TODO

            var result = controller.Get();

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<VehicleMakeResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<VehicleMakeResponseModel>>)result).Content;

            Assert.AreEqual(responseContent.Count, 2);
            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);

            makesMock.VerifyAll();
        }
    }
}
