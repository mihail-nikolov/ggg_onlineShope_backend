namespace GGG_OnlineShop.Data.Services.Contracts
{
    using SkladProDB.Models;
    using System.Linq;

    public interface IGoodsService: IBaseDataExternalService<Good>
    {
        IQueryable<Good> GetAllByCode(string code);
    }
}
