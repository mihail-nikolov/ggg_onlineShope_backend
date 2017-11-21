namespace GGG_OnlineShop.Data.Services.Contracts
{
    using InternalApiDB.Models;
    using System.Linq;

    public interface IOrderedItemsService: IBaseDataWithCreatorService<OrderedItem>
    {
        // TODO introduce enum for status
        IQueryable<OrderedItem> GetAllPending();

        IQueryable<OrderedItem> GetAllFinished();
    }
}
