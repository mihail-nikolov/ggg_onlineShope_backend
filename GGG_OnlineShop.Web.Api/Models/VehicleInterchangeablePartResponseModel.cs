namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;
    using AutoMapper;

    public class VehicleInterchangeablePartResponseModel : IMapFrom<VehicleGlassInterchangeablePart>, IHaveCustomMappings
    {
        public string Description { get; set; }

        public string EuroCode { get; set; }

        public string OesCode { get; set; }

        public string MaterialNumber { get; set; }

        public string LocalCode { get; set; }

        public string ScanCode { get; set; }

        public string NagsCode { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            // todo constant for min eurocode length
            configuration.CreateMap<VehicleGlassInterchangeablePart, VehicleInterchangeablePartResponseModel>("VehicleInterchangeablePartResponseModel")
             .ForMember(x => x.EuroCode, opt => opt.MapFrom(x => x.EuroCode.Split(';')[0].Length > 5 ? x.EuroCode.Split(';')[0] : x.EuroCode));
        }
    }
}