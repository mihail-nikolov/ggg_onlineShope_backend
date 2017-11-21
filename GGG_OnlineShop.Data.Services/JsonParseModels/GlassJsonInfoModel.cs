namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;

    public class GlassJsonInfoModel : IMapTo<VehicleGlass>, IHaveCustomMappings
    {
        public GlassJsonInfoModel()
        {
            this.Characteristics = new List<string>();
            this.Images = new List<ImageJsonInfoModel>();
            this.BodyTypes = new List<BodyTypeJsonInfoModel>();
            this.InterchangeableParts = new List<InterchangeableParJsonInfoModel>();
            this.Superceeds = new List<SuperceedJsonInfoModel>();
            this.Accessories = new List<AccessoryJsonInfoModel>();
        }

        [Required]
        [StringLength(400, ErrorMessage = "max len:{1}")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string EuroCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string OesCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string Make { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string Model { get; set; }

        // TODO - think about ModelYearFrom and ModelYearTo - separated
        public ModelAndPartDatesJsonInfoModel ModelYear { get; set; }

        public ModelAndPartDatesJsonInfoModel PartYear { get; set; }

        public string ProductType { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string Modification { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string Tint { get; set; }

        [Range(0.1, double.MaxValue)]
        public double? FittingTimeHours { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string FittingType { get; set; } 

        [Range(0.1, double.MaxValue)]
        public double? Height { get; set; }

        [Range(0.1, double.MaxValue)]
        public double? Width { get; set; }

        public List<string> Characteristics { get; set; }

        public List<BodyTypeJsonInfoModel> BodyTypes { get; set; }

        public List<ImageJsonInfoModel> Images { get; set; }

        public bool HasFittingMethod { get; set; }

        public int FeaturedImageId { get; set; }

        public bool IsYesGlass { get; set; }

        public bool IsAcoustic { get; set; }

        public bool IsCalibration { get; set; }

        public List<InterchangeableParJsonInfoModel> InterchangeableParts { get; set; }

        public bool IsAccessory { get; set; }

        public List<SuperceedJsonInfoModel> Superceeds { get; set; }

        public List<AccessoryJsonInfoModel> Accessories { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string MaterialNumber { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string LocalCode { get; set; }

        [StringLength(100, ErrorMessage = "max len:{1}")]
        public string IndustryCode { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<GlassJsonInfoModel, VehicleGlass>("GlassJsonInfoModel")
                .ForMember(x => x.VehicleGlassInterchangeableParts, opt => opt.Ignore())
                .ForMember(x => x.ModelDate, opt => opt.MapFrom(x => x.ModelYear != null ? x.ModelYear.ToString(): null))
                .ForMember(x => x.PartDate, opt => opt.MapFrom(x => x.PartYear != null ? x.PartYear.ToString() : null));
        }

        public override string ToString()
        {
            string characteristics = string.Join(", ", Characteristics);
            string interchangableParts = string.Join(", ", InterchangeableParts);
            string accessorries = string.Join(", ", Accessories);
            string superceeds = string.Join(", ", Superceeds);
            string bodyTypes = string.Join(", ", BodyTypes);
            string images = string.Join(", ", Images);

            string infoString = $@"
Description: {Description}
EuroCode: {EuroCode}
OesCode: {OesCode}
Make: {Make}
Model: {Model}
------
ModelYear: {ModelYear}
------
PartYear: {PartYear}
------
ProductType: {ProductType}
Modification: {Modification}
Tint: {Tint}
Characteristics:{characteristics}
------
BodyTypes:{bodyTypes}
------
Images: {images}
------
IsAcoustic: {IsAcoustic}
IsCalibration: {IsCalibration}
IsAccessory: {IsAccessory}
-----
Accessorries:{accessorries}
-----
superceeds:{superceeds}
-----
InterchangableParts:{interchangableParts}
-----
";
            return infoString;
        }
    }
}

