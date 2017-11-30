namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using System.Collections.Generic;

    public class ProductQuantitiesService : IProductQuantitiesService
    {
        public ProductQuantitiesService(IGoodsService goods, IStoreService store)
        {
            this.Goods = goods;
            this.Store = store;
        }

        protected IGoodsService Goods { get; set; }

        protected IStoreService Store { get; set; }

        public IDictionary<string, int> GetQuantitiesByCode(string code)
        {
            Dictionary<string, int> productQuantities = new Dictionary<string, int>();

            var goods = this.Goods.GetAllByCode(code);
            foreach (var good in goods)
            {
                var quantities = this.Store.GetAllByGoodId(good.ID);
                foreach (var item in quantities)
                {
                    if (productQuantities.ContainsKey(good.Name))
                    {
                        productQuantities[good.Name] += (int)item.Qtty;
                    }
                    else
                    {
                        productQuantities[good.Name] = (int)item.Qtty;
                    }
                }
            }

            return productQuantities;
        }
    }
}
