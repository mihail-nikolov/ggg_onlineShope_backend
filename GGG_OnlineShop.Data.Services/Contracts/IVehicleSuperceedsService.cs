namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Linq;

    public interface IVehicleSuperceedsService : IBaseDataService<VehicleGlassSuperceed>
    {
        VehicleGlassSuperceed GetByOldEuroCode(string oldEuroCode);

        // oes is not unique - several glasses could have 1 oes
        IQueryable<VehicleGlassSuperceed> GetByOldOesCode(string oldOesCode);

        VehicleGlassSuperceed GetByOldLocalCode(string oldLocalCode);

        VehicleGlassSuperceed GetByOldMaterialNumber(string oldMaterialNumber);

        VehicleGlassSuperceed GetSuperceed(string oldEuroCode, string oldOesCode,
                                           string oldLocalCode, string oldMaterialNumber);
    }
}
