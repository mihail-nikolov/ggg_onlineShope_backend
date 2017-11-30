namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using System.Linq;
    using Common;

    public class UsersService : BaseDataService<User>, IUsersService 
    {
        public UsersService(IInternalDbRepository<User> dataSet) : base(dataSet)
        {
        }

        public IQueryable<User> GetAllNotActivated()
        {
            return this.GetAll().Where(u => !u.IsAccountActive);
        }

        public User Update(User user)
        {
            var userFromDb = this.GetById(user.Id);
            userFromDb.IsAccountActive = user.IsAccountActive;
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
