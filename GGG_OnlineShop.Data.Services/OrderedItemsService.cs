namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;

    public class OrderedItemsService : BaseDataWithCreatorService<OrderedItem>, IOrderedItemsService
    {
        public OrderedItemsService(IDbRepository<OrderedItem> dataSet, IDbRepository<User> users)
            : base(dataSet, users)
        {
        }

        public IQueryable<OrderedItem> GetAllFinished()
        {
            return this.Data.All().Where(x => x.Finished == true);
        }

        public IQueryable<OrderedItem> GetAllPending()
        {
            return this.Data.All().Where(x => x.Finished != true);
        }
    }
}
