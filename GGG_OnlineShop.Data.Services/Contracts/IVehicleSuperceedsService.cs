namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;

    public interface IVehicleSuperceedsService : IBaseDataService<VehicleGlassSuperceed>
    {
        VehicleGlassSuperceed GetByOldEuroCode(string oldEuroCode);

        VehicleGlassSuperceed GetByOldOesCode(string oldOesCode);

        VehicleGlassSuperceed GetByOldLocalCode(string oldLocalCode);

        VehicleGlassSuperceed GetByOldMaterialNumber(string oldMaterialNumber);

        VehicleGlassSuperceed GetSuperceed(string oldEuroCode, string oldOesCode,
                                           string oldLocalCode, string oldMaterialNumber);
    }
}
