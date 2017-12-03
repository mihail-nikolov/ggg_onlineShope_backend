﻿namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;

    public class VehicleBodyTypesService : BaseDataService<VehicleBodyType>, IVehicleBodyTypesService
    {
        public VehicleBodyTypesService(IInternalDbRepository<VehicleBodyType> dataSet) : base(dataSet)
        {
        }

        public VehicleBodyType GetByCode(string code)
        {
            var bodyType = this.Data.All().Where(b => b.Code.ToLower() == code.ToLower()).First();
            return bodyType;
        }
    }
}
