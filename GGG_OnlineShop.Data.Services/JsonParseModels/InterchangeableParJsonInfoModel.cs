namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class InterchangeableParJsonInfoModel : IMapTo<VehicleGlassInterchangeablePart>
    {
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

        [Required]
        [StringLength(400, ErrorMessage = "max len:{1}")]
        public string Description { get; set; }

        public override string ToString()
        {
            string infoString = $@"
    Description: {Description}
    EuroCode: {EuroCode}
    MaterialNumber: {MaterialNumber}
    LocalCode: {LocalCode}
    ScanCode: {ScanCode}
    NagsCode: {NagsCode}
";
            return infoString;
        }
    }
}