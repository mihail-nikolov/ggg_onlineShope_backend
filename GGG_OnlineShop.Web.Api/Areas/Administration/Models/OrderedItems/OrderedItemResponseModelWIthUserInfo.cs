namespace GGG_OnlineShop.Web.Api.Areas.Administration.Models.OrderedItems
{
    using AutoMapper;
    using InternalApiDB.Models;
    using Infrastructure;
    using Api.Models;

    public class OrderedItemResponseModelWIthUserInfo : OrderedItemResponseModel, IHaveCustomMappings
    {
        public string AnonymousUserInfo { get; set; }

        public string AnonymousUserЕmail { get; set; }

        public string UserInfo { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<OrderedItem, OrderedItemResponseModelWIthUserInfo>("OrderedItemResponseModelWIthUserInfo")
              .ForMember(x => x.UserInfo, opt => opt.MapFrom(x => !string.IsNullOrEmpty(x.UserId) ?
                                                             x.User.Bulstat + "; " + x.User.CompanyName + "; "
                                                             + x.User.Email + "; " + x.User.PhoneNumber : string.Empty));
        }
    }
}