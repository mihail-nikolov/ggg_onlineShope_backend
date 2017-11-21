namespace GGG_OnlineShop.InternalApiDB.Data
{
    using System.Data.Entity;
    using Models;
    using System.Data.Entity.Infrastructure;

    public interface IInternalApiDbContext
    {
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        void Dispose();

        IDbSet<User> Users { get; set; }
    }
}
