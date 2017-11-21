namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;

    public class VehicleMakesService : BaseDataService<VehicleMake>, IVehicleMakesService
    {
        public VehicleMakesService(IDbRepository<VehicleMake> dataSet) : base(dataSet)
        {
        }

        public VehicleMake GetByName(string vehicleMake)
        {
            var make = this.Data.All().Where(m => m.Name.ToLower() == vehicleMake.ToLower()).FirstOrDefault();
            return make;
        }
    }
}
