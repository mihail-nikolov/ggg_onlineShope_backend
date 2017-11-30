namespace GGG_OnlineShop.Data.Common
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using SkladProDB.Data;
    using GGG_OnlineShop.Common;

    public class ExternalDbRepository<T> : IExternalDbRepository<T>
        where T : class
    {
        public ExternalDbRepository(IExternalApiDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException(GlobalConstants.DBContextRequiredErrorMessage, nameof(context));
            }

            this.Context = context;
            this.DbSet = Context.Set<T>();
        }

        private IDbSet<T> DbSet { get; }

        private IExternalApiDbContext Context { get; }

        public IQueryable<T> All()
        {
            return this.DbSet;
        }

        public T GetById(object id)
        {
            var item = this.DbSet.Find(id);

            return item;
        }
    }
}
