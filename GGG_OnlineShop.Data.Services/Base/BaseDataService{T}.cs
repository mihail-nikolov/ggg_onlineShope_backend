﻿namespace GGG_OnlineShop.Data.Services.Base
{
    using Contracts;
    using Common;
    using InternalApiDB.Models.Base;
    using System;
    using System.Linq;

    public abstract class BaseDataService<T> : IBaseDataService<T>
        where T : class, IDeletableEntity, IAuditInfo
    {
        public BaseDataService(IInternalDbRepository<T> dataSet)
        {
            this.Data = dataSet;
        }

        protected IInternalDbRepository<T> Data { get; set; }

        public virtual void Add(T item)
        {
            this.Data.Add(item);
            this.Data.Save();
        }

        public virtual void Delete(object id)
        {
            var entity = this.Data.GetById(id);
            if (entity == null)
            {
                throw new InvalidOperationException("No entity with provided id found.");
            }

            this.Data.Delete(entity);
            this.Data.Save();
        }

        public virtual IQueryable<T> GetAll()
        {
            return this.Data.All();
        }

        public virtual T GetById(object id)
        {
            return this.Data.GetById(id);
        }

        public virtual void Save()
        {
            this.Data.Save();
        }

        public void Dispose()
        {
            this.Data.Dispose();
        }
    }
}
