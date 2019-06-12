using GGG_OnlineShop.InternalApiDB.Models.Enums;

namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Linq;

    public interface IOrdersService: IBaseDataWithCreatorService<Order>
    {
        IQueryable<Order> GetNewOrders();

        IQueryable<Order> GetOrderedProducts();

        IQueryable<Order> GetDoneOrders();

        bool IsValidOrder(Order product);

        IQueryable<Order> GetOrderByStatus(DeliveryStatus status);
    }
}
