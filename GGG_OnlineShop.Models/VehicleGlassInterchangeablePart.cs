namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassInterchangeablePart: BaseModel<int>
    {
        private ICollection<VehicleGlass> vehicleGlasses;

        public VehicleGlassInterchangeablePart()
        {
            this.vehicleGlasses = new HashSet<VehicleGlass>();
        }

        [Required]
        [StringLength(400, ErrorMessage = "max len:{1}")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string EuroCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OesCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string MaterialNumber { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string LocalCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string ScanCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string NagsCode { get; set; }

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