﻿namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using System.Linq;
    using Common;
    using GGG_OnlineShop.Common;
    using Base;

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
                order.UserInfo = string.Format(GlobalConstants.DeletedUserInfo, user.PhoneNumber);
                order.UserЕmail = user.Email;
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
            userFromDb.OnlyHighCostVisible = user.OnlyHighCostVisible;
            userFromDb.IsDeferredPaymentAllowed = user.IsDeferredPaymentAllowed;
            userFromDb.Bulstat = user.Bulstat;
            userFromDb.Name = user.Name;
            this.Save();

            return userFromDb;
        }

        public User UpdateContactInfo(User user)
        {
            var userFromDb = this.GetById(user.Id);
            userFromDb.DeliveryCountry = user.DeliveryCountry;
            userFromDb.DeliveryTown = user.DeliveryTown;
            userFromDb.DeliveryAddress = user.DeliveryAddress;
            userFromDb.PhoneNumber = user.PhoneNumber;
            this.Save();

            return userFromDb;
        }

        // Probably to return string with the needed info
        public bool IsValidUser(User user)
        {
            bool result = true;

            if (user.IsCompany)
            {
                if (string.IsNullOrEmpty(user.Bulstat))
                {
                    result = false;
                }

                if (result && Data.All().FirstOrDefault(x => x.Bulstat == user.Bulstat) != null)
                {
                    result = false;
                }
                
            }
            else if (!user.IsCompany && !string.IsNullOrEmpty(user.Bulstat))
            {
                result = false;
            }

            return result;
        }
    }
}
