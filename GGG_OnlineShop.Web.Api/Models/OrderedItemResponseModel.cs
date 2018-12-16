namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;

    public class OrderedItemResponseModel : IMapFrom<OrderedItem>
    {
        public string Manufacturer { get; set; }

        public string EuroCode { get; set; }

        public string OtherCodes { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}