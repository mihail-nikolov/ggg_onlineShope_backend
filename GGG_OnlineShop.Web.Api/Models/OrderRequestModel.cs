using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GGG_OnlineShop.Common;
using GGG_OnlineShop.Infrastructure;
using GGG_OnlineShop.InternalApiDB.Models;
using GGG_OnlineShop.InternalApiDB.Models.Enums;

namespace GGG_OnlineShop.Web.Api.Models
{
    public class OrderRequestModel : IMapTo<Order>
    {
        [StringLength(GlobalConstants.FullAddressMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string FullAddress { get; set; }

        [Required]
        public DeliveryStatus Status { get; set; }

        [StringLength(GlobalConstants.DeliveryNotesMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string DeliveryNotes { get; set; }

        public bool WithInstallation { get; set; }

        public bool InstallationRuse { get; set; }

        public bool InstallationSofia { get; set; }
        // TODO  field for invoice

        [Required]
        public bool IsInvoiceNeeded { get; set; }

        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double PaidPrice { get; set; }

        [Required]
        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double Price { get; set; }

        public double DiscountPercentage { get; set; }

        [Required]
        [StringLength(GlobalConstants.UserЕmailMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string UserЕmail { get; set; }

        [Required]
        [StringLength(GlobalConstants.UserInfoMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string UserInfo { get; set; }

        public string UserId { get; set; }

        public List<OrderedItemRequestModel> OrderedItems { get; set; }

        public override string ToString()
        {
            string invoice = EnglishBulgarianDictionary.Namings[IsInvoiceNeeded.ToString()];
            string installation = EnglishBulgarianDictionary.Namings[WithInstallation.ToString()];
            string status = EnglishBulgarianDictionary.Namings[Status.ToString()];

            string info = "";
            foreach (var orderedItem in OrderedItems)
            {
                info += $"{orderedItem}\n=======================\n";
            }

            info += $@"
Пълен адрес:         {FullAddress}
Статус:              {status}
Бележки за доставка: {DeliveryNotes}
Необходим монтаж:    {installation}
Необходима фактура:  {invoice}
Цена:                {Price} лв
Цена с отстъпка:     {Price - DiscountPercentage*Price} лв
Платено:             {PaidPrice} лв
";
            return info;
        }
    }
}