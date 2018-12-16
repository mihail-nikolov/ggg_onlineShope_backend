namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;
    using Base;
    using InternalApiDB.Models.Enums;

    public class OrdersService : BaseDataWithCreatorService<Order>, IOrdersService
    {
        public OrdersService(IInternalDbRepository<Order> dataSet, IInternalDbRepository<User> users)
            : base(dataSet, users)
        {
        }

        public IQueryable<Order> GetDoneOrders()
        {
            return this.Data.All().Where(x => x.Status == DeliveryStatus.Done);
        }

        public IQueryable<Order> GetNewOrders()
        {
            return this.Data.All().Where(x => x.Status == DeliveryStatus.Unpaid); // newest are the unpaid
        }

        public IQueryable<Order> GetOrderedProducts()
        {
            return this.Data.All().Where(x => x.Status == DeliveryStatus.Ordered);
        }

        public bool IsValidOrder(Order order)
        {
            bool result = true;

            // should have some codes passed
            foreach (var orderOrderedItem in order.OrderedItems)
            {
                if (result && (string.IsNullOrEmpty(orderOrderedItem.OtherCodes) && string.IsNullOrEmpty(orderOrderedItem.EuroCode)))
                {
                    result = false;
                }
            }

            //var user = order.User;
            //// when not paid and deffered payment not allowed
            //if (result && order.PaidPrice <= GlobalConstants.MinPrice && (user != null && !user.IsDeferredPaymentAllowed))
            //{
            //    result = false;
            //}

            return result;
        }
    }
}
