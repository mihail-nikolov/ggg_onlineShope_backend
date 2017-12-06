namespace GGG_OnlineShop.Web.Api.Areas.Administration.Models.OrderedItems
{
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class OrderedItemRequestUpdateStatusModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DeliveryStatus Status{ get; set; }
    }
}