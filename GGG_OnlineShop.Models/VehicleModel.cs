namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleModel: BaseModel<int>
    {
        private ICollection<Vehicle> vehicles;

        public VehicleModel()
        {
            this.vehicles = new HashSet<Vehicle>();
        }

        [Required]
        [StringLength(GlobalConstants.ModelNameMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.ModelNameMinLength)]
        public string Name { get; set; }

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