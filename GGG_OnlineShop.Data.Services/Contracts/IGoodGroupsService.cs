namespace GGG_OnlineShop.Data.Services.Contracts
{
    using SkladProDB.Models;
    using System.Collections.Generic;

    public interface IGoodGroupsService: IBaseDataExternalService<GoodsGroup>
    {
        ICollection<GoodsGroup> GetGoodGroupsByIds(IEnumerable<int?> goodGroupIds);
    }
}
