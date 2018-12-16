using System;

namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using GGG_OnlineShop.Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class UsersServiceTests
    {
        [TestMethod]
        public void GetByEmail_ShouldReturnNeededItem()
        {
            string testEmail = "2353AA";
            string testId = "userId";
            var items = new List<User>()
            {
                new User() {Id = testId,  Email = testEmail},
                new User() {Id = "1",  Email = "2"},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<User>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new UsersService(repositoryMock.Object);

            var response = service.GetByEmail(testEmail);

            Assert.AreEqual(testId, response.Id);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllNotActivated_ShouldReturnNeededItems()
        {
            var items = new List<User>()
            {
                new User() {Id = "1",  Email = "1", EmailConfirmed = true},
                new User() {Id = "2",  Email = "2", EmailConfirmed = false},
                new User() {Id = "3",  Email = "3", EmailConfirmed = false},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<User>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new UsersService(repositoryMock.Object);

            var response = service.GetAllNotActivated().ToList();

            Assert.AreEqual(2, response.Count);
            Assert.AreEqual("2", response[0].Id);
            Assert.AreEqual("3", response[1].Id);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Update_ShouldCallNeededMocks()
        {
            string testId = "1";
            var user = new User() { Id = testId };

            var repositoryMock = new Mock<IInternalDbRepository<User>>();
            repositoryMock.Setup(x => x.GetById(testId)).Returns(() => user);
            repositoryMock.Setup(x => x.Save());

            var service = new UsersService(repositoryMock.Object);

            var response = service.Update(user);

            Assert.AreEqual(testId, response.Id);
            repositoryMock.VerifyAll();
            repositoryMock.Verify(x => x.Save(), Times.Exactly(1));
            repositoryMock.Verify(x => x.GetById(testId), Times.Once);
        }

        [TestMethod]
        public void UpdateContactInfo_ShouldCallNeededMocks()
        {
            string testId = "1";
            var user = new User() { Id = testId };

            var repositoryMock = new Mock<IInternalDbRepository<User>>();
            repositoryMock.Setup(x => x.GetById(testId)).Returns(() => user);
            repositoryMock.Setup(x => x.Save());

            var service = new UsersService(repositoryMock.Object);

            var response = service.UpdateContactInfo(user);

            Assert.AreEqual(testId, response.Id);
            repositoryMock.VerifyAll();
            repositoryMock.Verify(x => x.Save(), Times.Exactly(1));
            repositoryMock.Verify(x => x.GetById(testId), Times.Once);
        }

        [TestMethod]
        public void CleanUserInfoFromOrders_ShouldCleanUserInfo()
        {
            string testId = "1";
            string testEmail = "testEmail";
            string testPhoneNumber = "testPhoneNumber";
            var user = new User()
            {
                Id = testId,
                Email = testEmail,
                PhoneNumber = testPhoneNumber,
                Orders = new List<Order>()
                {
                    new Order() { UserId = testId },
                    new Order() { UserId = testId },
                }
            };

            var repositoryMock = new Mock<IInternalDbRepository<User>>();
            repositoryMock.Setup(x => x.Save());

            var service = new UsersService(repositoryMock.Object);

            service.CleanUserInfoFromOrders(user);

            Assert.AreEqual(user.Orders.ToList()[0].UserId, null);
            Assert.AreEqual(user.Orders.ToList()[0].UserInfo, string.Format(GlobalConstants.DeletedUserInfo, user.PhoneNumber));
            Assert.AreEqual(user.Orders.ToList()[0].UserЕmail, user.Email);
            Assert.AreEqual(user.Orders.ToList()[1].UserId, null);
            Assert.AreEqual(user.Orders.ToList()[1].UserInfo, string.Format(GlobalConstants.DeletedUserInfo, user.PhoneNumber));
            Assert.AreEqual(user.Orders.ToList()[1].UserЕmail, user.Email);
            repositoryMock.VerifyAll();
            repositoryMock.Verify(x => x.Save(), Times.Exactly(1));
        }

        [TestMethod]
        public void IsValidUser_ShouldReturnFalse_WhenCompanyAndNoBulstat()
        {
            var user = new User() { Bulstat = null, IsCompany = true };

            var service = new UsersService(null);

            var response = service.IsCompanyAndBulstatCompatibiltyValid(user);
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void IsValidUser_ShouldReturnFalse_WhenCompanyAndBulstatNotUnique()
        {
            var user = new User() { Bulstat = "test", IsCompany = true };

            var users = new List<User>
            {
                user
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<User>>();
            repositoryMock.Setup(x => x.All()).Returns(users);

            var service = new UsersService(repositoryMock.Object);

            var response = service.IsCompanyAndBulstatCompatibiltyValid(user);

            Assert.IsFalse(response);
            repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void IsValidUser_ShouldReturnFalse_WhenNotCompanyBulstatNotNull()
        {
            var user = new User() { Bulstat = "123", IsCompany = false };
            var service = new UsersService(null);

            var response = service.IsCompanyAndBulstatCompatibiltyValid(user);

            Assert.IsFalse(response);
        }

        [TestMethod]
        public void IsValidUser_ShouldReturnTrue_WhenCompanyAndBulstatNotNull()
        {
            var user = new User() { Bulstat = "123", IsCompany = true };

            var users = new List<User>
            {
            }.AsQueryable();
            var repositoryMock = new Mock<IInternalDbRepository<User>>();
            repositoryMock.Setup(x => x.All()).Returns(users);

            var service = new UsersService(repositoryMock.Object);

            var response = service.IsCompanyAndBulstatCompatibiltyValid(user);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public void IsValidUser_ShouldReturnTrue_WhenNotCompanyAndNoBulstat()
        {
            var user = new User() { Bulstat = null, IsCompany = false };
            var service = new UsersService(null);

            var response = service.IsCompanyAndBulstatCompatibiltyValid(user);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public void IsBulstatValid_ShouldThrow_WhenEikLenghtNot9Or13()
        {
            var service = new UsersService(null);

            var result = service.IsBulstatValid("BG17530908123124");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBulstatValid_ShouldReturnTrueWhenOk9()
        {
            var service = new UsersService(null);

            var result = service.IsBulstatValid("BG175309086");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBulstatValid_ShouldReturnFalseWhen9AndNotValid()
        {
            var service = new UsersService(null);

            var result = service.IsBulstatValid("BG175309085");

            Assert.IsFalse(result);
        }
    }
}
