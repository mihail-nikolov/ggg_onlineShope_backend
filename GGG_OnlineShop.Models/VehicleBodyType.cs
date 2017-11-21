﻿namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
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
        [StringLength(100, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 1)]
        public string Code { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 0)]
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