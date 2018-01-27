namespace GGG_OnlineShop.Web.Api.Tests
{
    using Common;
    using Controllers;
    using Data.Services.Contracts;
    using InternalApiDB.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Http.Results;

    [TestClass]
    public class AccountControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void Register_ShouldThrowException_WhenAutoMapperNotInitialized()
        {
            var controller = new AccountController(null, null, null);

            controller.Register(null).Wait();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_ShouldThrowException_WhenAUsersNull()
        {
            var controller = new AccountController(null, null, null);

            controller.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateUserInfo_ShouldThrowException_WhenAutoMapperNotInitialized()
        {
            var controller = new AccountController(null, null, null);

            controller.UpdateUserInfo(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMyOrders_ShouldThrowException_WhenOrdersNotInitialized()
        {
            var controller = new AccountController(null, null, null);

            controller.GetMyOrders();
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void ForgotPassword_ShouldThrowException_WhenEmailsNotInitialized()
        {
            var controller = new AccountController(null, null, null);

            controller.ForgotPassword(null).Wait();
        }

        [TestMethod]
        public async Task Register_ShouldReturnBadRequest_whenInvalidUser()
        {
            mapper.Execute();

            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(x => x.IsValidUser(It.IsAny<User>())).Returns(false);

            AccountRegisterBindingModel request = new AccountRegisterBindingModel()
            {
                IsCompany = false,
                Bulstat = "1234"
            };

            var controller = new AccountController(usersMock.Object, null, null);

            var result = await controller.Register(request);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            Assert.IsTrue(responseMessage.Contains(GlobalConstants.InvalidCompanyBulstatCombination));

            usersMock.VerifyAll();
            usersMock.Verify(m => m.IsValidUser(It.Is<User>(x => x.Bulstat == "1234")));
        }

        [TestMethod]
        public async Task Register_ShouldReturnErrorResult_WhenCreateAsyncNotSucceded()
        {
            mapper.Execute();
            string testPassword = "1111";
            string testBulstat = "1234";

            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(x => x.IsValidUser(It.IsAny<User>())).Returns(true);

            string[] errors = new string[] { "testError1", "testError2" };

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), testPassword))
                                        .ReturnsAsync(() => new IdentityResult(errors));

            AccountRegisterBindingModel request = new AccountRegisterBindingModel()
            {
                IsCompany = false,
                Bulstat = testBulstat,
                Password = testPassword
            };

            var controller = new AccountController(usersMock.Object, null, null, userManagerMock.Object, null);

            var result = await controller.Register(request);

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            userManagerMock.VerifyAll();
            usersMock.VerifyAll();
            usersMock.Verify(m => m.IsValidUser(It.Is<User>(x => x.Bulstat == testBulstat)));
        }

        [TestMethod]
        public async Task Register_ShouldReturnOkResult_WhenUpdateContactInfoSucceded()
        {
            mapper.Execute();
            string testPassword = "1111";
            string testBulstat = "1234";

            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(x => x.IsValidUser(It.IsAny<User>())).Returns(true);

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), testPassword))
                                        .Returns(Task.FromResult(IdentityResult.Success));

            AccountRegisterBindingModel request = new AccountRegisterBindingModel()
            {
                IsCompany = false,
                Bulstat = testBulstat,
                Password = testPassword
            };

            var controller = new AccountController(usersMock.Object, null, null, userManagerMock.Object, null);

            var result = await controller.Register(request);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            userManagerMock.VerifyAll();
            usersMock.VerifyAll();
            usersMock.Verify(m => m.IsValidUser(It.Is<User>(x => x.Bulstat == testBulstat)));
        }

        [TestMethod]
        public void UpdateUserInfo_ShouldReturnOkResult_WhenUpdateContactInfoSucceded()
        {
            mapper.Execute();
            string deliveryCountry = "BG";
            string testId = "testId";

            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(x => x.UpdateContactInfo(It.IsAny<User>())).Returns(() => new User() { DeliveryCountry = deliveryCountry });

            // moq the user
            var claim = new Claim("test", testId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

            AccountInfoUpdateModel request = new AccountInfoUpdateModel()
            {
                DeliveryCountry = deliveryCountry
            };

            var controller = new AccountController(usersMock.Object, null, null)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var result = controller.UpdateUserInfo(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountInfoResponseModel>));
            var responseContent = ((OkNegotiatedContentResult<AccountInfoResponseModel>)result).Content;

            Assert.AreEqual(responseContent.DeliveryCountry, deliveryCountry);
            usersMock.VerifyAll();
            usersMock.Verify(m => m.UpdateContactInfo(It.Is<User>(x => x.DeliveryCountry == deliveryCountry)));
            usersMock.Verify(m => m.UpdateContactInfo(It.Is<User>(x => x.Id == testId)));
        }

        [TestMethod]
        public async Task ChangePassword_ShouldReturnOkResult_WhenChangePasswordAsyncSucceded()
        {
            mapper.Execute();
            string oldPassword = "oldTest";
            string newPassword = "newTest";
            string testUserId = "testId";

            // moq the user
            var claim = new Claim("test", testUserId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.ChangePasswordAsync(testUserId, oldPassword, newPassword))
                                        .Returns(Task.FromResult(IdentityResult.Success));

            ChangePasswordBindingModel request = new ChangePasswordBindingModel()
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            var controller = new AccountController(null, null, null, userManagerMock.Object, null)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var result = await controller.ChangePassword(request);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public async Task ChangePassword_ShouldReturnErrorResult_WhenChangePasswordAsyncNotSucceded()
        {
            mapper.Execute();
            string oldPassword = "oldTest";
            string newPassword = "newTest";
            string testUserId = "testId";

            // moq the user
            var claim = new Claim("test", testUserId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

            string[] errors = new string[] { "testError1", "testError2" };

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.ChangePasswordAsync(testUserId, oldPassword, newPassword))
                                        .ReturnsAsync(() => new IdentityResult(errors));

            ChangePasswordBindingModel request = new ChangePasswordBindingModel()
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            var controller = new AccountController(null, null, null, userManagerMock.Object, null)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var result = await controller.ChangePassword(request);

            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public void GetMyOrders_ShouldReturnOrdersInCorrectSequence()
        {
            mapper.Execute();
            string testUserId = "testId";

            // moq the user
            var claim = new Claim("test", testUserId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

            var orders = new List<OrderedItem>()
            {
                new OrderedItem() {Id = 3, Status = DeliveryStatus.Done, CreatedOn = DateTime.Now },
                new OrderedItem() {Id = 4, Status = DeliveryStatus.Done, CreatedOn = DateTime.MinValue },
                new OrderedItem() {Id = 5, Status = DeliveryStatus.Done, CreatedOn = DateTime.MinValue },
                new OrderedItem() {Id = 2, Status = DeliveryStatus.Ordered, CreatedOn = DateTime.MinValue },
                new OrderedItem() {Id = 1, Status = DeliveryStatus.New, CreatedOn = DateTime.MinValue },
            }.AsQueryable();
            var ordersMock = new Mock<IOrderedItemsService>();
            ordersMock.Setup(x => x.GetAllByUser(testUserId)).Returns(orders);

            var controller = new AccountController(null, ordersMock.Object, null)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var result = controller.GetMyOrders();

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<OrderedItemResponseModel>>));
            var responseContent = ((OkNegotiatedContentResult<List<OrderedItemResponseModel>>)result).Content;
            Assert.AreEqual(responseContent[0].Id, 1);
            Assert.AreEqual(responseContent[1].Id, 2);
            Assert.AreEqual(responseContent[2].Id, 3);
            Assert.AreEqual(responseContent[3].Id, 5);
            Assert.AreEqual(responseContent[4].Id, 4);
            ordersMock.VerifyAll();
        }

        [TestMethod]
        public async Task ForgotPassword_ShouldReturnBadRequest_WhenUserIsNull()
        {
            mapper.Execute();
            string testEmail = "testEmail";

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.FindByNameAsync(testEmail))
                                        .ReturnsAsync(() => null);

            AccountEmailRequestModel request = new AccountEmailRequestModel()
            {
                Email = testEmail
            };

            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ForgotPassword(request);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            Assert.IsTrue(responseMessage.Contains(GlobalConstants.FindingUserError));

            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public async Task ForgotPassword_ShouldReturnBadRequest_WhenEmailNotConfirmed()
        {
            mapper.Execute();
            string testEmail = "testEmail";
            string testId = "testId";

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.FindByNameAsync(testEmail))
                                        .ReturnsAsync(() => new User() { Email = testEmail, EmailConfirmed = false, Id = testId });

            AccountEmailRequestModel request = new AccountEmailRequestModel()
            {
                Email = testEmail
            };
            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ForgotPassword(request);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            Assert.IsTrue(responseMessage.Contains(GlobalConstants.FindingUserError));

            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public async Task ForgotPassword_ShouldReturnOkResult()
        {
            mapper.Execute();
            string testEmail = "testEmail";
            string testId = "testId";
            string testCode = "testCode";

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.FindByNameAsync(testEmail))
                                        .ReturnsAsync(() => new User() { Email = testEmail, EmailConfirmed = true, Id = testId });
            userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(testId))
                                      .ReturnsAsync(() => testCode);

            var emailsMock = new Mock<IEmailsService>();
            emailsMock.Setup(x => x.SendEmail(testEmail, GlobalConstants.ResetPasswordSubject,
                                              string.Format(GlobalConstants.ResetPasswordBody, testCode), GlobalConstants.SMTPServer,
                                              GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword));

            AccountEmailRequestModel request = new AccountEmailRequestModel()
            {
                Email = testEmail
            };
            var controller = new AccountController(null, null, emailsMock.Object, userManagerMock.Object, null);

            var result = await controller.ForgotPassword(request);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            var responseContent = ((OkNegotiatedContentResult<string>)result).Content;

            Assert.AreEqual(responseContent, testCode);

            userManagerMock.VerifyAll();
            emailsMock.VerifyAll();
        }

        [TestMethod]
        public async Task ResetPassword_ShouldReturnOkResult()
        {
            mapper.Execute();
            string testEmail = "testEmail";
            string testId = "testId";
            string testCode = "testCode";
            string testPassword = "testPassword";

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.FindByNameAsync(testEmail))
                                        .ReturnsAsync(() => new User() { Email = testEmail, EmailConfirmed = false, Id = testId });
            userManagerMock.Setup(x => x.ResetPasswordAsync(testId, testCode, testPassword))
                                       .Returns(Task.FromResult(IdentityResult.Success));

            AccountResetPasswordRequestModel request = new AccountResetPasswordRequestModel()
            {
                Email = testEmail,
                Code = testCode,
                Password = testPassword
            };
            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ResetPassword(request);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public async Task ResetPassword_ShouldReturnBadRequest_WhenResetPasswordAsyncFails()
        {
            mapper.Execute();
            string testEmail = "testEmail";
            string testId = "testId";
            string testCode = "testCode";
            string testPassword = "testPassword";

            string[] errors = new string[] { "testError1", "testError2" };

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.FindByNameAsync(testEmail))
                                        .ReturnsAsync(() => new User() { Email = testEmail, EmailConfirmed = false, Id = testId });
            userManagerMock.Setup(x => x.ResetPasswordAsync(testId, testCode, testPassword))
                                       .ReturnsAsync(() => new IdentityResult(errors));

            AccountResetPasswordRequestModel request = new AccountResetPasswordRequestModel()
            {
                Email = testEmail,
                Code = testCode,
                Password = testPassword
            };
            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ResetPassword(request);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public async Task ResetPassword_ShouldReturnBadRequest_WhenUserIsNull()
        {
            mapper.Execute();
            string testEmail = "testEmail";

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.FindByNameAsync(testEmail))
                                        .ReturnsAsync(() => null);

            AccountResetPasswordRequestModel request = new AccountResetPasswordRequestModel()
            {
                Email = testEmail
            };
            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ResetPassword(request);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            Assert.IsTrue(responseMessage.Contains(GlobalConstants.NoSuchAUserErroMessage));

            userManagerMock.VerifyAll();
        }


        [TestMethod]
        public async Task ConfirmEmail_ShouldReturnOkResult()
        {
            mapper.Execute();
            string testId = "testId";
            string testCode = "testCode";

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.ConfirmEmailAsync(testId, testCode))
                                       .Returns(Task.FromResult(IdentityResult.Success));

            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ConfirmEmail(testId, testCode);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public async Task ConfirmEmail_ShouldReturnBadRequest_WhenConfirmEmailAsyncFails()
        {
            mapper.Execute();
            string testId = "testId";
            string testCode = "testCode";

            string[] errors = new string[] { "testError1", "testError2" };

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.ConfirmEmailAsync(testId, testCode))
                                      .ReturnsAsync(() => new IdentityResult(errors));

            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ConfirmEmail(testId, testCode);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            Assert.IsTrue(responseMessage.Contains(GlobalConstants.EmailConfirmationFailedErrorMessage));

            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public async Task ConfirmEmail_ShouldReturnBadRequest_WhenUserIdIsNull()
        {
            mapper.Execute();
            string testCode = "testCode";

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);

            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ConfirmEmail(null, testCode);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            string expected = string.Format(GlobalConstants.WrongCodeErrorMessage, "");
            Assert.IsTrue(responseMessage.Contains(expected));

            userManagerMock.VerifyAll();
        }

        [TestMethod]
        public async Task ConfirmEmail_ShouldReturnBadRequest_WhenCodeIsNull()
        {
            mapper.Execute();
            string testId = "testId";

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);

            var controller = new AccountController(null, null, null, userManagerMock.Object, null);

            var result = await controller.ConfirmEmail(testId, null);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            string expected = string.Format(GlobalConstants.WrongCodeErrorMessage, testId);
            Assert.IsTrue(responseMessage.Contains(expected));

            userManagerMock.VerifyAll();
        }

        // TODO
        //[TestMethod]
        //public void RemoveUser_ShouldReturnBadRequest_WhenAdmin()
        //{
        //    mapper.Execute();
        //    string testUserId = "testUserId";

        //    // moq the user
        //    var claim = new Claim("test", testUserId);
        //    var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

        //    var controller = new AccountController(null, null, null)
        //    {
        //        User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
        //    };

        //    var result = controller.RemoveUser();

        //    Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

        //    string responseMessage = ((BadRequestErrorMessageResult)result).Message;
        //    Assert.IsTrue(responseMessage.Contains(GlobalConstants.CannotRemoveAdminErrorMessage));
        //}

        [TestMethod]
        public async Task RemoveUser_ShouldReturnOkResult()
        {
            mapper.Execute();
            string testEmail = "testEmail";
            string testUserId = "testUserId";

            // moq the user
            var claim = new Claim("test", testUserId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                                        .ReturnsAsync(() =>
                                            new User()
                                            {
                                                Email = testEmail,
                                                Id = testUserId,
                                                OrderedItems = new List<OrderedItem>() { new OrderedItem() { Id = 1 } }
                                            });
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>()))
                                      .Returns(Task.FromResult(IdentityResult.Success));

            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(x => x.CleanUserInfoFromOrders(It.IsAny<User>()));

            AccountController controller = new AccountController(usersMock.Object, null, null, userManagerMock.Object, null)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var result = await controller.RemoveUser();

            Assert.IsInstanceOfType(result, typeof(OkResult));

            userManagerMock.VerifyAll();
            usersMock.Verify(m => m.CleanUserInfoFromOrders(It.Is<User>(x => x.Id == testUserId)));
        }

        [TestMethod]
        public async Task RemoveUser_ShouldReturnBadRequest_WhenDeleteAsyncNotSuccess()
        {
            mapper.Execute();
            string testEmail = "testEmail";
            string testUserId = "testUserId";
            string testError1 = "testError1";
            string testError2 = "testError2";

            // moq the user
            var claim = new Claim("test", testUserId);
            var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

            string[] errors = new string[] { testError1, testError2 };

            var userStore = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<ApplicationUserManager>(userStore.Object);
            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                                        .ReturnsAsync(() =>
                                            new User()
                                            {
                                                Email = testEmail,
                                                Id = testUserId,
                                                OrderedItems = new List<OrderedItem>() { new OrderedItem() { Id = 1 } }
                                            });
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>()))
                                      .ReturnsAsync(() => new IdentityResult(errors));

            var usersMock = new Mock<IUsersService>();
            usersMock.Setup(x => x.CleanUserInfoFromOrders(It.IsAny<User>()));

            AccountController controller = new AccountController(usersMock.Object, null, null, userManagerMock.Object, null)
            {
                User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity)
            };

            var result = await controller.RemoveUser();

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            string responseMessage = ((BadRequestErrorMessageResult)result).Message;
            Assert.IsTrue(responseMessage.Contains(testError1));
            Assert.IsTrue(responseMessage.Contains(testError2));

            userManagerMock.VerifyAll();
            usersMock.Verify(m => m.CleanUserInfoFromOrders(It.Is<User>(x => x.Id == testUserId)));
        }
    }
}