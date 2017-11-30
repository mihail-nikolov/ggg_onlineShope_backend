namespace GGG_OnlineShop.Data.Common
{
    using System.Linq;

    public interface IExternalDbRepository<T> 
            where T : class
    {
        IQueryable<T> All();

        T GetById(object id);
    }
}
