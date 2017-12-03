namespace GGG_OnlineShop.Web.Api.Areas.Administration.Models.Users
{
    using Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class UserUpdateModel: IMapTo<User>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Range(GlobalConstants.MinPercentageReduction, GlobalConstants.MaxPercentageReduction)]
        public double PercentageReduction { get; set; }

        [Required]
        [StringLength(GlobalConstants.BulstatMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.BulstatMinLength)]
        public string Bulstat { get; set; }

        [Required]
        [StringLength(GlobalConstants.CompanyNameMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.CompanyNameMinLength)]
        public string CompanyName { get; set; }

        public bool IsSaintGobainVisible { get; set; }

        public bool IsPilkingtonVisible { get; set; }

        public bool IsYesglassVisible { get; set; }

        public bool IsNordglassVisible { get; set; }

        public bool IsLamexVisible { get; set; }

        public bool IsAGCVisible { get; set; }

        public bool IsFuyaoVisible { get; set; }

        public bool IsSharedVisible { get; set; }
    }
}