﻿namespace GGG_OnlineShop.Data.Services.ExternalDb
{
    using System.Linq;
    using Contracts;
    using Common;
    using SkladProDB.Models;
    using Base;

    public class StoreService : BaseDataExternalService<Store>, IStoreService
    {
        public StoreService(IExternalDbRepository<Store> dataSet) : base(dataSet)
        {
        }

        public IQueryable<Store> GetAllByGoodId(int goodId)
        {
            return this.Data.All().Where(a => a.GoodID == goodId);
        }
    }
}
