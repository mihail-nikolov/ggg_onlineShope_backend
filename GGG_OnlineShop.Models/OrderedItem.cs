namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderedItem: BaseModel<int>, IEntityWithCreator
    {
        [Required]
        [StringLength(GlobalConstants.ManufacturerMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Manufacturer { get; set; }

        [StringLength(GlobalConstants.EurocodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string EuroCode { get; set; }

        [StringLength(GlobalConstants.OtherCodesMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OtherCodes { get; set; }

        [Required]
        [StringLength(GlobalConstants.DescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        [Required]
        [StringLength(GlobalConstants.FullAddressMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string FullAddress { get; set; }

        [Required]
        public DeliveryStatus Status { get; set; }

        [StringLength(GlobalConstants.DeliveryNotesMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string DeliveryNotes { get; set; }

        [Required]
        public bool WithInstallation { get; set; }

        [Required]
        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double PaidPrice { get; set; }

        [Required]
        public bool IsDepositNeeded { get; set; }

        [Required]
        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double Price { get; set; }

        // e.g. Name, Phone, email
        [StringLength(GlobalConstants.AnonymousUserЕmailMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string AnonymousUserЕmail { get; set; }

        [StringLength(GlobalConstants.AnonymousUserInfoMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string AnonymousUserInfo { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
