namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;
    using Base;

    public class VehicleAccessoriesService : BaseDataService<VehicleGlassAccessory>, IVehicleAccessoriesService
    {
        public VehicleAccessoriesService(IInternalDbRepository<VehicleGlassAccessory> dataSet) : base(dataSet)
        {
        }

        public VehicleGlassAccessory GetAccessory(string industryCode, string materialNumber)
        {
            var accessory = this.GetByIndustryCode(industryCode);
            if (accessory == null)
            {
                accessory = this.GetByMaterialNumber(materialNumber);
            }

            return accessory;
        }

        public VehicleGlassAccessory GetByMaterialNumber(string materialNumber)
        {
            return this.Data.All().Where(a => a.MaterialNumber.ToLower() == materialNumber.ToLower()).FirstOrDefault();
        }

        public VehicleGlassAccessory GetByIndustryCode(string industryCode)
        {
            return this.Data.All().Where(a => a.IndustryCode.ToLower() == industryCode.ToLower()).FirstOrDefault();
        }
    }
}
