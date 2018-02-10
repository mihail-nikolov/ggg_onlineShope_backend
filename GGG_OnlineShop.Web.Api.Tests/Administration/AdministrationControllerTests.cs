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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DbInfoAdd_ShouldThrowException_WhenDbInfoFillerIsNull()
        {
            var controller = new AdministrationController(null, null); // TODO

            controller.DbInfoAdd(null);
        }

        [TestMethod]
        public void DbInfoAdd_ShouldReturnOkResult()
        {
            var dbFillerMock = new Mock<IGlassesInfoDbFiller>();
            dbFillerMock.Setup(x => x.FillInfo(It.IsAny<GlassJsonInfoModel[]>(), ""));

            var controller = new AdministrationController(dbFillerMock.Object, null); // TODO

            var result = controller.DbInfoAdd(null);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            var responseContent = ((OkNegotiatedContentResult<string>)result).Content;

            Assert.AreEqual(responseContent, GlobalConstants.DbFilledInFinishedMessage);
            dbFillerMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DbInfoAddFromFile_ShouldThrowException_WhenDbInfoFillerIsNull()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://testUri.com", null), new HttpResponse(null));

            var controller = new AdministrationController(null, null); // TODO
            var result = controller.DbInfoAddFromFile();
        }

        [TestMethod]
        public void DbInfoAddFromFile_ShouldReturnOkResult()
        {
            string testFile = "";
            var dbFillerMock = new Mock<IGlassesInfoDbFiller>();
            dbFillerMock.Setup(x => x.FillInfo(null, testFile));

            HttpContext.Current = new HttpContext(new HttpRequest(testFile, "http://testUri.com", null), new HttpResponse(null));

            var controller = new AdministrationController(dbFillerMock.Object, null); // TODO

            var result = controller.DbInfoAddFromFile();

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            var responseContent = ((OkNegotiatedContentResult<string>)result).Content;

            Assert.AreEqual(responseContent, GlobalConstants.DbFilledInFinishedMessage);
            dbFillerMock.VerifyAll();
        }
    }
}
