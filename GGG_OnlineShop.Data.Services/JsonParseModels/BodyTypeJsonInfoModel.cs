namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using GGG_OnlineShop.Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class BodyTypeJsonInfoModel: IMapTo<VehicleBodyType>
    {
        [Required]
        [StringLength(GlobalConstants.BodyTypeCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage, MinimumLength = GlobalConstants.BodyTypeCodeMinLength)]
        public string Code { get; set; }

        [StringLength(GlobalConstants.BodyTypeDescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        public override string ToString()
        {
            string infoString = $@"
    Code: {Code}
    Description: {Description}
";
            return infoString;
        }
    }
}