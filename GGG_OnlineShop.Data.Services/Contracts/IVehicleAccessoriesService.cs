namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;

    public interface IVehicleAccessoriesService: IBaseDataService<VehicleGlassAccessory>
    {
        VehicleGlassAccessory GetByIndustryCode(string industryCode);

        VehicleGlassAccessory GetByMaterialNumber(string materialNumber);

        VehicleGlassAccessory GetAccessory(string industryCode, string materialNumber);
    }
}
