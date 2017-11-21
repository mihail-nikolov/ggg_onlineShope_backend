namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using System.Linq;
    using Common;

    public class UsersService : BaseDataService<User>, IUsersService 
    {
        public UsersService(IDbRepository<User> dataSet) : base(dataSet)
        {
        }

        public IQueryable<User> GetAllNotActivated()
        {
            return this.GetAll().Where(u => !u.IsAccountActive);
        }
    }
}
