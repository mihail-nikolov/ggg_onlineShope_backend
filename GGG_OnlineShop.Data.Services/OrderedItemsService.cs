namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;
    using GGG_OnlineShop.Common;
    using Base;
    using InternalApiDB.Models.Enums;

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

            // should have some codes passed
            if (result && (string.IsNullOrEmpty(order.OtherCodes) && string.IsNullOrEmpty(order.EuroCode)))
            {
                result = false;
            }

            var user = order.User;
            // when not paid and deffered payment not allowed
            if (result && order.PaidPrice <= GlobalConstants.MinPrice && (user != null && !user.IsDeferredPaymentAllowed))
            {
                result = false;
            }

            return result;
        }
    }
}
