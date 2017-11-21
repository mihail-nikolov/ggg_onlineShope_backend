namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;

    public class ImageJsonInfoModel : IMapTo<VehicleGlassImage>, IHaveCustomMappings
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 1)]
        public string Caption { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ImageJsonInfoModel, VehicleGlassImage>("ImageJsonInfoModel")
             .ForMember(x => x.OriginalId, opt => opt.MapFrom(x => x.Id));
        }

        public override string ToString()
        {
            string infoString = $@"
    Id: {Id}
    Caption: {Caption}
";
            return infoString;
        }
    }
}