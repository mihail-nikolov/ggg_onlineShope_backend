namespace GGG_OnlineShop.Web.Api.Areas.Administration.ViewModels.OrderedItems
{
    using Models;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using InternalApiDB.Models;

    public class OrderedItemResponseModelWIthUserInfo: OrderedItemResponseModel
    {
        [Required]
        public string Bulstat { get; set; }

        public override void CreateMappings(IConfiguration configuration)
        {
             // TODO check this mapping again
            configuration.CreateMap<OrderedItem, OrderedItemResponseModelWIthUserInfo>("OrderedItemResponseModelWIthUserInfo")
              .ForMember(x => x.Description, opt => opt.MapFrom(x => x.VehicleGlass.Description))
              .ForMember(x => x.CreatedOn, opt => opt.MapFrom(x => x.CreatedOn))
              .ForMember(x => x.EuroCode, opt => opt.MapFrom(x => x.VehicleGlass.EuroCode))
              .ForMember(x => x.OesCode, opt => opt.MapFrom(x => x.VehicleGlass.OesCode))
              .ForMember(x => x.LocalCode, opt => opt.MapFrom(x => x.VehicleGlass.LocalCode))
              .ForMember(x => x.Bulstat, opt => opt.MapFrom(x => x.User.Bulstat));
        }
    }
}