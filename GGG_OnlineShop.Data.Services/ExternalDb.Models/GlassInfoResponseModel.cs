namespace GGG_OnlineShop.Data.Services.ExternalDb.Models
{
    using System.Collections.Generic;

    public class ProductInfoResponseModel
    {
        public int GoodId { get; set; }

        public Dictionary<string, int> StoreQUantities { get; set; }

        public string Group { get; set; }

        public string DescriptionWithName { get; set; }

        public string DescriptionWithoutName { get; set; }

        public double? Price { get; set; }

        public string GroupFromItemName { get; set; }
    }
}
