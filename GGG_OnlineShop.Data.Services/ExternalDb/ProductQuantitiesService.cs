namespace GGG_OnlineShop.Data.Services.ExternalDb
{
    using Contracts;
    using GGG_OnlineShop.Common;
    using InternalApiDB.Models;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductQuantitiesService : IProductQuantitiesService
    {
        public ProductQuantitiesService(IGoodsService goods, IStoreService store, IGoodGroupsService goodGroups, IObjectsService objectsService)
        {
            this.Goods = goods;
            this.ObjectsService = objectsService;
            this.GoodGroups = goodGroups;
            this.Store = store;
        }

        protected IGoodsService Goods { get; set; }

        protected IStoreService Store { get; set; }

        protected IGoodGroupsService GoodGroups { get; set; }

        protected IObjectsService ObjectsService { get; set; }

        public IEnumerable<ProductInfoResponseModel> GetPriceAndQuantitiesByCode(string code, User user)
        {
            Dictionary<string, ProductInfoResponseModel> productQuantities = new Dictionary<string, ProductInfoResponseModel>();

            var goods = this.Goods.GetAllByCode(code);
            var goodGroupIds = goods.Select(x => x.GroupID).ToList();

            var goodGroups = this.GoodGroups.GetGoodGroupsByIds(goodGroupIds);

            var groupIdNameDictionary = new Dictionary<int?, string>();

            foreach (var goodGroup in goodGroups)
            {
                groupIdNameDictionary[goodGroup.ID] = goodGroup.Name;
            }

            HashSet<string> forbiddenGroups = GetForbiddenGroups(user);
            var objects = this.ObjectsService.GetAll().ToList();
            Dictionary<int?, string> objectKeyName = new Dictionary<int?, string>();

            foreach (var obj in objects)
            {
                objectKeyName.Add(obj.ID, obj.Name);
            }

            foreach (var good in goods)
            {
                var quantities = this.Store.GetAllByGoodId(good.ID);
                var groupName = groupIdNameDictionary[good.GroupID];

                if (forbiddenGroups.Contains(groupName))
                {
                    continue;
                }

                foreach (var item in quantities)
                {
                    if (!objectKeyName.ContainsKey(item.ObjectID))
                    {
                        continue;
                    }

                    string storeName = objectKeyName[item.ObjectID];

                    if (productQuantities.ContainsKey(groupName))
                    {
                        if (productQuantities[groupName].StoreQUantities.ContainsKey(storeName))
                        {
                            productQuantities[groupName].StoreQUantities[storeName] += (int)item.Qtty;
                        }
                        else
                        {
                            productQuantities[groupName].StoreQUantities.Add(storeName, (int)item.Qtty);
                        }
                    }
                    else
                    {
                        string groupFromGoodName = string.Empty;
                        if (groupName == GlobalConstants.SharedGroup)
                        {
                            var nameWithoutSharedPrefix = good.Name.Replace("Общи ", string.Empty);
                            if (nameWithoutSharedPrefix == good.Name2)
                            {
                                // Общи glass is only added as a pattern for future db glass add
                                continue;
                            }
                            else
                            {
                                int startIndexOfDescription = good.Name.IndexOf(good.Name2);
                                groupFromGoodName = good.Name.Substring(0, startIndexOfDescription - 1).Replace("Общи ", string.Empty);
                            }
                        }

                        // show the item only if it is not a pattern (priceout2 > 0)
                        if (good.PriceOut2 > 0)
                        {
                            productQuantities[groupName] = new ProductInfoResponseModel()
                            {
                                Group = groupName,
                                StoreQUantities = new Dictionary<string, int>() { { storeName, (int)item.Qtty } },
                                Price = good.PriceOut2,
                                DescriptionWithName = good.Name,
                                DescriptionWithoutName = good.Name2,
                                GroupFromItemName = groupFromGoodName
                            };
                        }
                    }
                }
            }

            return productQuantities.Select(x => x.Value);
        }

        private HashSet<string> GetForbiddenGroups(User user)
        {
            HashSet<string> forbiddenGroups = new HashSet<string>();

            if (user != null)
            {
                if (!user.IsAGCVisible)
                {
                    forbiddenGroups.Add(GlobalConstants.AGCGroup);
                }

                if (!user.IsFuyaoVisible)
                {
                    forbiddenGroups.Add(GlobalConstants.FuyaoGroup);
                }

                if (!user.IsLamexVisible)
                {
                    forbiddenGroups.Add(GlobalConstants.LamexGroup);
                }

                if (!user.IsNordglassVisible)
                {
                    forbiddenGroups.Add(GlobalConstants.NordglassGroup);
                }

                if (!user.IsPilkingtonVisible)
                {
                    forbiddenGroups.Add(GlobalConstants.PilkingtonGroup);
                }

                if (!user.IsSaintGobainVisible)
                {
                    forbiddenGroups.Add(GlobalConstants.SaintGobainGroup);
                }

                if (!user.IsSharedVisible)
                {
                    forbiddenGroups.Add(GlobalConstants.SharedGroup);
                }

                if (!user.IsYesglassVisible)
                {
                    forbiddenGroups.Add(GlobalConstants.YesglassGroup);
                }
            }

            return forbiddenGroups;
        }
    }
}
