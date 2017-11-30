namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductQuantitiesService : IProductQuantitiesService
    {
        public ProductQuantitiesService(IGoodsService goods, IStoreService store, IGoodGroupsService goodGroups)
        {
            this.Goods = goods;
            this.Store = store;
            this.GoodGroups = goodGroups;
        }

        protected IGoodsService Goods { get; set; }

        protected IStoreService Store { get; set; }

        protected IGoodGroupsService GoodGroups { get; set; }

        public IDictionary<string, int> GetQuantitiesByCode(string code)
        {
            Dictionary<string, int> productQuantities = new Dictionary<string, int>();

            var goods = this.Goods.GetAllByCode(code);
            var goodGroupIds = goods.Select(x => x.GroupID).ToList();

            var goodGroups = this.GoodGroups.GetGoodGroupsByIds(goodGroupIds);

            var groupIdNameDictionary = new Dictionary<int?, string>();

            foreach (var goodGroup in goodGroups)
            {
                groupIdNameDictionary[goodGroup.ID] = goodGroup.Name;
            }

            foreach (var good in goods)
            {
                var quantities = this.Store.GetAllByGoodId(good.ID);
                var groupName = groupIdNameDictionary[good.GroupID];

                foreach (var item in quantities)
                {
                    if (productQuantities.ContainsKey(groupName))
                    {
                        productQuantities[groupName] += (int)item.Qtty;
                    }
                    else
                    {
                        productQuantities[groupName] = (int)item.Qtty;
                    }
                }
            }

            return productQuantities;
        }
    }
}
