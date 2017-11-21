namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;

    public interface IVehicleGlassCharacteristicsService : IBaseDataService<VehicleGlassCharacteristic>
    {
        VehicleGlassCharacteristic GetByName(string name);
    }
}
