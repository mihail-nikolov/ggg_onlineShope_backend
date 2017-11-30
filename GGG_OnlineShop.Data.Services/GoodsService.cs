namespace GGG_OnlineShop.Data.Services
{
    using System.Linq;
    using Common;
    using Contracts;
    using SkladProDB.Models;

    public class GoodsService : BaseDataExternalService<Good>, IGoodsService
    {
        public GoodsService(IExternalDbRepository<Good> dataSet) : base(dataSet)
        {
        }

        public IQueryable<Good> GetAllByCode(string code)
        {
            return this.Data.All().Where(a => a.Code == code);
        }
    }
}