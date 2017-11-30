namespace GGG_OnlineShop.Data.Services.Contracts
{
    using SkladProDB.Models;
    using System.Linq;

    public interface IStoreService: IBaseDataExternalService<Store>
    {
        IQueryable<Store> GetAllByGoodId(int goodId);
    }
}
