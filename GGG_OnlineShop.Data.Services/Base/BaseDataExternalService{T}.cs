namespace GGG_OnlineShop.Data.Services.Base
{
    using Common;
    using Contracts;
    using System.Linq;

    public abstract class BaseDataExternalService<T> : IBaseDataExternalService<T>
            where T : class
    {
        public BaseDataExternalService(IExternalDbRepository<T> dataSet)
        {
            this.Data = dataSet;
        }

        protected IExternalDbRepository<T> Data { get; set; }

        public virtual IQueryable<T> GetAll()
        {
            return this.Data.All();
        }

        public virtual T GetById(object id)
        {
            return this.Data.GetById(id);
        }
    }
}
