namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassSuperceed : BaseModel<int>
    {
        private ICollection<VehicleGlass> vehicleGlasses;

        public VehicleGlassSuperceed()
        {
            this.vehicleGlasses = new HashSet<VehicleGlass>();
        }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OldMaterialNumber { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OldOesCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OldEuroCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OldLocalCode { get; set; }

        [StringLength(150, ErrorMessage = "max len:{1}")]
        public string ChangeDate { get; set; }

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