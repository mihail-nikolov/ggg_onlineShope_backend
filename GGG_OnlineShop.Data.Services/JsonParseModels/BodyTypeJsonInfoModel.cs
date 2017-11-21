namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class BodyTypeJsonInfoModel: IMapTo<VehicleBodyType>
    {
        [Required]
        [StringLength(100, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 1)]
        public string Code { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 1)]
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