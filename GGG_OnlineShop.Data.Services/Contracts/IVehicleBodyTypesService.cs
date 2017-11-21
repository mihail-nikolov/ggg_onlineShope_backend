namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;

    public interface IVehicleBodyTypesService : IBaseDataService<VehicleBodyType>
    {
        VehicleBodyType GetByCode(string code);
    }
}
