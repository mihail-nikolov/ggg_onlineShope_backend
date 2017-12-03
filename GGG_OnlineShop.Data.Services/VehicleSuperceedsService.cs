namespace GGG_OnlineShop.Data.Services
{
    using System.Linq;
    using Contracts;
    using Common;
    using InternalApiDB.Models;

    public class VehicleSuperceedsService : BaseDataService<VehicleGlassSuperceed>, IVehicleSuperceedsService
    {
        public VehicleSuperceedsService(IInternalDbRepository<VehicleGlassSuperceed> dataSet) : base(dataSet)
        {
        }

        public VehicleGlassSuperceed GetByOldEuroCode(string oldEuroCode)
        {
            var superceed = this.Data.All().Where(s => s.OldEuroCode.ToLower() == oldEuroCode.ToLower()).FirstOrDefault();
            return superceed;
        }

        public VehicleGlassSuperceed GetByOldLocalCode(string oldLocalCode)
        {
            var superceed = this.Data.All().Where(s => s.OldLocalCode.ToLower() == oldLocalCode.ToLower()).FirstOrDefault();
            return superceed;
        }

        public VehicleGlassSuperceed GetByOldMaterialNumber(string oldMaterialNumber)
        {
            var superceed = this.Data.All().Where(s => s.OldMaterialNumber.ToLower() == oldMaterialNumber.ToLower()).FirstOrDefault();
            return superceed;
        }

        // oes is not unique - several glasses could have 1 oes
        public IQueryable<VehicleGlassSuperceed> GetByOldOesCode(string oldOesCode)
        {
            var superceed = this.Data.All().Where(s => s.OldOesCode.ToLower() == oldOesCode.ToLower());
            return superceed;
        }

        public VehicleGlassSuperceed GetSuperceed(string oldEuroCode, string oldLocalCode, string oldMaterialNumber)
        {
            // oes is not unique - several glasses could have 1 oes
            // => will not search by oes
            VehicleGlassSuperceed superceed;

            if (!string.IsNullOrEmpty(oldEuroCode))
            {
                superceed = this.GetByOldEuroCode(oldEuroCode);
            }
            else if (!string.IsNullOrEmpty(oldMaterialNumber))
            {
                superceed = this.GetByOldMaterialNumber(oldMaterialNumber);
            }
            else
            {
                // localcode
                superceed = this.GetByOldLocalCode(oldLocalCode);
            }

            return superceed;
        }
    }
}
