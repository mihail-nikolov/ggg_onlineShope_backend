namespace GGG_OnlineShop.Data.Services.Tests
{
    using Contracts;
    using GGG_OnlineShop.Common.Services.Contracts;
    using InternalApiDB.Models;
    using JsonParseModels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;

    [TestClass]
    public class GlassesInfoDbFillerTests
    {
        private string description = "Description";
        private string euroCode = "2233AS";
        private string make = "AlfaRomeo";
        private string model = "100";
        private string productType = "Windscreen";
        private string oesCode = "223121123";
        private string modification = "modification";
        private string tint = "green";
        private string fittingType = "hand";
        private string caption = "Caption";
        private string code = "H5";
        private string bodyTypeDescription = "hatchback-5dr";
        private string interEuroCode = "InterEuroCode";
        private string interOesCode = "InterOesCode";
        private string interMaterialNumber = "InterMaterialNumber";
        private string interLocalCode = "InterLocalCode";
        private string interScanCode = "InterScanCode";
        private string interNagsCode = "InterNagsCode";
        private string interDescription = "InterDescription";
        private string oldMaterialNumber = "OldMaterialNumber";
        private string oldOesCode = "OldOesCode";
        private string oldEuroCode = "OldEuroCode";
        private string oldLocalCode = "OldLocalCode";
        private string changeDate = "ChangeDate";
        private string accessoryIndustryCode = "AccessoryIndustryCode";
        private string accessoryDescription = "AccessoryDescription";
        private string accessoryMaterialNumber = "AccessoryMaterialNumber";
        private string materialNumber = "MaterialNumber";
        private string localCode = "LocalCode";
        private string industryCode = "IndustryCode";

        BaseAutomapperConfig mapper = new BaseAutomapperConfig();

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
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_LoggerShouldLogException_WhenJArrayParseFails()
        {
            string testFile = "testFile";
            string solutionDirectory = "solution";
            reader.Setup(x => x.Read(testFile)).Returns(() => "{wrong json}"); // returns wrong json
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);
            logger.Setup(x => x.LogError(It.Is<string>(y => y.Contains("Error while parsing string to Jarray:")),
                                         It.Is<string>(z => z.Contains($@"{solutionDirectory}\DbFillInErorrs_"))));

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            service.FillInfo(null, testFile);

            reader.VerifyAll();
            logger.VerifyAll();
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_LoggerShouldLogException_WhenItemParseFails()
        {
            string testFile = "testFile";
            string solutionDirectory = "solution";
            reader.Setup(x => x.Read(testFile)).Returns(() => "[{\"Width\": \"width\"}]"); // returns wrong json
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);
            logger.Setup(x => x.LogError(It.Is<string>(y => y.Contains("Error while parsing jObject")),
                                         It.Is<string>(z => z.Contains($@"{solutionDirectory}\DbFillInErorrs_"))));

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            service.FillInfo(null, testFile);

            reader.VerifyAll();
            logger.VerifyAll();
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_ShouldCallIReaderAndUsePassedFile_WhenNotNullOrEmptyString()
        {
            string testFile = "testFile";
            string solutionDirectory = "solution";
            reader.Setup(x => x.Read(testFile)).Returns(() => "[]"); // returns empty json array
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            service.FillInfo(null, testFile);

            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            reader.VerifyAll();
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_ShouldCallIReaderWithDefaultFile_WhenEmptyString()
        {
            string testFile = null;
            string solutionDirectory = "solution";
            reader.Setup(x => x.Read($@"{solutionDirectory}\ggg\products_test.json")).Returns(() => "[]"); // returns empty json array
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            service.FillInfo(null, testFile);

            reader.VerifyAll();
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_ShouldNotAddGlass_WhenServiceAddThrowsValidationException()
        {
            string solutionDirectory = "solution";
            string testFile = $@"{solutionDirectory}\ggg\products_test.json";
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            makes.Setup(x => x.Add(It.IsAny<VehicleMake>())).Throws(new DbEntityValidationException("error validation"));

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            List<GlassJsonInfoModel> glassesInfoList = new List<GlassJsonInfoModel>
            {
                new GlassJsonInfoModel {Description = "description", Make = string.Empty }
            };
            service.FillInfo(glassesInfoList, testFile);

            reader.Verify(x => x.Read(It.IsAny<string>()), Times.Never());
            glasses.Verify(x => x.Add(It.IsAny<VehicleGlass>()), Times.Never());
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_ShouldCallAllHandlersAddServices_WhenItemsNotExistInDb()
        {
            mapper.Execute();

            string solutionDirectory = "solution";
            string testFile = $@"{solutionDirectory}\ggg\products_test.json";
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);
            var glassesInfoList = GetFullGlassInfoModelsList();

            makes.SetupSequence(x => x.GetByName(make))
                .Returns(null)
                .Returns(new VehicleMake { Id = 1, Name = make });
            models.SetupSequence(x => x.GetByName(model))
                .Returns(null)
                .Returns(new VehicleModel { Id = 1, Name = model });
            bodytypes.SetupSequence(x => x.GetByCode(code))
                .Returns(null)
                .Returns(new VehicleBodyType { Id = 1, Code = code });
            vehicles.SetupSequence(x => x.GetVehicleByMakeModelAndBodyTypeIds(1, 1, 1))
                .Returns(null)
                .Returns(new Vehicle { Id = 1, MakeId = 1, ModelId = 1, BodyTypeId = 1 });
            characteristics.SetupSequence(x => x.GetByName("A"))
                .Returns(null)
                .Returns(new VehicleGlassCharacteristic { Id = 1, Name = "A" });
            characteristics.SetupSequence(x => x.GetByName("B"))
                .Returns(null)
                .Returns(new VehicleGlassCharacteristic { Id = 2, Name = "B" });
            images.SetupSequence(x => x.GetByOriginalId(2))
                .Returns(null)
                .Returns(new VehicleGlassImage { Id = 1, OriginalId = 2, Caption = caption });
            interchangeableParts.SetupSequence(x => x.GetInterchangeablePart(interEuroCode, interMaterialNumber, interLocalCode, interScanCode, interNagsCode))
               .Returns(null)
               .Returns(new VehicleGlassInterchangeablePart
               {
                   Id = 1,
                   EuroCode = interEuroCode,
                   MaterialNumber = interMaterialNumber,
                   LocalCode = interLocalCode,
                   ScanCode = interScanCode,
                   NagsCode = interNagsCode
               });
            interchangeableParts.SetupSequence(x => x.GetInterchangeablePart(interEuroCode, interMaterialNumber, interLocalCode, interScanCode, interNagsCode))
               .Returns(null)
               .Returns(new VehicleGlassInterchangeablePart
               {
                   Id = 1,
                   EuroCode = interEuroCode,
                   MaterialNumber = interMaterialNumber,
                   LocalCode = interLocalCode,
                   ScanCode = interScanCode,
                   NagsCode = interNagsCode
               });
            accessories.SetupSequence(x => x.GetAccessory(accessoryIndustryCode, accessoryMaterialNumber))
              .Returns(null)
              .Returns(new VehicleGlassAccessory
              {
                  Id = 1,
                  MaterialNumber = accessoryMaterialNumber,
                  IndustryCode = accessoryIndustryCode
              });
            superceeds.SetupSequence(x => x.GetSuperceed(oldEuroCode, oldLocalCode, oldMaterialNumber))
             .Returns(null)
             .Returns(new VehicleGlassSuperceed
             {
                 Id = 1,
                 OldEuroCode = oldEuroCode,
                 OldMaterialNumber = oldMaterialNumber,
                 OldLocalCode = oldLocalCode,
                 OldOesCode = oldOesCode
             });
            glasses.Setup(x => x.GetGlass(euroCode, materialNumber, industryCode, localCode))
                .Returns(new VehicleGlass
                {
                    Id = 1,
                    EuroCode = euroCode
                });

            // ACT
            service.FillInfo(glassesInfoList, testFile);

            // ASSERT
            makes.Verify(x => x.Add(It.Is<VehicleMake>(y => y.Name == make)), Times.Once());
            makes.VerifyAll();
            models.Verify(x => x.Add(It.Is<VehicleModel>(y => y.Name == model)), Times.Once());
            models.VerifyAll();
            bodytypes.Verify(x => x.Add(It.Is<VehicleBodyType>(y => y.Code == code && y.Description == bodyTypeDescription)), Times.Once());
            bodytypes.VerifyAll();
            vehicles.Verify(x => x.Add(It.Is<Vehicle>(y => y.MakeId == 1 && y.ModelId == 1 && y.BodyTypeId == 1)), Times.Once());
            vehicles.VerifyAll();
            characteristics.Verify(x => x.Add(It.Is<VehicleGlassCharacteristic>(y => y.Name == "A")), Times.Once());
            characteristics.Verify(x => x.Add(It.Is<VehicleGlassCharacteristic>(y => y.Name == "B")), Times.Once());
            characteristics.VerifyAll();
            images.Verify(x => x.Add(It.Is<VehicleGlassImage>(y => y.OriginalId == 2 && y.Caption == caption)), Times.Once());
            images.VerifyAll();
            interchangeableParts.Verify(x => x.Add(It.Is<VehicleGlassInterchangeablePart>(y =>
                                                        y.EuroCode == interEuroCode &&
                                                        y.MaterialNumber == interMaterialNumber &&
                                                        y.LocalCode == interLocalCode &&
                                                        y.ScanCode == interScanCode &&
                                                        y.OesCode == interOesCode &&
                                                        y.Description == interDescription &&
                                                        y.NagsCode == interNagsCode
                                                        )), Times.Once());
            interchangeableParts.VerifyAll();
            accessories.Verify(x => x.Add(It.Is<VehicleGlassAccessory>(y =>
                                                y.IndustryCode == accessoryIndustryCode &&
                                                y.MaterialNumber == accessoryMaterialNumber &&
                                                y.Description == accessoryDescription &&
                                                y.ReplacementRate == 2 &&
                                                y.Mandatory == false &&
                                                y.HasImages == false &&
                                                y.HasFittingMethod == true &&
                                                y.RecommendedQuantity == 2
                                                )), Times.Once());
            accessories.VerifyAll();
            superceeds.Verify(x => x.Add(It.Is<VehicleGlassSuperceed>(y =>
                                               y.OldOesCode == oldOesCode &&
                                               y.OldMaterialNumber == oldMaterialNumber &&
                                               y.OldLocalCode == oldLocalCode &&
                                               y.OldEuroCode == oldEuroCode &&
                                               y.ChangeDate == changeDate
                                               )), Times.Once());
            superceeds.VerifyAll();

            reader.Verify(x => x.Read(It.IsAny<string>()), Times.Never());
            glasses.Verify(x => x.Add(It.Is<VehicleGlass>(y =>
                                                 y.EuroCode == euroCode &&
                                                 y.MaterialNumber == materialNumber &&
                                                 y.LocalCode == localCode &&
                                                 y.IndustryCode == industryCode &&
                                                 y.Description == description &&
                                                 y.ProductType == productType &&
                                                 y.Modification == modification &&
                                                 y.Tint == tint &&
                                                 y.FittingType == fittingType &&
                                                 y.FittingTimeHours == 2 &&
                                                 y.HasFittingMethod == true &&
                                                 y.Height == 2 &&
                                                 y.Width == 2 &&
                                                 y.FeaturedImageId == 2 &&
                                                 y.IsAccessory == false &&
                                                 y.IsYesGlass == false &&
                                                 y.IsAcoustic == true &&
                                                 y.IsCalibration == true &&
                                                 y.ModelDate == "10/11 - 10/12" &&
                                                 y.PartDate == "10/11 - 10/12"
                                        )), Times.Once());
            glasses.VerifyAll();
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_ShouldNotCallAllHandlersAddServices_WhenItemsExistInDb()
        {
            mapper.Execute();

            string solutionDirectory = "solution";
            string testFile = $@"{solutionDirectory}\ggg\products_test.json";
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);
            var glassesInfoList = GetFullGlassInfoModelsList();

            makes.Setup(x => x.GetByName(make)).Returns(new VehicleMake { Id = 1, Name = make });
            models.Setup(x => x.GetByName(model)).Returns(new VehicleModel { Id = 1, Name = model });
            bodytypes.Setup(x => x.GetByCode(code))
                .Returns(new VehicleBodyType { Id = 1, Code = code });
            vehicles.Setup(x => x.GetVehicleByMakeModelAndBodyTypeIds(1, 1, 1))
                .Returns(new Vehicle { Id = 1, MakeId = 1, ModelId = 1, BodyTypeId = 1 });
            characteristics.Setup(x => x.GetByName("A"))
                .Returns(new VehicleGlassCharacteristic { Id = 1, Name = "A" });
            characteristics.Setup(x => x.GetByName("B"))
                .Returns(new VehicleGlassCharacteristic { Id = 2, Name = "B" });
            images.Setup(x => x.GetByOriginalId(2))
                .Returns(new VehicleGlassImage { Id = 1, OriginalId = 2, Caption = caption });
            interchangeableParts.Setup(x => x.GetInterchangeablePart(interEuroCode, interMaterialNumber, interLocalCode, interScanCode, interNagsCode))
               .Returns(new VehicleGlassInterchangeablePart
               {
                   Id = 1,
                   EuroCode = interEuroCode,
                   MaterialNumber = interMaterialNumber,
                   LocalCode = interLocalCode,
                   ScanCode = interScanCode,
                   NagsCode = interNagsCode
               });
            interchangeableParts.Setup(x => x.GetInterchangeablePart(interEuroCode, interMaterialNumber, interLocalCode, interScanCode, interNagsCode))
               .Returns(new VehicleGlassInterchangeablePart
               {
                   Id = 1,
                   EuroCode = interEuroCode,
                   MaterialNumber = interMaterialNumber,
                   LocalCode = interLocalCode,
                   ScanCode = interScanCode,
                   NagsCode = interNagsCode
               });
            accessories.Setup(x => x.GetAccessory(accessoryIndustryCode, accessoryMaterialNumber))
              .Returns(new VehicleGlassAccessory
              {
                  Id = 1,
                  MaterialNumber = accessoryMaterialNumber,
                  IndustryCode = accessoryIndustryCode
              });
            superceeds.Setup(x => x.GetSuperceed(oldEuroCode, oldLocalCode, oldMaterialNumber))
             .Returns(new VehicleGlassSuperceed
             {
                 Id = 1,
                 OldEuroCode = oldEuroCode,
                 OldMaterialNumber = oldMaterialNumber,
                 OldLocalCode = oldLocalCode,
                 OldOesCode = oldOesCode
             });
            glasses.Setup(x => x.GetGlass(euroCode, materialNumber, industryCode, localCode))
                .Returns(new VehicleGlass
                {
                    Id = 1,
                    EuroCode = euroCode
                });

            // ACT
            service.FillInfo(glassesInfoList, testFile);

            // ASSERT
            makes.Verify(x => x.Add(It.IsAny<VehicleMake>()), Times.Never());
            makes.VerifyAll();
            models.Verify(x => x.Add(It.IsAny<VehicleModel>()), Times.Never());
            models.VerifyAll();
            bodytypes.Verify(x => x.Add(It.IsAny<VehicleBodyType>()), Times.Never());
            bodytypes.VerifyAll();
            vehicles.Verify(x => x.Add(It.IsAny<Vehicle>()), Times.Never());
            vehicles.VerifyAll();
            characteristics.Verify(x => x.Add(It.IsAny<VehicleGlassCharacteristic>()), Times.Never());
            characteristics.VerifyAll();
            images.Verify(x => x.Add(It.IsAny<VehicleGlassImage>()), Times.Never());
            images.VerifyAll();
            interchangeableParts.Verify(x => x.Add(It.IsAny<VehicleGlassInterchangeablePart>()), Times.Never());
            interchangeableParts.VerifyAll();
            accessories.Verify(x => x.Add(It.IsAny<VehicleGlassAccessory>()), Times.Never());
            accessories.VerifyAll();
            superceeds.Verify(x => x.Add(It.IsAny<VehicleGlassSuperceed>()), Times.Never());
            superceeds.VerifyAll();

            reader.Verify(x => x.Read(It.IsAny<string>()), Times.Never());
            glasses.Verify(x => x.Add(It.Is<VehicleGlass>(y =>
                                                 y.EuroCode == euroCode &&
                                                 y.MaterialNumber == materialNumber &&
                                                 y.LocalCode == localCode &&
                                                 y.IndustryCode == industryCode &&
                                                 y.Description == description &&
                                                 y.ProductType == productType &&
                                                 y.Modification == modification &&
                                                 y.Tint == tint &&
                                                 y.FittingType == fittingType &&
                                                 y.FittingTimeHours == 2 &&
                                                 y.HasFittingMethod == true &&
                                                 y.Height == 2 &&
                                                 y.Width == 2 &&
                                                 y.FeaturedImageId == 2 &&
                                                 y.IsAccessory == false &&
                                                 y.IsYesGlass == false &&
                                                 y.IsAcoustic == true &&
                                                 y.IsCalibration == true &&
                                                 y.ModelDate == "10/11 - 10/12" &&
                                                 y.PartDate == "10/11 - 10/12"
                                        )), Times.Once());
            glasses.VerifyAll();
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_AllServicesShouldDoNothing_WhenGlassExistsInDbByEurocode()
        {
            mapper.Execute();

            string solutionDirectory = "solution";
            string testFile = $@"{solutionDirectory}\ggg\products_test.json";
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            List<GlassJsonInfoModel> glassesInfoList = new List<GlassJsonInfoModel>
            {
                new GlassJsonInfoModel {EuroCode = euroCode }
            };

            var codes = new List<string>
            {
                euroCode, "test1", "test2"
            }.AsQueryable();

            glasses.Setup(x => x.GetAllUniqueCodesFromDb())
                .Returns(codes);

            // ACT
            service.FillInfo(glassesInfoList, testFile);

            // ASSERT
            makes.Verify(x => x.Add(It.IsAny<VehicleMake>()), Times.Never());
            models.Verify(x => x.Add(It.IsAny<VehicleModel>()), Times.Never());
            bodytypes.Verify(x => x.Add(It.IsAny<VehicleBodyType>()), Times.Never());
            vehicles.Verify(x => x.Add(It.IsAny<Vehicle>()), Times.Never());
            characteristics.Verify(x => x.Add(It.IsAny<VehicleGlassCharacteristic>()), Times.Never());
            images.Verify(x => x.Add(It.IsAny<VehicleGlassImage>()), Times.Never());
            interchangeableParts.Verify(x => x.Add(It.IsAny<VehicleGlassInterchangeablePart>()), Times.Never());
            accessories.Verify(x => x.Add(It.IsAny<VehicleGlassAccessory>()), Times.Never());
            superceeds.Verify(x => x.Add(It.IsAny<VehicleGlassSuperceed>()), Times.Never());
            reader.Verify(x => x.Read(It.IsAny<string>()), Times.Never());
            glasses.Verify(x => x.Add(It.IsAny<VehicleGlass>()), Times.Never());
            glasses.VerifyAll();
            solution.VerifyAll();
        }
        [TestMethod]
        public void FillInfo_AllServicesShouldDoNothing_WhenGlassExistsInDbByMaterialnumber()
        {
            mapper.Execute();

            string solutionDirectory = "solution";
            string testFile = $@"{solutionDirectory}\ggg\products_test.json";
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            List<GlassJsonInfoModel> glassesInfoList = new List<GlassJsonInfoModel>
            {
                new GlassJsonInfoModel { MaterialNumber = materialNumber }
            };

            var codes = new List<string>
            {
                "test1", materialNumber
            }.AsQueryable();

            glasses.Setup(x => x.GetAllUniqueCodesFromDb())
                .Returns(codes);

            // ACT
            service.FillInfo(glassesInfoList, testFile);

            // ASSERT
            makes.Verify(x => x.Add(It.IsAny<VehicleMake>()), Times.Never());
            models.Verify(x => x.Add(It.IsAny<VehicleModel>()), Times.Never());
            bodytypes.Verify(x => x.Add(It.IsAny<VehicleBodyType>()), Times.Never());
            vehicles.Verify(x => x.Add(It.IsAny<Vehicle>()), Times.Never());
            characteristics.Verify(x => x.Add(It.IsAny<VehicleGlassCharacteristic>()), Times.Never());
            images.Verify(x => x.Add(It.IsAny<VehicleGlassImage>()), Times.Never());
            interchangeableParts.Verify(x => x.Add(It.IsAny<VehicleGlassInterchangeablePart>()), Times.Never());
            accessories.Verify(x => x.Add(It.IsAny<VehicleGlassAccessory>()), Times.Never());
            superceeds.Verify(x => x.Add(It.IsAny<VehicleGlassSuperceed>()), Times.Never());
            reader.Verify(x => x.Read(It.IsAny<string>()), Times.Never());
            glasses.Verify(x => x.Add(It.IsAny<VehicleGlass>()), Times.Never());
            glasses.VerifyAll();
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_AllServicesShouldDoNothing_WhenGlassExistsInDbByIndustrycode()
        {
            mapper.Execute();

            string solutionDirectory = "solution";
            string testFile = $@"{solutionDirectory}\ggg\products_test.json";
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            List<GlassJsonInfoModel> glassesInfoList = new List<GlassJsonInfoModel>
            {
                new GlassJsonInfoModel { IndustryCode = industryCode }
            };

            var codes = new List<string>
            {
                "test1", industryCode
            }.AsQueryable();

            glasses.Setup(x => x.GetAllUniqueCodesFromDb())
                .Returns(codes);

            // ACT
            service.FillInfo(glassesInfoList, testFile);

            // ASSERT
            makes.Verify(x => x.Add(It.IsAny<VehicleMake>()), Times.Never());
            models.Verify(x => x.Add(It.IsAny<VehicleModel>()), Times.Never());
            bodytypes.Verify(x => x.Add(It.IsAny<VehicleBodyType>()), Times.Never());
            vehicles.Verify(x => x.Add(It.IsAny<Vehicle>()), Times.Never());
            characteristics.Verify(x => x.Add(It.IsAny<VehicleGlassCharacteristic>()), Times.Never());
            images.Verify(x => x.Add(It.IsAny<VehicleGlassImage>()), Times.Never());
            interchangeableParts.Verify(x => x.Add(It.IsAny<VehicleGlassInterchangeablePart>()), Times.Never());
            accessories.Verify(x => x.Add(It.IsAny<VehicleGlassAccessory>()), Times.Never());
            superceeds.Verify(x => x.Add(It.IsAny<VehicleGlassSuperceed>()), Times.Never());
            reader.Verify(x => x.Read(It.IsAny<string>()), Times.Never());
            glasses.Verify(x => x.Add(It.IsAny<VehicleGlass>()), Times.Never());
            glasses.VerifyAll();
            solution.VerifyAll();
        }

        [TestMethod]
        public void FillInfo_AllServicesShouldDoNothing_WhenGlassExistsInDbByLocalcode()
        {
            mapper.Execute();

            string solutionDirectory = "solution";
            string testFile = $@"{solutionDirectory}\ggg\products_test.json";
            solution.Setup(x => x.GetSolutionPath()).Returns(() => solutionDirectory);

            GlassesInfoDbFiller service = new GlassesInfoDbFiller(glasses.Object, vehicles.Object, makes.Object, models.Object,
                                                                  bodytypes.Object, images.Object, characteristics.Object, interchangeableParts.Object,
                                                                  superceeds.Object, accessories.Object, logger.Object, reader.Object, solution.Object);

            List<GlassJsonInfoModel> glassesInfoList = new List<GlassJsonInfoModel>
            {
                new GlassJsonInfoModel { LocalCode = localCode }
            };

            var codes = new List<string>
            {
                localCode, materialNumber
            }.AsQueryable();

            glasses.Setup(x => x.GetAllUniqueCodesFromDb())
                .Returns(codes);

            // ACT
            service.FillInfo(glassesInfoList, testFile);

            // ASSERT
            makes.Verify(x => x.Add(It.IsAny<VehicleMake>()), Times.Never());
            models.Verify(x => x.Add(It.IsAny<VehicleModel>()), Times.Never());
            bodytypes.Verify(x => x.Add(It.IsAny<VehicleBodyType>()), Times.Never());
            vehicles.Verify(x => x.Add(It.IsAny<Vehicle>()), Times.Never());
            characteristics.Verify(x => x.Add(It.IsAny<VehicleGlassCharacteristic>()), Times.Never());
            images.Verify(x => x.Add(It.IsAny<VehicleGlassImage>()), Times.Never());
            interchangeableParts.Verify(x => x.Add(It.IsAny<VehicleGlassInterchangeablePart>()), Times.Never());
            accessories.Verify(x => x.Add(It.IsAny<VehicleGlassAccessory>()), Times.Never());
            superceeds.Verify(x => x.Add(It.IsAny<VehicleGlassSuperceed>()), Times.Never());
            reader.Verify(x => x.Read(It.IsAny<string>()), Times.Never());
            glasses.Verify(x => x.Add(It.IsAny<VehicleGlass>()), Times.Never());
            glasses.VerifyAll();
            solution.VerifyAll();
        }

        private List<GlassJsonInfoModel> GetFullGlassInfoModelsList()
        {
            List<GlassJsonInfoModel> glassesInfoList = new List<GlassJsonInfoModel>
            {
                new GlassJsonInfoModel
                {
                    Description = description,
                    EuroCode = euroCode,
                    Make = make,
                    Model = model,
                    ProductType = productType,
                    OesCode = oesCode,
                    ModelYear = new ModelAndPartDatesJsonInfoModel { From = new ModelPartYearMonthJsonInfoModel {Month = 10, Year = 11 }, To = new ModelPartYearMonthJsonInfoModel {Month = 10, Year = 12 } },
                    PartYear = new ModelAndPartDatesJsonInfoModel { From = new ModelPartYearMonthJsonInfoModel {Month = 10, Year = 11 }, To = new ModelPartYearMonthJsonInfoModel {Month = 10, Year = 12 } },
                    Modification = modification,
                    Tint = tint,
                    FittingTimeHours = 2,
                    FittingType = fittingType,
                    Height = 2,
                    Width = 2,
                    Characteristics  = new List<string> {"A", "B" },
                    BodyTypes = new List<BodyTypeJsonInfoModel>
                    {
                        new BodyTypeJsonInfoModel
                        {
                            Code  = code,
                            Description  = bodyTypeDescription,
                        }
                    },
                    Images = new List<ImageJsonInfoModel>
                    {
                        new ImageJsonInfoModel
                        {
                            Id  = 2,
                            Caption  = caption,
                        }
                    },
                    HasFittingMethod = true,
                    FeaturedImageId = 2,
                    IsYesGlass = false,
                    IsAcoustic = true,
                    IsCalibration = true,
                    InterchangeableParts = new List<InterchangeableParJsonInfoModel>
                    {
                        new InterchangeableParJsonInfoModel
                        {
                            EuroCode  = interEuroCode,
                            OesCode  = interOesCode,
                            MaterialNumber  = interMaterialNumber,
                            LocalCode  = interLocalCode,
                            ScanCode  = interScanCode,
                            NagsCode  = interNagsCode,
                            Description  = interDescription,
                        }
                    },
                    IsAccessory = false,
                    Superceeds = new List<SuperceedJsonInfoModel>
                    {
                        new SuperceedJsonInfoModel
                        {
                            OldMaterialNumber = oldMaterialNumber,
                            OldOesCode = oldOesCode,
                            OldEuroCode = oldEuroCode,
                            OldLocalCode = oldLocalCode,
                            ChangeDate = changeDate,
                        }
                    },
                    Accessories = new List<AccessoryJsonInfoModel>
                    {
                        new AccessoryJsonInfoModel
                        {
                             IndustryCode = accessoryIndustryCode,
                             Description = accessoryDescription,
                             MaterialNumber = accessoryMaterialNumber,
                             ReplacementRate = 2,
                             Mandatory = false,
                             HasImages = false,
                             HasFittingMethod = true,
                             RecommendedQuantity = 2,
                        }
                    },
                    MaterialNumber = materialNumber,
                    LocalCode = localCode,
                    IndustryCode = industryCode,
                }
            };

            return glassesInfoList;
        }
    }
}
