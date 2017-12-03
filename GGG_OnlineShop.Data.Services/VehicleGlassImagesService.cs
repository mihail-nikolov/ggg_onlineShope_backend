﻿namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using Common;
    using System.Linq;

    public class VehicleGlassImagesService : BaseDataService<VehicleGlassImage>, IVehicleGlassImagesService
    {
        public VehicleGlassImagesService(IInternalDbRepository<VehicleGlassImage> dataSet) : base(dataSet)
        {
        }

        public VehicleGlassImage GetByCaption(string caption)
        {
            var image = this.Data.All().Where(i => i.Caption.ToLower() == caption.ToLower()).First();
            return image;
        }

        public VehicleGlassImage GetByOriginalId(int originalId)
        {
            var image = this.Data.All().Where(i => i.OriginalId == originalId).First();
            return image;
        }
    }
}
