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
    public class AccountControllerTests
    {
        BaseAutomapperConfig mapper = new BaseAutomapperConfig();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_ShouldThrowException_WhenVehicleGlassesServiceIsNull()
        {
            var controller = new AccountController(null, null, null);

            //var result = controller.Get(2);
        }
    }
}
