namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
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
        private ICollection<OrderedItem> orderedItems;

        public VehicleGlass()
        {
            this.vehicles = new HashSet<Vehicle>();
            this.vehicleGlassInterchangeableParts = new HashSet<VehicleGlassInterchangeablePart>();
            this.vehicleGlassImages = new HashSet<VehicleGlassImage>();
            this.vehicleGlassCharacteristics = new HashSet<VehicleGlassCharacteristic>();
            this.vehicleGlassSuperceed = new HashSet<VehicleGlassSuperceed>();
            this.vehicleGlassAccessories = new HashSet<VehicleGlassAccessory>();
            this.orderedItems = new HashSet<OrderedItem>();
        }

        [Required]
        [StringLength(400, ErrorMessage = "max len:{1}")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string EuroCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OesCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string ModelDate { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string PartDate { get; set; }

        public string ProductType { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string Modification { get; set; }

        // TODO check again string lens
        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string Tint { get; set; }

        [Range(0, double.MaxValue)]
        public double? FittingTimeHours { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string FittingType { get; set; }

        [Range(0.1, double.MaxValue)]
        public double? Height { get; set; }

        [Range(0.1, double.MaxValue)]
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

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string MaterialNumber { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string LocalCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
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

        public virtual ICollection<OrderedItem> OrderedItems
        {
            get
            {
                return this.orderedItems;
            }
            set
            {
                this.orderedItems = value;
            }
        }
    }
}
