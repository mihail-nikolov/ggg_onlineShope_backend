﻿namespace GGG_OnlineShop.InternalApiDB.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.ComponentModel.DataAnnotations;
    using Base;
    using System;
    using Common;

    public class User : IdentityUser, IAuditInfo, IDeletableEntity
    {
        private ICollection<Order> orders;

        public User()
        {
            this.orders = new HashSet<Order>();
            this.CreatedOn = DateTime.Now;
        }

        [Required]
        [Range(GlobalConstants.MinPercentageReduction, GlobalConstants.MaxPercentageReduction)]
        public double PercentageReduction { get; set; }

        // min length is valid check only when company => bulstat is passed, else the bulstat is not part of the json object
        [StringLength(GlobalConstants.BulstatMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.BulstatMinLength)]
        public string Bulstat { get; set; }

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
        public override string PhoneNumber { get; set; }

        public bool IsCompany { get; set; }

        public bool IsDeferredPaymentAllowed { get; set; }

        public bool OnlyHighCostVisible { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Order> Orders
        {
            get => this.orders;
            set => this.orders = value;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
