namespace GGG_OnlineShop.Data.Services.Tests
{
    using Common;
    using Contracts;
    using GGG_OnlineShop.Common.Services.Contracts;
    using InternalApiDB.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class GlassesInfoDbFillerTests
    {
        private Mock<IReader> reader;
        private Mock<ILogger> logger;
        private Mock<IVehicleGlassesService> glasses;
        private Mock<IVehiclesService> vehicles;
        private Mock<IVehicleMakesService> makes;
        private Mock<IVehicleModelsService> models;
        private Mock<IVehicleBodyTypesService> bodytypes;
        private Mock<IVehicleGlassImagesService> images;
        private Mock<IVehicleGlassCharacteristicsService> characteristics;
        private Mock<IVehicleInterchangeablePartsService> interchangeableParts;
        private Mock<IVehicleSuperceedsService> superceeds;
        private Mock<IVehicleAccessoriesService> accessories;
        private Mock<ISolutionBaseConfig> solution;

        public GlassesInfoDbFillerTests()
        {
            reader = new Mock<IReader>();
            logger = new Mock<ILogger>();
            glasses = new Mock<IVehicleGlassesService>();
            vehicles = new Mock<IVehiclesService>();
            makes = new Mock<IVehicleMakesService>();
            models = new Mock<IVehicleModelsService>();
            bodytypes = new Mock<IVehicleBodyTypesService>();
            images = new Mock<IVehicleGlassImagesService>();
            characteristics = new Mock<IVehicleGlassCharacteristicsService>();
            interchangeableParts = new Mock<IVehicleInterchangeablePartsService>();
            superceeds = new Mock<IVehicleSuperceedsService>();
            accessories = new Mock<IVehicleAccessoriesService>();
            solution = new Mock<ISolutionBaseConfig>();
        }

        [TestMethod]
        public void FillInfo_ShouldCallIReader_WhenGlassesDictionaryIsNull()
        {
            string testFile = "testFile";
            reader.Setup(x => x.Read(testFile)).Returns(() => ""); // returns empty array
            solution.Setup(x => x.GetSolutionPath()).Returns(() => "solution");

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            service.FillInfo(null, testFile);

            reader.VerifyAll();
        }

        //public void FillInfo_LoggerShouldLogException_WhenJArrayParseFails()

        //[TestMethod]
        //public void FillInfo_LoggerShouldLogException_WhenItemParseFails()

        //[TestMethod]
        //public void FillInfo_LoggerShouldLogValidationException_WhenServiceAddThrowsValidationException()

        //[TestMethod]
        //public void FillInfo_LoggerShouldLogValidationException_WhenServiceAddThrowsValidationException()

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
