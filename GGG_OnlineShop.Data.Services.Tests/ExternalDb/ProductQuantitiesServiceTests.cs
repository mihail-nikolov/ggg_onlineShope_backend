using GGG_OnlineShop.InternalApiDB.Models.Enums;

namespace GGG_OnlineShop.Data.Services.Tests.ExternalDb
{
    using Contracts;
    using GGG_OnlineShop.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Services.ExternalDb;
    using SkladProDB.Models;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class ProductQuantitiesServiceTests
    {
        [TestMethod]
        public void GetPriceAndQuantitiesByCode_ShouldReturnEmptyList_WhenNoGoods()
        {
            string testCode = "2233AGN";

            var goodsServiceMock = new Mock<IGoodsService>();
            var goods = new List<Good>().AsQueryable();
            goodsServiceMock.Setup(x => x.GetAllByCode(testCode)).Returns(() => goods);

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            ProductQuantitiesService service =
                new ProductQuantitiesService(goodsServiceMock.Object, null, null, null, flagService.Object);

            var response = service.GetPriceAndQuantitiesByCode(testCode, null).ToList();

            Assert.AreEqual(response.Count, 0);
            goodsServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPriceAndQuantitiesByCode_ShouldReturnEmptyList_WhenPriceOut2LessThen0()
        {
            string testCode = "2233AGN";

            var goods = new List<Good>()
            {
                    new Good() { ID = 1, Code = testCode, PriceOut2 = 0, GroupID = 1 },
                    new Good() { ID = 2, Code = testCode, PriceOut2 = 0, GroupID = 2 }
            }.AsQueryable();

            var goodsServiceMock = new Mock<IGoodsService>();
            goodsServiceMock.Setup(x => x.GetAllByCode(testCode)).Returns(() => goods);


            var goodGroupsServiceMock = new Mock<IGoodGroupsService>();
            var goodGroupIds = goods.Select(x => x.GroupID).ToList();
            var goodGroups = new List<GoodsGroup>()
            {
                    new GoodsGroup() { ID = 1 },
                    new GoodsGroup() { ID = 2 }
            };
            goodGroupsServiceMock.Setup(x => x.GetGoodGroupsByIds(goodGroupIds)).Returns(() => goodGroups);

            var objectsServiceMock = new Mock<IObjectsService>();

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            ProductQuantitiesService service = new ProductQuantitiesService(goodsServiceMock.Object, null,
                                                                            goodGroupsServiceMock.Object, objectsServiceMock.Object, flagService.Object);

            var response = service.GetPriceAndQuantitiesByCode(testCode, null).ToList();

            Assert.AreEqual(response.Count, 0);
            goodsServiceMock.VerifyAll();
            goodGroupsServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPriceAndQuantitiesByCode_ShouldReturnEmptyList_WhenGoodsAreFromSharedGroupAndOnlyPattern()
        {
            string testCode = "2233AGN";

            var goods = new List<Good>()
            {
                    new Good() { ID = 1, Code = testCode, PriceOut2 = 1, GroupID = 1, Name = "Общи testDescription", Name2 = "testDescription" },
                    new Good() { ID = 2, Code = testCode, PriceOut2 = 1, GroupID = 2, Name = "Общи testDescription", Name2 = "testDescription" }
            }.AsQueryable();

            var goodsServiceMock = new Mock<IGoodsService>();
            goodsServiceMock.Setup(x => x.GetAllByCode(testCode)).Returns(() => goods);

            var goodGroupsServiceMock = new Mock<IGoodGroupsService>();
            var goodGroupIds = goods.Select(x => x.GroupID).ToList();
            var goodGroups = new List<GoodsGroup>()
            {
                    new GoodsGroup() { ID = 1, Name = GlobalConstants.SharedGroup },
                    new GoodsGroup() { ID = 2, Name = GlobalConstants.SharedGroup }
            };
            goodGroupsServiceMock.Setup(x => x.GetGoodGroupsByIds(goodGroupIds)).Returns(() => goodGroups);

            var objectsServiceMock = new Mock<IObjectsService>();

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            ProductQuantitiesService service = new ProductQuantitiesService(goodsServiceMock.Object, null,
                                                                            goodGroupsServiceMock.Object, objectsServiceMock.Object, flagService.Object);

            var response = service.GetPriceAndQuantitiesByCode(testCode, null).ToList();

            Assert.AreEqual(response.Count, 0);
            goodsServiceMock.VerifyAll();
            goodGroupsServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPriceAndQuantitiesByCode_ShouldCallGetAllByCodeWithCleanedCode_WhenYesglassCodePassed()
        {
            string testCode = "2233AGN-ALT";
            string cleanedTestCode = testCode.Replace("-ALT", "");

            var goods = new List<Good>().AsQueryable();

            var goodsServiceMock = new Mock<IGoodsService>();
            goodsServiceMock.Setup(x => x.GetAllByCode(cleanedTestCode)).Returns(goods);

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            ProductQuantitiesService service = new ProductQuantitiesService(goodsServiceMock.Object, null, null, null, flagService.Object);

            service.GetPriceAndQuantitiesByCode(testCode, null);

            goodsServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPriceAndQuantitiesByCode_ShouldReturnEmptyList_WhenNoSuchAnObject()
        {
            string testCode = "2233AGN";

            var goods = new List<Good>()
            {
                new Good() { ID = 1, Code = testCode, PriceOut2 = 1, GroupID = 1 },
                new Good() { ID = 2, Code = testCode, PriceOut2 = 1, GroupID = 2 }
            }.AsQueryable();

            var goodsServiceMock = new Mock<IGoodsService>();
            goodsServiceMock.Setup(x => x.GetAllByCode(testCode)).Returns(() => goods);

            var goodGroupsServiceMock = new Mock<IGoodGroupsService>();
            var goodGroupIds = goods.Select(x => x.GroupID).ToList();
            var goodGroups = new List<GoodsGroup>()
            {
                    new GoodsGroup() { ID = 1, Name = GlobalConstants.SaintGobainGroup },
                    new GoodsGroup() { ID = 2, Name = GlobalConstants.SaintGobainGroup }
            };
            goodGroupsServiceMock.Setup(x => x.GetGoodGroupsByIds(goodGroupIds)).Returns(() => goodGroups);

            var objectsServiceMock = new Mock<IObjectsService>();
            var objects = new List<ObjectSkladPro>()
            {
                new ObjectSkladPro() { ID = 1, Name = "Русе" },
                new ObjectSkladPro() { ID = 2, Name = "Слатина" },
            }.AsQueryable();
            objectsServiceMock.Setup(x => x.GetAll()).Returns(() => objects);

            var stores = new List<Store>()
            {
                new Store() { ID = 1, GoodID = 1, ObjectID = 3 },
                new Store() { ID = 2, GoodID = 1, ObjectID = 3 },
                new Store() { ID = 3, GoodID = 1, ObjectID = 3 },
                new Store() { ID = 4, GoodID = 2, ObjectID = 3 },
                new Store() { ID = 5, GoodID = 2, ObjectID = 3 },
            }.AsQueryable();

            var storesServiceMock = new Mock<IStoreService>();
            storesServiceMock.Setup(x => x.GetAllByGoodId(1)).Returns(() => stores);

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            ProductQuantitiesService service = new ProductQuantitiesService(goodsServiceMock.Object, storesServiceMock.Object,
                                                                            goodGroupsServiceMock.Object, objectsServiceMock.Object, flagService.Object);

            var response = service.GetPriceAndQuantitiesByCode(testCode, null).ToList();

            Assert.AreEqual(response.Count, 0);
            goodsServiceMock.VerifyAll();
            goodGroupsServiceMock.VerifyAll();
            storesServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPriceAndQuantitiesByCode_ShouldReturnEmptyList_WhenAllGroupsAreForbidden()
        {
            string testCode = "2233AGN";
            InternalApiDB.Models.User user = new InternalApiDB.Models.User()
            {
                OnlyHighCostVisible = false,
            };

            var goods = new List<Good>()
            {
                new Good() { ID = 1, Code = testCode, PriceOut2 = 1, GroupID = 1 },
                new Good() { ID = 2, Code = testCode, PriceOut2 = 1, GroupID = 2 },
                new Good() { ID = 3, Code = testCode, PriceOut2 = 1, GroupID = 3 },
                new Good() { ID = 4, Code = testCode, PriceOut2 = 1, GroupID = 4 },
                new Good() { ID = 5, Code = testCode, PriceOut2 = 1, GroupID = 5 },
                new Good() { ID = 6, Code = testCode, PriceOut2 = 1, GroupID = 6 },
                new Good() { ID = 7, Code = testCode, PriceOut2 = 1, GroupID = 7 },
                new Good() { ID = 8, Code = testCode, PriceOut2 = 1, GroupID = 8 }
            }.AsQueryable();

            var goodsServiceMock = new Mock<IGoodsService>();
            goodsServiceMock.Setup(x => x.GetAllByCode(testCode)).Returns(() => goods);

            var goodGroupsServiceMock = new Mock<IGoodGroupsService>();
            var goodGroupIds = goods.Select(x => x.GroupID).ToList();
            var goodGroups = new List<GoodsGroup>()
            {
                    new GoodsGroup() { ID = 1, Name = GlobalConstants.AGCGroup },
                    new GoodsGroup() { ID = 2, Name = GlobalConstants.FuyaoGroup },
                    new GoodsGroup() { ID = 3, Name = GlobalConstants.LamexGroup },
                    new GoodsGroup() { ID = 4, Name = GlobalConstants.NordglassGroup },
                    new GoodsGroup() { ID = 5, Name = GlobalConstants.PilkingtonGroup },
                    new GoodsGroup() { ID = 6, Name = GlobalConstants.SaintGobainGroup },
                    new GoodsGroup() { ID = 7, Name = GlobalConstants.SharedGroup },
                    new GoodsGroup() { ID = 8, Name = GlobalConstants.YesglassGroup }
            };
            goodGroupsServiceMock.Setup(x => x.GetGoodGroupsByIds(goodGroupIds)).Returns(() => goodGroups);

            var objectsServiceMock = new Mock<IObjectsService>();
            var objects = new List<ObjectSkladPro>() { }.AsQueryable();
            objectsServiceMock.Setup(x => x.GetAll()).Returns(() => objects);

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            ProductQuantitiesService service = new ProductQuantitiesService(goodsServiceMock.Object, null,
                                                                            goodGroupsServiceMock.Object, objectsServiceMock.Object, flagService.Object);

            var response = service.GetPriceAndQuantitiesByCode(testCode, user).ToList();

            Assert.AreEqual(response.Count, 0);
            goodsServiceMock.VerifyAll();
            goodGroupsServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPriceAndQuantitiesByCode_ShouldReturnNeededStoreItems_WhenUserWithAllManufacturersVisible()
        {
            string testCode = "2233AGN";
            InternalApiDB.Models.User user = new InternalApiDB.Models.User()
            {
                OnlyHighCostVisible = true,
            };

            string descriptionName1 = "test1";
            string descriptionName2 = "test2";
            string descriptionName3 = "test3";
            string descriptionName4 = "test4";
            string descriptionName5 = "test5";
            string descriptionName6 = "test6";
            string descriptionName7 = "test7";
            string descriptionName8 = "test8";
            var goods = new List<Good>()
            {
                new Good() { ID = 1, Code = testCode, PriceOut2 = 1, GroupID = 1, Name = $"{ GlobalConstants.AGCGroup } {descriptionName1}", Name2 = descriptionName1 },
                new Good() { ID = 2, Code = testCode, PriceOut2 = 2, GroupID = 2, Name = $"{ GlobalConstants.FuyaoGroup } {descriptionName2}", Name2 = descriptionName2 },
                new Good() { ID = 3, Code = testCode, PriceOut2 = 3, GroupID = 3, Name = $"{ GlobalConstants.LamexGroup } {descriptionName3}", Name2 = descriptionName3 },
                new Good() { ID = 4, Code = testCode, PriceOut2 = 4, GroupID = 4, Name = $"{ GlobalConstants.NordglassGroup } {descriptionName4}", Name2 = descriptionName4 },
                new Good() { ID = 5, Code = testCode, PriceOut2 = 5, GroupID = 5, Name = $"{ GlobalConstants.PilkingtonGroup } {descriptionName5}", Name2 = descriptionName5 },
                new Good() { ID = 6, Code = testCode, PriceOut2 = 6, GroupID = 6, Name = $"{ GlobalConstants.SaintGobainGroup } {descriptionName6}", Name2 = descriptionName6 },
                new Good() { ID = 7, Code = testCode, PriceOut2 = 7, GroupID = 7, Name = $"{ GlobalConstants.SharedGroup } ABCman {descriptionName7}", Name2 = descriptionName7 },
                new Good() { ID = 8, Code = testCode, PriceOut2 = 8, GroupID = 8, Name = $"{ GlobalConstants.YesglassGroup }1 {descriptionName8}", Name2 = descriptionName8 },
                new Good() { ID = 9, Code = testCode, PriceOut2 = 9, GroupID = 8, Name = $"{ GlobalConstants.YesglassGroup } {descriptionName8}", Name2 = descriptionName8 }
            }.AsQueryable();

            var goodsServiceMock = new Mock<IGoodsService>();
            goodsServiceMock.Setup(x => x.GetAllByCode(testCode)).Returns(() => goods);

            var goodGroupsServiceMock = new Mock<IGoodGroupsService>();
            var goodGroupIds = goods.Select(x => x.GroupID).ToList();
            var goodGroups = new List<GoodsGroup>()
            {
                    new GoodsGroup() { ID = 1, Name = GlobalConstants.AGCGroup },
                    new GoodsGroup() { ID = 2, Name = GlobalConstants.FuyaoGroup },
                    new GoodsGroup() { ID = 3, Name = GlobalConstants.LamexGroup },
                    new GoodsGroup() { ID = 4, Name = GlobalConstants.NordglassGroup },
                    new GoodsGroup() { ID = 5, Name = GlobalConstants.PilkingtonGroup },
                    new GoodsGroup() { ID = 6, Name = GlobalConstants.SaintGobainGroup },
                    new GoodsGroup() { ID = 7, Name = GlobalConstants.SharedGroup },
                    new GoodsGroup() { ID = 8, Name = GlobalConstants.YesglassGroup }
            };
            goodGroupsServiceMock.Setup(x => x.GetGoodGroupsByIds(goodGroupIds)).Returns(() => goodGroups);

            string objectSLatina = "Слатина";
            var objectsServiceMock = new Mock<IObjectsService>();
            var objects = new List<ObjectSkladPro>()
            {
                new ObjectSkladPro { ID = 1, Name = "random" },
                new ObjectSkladPro { ID = 2, Name = objectSLatina }
            }.AsQueryable();
            objectsServiceMock.Setup(x => x.GetAll()).Returns(() => objects);

            var stores1 = new List<Store>()
            {
                new Store() { ID = 1, GoodID = 1, ObjectID = 1, Qtty = 1 },
                new Store() { ID = 2, GoodID = 1, ObjectID = 1, Qtty = 1 },
                new Store() { ID = 3, GoodID = 1, ObjectID = 2, Qtty = 1 },
            }.AsQueryable();
            var stores2 = new List<Store>()
            {
                new Store() { ID = 4, GoodID = 2, ObjectID = 1, Qtty = 1 },
                new Store() { ID = 5, GoodID = 2, ObjectID = 1, Qtty = 0 },
            }.AsQueryable();
            var stores3 = new List<Store>()
            {
                new Store() { ID = 6, GoodID = 3, ObjectID = 2, Qtty = 2 },
                new Store() { ID = 7, GoodID = 3, ObjectID = 2, Qtty = 1 },
            }.AsQueryable();
            var stores4 = new List<Store>() { new Store() { ID = 8, GoodID = 4, ObjectID = 1, Qtty = 1 } }.AsQueryable();
            var stores5 = new List<Store>() { new Store() { ID = 9, GoodID = 5, ObjectID = 2, Qtty = 0 } }.AsQueryable();
            var stores6 = new List<Store>() { new Store() { ID = 10, GoodID = 6, ObjectID = 2, Qtty = 1 } }.AsQueryable();
            var stores7 = new List<Store>()
            {
                new Store() { ID = 11, GoodID = 7, ObjectID = 1, Qtty = 1 },
                new Store() { ID = 12, GoodID = 7, ObjectID = 1, Qtty = 1 }
            }.AsQueryable();
            var stores8 = new List<Store>()
            {
                new Store() { ID = 13, GoodID = 8, ObjectID = 1, Qtty = 2 },
                new Store() { ID = 14, GoodID = 8, ObjectID = 2, Qtty = 1 },
            }.AsQueryable();
            var stores9 = new List<Store>() { }.AsQueryable();

            var storesServiceMock = new Mock<IStoreService>();
            storesServiceMock.Setup(x => x.GetAllByGoodId(1)).Returns(() => stores1);
            storesServiceMock.Setup(x => x.GetAllByGoodId(2)).Returns(() => stores2);
            storesServiceMock.Setup(x => x.GetAllByGoodId(3)).Returns(() => stores3);
            storesServiceMock.Setup(x => x.GetAllByGoodId(4)).Returns(() => stores4);
            storesServiceMock.Setup(x => x.GetAllByGoodId(5)).Returns(() => stores5);
            storesServiceMock.Setup(x => x.GetAllByGoodId(6)).Returns(() => stores6);
            storesServiceMock.Setup(x => x.GetAllByGoodId(7)).Returns(() => stores7);
            storesServiceMock.Setup(x => x.GetAllByGoodId(8)).Returns(() => stores8);
            storesServiceMock.Setup(x => x.GetAllByGoodId(9)).Returns(() => stores9);

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            ProductQuantitiesService service = new ProductQuantitiesService(goodsServiceMock.Object, storesServiceMock.Object,
                                                                            goodGroupsServiceMock.Object, objectsServiceMock.Object, flagService.Object);

            var response = service.GetPriceAndQuantitiesByCode(testCode, user).ToList();

            Assert.AreEqual(response.Count, 8);

            Assert.AreEqual(response[0].GoodId, 1);
            Assert.AreEqual(response[0].Group, GlobalConstants.AGCGroup);
            Assert.AreEqual(response[0].DescriptionWithName, $"{ GlobalConstants.AGCGroup } {descriptionName1}");
            Assert.AreEqual(response[0].DescriptionWithoutName, descriptionName1);
            Assert.AreEqual(response[0].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[0].Price, 1);
            Assert.AreEqual(response[0].StoreQUantities.Count, 2);
            Assert.AreEqual(response[0].StoreQUantities[objectSLatina], 1);

            Assert.AreEqual(response[1].GoodId, 2);
            Assert.AreEqual(response[1].Group, GlobalConstants.FuyaoGroup);
            Assert.AreEqual(response[1].DescriptionWithName, $"{ GlobalConstants.FuyaoGroup } {descriptionName2}");
            Assert.AreEqual(response[1].DescriptionWithoutName, descriptionName2);
            Assert.AreEqual(response[1].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[1].Price, 2);
            Assert.AreEqual(response[1].StoreQUantities.Count, 1);

            Assert.AreEqual(response[2].GoodId, 3);
            Assert.AreEqual(response[2].Group, GlobalConstants.LamexGroup);
            Assert.AreEqual(response[2].DescriptionWithName, $"{ GlobalConstants.LamexGroup } {descriptionName3}");
            Assert.AreEqual(response[2].DescriptionWithoutName, descriptionName3);
            Assert.AreEqual(response[2].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[2].Price, 3);
            Assert.AreEqual(response[2].StoreQUantities.Count, 1);
            Assert.AreEqual(response[2].StoreQUantities[objectSLatina], 3);

            Assert.AreEqual(response[3].GoodId, 4);
            Assert.AreEqual(response[3].Group, GlobalConstants.NordglassGroup);
            Assert.AreEqual(response[3].DescriptionWithName, $"{ GlobalConstants.NordglassGroup } {descriptionName4}");
            Assert.AreEqual(response[3].DescriptionWithoutName, descriptionName4);
            Assert.AreEqual(response[3].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[3].Price, 4);
            Assert.AreEqual(response[3].StoreQUantities.Count, 1);

            Assert.AreEqual(response[4].GoodId, 5);
            Assert.AreEqual(response[4].Group, GlobalConstants.PilkingtonGroup);
            Assert.AreEqual(response[4].DescriptionWithName, $"{ GlobalConstants.PilkingtonGroup } {descriptionName5}");
            Assert.AreEqual(response[4].DescriptionWithoutName, descriptionName5);
            Assert.AreEqual(response[4].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[4].Price, 5);
            Assert.AreEqual(response[4].StoreQUantities.Count, 1);
            Assert.AreEqual(response[4].StoreQUantities[objectSLatina], 0);

            Assert.AreEqual(response[5].GoodId, 6);
            Assert.AreEqual(response[5].Group, GlobalConstants.SaintGobainGroup);
            Assert.AreEqual(response[5].DescriptionWithName, $"{ GlobalConstants.SaintGobainGroup } {descriptionName6}");
            Assert.AreEqual(response[5].DescriptionWithoutName, descriptionName6);
            Assert.AreEqual(response[5].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[5].Price, 6);
            Assert.AreEqual(response[5].StoreQUantities.Count, 1);
            Assert.AreEqual(response[5].StoreQUantities[objectSLatina], 1);

            Assert.AreEqual(response[6].GoodId, 7);
            Assert.AreEqual(response[6].Group, GlobalConstants.SharedGroup);
            Assert.AreEqual(response[6].DescriptionWithName, $"{ GlobalConstants.SharedGroup } ABCman {descriptionName7}");
            Assert.AreEqual(response[6].DescriptionWithoutName, descriptionName7);
            Assert.AreEqual(response[6].GroupFromItemName, "ABCman");
            Assert.AreEqual(response[6].Price, 7);
            Assert.AreEqual(response[6].StoreQUantities.Count, 1);

            Assert.AreEqual(response[7].GoodId, 8);
            Assert.AreEqual(response[7].Group, GlobalConstants.YesglassGroup);
            Assert.AreEqual(response[7].DescriptionWithName, $"{ GlobalConstants.YesglassGroup }1 {descriptionName8}");
            Assert.AreEqual(response[7].DescriptionWithoutName, descriptionName8);
            Assert.AreEqual(response[7].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[7].Price, 8);
            Assert.AreEqual(response[7].StoreQUantities.Count, 2);
            Assert.AreEqual(response[7].StoreQUantities[objectSLatina], 1);

            goodsServiceMock.VerifyAll();
            storesServiceMock.VerifyAll();
            goodGroupsServiceMock.VerifyAll();
            objectsServiceMock.VerifyAll();
        }


        [TestMethod]
        public void GetPriceAndQuantitiesByCode_ShouldReturnNeededStoreItems_WhenNoUser()
        {
            string testCode = "2233AGN";
            string descriptionName1 = "test1";
            string descriptionName2 = "test2";
            string descriptionName3 = "test3";
            string descriptionName4 = "test4";
            string descriptionName5 = "test5";
            string descriptionName6 = "test6";
            string descriptionName7 = "test7";
            string descriptionName8 = "test8";
            var goods = new List<Good>()
            {
                new Good() { ID = 1, Code = testCode, PriceOut2 = 1, GroupID = 1, Name = $"{ GlobalConstants.AGCGroup } {descriptionName1}", Name2 = descriptionName1 },
                new Good() { ID = 2, Code = testCode, PriceOut2 = 2, GroupID = 2, Name = $"{ GlobalConstants.FuyaoGroup } {descriptionName2}", Name2 = descriptionName2 },
                new Good() { ID = 3, Code = testCode, PriceOut2 = 3, GroupID = 3, Name = $"{ GlobalConstants.LamexGroup } {descriptionName3}", Name2 = descriptionName3 },
                new Good() { ID = 4, Code = testCode, PriceOut2 = 4, GroupID = 4, Name = $"{ GlobalConstants.NordglassGroup } {descriptionName4}", Name2 = descriptionName4 },
                new Good() { ID = 5, Code = testCode, PriceOut2 = 5, GroupID = 5, Name = $"{ GlobalConstants.PilkingtonGroup } {descriptionName5}", Name2 = descriptionName5 },
                new Good() { ID = 6, Code = testCode, PriceOut2 = 6, GroupID = 6, Name = $"{ GlobalConstants.SaintGobainGroup } {descriptionName6}", Name2 = descriptionName6 },
                new Good() { ID = 7, Code = testCode, PriceOut2 = 7, GroupID = 7, Name = $"{ GlobalConstants.SharedGroup } ABCman {descriptionName7}", Name2 = descriptionName7 },
                new Good() { ID = 8, Code = testCode, PriceOut2 = 8, GroupID = 8, Name = $"{ GlobalConstants.YesglassGroup }1 {descriptionName8}", Name2 = descriptionName8 },
                new Good() { ID = 9, Code = testCode, PriceOut2 = 9, GroupID = 8, Name = $"{ GlobalConstants.YesglassGroup } {descriptionName8}", Name2 = descriptionName8 }
            }.AsQueryable();

            var goodsServiceMock = new Mock<IGoodsService>();
            goodsServiceMock.Setup(x => x.GetAllByCode(testCode)).Returns(() => goods);

            var goodGroupsServiceMock = new Mock<IGoodGroupsService>();
            var goodGroupIds = goods.Select(x => x.GroupID).ToList();
            var goodGroups = new List<GoodsGroup>()
            {
                    new GoodsGroup() { ID = 1, Name = GlobalConstants.AGCGroup },
                    new GoodsGroup() { ID = 2, Name = GlobalConstants.FuyaoGroup },
                    new GoodsGroup() { ID = 3, Name = GlobalConstants.LamexGroup },
                    new GoodsGroup() { ID = 4, Name = GlobalConstants.NordglassGroup },
                    new GoodsGroup() { ID = 5, Name = GlobalConstants.PilkingtonGroup },
                    new GoodsGroup() { ID = 6, Name = GlobalConstants.SaintGobainGroup },
                    new GoodsGroup() { ID = 7, Name = GlobalConstants.SharedGroup },
                    new GoodsGroup() { ID = 8, Name = GlobalConstants.YesglassGroup }
            };
            goodGroupsServiceMock.Setup(x => x.GetGoodGroupsByIds(goodGroupIds)).Returns(() => goodGroups);

            string objectSLatina = "Слатина";
            var objectsServiceMock = new Mock<IObjectsService>();
            var objects = new List<ObjectSkladPro>()
            {
                new ObjectSkladPro { ID = 1, Name = "random" },
                new ObjectSkladPro { ID = 2, Name = objectSLatina }
            }.AsQueryable();
            objectsServiceMock.Setup(x => x.GetAll()).Returns(() => objects);

            var stores1 = new List<Store>()
            {
                new Store() { ID = 1, GoodID = 1, ObjectID = 1, Qtty = 1 },
                new Store() { ID = 2, GoodID = 1, ObjectID = 1, Qtty = 1 },
                new Store() { ID = 3, GoodID = 1, ObjectID = 2, Qtty = 1 },
            }.AsQueryable();
            var stores2 = new List<Store>()
            {
                new Store() { ID = 4, GoodID = 2, ObjectID = 1, Qtty = 1 },
                new Store() { ID = 5, GoodID = 2, ObjectID = 1, Qtty = 0 },
            }.AsQueryable();
            var stores3 = new List<Store>()
            {
                new Store() { ID = 6, GoodID = 3, ObjectID = 2, Qtty = 2 },
                new Store() { ID = 7, GoodID = 3, ObjectID = 2, Qtty = 1 },
            }.AsQueryable();
            var stores4 = new List<Store>() { new Store() { ID = 8, GoodID = 4, ObjectID = 1, Qtty = 1 } }.AsQueryable();
            var stores5 = new List<Store>() { new Store() { ID = 9, GoodID = 5, ObjectID = 2, Qtty = 0 } }.AsQueryable();
            var stores6 = new List<Store>() { new Store() { ID = 10, GoodID = 6, ObjectID = 2, Qtty = 1 } }.AsQueryable();
            var stores7 = new List<Store>()
            {
                new Store() { ID = 11, GoodID = 7, ObjectID = 1, Qtty = 1 },
                new Store() { ID = 12, GoodID = 7, ObjectID = 1, Qtty = 1 }
            }.AsQueryable();
            var stores8 = new List<Store>()
            {
                new Store() { ID = 13, GoodID = 8, ObjectID = 1, Qtty = 2 },
                new Store() { ID = 14, GoodID = 8, ObjectID = 2, Qtty = 1 },
            }.AsQueryable();
            var stores9 = new List<Store>() { }.AsQueryable();

            var storesServiceMock = new Mock<IStoreService>();
            storesServiceMock.Setup(x => x.GetAllByGoodId(1)).Returns(() => stores1);
            storesServiceMock.Setup(x => x.GetAllByGoodId(2)).Returns(() => stores2);
            storesServiceMock.Setup(x => x.GetAllByGoodId(3)).Returns(() => stores3);
            storesServiceMock.Setup(x => x.GetAllByGoodId(4)).Returns(() => stores4);
            storesServiceMock.Setup(x => x.GetAllByGoodId(5)).Returns(() => stores5);
            storesServiceMock.Setup(x => x.GetAllByGoodId(6)).Returns(() => stores6);
            storesServiceMock.Setup(x => x.GetAllByGoodId(7)).Returns(() => stores7);
            storesServiceMock.Setup(x => x.GetAllByGoodId(8)).Returns(() => stores8);
            storesServiceMock.Setup(x => x.GetAllByGoodId(9)).Returns(() => stores9);

            var flagService = new Mock<IFlagService>();
            flagService.Setup(x => x.GetFlagValue(FlagType.ShowOnlyHighCostGroups)).Returns(() => true);

            ProductQuantitiesService service = new ProductQuantitiesService(goodsServiceMock.Object, storesServiceMock.Object,
                                                                            goodGroupsServiceMock.Object, objectsServiceMock.Object, flagService.Object);

            var response = service.GetPriceAndQuantitiesByCode(testCode, null).ToList();

            Assert.AreEqual(response.Count, 8);

            Assert.AreEqual(response[0].GoodId, 1);
            Assert.AreEqual(response[0].Group, GlobalConstants.AGCGroup);
            Assert.AreEqual(response[0].DescriptionWithName, $"{ GlobalConstants.AGCGroup } {descriptionName1}");
            Assert.AreEqual(response[0].DescriptionWithoutName, descriptionName1);
            Assert.AreEqual(response[0].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[0].Price, 1);
            Assert.AreEqual(response[0].StoreQUantities.Count, 2);
            Assert.AreEqual(response[0].StoreQUantities[objectSLatina], 1);

            Assert.AreEqual(response[1].GoodId, 2);
            Assert.AreEqual(response[1].Group, GlobalConstants.FuyaoGroup);
            Assert.AreEqual(response[1].DescriptionWithName, $"{ GlobalConstants.FuyaoGroup } {descriptionName2}");
            Assert.AreEqual(response[1].DescriptionWithoutName, descriptionName2);
            Assert.AreEqual(response[1].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[1].Price, 2);
            Assert.AreEqual(response[1].StoreQUantities.Count, 1);

            Assert.AreEqual(response[2].GoodId, 3);
            Assert.AreEqual(response[2].Group, GlobalConstants.LamexGroup);
            Assert.AreEqual(response[2].DescriptionWithName, $"{ GlobalConstants.LamexGroup } {descriptionName3}");
            Assert.AreEqual(response[2].DescriptionWithoutName, descriptionName3);
            Assert.AreEqual(response[2].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[2].Price, 3);
            Assert.AreEqual(response[2].StoreQUantities.Count, 1);
            Assert.AreEqual(response[2].StoreQUantities[objectSLatina], 3);

            Assert.AreEqual(response[3].GoodId, 4);
            Assert.AreEqual(response[3].Group, GlobalConstants.NordglassGroup);
            Assert.AreEqual(response[3].DescriptionWithName, $"{ GlobalConstants.NordglassGroup } {descriptionName4}");
            Assert.AreEqual(response[3].DescriptionWithoutName, descriptionName4);
            Assert.AreEqual(response[3].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[3].Price, 4);
            Assert.AreEqual(response[3].StoreQUantities.Count, 1);

            Assert.AreEqual(response[4].GoodId, 5);
            Assert.AreEqual(response[4].Group, GlobalConstants.PilkingtonGroup);
            Assert.AreEqual(response[4].DescriptionWithName, $"{ GlobalConstants.PilkingtonGroup } {descriptionName5}");
            Assert.AreEqual(response[4].DescriptionWithoutName, descriptionName5);
            Assert.AreEqual(response[4].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[4].Price, 5);
            Assert.AreEqual(response[4].StoreQUantities.Count, 1);
            Assert.AreEqual(response[4].StoreQUantities[objectSLatina], 0);

            Assert.AreEqual(response[5].GoodId, 6);
            Assert.AreEqual(response[5].Group, GlobalConstants.SaintGobainGroup);
            Assert.AreEqual(response[5].DescriptionWithName, $"{ GlobalConstants.SaintGobainGroup } {descriptionName6}");
            Assert.AreEqual(response[5].DescriptionWithoutName, descriptionName6);
            Assert.AreEqual(response[5].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[5].Price, 6);
            Assert.AreEqual(response[5].StoreQUantities.Count, 1);
            Assert.AreEqual(response[5].StoreQUantities[objectSLatina], 1);

            Assert.AreEqual(response[6].GoodId, 7);
            Assert.AreEqual(response[6].Group, GlobalConstants.SharedGroup);
            Assert.AreEqual(response[6].DescriptionWithName, $"{ GlobalConstants.SharedGroup } ABCman {descriptionName7}");
            Assert.AreEqual(response[6].DescriptionWithoutName, descriptionName7);
            Assert.AreEqual(response[6].GroupFromItemName, "ABCman");
            Assert.AreEqual(response[6].Price, 7);
            Assert.AreEqual(response[6].StoreQUantities.Count, 1);

            Assert.AreEqual(response[7].GoodId, 8);
            Assert.AreEqual(response[7].Group, GlobalConstants.YesglassGroup);
            Assert.AreEqual(response[7].DescriptionWithName, $"{ GlobalConstants.YesglassGroup }1 {descriptionName8}");
            Assert.AreEqual(response[7].DescriptionWithoutName, descriptionName8);
            Assert.AreEqual(response[7].GroupFromItemName, string.Empty);
            Assert.AreEqual(response[7].Price, 8);
            Assert.AreEqual(response[7].StoreQUantities.Count, 2);
            Assert.AreEqual(response[7].StoreQUantities[objectSLatina], 1);

            goodsServiceMock.VerifyAll();
            storesServiceMock.VerifyAll();
            goodGroupsServiceMock.VerifyAll();
            objectsServiceMock.VerifyAll();
        }
    }
}
