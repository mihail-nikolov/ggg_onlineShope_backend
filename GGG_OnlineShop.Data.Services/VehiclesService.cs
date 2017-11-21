namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using System.Linq;
    using Common;
    using System.Collections.Generic;

    public class VehiclesService : BaseDataService<Vehicle>, IVehiclesService
    {
        public VehiclesService(IDbRepository<Vehicle> dataSet) : base(dataSet)
        {
        }

        public IQueryable<Vehicle> GetAllByMakeId(int makeId)
        {
            var vehicles = this.Data.All().Where(v => v.MakeId == makeId);
            return vehicles;
        }

        public IQueryable<Vehicle> GetAllByMakeAndModelIds(int makeId, int? modelId)
        {
            var vehicles = this.Data.All().Where(v =>
                                              ((v.MakeId == makeId) &&
                                               (v.ModelId == modelId)));
            return vehicles;
        }

        // TODO think about using Iqueriably everywhere, because of mapping later
        public Vehicle GetVehicleByMakeModelAndBodyTypeIds(int makeId, int? modelId, int? bodyTypeId)
        {
            var vehicle = this.Data.All().Where(v =>
                                              ((v.MakeId == makeId) &&
                                               (v.ModelId == modelId) &&
                                               (v.BodyTypeId == bodyTypeId))).FirstOrDefault();
            return vehicle;
        }

        public List<int?> GetModelIdsByMakeId(int makeId)
        {
            var modelIds = this.GetAllByMakeId(makeId).Select(o => o.ModelId).Distinct().ToList();
            return modelIds;
        }

        public List<int?> GetBodyTypeIdsByModelIdAndMakeId(int makeId, int? modelId)
        {
            var bodyTypeIds = this.GetAllByMakeAndModelIds(makeId, modelId).Select(o => o.BodyTypeId).Distinct().ToList();
            return bodyTypeIds;
        }

        public IQueryable<VehicleGlass> GetApplicableGLasses(Vehicle vehicle)
        {
            var glasses = vehicle.VehicleGlasses.AsQueryable();
            return glasses;
        }

        public IQueryable<VehicleGlass> GetApplicableGLassesByProductType(Vehicle vehicle, string productType)
        {
            var glasses = vehicle.VehicleGlasses.AsQueryable().Where(g => g.ProductType == productType);
            return glasses;
        }
    }
}
