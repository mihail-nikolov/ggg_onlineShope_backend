namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models.Base;
    using System.Linq;

    public interface IBaseDataService<T>
    where T : class, IDeletableEntity, IAuditInfo
    {
        void Add(T item);

        void Delete(object id);

        IQueryable<T> GetAll();

        T GetById(object id);

        void Save();

        void Dispose();
    }
}
