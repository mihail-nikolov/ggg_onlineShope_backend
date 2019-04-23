using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GGG_OnlineShop.Common;
using GGG_OnlineShop.InternalApiDB.Models.Base;
using GGG_OnlineShop.InternalApiDB.Models.Enums;

namespace GGG_OnlineShop.InternalApiDB.Models
{
    public class Order: BaseModel<int>, IEntityWithCreator
    {
        private ICollection<OrderedItem> orderedItems;

        public Order()
        {
            this.orderedItems = new HashSet<OrderedItem>();
        }

        [Required]
        [StringLength(GlobalConstants.FullAddressMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string FullAddress { get; set; }

        [Required]
        public DeliveryStatus Status { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(GlobalConstants.DeliveryNotesMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string DeliveryNotes { get; set; }

        [Required]
        public bool WithInstallation { get; set; }

        [Required]
        public bool IsInvoiceNeeded { get; set; }

        //[Required]
        public int InvoiceNumber { get; set; }

        [Required]
        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double PaidPrice { get; set; }

        [Required]
        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double Price { get; set; }

        [Required]
        [StringLength(GlobalConstants.UserЕmailMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string UserЕmail { get; set; }

        // e.g. Name, Phone, address
        [Required]
        [StringLength(GlobalConstants.UserInfoMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string UserInfo { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public double DiscountPercentage { get; set; }

        public virtual ICollection<OrderedItem> OrderedItems
        {
            get => this.orderedItems;
            set => this.orderedItems = value;
        }
    }
}
