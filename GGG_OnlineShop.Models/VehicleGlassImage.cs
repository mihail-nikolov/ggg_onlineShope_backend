namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
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
        [StringLength(100, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 1)]
        public string Caption { get; set; }

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
