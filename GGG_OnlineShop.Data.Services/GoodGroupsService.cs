namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using SkladProDB.Models;
    using Common;
    using System.Collections.Generic;

    public class GoodGroupsService : BaseDataExternalService<GoodsGroup>, IGoodGroupsService
    {
        public GoodGroupsService(IExternalDbRepository<GoodsGroup> dataSet) : base(dataSet)
        {
        }

        public ICollection<GoodsGroup> GetGoodGroupsByIds(IEnumerable<int?> goodGroupIds)
        {
            List<GoodsGroup> goodGroups = new List<GoodsGroup>();
            foreach (var id in goodGroupIds)
            {
                goodGroups.Add(this.GetById(id));
            }

            return goodGroups;
        }
    }
}
