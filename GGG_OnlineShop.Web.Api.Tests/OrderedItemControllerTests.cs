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
    using System.ComponentModel.DataAnnotations;

    [TestClass]
    public class OrderedItemsControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();

        // TODO - think about modelstate test
        //[TestMethod]
        //public void GetVehicleBodyTypeByMakeAndModelIds_ShouldReturnBadRequest_WhenInvalidModel()
        //{
            
        //    var ordersMock = new Mock<IOrderedItemsService>();
        //    var usersMock = new Mock<IUsersService>();
        //    var controller = new OrderedItemController(ordersMock.Object, usersMock.Object);

        //    var model = new OrderedItemRequestModel()
        //    {
        //        Manufacturer = "nordglass",
        //        Price = 0.01,
        //        PaidPrice = 0.01
        //    };

        //    controller.ModelState.AddModelError("testError", "testError");
        //    var result = controller.Order(model);

        //    Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        //    string responseMessage = ((BadRequestErrorMessageResult)result).Message;
        //    Assert.IsTrue(responseMessage.Contains("testError"));
        //}


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Order_ShouldThrowException_WhenUsersServiceIsNull()
        {
            var usersMock = new Mock<IUsersService>();
            var controller = new OrderedItemController(null, usersMock.Object);

           
            var model = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass"
            };

            var result = controller.Order(model);
        }

        [TestMethod]
        public void Order_ShouldAddNewOrderWithAlternativeUserInfo_WhenNotAuthorized()
        {
            mapper.Execute();

            var usersMock = new Mock<IUsersService>();

            var ordersMock = new Mock<IOrderedItemsService>();
            var modelToAdd = new OrderedItem()
            {
                Manufacturer = "nordglass",
                Status = DeliveryStatus.New,
                FullAddress = string.Empty,
                WithInstallation = false,
                IsDepositNeeded = false,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                PaidPrice = 0,
                Price = 1,
                AnonymousUserInfo = "AnonymousUserInfo",
                AnonymousUserЕmail = "AnonymousUserЕmail",
            };
            ordersMock.Setup(v => v.Add(modelToAdd));

            var controller = new OrderedItemController(ordersMock.Object, usersMock.Object);

            //[StringLength(GlobalConstants.FullAddressMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
            //public string FullAddress { get; set; }
            //public string UserId { get; set; }
            //public bool UseAlternativeAddress { get; set; }

            var model = new OrderedItemRequestModel()
            {
                Manufacturer = "nordglass",
                //Status = DeliveryStatus.New,
                DeliveryNotes = "DeliveryNotes",
                Description = "Description",
                Price = 1,
                AnonymousUserInfo = "AnonymousUserInfo",
                AnonymousUserЕmail = "AnonymousUserЕmail",
            };

            var result = controller.Order(model);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            ordersMock.VerifyAll();
        }
    }
}
