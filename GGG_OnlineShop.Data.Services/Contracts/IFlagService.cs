namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using InternalApiDB.Models.Enums;

    public interface IFlagService : IBaseDataService<Flag>
    {
        bool GetFlagValue(FlagType name);
    }
}
