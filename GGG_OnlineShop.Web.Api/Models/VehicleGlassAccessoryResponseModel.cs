namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;

    public class VehicleGlassAccessoryResponseModel: IMapFrom<VehicleGlassAccessory>
    {
        public string IndustryCode { get; set; }

        public string MaterialNumber { get; set; }

        public string Description { get; set; }

        public double ReplacementRate { get; set; }

        public bool Mandatory { get; set; }

        public int RecommendedQuantity { get; set; }
    }
}