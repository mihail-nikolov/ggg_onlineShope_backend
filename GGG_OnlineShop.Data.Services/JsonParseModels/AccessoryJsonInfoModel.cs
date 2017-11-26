namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using GGG_OnlineShop.Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class AccessoryJsonInfoModel: IMapTo<VehicleGlassAccessory>
    {
        [StringLength(GlobalConstants.IndustryCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string IndustryCode { get; set; }

        [StringLength(GlobalConstants.DescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        [StringLength(GlobalConstants.MaterialNumberMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string MaterialNumber { get; set; }

        [Range(GlobalConstants.MinAccessoryReplacementRate, GlobalConstants.MaxAccessoryReplacementRate)]
        public double ReplacementRate { get; set; }

        public bool Mandatory { get; set; }

        public bool HasImages { get; set; }

        public bool HasFittingMethod { get; set; }

        [Range(GlobalConstants.MinAccessoryRecommendedQuantity, GlobalConstants.MaxAccessoryRecommendedQuantity)]
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