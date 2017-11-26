namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassCharacteristic: BaseModel<int>
    {
        private ICollection<VehicleGlass> glasses;

        public VehicleGlassCharacteristic()
        {
            this.glasses = new HashSet<VehicleGlass>();
        }

        [Required]
        [StringLength(GlobalConstants.ProductCharacteristicMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.ProductCharacteristicMinLength)]
        public string Name { get; set; }

        public virtual ICollection<VehicleGlass> VehicleGlasses
        {
            get
            {
                return this.glasses;
            }
            set
            {
                this.glasses = value;
            }
        }
    }
}