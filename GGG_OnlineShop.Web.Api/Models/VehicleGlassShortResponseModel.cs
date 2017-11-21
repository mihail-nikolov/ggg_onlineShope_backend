namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;

    public class VehicleGlassShortResponseModel : IMapFrom<VehicleGlass>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string EuroCode { get; set; }

        public string OesCode { get; set; }

        public string MaterialNumber { get; set; }

        public string LocalCode { get; set; }

        public string IndustryCode { get; set; }
    }
}