namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassImage: BaseModel<int>
    {
        private ICollection<VehicleGlass> vehicleGlasses;

        public VehicleGlassImage()
        {
            this.vehicleGlasses = new HashSet<VehicleGlass>();
        }

        [Required]
        [StringLength(GlobalConstants.ProductImageCaptionMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.ProductImageCaptionMinLength)]
        public string Caption { get; set; }

        [Required]
        public int OriginalId { get; set; }

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
