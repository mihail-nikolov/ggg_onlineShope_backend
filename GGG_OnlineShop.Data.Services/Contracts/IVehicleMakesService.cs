namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;

    public interface IVehicleMakesService : IBaseDataService<VehicleMake>
    {
        VehicleMake GetByName(string vehicleMake);
    }
}
