﻿namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using Common;
    using InternalApiDB.Models;
    using System.Linq;

    public class VehicleModelsService : BaseDataService<VehicleModel>, IVehicleModelsService
    {
        public VehicleModelsService(IDbRepository<VehicleModel> dataSet) : base(dataSet)
        {
        }

        public VehicleModel GetByName(string modelName)
        {
            var vehicleModel = this.Data.All().Where(m =>m.Name.ToLower() == modelName.ToLower()).FirstOrDefault();
            return vehicleModel;
        }
    }
}
