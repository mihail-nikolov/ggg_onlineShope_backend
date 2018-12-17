namespace GGG_OnlineShop.Data.Services
{
    using System.Linq;
    using Common;
    using Base;
    using Contracts;
    using InternalApiDB.Models;
    using InternalApiDB.Models.Enums;

    public class FlagService : BaseDataService<Flag>, IFlagService
    {
        public FlagService(IInternalDbRepository<Flag> dataSet) : base(dataSet)
        {
        }

        public bool GetFlagValue(FlagType name)
        {
           var flag = Data.All().FirstOrDefault(x => x.FlagTypeEnum == name);

            return flag != null && flag.Value;
        }

        public override void Add(Flag flag)
        {
            var existingFlag = Data.All().FirstOrDefault(x => x.Name == flag.FlagTypeEnum.ToString());
            if (existingFlag != null)
            {
                existingFlag.Value = flag.Value;
            }
            else
            {
                this.Data.Add(flag);
            }

            this.Data.Save();
        }
    }
}
