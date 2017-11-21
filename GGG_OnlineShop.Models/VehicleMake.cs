namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class VehicleMake : BaseModel<int>
    {
        private ICollection<Vehicle> vehicles;

        public VehicleMake()
        {
            this.vehicles = new HashSet<Vehicle>();
        }

        [Required]
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