﻿namespace GGG_OnlineShop.Infrastructure
{
    using AutoMapper;

    public interface IHaveCustomMappings
    {
         void CreateMappings(IConfiguration configuration);
    }
}