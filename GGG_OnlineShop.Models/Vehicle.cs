namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Vehicle: BaseModel<int>
    {
        private ICollection<VehicleGlass> glasses;

        public Vehicle()
        {
            this.glasses = new HashSet<VehicleGlass>();
        }

        [Required]
        [ForeignKey("Make")]
        public int MakeId { get; set; }

        public VehicleMake Make { get; set; }

        [ForeignKey("Model")]
        public int? ModelId { get; set; }

        public VehicleModel Model { get; set; }

        [ForeignKey("BodyType")]
        public int? BodyTypeId { get; set; }

        public VehicleBodyType BodyType { get; set; }

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
