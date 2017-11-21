namespace GGG_OnlineShop.Web.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassRequestModel
    {
        [Required]
        public int MakeId { get; set; }

        public int? ModelId { get; set; }

        public int? BodyTypeId { get; set; }

        public string ProductType { get; set; }
    }
}