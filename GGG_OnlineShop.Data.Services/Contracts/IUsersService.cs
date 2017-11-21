﻿namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Linq;

    public interface IUsersService : IBaseDataService<User>
    {
        IQueryable<User> GetAllNotActivated(); 
    }
}
