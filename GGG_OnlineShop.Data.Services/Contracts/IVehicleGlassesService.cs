namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Linq;

    public interface IVehicleGlassesService : IBaseDataService<VehicleGlass>
    {
        VehicleGlass GetByEuroCode(string euroCode);

        IQueryable<VehicleGlass> GetGlassesByEuroCode(string euroCode);

        // oes is not unique - several glasses could have 1 oes
        IQueryable<VehicleGlass> GetByOesCode(string oesCode);

        VehicleGlass GetByMaterialNumber(string materialNumber);

        VehicleGlass GetByLocalCode(string localCode);

        VehicleGlass GetByIndustryCode(string industryCode);

        VehicleGlass GetGlass(string euroCode, string materialNumber, string industryCode, string localCode);

        IQueryable<VehicleGlassAccessory> GetAccessories(int glassId);

        IQueryable<VehicleGlass> GetByRandomCode(string code);

        // method is used to get the product code by which we will search in SkladProDb
        string GetCode(VehicleGlass product);

        // this method will be used to get optimize filling in the DB
        // will have all unique codes from Db and when a new glass is passed the code will be checked in this codes collection
        IQueryable<string> GetAllUniqueCodesFromDb();
    }
}
