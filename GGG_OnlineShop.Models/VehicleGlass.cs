namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlass : BaseModel<int>
    {
        private ICollection<Vehicle> vehicles;
        private ICollection<VehicleGlassInterchangeablePart> vehicleGlassInterchangeableParts;
        private ICollection<VehicleGlassImage> vehicleGlassImages;
        private ICollection<VehicleGlassCharacteristic> vehicleGlassCharacteristics;
        private ICollection<VehicleGlassSuperceed> vehicleGlassSuperceed;
        private ICollection<VehicleGlassAccessory> vehicleGlassAccessories;

        public VehicleGlass()
        {
            this.vehicles = new HashSet<Vehicle>();
            this.vehicleGlassInterchangeableParts = new HashSet<VehicleGlassInterchangeablePart>();
            this.vehicleGlassImages = new HashSet<VehicleGlassImage>();
            this.vehicleGlassCharacteristics = new HashSet<VehicleGlassCharacteristic>();
            this.vehicleGlassSuperceed = new HashSet<VehicleGlassSuperceed>();
            this.vehicleGlassAccessories = new HashSet<VehicleGlassAccessory>();
        }

        [StringLength(GlobalConstants.EurocodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string EuroCode { get; set; }

        [StringLength(GlobalConstants.OesCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OesCode { get; set; }

        [StringLength(GlobalConstants.MaterialNumberMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string MaterialNumber { get; set; }

        [Required]
        [StringLength(GlobalConstants.DescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        [StringLength(GlobalConstants.ModelDateMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string ModelDate { get; set; }

        [StringLength(GlobalConstants.PartDateMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string PartDate { get; set; }

        public string ProductType { get; set; }

        [StringLength(GlobalConstants.ModificationMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Modification { get; set; }

        [StringLength(GlobalConstants.TintMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Tint { get; set; }

        [Range(GlobalConstants.MinProductFittingTimeHours, GlobalConstants.MaxProductFittingTimeHours)]
        public double? FittingTimeHours { get; set; }

        [StringLength(GlobalConstants.FittingTypeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string FittingType { get; set; }

        [Range(GlobalConstants.MinProductHeight, GlobalConstants.MaxProductHeight)]
        public double? Height { get; set; }

        [Range(GlobalConstants.MinProductWidth, GlobalConstants.MaxProductWidth)]
        public double? Width { get; set; }

        public virtual ICollection<VehicleGlassCharacteristic> VehicleGlassCharacteristics
        {
            get
            {
                return this.vehicleGlassCharacteristics;
            }
            set
            {
                this.vehicleGlassCharacteristics = value;
            }
        }

        public virtual ICollection<VehicleGlassAccessory> VehicleGlassAccessories
        {
            get
            {
                return this.vehicleGlassAccessories;
            }
            set
            {
                this.vehicleGlassAccessories = value;
            }
        }

        public virtual ICollection<VehicleGlassSuperceed> VehicleGlassSuperceeds
        {
            get
            {
                return this.vehicleGlassSuperceed;
            }
            set
            {
                this.vehicleGlassSuperceed = value;
            }
        }

        public virtual ICollection<VehicleGlassImage> VehicleGlassImages
        {
            get
            {
                return this.vehicleGlassImages;
            }
            set
            {
                this.vehicleGlassImages = value;
            }
        }

        public bool HasFittingMethod { get; set; }

        public int FeaturedImageId { get; set; }

        public bool IsAcoustic { get; set; }

        public bool IsCalibration { get; set; }

        public bool IsYesGlass { get; set; }

        public bool IsAccessory { get; set; }

        [StringLength(GlobalConstants.LocalCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string LocalCode { get; set; }

        [StringLength(GlobalConstants.IndustryCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string IndustryCode { get; set; }

        public virtual ICollection<VehicleGlassInterchangeablePart> VehicleGlassInterchangeableParts
        {
            get
            {
                return this.vehicleGlassInterchangeableParts;
            }
            set
            {
                this.vehicleGlassInterchangeableParts = value;
            }
        }

        public virtual ICollection<Vehicle> Vehicles
        {
            get
            {
                return this.vehicles;
            }
            set
            {
                this.vehicles = value;
            }
        }
    }
}
