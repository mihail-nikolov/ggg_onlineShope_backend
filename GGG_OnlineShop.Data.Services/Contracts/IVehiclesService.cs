namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface IVehiclesService : IBaseDataService<Vehicle>
    {
        IQueryable<Vehicle> GetAllByMakeId(int makeId);

        IQueryable<Vehicle> GetAllByMakeAndModelIds(int makeId, int? modelId);

        Vehicle GetVehicleByMakeModelAndBodyTypeIds(int makeId, int? modelId, int? bodyTypeId);

        List<int?> GetModelIdsByMakeId(int makeId);

        List<int?> GetBodyTypeIdsByModelIdAndMakeId(int makeId, int? modelId);

        IQueryable<VehicleGlass> GetApplicableGLasses(Vehicle vehicle);

        IQueryable<VehicleGlass> GetApplicableGLassesByProductType(Vehicle vehicle, string productType);
    }
}
