﻿namespace GGG_OnlineShop.Web.Api.Areas.Administration.Models.OrderedItems
{
    using AutoMapper;
    using InternalApiDB.Models;
    using Infrastructure;
    using Api.Models;

    public class OrderResponseModelWIthUserInfo : OrderResponseModel
        //, IHaveCustomMappings
    {
        public string UserЕmail { get; set; }

        public string UserInfo { get; set; }

        //public void CreateMappings(IConfiguration configuration)
        //{
        //    configuration.CreateMap<OrderedItem, OrderedItemResponseModelWIthUserInfo>("OrderedItemResponseModelWIthUserInfo")
        //      .ForMember(x => x.UserInfo, opt => opt.MapFrom(x => !string.IsNullOrEmpty(x.UserId) ?
        //                                                     (x.User.IsCompany ? x.User.Bulstat + "; " : string.Empty) + x.User.Name + "; "
        //                                                     + x.User.Email + "; " + x.User.PhoneNumber : string.Empty));
        //}
    }
}