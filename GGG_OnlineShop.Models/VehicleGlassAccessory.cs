namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassAccessory: BaseModel<int>
    {
        private ICollection<VehicleGlass> vehicleGlasses;

        public VehicleGlassAccessory()
        {
            this.vehicleGlasses = new HashSet<VehicleGlass>();
        }

        [StringLength(GlobalConstants.MaterialNumberMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string MaterialNumber { get; set; }

        [StringLength(GlobalConstants.IndustryCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string IndustryCode { get; set; }

        [StringLength(GlobalConstants.DescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        [Range(GlobalConstants.MinAccessoryReplacementRate, GlobalConstants.MaxAccessoryReplacementRate)]
        public double ReplacementRate { get; set; }

        public bool Mandatory { get; set; }

        [Range(GlobalConstants.MinAccessoryRecommendedQuantity, GlobalConstants.MaxAccessoryRecommendedQuantity)]
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
    }
}