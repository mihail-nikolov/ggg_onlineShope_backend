namespace GGG_OnlineShop.Web.Api.Areas.Administration.Models.OrderedItems
{
    using InternalApiDB.Models;

    public class OrderedItemRequestUpdateStatusModel
    {
        public int Id { get; set; }

        public DeliveryStatus Status{ get; set; }
    }
}