namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;
    using Base;

    public class VehicleBodyTypesService : BaseDataService<VehicleBodyType>, IVehicleBodyTypesService
    {
        public VehicleBodyTypesService(IInternalDbRepository<VehicleBodyType> dataSet) : base(dataSet)
        {
        }

        public VehicleBodyType GetByCode(string code)
        {
            var bodyType = this.Data.All().FirstOrDefault(b => b.Code.ToLower() == code.ToLower());
            return bodyType;
        }
    }
}
