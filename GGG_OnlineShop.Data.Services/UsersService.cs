﻿namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using System.Linq;
    using Common;
    using GGG_OnlineShop.Common;

    public class UsersService : BaseDataService<User>, IUsersService 
    {
        public UsersService(IInternalDbRepository<User> dataSet) : base(dataSet)
        {
        }

        public void CleanUserInfoFromOrders(User user)
        {
            var orders = user.OrderedItems;

            foreach (var order in orders)
            {
                order.AnonymousUserInfo = string.Format(GlobalConstants.DeletedUserInfo, user.PhoneNumber);
                order.AnonymousUserЕmail = user.Email;
                order.UserId = null;
            }

            this.Data.Save();
        }

        public IQueryable<User> GetAllNotActivated()
        {
            return this.GetAll().Where(u => !u.EmailConfirmed);
        }

        public User GetByEmail(string email)
        {
            return this.GetAll().Where(u => u.Email == email).FirstOrDefault();
        }

        public User Update(User user)
        {
            var userFromDb = this.GetById(user.Id);
            userFromDb.PercentageReduction = user.PercentageReduction;
            userFromDb.Bulstat = user.Bulstat;
            userFromDb.CompanyName = user.CompanyName;
            this.Save();

            return this.GetById(userFromDb.Id);
        }

        public User UpdateContactInfo(User user)
        {
            var userFromDb = this.GetById(user.Id);
            userFromDb.DeliveryCountry = user.DeliveryCountry;
            userFromDb.DeliveryTown = user.DeliveryTown;
            userFromDb.DeliveryAddress = user.DeliveryAddress;
            userFromDb.PhoneNumber = user.PhoneNumber;
            this.Save();

            return this.GetById(userFromDb.Id);
        }
    }
}
