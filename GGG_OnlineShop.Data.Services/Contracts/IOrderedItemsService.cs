namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Linq;

    public interface IOrderedItemsService: IBaseDataWithCreatorService<OrderedItem>
    {
        IQueryable<OrderedItem> GetNewOrders();

        IQueryable<OrderedItem> GetOrderedProducts();

        IQueryable<OrderedItem> GetDoneOrders();

        bool ValidateOrder(OrderedItem product);
    }
}
