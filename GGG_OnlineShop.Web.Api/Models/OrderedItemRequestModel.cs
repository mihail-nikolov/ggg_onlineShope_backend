namespace GGG_OnlineShop.Web.Api.Models
{
    using Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using InternalApiDB.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class OrderedItemRequestModel : IMapTo<OrderedItem>
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
        
        [StringLength(GlobalConstants.FullAddressMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string FullAddress { get; set; }

        [Required]
        public DeliveryStatus Status { get; set; }

        [StringLength(GlobalConstants.DeliveryNotesMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string DeliveryNotes { get; set; }

        [Required]
        public bool WithInstallation { get; set; }

        // TODO  field for invoice

        [Required]
        public bool IsInvoiceNeeded { get; set; }

        //[Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        //public double PaidPrice { get; set; }

        [Required]
        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double Price { get; set; }

        [StringLength(GlobalConstants.AnonymousUserЕmailMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string AnonymousUserЕmail { get; set; }

        [StringLength(GlobalConstants.AnonymousUserInfoMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string AnonymousUserInfo { get; set; }

        public string UserId { get; set; }

        public bool UseAlternativeAddress { get; set; }

        public override string ToString()
        {
            string invoice = EnglishBulgarianDictionary.Namings[IsInvoiceNeeded.ToString()];
            string installation = EnglishBulgarianDictionary.Namings[IsInvoiceNeeded.ToString()];
            string status = EnglishBulgarianDictionary.Namings[Status.ToString()];

            string info = $@"
Производител:        {Manufacturer}
EuroCode:            {EuroCode}
Други кодове:        {OtherCodes}
Описание:            {Description}
Пълен адрес:         {FullAddress}
Статус:              {status}
Бележки за доставка: {DeliveryNotes}
Необходим монтаж:    {installation}
Необходима фактура:  {invoice}
Цена:                {Price} лв
";
            return info;
        }
    }
}