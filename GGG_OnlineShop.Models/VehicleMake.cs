namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleMake : BaseModel<int>
    {
        private ICollection<Vehicle> vehicles;

        public VehicleMake()
        {
            this.vehicles = new HashSet<Vehicle>();
        }

        [Required]
        [StringLength(GlobalConstants.MakeNameMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.MakeNameMinLength)]
        public string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles
        {
            get => this.vehicles;
            set => this.vehicles = value;
        }
    }
}