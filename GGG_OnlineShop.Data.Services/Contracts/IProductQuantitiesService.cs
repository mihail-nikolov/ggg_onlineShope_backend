namespace GGG_OnlineShop.Data.Services.Contracts
{
    using System.Collections.Generic;

    public interface IProductQuantitiesService
    {
        IDictionary<string, int> GetQuantitiesByCode(string code);
    }
}
