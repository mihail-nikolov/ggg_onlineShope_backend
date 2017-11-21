namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class AccessoryJsonInfoModel: IMapTo<VehicleGlassAccessory>
    {
        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string IndustryCode { get; set; }

        [StringLength(400, ErrorMessage = "max len:{1}")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string MaterialNumber { get; set; }

        [Range(0, double.MaxValue)]
        public double ReplacementRate { get; set; }

        public bool Mandatory { get; set; }

        public bool HasImages { get; set; }

        public bool HasFittingMethod { get; set; }

        [Range(0, int.MaxValue)]
        public int RecommendedQuantity { get; set; }

        public override string ToString()
        {
            string infoString = $@"
    IndustryCode: {IndustryCode}
    Description: {Description}
    MaterialNumber: {MaterialNumber}
    ReplacementRate: {ReplacementRate}
    Mandatory: {Mandatory}
    HasImages: {HasImages}
    HasFittingMethod: {HasFittingMethod}
    RecommendedQuantity: {RecommendedQuantity}
";
            return infoString;
        }
    }
}