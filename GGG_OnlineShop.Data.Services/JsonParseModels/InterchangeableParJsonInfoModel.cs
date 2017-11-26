namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using GGG_OnlineShop.Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class InterchangeableParJsonInfoModel : IMapTo<VehicleGlassInterchangeablePart>
    {
        [StringLength(GlobalConstants.InterchangeableEurocodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string EuroCode { get; set; }

        [StringLength(GlobalConstants.OesCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OesCode { get; set; }

        [StringLength(GlobalConstants.MaterialNumberMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string MaterialNumber { get; set; }

        [StringLength(GlobalConstants.LocalCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string LocalCode { get; set; }

        [StringLength(GlobalConstants.ScanCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string ScanCode { get; set; }

        [StringLength(GlobalConstants.NagsCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string NagsCode { get; set; }

        [Required]
        [StringLength(GlobalConstants.DescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
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