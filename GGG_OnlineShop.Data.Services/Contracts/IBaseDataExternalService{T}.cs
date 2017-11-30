namespace GGG_OnlineShop.Data.Services.Contracts
{
    using System.Linq;

    public interface IBaseDataExternalService<T>
        where T : class
    {
        IQueryable<T> GetAll();

        T GetById(object id);
    }
}
