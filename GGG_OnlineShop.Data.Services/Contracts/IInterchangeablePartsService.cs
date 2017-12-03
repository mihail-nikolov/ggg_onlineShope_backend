namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Linq;

    public interface IVehicleInterchangeablePartsService : IBaseDataService<VehicleGlassInterchangeablePart>
    {
        VehicleGlassInterchangeablePart GetByEuroCode(string euroCode);

        // oes is not unique - several glasses could have 1 oes
        IQueryable<VehicleGlassInterchangeablePart> GetByOesCode(string oesCode);

        VehicleGlassInterchangeablePart GetByMaterialNumber(string materialNumber);

        VehicleGlassInterchangeablePart GetByLocalCode(string localCode);

        VehicleGlassInterchangeablePart GetByScanCode(string scanCode);

        VehicleGlassInterchangeablePart GetByNagsCode(string nagsCode);

        VehicleGlassInterchangeablePart GetInterchangeablePart(string euroCode, string materialNumber, string localCode,
                                                               string scanCode, string nagsCode);
    }
}