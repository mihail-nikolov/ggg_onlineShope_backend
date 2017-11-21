namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;

    public class VehicleInterchangeablePartsService : BaseDataService<VehicleGlassInterchangeablePart>, IVehicleInterchangeablePartsService
    {
        public VehicleInterchangeablePartsService(IDbRepository<VehicleGlassInterchangeablePart> dataSet) : base(dataSet)
        {
        }

        public VehicleGlassInterchangeablePart GetByEuroCode(string euroCode)
        {
            var glass = this.Data.All().Where(g => g.EuroCode.ToLower() == euroCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByOesCode(string oesCode)
        {
            var glass = this.Data.All().Where(g => g.OesCode.ToLower() == oesCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByMaterialNumber(string materialNumber)
        {
            var glass = this.Data.All().Where(g => g.MaterialNumber.ToLower() == materialNumber.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByLocalCode(string localCode)
        {
            var glass = this.Data.All().Where(g => g.LocalCode.ToLower() == localCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByScanCode(string scanCode)
        {
            var glass = this.Data.All().Where(g => g.ScanCode.ToLower() == scanCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetByNagsCode(string nagsCode)
        {
            var glass = this.Data.All().Where(g => g.NagsCode.ToLower() == nagsCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlassInterchangeablePart GetInterchangeablePart(string euroCode, string oesCode, 
                                                                      string materialNumber, string localCode,
                                                                      string scanCode, string nagsCode)
        {
            VehicleGlassInterchangeablePart interchangeablePart;

            if (!string.IsNullOrEmpty(euroCode))
            {
                interchangeablePart = this.GetByEuroCode(euroCode);
            }
            else if (!string.IsNullOrEmpty(oesCode))
            {
                interchangeablePart = this.GetByOesCode(oesCode);
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
