namespace GGG_OnlineShop.Web.Api.Models
{
    using AutoMapper;
    using Infrastructure;
    using InternalApiDB.Models;
    using System;

    public class OrderedItemResponseModel : IMapFrom<OrderedItem>, IMapFrom<VehicleGlass>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string EuroCode { get; set; }

        public string OesCode { get; set; }

        public string MaterialNumber { get; set; }

        public string LocalCode { get; set; }

        public string IndustryCode { get; set; }

        public bool Finished { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<OrderedItem, OrderedItemResponseModel>("OrderedItemResponseModel")
              .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
              .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn))
              .ForMember(x => x.Description, opt => opt.MapFrom(x => x.VehicleGlass.Description))
              .ForMember(x => x.EuroCode, opt => opt.MapFrom(x => x.VehicleGlass.EuroCode))
              .ForMember(x => x.OesCode, opt => opt.MapFrom(x => x.VehicleGlass.OesCode))
              .ForMember(x => x.LocalCode, opt => opt.MapFrom(x => x.VehicleGlass.LocalCode));
        }
    }
}