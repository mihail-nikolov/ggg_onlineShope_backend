namespace GGG_OnlineShop.InternalApiDB.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.ComponentModel.DataAnnotations;
    using Base;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User : IdentityUser, IAuditInfo, IDeletableEntity
    {
        private ICollection<OrderedItem> orderedItems;

        public User()
        {
            this.orderedItems = new HashSet<OrderedItem>();
            this.CreatedOn = DateTime.Now;
        }

        //[Range(RatingConstants.MinRating, RatingConstants.MaxRating)]
        [Range(0, 100)]
        public int? PercentageReduction { get; set; }

        public virtual ICollection<OrderedItem> OrderedItems
        {
            get
            {
                return this.orderedItems;
            }
            set
            {
                this.orderedItems = value;
            }
        }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 2)]
        public string Bulstat { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 3)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 2)]
        public string DeliveryAddress { get; set; }

        public bool IsAccountActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
