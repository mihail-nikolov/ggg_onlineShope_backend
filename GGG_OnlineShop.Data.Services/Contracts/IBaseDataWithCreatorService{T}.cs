namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models.Base;
    using System.Linq;

    public interface IBaseDataWithCreatorService<T> : IBaseDataService<T>
    where T : class, IDeletableEntity, IAuditInfo
    {
        void Delete(object id, string userId);

        IQueryable<T> GetAllByUser(string userId);
    }
}
