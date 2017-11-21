namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;

    public interface IVehicleAccessoriesService: IBaseDataService<VehicleGlassAccessory>
    {
        VehicleGlassAccessory GetAccessoryByIndustryCode(string industryCode);

        VehicleGlassAccessory GetAccessoryByMaterialNumber(string materialNumber);

        VehicleGlassAccessory GetAccessory(string industryCode, string materialNumber);
    }
}
