namespace GGG_OnlineShop.Data.Common
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using InternalApiDB.Data;
    using InternalApiDB.Models.Base;
    using GGG_OnlineShop.Common;

    public class InternalDbRepository<T> : IInternalDbRepository<T>
        where T : class, IAuditInfo, IDeletableEntity
    {
        public InternalDbRepository(IInternalApiDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException(GlobalConstants.DBContextRequiredErrorMessage, nameof(context));
            }

            this.Context = context;
            this.DbSet = Context.Set<T>();
        }

        private IDbSet<T> DbSet { get; }

        private IInternalApiDbContext Context { get; }

        public IQueryable<T> All()
        {
            return this.DbSet.Where(x => !x.IsDeleted);
        }

        public IQueryable<T> AllWithDeleted()
        {
            return this.DbSet;
        }

        public T GetById(object id)
        {
            var item = this.DbSet.Find(id);
            if (item == null || item.IsDeleted)
            {
                return null;
            }

            return item;
        }

        public void Add(T entity)
        {
            this.DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
        }

        public void HardDelete(T entity)
        {
            this.DbSet.Remove(entity);
        }

        public void Save()
        {
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}