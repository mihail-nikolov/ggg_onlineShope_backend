namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassAccessory: BaseModel<int>
    {
        private ICollection<VehicleGlass> vehicleGlasses;
        private ICollection<OrderedItem> orderedItems;

        public VehicleGlassAccessory()
        {
            this.vehicleGlasses = new HashSet<VehicleGlass>();
            this.orderedItems = new HashSet<OrderedItem>();
        }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string IndustryCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string MaterialNumber { get; set; }

        [StringLength(400, ErrorMessage = "max len:{1}")]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public double ReplacementRate { get; set; }

        public bool Mandatory { get; set; }

        [Range(0, int.MaxValue)]
        public int RecommendedQuantity { get; set; }

        public bool HasImages { get; set; }

        public bool HasFittingMethod { get; set; }

        public virtual ICollection<VehicleGlass> VehicleGlasses
        {
            get
            {
                return this.vehicleGlasses;
            }
            set
            {
                this.vehicleGlasses = value;
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