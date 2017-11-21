namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
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
        [StringLength(200, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 1)]
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