namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using GGG_OnlineShop.Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class SuperceedJsonInfoModel : IMapTo<VehicleGlassSuperceed>
    {
        [StringLength(GlobalConstants.MaterialNumberMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OldMaterialNumber { get; set; }

        [StringLength(GlobalConstants.OesCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OldOesCode { get; set; }

        [StringLength(GlobalConstants.EurocodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OldEuroCode { get; set; }

        [StringLength(GlobalConstants.LocalCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OldLocalCode { get; set; }

        [StringLength(GlobalConstants.SuperceedChangeDateMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string ChangeDate { get; set; }

        public override string ToString()
        {
            string infoString = $@"
    OldMaterialNumber: {OldMaterialNumber}
    OldOesCode: {OldOesCode}
    OldEuroCode: {OldEuroCode}
    OldLocalCode: {OldLocalCode}
    ChangeDate: {ChangeDate}
";
            return infoString;
        }
    }
}