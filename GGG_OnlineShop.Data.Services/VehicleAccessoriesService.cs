namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;

    public class VehicleAccessoriesService : BaseDataService<VehicleGlassAccessory>, IVehicleAccessoriesService
    {
        public VehicleAccessoriesService(IInternalDbRepository<VehicleGlassAccessory> dataSet) : base(dataSet)
        {
        }

        public VehicleGlassAccessory GetAccessory(string industryCode, string materialNumber)
        {
            var glass = this.GetAccessoryByIndustryCode(industryCode);
            if (glass == null)
            {
                glass = this.GetAccessoryByMaterialNumber(materialNumber);
            }

            return glass;
        }

        public VehicleGlassAccessory GetAccessoryByMaterialNumber(string materialNumber)
        {
            return this.Data.All().Where(a => a.MaterialNumber.ToLower() == materialNumber.ToLower()).First();
        }

        public VehicleGlassAccessory GetAccessoryByIndustryCode(string industryCode)
        {
            return this.Data.All().Where(a => a.IndustryCode.ToLower() == industryCode.ToLower()).First();
        }
    }
}
