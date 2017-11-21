namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class SuperceedJsonInfoModel : IMapTo<VehicleGlassSuperceed>
    {
        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OldMaterialNumber { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OldOesCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OldEuroCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OldLocalCode { get; set; }

        [StringLength(150, ErrorMessage = "max len:{1}")]
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