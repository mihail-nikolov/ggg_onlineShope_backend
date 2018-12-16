using GGG_OnlineShop.InternalApiDB.Models.Enums;

namespace GGG_OnlineShop.Data.Services.ExternalDb
{
    using Contracts;
    using GGG_OnlineShop.Common;
    using Models;
    using SkladProDB.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductQuantitiesService : IProductQuantitiesService
    {
        private readonly IFlagService _flagService;
        private readonly bool _publicHighCostVisible;

        public ProductQuantitiesService(IGoodsService goods, IStoreService store, IGoodGroupsService goodGroups, IObjectsService objectsService, IFlagService flagService)
        {
            _flagService = flagService;
            this.Goods = goods;
            this.ObjectsService = objectsService;
            this.GoodGroups = goodGroups;
            this.Store = store;
            _publicHighCostVisible = flagService.GetFlagValue(FlagType.ShowOnlyHighCostGroups);
        }

        protected IGoodsService Goods { get; set; }

        protected IStoreService Store { get; set; }

        protected IGoodGroupsService GoodGroups { get; set; }

        protected IObjectsService ObjectsService { get; set; }

        public IEnumerable<ProductInfoResponseModel> GetPriceAndQuantitiesByCode(string code, InternalApiDB.Models.User user)
        {
            List<ProductInfoResponseModel> productQuantities = new List<ProductInfoResponseModel>();

            // if yesglass - contains -ALT - so should remove it
            string yesglassMark = "-ALT";
            if (code.Contains(yesglassMark))
            {
                code = code.Replace(yesglassMark, "");
            }
            var goods = this.Goods.GetAllByCode(code).ToList();
            if (goods.Count == 0)
            {
                return productQuantities;
            }

            bool onlyHighCostAllowed = _publicHighCostVisible;
            if (user != null)
            {
                onlyHighCostAllowed = user.OnlyHighCostVisible; // user option will override the public one
            }

            var goodGroupIds = goods.Select(x => x.GroupID).ToList();
            var goodGroups = this.GoodGroups.GetGoodGroupsByIds(goodGroupIds);
            var groupIdNameDictionary = new Dictionary<int?, string>();
            foreach (var goodGroup in goodGroups)
            {
                groupIdNameDictionary[goodGroup.ID] = goodGroup.Name;
            }

            var objects = this.ObjectsService.GetAll().ToList();
            Dictionary<int?, string> objectKeyName = new Dictionary<int?, string>();
            foreach (var obj in objects)
            {
                if (obj.Name == "Люлин")
                {
                   continue;
                }

                string name = obj.Name == "Слатина" ? "София" : obj.Name;
                objectKeyName.Add(obj.ID, name);
            }

            foreach (var good in goods)
            {
                // show the item only if it is not a pattern (priceout2 > 0)
                if (good.PriceOut2 <= 0)
                {
                    continue;
                }

                var groupName = groupIdNameDictionary[good.GroupID];
                if (onlyHighCostAllowed && !GlobalConstants.HighCostGroups.Contains(groupName))
                {
                    continue;
                }

                string groupFromGoodName = string.Empty;
                if (groupName == GlobalConstants.SharedGroup)
                {
                    groupFromGoodName = GetGroupNameFromName(good);
                    if (string.IsNullOrEmpty(groupFromGoodName))
                    {
                        continue;
                    }
                }

                var quantities = this.Store.GetAllByGoodId(good.ID);
                foreach (var item in quantities)
                {
                    if (!objectKeyName.ContainsKey(item.ObjectID))
                    {
                        continue;
                    }

                    string storeName = objectKeyName[item.ObjectID];

                    ProductInfoResponseModel currentGoodResponse = productQuantities.FirstOrDefault(x => x.GoodId == good.ID);
                    if (currentGoodResponse != null)
                    {
                        int index = productQuantities.IndexOf(currentGoodResponse);
                        if (productQuantities[index].StoreQUantities.ContainsKey(storeName))
                        {
                            productQuantities[index].StoreQUantities[storeName] += (int)item.Qtty;
                        }
                        else
                        {
                            productQuantities[index].StoreQUantities.Add(storeName, (int)item.Qtty);
                        }
                    }
                    else
                    {
                        productQuantities.Add(new ProductInfoResponseModel()
                        {
                            GoodId = good.ID,
                            Group = groupName,
                            StoreQUantities = new Dictionary<string, int>() { { storeName, (int)item.Qtty } },
                            Price = good.PriceOut2,
                            DescriptionWithName = good.Name,
                            DescriptionWithoutName = good.Name2,
                            GroupFromItemName = groupFromGoodName
                        });
                    }
                }
            }

            return productQuantities;
        }

        private string GetGroupNameFromName(Good good)
        {
            string groupFromGoodName = string.Empty;

            var nameWithoutSharedPrefix = good.Name.Replace("Общи ", string.Empty);
            if (nameWithoutSharedPrefix != good.Name2)
            {
                int startIndexOfDescription = good.Name.IndexOf(good.Name2);
                groupFromGoodName = good.Name.Substring(0, startIndexOfDescription - 1).Replace("Общи ", string.Empty);
            }

            return groupFromGoodName;
        }
    }
}
