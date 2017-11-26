namespace GGG_OnlineShop.Web.Api.Models
{
    using Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class AccountInfoUpdateModel : IMapTo<User>
    {
        [Required]
        [StringLength(GlobalConstants.DeliveryCountryMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.DeliveryCountryMinLength)]
        public string DeliveryCountry { get; set; }

        [Required]
        [StringLength(GlobalConstants.DeliveryTownMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.DeliveryTownMinLength)]
        public string DeliveryTown { get; set; }

        [Required]
        [StringLength(GlobalConstants.DeliveryAddressMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.DeliveryAddressMinLength)]
        public string DeliveryAddress { get; set; }

        [Required]
        [StringLength(GlobalConstants.PhoneNumberMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.PhoneNumberMinLength)]
        public string PhoneNumber { get; set; }
    }
}