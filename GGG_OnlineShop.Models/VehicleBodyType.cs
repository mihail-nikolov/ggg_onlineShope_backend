namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleBodyType: BaseModel<int>
    {
        private ICollection<Vehicle> vehicles;

        public VehicleBodyType()
        {
            this.vehicles = new HashSet<Vehicle>();
        }

        [Required]
        [StringLength(GlobalConstants.BodyTypeCodeMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.BodyTypeCodeMinLength)]
        public string Code { get; set; }

        [StringLength(GlobalConstants.BodyTypeDescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

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