namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class VehicleInterchangeablePartResponseModel : IMapFrom<VehicleGlassInterchangeablePart>
    {
        public int Id { get; set; }

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
    }
}