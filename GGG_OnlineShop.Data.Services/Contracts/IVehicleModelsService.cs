namespace GGG_OnlineShop.Data.Services.Contracts
{
    using GGG_OnlineShop.InternalApiDB.Models;

    public interface IVehicleModelsService : IBaseDataService<VehicleModel>
    {
        VehicleModel GetByName(string modelName);
    }
}
