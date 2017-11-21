namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using JsonParseModels;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using InternalApiDB.Models;
    using AutoMapper;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using GGG_OnlineShop.Common.Services.Contracts;
    using Infrastructure;
    using System.IO;
    using System.Web;

    public class GlassesInfoDbFiller : IGlassesInfoDbFiller
    {
        // TODO - check for not implemented
        private static string solutionDirectory = Path.GetDirectoryName(Path.GetDirectoryName(HttpRuntime.AppDomainAppPath));

        private string errorsfilePathToWrite = $@"{solutionDirectory}\DbFillInErorrs_{DateTime.Now.ToString("ddMMyy_HHmm")}.txt";
        private string parsedGlassesInfofilePath = $@"{solutionDirectory}\DbFillInInfo_{DateTime.Now.ToString("ddMMyy_HHmm")}.txt";
        private string jsonFilePathToRead = $@"{solutionDirectory}\ggg\products_test.json";

        public GlassesInfoDbFiller(IVehicleGlassesService glasses,
                                   IVehiclesService vehicles,
                                   IVehicleMakesService makes,
                                   IVehicleModelsService models,
                                   IVehicleBodyTypesService bodytypes,
                                   IVehicleGlassImagesService images,
                                   IVehicleGlassCharacteristicsService characteristics,
                                   IVehicleInterchangeablePartsService interchangeableParts,
                                   IVehicleSuperceedsService superceeds,
                                   IVehicleAccessoriesService accessories,
                                   ILogger logger,
                                   IReader reader
                                )
        {
            this.Glasses = glasses;
            this.Vehicles = vehicles;
            this.Makes = makes;
            this.Models = models;
            this.Bodytypes = bodytypes;
            this.Images = images;
            this.Characteristics = characteristics;
            this.InterchangeableParts = interchangeableParts;
            this.Superceeds = superceeds;
            this.Accessories = accessories;

            this.Logger = logger;
            this.Reader = reader;
        }
        //------------------------------ Services ----------------------------------------------

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }

        protected IVehicleGlassesService Glasses { get; set; }

        protected IVehiclesService Vehicles { get; set; }

        protected IVehicleMakesService Makes { get; set; }

        protected IVehicleModelsService Models { get; set; }

        protected IVehicleBodyTypesService Bodytypes { get; set; }

        protected IVehicleGlassImagesService Images { get; set; }

        protected IVehicleGlassCharacteristicsService Characteristics { get; set; }

        protected IVehicleInterchangeablePartsService InterchangeableParts { get; private set; }

        protected IVehicleSuperceedsService Superceeds { get; private set; }

        protected IVehicleAccessoriesService Accessories { get; private set; }

        protected ILogger Logger { get; set; }

        protected IReader Reader { get; set; }

        //------------------------------ Public FillInfo ----------------------------------------------

        public void FillInfo(IList<GlassJsonInfoModel> glasses, string passedFile="")
        {
            try
            {
                if (glasses != null)
                {
                    this.AddArrayOfGlassJsonInfoModels(glasses);
                }
                else
                {
                    this.AddArrayOfJsonsToDb(passedFile);
                }
            }
            catch (Exception e)
            {
                // uncomment
                Logger.LogError(e.Message, errorsfilePathToWrite);
            }
        }

        //------------------------------ ActualFillers ----------------------------------------------

        private void AddArrayOfGlassJsonInfoModels(IList<GlassJsonInfoModel> glasses)
        {
            for (int i = 0; i < glasses.Count; i++)
            {
                var glass = glasses[i];

                try
                {
                    this.AddInDbAndLogInfo(glass);
                }
                catch (DbEntityValidationException dbEx)
                {
                    this.ValidationExceptionCatcher(dbEx, glass, i);
                }
                catch (Exception e)
                {
                    // uncomment
                    this.Logger.LogError($"Error while parsing jObject --{glass.ToString()}-- under index ({i}) to Glass: {e.Message}",
                                       errorsfilePathToWrite);
                }
            }
        }

        private void AddArrayOfJsonsToDb(string passedFile)
        {
            string fileToUse = jsonFilePathToRead;

            if (!string.IsNullOrEmpty(passedFile))
            {
                fileToUse = passedFile;
            }

            string jsonInfoString = this.Reader.Read(fileToUse);
            JArray glassesJArray = this.ParseStringToJarray(jsonInfoString);

            for (int i = 0; i < glassesJArray.Count; i++)
            {
                var glassItemJson = glassesJArray[i];
                GlassJsonInfoModel newGlass = new GlassJsonInfoModel();
                try
                {
                    newGlass = glassItemJson.ToObject<GlassJsonInfoModel>();
                    this.AddInDbAndLogInfo(newGlass);
                }
                catch (DbEntityValidationException dbEx)
                {
                    ValidationExceptionCatcher(dbEx, newGlass, i);
                }
                catch (Exception e)
                {
                    // uncomment
                    this.Logger.LogError($"Error while parsing jObject --{glassItemJson.ToString()}-- under index ({i}) to Glass: {e.Message}",
                                        errorsfilePathToWrite);
                }
            }
        }

        //------------------------------ Helper Methods ----------------------------------------------

        private void ValidationExceptionCatcher(DbEntityValidationException dbEx, GlassJsonInfoModel glass, int index)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    string info = string.Format("Property: [{0}] Error: [{1}]",
                                            validationError.PropertyName,
                                            validationError.ErrorMessage);
                    Trace.TraceInformation(info);
                    // uncomment
                    this.Logger.LogError($"Validation Error: jObject with Eurocode --{glass.EuroCode}-- index ({index}) to Glass, DB exception: {info}",
                                            errorsfilePathToWrite);
                }
            }
        }

        private void AddInDbAndLogInfo(GlassJsonInfoModel glass)
        {
            bool added = this.CheckGlassJsonInfoModelAndAddToDb(glass);
            if (added)
            {
                // uncomment
                //this.Logger.LogInfo(glass.ToString(), parsedGlassesInfofilePath);
                //this.Logger.LogInfo("=======================================", parsedGlassesInfofilePath);
            }
        }

        private JArray ParseStringToJarray(string jsonArrayString)
        {
            try
            {
                JArray jarray = JArray.Parse(jsonArrayString);
                return jarray;
            }
            catch (Exception e)
            {
                // uncomment
                this.Logger.LogError($"Error while parsing string to Jarray: {e.Message}", errorsfilePathToWrite);
                return null;
            }
        }

        //------------------------------ Glass Info Splitters----------------------------------------------

        private bool CheckGlassJsonInfoModelAndAddToDb(GlassJsonInfoModel glassInfoModel)
        {
            bool added = false;
            VehicleGlass glassFromDb = this.Glasses.GetGlass(glassInfoModel.EuroCode,
                                                             glassInfoModel.OesCode,
                                                             glassInfoModel.MaterialNumber,
                                                             glassInfoModel.LocalCode,
                                                             glassInfoModel.IndustryCode);
            if (glassFromDb == null)
            {
                // Get car IDs or create ones and all related entities
                int makeId = this.CheckAndGetMakeId(glassInfoModel.Make);
                int? modelId = this.CheckAndGetModelId(glassInfoModel.Model);
                var bodyTypeIds = this.CheckAndGetBodyTypeId(glassInfoModel.BodyTypes);
                var vehicles = this.CheckAndGetVehicleIds(makeId, modelId, bodyTypeIds);

                var characteristics = this.CheckAndGetCharacteristics(glassInfoModel.Characteristics);
                var images = this.CheckAndGetImages(glassInfoModel.Images);
                var interchaneableParts = this.CheckAndGetInterchaneableParts(glassInfoModel.InterchangeableParts);
                // -------------------
                var superceeds = this.CheckAndGetAccessories(glassInfoModel.Accessories);
                var accessories = this.CheckAndGetSuperceeds(glassInfoModel.Superceeds);

                this.CreateNewGlassWithAllRelations(glassInfoModel, vehicles, characteristics, images, interchaneableParts, accessories, superceeds);

                added = true;
            }

            return added;
        }

        private List<VehicleGlassSuperceed> CheckAndGetSuperceeds(List<SuperceedJsonInfoModel> superceeds)
        {
            List<VehicleGlassSuperceed> vehicleSupeceeds = new List<VehicleGlassSuperceed>();
            foreach (var superceed in superceeds)
            {
                var superceedFromDb = this.Superceeds.GetSuperceed(superceed.OldEuroCode,
                                                                   superceed.OldOesCode,
                                                                   superceed.OldLocalCode,
                                                                   superceed.OldMaterialNumber);
                if (superceedFromDb == null)
                {
                    var newSuperceed = this.Mapper.Map<VehicleGlassSuperceed>(superceed);

                    this.Superceeds.Add(newSuperceed);
                    var superceedToAdd = this.Superceeds.GetSuperceed(superceed.OldEuroCode,
                                                                      superceed.OldOesCode,
                                                                      superceed.OldLocalCode,
                                                                      superceed.OldMaterialNumber);
                    vehicleSupeceeds.Add(superceedToAdd);
                }
                else
                {
                    vehicleSupeceeds.Add(superceedFromDb);
                }
            }

            return vehicleSupeceeds;
        }

        private List<VehicleGlassAccessory> CheckAndGetAccessories(List<AccessoryJsonInfoModel> accessories)
        {
            List<VehicleGlassAccessory> vehicleAccessories = new List<VehicleGlassAccessory>();
            foreach (var accessory in accessories)
            {
                var accessoryFromDb = this.Accessories.GetAccessory(accessory.IndustryCode, accessory.MaterialNumber);
                if (accessoryFromDb == null)
                {
                    var newAccessory = this.Mapper.Map<VehicleGlassAccessory>(accessory);

                    this.Accessories.Add(newAccessory);

                    var accessoryToAdd = this.Accessories.GetAccessory(accessory.IndustryCode, accessory.MaterialNumber);
                    vehicleAccessories.Add(accessoryToAdd);
                }
                else
                {
                    vehicleAccessories.Add(accessoryFromDb);
                }
            }

            return vehicleAccessories;
        }

        private void CreateNewGlassWithAllRelations(GlassJsonInfoModel glassInfoModel,
                                                    List<Vehicle> vehicles, List<VehicleGlassCharacteristic> characteristics,
                                                    List<VehicleGlassImage> images, List<VehicleGlassInterchangeablePart> interchaneableParts,
                                                    List<VehicleGlassSuperceed> superceeds, List<VehicleGlassAccessory> accessories)
        {
            VehicleGlass newGlass = this.Mapper.Map<VehicleGlass>(glassInfoModel);
            this.Glasses.Add(newGlass);

            var glass = this.Glasses.GetGlass(newGlass.EuroCode, newGlass.OesCode,
                                              newGlass.MaterialNumber, newGlass.LocalCode,
                                              newGlass.IndustryCode
                                              );

            foreach (var vehicle in vehicles)
            {
                vehicle.VehicleGlasses.Add(glass);
            }
            foreach (var characteristic in characteristics)
            {
                characteristic.VehicleGlasses.Add(glass);
            }
            foreach (var image in images)
            {
                image.VehicleGlasses.Add(glass);
            }
            foreach (var interchangeablePart in interchaneableParts)
            {
                interchangeablePart.VehicleGlasses.Add(glass);
            }

            foreach (var supereed in superceeds)
            {
                supereed.VehicleGlasses.Add(glass);
            }

            foreach (var accessory in accessories)
            {
                accessory.VehicleGlasses.Add(glass);
            }
        }

        private List<VehicleGlassInterchangeablePart> CheckAndGetInterchaneableParts(List<InterchangeableParJsonInfoModel> interchangeableParts)
        {
            List<VehicleGlassInterchangeablePart> vehicleInterchaneableParts = new List<VehicleGlassInterchangeablePart>();
            foreach (var interchangeablePart in interchangeableParts)
            {
                var interchangeablePartFromDb = this.InterchangeableParts.GetInterchangeablePart(interchangeablePart.EuroCode,
                                                                                                 interchangeablePart.OesCode,
                                                                                                 interchangeablePart.MaterialNumber,
                                                                                                 interchangeablePart.LocalCode,
                                                                                                 interchangeablePart.ScanCode,
                                                                                                 interchangeablePart.NagsCode);
                if (interchangeablePartFromDb == null)
                {
                    var newInterchangeablePart = this.Mapper.Map<VehicleGlassInterchangeablePart>(interchangeablePart);

                    this.InterchangeableParts.Add(newInterchangeablePart);
                    var interchangeableGlassToAdd = this.InterchangeableParts.GetInterchangeablePart(interchangeablePart.EuroCode,
                                                                                                     interchangeablePart.OesCode,
                                                                                                     interchangeablePart.MaterialNumber,
                                                                                                     interchangeablePart.LocalCode,
                                                                                                     interchangeablePart.ScanCode,
                                                                                                     interchangeablePart.NagsCode);
                    vehicleInterchaneableParts.Add(interchangeableGlassToAdd);
                }
                else
                {
                    vehicleInterchaneableParts.Add(interchangeablePartFromDb);
                }
            }

            return vehicleInterchaneableParts;
        }

        private List<VehicleGlassImage> CheckAndGetImages(List<ImageJsonInfoModel> images)
        {
            List<VehicleGlassImage> imagesToadd = new List<VehicleGlassImage>();
            foreach (var image in images)
            {
                VehicleGlassImage existingImage = this.Images.GetByOriginalId(image.Id);
                if (existingImage == null)
                {
                    VehicleGlassImage newImage = this.Mapper.Map<VehicleGlassImage>(image);
                    this.Images.Add(newImage);
                    imagesToadd.Add(this.Images.GetByOriginalId(newImage.OriginalId));
                }
                else
                {
                    imagesToadd.Add(existingImage);
                }
            }

            return imagesToadd;
        }

        private List<VehicleGlassCharacteristic> CheckAndGetCharacteristics(List<string> characteristics)
        {
            List<VehicleGlassCharacteristic> characteristicsToAdd = new List<VehicleGlassCharacteristic>();
            foreach (var characteristicName in characteristics)
            {
                VehicleGlassCharacteristic existingcharacteristic = this.Characteristics.GetByName(characteristicName);
                if (existingcharacteristic == null)
                {
                    VehicleGlassCharacteristic newCharacteristic = new VehicleGlassCharacteristic() { Name = characteristicName };

                    this.Characteristics.Add(newCharacteristic);
                    characteristicsToAdd.Add(this.Characteristics.GetByName(newCharacteristic.Name));
                }
                else
                {
                    characteristicsToAdd.Add(existingcharacteristic);
                }
            }

            return characteristicsToAdd;
        }

        private List<Vehicle> CheckAndGetVehicleIds(int makeId, int? modelId, List<int> bodyTypeIds)
        {
            List<Vehicle> vehiclesToAdd = new List<Vehicle>();

            if (bodyTypeIds.Count == 0)
            {
                var Vehicle = this.GetAndCheckVehicleIdByMakeIdModelIdBodyTypeId(makeId, modelId, null);
                vehiclesToAdd.Add(Vehicle);
            }
            else
            {
                foreach (var bodyTypeId in bodyTypeIds)
                {
                    var Vehicle = this.GetAndCheckVehicleIdByMakeIdModelIdBodyTypeId(makeId, modelId, bodyTypeId);
                    vehiclesToAdd.Add(Vehicle);
                }
            }

            return vehiclesToAdd;
        }

        private Vehicle GetAndCheckVehicleIdByMakeIdModelIdBodyTypeId(int makeId, int? modelId, int? bodyTypeId)
        {
            var vehicle = this.Vehicles.GetVehicleByMakeModelAndBodyTypeIds(makeId, modelId, bodyTypeId);
            if (vehicle == null)
            {
                Vehicle newVehicle = new Vehicle() { MakeId = makeId, ModelId = modelId, BodyTypeId = bodyTypeId };
                this.Vehicles.Add(newVehicle);

                vehicle = this.Vehicles.GetVehicleByMakeModelAndBodyTypeIds(newVehicle.MakeId, newVehicle.ModelId, newVehicle.BodyTypeId);
            }

            return vehicle;
        }

        private int CheckAndGetMakeId(string makeName)
        {
            int makeId = 0;

            var make = this.Makes.GetByName(makeName);
            if (make == null)
            {
                VehicleMake newMake = new VehicleMake() { Name = makeName };
                this.Makes.Add(newMake);

                makeId = this.Makes.GetByName(newMake.Name).Id;
            }
            else
            {
                makeId = make.Id;
            }

            return makeId;
        }

        private int? CheckAndGetModelId(string modelName)
        {
            int? modelId = null;
            if (!string.IsNullOrEmpty(modelName))
            {
                VehicleModel model = this.Models.GetByName(modelName);
                if (model == null)
                {
                    VehicleModel newModel = new VehicleModel() { Name = modelName };
                    this.Models.Add(newModel);

                    modelId = this.Models.GetByName(newModel.Name).Id;
                }
                else
                {
                    modelId = model.Id;
                }
            }

            return modelId;
        }

        private List<int> CheckAndGetBodyTypeId(List<BodyTypeJsonInfoModel> bodyTypes)
        {
            List<int> bodyTypeIds = new List<int>();
            foreach (var bodyTypeJsonModel in bodyTypes)
            {
                string code = bodyTypeJsonModel.Code;
                VehicleBodyType bodyType = this.Bodytypes.GetByCode(code);
                if (bodyType == null)
                {
                    VehicleBodyType newbodyType = this.Mapper.Map<VehicleBodyType>(bodyTypeJsonModel);
                    this.Bodytypes.Add(newbodyType);
                }

                int bodyTypeIdToAdd = this.Bodytypes.GetByCode(code).Id;
                if (!bodyTypeIds.Contains(bodyTypeIdToAdd))
                {
                    bodyTypeIds.Add(bodyTypeIdToAdd);
                }
            }

            return bodyTypeIds;
        }
    }
}
