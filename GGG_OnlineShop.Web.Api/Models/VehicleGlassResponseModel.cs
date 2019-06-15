namespace GGG_OnlineShop.Web.Api.Models
{
    using AutoMapper;
    using InternalApiDB.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class VehicleGlassResponseModel : VehicleGlassShortResponseModel
    {
        public string ModelDate { get; set; }

        public string PartDate { get; set; }

        public string Modification { get; set; }

        public string Tint { get; set; }

        public double? FittingTimeHours { get; set; }

        public string FittingType { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public bool IsAcoustic { get; set; }

        public bool IsCalibration { get; set; }

        public List<string> Characteristics { get; set; }

        public bool HasFittingMethod { get; set; }

        public bool IsYesGlass { get; set; }

        public virtual ICollection<VehicleGlassAccessoryResponseModel> Accessories { get; set; }

        public virtual ICollection<VehicleInterchangeablePartResponseModel> InterchangeableParts { get; set; }

        public override void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<VehicleGlass, VehicleGlassResponseModel>("VehicleGlassResponseModel")
               .ForMember(x => x.Images, opt => opt.MapFrom(x => x.VehicleGlassImages.Select(i => i.OriginalId).ToList()))
               .ForMember(x => x.InterchangeableParts, opt => opt.MapFrom(x => x.VehicleGlassInterchangeableParts))
               .ForMember(x => x.Accessories, opt => opt.MapFrom(x => x.VehicleGlassAccessories))
               .ForMember(x => x.Characteristics, opt => opt.MapFrom(x => x.VehicleGlassCharacteristics.Select(i => i.Name).ToList()));
        }
    }
}