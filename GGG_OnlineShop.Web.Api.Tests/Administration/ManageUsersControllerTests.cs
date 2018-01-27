namespace GGG_OnlineShop.Web.Api.Tests.Administration
{
    using Areas.Administration.Controllers;
    using Areas.Administration.Models.Users;
    using Common;
    using Data.Services.Contracts;
    using InternalApiDB.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using System.Web.Http.Routing;

    [TestClass]
    public class ManageUsersControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_ShouldThrowException_WhenUsersServiceIsNull()
        {
            var controller = new ManageUsersController(null, null);

            controller.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateUserInfo_ShouldThrowException_WhenUsersServiceIsNull()
        {
            var controller = new ManageUsersController(null, null);

            controller.UpdateUserInfo(null);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void SendEmailConfirmation_ShouldThrowException_WhenUsersServiceIsNull()
        {
            var controller = new ManageUsersController(null, null);

            controller.SendEmailConfirmation(null).Wait();
        }

        [TestMethod]
        public void UpdateUserInfo_ShouldReturnBadRequest_WhenIsValidUserIsFalse()
        {
            mapper.Execute();

            var users = new Mock<IUsersService>();
            users.Setup(x => x.IsValidUser(It.IsAny<User>())).Returns(() => false);

            var controller = new ManageUsersController(users.Object, null);

            var result = controller.UpdateUserInfo(null);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            Assert.IsTrue(responseMessage.Contains(GlobalConstants.InvalidCompanyBulstatCombination));
            users.VerifyAll();
        }

        [TestMethod]
        public void UpdateUserInfo_ShouldReturnOkResult_WhenCreateAsyncSucceded()
        {
            mapper.Execute();
            string testId = "testId";

            var users = new Mock<IUsersService>();
            users.Setup(x => x.IsValidUser(It.IsAny<User>())).Returns(() => true);
            users.Setup(x => x.Update(It.IsAny<User>())).Returns(() => new User() { Id = testId, IsPilkingtonVisible = true });

            UserUpdateModel request = new UserUpdateModel() { Id = testId, IsPilkingtonVisible = true };
            var controller = new ManageUsersController(users.Object, null);

            var result = controller.UpdateUserInfo(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<UserResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<UserResponseModel>)result).Content;
            Assert.AreEqual(responseContent.Id, testId);
            Assert.AreEqual(responseContent.IsPilkingtonVisible, true);
            users.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnUsersInCorrectSequence()
        {
            mapper.Execute();
            string testUserId1 = "testId1";
            string testUserId2 = "testId2";
            string testUserId3 = "testId3";
            string testUserId4 = "testId4";
            string testUserId5 = "testId5";

            var users = new List<User>()
            {
                new User() {Id = testUserId1, CreatedOn = DateTime.Now },
                new User() {Id = testUserId2, CreatedOn = DateTime.MinValue },
                new User() {Id = testUserId3, CreatedOn = DateTime.MinValue },
                new User() {Id = testUserId4, CreatedOn = DateTime.MinValue },
                new User() {Id = testUserId5, CreatedOn = DateTime.MinValue },
            }.AsQueryable();
            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(x => x.GetAll()).Returns(users);

            var controller = new ManageUsersController(usersMock.Object, null);

            var result = controller.Get();

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<UserResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<UserResponseModel>>)result).Content;
            Assert.AreEqual(responseContent[0].Id, testUserId1);
            Assert.AreEqual(responseContent[1].Id, testUserId2);
            Assert.AreEqual(responseContent[2].Id, testUserId3);
            Assert.AreEqual(responseContent[3].Id, testUserId4);
            Assert.AreEqual(responseContent[4].Id, testUserId5);
            usersMock.VerifyAll();
        }

        [TestMethod]
        public async Task SendEmailConfirmation_ShouldReturnOkResult()
        {
            mapper.Execute();
            string testEmail = "testEmail";
            string testId = "testId";
            string testCode = "testCode";
            string testRoute = "/testRoute";
            string fullCallbackUrl = $"{GlobalConstants.AppDomainPath}/confirmemail?userid={testId}&code={testCode}";

            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(x => x.GetByEmail(testEmail)).Returns(() => new User() { Id = testId, Email = testEmail });

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(testId)).ReturnsAsync(testCode);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(testEmail, GlobalConstants.ConfirmEmailSubject,
                                              string.Format(GlobalConstants.ConfirmEmailBody, fullCallbackUrl), GlobalConstants.SMTPServer,
                                              GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword));

            var urlMock = new Mock<UrlHelper>();
            urlMock.Setup(m => m.Route(It.IsAny<string>(), It.IsAny<object>())).Returns(testRoute);

            AccountEmailRequestModel request = new AccountEmailRequestModel() { Email = testEmail };
            var controller = new ManageUsersController(usersMock.Object, emailsMock.Object, userManagerMock.Object)
            { Url = urlMock.Object };

            var result = await controller.SendEmailConfirmation(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            var responseContent = ((OkNegotiatedContentResult<string>)result).Content;
            Assert.AreEqual(fullCallbackUrl, responseContent);
            usersMock.VerifyAll();
            userManagerMock.VerifyAll();
            emailsMock.VerifyAll();
        }

        // TODO - probably uncomment and adapt when implement remove
        //[TestMethod]
        //public async Task RemoveUser_ShouldReturnOkResult()
        //{
        //    mapper.Execute();
        //    string testEmail = "testEmail";
        //    string testUserId = "testUserId";

        //    // moq the user
        //    var claim = new Claim("test", testUserId);
        //    var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

        //    var userStore = new Mock<IUserStore<User>>();
        //    var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
        //    userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
        //                                .ReturnsAsync(() =>
        //                                    new User()
        //                                    {
        //                                        Email = testEmail,
        //                                        Id = testUserId,
        //                                        OrderedItems = new List<OrderedItem>() { new OrderedItem() { Id = 1 } }
        //                                    });
        //    userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>()))
        //                              .Returns(Task.FromResult(IdentityResult.Success));

        //    var usersMock = new Mock<IUsersService>();
        //    usersMock.Setup(x => x.CleanUserInfoFromOrders(It.IsAny<User>()));

        //    AccountController controller = new AccountController(usersMock.Object, null, null, userManagerMock.Object, null)
        //    {
        //        User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
        //    };

        //    var result = await controller.RemoveUser();

        //    Assert.IsInstanceOfType(result, typeof(OkResult));

        //    userManagerMock.VerifyAll();
        //    usersMock.Verify(m => m.CleanUserInfoFromOrders(It.Is<User>(x => x.Id == testUserId)));
        //}

        //[TestMethod]
        //public async Task RemoveUser_ShouldReturnBadRequest_WhenDeleteAsyncNotSuccess()
        //{
        //    mapper.Execute();
        //    string testEmail = "testEmail";
        //    string testUserId = "testUserId";
        //    string testError1 = "testError1";
        //    string testError2 = "testError2";

        //    // moq the user
        //    var claim = new Claim("test", testUserId);
        //    var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

        //    string[] errors = new string[] { testError1, testError2 };

        //    var userStore = new Mock<IUserStore<User>>();
        //    var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
        //    userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
        //                                .ReturnsAsync(() =>
        //                                    new User()
        //                                    {
        //                                        Email = testEmail,
        //                                        Id = testUserId,
        //                                        OrderedItems = new List<OrderedItem>() { new OrderedItem() { Id = 1 } }
        //                                    });
        //    userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>()))
        //                              .ReturnsAsync(() => new IdentityResult(errors));

        //    var usersMock = new Mock<IUsersService>();
        //    usersMock.Setup(x => x.CleanUserInfoFromOrders(It.IsAny<User>()));

        //    AccountController controller = new AccountController(usersMock.Object, null, null, userManagerMock.Object, null)
        //    {
        //        User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
        //    };

        //    var result = await controller.RemoveUser();

        //    Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        //    string responseMessage = ((BadRequestErrorMessageResult)result).Message;
        //    Assert.IsTrue(responseMessage.Contains(testError1));
        //    Assert.IsTrue(responseMessage.Contains(testError2));

        //    userManagerMock.VerifyAll();
        //    usersMock.Verify(m => m.CleanUserInfoFromOrders(It.Is<User>(x => x.Id == testUserId)));
        //}
    }
}
