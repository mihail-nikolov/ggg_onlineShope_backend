namespace GGG_OnlineShop.Data.Services.ExternalDb
{
    using Base;
    using Contracts;
    using Common;

    public class ObjectsService : BaseDataExternalService<SkladProDB.Models.ObjectSkladPro>, IObjectsService
    {
        public ObjectsService(IExternalDbRepository<SkladProDB.Models.ObjectSkladPro> dataSet) : base(dataSet)
        {
        }
    }
}
