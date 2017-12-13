namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;
    using Base;

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

        public IQueryable<VehicleGlass> GetGlassesByEuroCode(string euroCode)
        {
            var glasses = this.Data.All().Where(g => g.EuroCode.ToLower().StartsWith(euroCode.ToLower()));
            return glasses;
        }

        // oes is not unique - several glasses could have 1 oes
        public IQueryable<VehicleGlass> GetByOesCode(string oesCode)
        {
            var glasses = this.Data.All().Where(g => g.OesCode.ToLower() == oesCode.ToLower());
            return glasses;
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

        public VehicleGlass GetGlass(string euroCode, string materialNumber, string industryCode, string localCode)
        {
            VehicleGlass glass;
            // oes is not unique - several glasses could have 1 oes
            // => will not search by oes
            if (!string.IsNullOrEmpty(euroCode))
            {
                glass = this.GetByEuroCode(euroCode);
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

        // method is used to get the product code by which we will search in SkladProDb
        public string GetCode(VehicleGlass product)
        {
            string code = string.Empty;
            if (!string.IsNullOrEmpty(product.EuroCode))
            {
                code = product.EuroCode;
            }
            else if (!string.IsNullOrEmpty(product.MaterialNumber))
            {
                code = product.MaterialNumber;
            }
            else if (!string.IsNullOrEmpty(product.IndustryCode))
            {
                code = product.IndustryCode;
            }
            else if (!string.IsNullOrEmpty(product.LocalCode))
            {
                code = product.LocalCode;
            }
            else
            {
                // OesCode
                code = product.OesCode;
            }

            return code;
        }

        // this method will be used to get optimize filling in the DB
        // will have all unique codes from Db and when a new glass is passed the code will be checked in this codes collection
        public IQueryable<string> GetAllUniqueCodesFromDb()
        {
            var codes = this.Data.All().Select(
                            x =>
                                (!string.IsNullOrEmpty(x.EuroCode) ? x.EuroCode :
                                    (!string.IsNullOrEmpty(x.MaterialNumber) ? x.MaterialNumber :
                                        (!string.IsNullOrEmpty(x.IndustryCode) ? x.IndustryCode : x.LocalCode))));
            return codes;
        }
    }
}
