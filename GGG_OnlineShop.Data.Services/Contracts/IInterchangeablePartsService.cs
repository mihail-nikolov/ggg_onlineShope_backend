namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;

    public interface IVehicleInterchangeablePartsService : IBaseDataService<VehicleGlassInterchangeablePart>
    {
        VehicleGlassInterchangeablePart GetByEuroCode(string euroCode);

        VehicleGlassInterchangeablePart GetByOesCode(string oesCode);

        VehicleGlassInterchangeablePart GetByMaterialNumber(string materialNumber);

        VehicleGlassInterchangeablePart GetByLocalCode(string localCode);

        VehicleGlassInterchangeablePart GetByScanCode(string scanCode);

        VehicleGlassInterchangeablePart GetByNagsCode(string nagsCode);
    
        VehicleGlassInterchangeablePart GetInterchangeablePart(string euroCode, string oesCode,
                                                               string materialNumber, string localCode,
                                                               string scanCode, string nagsCode);
    }
}