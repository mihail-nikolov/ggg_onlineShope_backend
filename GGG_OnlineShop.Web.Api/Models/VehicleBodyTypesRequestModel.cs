namespace GGG_OnlineShop.Web.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VehicleBodyTypesRequestModel
    {
        [Required]
        public int MakeId { get; set; }

        public int? ModelId { get; set; }
    }
}