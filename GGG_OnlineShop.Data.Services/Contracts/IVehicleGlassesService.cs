namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Linq;

    public interface IVehicleGlassesService : IBaseDataService<VehicleGlass>
    {
        VehicleGlass GetByEuroCode(string euroCode);

        // oes is not unique - several glasses could have 1 oes
        IQueryable<VehicleGlass> GetByOesCode(string oesCode);

        VehicleGlass GetByMaterialNumber(string materialNumber);

        VehicleGlass GetByLocalCode(string localCode);

        VehicleGlass GetByIndustryCode(string industryCode);

        VehicleGlass GetGlass(string euroCode, string materialNumber, string localCode, string industryCode);

        IQueryable<VehicleGlassAccessory> GetAccessories(int glassId);

        IQueryable<VehicleGlass> GetByRandomCode(string code);

        string GetCode(VehicleGlass product);
    }
}
