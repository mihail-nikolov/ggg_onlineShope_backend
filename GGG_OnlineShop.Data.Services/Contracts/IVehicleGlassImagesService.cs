namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;

    public interface IVehicleGlassImagesService : IBaseDataService<VehicleGlassImage>
    {
        VehicleGlassImage GetByCaption(string caption);

        VehicleGlassImage GetByOriginalId(int originalId);
    }
}
