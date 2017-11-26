namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using GGG_OnlineShop.Common;

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
        [StringLength(GlobalConstants.DescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        [StringLength(GlobalConstants.EurocodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string EuroCode { get; set; }

        [StringLength(GlobalConstants.OesCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OesCode { get; set; }

        [Required]
        [StringLength(GlobalConstants.MakeNameMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Make { get; set; }

        [StringLength(GlobalConstants.ModelNameMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Model { get; set; }

        public ModelAndPartDatesJsonInfoModel ModelYear { get; set; }

        public ModelAndPartDatesJsonInfoModel PartYear { get; set; }

        public string ProductType { get; set; }

        [StringLength(GlobalConstants.ModificationMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Modification { get; set; }

        [StringLength(GlobalConstants.TintMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Tint { get; set; }

        [Range(GlobalConstants.MinProductFittingTimeHours, GlobalConstants.MaxProductFittingTimeHours)]
        public double? FittingTimeHours { get; set; }

        [StringLength(GlobalConstants.FittingTypeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string FittingType { get; set; } 

        [Range(GlobalConstants.MinProductHeight, GlobalConstants.MaxProductHeight)]
        public double? Height { get; set; }

        [Range(GlobalConstants.MinProductWidth, GlobalConstants.MaxProductWidth)]
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

        [StringLength(GlobalConstants.MaterialNumberMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string MaterialNumber { get; set; }

        [StringLength(GlobalConstants.LocalCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string LocalCode { get; set; }

        [StringLength(GlobalConstants.IndustryCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
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

