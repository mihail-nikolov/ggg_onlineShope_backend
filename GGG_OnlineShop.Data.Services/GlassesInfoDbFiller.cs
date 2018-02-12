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

    public class GlassesInfoDbFiller : IGlassesInfoDbFiller
    {
        private string solutionDirectory;
        private string errorsfilePathToWrite;
        private string parsedGlassesInfofilePath;
        private string defaultJsonFilePathToRead;

        private ISet<string> GlassesCodesProjectionFromDb;

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
                                   IReader reader,
                                   ISolutionBaseConfig solutionConfig
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
            this.SolutionConfig = solutionConfig;

            solutionDirectory = this.SolutionConfig.GetSolutionPath();
            errorsfilePathToWrite = $@"{solutionDirectory}\DbFillInErorrs_{DateTime.Now.ToString("ddMMyy_HHmm")}.txt";
            parsedGlassesInfofilePath = $@"{solutionDirectory}\DbFillInInfo_{DateTime.Now.ToString("ddMMyy_HHmm")}.txt";
            defaultJsonFilePathToRead = $@"{solutionDirectory}\ggg\products_test.json";
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

        protected ISolutionBaseConfig SolutionConfig { get; set; }

        //------------------------------ Public FillInfo ----------------------------------------------

        public void FillInfo(IList<GlassJsonInfoModel> glasses, string passedFile = "")
        {
            try
            {
                CreateGlassesDictionary();

                if (glasses != null)
                {
                    this.AddArrayOfGlassJsonInfoModels(glasses);
                }
                else
                {
                    this.AddArrayOfJsonsFromFileToDb(passedFile);
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
                    this.Logger.LogError($"Error while adding GlassJsonInfoModel --{glass.ToString()}-- under index ({i}). Exception message: {e.Message}",
                                       errorsfilePathToWrite);
                }
            }
        }

        private void AddArrayOfJsonsFromFileToDb(string passedFile)
        {
            string fileToUse = defaultJsonFilePathToRead;

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

        private bool IsGlassAlreadyAdded(string eurocode, string materialnumber, string industrycode, string localcode)
        {
            bool answer = false;

            if (!string.IsNullOrEmpty(eurocode))
            {
                if (this.GlassesCodesProjectionFromDb.Contains(eurocode)) answer = true;
            }
            else if (!string.IsNullOrEmpty(materialnumber))
            {
                if (this.GlassesCodesProjectionFromDb.Contains(materialnumber)) answer = true;
            }
            else if (!string.IsNullOrEmpty(industrycode))
            {
                if (this.GlassesCodesProjectionFromDb.Contains(industrycode)) answer = true;
            }
            else
            {
                if (this.GlassesCodesProjectionFromDb.Contains(localcode)) answer = true;
            }

            return answer;
        }

        private void CreateGlassesDictionary()
        {
            GlassesCodesProjectionFromDb = new HashSet<string>();
            var codes = this.Glasses.GetAllUniqueCodesFromDb();

            foreach (var code in codes)
            {
                GlassesCodesProjectionFromDb.Add(code);
            }
        }

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
                return new JArray();
            }
        }

        //------------------------------ Glass Info Splitters----------------------------------------------

        private bool CheckGlassJsonInfoModelAndAddToDb(GlassJsonInfoModel glassInfoModel)
        {
            bool added = false;

            if (!IsGlassAlreadyAdded(glassInfoModel.EuroCode,
                                     glassInfoModel.MaterialNumber,
                                     glassInfoModel.LocalCode,
                                     glassInfoModel.IndustryCode))
            {
                // Get car IDs or create ones and all related entities
                int makeId = this.CheckAndGetMakeId(glassInfoModel.Make);
                int? modelId = this.CheckAndGetModelId(glassInfoModel.Model);
                var bodyTypeIds = this.CheckAndGetBodyTypeId(glassInfoModel.BodyTypes);
                var vehicles = this.CheckAndGetVehicleIds(makeId, modelId, bodyTypeIds);

                var characteristics = this.HandleCharacteristics(glassInfoModel.Characteristics);
                var images = this.HandleImages(glassInfoModel.Images);
                var interchaneableParts = this.HandleInterchaneableParts(glassInfoModel.InterchangeableParts);
                var superceeds = this.HandleAccessories(glassInfoModel.Accessories);
                var accessories = this.HandleSuperceeds(glassInfoModel.Superceeds);

                this.CreateNewGlassWithAllRelations(glassInfoModel, vehicles, characteristics, images, interchaneableParts, accessories, superceeds);

                added = true;
            }

            return added;
        }

        private void CreateNewGlassWithAllRelations(GlassJsonInfoModel glassInfoModel,
                                                    List<Vehicle> vehicles, List<VehicleGlassCharacteristic> characteristics,
                                                    List<VehicleGlassImage> images, List<VehicleGlassInterchangeablePart> interchaneableParts,
                                                    List<VehicleGlassSuperceed> superceeds, List<VehicleGlassAccessory> accessories)
        {
            VehicleGlass newGlass = this.Mapper.Map<VehicleGlass>(glassInfoModel);
            this.Glasses.Add(newGlass);

            var glass = this.Glasses.GetGlass(newGlass.EuroCode, newGlass.MaterialNumber,
                                              newGlass.IndustryCode, newGlass.LocalCode);

            // -------------- for optimization
            var uniqueCode = this.Glasses.GetCode(glass);
            this.GlassesCodesProjectionFromDb.Add(uniqueCode);
            // -------------- for optimization

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

        // parses the superceed for a given product.
        // if the same already exists -> return them 
        // else adds them -> return them
        private List<VehicleGlassSuperceed> HandleSuperceeds(List<SuperceedJsonInfoModel> superceeds)
        {
            List<VehicleGlassSuperceed> vehicleSupeceeds = new List<VehicleGlassSuperceed>();
            foreach (var superceed in superceeds)
            {
                var superceedFromDb = this.Superceeds.GetSuperceed(superceed.OldEuroCode,
                                                                   superceed.OldLocalCode,
                                                                   superceed.OldMaterialNumber);
                if (superceedFromDb == null)
                {
                    var newSuperceed = this.Mapper.Map<VehicleGlassSuperceed>(superceed);

                    this.Superceeds.Add(newSuperceed);
                    var superceedToAdd = this.Superceeds.GetSuperceed(superceed.OldEuroCode,
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

        // parses the accessories for a given product.
        // if the same already exists -> return them 
        // else adds them -> return them
        private List<VehicleGlassAccessory> HandleAccessories(List<AccessoryJsonInfoModel> accessories)
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

        // parses the interchangeable parts for a given product.
        // if the same already exists -> return them 
        // else adds them -> return them
        private List<VehicleGlassInterchangeablePart> HandleInterchaneableParts(List<InterchangeableParJsonInfoModel> interchangeableParts)
        {
            List<VehicleGlassInterchangeablePart> vehicleInterchaneableParts = new List<VehicleGlassInterchangeablePart>();
            foreach (var interchangeablePart in interchangeableParts)
            {
                var interchangeablePartFromDb = this.InterchangeableParts.GetInterchangeablePart(interchangeablePart.EuroCode,
                                                                                                 interchangeablePart.MaterialNumber,
                                                                                                 interchangeablePart.LocalCode,
                                                                                                 interchangeablePart.ScanCode,
                                                                                                 interchangeablePart.NagsCode);
                if (interchangeablePartFromDb == null)
                {
                    var newInterchangeablePart = this.Mapper.Map<VehicleGlassInterchangeablePart>(interchangeablePart);

                    this.InterchangeableParts.Add(newInterchangeablePart);
                    var interchangeableGlassToAdd = this.InterchangeableParts.GetInterchangeablePart(interchangeablePart.EuroCode,
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

        // parses the images for a given product.
        // if the same already exists -> return them 
        // else adds them -> return them
        private List<VehicleGlassImage> HandleImages(List<ImageJsonInfoModel> images)
        {
            List<VehicleGlassImage> imagesToadd = new List<VehicleGlassImage>();
            foreach (var image in images)
            {
                VehicleGlassImage existingImage = this.Images.GetByOriginalId(image.Id);
                if (existingImage == null)
                {
                    VehicleGlassImage newImage = this.Mapper.Map<VehicleGlassImage>(image);
                    this.Images.Add(newImage);
                    imagesToadd.Add(newImage);
                }
                else
                {
                    imagesToadd.Add(existingImage);
                }
            }

            return imagesToadd;
        }

        // parses the GetCharacteristics for a given product.
        // if the same already exists -> return them 
        // else adds them -> return them
        private List<VehicleGlassCharacteristic> HandleCharacteristics(List<string> characteristics)
        {
            List<VehicleGlassCharacteristic> characteristicsToAdd = new List<VehicleGlassCharacteristic>();
            foreach (var characteristicName in characteristics)
            {
                VehicleGlassCharacteristic existingcharacteristic = this.Characteristics.GetByName(characteristicName);
                if (existingcharacteristic == null)
                {
                    VehicleGlassCharacteristic newCharacteristic = new VehicleGlassCharacteristic() { Name = characteristicName };

                    this.Characteristics.Add(newCharacteristic);
                    characteristicsToAdd.Add(newCharacteristic);
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

                makeId = newMake.Id;
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

                    modelId = newModel.Id;
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
                    bodyType = this.Mapper.Map<VehicleBodyType>(bodyTypeJsonModel);
                    this.Bodytypes.Add(bodyType);
                }

                int bodyTypeIdToAdd = bodyType.Id;
                if (!bodyTypeIds.Contains(bodyTypeIdToAdd))
                {
                    bodyTypeIds.Add(bodyTypeIdToAdd);
                }
            }

            return bodyTypeIds;
        }
    }
}
