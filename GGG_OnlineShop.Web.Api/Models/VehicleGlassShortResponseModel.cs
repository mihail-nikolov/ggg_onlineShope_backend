namespace GGG_OnlineShop.Web.Api.Models
{
    using AutoMapper;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class VehicleGlassShortResponseModel : IMapFrom<VehicleGlass>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string EuroCode { get; set; }

        public string OesCode { get; set; }

        public string MaterialNumber { get; set; }

        public string LocalCode { get; set; }

        public string IndustryCode { get; set; }

        public string ProductType { get; set; }

        public string Position { get; set; }

        public bool IsAccessory { get; set; }

        public int? FeaturedImageId { get; set; }

        public List<int> Images { get; set; }

        public virtual void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<VehicleGlass, VehicleGlassShortResponseModel>("VehicleGlassShortResponseModel")
               .ForMember(x => x.Images, opt => opt.MapFrom(x => x.VehicleGlassImages.Select(i => i.OriginalId).ToList()));
        }
    }
}