namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;
    using GGG_OnlineShop.Common;

    public class OrderedItemsService : BaseDataWithCreatorService<OrderedItem>, IOrderedItemsService
    {
        public OrderedItemsService(IDbRepository<OrderedItem> dataSet, IDbRepository<User> users)
            : base(dataSet, users)
        {
        }

        public IQueryable<OrderedItem> GetDoneOrders()
        {
            return this.Data.All().Where(x => x.Status == DeliveryStatus.Done);
        }

        public IQueryable<OrderedItem> GetNewOrders()
        {
            return this.Data.All().Where(x => x.Status == DeliveryStatus.New);
        }

        public IQueryable<OrderedItem> GetOrderedProducts()
        {
            return this.Data.All().Where(x => x.Status == DeliveryStatus.Ordered);
        }

        public bool ValidateOrder(OrderedItem product)
        {
            bool result = true;
            var neededPrice = product.Price * GlobalConstants.MinPercentPaidPrice;
            if (product.IsDepositNeeded && product.PaidPrice < neededPrice)
            {
                result = false;
            }

            if ((string.IsNullOrEmpty(product.AnonymousUserInfo) || string.IsNullOrEmpty(product.AnonymousUserЕmail))
                                                                 && string.IsNullOrEmpty(product.UserId))
            {
                result = false;
            }

            return result;
        }
    }
}
