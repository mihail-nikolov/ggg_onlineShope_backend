namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using Common;
    using InternalApiDB.Models;
    using System.Linq;
    using Base;

    public class VehicleGlassCharacteristicsService : BaseDataService<VehicleGlassCharacteristic>, IVehicleGlassCharacteristicsService
    {
        public VehicleGlassCharacteristicsService(IInternalDbRepository<VehicleGlassCharacteristic> dataSet) : base(dataSet)
        {
        }

        public VehicleGlassCharacteristic GetByName(string name)
        {
            var glassCharacteristic = this.Data.All().Where(c => c.Name.ToLower() == name.ToLower()).FirstOrDefault();
            return glassCharacteristic;
        }
    }
}
