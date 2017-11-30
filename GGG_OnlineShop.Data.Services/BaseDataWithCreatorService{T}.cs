namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using InternalApiDB.Models.Base;
    using Common;
    using System;
    using System.Linq;
    using GGG_OnlineShop.Common;

    public class BaseDataWithCreatorService<T> : BaseDataService<T>, IBaseDataWithCreatorService<T>
       where T : class, IDeletableEntity, IAuditInfo, IEntityWithCreator
    {
        public BaseDataWithCreatorService(IInternalDbRepository<T> dataSet, IInternalDbRepository<User> users)
            : base(dataSet)
        {
            this.Users = users;
        }

        protected IInternalDbRepository<User> Users { get; set; }

        public IQueryable<T> GetAllByUser(string userId)
        {
            return this.Data
                .All()
                .Where(x => x.UserId == userId);
        }

        public virtual void Delete(object id, string userId)
        {
            var user = this.Users.GetById(userId);
            var isAdmin = user.Roles.Any(x => x.RoleId == GlobalConstants.AdministratorRoleName);
            var entity = this.Data.GetById(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"No entity with provided id ({id}) found.");
            }

            if (entity.UserId != userId && !isAdmin)
            {
                throw new InvalidOperationException("Cannot delete entity. Unauthorized request.");
            }

            this.Data.Delete(entity);
            this.Data.Save();
        }
    }
}
