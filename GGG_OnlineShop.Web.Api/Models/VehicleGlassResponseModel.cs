namespace GGG_OnlineShop.Web.Api.Models
{
    using AutoMapper;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class VehicleGlassResponseModel : IMapFrom<VehicleGlass>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string EuroCode { get; set; }

        public string OesCode { get; set; }

        // TODO - probably not needed here
        // ---------------------
        //public string Make { get; set; }

        //public string Model { get; set; }

        //public string BodyType { get; set; }

         //--------------------

        public string ModelDate { get; set; }

        public string PartDate { get; set; }

        public string ProductType { get; set; }

        public string Modification { get; set; }

        public string Tint { get; set; }

        public double? FittingTimeHours { get; set; }

        public string FittingType { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public bool IsAcoustic { get; set; }

        public bool IsCalibration { get; set; }

        public bool IsAccessory { get; set; }

        public string MaterialNumber { get; set; }

        public string LocalCode { get; set; }

        public string IndustryCode { get; set; }

        public List<int> Images { get; set; }

        public List<string> Characteristics { get; set; }

        public bool HasFittingMethod { get; set; }

        public int FeaturedImageId { get; set; }

        public bool IsYesGlass { get; set; }

        public virtual ICollection<VehicleGlassAccessoryResponseModel> Accessories { get; set; }

        public virtual ICollection<VehicleInterchangeablePartResponseModel> InterchangeableParts { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<VehicleGlass, VehicleGlassResponseModel>("VehicleGlassResponseModel")
               .ForMember(x => x.Images, opt => opt.MapFrom(x => x.VehicleGlassImages.Select(i => i.OriginalId).ToList()))
               .ForMember(x => x.InterchangeableParts, opt => opt.MapFrom(x => x.VehicleGlassInterchangeableParts))
               .ForMember(x => x.Accessories, opt => opt.MapFrom(x => x.VehicleGlassAccessories))
               .ForMember(x => x.Characteristics, opt => opt.MapFrom(x => x.VehicleGlassCharacteristics.Select(i => i.Name).ToList()));
        }
    }
}