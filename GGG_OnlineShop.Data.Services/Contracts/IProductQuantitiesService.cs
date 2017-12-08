namespace GGG_OnlineShop.Data.Services.Contracts
{
    using ExternalDb.Models;
    using InternalApiDB.Models;
    using System.Collections.Generic;

    public interface IProductQuantitiesService
    {
        IEnumerable<GlassInfoResponseModel> GetPriceAndQuantitiesByCode(string code, User user);
    }
}
