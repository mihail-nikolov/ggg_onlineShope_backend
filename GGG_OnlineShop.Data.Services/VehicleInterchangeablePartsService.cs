namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;

    public class VehicleInterchangeablePartsService : BaseDataService<VehicleGlassInterchangeablePart>, IVehicleInterchangeablePartsService
    {
        public VehicleInterchangeablePartsService(IInternalDbRepository<VehicleGlassInterchangeablePart> dataSet) : base(dataSet)
        {
        }

        public VehicleGlassInterchangeablePart GetByEuroCode(string euroCode)
        {
            var glass = this.Data.All().Where(g => g.EuroCode.ToLower() == euroCode.ToLower()).First();
            return glass;
        }

        // oes is not unique - several glasses could have 1 oes
        public IQueryable<VehicleGlassInterchangeablePart> GetByOesCode(string oesCode)
        {
            var glass = this.Data.All().Where(g => g.OesCode.ToLower() == oesCode.ToLower());
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByMaterialNumber(string materialNumber)
        {
            var glass = this.Data.All().Where(g => g.MaterialNumber.ToLower() == materialNumber.ToLower()).First();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByLocalCode(string localCode)
        {
            var glass = this.Data.All().Where(g => g.LocalCode.ToLower() == localCode.ToLower()).First();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByScanCode(string scanCode)
        {
            var glass = this.Data.All().Where(g => g.ScanCode.ToLower() == scanCode.ToLower()).First();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByNagsCode(string nagsCode)
        {
            var glass = this.Data.All().Where(g => g.NagsCode.ToLower() == nagsCode.ToLower()).First();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetInterchangeablePart(string euroCode, string oesCode,
                                                                      string materialNumber, string localCode,
                                                                      string scanCode, string nagsCode)
        {
            // oes is not unique - several glasses could have 1 oes
            // => will not search by oes
            VehicleGlassInterchangeablePart interchangeablePart;

            if (!string.IsNullOrEmpty(euroCode))
            {
                interchangeablePart = this.GetByEuroCode(euroCode);
            }
            else if (!string.IsNullOrEmpty(materialNumber))
            {
                interchangeablePart = this.GetByMaterialNumber(materialNumber);
            }
            else if (!string.IsNullOrEmpty(scanCode))
            {
                interchangeablePart = this.GetByScanCode(scanCode);
            }
            else if (!string.IsNullOrEmpty(nagsCode))
            {
                interchangeablePart = this.GetByNagsCode(nagsCode);
            }
            else
            {
                // localcode
                interchangeablePart = this.GetByLocalCode(localCode);
            }

            return interchangeablePart;
        }
    }
}
