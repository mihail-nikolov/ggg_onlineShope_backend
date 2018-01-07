namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;
    using GGG_OnlineShop.Common;
    using Base;

    public class OrderedItemsService : BaseDataWithCreatorService<OrderedItem>, IOrderedItemsService
    {
        public OrderedItemsService(IInternalDbRepository<OrderedItem> dataSet, IInternalDbRepository<User> users)
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

        public bool IsValidOrder(OrderedItem order)
        {
            bool result = true;
            var neededPrice = order.Price * GlobalConstants.MinPercentPaidPrice;

            // should have some codes passed
            if (result && (string.IsNullOrEmpty(order.OtherCodes) && string.IsNullOrEmpty(order.EuroCode)))
            {
                result = false;
            }

            // if not enough money paid
            if (result && (order.IsDepositNeeded && order.PaidPrice < neededPrice))
            {
                var user = order.User; // TODO test this!
                // and registered user
                if (user != null)
                {
                    // deffered payment not allowed -> invalid
                    if (!user.IsDeferredPaymentAllowed)
                    {
                        result = false;
                    }
                }
                // not registered user => no way to have deffered payment -> invalid
                else
                {
                    result = false;
                }
            }

            if (result && ((string.IsNullOrEmpty(order.AnonymousUserInfo) || string.IsNullOrEmpty(order.AnonymousUserЕmail))
                                                                 && string.IsNullOrEmpty(order.UserId)))
            {
                result = false;
            }

            return result;
        }
    }
}
