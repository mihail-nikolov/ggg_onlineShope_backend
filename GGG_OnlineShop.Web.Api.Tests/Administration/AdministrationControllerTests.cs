using GGG_OnlineShop.InternalApiDB.Models.Enums;

namespace GGG_OnlineShop.Web.Api.Tests.Administration
{
    using Areas.Administration.Controllers;
    using Common;
    using Data.Services.Contracts;
    using Data.Services.JsonParseModels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Web;
    using System.Web.Http.Results;

    [TestClass]
    public class AdministrationControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();
        private readonly Mock<ILogsService> mockedLogger = new Mock<ILogsService>();
        private readonly string controllerName = "AdministrationController";

        [TestMethod]
        public void DbInfoAdd_ShouldReturnInternalServerErrorAndLogError_WhenDbInfoFillerIsNull()
        {
            mockedLogger.Setup(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "DbInfoAdd"));

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            var controller = new AdministrationController(null, mockedLogger.Object, flagService.Object);

            var result = controller.DbInfoAdd(null);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.VerifyAll();
        }

        [TestMethod]
        public void DbInfoAdd_ShouldReturnOkResult()
        {
            var dbFillerMock = new Mock<IGlassesInfoDbFiller>();
            dbFillerMock.Setup(x => x.FillInfo(It.IsAny<GlassJsonInfoModel[]>(), ""));

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            var controller = new AdministrationController(dbFillerMock.Object, null, flagService.Object);

            var result = controller.DbInfoAdd(null);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            var responseContent = ((OkNegotiatedContentResult<string>)result).Content;

            Assert.AreEqual(responseContent, GlobalConstants.DbFilledInFinishedMessage);
            dbFillerMock.VerifyAll();
        }

        [TestMethod]
        public void DbInfoAddFromFile_ShouldReturnInternalServerErrorAndLogError_WhenDbInfoFillerIsNull()
        {
            mockedLogger.Setup(x => x.LogError(It.IsAny<Exception>(), "", controllerName, "DbInfoAddFromFile"));
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://testUri.com", null), new HttpResponse(null));

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            var controller = new AdministrationController(null, mockedLogger.Object, flagService.Object);

            var result = controller.DbInfoAddFromFile();

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
            mockedLogger.VerifyAll();
        }

        [TestMethod]
        public void DbInfoAddFromFile_ShouldReturnOkResult()
        {
            string testFile = "";
            var dbFillerMock = new Mock<IGlassesInfoDbFiller>();
            dbFillerMock.Setup(x => x.FillInfo(null, testFile));

            HttpContext.Current = new HttpContext(new HttpRequest(testFile, "http://testUri.com", null), new HttpResponse(null));

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            var controller = new AdministrationController(dbFillerMock.Object, null, flagService.Object);

            var result = controller.DbInfoAddFromFile();

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            var responseContent = ((OkNegotiatedContentResult<string>)result).Content;

            Assert.AreEqual(responseContent, GlobalConstants.DbFilledInFinishedMessage);
            dbFillerMock.VerifyAll();
        }
    }
}
