namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class GlassesInfoDbFillerTests
    {
        [TestMethod]
        public void FillInfo_ShouldCallIReader_WhenGlassesDictionaryIsNull()
        {
            var items = new List<OrderedItem>()
            {
                new OrderedItem() {Id = 1,  Status = DeliveryStatus.New},
                new OrderedItem() {Id = 2,  Status = DeliveryStatus.Ordered},
                new OrderedItem() {Id = 3,  Status = DeliveryStatus.Done},
                new OrderedItem() {Id = 4,  Status = DeliveryStatus.Done},
            }.AsQueryable();

            var repositoryMock = new Mock<IInternalDbRepository<OrderedItem>>();
            repositoryMock.Setup(x => x.All()).Returns(() => items);

            var service = new OrderedItemsService(repositoryMock.Object, null);

            var result = service.GetDoneOrders().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].Id);
            Assert.AreEqual(4, result[1].Id);
            repositoryMock.VerifyAll();
        }

        //[TestMethod]
        //public void FillInfo_ShouldCallIReaderAndUsePassedFile_WhenNotNullOrEmptyString()

        //[TestMethod]
        //public void FillInfo_ShouldCallIReaderWithDefaultFile_WhenEmptyString()

        //[TestMethod]
        //public void FillInfo_ShouldCallAllHandlesAddServices_WhenItemsNotExistInDb()
        // TODO verify all of these
        //foreach (var image in images)
        //    {
        //        image.VehicleGlasses.Add(glass);
        //    }
        //    foreach (var interchangeablePart in interchaneableParts)
        //    {
        //        interchangeablePart.VehicleGlasses.Add(glass);
        //    }

        //[TestMethod]
        //public void FillInfo_ShouldNotCallAllHandlesAddServices_WhenItemsExistInDb()

        //[TestMethod]
        //public void FillInfo_LoggerShouldLogException_WhenJArrayParseFails()

        //[TestMethod]
        //public void FillInfo_LoggerShouldLogException_WhenItemParseFails()

        //[TestMethod]
        //public void FillInfo_LoggerShouldLogValidationException_WhenServiceAddThrowsValidationException()

        //[TestMethod]
        //public void FillInfo_LoggerShouldLogValidationException_WhenServiceAddThrowsValidationException()

        //[TestMethod]
        //public void FillInfo_AllServicesShouldDoNothing_WhenGlassExistsInDbByEurocode()

        //[TestMethod]
        //public void FillInfo_AllServicesShouldDoNothing_WhenGlassExistsInDbByMaterialnumber()

        //[TestMethod]
        //public void FillInfo_AllServicesShouldDoNothing_WhenGlassExistsInDbByIndustrycode()

        //[TestMethod]
        //public void FillInfo_AllServicesShouldDoNothing_WhenGlassExistsInDbByLocalcode()
    }
}
