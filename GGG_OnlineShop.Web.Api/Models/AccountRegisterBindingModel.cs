﻿namespace GGG_OnlineShop.Web.Api.Models
{
    using Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class AccountRegisterBindingModel : IMapTo<User>
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public bool IsCompany { get; set; }

        [Required]
        [Range(GlobalConstants.MinPercentageReduction, GlobalConstants.MaxPercentageReduction)]
        public double PercentageReduction { get; set; }
        
        public string Bulstat { get; set; }

        // will be used for company names and first + last names for normal users
        [Required]
        [StringLength(GlobalConstants.NameMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.NameMinLength)]
        public string Name { get; set; }

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

        [Required]
        [StringLength(GlobalConstants.PasswordMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}