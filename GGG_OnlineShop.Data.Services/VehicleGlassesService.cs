namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;

    public class VehicleGlassesService : BaseDataService<VehicleGlass>, IVehicleGlassesService
    {
        public VehicleGlassesService(IInternalDbRepository<VehicleGlass> dataSet) : base(dataSet)
        {
        }

        public VehicleGlass GetByEuroCode(string euroCode)
        {
            var glass = this.Data.All().Where(g => g.EuroCode.ToLower() == euroCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlass GetByOesCode(string oesCode)
        {
            var glass = this.Data.All().Where(g => g.OesCode.ToLower() == oesCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlass GetByMaterialNumber(string materialNumber)
        {
            var glass = this.Data.All().Where(g => g.MaterialNumber.ToLower() == materialNumber.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlass GetByLocalCode(string localCode)
        {
            var glass = this.Data.All().Where(g => g.LocalCode.ToLower() == localCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlass GetByIndustryCode(string industryCode)
        {
            var glass = this.Data.All().Where(g => g.IndustryCode.ToLower() == industryCode.ToLower()).FirstOrDefault();
            return glass;
        }

        public VehicleGlass GetGlass(string euroCode, string oesCode, string materialNumber, string localCode, string industryCode)
        {
            VehicleGlass glass;
            if (!string.IsNullOrEmpty(euroCode))
            {
                glass = this.GetByEuroCode(euroCode);
            }
            else if (!string.IsNullOrEmpty(oesCode))
            {
                glass = this.GetByOesCode(oesCode);
            }
            else if (!string.IsNullOrEmpty(materialNumber))
            {
                glass = this.GetByMaterialNumber(materialNumber);
            }
            else if (!string.IsNullOrEmpty(industryCode))
            {
                glass = this.GetByIndustryCode(industryCode);
            }
            else
            {
                // localcode
                glass = this.GetByLocalCode(localCode);
            }

            return glass;
        }

        public IQueryable<VehicleGlass> GetByRandomCode(string code)
        {
            var glasses = this.Data.All().Where(g => g.EuroCode.ToLower().Contains(code.ToLower()))
                            .Union(this.Data.All().Where(g => g.OesCode.ToLower().Contains(code.ToLower())))
                            .Union(this.Data.All().Where(g => g.MaterialNumber.ToLower().Contains(code.ToLower())))
                            .Union(this.Data.All().Where(g => g.IndustryCode.ToLower().Contains(code.ToLower())))
                            .Union(this.Data.All().Where(g => g.LocalCode.ToLower().Contains(code.ToLower())));
            return glasses;
        }

        public IQueryable<VehicleGlassAccessory> GetAccessories(int glassId)
        {
            IQueryable<VehicleGlassAccessory> accessories;
            accessories = this.Data.GetById(glassId).VehicleGlassAccessories.AsQueryable();

            return accessories;
        }
    }
}
