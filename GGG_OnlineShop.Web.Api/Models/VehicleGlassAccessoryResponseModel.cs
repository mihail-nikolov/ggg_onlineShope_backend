namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassAccessoryResponseModel: IMapFrom<VehicleGlassAccessory>
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string IndustryCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string MaterialNumber { get; set; }

        [StringLength(400, ErrorMessage = "max len:{1}")]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public double ReplacementRate { get; set; }

        public bool Mandatory { get; set; }

        [Range(0, int.MaxValue)]
        public int RecommendedQuantity { get; set; }
    }
}