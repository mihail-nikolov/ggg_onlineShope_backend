namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderedItem: BaseModel<int>, IEntityWithCreator
    {
        [Required]
        [StringLength(400, ErrorMessage = "max len:{1}")]
        public string Address { get; set; }

        [Required]
        [StringLength(600, ErrorMessage = "max len:{1}")]
        public string DeliveryInfo { get; set; }

        public VehicleGlass VehicleGlass { get; set; }

        [Required]
        [ForeignKey("VehicleGlass")]
        public int VehicleGlassId { get; set; }

        public VehicleGlassAccessory VehicleGlassAccessory { get; set; }

        [Required]
        [ForeignKey("VehicleGlassAccessory")]
        public int VehicleGlassAccessoryId { get; set; }

        public bool Finished { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
